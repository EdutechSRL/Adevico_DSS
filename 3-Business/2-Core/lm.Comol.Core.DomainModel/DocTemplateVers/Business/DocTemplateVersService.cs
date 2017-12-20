using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceExport = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
using Management = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management;

namespace lm.Comol.Core.DomainModel.DocTemplateVers.Business
{
    /// <summary>
    /// Questa classe servità ad uso e consumo dei soli servizi esterni e gli oggetti restituiti sono "Transient".
    /// </summary>
    public class DocTemplateVersService: lm.Comol.Core.Business.BaseCoreServices
    {
#region Init class
        public DocTemplateVersService():base() { }
        public DocTemplateVersService(iApplicationContext oContext) : base( oContext)
        {
        }
        public DocTemplateVersService(iDataContext oDC)
            : base(oDC)
        {
        }
#endregion

        //  ---==---     RIVEDERE!!!!    ---==---
        //
        //         GET DEI SERVIZI
        //   Va controllato lo stato? (IsDraft, IsSystem, etc... o semplicemente lo prendo "così com'è via ID?)
        //
        //          IsActive
        //   Relativamente a tutti i singoli elementi. Necessario puntualizzarne le logiche di utilizzo 
        //      e quanto/come queste siano complementari con Delete di sistema...
        //

        /// <summary>
        /// Recupera l'ultima versione disponibile per un template
        /// </summary>
        /// <param name="TempalteID">L'Id del template</param>
        /// <returns>L'ultima versione valida di un template.</returns>
        /// <remarks>
        /// Al momento ho scelto di utilizzare SOLO le versioni "disponibili".
        /// Per le versioni in Draft o con Active = false, sarà necessario fornire l'ID Version.
        /// In quel caso verrà utilizzata la versione specificata, indipendentemente dal suo stato.
        /// </remarks>
        public ServiceExport.DTO_Template TemplateGet(Int64 TempalteID, String BasePath)
        {
            return TemplateGet(TempalteID, 0, BasePath); //Last version
        }
        /// <summary>
        /// Recupera una versione specifica
        /// </summary>
        /// <param name="TempalteID">L'ID del Template</param>
        /// <param name="VersionID">L'ID della versione. Se more o uguale a 0 </param>
        /// <returns>La versione specificata di un Template</returns>
        /// /// <remarks>
        /// Al momento ho scelto di utilizzare SOLO le versioni "disponibili".
        /// Per le versioni in Draft o con Active = false, sarà necessario fornire l'ID Version.
        /// In quel caso verrà utilizzata la versione specificata, indipendentemente dal suo stato.
        /// </remarks>
        public ServiceExport.DTO_Template TemplateGet(Int64 TemplateID, Int64 VersionID, String BasePath)
        {
            TemplateVersion srcVersion = null;

            if(VersionID > 0)
            {
                srcVersion = Manager.Get<TemplateVersion>(VersionID);
            } 
            else 
            {
                Template srcTemplate = Manager.Get<Template>(TemplateID);
                if(srcTemplate == null)
                    return null;
         
                srcVersion = (from TemplateVersion tmpVers in srcTemplate.Versions
                              where tmpVers.IsDraft == false && tmpVers.IsActive == true
                                orderby tmpVers.Version descending
                              select tmpVers).FirstOrDefault();

                if (srcVersion == null)
                {
                    srcVersion = (from TemplateVersion tmpVers in srcTemplate.Versions
                                  where tmpVers.IsDraft == false && tmpVers.IsActive == false
                                  orderby tmpVers.Version descending
                                  select tmpVers).FirstOrDefault();
                }

                if (srcVersion == null)
                {
                    //SE non ci sono altre versioni definitive,
                    //prendo quella in bozza (ne è prevista una sola!)
                    srcVersion = (from TemplateVersion tmpVers in srcTemplate.Versions
                                  where tmpVers.IsDraft == true
                                  select tmpVers).FirstOrDefault();
                }
            }

            if(srcVersion == null || srcVersion.Id <= 0)
                return null;

            ServiceExport.DTO_Template expTemplate = new ServiceExport.DTO_Template();

            expTemplate.TemplateId = srcVersion.Template.Id;
            expTemplate.Name = srcVersion.Template.Name;
            expTemplate.VersionId = srcVersion.Id;
            expTemplate.Version = srcVersion.Version;

            expTemplate.UseSkinHeaderFooter = (srcVersion.Template.Type == TemplateType.Skin);

            expTemplate.IsSystem = srcVersion.Template.IsSystem;

            expTemplate.IsEditable = !srcVersion.Template.IsSystem && srcVersion.Template.HasDraft;

            // MODULES
            expTemplate.Modules = new List<ServiceExport.DTO_Modules>();
            if (srcVersion.Template != null)
            {
                var queryService = (from s in Manager.GetIQ<ServiceContent>() where s.Deleted== BaseStatusDeleted.None select s);
                expTemplate.Modules = queryService.Where(s => s.Template.Id == srcVersion.Template.Id).Select(s => new ServiceExport.DTO_Modules() { Id=s.Id, ModuleName=s.ModuleName , IdModule = s.ModuleId, IsActive = s.IsActive, ModuleCode = s.ModuleCode }).ToList();
                ////Template tmpl = (Template)srcVersion.Template;
                //////List<ServiceContent> Services = tmpl.Services.ToList();

                ////if (srcVersion.Template.Services != null && tmpl.Services.Count() > 0)
                ////{
                //foreach(ServiceContent srvc in srcVersion.Template.Services.Where(srv => srv.IsActive))
                //{
                //    ServiceExport.DTO_Modules srv = new ServiceExport.DTO_Modules();
                //    srv.Id = srvc.Id;
                //    srv.IdModule = srv.IdModule;
                //    srv.IsActive = srv.IsActive;
                //    srv.ModuleCode = srv.ModuleCode;
                //    srv.ModuleName = srv.ModuleName;
                //    expTemplate.Modules.Add(srv);
                //}
                ////}
            }

            PageElement BodyEl = (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.Body && el.IsActive)
                                  orderby PgEl.SubVersion descending
                                  select PgEl).FirstOrDefault();

            ServiceExport.DTO_ElementText Body = new ServiceExport.DTO_ElementText();

            if (BodyEl != null && BodyEl.GetType() == typeof(ElementText))
            {
                ElementText src = (ElementText)BodyEl;
                if (src != null)
                {
                    Body.Alignment = src.Alignment;
                    Body.Id = src.Id;
                    Body.IsHTML = src.IsHTML;
                    Body.Text = src.Text;
                }

            }
            else
            {
                Body = new ServiceExport.DTO_ElementText();
                Body.Id = -1;
                Body.IsHTML = true;
                Body.Text = "<p></p>";
            }
            expTemplate.Body = Body;

            // HEADER
            ImageDataDTO Data = new ImageDataDTO { BaseUrl = BasePath, TemplateId = expTemplate.TemplateId, VersionId = expTemplate.VersionId };

            expTemplate.Header = new ServiceExport.DTO_HeaderFooter();
            expTemplate.Header.Left = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.HeaderLeft && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                                  orderby PgEl.SubVersion descending
                                  select PgEl).FirstOrDefault()
                , Data);
            expTemplate.Header.Center = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.HeaderCenter && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                 orderby PgEl.SubVersion descending
                 select PgEl).FirstOrDefault()
                , Data);
            expTemplate.Header.Right = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.HeaderRight && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                 orderby PgEl.SubVersion descending
                 select PgEl).FirstOrDefault()
                , Data);

            // FOOTER
            expTemplate.Footer = new ServiceExport.DTO_HeaderFooter();
            expTemplate.Footer.Left = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.FooterLeft && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                 orderby PgEl.SubVersion descending
                 select PgEl).FirstOrDefault()
                , Data);
            expTemplate.Footer.Center = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.FooterCenter && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                 orderby PgEl.SubVersion descending
                 select PgEl).FirstOrDefault()
                , Data);
            expTemplate.Footer.Right = ElementConvert(
                (from PageElement PgEl in srcVersion.Elements.Where(el => el.Position == ElementPosition.FooterRight && el.IsActive && el.Deleted == BaseStatusDeleted.None)
                 orderby PgEl.SubVersion descending
                 select PgEl).FirstOrDefault()
                , Data);

            // SETTING
            expTemplate.Settings = SettingsConvert(
                    (from DocTemplateVers.Settings Sett
                         in srcVersion.Settings.Where(el => el.IsActive && el.Deleted == BaseStatusDeleted.None)
                     orderby Sett.SubVersion descending
                     select Sett
                     ).FirstOrDefault()
                     );

            if (expTemplate.Settings != null &&
                !string.IsNullOrEmpty(expTemplate.Settings.BackgroundImagePath) &&
                !expTemplate.Settings.BackgroundImagePath.StartsWith("http"))
            {
                expTemplate.Settings.BackgroundImagePath = lm.Comol.Core.DomainModel.DocTemplateVers.Business.ImageHelper.GetImageUrl(expTemplate.Settings.BackgroundImagePath, BasePath, expTemplate.TemplateId, expTemplate.VersionId);
            }

            // SIGNATURES
            //IList<Signature> SignTEST = srcVersion.Signatures.ToList();


            expTemplate.Signatures = (
                from Signature srcSigns 
                    in srcVersion.Signatures.Where(srv => srv.IsActive)
                select SignatureConver(srcSigns, Data)).ToList();

            return expTemplate;
        }
        
        #region Element convertion - private

        private ServiceExport.DTO_Element ElementConvert(PageElement srcElement, ImageDataDTO Data)
        {
            ServiceExport.DTO_Element ElOut = null;

            if (srcElement != null)
            {

                if(srcElement.GetType() == typeof(ElementText))
                {
                    ServiceExport.DTO_ElementText OutText = new ServiceExport.DTO_ElementText();

                    ElementText src = (ElementText)srcElement;
                    if (src != null)
                    {
                        OutText.IsHTML = src.IsHTML;
                        OutText.Text = src.Text;
                    }
                    ElOut = OutText;
                }

                if (srcElement.GetType() == typeof(ElementImage))
                {
                    ServiceExport.DTO_ElementImage OutImg = new ServiceExport.DTO_ElementImage();
                    ElementImage src = (ElementImage)srcElement;
                    if (src != null)
                    {
                        OutImg.Height = src.Height;
                        OutImg.Path = ImageHelper.GetImageUrl(Data, src.Path);
                        OutImg.Width = src.Width;
                    }
                    ElOut = OutImg;
                }

                if (srcElement.GetType() == typeof(ElementVoid))
                {
                    ElOut = null;
                }

                if (ElOut != null)
                {
                    ElOut.Id = srcElement.Id;
                    ElOut.Alignment = srcElement.Alignment;
                }
            }
            return ElOut;
        }
        private ServiceExport.DTO_Modules ModulesConvert(ServiceContent srcModules)
        {
            ServiceExport.DTO_Modules Module = new ServiceExport.DTO_Modules();

            if (srcModules != null)
            {
                Module.Id = srcModules.Id;
                Module.IdModule = srcModules.ModuleId;
                Module.IsActive = srcModules.IsActive;
                Module.ModuleCode = srcModules.ModuleCode;
                Module.ModuleName = srcModules.ModuleName;

            }
            return Module;
        }
        private ServiceExport.DTO_Signature SignatureConver(Signature srcSignature, ImageDataDTO Data)
        {
            ServiceExport.DTO_Signature Signature = new ServiceExport.DTO_Signature();

            if (srcSignature != null)
            {
                Signature.HasImage = srcSignature.HasImage;
                Signature.HasPDFPositioning = srcSignature.HasPDFPositioning;
                Signature.Height = srcSignature.Height;
                Signature.Id = srcSignature.Id;
                Signature.IsHTML = srcSignature.IsHTML;
                Signature.Order = srcSignature.Placing;
                Signature.Path = ImageHelper.GetImageUrl(Data, srcSignature.Path);
                Signature.PosBottom = srcSignature.PosBottom;
                Signature.Position = srcSignature.Position;
                Signature.PosLeft = srcSignature.PosLeft;
                Signature.Text = srcSignature.Text;
                Signature.Width = srcSignature.Width;
                Signature.PagePlacingMask = srcSignature.PagePlacingMask;
                Signature.PagePlacingRange = srcSignature.PagePlacingRange;
            }

            return Signature;
        }
        private ServiceExport.DTO_Settings SettingsConvert(DocTemplateVers.Settings srcSettings)
        {
            ServiceExport.DTO_Settings Settings = new ServiceExport.DTO_Settings();

            if (srcSettings != null)
            {
                Settings.Author = srcSettings.Author;
                Settings.BackgroundAlpha = srcSettings.BackgroundAlpha;
                Settings.BackgroundBlue = srcSettings.BackgroundBlue;
                Settings.BackgroundGreen = srcSettings.BackgroundGreen;
                Settings.BackGroundImageFormat = srcSettings.BackGroundImageFormat;
                Settings.BackgroundImagePath = srcSettings.BackgroundImagePath;

                

                Settings.BackgroundRed = srcSettings.BackgroundRed;
                Settings.Creator = srcSettings.Creator;
                //Settings.HasHeaderOnFirstPage = srcSettings.HasHeaderOnFirstPage;
                Settings.Height = srcSettings.Height;
                Settings.Id = srcSettings.Id;
                Settings.IsActive = srcSettings.IsActive;
                Settings.Keywords = srcSettings.Keywords;
                Settings.MarginBottom = srcSettings.MarginBottom;
                Settings.MarginLeft = srcSettings.MarginLeft;
                Settings.MarginRight = srcSettings.MarginRight;
                Settings.MarginTop = srcSettings.MarginTop;
                //Settings.PageNumberAlignment = srcSettings.PageNumberAlignment;
                Settings.Producer = srcSettings.Producer;
                //Settings.ShowPageNumber = srcSettings.ShowPageNumber;
                Settings.Size = srcSettings.Size;
                Settings.Subject = srcSettings.Subject;
                Settings.Title = srcSettings.Title;
                Settings.Width = srcSettings.Width;

                Settings.PagePlacingMask = srcSettings.PagePlacingMask;
                Settings.PagePlacingRange = srcSettings.PagePlacingRange;

                if (Settings.Size != DocTemplateVers.PageSize.custom)
                {
                    lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.PageSizeValue PgSzV =
                        lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.GetSize(Settings.Size, "px");
                    Settings.Width = PgSzV.Width;
                    Settings.Height = PgSzV.Height;
                }

                if (Settings.Width < lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5))
                {
                    Settings.Width = lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5);
                }
                if (Settings.Height < lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5))
                {
                    Settings.Height = lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5);
                }
            }

            return Settings;
        }
        
        #endregion

        /// <summary>
        /// Recupera la lista di tutti i template e relative versioni disponibili
        /// </summary>
        /// <returns>I template che hanno ALMENO una versione valida e tutte le relative varisoni valide: IsActive == TRUE && IsDRAFT == FALSE</returns>
        //public IList<ServiceExport.DTO_Template> TemplateGetAllAvailable()
        //{
        //    //Recupero da DTO_List
        //    IList<ServiceExport.DTO_Template> avTemplates = new List<ServiceExport.DTO_Template>();

        //    IList<DocTemplateVers.Domain.DTO.Management.DTO_ListTemplate> curTemplates = Manager.GetAll<Domain.DTO.Management.DTO_ListTemplate>().Where(tmp => tmp.IsActive == true && tmp.HasDefinitive == true).ToList();

        //    if (curTemplates != null && curTemplates.Count() > 0)
        //    {
        //        foreach (Domain.DTO.Management.DTO_ListTemplate template in curTemplates)
        //        {
        //            if (template.TemplateVersions != null && template.TemplateVersions.Count() > 0)
        //            {
        //                foreach (Domain.DTO.Management.DTO_ListTemplateVersion Version in template.TemplateVersions)
        //                {
        //                    ServiceExport.DTO_Template avTempl = new ServiceExport.DTO_Template();
        //                    avTempl.IsSystem = template.IsSystem;
        //                    avTempl.Name = template.Name;
        //                    avTempl.TemplateId = template.Id;
        //                    avTempl.UseSkinHeaderFooter = (template.Type == TemplateType.Skin);
        //                    avTempl.Version = Version.Version;
        //                    avTempl.VersionId = Version.Id;
        //                    avTemplates.Add(avTempl);
        //                }
        //            }
        //        }
        //    }
            
        //    // ORDINAMENTI vari...

        //    return avTemplates;
        //}
        /// <summary>
        /// Recupera la lista dei template e relative versioni disponibili associate ad un determinato servizio
        /// </summary>
        /// <param name="ServiceCode">Codice del servizio di riferimento</param>
        /// <returns>I template che hanno ALMENO una versione valida e tutte le relative varisoni valide: IsActive == TRUE && IsDRAFT == FALSE</returns>
        //public IList<ServiceExport.DTO_Template> TemplateGetAllAvailable(String ServiceCode)
        //{
        //    return null;
        //}
        
        /// <summary>
        /// Recupera una lista di DTO adatti all'inserimento in una DropDownList per la selezione del Template/Version
        /// </summary>
        /// <param name="filters">Filtri vari: DA DEFINIRE!!!</param>
        /// <returns>La lista di DTO</returns>
        /// <remarks>Controllare e verificare i vari parametri da passare per il recupero corretto delle varie liste</remarks>
        /// 
        public List<ServiceExport.DTO_sTemplate> GetAvailableTemplates(long idTemplate, long idVersion, long idModule)
        {
            List<ServiceExport.DTO_sTemplate> items = new List<ServiceExport.DTO_sTemplate>();


            // Get all templates

            var queryService = (from s in Manager.GetIQ<ServiceContent>() where s.Deleted== BaseStatusDeleted.None select s);
            List<long> idItems = (from t in Manager.GetIQ<Template>()
                                  where t.Deleted == BaseStatusDeleted.None && t.IsActive && t.HasDefinitive
                                  select t.Id).ToList();
            //Modificato HasActive in IsActive (HasDefinitive = true "include" HasActive = true) - M.B.
            idItems = idItems.Where( id=>(queryService.Where(s => s.ModuleId == idModule && s.Template.Id==id).Any()
                     || !queryService.Where(s => s.Template.Id==id).Any())).ToList();

            items = (from t in Manager.GetIQ<Template>()
                     where idItems.Contains(t.Id) 
                     select new ServiceExport.DTO_sTemplate()
                     {
                         HasActive= t.HasActive,
                         HasDefinitive= t.HasDefinitive,
                         HasDraft= t.HasDraft,
                         Id= t.Id,
                         IsActive = t.IsActive,
                         IsSystem= t.IsSystem,
                         Name= t.Name
                     }).ToList();

            items.ForEach(t => t.Versions = (from v in Manager.GetIQ<TemplateVersion>()
                                             where ( v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.IsActive && !v.IsDraft)) && v.Template.Id==t.Id 
                                             select new ServiceExport.DTO_sTemplateVersion() {
                                                  Id=v.Id,
                                                  IsActive=v.IsActive,
                                                  IsDraft= v.IsDraft,
                                                  Version= v.Version,
                                                  IdTemplate = v.Template.Id,
                                                  Lastmodify= (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn,
                                                  IsSelected = (v.Id== idVersion && t.Id== idTemplate)
                                             }
                                           ).ToList().OrderByDescending(v => v.Version).ToList());

            ServiceExport.DTO_sTemplate template = items.Where(t => t.Versions.Count == 0 && t.Id == idTemplate).FirstOrDefault();
            if (template != null) {

                //ServiceExport.DTO_sTemplateVersion ver = (from v in Manager.GetIQ<TemplateVersion>()
                //                                          where (v.Id == idVersion || (v.Deleted == BaseStatusDeleted.None && v.IsActive && !v.IsDraft)) && v.Template.Id == template.Id
                //                             select new ServiceExport.DTO_sTemplateVersion() {
                //                                  Id = v.Id,
                //                                  IsActive = v.IsActive,
                //                                  IsDraft = v.IsDraft,
                //                                  Version = v.Version,
                //                                  IdTemplate = template.Id,
                //                                  Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn,
                //                                  IsSelected = (v.Id == idVersion && template.Id == idTemplate) 
                //                             }
                //                           ).OrderByDescending(v => v.Version).Skip(0).Take(1).ToList().FirstOrDefault();

                TemplateVersion tVer = Manager.Get<TemplateVersion>(idVersion);

                if(tVer != null && tVer.Deleted == BaseStatusDeleted.None && tVer.IsActive && tVer.IsDraft)
                {
                    ServiceExport.DTO_sTemplateVersion ver = new ServiceExport.DTO_sTemplateVersion()
                    {
                        Id = tVer.Id,
                        IsActive = tVer.IsActive,
                        IsDraft = tVer.IsDraft,
                        Version = tVer.Version,
                        IdTemplate = template.Id,
                        Lastmodify = (tVer.ModifiedOn == null) ? tVer.CreatedOn : tVer.ModifiedOn,
                        IsSelected = (tVer.Id == idVersion && template.Id == idTemplate)
                    };

                    template.Versions.Add(ver);
                }
            }

            DateTime lastVersion = DateTime.Now;
            items.Where(t => t.Versions.Count > 0).ToList().ForEach(t=> t.Versions.Insert(0, 
                new ServiceExport.DTO_sTemplateVersion(){
                 Id=0,
                 IsActive=true,
                 IsDraft= false,
                 Lastmodify= lastVersion,
                 Version=0,
                 IdTemplate = t.Id,
                 IsSelected = (idVersion ==0 && t.Id == idTemplate)
                }));


            //Add Selected: Solo se il TEMPLATE indicato NON è stato trovato!
            if (!items.Where(t=> t.Versions.Where(v=> v.IsSelected==true).Any()).Any() && (idTemplate>0 || idVersion>0)){
                ServiceExport.DTO_sTemplate dtoTemplate = null;
                ServiceExport.DTO_sTemplateVersion dtoVersion = null;

                if (idVersion <= 0) //LAST VERSION! Se non ci sono versioni "Definitive", recupero l'ultima "Deprecata"...
                {
                    dtoTemplate = (from t in Manager.GetIQ<Template>()
                             where t.Id== idTemplate 
                             select new ServiceExport.DTO_sTemplate()
                             {
                                 HasActive= t.HasActive,
                                 HasDefinitive= t.HasDefinitive,
                                 HasDraft= t.HasDraft,
                                 Id= t.Id,
                                 IsActive = t.IsActive,
                                 IsSystem= t.IsSystem,
                                 Name= t.Name
                         }).Skip(0).Take(1).ToList().FirstOrDefault();

                    if (dtoTemplate != null)
                    {

                        TemplateVersion tVersion = (from v in Manager.GetIQ<TemplateVersion>()
                                                  where (dtoTemplate.HasDefinitive && !v.IsDraft)
                                                  select v)
                                                  .OrderByDescending(v => v.IsActive)
                                                  .ThenBy(v => v.Version)
                                                  .Skip(0).Take(1).ToList().FirstOrDefault();

                        if(tVersion != null)
                        {
                            dtoVersion = new ServiceExport.DTO_sTemplateVersion()
                            {
                                Id = tVersion.Id,
                                IsActive = tVersion.IsActive,
                                IsDraft = tVersion.IsDraft,
                                Version = tVersion.Version,
                                IdTemplate = tVersion.Template.Id,
                                Lastmodify = (tVersion.ModifiedOn == null) ? tVersion.CreatedOn : tVersion.ModifiedOn
                            };
                        }
                        
                        //dtoVersion = (from v in Manager.GetIQ<TemplateVersion>()
                        //                     where (dtoTemplate.HasDefinitive && v.IsDraft==false && v.IsActive ) || (!dtoTemplate.HasDefinitive && v.IsActive)
                        //                     select new ServiceExport.DTO_sTemplateVersion() {
                        //                          Id = v.Id,
                        //                          IsActive = v.IsActive,
                        //                          IsDraft = v.IsDraft,
                        //                          Version = v.Version,
                        //                           IdTemplate = v.Template.Id,
                        //                          Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn
                        //                     }
                        //                   ).OrderByDescending(v => v.Version).Skip(0).Take(1).ToList().FirstOrDefault();
                    }
                }
                else
                {
                    dtoVersion = (from v in Manager.GetIQ<TemplateVersion>()
                                            where v.Id== idVersion 
                                            select new ServiceExport.DTO_sTemplateVersion() {
                                                Id = v.Id,
                                                IsActive = v.IsActive,
                                                IsDraft = v.IsDraft,
                                                Version = v.Version,
                                                IdTemplate = v.Template.Id,
                                                Lastmodify = (v.ModifiedOn==null) ? v.CreatedOn: v.ModifiedOn
                                            }
                                        ).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (dtoVersion != null) {
                        dtoTemplate = (from t in Manager.GetIQ<Template>()
                             where t.Id== idTemplate 
                             select new ServiceExport.DTO_sTemplate()
                             {
                                 HasActive= t.HasActive,
                                 HasDefinitive= t.HasDefinitive,
                                 HasDraft= t.HasDraft,
                                 Id= t.Id,
                                 IsActive = t.IsActive,
                                 IsSystem= t.IsSystem,
                                 Name= t.Name
                         }).Skip(0).Take(1).ToList().FirstOrDefault();
                        //Controllo la versione recuperata da dB:
                        // Se NON soddisfa i requisiti, viene impostata a null
                        if (!(dtoTemplate != null && dtoTemplate.Id == dtoVersion.IdTemplate))
                            dtoVersion = null;
                    }
                }

                if (dtoTemplate != null && dtoVersion != null)
                {

                    if (dtoTemplate.Versions == null)
                        dtoTemplate.Versions = new List<ServiceExport.DTO_sTemplateVersion>();


                    dtoTemplate.Versions.Add(dtoVersion);


                    dtoTemplate.Versions.Insert(0,
                        new ServiceExport.DTO_sTemplateVersion()
                        {
                            Id = 0,
                            IsActive = true,
                            IsDraft = false,
                            Lastmodify = lastVersion,
                            Version = 0,
                            IdTemplate = dtoTemplate.Id,
                            IsSelected = (idVersion == 0 && dtoTemplate.Id == idTemplate)
                        });
                    
                    items.RemoveAll(t => t.Id == dtoTemplate.Id);

                    items.Add(dtoTemplate);
                }
            }



            items.ForEach(t => t.Services = queryService.Where(s => s.Template.Id == t.Id).Select(s => new ServiceExport.DTO_sServiceContent() { IdModule = s.ModuleId, IsActive = s.IsActive, ModuleCode = s.ModuleCode }).ToList());
            return items;
        }
        public Boolean isValidTemplate(long idTemplate, long idVersion)
        {
            Boolean result = false;
            if (idTemplate>0 || idVersion >0){
                try{
                    Manager.BeginTransaction();
                    ServiceExport.DTO_sTemplate dTemplate = null;
                    ServiceExport.DTO_sTemplateVersion version = null;

                    if (idVersion>0)
                        version = (from v in Manager.GetIQ<TemplateVersion>()
                                  where v.Id == idVersion
                                  select new ServiceExport.DTO_sTemplateVersion() { Id=v.Id, Deleted= v.Deleted, IsActive= v.IsActive , IsSelected= v.IsDraft, IdTemplate=v.Template.Id} ).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (version!=null && version.IdTemplate != idTemplate)
                        idTemplate =version.IdTemplate;

                    if (idTemplate>0)
                        dTemplate = (from t in Manager.GetIQ<Template>()
                                  where t.Id == idTemplate
                                  select new ServiceExport.DTO_sTemplate() { Id=t.Id, HasDefinitive= t.HasActive , HasActive= t.HasActive, Deleted= t.Deleted} ).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (dTemplate!=null)
                        result = (dTemplate.Deleted == BaseStatusDeleted.None && dTemplate.HasActive && dTemplate.HasDefinitive) && (version == null || (version!= null && version.IsActive && version.IsDraft == false && version.Deleted == BaseStatusDeleted.None));

                    Manager.Commit();
                }
                catch(Exception ex){
                    Manager.RollBack();
                }
            }
            return result;
        }

        public Boolean HasAvailableTemplates(long idModule)
        {
            Boolean result = false;

            var queryService = (from s in Manager.GetIQ<ServiceContent>() where s.Deleted == BaseStatusDeleted.None select s);

            List<long> idItems = (from t in Manager.GetIQ<Template>()
                      where t.Deleted == BaseStatusDeleted.None && t.HasActive && t.HasDefinitive 
                      select t.Id).ToList();
            //var qd = queryService.ToList();
            //var p= (queryService.Where(s => s.ModuleId == idModule && idItems.Contains(s.Template.Id))).ToList();
            //var q = (queryService.Where(s => !idItems.Contains(s.Template.Id))).ToList();
            result = idItems.Any() && (queryService.Where(s => s.ModuleId == idModule && idItems.Contains(s.Template.Id)).Any()
                      || !queryService.Where(s => !idItems.Contains(s.Template.Id)).Any());

            return result;
        }

        //public IList<ServiceExport.DTO_sTemplate> TemplateGetForDDL(Int64 TemplateId, Int64 VersionId, Int64 ServiceId)
        //{
        //    IList<ServiceExport.DTO_sTemplate> Templates = new List<ServiceExport.DTO_sTemplate>();

        //    //Recupero dei Template
        //    //Aggiungere QUI eventuali FILTRI di varia natura...

        //    //Parto dal presupposto che anche se lo stesso servizio è associato più volte ad un template, per me va uguale...
        //    List<ServiceContent> SCs = Manager.GetAll<ServiceContent>(sc => (sc.ModuleId == ServiceId)).ToList();

        //    ServiceContent SC = (
        //        from ServiceContent sc in Manager.GetAll<ServiceContent>(sc => (sc.ModuleId == ServiceId)) // && (sc.Template.Id == TemplateId))
        //        where (sc.Template != null && sc.Template.Id == TemplateId)
        //        select sc
        //        )
        //        .FirstOrDefault();

        //    IList<Template> curTemplates;


        //    //Si considerano i TEMPLATE CHE:
        //    // - Attivi
        //    // - Con Definitivi


        //    if (SC != null)
        //    {
        //        curTemplates = Manager.GetAll<Template>(
        //        tmp => tmp.HasDefinitive == true && tmp.IsActive == true && (tmp.Services == null || tmp.Services.Count() == 0 || tmp.Services.Contains(SC)));
        //    }
        //    else
        //    {
        //        curTemplates = Manager.GetAll<Template>(
        //            //tmp => (tmp.HasDefinitive == true) && (tmp.IsActive == true));         
        //        tmp => tmp.HasDefinitive == true && tmp.IsActive == true && (tmp.Services == null || tmp.Services.Count() == 0));
        //    }

        //    //Manager.GetAll<Template>(tmp => (tmp.IsActive == true && tmp.HasDefinitive == true) ||
        //    //(tmp.Services.Count() == 0) || (tmp.Services.Where(srv => srv.Id == ServiceId).Count() > 0));


        //    Boolean SelectedFound = false;

        //    if (curTemplates != null && curTemplates.Count() > 0)
        //    {
        //        foreach (Template tmpl in curTemplates)
        //        {
        //            if (tmpl.Versions != null && tmpl.Versions.Count() > 0)
        //            {

        //                ServiceExport.DTO_TemplateDDL tmplDDL = new ServiceExport.DTO_TemplateDDL();
        //                tmplDDL.Id = tmpl.Id;
        //                tmplDDL.HasActive = tmpl.HasActive;
        //                tmplDDL.HasDefinitive = tmpl.HasDefinitive;
        //                tmplDDL.HasDraft = tmpl.HasDraft;
        //                tmplDDL.IsActive = tmpl.IsActive;
        //                tmplDDL.IsSystem = tmpl.IsSystem;
        //                tmplDDL.Name = tmpl.Name;
        //                tmplDDL.Versions = new List<ServiceExport.DTO_VersionDLL>();

        //                // Primo GIRO: INSERIMENTO VERSION!

        //                foreach (TemplateVersion ver in tmpl.Versions.OrderByDescending(v => v.Version))
        //                {
        //                    if ((ver.Id == VersionId)
        //                        || (ver.IsActive == true && ver.IsDraft == false)
        //                        )
        //                    {
        //                        ServiceExport.DTO_VersionDLL verDDL = new ServiceExport.DTO_VersionDLL();
        //                        verDDL.Id = ver.Id;
        //                        verDDL.IsActive = ver.IsActive;
        //                        verDDL.IsDraft = ver.IsDraft;
        //                        if (ver.ModifiedOn == null)
        //                        {
        //                            verDDL.Lastmodify = ver.CreatedOn;
        //                        }
        //                        else
        //                        {
        //                            verDDL.Lastmodify = ver.ModifiedOn;
        //                        }

        //                        verDDL.Version = ver.Version;

        //                        if ((SelectedFound == false) && tmplDDL.Id == TemplateId && VersionId == ver.Id)
        //                        {
        //                            SelectedFound = true;
        //                            verDDL.IsSelected = true;
        //                        }
        //                        else
        //                        {
        //                            verDDL.IsSelected = false;
        //                        }

        //                        tmplDDL.Versions.Add(verDDL);
        //                    }
        //                }

        //                if (tmplDDL.Versions.Count < 0 && tmplDDL.Id == TemplateId)
        //                {
        //                    TemplateVersion ver = tmpl.Versions.Where(v => v.IsDraft == false).OrderByDescending(v => v.Version).FirstOrDefault();
        //                    if (ver != null)
        //                    {
        //                        ServiceExport.DTO_VersionDLL verDDL = new ServiceExport.DTO_VersionDLL();
        //                        verDDL.Id = ver.Id;
        //                        verDDL.IsActive = ver.IsActive;
        //                        verDDL.IsDraft = ver.IsDraft;
        //                        if (ver.ModifiedOn == null)
        //                        {
        //                            verDDL.Lastmodify = ver.CreatedOn;
        //                        }
        //                        else
        //                        {
        //                            verDDL.Lastmodify = ver.ModifiedOn;
        //                        }

        //                        verDDL.Version = ver.Version;

        //                        if ((SelectedFound == false) && tmplDDL.Id == TemplateId && VersionId == ver.Id)
        //                        {
        //                            SelectedFound = true;
        //                            verDDL.IsSelected = true;
        //                        }
        //                        else
        //                        {
        //                            verDDL.IsSelected = false;
        //                        }

        //                        tmplDDL.Versions.Add(verDDL);
        //                    }

        //                }


        //                if (tmplDDL.Versions.Count() > 0)
        //                {
        //                    //AGGIUNGO "LAST"
        //                    ServiceExport.DTO_VersionDLL lastDDL = new ServiceExport.DTO_VersionDLL();
        //                    lastDDL.Id = 0;
        //                    lastDDL.IsActive = true;
        //                    lastDDL.IsDraft = false;
        //                    lastDDL.Lastmodify = DateTime.Now;
        //                    lastDDL.Version = 0;

        //                    if (SelectedFound == false && TemplateId == tmplDDL.Id && VersionId <= 0)
        //                    {
        //                        SelectedFound = true;
        //                        lastDDL.IsSelected = true;
        //                    }
        //                    else
        //                    {
        //                        lastDDL.IsSelected = false;
        //                    }

        //                    tmplDDL.Versions.Insert(0, lastDDL);

        //                    tmplDDL.Services = new List<ServiceExport.DTO_ServiceDLL>();
        //                    foreach (ServiceContent srvc in tmpl.Services)
        //                    {
        //                        ServiceExport.DTO_ServiceDLL srvdll = new ServiceExport.DTO_ServiceDLL();
        //                        srvdll.IsActive = srvc.IsActive;
        //                        srvdll.ModuleCode = srvc.ModuleCode;
        //                        srvdll.ModuleId = srvc.ModuleId;
        //                        srvdll.ModuleName = srvc.ModuleName;
        //                        srvdll.Version = srvc.Version;

        //                        tmplDDL.Services.Add(srvdll);
        //                    }
        //                    Templates.Add(tmplDDL);
        //                }
        //            }

        //            if (VersionId <= 0 && TemplateId == tmpl.Id)
        //            {
        //                SelectedFound = true;
        //            }
        //        }
        //    }

        //    //Add Selected: Solo se il TEMPLATE indicato NON è stato trovato!
        //    if (SelectedFound == false)
        //    {
        //        Template selTemplate;
        //        TemplateVersion selVersion = null;

        //        if (VersionId <= 0) //LAST VERSION! Se non ci sono versioni "Definitive", recupero l'ultima "Deprecata"...
        //        {
        //            selTemplate = Manager.Get<Template>(TemplateId);
        //            if (selTemplate.Versions != null && selTemplate.Versions.Count() > 0)
        //            {
        //                if (selTemplate.HasDefinitive)
        //                {
        //                    selVersion = (
        //                        from TemplateVersion vrs
        //                        in selTemplate.Versions
        //                        where vrs.IsDraft == false && vrs.IsActive == true
        //                        orderby vrs.Version descending
        //                        select vrs
        //                    ).FirstOrDefault();
        //                }
        //                else
        //                {
        //                    selVersion = (
        //                        from TemplateVersion vrs
        //                        in selTemplate.Versions
        //                        where vrs.IsActive == true
        //                        orderby vrs.Version descending
        //                        select vrs
        //                        ).FirstOrDefault();
        //                }
        //            }

        //        }
        //        else
        //        {
        //            selVersion = Manager.Get<TemplateVersion>(VersionId);
        //            selTemplate = selVersion.Template;
        //            //Controllo la versione recuperata da dB:
        //            // Se NON soddisfa i requisiti, viene impostata a null
        //            if (!(selVersion != null && selVersion.Template != null && selVersion.Template.Id == TemplateId))
        //                selVersion = null;
        //        }

        //        if (selTemplate != null && selVersion != null)
        //        {

        //            ServiceExport.DTO_TemplateDDL tmplDDL = new ServiceExport.DTO_TemplateDDL();
        //            tmplDDL.Id = selTemplate.Id;
        //            tmplDDL.HasActive = selTemplate.HasActive;
        //            tmplDDL.HasDefinitive = selTemplate.HasDefinitive;
        //            tmplDDL.HasDraft = selTemplate.HasDraft;
        //            tmplDDL.IsActive = selTemplate.IsActive;
        //            tmplDDL.IsSystem = selTemplate.IsSystem;
        //            tmplDDL.Name = selTemplate.Name;
        //            tmplDDL.Versions = new List<ServiceExport.DTO_VersionDLL>();

        //            //Boolean Selected = false;

        //            ServiceExport.DTO_VersionDLL verDDL = new ServiceExport.DTO_VersionDLL();
        //            verDDL.Id = selVersion.Id;
        //            verDDL.IsActive = selVersion.IsActive;
        //            verDDL.IsDraft = selVersion.IsDraft;
        //            if (selVersion.ModifiedOn == null)
        //            {
        //                verDDL.Lastmodify = selVersion.CreatedOn;
        //            }
        //            else
        //            {
        //                verDDL.Lastmodify = selVersion.ModifiedOn;
        //            }
        //            verDDL.Version = selVersion.Version;
        //            if (VersionId <= 0 && selVersion.Id == VersionId)
        //            {
        //                verDDL.IsSelected = true;
        //                SelectedFound = true;
        //            }

        //            tmplDDL.Versions.Add(verDDL);

        //            //AGGIUNGO "LAST"
        //            ServiceExport.DTO_VersionDLL lastDDL = new ServiceExport.DTO_VersionDLL();
        //            lastDDL.Id = 0;
        //            lastDDL.IsActive = false;
        //            lastDDL.IsDraft = false;
        //            lastDDL.Lastmodify = DateTime.Now;
        //            lastDDL.Version = 0;

        //            if (SelectedFound == false && TemplateId == tmplDDL.Id && VersionId <= 0)
        //            {
        //                SelectedFound = true;
        //                lastDDL.IsSelected = true;
        //            }
        //            else
        //            {
        //                lastDDL.IsSelected = false;
        //            }

        //            tmplDDL.Versions.Insert(0, lastDDL);

        //            tmplDDL.Services = new List<ServiceExport.DTO_ServiceDLL>();
        //            foreach (ServiceContent srvc in selTemplate.Services)
        //            {
        //                ServiceExport.DTO_ServiceDLL srvdll = new ServiceExport.DTO_ServiceDLL();
        //                srvdll.IsActive = srvc.IsActive;
        //                srvdll.ModuleCode = srvc.ModuleCode;
        //                srvdll.ModuleId = srvc.ModuleId;
        //                srvdll.ModuleName = srvc.ModuleName;
        //                srvdll.Version = srvc.Version;

        //                tmplDDL.Services.Add(srvdll);
        //            }


        //            Templates.Add(tmplDDL);

        //        }

        //        //aggiungo la versione
        //    }

        //    return Templates;
        //}



    }
}