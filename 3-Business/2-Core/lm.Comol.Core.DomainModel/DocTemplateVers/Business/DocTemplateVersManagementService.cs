using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Management = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management;


namespace lm.Comol.Core.DomainModel.DocTemplateVers.Business
{
    /// <summary>
    /// Service per la GESTIONE (CRUD) degli elementi di Template.
    /// </summary>
    public class DocTemplateVersManagementService: lm.Comol.Core.Business.BaseCoreServices
    {

#region Init class

        public DocTemplateVersManagementService():base() {
        
        }
        public DocTemplateVersManagementService(iApplicationContext oContext) : base( oContext)
        {

        }
        public DocTemplateVersManagementService(iDataContext oDC)
            : base(oDC)
        {

        }

#endregion


    #region Add
        
        /// <summary>
        /// Crea una copia di un template
        /// </summary>
        /// <param name="TempalteId">L'ID del template sorgete</param>
        /// <returns>ID del nuovo Template (o NUOVO TEMPLATE?)</returns>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Int64 TemplateCopyCreateNew(Int64 TemplateId, String NewName, TemplateType NewType, IList<Int64> NewServicesIds, ref Boolean IsSystem)
        {
            if (string.IsNullOrEmpty(NewName))
            {
                throw new ArgumentNullException("NewName", "Cannot be null.");
            }

            Int64 NewId = 0;
            Int64 NewVersId = 0;
            if (Permission.AddTemplate) // && TemplateId > 0)
            {
                Domain.Core.dtoFileCopyBlock dtoFiles = new Domain.Core.dtoFileCopyBlock();

                Template NewTempl = new Template();
                Template SourceTempl = null;

                if (TemplateId > 0)
                {
                    SourceTempl = Manager.Get<Template>(TemplateId);
                }





                //Services    *****************************************************************************
                NewTempl.Services = new List<ServiceContent>();

                if (NewServicesIds != null && NewServicesIds.Count() > 0)
                {

                    IList<Domain.DTO.Management.DTO_AddServices> dtoServices =
                        (from Domain.DTO.Management.DTO_AddServices dtosrvc in ServicesGetAvailable()
                         where NewServicesIds.Contains(dtosrvc.ServicesId)
                         select dtosrvc
                             ).ToList();

                    if (dtoServices != null && dtoServices.Count() > 0)
                    {

                        foreach (Domain.DTO.Management.DTO_AddServices dtosrv in dtoServices)
                        {
                            ServiceContent SrvConten = new ServiceContent();

                            SrvConten.CreateMetaInfo(this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                            SrvConten.IsActive = true;
                            SrvConten.ModuleCode = dtosrv.ServiceCode;
                            SrvConten.ModuleId = dtosrv.ServicesId;
                            SrvConten.ModuleName = dtosrv.ServiceName;
                            SrvConten.Template = NewTempl;

                            NewTempl.Services.Add(SrvConten);
                        }
                    }
                }

                TemplateVersion SourceVersion = null;
                TemplateVersion NewVersion = new TemplateVersion();

                //Get Version from Source Template
                if (SourceTempl != null)
                {
                    //if (SourceTempl.IsSystem)
                    //{
                    //    SourceVersion = (from TemplateVersion tvs in SourceTempl.Versions
                    //                     where !tvs.IsDraft && tvs.IsActive
                    //                     orderby tvs.ModifiedOn
                    //                     select tvs).FirstOrDefault();
                    //}
                    //else
                    if (SourceTempl.HasActive)  //Ultima DEFINITIVA
                    {
                        SourceVersion = (from TemplateVersion tvs in SourceTempl.Versions
                                         where !tvs.IsDraft && tvs.IsActive
                                         orderby tvs.ModifiedOn
                                         select tvs).LastOrDefault();
                    }
                    else if (SourceTempl.HasDefinitive) //Ultima DEPRECATA
                    {
                        SourceVersion = (from TemplateVersion tvs in SourceTempl.Versions
                                         where !tvs.IsDraft
                                         orderby tvs.ModifiedOn
                                         select tvs).LastOrDefault();
                    }
                    else        //Ultima DRAFT
                    {
                        SourceVersion = (from TemplateVersion tvs in SourceTempl.Versions
                                         orderby tvs.ModifiedOn
                                         select tvs).LastOrDefault();
                    }
                }

                int ElementVersion = 0;  //Per un NUOVO/Copia Template/Version, la verisone di ogni elementi è sempre = 1

                //Se ho trovato la version, ne creo una copia...
                if (SourceVersion != null)
                {
                    NewVersion.HasSignatures = SourceVersion.HasSignatures;
                    int count = 1;

                    dtoFiles.OldTemplateId = SourceTempl.Id;
                    dtoFiles.OldVersionId = SourceVersion.Id;

                    //Elements
                    if (SourceVersion.Elements != null && SourceVersion.Elements.Count() > 0)
                    {
                        IList<PageElement> oElms = (from PageElement elm in SourceVersion.Elements
                                                    where (elm.IsActive == true && elm.Deleted == BaseStatusDeleted.None)
                                                    select elm).ToList();

                        NewVersion.Elements = new List<PageElement>();

                        if (oElms != null && oElms.Count() > 0)
                        {
                            NewVersion.Elements = new List<PageElement>();
                            foreach (PageElement oelm in oElms)
                            {
                                PageElement NewElement = oelm.Copy(NewVersion, true, count, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                                if (oelm.GetType() == typeof(DocTemplateVers.ElementImage))
                                {
                                    DocTemplateVers.ElementImage Img = (DocTemplateVers.ElementImage)NewElement;
                                    Img.Path = dtoFiles.AddFile(Img.Path);//ImageCopy(Img.Path, SourceVersion.Template.Id, SourceVersion.Id, New);
                                    NewElement = Img;
                                }

                                NewVersion.Elements.Add(NewElement);


                            }
                        }
                    }

                    //Settings
                    if (SourceVersion.Settings != null && SourceVersion.Settings.Count() > 0)
                    {
                        Settings oSetts = (from Settings stt in SourceVersion.Settings
                                                  where (stt.IsActive == true && stt.Deleted == BaseStatusDeleted.None)
                                                  orderby stt.SubVersion descending
                                                  select stt).ToList().FirstOrDefault();

                        NewVersion.Settings = new List<Settings>();

                        if (oSetts != null)
                        {
                            //
                            Settings NewSettings = oSetts.Copy(NewVersion, true, ElementVersion, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                            if (!String.IsNullOrEmpty(oSetts.BackgroundImagePath))
                            {
                                NewSettings.BackgroundImagePath = dtoFiles.AddFile(oSetts.BackgroundImagePath); // ImageCopy(ost.BackgroundImagePath);
                            }
                            NewVersion.Settings.Add(NewSettings);
                        }
                    }

                    //Signatures
                    

                    if (SourceVersion.Signatures != null && SourceVersion.Signatures.Count() > 0)
                    {
                        IList<Signature> oSigns = (from Signature sgn in SourceVersion.Signatures
                                                   where (sgn.IsActive == true && sgn.Deleted == BaseStatusDeleted.None)
                                                   select sgn).ToList();

                        NewVersion.Signatures = new List<Signature>();

                        if (oSigns != null && oSigns.Count() > 0)
                        {
                            foreach (Signature ost in oSigns)
                            {
                                //NewVersion.Signatures.Add(
                                Signature NewSgn = ost.Copy(NewVersion, true, ElementVersion, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                                if (NewSgn.HasImage)
                                {
                                    NewSgn.Path = dtoFiles.AddFile(NewSgn.Path); // ImageCopy(osg.Path);
                                }

                                NewVersion.Signatures.Add(NewSgn);
                            }
                        }
                    }

                }
                else
                {      //NON HO VERSIONI SORGENTE, uso una versione di default
                    NewVersion = VersionGetNew();
                }

                NewTempl.Versions = new List<TemplateVersion>();
                NewTempl.Type = NewType;
                

                if (NewTempl.Type == TemplateType.System)
                {
                    NewTempl.IsSystem = true;
                    NewTempl.IsActive = true;
                    NewTempl.HasActive = true;
                    NewTempl.HasDefinitive = true;
                    NewTempl.HasDraft = false;

                    if (!NewVersion.IsActive || NewVersion.IsDraft)
                    {
                        throw new ArgumentException("Template without Active Version!");
                    }
                    NewVersion.IsActive = true;
                    NewVersion.IsDraft = false;
                    NewVersion.Template = NewTempl;
                    NewVersion.Version = 0;
                    NewVersion.CreateMetaInfo(this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                    NewTempl.Versions.Add(NewVersion);

                    IsSystem = true;
                }
                else
                { 
                    NewTempl.IsSystem = false;
                    NewTempl.IsActive = false;
                    NewTempl.HasActive = false;
                    NewTempl.HasDefinitive = false;
                    NewTempl.HasDraft = true;

                    NewVersion.IsActive = false;
                    NewVersion.IsDraft = true;
                    NewVersion.Template = NewTempl;
                    NewVersion.Version = 0;
                    NewVersion.CreateMetaInfo(this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                    NewTempl.Versions.Add(NewVersion);

                    IsSystem = false;
                }

                
                NewTempl.Name = NewName;
                NewTempl.CreateMetaInfo(this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                
                try
                {
                    Manager.BeginTransaction();
                    //Manager.SaveOrUpdate<DocTemplateVers.TemplateVersion>(NewVersion);
                    Manager.SaveOrUpdate<DocTemplateVers.Template>(NewTempl);
                    Manager.Commit();
                    NewId = NewTempl.Id;

                    NewVersId = (from DocTemplateVers.TemplateVersion vrs in NewTempl.Versions select vrs.Id).FirstOrDefault();

                    dtoFiles.NewTemplateId = NewId;
                    dtoFiles.NewVersionId = NewVersId;

                    ImageCopyBlock(dtoFiles);
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    NewId = -1;
                }
            }



            return NewId;
        }

        public Domain.DTO.Management.DTO_AddTemplate TemplateGetForAdd(Int64 TemplateId)
        {
            Domain.DTO.Management.DTO_AddTemplate Template = new Management.DTO_AddTemplate();

            Template srctmpl = Manager.Get<Template>(TemplateId);

            if (srctmpl != null)
            {
                Template.Name = srctmpl.Name;
                Template.Type = srctmpl.Type;
                Template.Services = new List<Domain.DTO.Management.DTO_AddServices>();

                if (srctmpl.Services != null && srctmpl.Services.Count() > 0)
                {
                    IList<Int64> SrvcIds = (from ServiceContent sc in srctmpl.Services where sc.IsActive select sc.ModuleId).ToList();

                    foreach(Domain.DTO.Management.DTO_AddServices dtoSrvc in this.ServicesGetAvailable())
                    {
                        dtoSrvc.Selected = (SrvcIds.Contains(dtoSrvc.ServicesId));
                        Template.Services.Add(dtoSrvc);
                    }
                }

                //Per la creazione di un TEMPLATE di SISTEMA:
                //L'abilito SE parto da un TEMPLATE che ha verisoni DEFINITIVE!
                Template.IsActiveSystem = (srctmpl.Type == TemplateType.Skin || srctmpl.Type == TemplateType.Standard) && srctmpl.HasDefinitive;

            }


            return Template;
        }
      
        #region SERVICE     !!!!!!!!!                -RIVEDERE CON LE FUNZIONI CORRETTE!!!! (Vedi pagine di edit template vecchie (internazionalizzazione e recupero)
        public IList<Domain.DTO.Management.DTO_AddServices> ServicesGetAvailable()
        {

            IList<Domain.DTO.Management.DTO_AddServices> srvs = (
                from ModuleDefinition mdl
                in Manager.GetIQ<ModuleDefinition>()
                where 
                    ((mdl.Code == "SRVEDUP")
                    || (mdl.Code == "SRVCFP"))
                    && mdl.Available
                select new Domain.DTO.Management.DTO_AddServices() { ServicesId = mdl.Id, ServiceName = mdl.Name, ServiceCode = mdl.Code }
                ).ToList();

            
            //Domain.DTO.Management.DTO_AddServices srv = new Domain.DTO.Management.DTO_AddServices();
            //srv.ServiceName = "Percorso formativo";
            //srv.ServicesId = 43;
            //srv.ServiceCode = "SRVEDUP";

            //srvs.Add(srv);

            //Servizio Edupath

            //Dim serv As PlainService = modules.Where(Function(m) m.Code = "SRVEDUP").FirstOrDefault()
            //Dim AvServices As New List(Of TemplateVers.Domain.DTO.Management.DTO_AddServices)()
            //Dim srvItm As New TemplateVers.Domain.DTO.Management.DTO_AddServices()
            //srvItm.ServiceCode = serv.Code
            //srvItm.ServiceName = serv.Name
            //srvItm.ServicesId = serv.ID

            return srvs;
        }

        //private IList<Domain.DTO.Management.DTO_AddServices> GetSelectedServices()
        //{
        //    IList<Int64> SelectedIds = View.SelectedServicesId;
        //    IList<Templ.Domain.DTO.Management.DTO_ListService> dtoServices = new List<Templ.Domain.DTO.Management.DTO_ListService>();

        //    if (SelectedIds != null && SelectedIds.Count > 0)
        //    {
        //        dtoServices = (
        //        from Templ.Domain.DTO.Management.DTO_ListService dtosrv in GetAvailableServices()
        //        where SelectedIds.Contains(dtosrv.IdModule)
        //        select dtosrv).ToList();
        //    }

        //    return dtoServices;

        //    //IList<Templ.Domain.DTO.Management.DTO_ListService> srvs = new List<Templ.Domain.DTO.Management.DTO_ListService>();

        //    //foreach (Int64 Id in View.SelectedServicesId)
        //    //{
        //    //    //METTERCI IL RECUPERO DEI DATI REALI!!!!
        //    //    Templ.Domain.DTO.Management.DTO_ListService srv = new Templ.Domain.DTO.Management.DTO_ListService();
        //    //    srv.IdModule = Id;
        //    //    srv.Name = "Service_" + Id.ToString();
        //    //    srv.Code = "SRV_" + Id.ToString();
        //    //}

        //    //return srvs;
        //}
        #endregion
    #endregion


#region "Permission"
    #region Public
        /// <summary>
        /// Get Service Module Id
        /// </summary>
        /// <returns>Service Module Id</returns>
        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleDocTemplate.UniqueCode);
        }
        /// <summary>
        /// Get Specific Service Permission
        /// </summary>
        /// <param name="personId">
        /// Specific Person ID
        /// </param>
        /// <param name="idCommunity">
        /// Specific Community ID
        /// </param>
        /// <returns>ModuleDocTemplate permission</returns>
        public ModuleDocTemplate GetServicePermission(int personId, int idCommunity)
        {
            Person person = Manager.GetPerson(personId);
            return GetServicePermission(person, idCommunity);
        }
        /// <summary>
        /// Get Specific Service Permission
        /// </summary>
        /// <param name="person">
        /// Specific Person
        /// </param>
        /// <param name="idCommunity">
        /// Specific Community ID
        /// </param>
        /// <returns>ModuleDocTemplate permission</returns>
        public ModuleDocTemplate GetServicePermission(Person person, int idCommunity)
        {
            ModuleDocTemplate module = new ModuleDocTemplate();
            if (person == null)
                person = (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
            if (idCommunity == 0)
                module = ModuleDocTemplate.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
            else
                module = new ModuleDocTemplate(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID()));
            return module;
        }

        public bool ServiceIsEnabled(Int32 ServiceId)
        {
            ModuleDefinition mod = Manager.Get<ModuleDefinition>(ServiceId);
            return ((mod != null) && mod.Available);
        }
    #endregion
    #region Internal
        private int _UserId;
        /// <summary>
        /// Get Current UserId
        /// </summary>
        private int UserId
        {
            get
            {
                if (_UserId == null || _UserId == 0)
                {
                    _UserId = base.UC.CurrentUserID;
                }
                return _UserId;
            }
        }

        private ModuleDocTemplate _Permission;
        /// <summary>
        /// Get ModuleDocTemplate Permission for current User and Current Community
        /// </summary>
        private ModuleDocTemplate Permission
        {
            get
            {
                ////per TEST
                //ModuleDocTemplate Perm = new ModuleDocTemplate();
                //Perm.AddTemplate = true;
                //Perm.DeleteTemplates = true;
                //Perm.EditBuiltInTemplates = true;
                //Perm.EditTemplates = true;
                //Perm.ManageTemplates = true;
                //Perm.ViewTemplates = true;

                //return Perm;
                ////end TEST!

                if (_Permission == null)
                {
                    _Permission = this.GetServicePermission(this.UC.CurrentUserID, this.UC.CurrentCommunityID);
                }
                return _Permission;
            }
        }

        private Person _CU;
        private Person CurrentUser
        {
            get
            {
                if (_CU == null)
                {
                    _CU = Manager.GetPerson(UserId);
                }
                return _CU;
            }
        }

        private Management.DTO_EditElementPermission _ItemPermission;
        /// <summary>
        /// Recupera i permessi dei singoli elementi
        /// </summary>
        /// <returns>I permessi per i singoli elementi</returns>
        /// <remarks>
        /// Al momento i permessi sono uguali per TUTTI gli oggetti, ma QUI è possibile implementare logiche diverse
        /// basate sui singoli oggetti o altre logiche...
        /// </remarks>
        private Management.DTO_EditElementPermission GetItemPermission()
        {
            if (_ItemPermission == null)
            {
                _ItemPermission = new Management.DTO_EditElementPermission();
                _ItemPermission.Delete = Permission.DeleteTemplates;
                _ItemPermission.Preview = Permission.ViewTemplates;
                _ItemPermission.Recover = Permission.DeleteTemplates;
            }

            return _ItemPermission;
        }
    #endregion
#endregion

#region LIST
        //TEST EFFETTUATI:
        // Creazione nuovo template
        // cancellazione logica e recupero template "vuoto"
        // copia template "vuoto"
        // cancellazione fisica dei 2 template
        
    #region List Template
        /// <summary>
        /// Recupera la lista di DTO_Template per la visualizzazione della lista.
        /// </summary>
        /// <param name="PersonID">
        /// Id Person:
        /// in base ai permessi del modulo, vengono recuperati gli oggetti opportuni e settati i permessi associati al singolo oggetto
        /// </param>
        /// <param name="Filters">
        /// MARCATORE
        /// Attualmente solo come segnaposto. Saranno successivamente definiti eventuali filtri, paginazioni, etc...
        /// </param>
        /// <returns></returns>
        public IList<Management.DTO_ListTemplate> TemplateListGet(Domain.DTO.Management.TemplateFilter Filter, Domain.DTO.Management.TemplateOrderCol OrderBy, Boolean Ascending)
        {
            IList<Management.DTO_ListTemplate> Templates = new List<Management.DTO_ListTemplate>();

            if (Permission.ViewTemplates)
            {
                IList<Template> SourceTemplates = null;
                
                switch (Filter)
                {
                    case Management.TemplateFilter.ALL:
                        SourceTemplates = Manager.GetAll<Template>().ToList();
                        break;
                    case Management.TemplateFilter.Definitive:
                        SourceTemplates = Manager.GetAll<Template>(t => t.HasDefinitive == true).ToList();
                        break;
                    case Management.TemplateFilter.Deleted:
                        SourceTemplates = Manager.GetAll<Template>(t => t.Deleted != BaseStatusDeleted.None).ToList();
                        break;
                    case Management.TemplateFilter.Draft:
                        SourceTemplates = Manager.GetAll<Template>(t => t.HasDraft == true).ToList();
                        break;
                }

                
                //.ToList();
                

                //Templates = Manager.GetAll<Management.DTO_ListTemplate>().ToList();
                if (SourceTemplates != null)
                {


                    foreach (Template SourceTmpl in SourceTemplates)
                    {
                        Management.DTO_ListTemplate tmpl = new Management.DTO_ListTemplate();

                        // Base Data
                        tmpl.Creation = SourceTmpl.CreatedOn;
                        tmpl.Deleted = SourceTmpl.Deleted;
                        tmpl.HasActive = SourceTmpl.HasActive;
                        tmpl.HasDefinitive = SourceTmpl.HasDefinitive;
                        tmpl.HasDraft = SourceTmpl.HasDraft;
                        tmpl.Id = SourceTmpl.Id;
                        tmpl.IsActive = SourceTmpl.IsActive;
                        tmpl.IsSystem = SourceTmpl.IsSystem;
                        tmpl.LastModify = SourceTmpl.ModifiedOn;
                        tmpl.Name = SourceTmpl.Name;
                        tmpl.Type = SourceTmpl.Type;

                        // Permissions
                        tmpl.Permissions = new Management.DTO_ListPermission();

                        tmpl.Permissions.AllowNewVersion = Permission.EditTemplates 
                            && tmpl.HasDraft == false 
                            && (!tmpl.IsSystem); // || tmpl.IsSystem && Permission.EditBuiltInTemplates);

                        tmpl.Permissions.Activate = Permission.ManageTemplates && (SourceTmpl.IsActive == false) && !tmpl.IsSystem && tmpl.HasDefinitive;
                        tmpl.Permissions.DeActivate = Permission.ManageTemplates && (SourceTmpl.IsActive == true) && !tmpl.IsSystem;

                        tmpl.Permissions.DeleteVirtual = Permission.DeleteTemplates && tmpl.Deleted == BaseStatusDeleted.None && !tmpl.IsSystem && !tmpl.HasDefinitive;
                        tmpl.Permissions.DeletePhisical = Permission.DeleteTemplates && tmpl.Deleted != BaseStatusDeleted.None && !tmpl.IsSystem && !tmpl.HasDefinitive;
                        tmpl.Permissions.UndeleteVirtual = Permission.DeleteTemplates && tmpl.Deleted != BaseStatusDeleted.None && !tmpl.IsSystem && !tmpl.HasDefinitive;

                        tmpl.Permissions.Edit = Permission.EditTemplates;


                        //Versions
                        tmpl.TemplateVersions = new List<Management.DTO_ListTemplateVersion>();

                        if (SourceTmpl.Versions != null && SourceTmpl.Versions.Count() > 0)
                        {
                            foreach (TemplateVersion SourceVer in SourceTmpl.Versions)
                            {
                                Management.DTO_ListTemplateVersion vers = new Management.DTO_ListTemplateVersion();

                                //Base Data - VERSION

                                vers.Creation = SourceVer.CreatedOn;
                                vers.Deleted = SourceVer.Deleted;
                                vers.Id = SourceVer.Id;
                                vers.IsActive = SourceVer.IsActive;
                                vers.IsDraft = SourceVer.IsDraft;
                                vers.LastModify = SourceVer.ModifiedOn;
                                vers.Template = tmpl;
                                vers.Version = SourceVer.Version;

                                if (SourceVer.ModifiedBy != null)
                                {
                                    vers.LastModifiedBy = SourceVer.ModifiedBy.SurnameAndName;
                                }
                                else if (SourceVer.CreatedBy != null)
                                {
                                    vers.LastModifiedBy = SourceVer.CreatedBy.SurnameAndName;
                                }
                                else
                                {
                                    vers.LastModifiedBy = "";
                                }

                                //Permission - VERSION

                                vers.Permissions = new Management.DTO_ListVersionPermission();

                                vers.Permissions.DeleteVirtual = Permission.DeleteTemplates && vers.Deleted == BaseStatusDeleted.None && vers.IsDraft;
                                vers.Permissions.UndeleteVirtual = Permission.DeleteTemplates && vers.Deleted != BaseStatusDeleted.None && vers.IsDraft;
                                vers.Permissions.DeletePhisical = Permission.DeleteTemplates && vers.Deleted != BaseStatusDeleted.None && vers.IsDraft;

                                vers.Permissions.Copy = Permission.EditTemplates && vers.Template.HasDraft == false;

                                vers.Permissions.Activate = Permission.EditTemplates && !vers.IsActive;
                                vers.Permissions.DeActivate = Permission.EditTemplates && vers.IsActive;

                                vers.Permissions.AllowNewVersion = Permission.ManageTemplates;

                                vers.Permissions.Edit = Permission.EditTemplates && vers.IsDraft;
                                vers.Permissions.Preview = Permission.ViewTemplates;
                                //vers.Permissions.SetDefinitive = Permission.ManageTemplates && vers.IsDraft;  /<-- Diventa "Attiva"


                                tmpl.TemplateVersions.Add(vers);
                            }


                        }

                        //Services
                        tmpl.Services = new List<Management.DTO_ListService>();

                        if (SourceTmpl.Services != null && SourceTmpl.Services.Count() > 0)
                        {
                            foreach (ServiceContent SourceSrv in SourceTmpl.Services)
                            {
                                Management.DTO_ListService srv = new Management.DTO_ListService();
                                srv.Id = SourceSrv.Id;
                                srv.Code = SourceSrv.ModuleCode;
                                srv.IdModule = SourceSrv.ModuleId;
                                srv.Name = SourceSrv.ModuleName;
                                srv.Template = tmpl;

                                tmpl.Services.Add(srv);
                            }
                        }

                        Templates.Add(tmpl);
                    }

                    if (Ascending)
                    {
                        switch (OrderBy)
                        {
                            case Management.TemplateOrderCol.Name:
                                Templates = Templates.OrderBy(t => t.Name).ToList();
                                break;
                            //case Management.TemplateOrderCol.Status:
                            //    Templates = Templates.OrderBy(t => t.s).ToList();
                            //    break;
                            case Management.TemplateOrderCol.UpdatedOn:
                                Templates = Templates.OrderBy(t => t.Creation).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (OrderBy)
                        {
                            case Management.TemplateOrderCol.Name:
                                Templates = Templates.OrderByDescending(t => t.Name).ToList();
                                break;
                            //case Management.TemplateOrderCol.Status:
                            //    Templates = Templates.OrderByDescending(t => t.s).ToList();
                            //    break;
                            case Management.TemplateOrderCol.UpdatedOn:
                                Templates = Templates.OrderByDescending(t => t.Creation).ToList();
                                break;
                        }
                    }
                }
                else
                {
                    SourceTemplates = new List<Template>();
                }
            }
            return Templates;
        }

        /// <summary>
        /// Cancellazione logica di un Template
        /// </summary>
        /// <param name="TemplateId">Id del template da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean TemplateDeleteLogical(Int64 TemplateId)
        {
            Boolean Deleted = false;
            if (this.Permission.DeleteTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    DocTemplateVers.Template Template = Manager.Get<DocTemplateVers.Template>(TemplateId);

                    if (Template != null && !Template.HasDefinitive)
                    {
                        Template.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);


                        //Services
                        if (Template.Services != null && Template.Services.Count > 0)
                        {
                            foreach (DocTemplateVers.ServiceContent svc in Template.Services)
                            {
                                if (svc.Deleted == BaseStatusDeleted.None)
                                {
                                    svc.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                    svc.Deleted = BaseStatusDeleted.Cascade;
                                }
                            }
                        }


                        if (Template.Versions != null && Template.Versions.Count() > 0)
                        {
                            foreach(TemplateVersion vrs in Template.Versions)
                            {
                                if (vrs.Deleted == BaseStatusDeleted.None)
                                {
                                    vrs.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                    vrs.Deleted = BaseStatusDeleted.Cascade;
                                }
                                //settings
                                if (vrs.Settings != null && vrs.Settings.Count > 0)
                                {
                                    foreach (DocTemplateVers.Settings stg in vrs.Settings)
                                    {
                                        if(stg.Deleted == BaseStatusDeleted.None)
                                        {
                                            stg.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            stg.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                }
                                //Signatures
                                if (vrs.Signatures != null && vrs.Signatures.Count > 0)
                                {
                                    foreach (DocTemplateVers.Signature sgn in vrs.Signatures)
                                    {
                                        if(sgn.Deleted == BaseStatusDeleted.None)
                                        {
                                            sgn.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            sgn.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                }
                                
                                //Elements
                                if (vrs.Elements != null && vrs.Elements.Count > 0)
                                {
                                    foreach (DocTemplateVers.PageElement pge in vrs.Elements)
                                    {
                                        if(pge.Deleted == BaseStatusDeleted.None)
                                        {
                                            pge.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            pge.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                }
                            }
                        }
                        
                        Manager.SaveOrUpdate<DocTemplateVers.Template>(Template);
                        Deleted = true;
                    }
                    
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Deleted = false;
                }
            }
            return Deleted;
        }
        /// <summary>
        /// Cancellazione FISICA di un template
        /// </summary>
        /// <param name="TemplateId">Id del template da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean TemplateDeletePhisical(Int64 TemplateId)
        {
            //If HasPermessi && ! Template.HasDefinitive && IsDeleted
            // Delete
            Boolean Deleted = false;
            IList<String> FileToDelete = new List<String>();

            if (this.Permission.DeleteTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    DocTemplateVers.Template Template = Manager.Get<DocTemplateVers.Template>(TemplateId);
                    //Template.UpdateInfo();
                    

                    //DocTemplateVers.TemplateVersion vers = new TemplateVersion();


                    FileToDelete = (from DocTemplateVers.TemplateVersion vrs in Template.Versions
                                    from DocTemplateVers.PageElement elm in vrs.Elements
                                    where elm.GetType() == typeof(DocTemplateVers.ElementImage)
                                    select ((DocTemplateVers.ElementImage)elm).Path).ToList();


                    if (Template != null && !Template.HasActive && !Template.HasDefinitive)
                    {
                        //if (Settings != null)
                        //    Settings.Count();
                        //if (Elements != null)
                        //    Elements.Count();
                        //if (Services != null)
                        //    Services.Count();
                        //if (Signatures != null)
                        //    Signatures.Count();
                        //Manager.BeginTransaction();

                        //Template.Versions.ToList().ForEach(v => Manager.DeletePhysical<DocTemplateVers.TemplateVersion>(v));
                        //Manager.SaveOrUpdate<Template>(Template);
                        Manager.DeletePhysical<DocTemplateVers.Template>(Template);
                        Deleted = true;
                    }
                    Manager.Flush();
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Deleted = false;
                }
            }
            return Deleted;
        }
        /// <summary>
        /// Recupero di un template cancellato logicamente
        /// </summary>
        /// <param name="TemplateId">Id del Template da recuperare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean TemplateUnDelete(Int64 TemplateId)
        {
            //If HasPermessi && ! Template.HasDefinitive && IsDeleted
            // Undelete
            Boolean Recovered = false;
            if (this.Permission.DeleteTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    DocTemplateVers.Template Template = Manager.Get<DocTemplateVers.Template>(TemplateId);

                    if (Template != null)
                    {
                        Template.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                        //Services
                        if (Template.Services != null && Template.Services.Count > 0)
                        {
                            foreach (DocTemplateVers.ServiceContent svc in Template.Services)
                            {
                                if (svc.Deleted == BaseStatusDeleted.Cascade)
                                    svc.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                //svc.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }

                        if (Template != null && !Template.HasDefinitive)
                        {
                            Template.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                            if (Template.Versions != null && Template.Versions.Count() > 0)
                            {
                                foreach (TemplateVersion vrs in Template.Versions)
                                {
                                    if (vrs.Deleted == BaseStatusDeleted.Cascade)
                                        vrs.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                    //vrs.Deleted = BaseStatusDeleted.Cascade;

                                    //settings
                                    if (vrs.Settings != null && vrs.Settings.Count > 0)
                                    {
                                        foreach (DocTemplateVers.Settings stg in vrs.Settings)
                                        {
                                            if (stg.Deleted == BaseStatusDeleted.Cascade)
                                                stg.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            //stg.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                    //Signatures
                                    if (vrs.Signatures != null && vrs.Signatures.Count > 0)
                                    {
                                        foreach (DocTemplateVers.Signature sgn in vrs.Signatures)
                                        {
                                            if (sgn.Deleted == BaseStatusDeleted.Cascade)
                                                sgn.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            //sgn.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                   
                                    //Elements
                                    if (vrs.Elements != null && vrs.Elements.Count > 0)
                                    {
                                        foreach (DocTemplateVers.PageElement pge in vrs.Elements)
                                        {
                                            if (pge.Deleted == BaseStatusDeleted.Cascade)
                                                pge.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                            //pge.Deleted = BaseStatusDeleted.Cascade;
                                        }
                                    }
                                }
                            }
                            Manager.SaveOrUpdate<DocTemplateVers.Template>(Template);
                            Recovered = true;
                        }

                        Manager.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Recovered = false;
                }
            }
            return Recovered;
        }

        /// <summary>
        /// Recupero di un template cancellato logicamente
        /// </summary>
        /// <param name="TemplateId">Id del Template da recuperare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean TemplateSetActive(Int64 TemplateId, Boolean IsActive)
        {
            //If HasPermessi && ! Template.HasDefinitive && IsDeleted
            // Undelete
            Boolean Enabled = false;
            if (this.Permission.EditTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    DocTemplateVers.Template Template = Manager.Get<DocTemplateVers.Template>(TemplateId);

                    if (Template != null)
                    {
                        Template.UpdateMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                        Template.IsActive = IsActive;
                        
                        Manager.SaveOrUpdate<DocTemplateVers.Template>(Template);
                        Enabled = true;
                        
                        Manager.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Enabled = false;
                }
            }
            return Enabled;
        }

        ///// <summary>
        ///// Crea un nuovo Template
        ///// </summary>
        ///// <param name="Name">Nome da assegnare al nuovo template</param>
        ///// <param name="TemplateType">Tipo del nuovo template</param>
        ///// <returns>L'ID del nuovo template appena creato</returns>
        ///// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        //public Int64 TemplateCreateNew(String Name, TemplateType TemplateType)
        //{
        //    Int64 Id = 0;

        //    Template NewTemplate = new Template();
        //    NewTemplate.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
        //    NewTemplate.HasActive = false;
        //    NewTemplate.HasDraft = false;
        //    NewTemplate.IsActive = false;
        //    NewTemplate.Type = TemplateType;

        //    NewTemplate.IsSystem = (TemplateType == DocTemplateVers.TemplateType.System);

        //    NewTemplate.Name = Name;
        //    NewTemplate.Versions = new List<DocTemplateVers.TemplateVersion>();

            

        //    try
        //    {
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<DocTemplateVers.Template>(NewTemplate);
        //        Manager.Commit();
        //        Id = NewTemplate.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Manager.IsInTransaction())
        //            Manager.RollBack();
        //        Id = -1;
        //    }

        //    return Id;
        //}
    #endregion
    #region List Version
        // <summary>
        // Imposta una versione come DEFINITIVA
        // </summary>
        // <param name="TempalteId">ID del relativo Template</param>
        // <param name="VersionId">ID della versione da rendere definitiva</param>
        // <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        //public Boolean VersionSetDefinitive(Int64 VersionId)
        //{
        //    Boolean Set = false;
        //    if (Permission.ManageTemplates)
        //    {
        //        try
        //        {
        //            Manager.BeginTransaction();
        //            TemplateVersion Version = Manager.Get<TemplateVersion>(VersionId);
        //            Version.UpdateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
        //            if (Version.Deleted == BaseStatusDeleted.None && Version.IsDraft)
        //            {
        //                Version.IsDraft = false;
        //                Version.IsActive = true;
        //                Version.Template.UpdateInfo();
        //            }

        //            Manager.SaveOrUpdate<TemplateVersion>(Version);
        //            Manager.Commit();
        //            Set = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            if (Manager.IsInTransaction())
        //                Manager.RollBack();
        //            Set = false;
        //        }
        //    }
        //    return Set;
        //}

        /// <summary>
        /// Imposta una versione come ATTIVA (Selezionabile da altri servizi)
        /// </summary>
        /// <param name="TempalteId">Id del relativo Template</param>
        /// <param name="VersionId">Id del relativo TemplateVersion</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean VersionSetActive(Int64 TempalteId, Int64 VersionId)
        {
            Boolean Set = false;
            if (Permission.ManageTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    TemplateVersion Version = Manager.Get<TemplateVersion>(VersionId);
                    Version.UpdateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    if (Version.Deleted == BaseStatusDeleted.None)// && !Version.IsDraft)
                    {
                        Version.IsDraft = false;
                        Version.IsActive = true;
                        Version.Template.HasActive = true;
                        Version.Template.HasDefinitive = true;
                        Version.Template.IsActive = true;
                        Version.Template.UpdateInfo();
                    }

                    Manager.SaveOrUpdate<TemplateVersion>(Version);
                    Manager.Commit();
                    Set = true;
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Set = false;
                }
            }
            return Set;
        }
        /// <summary>
        /// Imposta una versione come DISATTIVA (NON selezionabile da altri servizi)
        /// </summary>
        /// <param name="TempalteId">Id del relativo Template</param>
        /// <param name="VersionId">Id del relativo TemplateVersion</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean VersionSetDeActive(Int64 TempalteId, Int64 VersionId)
        {
            Boolean Set = false;
            if (Permission.ManageTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    TemplateVersion Version = Manager.Get<TemplateVersion>(VersionId);
                    Version.UpdateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    if (Version.Deleted == BaseStatusDeleted.None && !Version.IsDraft)
                    {
                        Version.IsDraft = false;
                        Version.IsActive = false;

                        if ((from TemplateVersion ver in Version.Template.Versions where (ver.IsActive == true) select ver.Id).Count() > 0)
                        {
                            Version.Template.HasActive = true;
                        }
                        else
                        {
                            Version.Template.HasActive = false;
                        }
                        
                        Version.Template.UpdateInfo();
                    }

                    Manager.SaveOrUpdate<TemplateVersion>(Version);
                    Manager.Commit();
                    Set = true;
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Set = false;
                }
            }
            return Set;
        }
        /// <summary>
        /// Crea una copia in BOZZA dell'ultima versione disponibile per un Template
        /// </summary>
        /// <param name="TemplateId">L'ID del relativo Template</param>
        /// <returns>L'ID della nuova Version</returns>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Int64 VersionCreateCopy(Int64 TemplateId)
        {
            return VersionCreateCopy(TemplateId, 0);
        }
        /// <summary>
        /// Crea una copia in BOZZA dell'ultima versione disponibile per un Template
        /// </summary>
        /// <param name="TemplateId">L'ID del relativo Template</param>
        /// <param name="VersionId">
        /// L'ID della versione sorgente.
        /// Se = 0, verrà presa la DEFINITIVA più recente, oppure creata una nuova.
        /// </param>
        /// <returns>L'ID della nuova Version</returns>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>/// <param name="TemplateId"></param>
        public Int64 VersionCreateCopy(Int64 TemplateId, Int64 VersionId)
        {
            Template SourceTempl = Manager.Get<Template>(TemplateId);
            TemplateVersion SourceVersion = null;
            Int64 NewId = 0;

            if (this.Permission.ManageTemplates && SourceTempl != null)
            {
                //int LatestVersion = 0;

                Domain.Core.dtoFileCopyBlock dtoFiles = new Domain.Core.dtoFileCopyBlock();
                

                if(SourceTempl.HasDraft == false && SourceTempl.Versions != null && SourceTempl.Versions.Count > 0)
                {
                    if (VersionId <= 0)
                    {
                        SourceVersion = (from TemplateVersion vrs
                                            in SourceTempl.Versions
                                         where vrs.IsDraft == false
                                         orderby vrs.Version descending
                                         select vrs).FirstOrDefault();
                        //LatestVersion = SourceVersion.Version;
                    }
                    else
                    {
                        SourceVersion = (from TemplateVersion vrs
                                            in SourceTempl.Versions
                                         where vrs.Id == VersionId
                                         orderby vrs.Version descending
                                         select vrs).FirstOrDefault();
                        //LatestVersion = (from TemplateVersion vrs
                        //                    in SourceTempl.Versions
                        //                 where vrs.IsDraft == false
                        //                 orderby vrs.Version descending
                        //                 select vrs.Version).FirstOrDefault();
                    }

                }

                
                if (SourceVersion != null)
                {
                    
                    TemplateVersion NewVersion = new TemplateVersion();

                    NewVersion.CreateMetaInfo(this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                    NewVersion.HasSignatures = SourceVersion.HasSignatures;
                    NewVersion.IsActive = false;
                    NewVersion.IsDraft = true;
                    NewVersion.Template = SourceTempl;
                    NewVersion.Version = SourceTempl.Versions.Count;
                    int count = 1;

                    dtoFiles.OldTemplateId = SourceTempl.Id;
                    dtoFiles.OldVersionId = SourceVersion.Id;

                    //Elements
                    if (SourceVersion.Elements != null && SourceVersion.Elements.Count() > 0)
                    {
                        IList<PageElement> oElms = (from PageElement elm in SourceVersion.Elements
                                                    where (elm.IsActive == true && elm.Deleted == BaseStatusDeleted.None)
                                                    select elm).ToList();


                        if (oElms != null && oElms.Count() > 0)
                        {
                            NewVersion.Elements = new List<PageElement>();
                            foreach (PageElement oelm in oElms)
                            {
                                PageElement NewElement = oelm.Copy(NewVersion, true, count, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                                if (oelm.GetType() == typeof(DocTemplateVers.ElementImage))
                                {
                                    DocTemplateVers.ElementImage Img = (DocTemplateVers.ElementImage)NewElement;
                                    Img.Path = dtoFiles.AddFile(Img.Path);//ImageCopy(Img.Path, SourceVersion.Template.Id, SourceVersion.Id, New);
                                    NewElement = Img;
                                }

                                NewVersion.Elements.Add(NewElement);

                                //count += 1;
                            }
                        }
                    }

                    //Settings
                    if (SourceVersion.Settings != null && SourceVersion.Settings.Count() > 0)
                    {
                        //IList<Settings> oSetts = (from Settings stt in SourceVersion.Settings
                        //                          where (stt.IsActive == true && stt.Deleted == BaseStatusDeleted.None)
                        //                          orderby stt.SubVersion descending
                        //                          select stt).ToList();
                        Settings oSett = (from Settings stt in SourceVersion.Settings
                                                  where (stt.IsActive == true && stt.Deleted == BaseStatusDeleted.None)
                                                  orderby stt.SubVersion descending
                                                  select stt).FirstOrDefault();

                        NewVersion.Settings = new List<Settings>();

                        if (oSett != null)// && oSetts.Count() > 0)
                        {
                            //foreach (Settings ost in oSetts)
                            //{
                            Settings NewSettings = oSett.Copy(NewVersion, true, count, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                            if (!String.IsNullOrEmpty(oSett.BackgroundImagePath))
                            {
                                NewSettings.BackgroundImagePath = dtoFiles.AddFile(oSett.BackgroundImagePath); // ImageCopy(ost.BackgroundImagePath);
                            }

                            NewVersion.Settings.Add(NewSettings);
                            //}
                        }
                    }

                    //Signatures
                    if (SourceVersion.Signatures != null && SourceVersion.Signatures.Count() > 0)
                    {
                        IList<Signature> oSigns = (from Signature sgn in SourceVersion.Signatures
                                                   where (sgn.IsActive == true && sgn.Deleted == BaseStatusDeleted.None)
                                                   select sgn).ToList();

                        NewVersion.Signatures = new List<Signature>();

                        if (oSigns != null && oSigns.Count() > 0)
                        {
                            foreach (Signature osg in oSigns)
                            {
                                Signature NewSgn = osg.Copy(NewVersion, true, count, this.CurrentUser, UC.IpAddress, UC.ProxyIpAddress);

                                if (osg.HasImage)
                                {
                                    NewSgn.Path = dtoFiles.AddFile(osg.Path); // ImageCopy(osg.Path);
                                }

                                NewVersion.Signatures.Add(NewSgn);
                            }
                        }
                    }

                    SourceTempl.Versions.Add(NewVersion);
                    SourceTempl.HasDraft = true;

                    try
                    {
                        Manager.BeginTransaction();
                        Manager.SaveOrUpdate<DocTemplateVers.TemplateVersion>(NewVersion);
                        Manager.SaveOrUpdate<DocTemplateVers.Template>(SourceTempl);
                        Manager.Commit();
                        NewId = NewVersion.Id;
                        
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        NewId = -1;
                    }

                    dtoFiles.NewTemplateId = NewVersion.Template.Id;
                    dtoFiles.NewVersionId = NewId;

                    ImageCopyBlock(dtoFiles);
                }   
            }
            return NewId;
        }

        /// <summary>
        /// Cancellazione logica di una versione
        /// </summary>
        /// <param name="TemplateId">ID del relativo Template</param>
        /// <param name="VersionId">ID della versione da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean VersionDeleteLogical(Int64 VersionId)
        {
            Boolean Deleted = false;
            if (this.Permission.DeleteTemplates)
            {
                try
                {
                    TemplateVersion vrs = Manager.Get<TemplateVersion>(VersionId);


                    if(vrs != null && vrs.IsDraft)
                    {
                        if (vrs.Deleted == BaseStatusDeleted.None)
                        {
                            vrs.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                            vrs.Deleted = BaseStatusDeleted.Manual;
                        }
                        //settings
                        if (vrs.Settings != null && vrs.Settings.Count > 0)
                        {
                            foreach (DocTemplateVers.Settings stg in vrs.Settings.Where(s => s.Deleted == BaseStatusDeleted.None))
                            {
                                stg.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                stg.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }
                        //Signatures
                        if (vrs.Signatures != null && vrs.Signatures.Count > 0)
                        {
                            foreach (DocTemplateVers.Signature sgn in vrs.Signatures.Where(s => s.Deleted == BaseStatusDeleted.None))
                            {
                                sgn.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                sgn.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }

                        //Elements
                        if (vrs.Elements != null && vrs.Elements.Count > 0)
                        {
                            foreach (DocTemplateVers.PageElement pge in vrs.Elements.Where(s => s.Deleted == BaseStatusDeleted.None))
                            {
                                pge.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                pge.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }

                        vrs.Template.UpdateInfo();

                        Manager.SaveOrUpdate<TemplateVersion>(vrs);
                        Deleted = true;
                    }

                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Deleted = false;
                }
            }
            return Deleted;
        }
        /// <summary>
        /// Cancellazione FISICA di una versione 
        /// </summary>
        /// <param name="TemplateId">ID del relativo Template</param>
        /// <param name="VersionId">ID della versione da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean VersionDeletePhisical(Int64 VersionId)
        {
            Boolean Deleted = false;
            if (Permission.DeleteTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    TemplateVersion vers = Manager.Get<TemplateVersion>(VersionId);
                    Template tmpl = vers.Template;

                    if (vers.IsDraft && vers.Deleted != BaseStatusDeleted.None)
                    {
                        Manager.DeletePhysical<TemplateVersion>(vers);
                        tmpl.HasDraft = false;

                        Manager.Commit();
                        Deleted = true;
                    }
                }
                catch
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Deleted = false;
                }
            }
            return Deleted;
        }
        /// <summary>
        /// Recupero di una versione cancellata logicamente
        /// </summary>
        /// <param name="TemplateId">ID del relativo Template</param>
        /// <param name="VersionId">ID della versione da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean VersionUnDelete(Int64 VersionId)
        {
            //If HasPermessi && ! Template.HasDefinitive && IsDeleted
            // Undelete
            Boolean Recovered = false;
            if (this.Permission.DeleteTemplates)
            {
                try
                {
                    Manager.BeginTransaction();
                    
                    TemplateVersion vrs = Manager.Get<TemplateVersion>(VersionId);

                    if (vrs != null && vrs.IsDraft && vrs.Deleted != BaseStatusDeleted.None)
                    {
                        //if (vrs.Deleted != BaseStatusDeleted.None)
                        vrs.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                        //vrs.Deleted = BaseStatusDeleted.Cascade;

                        //settings
                        if (vrs.Settings != null && vrs.Settings.Count > 0)
                        {
                            foreach (DocTemplateVers.Settings stg in vrs.Settings)
                            {
                                if (stg.Deleted == BaseStatusDeleted.Cascade)
                                    stg.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                //stg.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }
                        //Signatures
                        if (vrs.Signatures != null && vrs.Signatures.Count > 0)
                        {
                            foreach (DocTemplateVers.Signature sgn in vrs.Signatures)
                            {
                                if (sgn.Deleted == BaseStatusDeleted.Cascade)
                                    sgn.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                //sgn.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }

                        //Elements
                        if (vrs.Elements != null && vrs.Elements.Count > 0)
                        {
                            foreach (DocTemplateVers.PageElement pge in vrs.Elements)
                            {
                                if (pge.Deleted == BaseStatusDeleted.Cascade)
                                    pge.RecoverMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                                //pge.Deleted = BaseStatusDeleted.Cascade;
                            }
                        }

                        vrs.Template.UpdateInfo();

                        Manager.SaveOrUpdate<TemplateVersion>(vrs);
                        Manager.Commit();
                        Recovered = true;
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Recovered = false;
                }
            }
            return Recovered;
        }

    #endregion
#endregion

#region EDIT

    #region Version
        private TemplateVersion VersionGetNew()
        {
            TemplateVersion tv = new TemplateVersion();

            tv.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);

            //Settings
            tv.Settings = new List<DocTemplateVers.Settings>();
            DocTemplateVers.Settings set = this.SettignsGetNew();

            tv.Elements = new List<PageElement>();
            ElementText Body = new ElementText();
            
            tv.HasSignatures = false;
            tv.IsActive = false;
            tv.IsDraft = true;
            //tv.Services
            //tv.Template
            tv.Version = 0;
            tv.SubVersion = 1;

            set.TemplateVersion = tv;

            tv.Settings.Add(set);

            return tv;
        }

        /// <summary>
        /// Recupera i dati di una VERSIONE per il suo EDIT
        /// </summary>
        /// <param name="VersionId">L'Id della versione da caricare. Se = 0 verrà presa la versione in DRAFT del relativo Template.</param>
        /// <param name="TemplateId">L'Id del template a cui appartiene la versione da modificare.</param>
        /// <returns>Tutti i dati della relativa versione</returns>
        public Management.DTO_EditTemplateVersion VersionGetEdit(Int64 TemplateId, Int64 VersionId)
        {
            if (!Permission.EditTemplates)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NoPremission };

            if (VersionId <= 0)
            {
                VersionId = (from TemplateVersion vrs in Manager.Get<Template>(TemplateId).Versions
                                where vrs.IsDraft
                                select vrs.Id).FirstOrDefault();
            }

            if (VersionId <= 0)
            {
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NotVersionForTemplate };
            }
            
            Management.DTO_EditTemplateVersion DTOVer = new Management.DTO_EditTemplateVersion();
            TemplateVersion Source = Manager.Get<TemplateVersion>(VersionId);

            if(Source == null)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NotFound };
            else if(Source.Template == null)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NoTemplate };
            else if(Source.Template.Type == TemplateType.System)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.IsSystem };
            else if(!Permission.EditBuiltInTemplates)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NoPremission };
            else if(!Source.IsDraft)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NotDraft };
            else if (Source.Deleted != BaseStatusDeleted.None)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.IsDeleted };

            //if (Source.Template == null || (Source.Template.Type == TemplateType.System && !Permission.EditBuiltInTemplates))
            //    return null;

            if (Source != null && Source.Deleted == BaseStatusDeleted.None)
            {
                DTOVer.Id = Source.Id;
                DTOVer.IdTemplate = Source.Template.Id;
                DTOVer.TemplateName = Source.Template.Name;
                DTOVer.Permissions = new Management.DTO_EditPermission();
                DTOVer.Permissions.Export = Permission.ViewTemplates;
                DTOVer.Permissions.GetSkin = Permission.ManageTemplates;
                DTOVer.Permissions.List = Permission.ViewTemplates;
                DTOVer.Permissions.ModifyPageElement = Permission.EditTemplates;
                DTOVer.Permissions.ModifySettings = Permission.EditTemplates;
                DTOVer.Permissions.Preview = Permission.ViewTemplates;
                DTOVer.Permissions.SetDefinitive = Permission.ManageTemplates;

                // Header Left
                PageElement HLeft = (from PageElement sEl in Source.Elements
                                    where sEl.Position == ElementPosition.HeaderLeft
                                    && sEl.Deleted == BaseStatusDeleted.None
                                    && sEl.IsActive == true
                                    orderby sEl.Position descending
                                    select sEl).FirstOrDefault();

                DTOVer.HeaderLeft = new Management.DTO_EditItem<PageElement>();

                if (HLeft != null)
                    DTOVer.HeaderLeft.Data = HLeft;
                
                DTOVer.HeaderLeft.Permissions = GetItemPermission();

                // Header Center
                PageElement HCenter = (from PageElement sEl in Source.Elements
                                       where sEl.Position == ElementPosition.HeaderCenter
                                     && sEl.Deleted == BaseStatusDeleted.None
                                     && sEl.IsActive == true
                                     orderby sEl.Position descending
                                     select sEl).FirstOrDefault();

                DTOVer.HeaderCenter = new Management.DTO_EditItem<PageElement>();

                if (HCenter != null)
                    DTOVer.HeaderCenter.Data = HCenter;

                DTOVer.HeaderCenter.Permissions = GetItemPermission();

                // Header Right
                PageElement HRight = (from PageElement sEl in Source.Elements
                                      where sEl.Position == ElementPosition.HeaderRight
                                      && sEl.Deleted == BaseStatusDeleted.None
                                      && sEl.IsActive == true
                                      orderby sEl.Position descending
                                      select sEl).FirstOrDefault();

                DTOVer.HeaderRight = new Management.DTO_EditItem<PageElement>();

                if (HRight != null)
                    DTOVer.HeaderRight.Data = HRight;

                DTOVer.HeaderRight.Permissions = GetItemPermission();

                //BODY
                PageElement Body = (from PageElement sEl in Source.Elements
                                    where sEl.Position == ElementPosition.Body
                                    && sEl.Deleted == BaseStatusDeleted.None
                                    && sEl.IsActive == true
                                    && (sEl.GetType() == typeof(ElementText))
                                    orderby sEl.Position descending
                                    select sEl).FirstOrDefault();

                DTOVer.Body = new Management.DTO_EditItem<ElementText>();

                if (Body != null && Body.GetType() == typeof(ElementText))
                {
                    DTOVer.Body.Data = (ElementText)Body;
                }
                    
                
                DTOVer.Body.Permissions = GetItemPermission();

                // Footer Left
                PageElement FLeft = (from PageElement sEl in Source.Elements
                                     where sEl.Position == ElementPosition.FooterLeft
                                     && sEl.Deleted == BaseStatusDeleted.None
                                     && sEl.IsActive == true
                                     orderby sEl.Position descending
                                     select sEl).FirstOrDefault();

                DTOVer.FooterLeft = new Management.DTO_EditItem<PageElement>();

                if (FLeft != null)
                    DTOVer.FooterLeft.Data = FLeft;

                DTOVer.FooterLeft.Permissions = GetItemPermission();

                // Header Center
                PageElement FCenter = (from PageElement sEl in Source.Elements
                                       where sEl.Position == ElementPosition.FooterCenter
                                     && sEl.Deleted == BaseStatusDeleted.None
                                     && sEl.IsActive == true
                                       orderby sEl.Position descending
                                       select sEl).FirstOrDefault();

                DTOVer.FooterCenter = new Management.DTO_EditItem<PageElement>();

                if (FCenter != null)
                    DTOVer.FooterCenter.Data = FCenter;

                DTOVer.FooterCenter.Permissions = GetItemPermission();

                // Header Right
                PageElement FRight = (from PageElement sEl in Source.Elements
                                      where sEl.Position == ElementPosition.FooterRight
                                      && sEl.Deleted == BaseStatusDeleted.None
                                      && sEl.IsActive == true
                                      orderby sEl.Position descending
                                      select sEl).FirstOrDefault();

                DTOVer.FooterRight = new Management.DTO_EditItem<PageElement>();

                if (FRight != null)
                    DTOVer.FooterRight.Data = FRight;

                DTOVer.FooterRight.Permissions = GetItemPermission();
                
                //altri dati
                //DTOVer.Permissions
                
                //DTOVer.Signatures
                if (Source.Signatures != null) // && Source.Services.Count() > 0)
                {
                    //IList<ServiceContent> sScs = 
                    DTOVer.Signatures = (from Signature sSg in Source.Signatures
                                       where sSg.IsActive == true && sSg.Deleted == BaseStatusDeleted.None
                                       orderby sSg.Position
                                       select new Management.DTO_EditItem<Signature> { Data = sSg, Permissions = GetItemPermission() }
                                       ).ToList();
                }
                else { DTOVer.Signatures = new List<Management.DTO_EditItem<Signature>>(); }

                //DTOVer.Setting
                if (Source.Settings != null) // && Source.Services.Count() > 0)
                {
                    //IList<ServiceContent> sScs = 
                    DTOVer.Setting = (from DocTemplateVers.Settings sSt in Source.Settings
                                         where sSt.IsActive == true && sSt.Deleted == BaseStatusDeleted.None
                                      orderby sSt.SubVersion descending
                                      select new Management.DTO_EditItem<DocTemplateVers.Settings> { Data = sSt, Permissions = GetItemPermission() }
                                       ).FirstOrDefault();
                }
                else {
                    DTOVer.Setting = new Management.DTO_EditItem<DocTemplateVers.Settings>();
                    DTOVer.Setting.Permissions = GetItemPermission();
                }

                //DTOVer.Services
                if (Source.Template.Services != null && Source.Template.Services.Count > 0)
                {
                    DTOVer.Services = (from DocTemplateVers.ServiceContent sct in Source.Template.Services
                                       select new Management.DTO_EditItem<DocTemplateVers.ServiceContent> { Data = sct, Permissions = GetItemPermission() }
                                       ).ToList();
                }
                else
                {
                    DTOVer.Services = null;
                }

            }

            return DTOVer;
        }

        /// <summary>
        /// Recupera i dati di una VERSIONE per il suo EDIT AVANZATO
        /// </summary>
        /// <param name="VersionId">L'Id della versione da caricare</param>
        /// <returns>Tutti i dati della relativa versione</returns>
        /// <remarks>Si basa su VersionGetEdit() per tutti i dati base e vi aggiunge le liste con le versioni precedenti dei vari oggetti.</remarks>
        public Management.DTO_EditTemplateVersion VersionGetEditAdvance(Int64 TemplateId, Int64 VersionId)
        {
            if (!Permission.EditTemplates)
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NoPremission };

            Management.DTO_EditTemplateVersion DTOVer = VersionGetEdit(TemplateId, VersionId);
            if (DTOVer.Error != Management.VersionEditError.none)
                return DTOVer;

            TemplateVersion Source = Manager.Get<TemplateVersion>(VersionId);

            if (Source == null || (Source.Template.Type == TemplateType.System && !Permission.EditBuiltInTemplates))
                return new Management.DTO_EditTemplateVersion { Error = Management.VersionEditError.NoPremission };

            //Header Left
            if (DTOVer.HeaderLeft != null && DTOVer.HeaderLeft.Data != null)
            {
                DTOVer.HeaderLeft.PreviousVersion = (from PageElement etb in Source.Elements
                                                     where etb.Id != DTOVer.HeaderLeft.Data.Id
                                                   && etb.Position == ElementPosition.HeaderLeft
                                                     orderby etb.SubVersion descending
                                                     select new Management.DTO_EditPreviousVersion {  Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.HeaderLeft.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }


            //Header Center
            if (DTOVer.HeaderCenter != null && DTOVer.HeaderCenter.Data != null)
            {
                DTOVer.HeaderCenter.PreviousVersion = (from PageElement etb in Source.Elements
                                                       where etb.Id != DTOVer.HeaderCenter.Data.Id
                                                          && etb.Position == ElementPosition.HeaderCenter
                                                       orderby etb.SubVersion descending
                                                       select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.HeaderCenter.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            //Header Right
            if (DTOVer.HeaderRight != null && DTOVer.HeaderRight.Data != null)
            {
                DTOVer.HeaderRight.PreviousVersion = (from PageElement etb in Source.Elements
                                                      where etb.Id != DTOVer.HeaderRight.Data.Id
                                                         && etb.Position == ElementPosition.HeaderRight
                                                      orderby etb.SubVersion descending
                                                      select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.HeaderRight.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            //Body
            if(DTOVer.Body != null && DTOVer.Body.Data != null)
            {
                DTOVer.Body.PreviousVersion = (from PageElement etb in Source.Elements
                                               where etb.Id != DTOVer.Body.Data.Id
                                                   && etb.Position == ElementPosition.Body
                                               orderby etb.SubVersion descending
                                               select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
                //&& (etb.GetType() == typeof(ElementText))
            }
            else { DTOVer.Body.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            //Footer Left
            if (DTOVer.FooterLeft != null && DTOVer.FooterLeft.Data != null)
            {
                DTOVer.FooterLeft.PreviousVersion = (from PageElement etb in Source.Elements
                                                     where etb.Id != DTOVer.FooterLeft.Data.Id
                                                         && etb.Position == ElementPosition.FooterLeft
                                                     orderby etb.SubVersion descending
                                                     select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.FooterLeft.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            //Header Center
            if (DTOVer.FooterCenter != null && DTOVer.FooterCenter.Data != null)
            {
                DTOVer.FooterCenter.PreviousVersion = (from PageElement etb in Source.Elements
                                                       where etb.Id != DTOVer.FooterCenter.Data.Id
                                                           && etb.Position == ElementPosition.FooterCenter
                                                       orderby etb.SubVersion descending
                                                       select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.FooterCenter.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            //Header Right
            if (DTOVer.FooterRight != null && DTOVer.FooterRight.Data != null)
            {
                DTOVer.FooterRight.PreviousVersion = (from PageElement etb in Source.Elements
                                                      where etb.Id != DTOVer.FooterRight.Data.Id
                                                          && etb.Position == ElementPosition.FooterRight
                                                      orderby etb.SubVersion descending
                                                      select new Management.DTO_EditPreviousVersion { Id = etb.Id, Lastmodify = etb.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.FooterRight.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }

            // Setting
            if (DTOVer.Setting != null && DTOVer.Setting.Data != null && Source.Settings != null)
            {
                DTOVer.Setting.PreviousVersion = (from DocTemplateVers.Settings stt in Source.Settings
                                                  where stt.Id != DTOVer.Setting.Data.Id
                                                  orderby stt.SubVersion descending
                                                  select new Management.DTO_EditPreviousVersion { Id = stt.Id, Lastmodify = stt.ModifiedOn }
                                               ).ToList();
            }
            else { DTOVer.Setting.PreviousVersion = new List<Management.DTO_EditPreviousVersion>(); }



            // Signatures
            if (Source.Signatures != null)  //Ho versioni correnti!
            { 
                if (DTOVer.Signatures != null && DTOVer.Signatures.Count > 0)
                {
                    var PrevSignId = (from Management.DTO_EditItem<Signature> DTEsgn in DTOVer.Signatures select DTEsgn.Data.Id).ToList();

                    //if (PrevSignId != null && PrevSignId.Count() > 0)
                    //{
                    DTOVer.SignaturesPrevious = (from DocTemplateVers.Signature ssg in Source.Signatures
                                        where !PrevSignId.Contains(ssg.Id)
                                        orderby ssg.SubVersion descending
                                        select new Management.DTO_EditPreviousVersion { Id = ssg.Id, Lastmodify = ssg.ModifiedOn }
                                ).ToList();
                    //}
                    //else
                    //{
                    //}
                }
                else    // NON ho versioni correnti: prendo TUTTE le eventuali firme...
                {
                    DTOVer.SignaturesPrevious = (from DocTemplateVers.Signature ssg in Source.Signatures
                                                 orderby ssg.SubVersion descending
                                                 select new Management.DTO_EditPreviousVersion { Id = ssg.Id, Lastmodify = ssg.ModifiedOn }
                                        ).ToList();
                }
            }
            if (DTOVer.Signatures == null)
                DTOVer.Signatures = new List<Management.DTO_EditItem<Signature>>();
            if (DTOVer.SignaturesPrevious == null)
                DTOVer.SignaturesPrevious = new List<Management.DTO_EditPreviousVersion>();

            return DTOVer;
        }
        /// <summary>
        /// Modifica i dati di una versione: ONLY UPDATE!!!!
        /// </summary>
        /// <param name="TemplateVersion">I nuovi dati della versione.</param>
        /// <remarks>
        /// Verranno controllati PERMESSI e LOGICHE di BUSINESS.
        /// VERSIONING degli oggetti contenuti.
        /// </remarks>
        public Boolean VersionUpdate(Management.DTO_EditTemplateVersion Version)
        {
            Boolean Updated = false;
            //Domain.Core.dtoFileCopyBlock dtoFiles = new Domain.Core.dtoFileCopyBlock();


            if (Version != null && Permission.EditTemplates)
            {
                DocTemplateVers.TemplateVersion UpdateVersion = null;


                try
                {
                    Manager.BeginTransaction();


                    if (Version.Id > 0)
                        UpdateVersion = Manager.Get<TemplateVersion>(Version.Id);

                    if (UpdateVersion == null || !UpdateVersion.IsDraft)
                    {
                        Manager.RollBack();
                        return false;
                    }

                    UpdateVersion.UpdateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    UpdateVersion.SubVersion += 1;

                    //Prametri non modificabili
                    //UpdateVersion.IsActive
                    //UpdateVersion.Version
                    //UpdateVersion.Template
                    //UpdateVersion.Template.UpdateInfo();

                    //dtoFiles.OldTemplateId = UpdateVersion.Template.Id;
                    //dtoFiles.OldVersionId = UpdateVersion.Id;

                    //Chek Elements

                    if (Version.HeaderLeft == null)
                        Version.HeaderLeft = new Management.DTO_EditItem<PageElement>();
                    if (Version.HeaderLeft.Data == null)
                        Version.HeaderLeft.Data = new ElementVoid();

                    if (Version.HeaderCenter == null)
                        Version.HeaderCenter = new Management.DTO_EditItem<PageElement>();
                    if (Version.HeaderCenter.Data == null)
                        Version.HeaderCenter.Data = new ElementVoid();

                    if (Version.HeaderRight == null)
                        Version.HeaderRight = new Management.DTO_EditItem<PageElement>();
                    if (Version.HeaderRight.Data == null)
                        Version.HeaderRight.Data = new ElementVoid();

                    if (Version.Body.Data == null)
                    {
                        ElementText BodyEl = new ElementText();
                        BodyEl.IsHTML = true;
                        BodyEl.Text = "";
                        Version.Body.Data = BodyEl;
                    }


                    if (Version.FooterLeft == null)
                        Version.FooterLeft = new Management.DTO_EditItem<PageElement>();
                    if (Version.FooterLeft.Data == null)
                        Version.FooterLeft.Data = new ElementVoid();

                    if (Version.FooterCenter == null)
                        Version.FooterCenter = new Management.DTO_EditItem<PageElement>();
                    if (Version.FooterCenter.Data == null)
                        Version.FooterCenter.Data = new ElementVoid();

                    if (Version.FooterRight == null)
                        Version.FooterRight = new Management.DTO_EditItem<PageElement>();
                    if (Version.FooterRight.Data == null)
                        Version.FooterRight.Data = new ElementVoid();

                    Version.HeaderLeft.Data.Position = ElementPosition.HeaderLeft;
                    Version.HeaderCenter.Data.Position = ElementPosition.HeaderCenter;
                    Version.HeaderRight.Data.Position = ElementPosition.HeaderRight;

                    Version.Body.Data.Position = ElementPosition.Body;

                    Version.FooterLeft.Data.Position = ElementPosition.FooterLeft;
                    Version.FooterCenter.Data.Position = ElementPosition.FooterCenter;
                    Version.FooterRight.Data.Position = ElementPosition.FooterRight;

                    //UpdateVersion.Elements
                    this.PageElementsUpdate(ref UpdateVersion, Version.HeaderLeft.Data);
                    this.PageElementsUpdate(ref UpdateVersion, Version.HeaderCenter.Data);
                    this.PageElementsUpdate(ref UpdateVersion, Version.HeaderRight.Data);
                    if (Version.Body.Data.GetType() == typeof(ElementText))
                        this.PageElementsUpdate(ref UpdateVersion, Version.Body.Data);
                    this.PageElementsUpdate(ref UpdateVersion, Version.FooterLeft.Data);
                    this.PageElementsUpdate(ref UpdateVersion, Version.FooterCenter.Data);
                    this.PageElementsUpdate(ref UpdateVersion, Version.FooterRight.Data);

                    //UpdateVersion.Signatures
                    SignatureUpdate(ref UpdateVersion, Version.Signatures);

                    ////UpdateVersion.Services
                    //ServiceUpdate(ref UpdateVersion, Version.Services);

                    //UpdateVersion.Settings
                    SettingsUpdate(ref UpdateVersion, Version.Setting.Data);

                    Updated = true;

                    Manager.SaveOrUpdate<TemplateVersion>(UpdateVersion);
                    Manager.Commit();
                }
                catch(Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    Updated = false;
                }
            }

            return Updated;
        }

        /// <summary>
        /// Aggiunge una nuova version "standard" al Template.
        /// </summary>
        /// <param name="TemplateId">Il template a cui aggiungerla</param>
        /// <returns>L'ID della nuova versione aggiunta. Minore di ZERO in caso di errori (es: Tempalte ha già DRAFT!)</returns>
        public Int64 VersionAddNew(Int64 TemplateId)
        {
            Int64 IdAdded = 0;

            if(Permission.EditTemplates)
            {
                Manager.BeginTransaction();
                Template Templ = Manager.Get<Template>(TemplateId);

                if(Templ != null && !Templ.HasDraft)
                {
                    Templ.UpdateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    TemplateVersion NewTmplVer = VersionGetNew();
                    NewTmplVer.Template = Templ;

                    if (Templ.Versions == null)
                    {
                        Templ.Versions = new List<TemplateVersion>();
                    }

                    NewTmplVer.Version = (from TemplateVersion ver in Templ.Versions orderby ver.Version descending  
                                          select ver.Version).FirstOrDefault()  + 1;
                    Templ.Versions.Add(NewTmplVer);

                    try
                    {
                        Manager.SaveOrUpdate<DocTemplateVers.Template>(Templ);
                        Manager.Commit();
                        IdAdded = NewTmplVer.Id;
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        IdAdded = -1;
                    }
                    finally
                    {
                        //if(Manager.IsInTransaction())
                        //    Manager.c;
                    }
                }
            }
            return IdAdded;
        }
    #endregion

        //    RIVEDERE TUTTO DA QUI!
        //          + List Template GET!!!

    #region Settings
        /// <summary>
        /// Aggiorna un oggetto PageElement
        /// </summary>
        /// <param name="VersionId">L'ID della relativa Versione</param>
        /// <param name="Element">I dati del nuovo elemento</param>
        /// <returns>
        ///     Indicazione se l'oggetto è stato aggiornato, se l'aggiornamente non era necessario, etc...
        /// </returns>
        /// <remarks>
        ///     Se ELEMENT è nullo, non verrà aggiornato.
        /// </remarks>
        private ItemUpdating SettingsUpdate(ref TemplateVersion Version, DocTemplateVers.Settings Settings)
        {
            if (Settings == null)
                return ItemUpdating.Error;

            ItemUpdating Updated = ItemUpdating.NoPermission;

            if (Permission.EditTemplates)
            {
                DocTemplateVers.Settings PrevSettings = null;
                if (Settings.Id > 0)
                    PrevSettings = Manager.Get<DocTemplateVers.Settings>(Settings.Id);

                if (PrevSettings == null)
                {
                    PrevSettings = (from DocTemplateVers.Settings stt in Version.Settings
                                   where  stt.IsActive == true
                                   && stt.Deleted == BaseStatusDeleted.None
                                    orderby stt.SubVersion descending
                                   select stt
                                       ).FirstOrDefault();
                }

                if (SettingsHasUpdate(PrevSettings, Settings))
                {
                    if (PrevSettings != null)
                    {
                        if (!String.IsNullOrEmpty(Settings.BackgroundImagePath) && Settings.BackgroundImagePath.StartsWith("#"))
                        {
                            Settings.BackgroundImagePath = Settings.BackgroundImagePath.Remove(0, 1);
                            ImageMoveTemp(Settings.BackgroundImagePath, Version.Template.Id, Version.Id);
                        }

                        PrevSettings.IsActive = false;
                        PrevSettings.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                        PrevSettings.Deleted = BaseStatusDeleted.Automatic;
                        //Settings.SubVersion = Version.SubVersion;//PrevSettings.SubVersion + 1;
                        Updated = ItemUpdating.Updated;
                    }
                    else
                    {
                        Updated = ItemUpdating.Added;
                        
                    }
                    Settings.SubVersion = Version.SubVersion;

                    //// - TEMPORANEO, finchè non sarà deciso COME gestire tali dati...
                    //if(string.IsNullOrEmpty(Settings.Title))
                    //    Settings.Title = PrevSettings.Title;

                    //if (string.IsNullOrEmpty(Settings.Subject))
                    //    Settings.Subject = PrevSettings.Subject;

                    //if (string.IsNullOrEmpty(Settings.Author))
                    //    Settings.Author = PrevSettings.Author;

                    //if (string.IsNullOrEmpty(Settings.Creator))
                    //    Settings.Creator = PrevSettings.Creator;

                    //if (string.IsNullOrEmpty(Settings.Producer))
                    //    Settings.Producer = PrevSettings.Producer;

                    //if (string.IsNullOrEmpty(Settings.Keywords))
                    //    Settings.Keywords = PrevSettings.Keywords;


                    try
                    {
                        //Manager.Detach<PageElement>(Element);
                        Settings.Deleted = BaseStatusDeleted.None;
                        Settings.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                        Settings.IsActive = true;
                        Settings.TemplateVersion = Version;
                        Version.Settings.Add(Settings);

                        return ItemUpdating.Updated;
                        //Manager.SaveOrUpdate<PageElement>(Element);
                    }
                    catch
                    {
                        return ItemUpdating.Error;
                    }

                }
                else
                {
                    return ItemUpdating.NotNecessary;
                }

            }

            return Updated;
        }
        /// <summary>
        /// Controlla se i dati inviati corrispondono ai precedenti.
        /// </summary>
        /// <param name="SourceElement">Elemento sorgente</param>
        /// <param name="NewElement">Nuovo elemento</param>
        /// <returns>True se vengono rilevate differenze tra i due oggetti ed è necessario aggiornare l'oggetto.</returns>
        private Boolean SettingsHasUpdate(DocTemplateVers.Settings SourceSettings, DocTemplateVers.Settings NewSettings)
        {
            //Boolean IsEqual = false;
            if (SourceSettings == null && NewSettings == null)
                return false;
            if (SourceSettings == null)
                return true;
            if (NewSettings == null)
                return true;
            //if (SourceSettings.Id != NewSettings.Id)
            //    return true;
            if (SourceSettings.Author != NewSettings.Author)
                return true;
            if (SourceSettings.BackgroundAlpha != NewSettings.BackgroundAlpha)
                return true;
            if (SourceSettings.BackgroundBlue != NewSettings.BackgroundBlue)
                return true;
            if (SourceSettings.BackgroundGreen != NewSettings.BackgroundGreen)
                return true;
            if (SourceSettings.BackGroundImageFormat != NewSettings.BackGroundImageFormat)
                return true;
            if (SourceSettings.BackgroundImagePath != NewSettings.BackgroundImagePath)
                return true;
            if (SourceSettings.BackgroundRed != NewSettings.BackgroundRed)
                return true;
            //if (SourceSettings.HasHeaderOnFirstPage != NewSettings.HasHeaderOnFirstPage)
            //    return true;
            if (SourceSettings.Height != NewSettings.Height)
                return true;
            //if (SourceSettings.IsActive != NewSettings.IsActive)
            //    return true;
            if (SourceSettings.Keywords != NewSettings.Keywords)
                return true;
            if (SourceSettings.MarginBottom != NewSettings.MarginBottom)
                return true;
            if (SourceSettings.MarginLeft != NewSettings.MarginLeft)
                return true;
            if (SourceSettings.MarginRight != NewSettings.MarginRight)
                return true;
            if (SourceSettings.MarginTop != NewSettings.MarginTop)
                return true;
            //if (SourceSettings.PageNumberAlignment != NewSettings.PageNumberAlignment)
            //    return true;
            if (SourceSettings.Producer != NewSettings.Producer)
                return true;
            //if (SourceSettings.ShowPageNumber != NewSettings.ShowPageNumber)
            //    return true;
            if (SourceSettings.Size != NewSettings.Size)
                return true;
            if (SourceSettings.Subject != NewSettings.Subject)
                return true;
            if (SourceSettings.Title != NewSettings.Title)
                return true;
            if (SourceSettings.Width != NewSettings.Width)
                return true;
            if (SourceSettings.PagePlacingMask != NewSettings.PagePlacingMask)
                return true;
            if (SourceSettings.PagePlacingRange != NewSettings.PagePlacingRange)
                return true;


            return false;
        }

        /// <summary>
        /// Cancellazione FISICA di un VECCHIO setting
        /// </summary>
        /// <param name="VersionId">ID della relativa Versione</param>
        /// <param name="SettingId">ID dei settings da cancellare</param>
        public Boolean SettingDeletePhisical(Int64 SettingId)
        {
            Boolean IsDeleted = false;
            if (Permission.EditTemplates || Permission.DeleteTemplates)
            {
                DocTemplateVers.Settings Settings = Manager.Get<DocTemplateVers.Settings>(SettingId);
                if (Settings.IsActive == false && Settings.Deleted != BaseStatusDeleted.None)
                {
                    if (!String.IsNullOrEmpty(Settings.BackgroundImagePath))
                    {
                        ImageDelete(Settings.BackgroundImagePath, Settings.TemplateVersion.Template.Id, Settings.TemplateVersion.Id);
                    }

                    try
                    {
                        Manager.BeginTransaction();
                        Manager.DeletePhysical<DocTemplateVers.Settings>(Settings);
                        IsDeleted = true;
                        Manager.Commit();                    
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                }
            }
            return IsDeleted;
        }
        /// <summary>
        /// Recupera un setting precedente
        /// </summary>
        /// <param name="VersionId">La relativa versione</param>
        /// <param name="SettingId">L'ID del settings da recuperare</param>
        /// <remarks>
        /// Verranno controllati PERMESSI e LOGICHE di BUSINESS
        /// VERSIONING
        /// </remarks>
        public Int64 SettingRecoverSave(Int64 SettingId)
        {
            Int64 NewId = -1;

            if (Permission.EditTemplates)
            {
                Manager.BeginTransaction();

                try
                {
                    //Se ho permessi && Setting != CurrentSettings
                    DocTemplateVers.Settings OldSetting = Manager.Get<DocTemplateVers.Settings>(SettingId);
                    DocTemplateVers.Settings NewSetting = new DocTemplateVers.Settings();

                    DocTemplateVers.Settings CurrentSettings = null;
                    TemplateVersion CurrentVersion = null;

                    if (OldSetting != null)
                    {
                        CurrentVersion = OldSetting.TemplateVersion;
                    }

                    if (CurrentVersion == null)
                        return - 2;

                    CurrentSettings = (from DocTemplateVers.Settings st in CurrentVersion.Settings.Where(st =>
                                          (st.Deleted == BaseStatusDeleted.None && st.IsActive == true))
                                       orderby st.SubVersion descending
                                       select st
                                          ).FirstOrDefault();

                    if (CurrentSettings != null)
                    {
                        CurrentSettings.SetDeleteMetaInfo(CurrentUser, UC.IpAddress, UC.ProxyIpAddress);
                        CurrentSettings.Deleted = BaseStatusDeleted.Automatic;
                        //NewSetting.SubVersion = OldSetting.SubVersion + 1;
                    }
                    else
                    {
                        //NewSetting.SubVersion = CurrentVersion.Settings.Count() + 1;
                    }

                    CurrentVersion.SubVersion += 1;

                    //NewSetting.TemplateVersion.SubVersion += 1;


                    NewSetting.Author = OldSetting.Author;
                    NewSetting.BackgroundAlpha = OldSetting.BackgroundAlpha;
                    NewSetting.BackgroundBlue = OldSetting.BackgroundBlue;
                    NewSetting.BackgroundGreen = OldSetting.BackgroundGreen;
                    NewSetting.BackGroundImageFormat = OldSetting.BackGroundImageFormat;
                    NewSetting.BackgroundImagePath = OldSetting.BackgroundImagePath;
                    NewSetting.BackgroundRed = OldSetting.BackgroundRed;
                    //NewSetting.HasHeaderOnFirstPage = OldSetting.HasHeaderOnFirstPage;
                    NewSetting.Height = OldSetting.Height;
                    NewSetting.IsActive = true;
                    NewSetting.Keywords = OldSetting.Keywords;
                    NewSetting.MarginBottom = OldSetting.MarginBottom;
                    NewSetting.MarginLeft = OldSetting.MarginLeft;
                    NewSetting.MarginRight = OldSetting.MarginRight;
                    NewSetting.MarginTop = OldSetting.MarginTop;
                    //NewSetting.PageNumberAlignment = OldSetting.PageNumberAlignment;
                    NewSetting.Producer = OldSetting.Producer;
                    //NewSetting.ShowPageNumber = OldSetting.ShowPageNumber;
                    NewSetting.Size = OldSetting.Size;
                    NewSetting.Subject = OldSetting.Subject;
                    NewSetting.SubVersion = CurrentVersion.SubVersion;

                    NewSetting.PagePlacingMask = OldSetting.PagePlacingMask;
                    NewSetting.PagePlacingRange = OldSetting.PagePlacingRange;

                    NewSetting.Title = OldSetting.Title;

                    NewSetting.TemplateVersion = CurrentVersion;


                    CurrentSettings.IsActive = false;

                    Manager.SaveOrUpdate<DocTemplateVers.Settings>(NewSetting);
                    Manager.SaveOrUpdate<DocTemplateVers.Settings>(CurrentSettings);

                    NewId = NewSetting.Id;

                    CurrentVersion.Settings.Add(NewSetting);
                    Manager.Commit();
                }
                catch
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
                

            }

            return NewId;
        }

        /// <summary>
        /// Recupera i dati precedentemente salvati
        /// </summary>
        /// <param name="SettingId">L'id dei relativi dati</param>
        /// <returns></returns>
        public Management.DTO_EditItem<Settings> SettingsGetRecover(Int64 SettingId, Boolean WithPrevVersion, Int64 TemplateId, Int64 VersionId)
        {


            if (SettingId <= 0 || !Permission.EditTemplates)
                return null; // new Management.DTO_EditItem<Settings> { Error = Management.VersionEditError.NoPremission };

            Settings source = Manager.Get<Settings>(SettingId);
            Manager.Detach<Settings>(source); //Per sicurezza...

            if (source == null)
                return null;

            Management.DTO_EditItem<Settings> RecoveredSettings = new Management.DTO_EditItem<Settings>();

            if (!string.IsNullOrEmpty(source.BackgroundImagePath))
            {
                source.BackgroundImagePath = this.RecoverCopy(source.BackgroundImagePath, TemplateId, VersionId);
            }
            
            if (WithPrevVersion == true)
            {
                RecoveredSettings.PreviousVersion = (from Settings sst in Manager.GetIQ<Settings>()
                                                        where sst.Id != SettingId
                                                        && sst.TemplateVersion.Id == source.TemplateVersion.Id
                                                        && sst.IsActive == false
                                                        orderby sst.SubVersion descending
                                                        select new Management.DTO_EditPreviousVersion { Id = sst.Id, Lastmodify = sst.ModifiedOn }
                                ).ToList();
            }
            else
            {
                RecoveredSettings.PreviousVersion = null;
            }
                
            //RecoveredSettings.Permissions = Permission;

            RecoveredSettings.Data = source;
            return RecoveredSettings;
        }
        ///// <summary>
        ///// Recupera un elenco dei precedenti Settings (Editor Avanzato)
        ///// </summary>
        ///// <param name="VersionId">Id della relativa versione</param>
        ///// <returns>Lista dei settings precedenti</returns>
        ///// <remarks>
        ///// Verranno controllati PERMESSI e LOGICHE di BUSINESS
        ///// VERSIONING
        ///// </remarks>
        //public IList<Management.DTO_EditItem<Settings>> SettingsGetOld(Int64 VersionId)
        //{
        //    return null;
        //}

        /// <summary>
        /// Crea un nuovo Setting
        /// </summary>
        /// <returns>Un settings con i parametri standard</returns>
        /// <remarks>
        /// Eventualmente recuperare/usare i settings in configurazione...
        /// </remarks>
        private DocTemplateVers.Settings SettignsGetNew()
        {
            DocTemplateVers.Settings sett = new DocTemplateVers.Settings();
            sett.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);

            sett.Author = "Autor";
            sett.BackgroundAlpha = 0;
            sett.BackgroundBlue = 0;
            sett.BackgroundGreen = 0;
            sett.BackgroundRed = 0;
            sett.BackGroundImageFormat = DocTemplateVers.BackgrounImagePosition.Center;
            sett.BackgroundImagePath = "";
            //sett.HasHeaderOnFirstPage = true;
            sett.Height = 321;
            sett.IsActive = true;
            sett.Keywords = "keyword";
            sett.MarginBottom = 5;
            sett.MarginLeft = 5;
            sett.MarginRight = 5;
            sett.MarginTop = 5;
            //sett.PageNumberAlignment = DocTemplateVers.ElementAlignment.MiddleCenter;
            sett.Producer = "Producer";
            //sett.ShowPageNumber = false;
            sett.Size = DocTemplateVers.PageSize.A4;
            sett.Subject = "Subject";
            sett.Title = "Title";
            sett.SubVersion = 1;
            sett.Width = 0;

            sett.PagePlacingMask = 1;
            sett.PagePlacingRange = "";

            return sett;
        }
    #endregion
    #region PageElements
        /// <summary>
        /// Aggiorna un oggetto PageElement
        /// </summary>
        /// <param name="VersionId">L'ID della relativa Versione</param>
        /// <param name="Element">I dati del nuovo elemento</param>
        /// <returns>
        ///     Indicazione se l'oggetto è stato aggiornato, se l'aggiornamente non era necessario, etc...
        /// </returns>
        /// <remarks>
        ///     Se ELEMENT è nullo, non verrà aggiornato.
        /// </remarks>
        private ItemUpdating PageElementsUpdate(ref TemplateVersion Version, PageElement Element)
        {
            if (Element == null)
                return ItemUpdating.Error;
            
            ItemUpdating Updated = ItemUpdating.NoPermission;
            
            if (Permission.EditTemplates)
            {

                //Le immagini temporanee iniziano per #.
                //Necessario SPOSTARLE nella cartella corretta, ELIMINANDO il file temporaneo!
                if (Element.GetType() == typeof(ElementImage))
                {
                    ElementImage img = (ElementImage)Element;

                    if (img.Path.StartsWith("#"))
                    {
                        img.Path = img.Path.Remove(0, 1);
                        ImageMoveTemp(img.Path, Version.Template.Id, Version.Id);
                    }

                    Element = img;
                }

                PageElement PrevElement = null;
                if(Element.Id > 0)
                    PrevElement = Manager.Get<PageElement>(Element.Id);

                if (PrevElement == null)
                {
                    PrevElement = (from PageElement pge in Version.Elements
                                   where pge.Position == Element.Position
                                   && pge.IsActive == true
                                   && pge.Deleted == BaseStatusDeleted.None
                                   orderby pge.SubVersion descending
                                   select pge
                                       ).FirstOrDefault();
                }

                
                if (PageElementHasUpdate(PrevElement, Element))
                {
                    if (Element == null)
                    {
                        Element = new ElementVoid();
                    }

                    if (PrevElement != null)
                    {
 
                        PrevElement.IsActive = false;
                        PrevElement.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                        PrevElement.Deleted = BaseStatusDeleted.Automatic;
                        Updated = ItemUpdating.Updated;
                    }
                    else
                    {
                        Updated = ItemUpdating.Added;
                        //Element.SubVersion = 0;
                    }

                    Element.SubVersion = Version.SubVersion;//PrevElement.SubVersion + 1;

                    try
                    {
                        //Manager.Detach<PageElement>(Element);
                        Element.Deleted = BaseStatusDeleted.None;
                        Element.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                        Element.IsActive = true;
                        Element.TemplateVersion = Version;
                        Version.Elements.Add(Element);

                        return ItemUpdating.Updated;
                        //Manager.SaveOrUpdate<PageElement>(Element);
                    }
                    catch
                    {
                        return ItemUpdating.Error;
                    }

                }
                else
                {
                    return ItemUpdating.NotNecessary;
                }

            }

            return Updated;
        }
        /// <summary>
        /// Controlla se i dati inviati corrispondono ai precedenti.
        /// </summary>
        /// <param name="SourceElement">Elemento sorgente</param>
        /// <param name="NewElement">Nuovo elemento</param>
        /// <returns>True se vengono rilevate differenze tra i due oggetti ed è necessario aggiornare l'oggetto.</returns>
        private Boolean PageElementHasUpdate(PageElement SourceElement, PageElement NewElement)
        {
            //Boolean IsEqual = false;
            if (SourceElement == null && NewElement == null)
                return false;

            if (SourceElement == null)
                return true;

            if (NewElement == null)
                return true;

            //if (SourceElement.Id != NewElement.Id)
            //    return true;

            if (SourceElement.Alignment != NewElement.Alignment)
                return true;

            if (SourceElement.Position != NewElement.Position)
                return true;

            if (SourceElement.GetType() != NewElement.GetType())
                return true;

            if(SourceElement.GetType() == typeof(ElementText))
            {
                try
                {
                    ElementText SourceEl = (ElementText)SourceElement;
                    ElementText NewEl = (ElementText)NewElement;

                    if (SourceEl.IsHTML != NewEl.IsHTML)
                        return true;

                    if (SourceEl.Text != NewEl.Text)
                        return true;
                }
                catch { return true; }
            } else if(SourceElement.GetType() == typeof(ElementImage))
            {
                ElementImage SourceEl = (ElementImage)SourceElement;
                ElementImage NewEl = (ElementImage)NewElement;

                if (SourceEl.Path != NewEl.Path)
                    return true;
                if (SourceEl.Height != NewEl.Height)
                    return true;
                if (SourceEl.Width != NewEl.Width)
                    return true;
            }

            return false;
        }

        ///// <summary>
        ///// Aggiorna un oggetto PageElement
        ///// </summary>
        ///// <param name="VersionId">L'ID della relativa Versione</param>
        ///// <param name="OldSettingId">L'ID delle impostazioni precedenti</param>
        ///// <param name="Element">I dati del nuovo elemento</param>
        ///// <remarks>
        ///// Verranno controllati PERMESSI e LOGICHE di BUSINESS
        ///// VERSIONING
        ///// </remarks>
        //public void PageElementsUpdate_(Int64 VersionId, Int64 OldSettingId, PageElement Element)
        //{
        //    //Metto in "OLD" l'attuale Element relativo a quella posizione (Se presente)

        //    //Associo i nuovi alla "Version" corrente

        //    //Salvo i nuovi settings
        //}
        /// <summary>
        /// Cancellazione FISICA di un elmento di Pagina
        /// </summary>
        /// <param name="VersionId">Id della relativa Versione</param>
        /// <param name="ElementId">Id dell'elemento da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean PageElementsDeletePhisical(Int64 TemplateId, Int64 VersionId, Int64 ElementId)
        {
            Boolean IsDeleted = false;
            if (Permission.EditTemplates || Permission.DeleteTemplates)
            {
                DocTemplateVers.PageElement PageEl = Manager.Get<DocTemplateVers.PageElement>(ElementId);
                if (PageEl.IsActive == false && PageEl.Deleted != BaseStatusDeleted.None)
                {
                    if(PageEl.GetType() == typeof(DocTemplateVers.ElementImage))
                    {
                        DocTemplateVers.ElementImage Img = (DocTemplateVers.ElementImage) PageEl;
                        ImageDelete(Img.Path, TemplateId, VersionId);
                    }
                    try
                    {
                        Manager.BeginTransaction();
                        Manager.DeletePhysical<DocTemplateVers.PageElement>(PageEl);
                        IsDeleted = true;
                        Manager.Commit();
                        IsDeleted = true;
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        IsDeleted = false;
                    }
                }
            }
            return IsDeleted;
        }
        /// <summary>
        /// Recupera un dato elemento e lo imposta come corrente
        /// </summary>
        /// <param name="VersionId">Id della relativa verisone</param>
        /// <param name="ElementId">Id dell'elemento da recuperare</param>
        /// <remarks>
        /// Verranno controllati PERMESSI e LOGICHE di BUSINESS
        /// VERSIONING
        /// </remarks>
        public Int64 PageElementsRecoverSave(Int64 ElementId)
        {
            Int64 NewId = 0;
            //Se ho permessi && Setting != CurrentSettings

            Manager.BeginTransaction();

            PageElement OldElement = Manager.Get<PageElement>(ElementId);
            PageElement NewElement = null;

            if (OldElement == null)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();

                return -2;
            }

            Domain.Core.dtoFileCopyBlock dtoFilecopy = new Domain.Core.dtoFileCopyBlock();

            if (OldElement.GetType() == typeof(ElementText))
            {
                ElementText _OldElement = (ElementText)OldElement;
                ElementText _NewElement = new ElementText();
                _NewElement.Text = _OldElement.Text;
                _NewElement.IsHTML = _OldElement.IsHTML;
                NewElement = _NewElement;
            }
            else if (OldElement.GetType() == typeof(ElementImage))
            {
                ElementImage _OldElement = (ElementImage)OldElement;
                ElementImage _NewElement = new ElementImage();
                _NewElement.Height = _OldElement.Height;
                _NewElement.Path = dtoFilecopy.AddFile(_OldElement.Path); // ImageCopy(_OldElement.Path);
                _NewElement.Width = _OldElement.Width;
                NewElement = _NewElement;
            }

            PageElement CurrentElement = (from PageElement pge in OldElement.TemplateVersion.Elements
                                          where pge.IsActive == true
                                          && pge.Deleted == BaseStatusDeleted.None
                                          && pge.Position == OldElement.Position
                                          orderby pge.SubVersion descending
                                          select pge).FirstOrDefault();

            CurrentElement.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
            CurrentElement.IsActive = false;
            CurrentElement.Deleted = BaseStatusDeleted.Automatic;

            NewElement.TemplateVersion = OldElement.TemplateVersion;
            NewElement.TemplateVersion.SubVersion += 1;
            NewElement.SubVersion = NewElement.TemplateVersion.SubVersion; //CurrentElement.Version + 1;

            NewElement.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
            NewElement.Alignment = OldElement.Alignment;
            NewElement.IsActive = true;
            NewElement.Position = OldElement.Position;
            //NewElement.TemplateVersion = OldElement.TemplateVersion;

            OldElement.TemplateVersion.Elements.Add(NewElement);

            
            dtoFilecopy.OldTemplateId = OldElement.TemplateVersion.Template.Id;
            dtoFilecopy.NewTemplateId = dtoFilecopy.OldTemplateId;
            dtoFilecopy.OldVersionId = OldElement.TemplateVersion.Id;
            dtoFilecopy.NewVersionId = dtoFilecopy.OldVersionId;

            ImageCopyBlock(dtoFilecopy);
            //if (ImageCopyBlock(dtoFilecopy) > 0)
            //    //...
            
            try
            {
                Manager.SaveOrUpdate<PageElement>(NewElement);
                NewId = NewElement.Id;
            }
            catch
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();

                NewId = -1;
            }
            
            return NewId;
        }


        /// <summary>
        /// Recupera i dati precedentemente salvati
        /// </summary>
        /// <param name="SettingId">L'id dei relativi dati</param>
        /// <returns></returns>
        public Management.DTO_EditItem<PageElement> PageElementGetRecover(Int64 PgEId, Boolean WithPrevVersion, Int64 TmplId, Int64 VrsId)
        {


            if (PgEId <= 0 || !Permission.EditTemplates)
                return null; // new Management.DTO_EditItem<Settings> { Error = Management.VersionEditError.NoPremission };

            PageElement source = Manager.Get<PageElement>(PgEId);
            Manager.Detach<PageElement>(source);  //Per sicurezza...

            if (source == null)
                return null;

            Management.DTO_EditItem<PageElement> RecoveredSettings = new Management.DTO_EditItem<PageElement>();


            //Le immagini temporanee iniziano per #.
            //Necessario SPOSTARLE nella cartella corretta, ELIMINANDO il file temporaneo!
            if (source.GetType() == typeof(ElementImage))
            {
                ElementImage img = (ElementImage)source;
                img.Path = RecoverCopy(img.Path, TmplId, VrsId);

                source = img;
            }

            if (WithPrevVersion == true)
            {
                RecoveredSettings.PreviousVersion = (from Settings pge in Manager.GetIQ<Settings>()
                                                     where pge.Id != PgEId
                                                     && pge.TemplateVersion.Id == source.TemplateVersion.Id
                                                     && pge.IsActive == false
                                                     orderby pge.SubVersion descending
                                                     select new Management.DTO_EditPreviousVersion { Id = pge.Id, Lastmodify = pge.ModifiedOn }
                                ).ToList();
            }
            else
            {
                RecoveredSettings.PreviousVersion = null;
            }

            //RecoveredSettings.Permissions = Permission;

            RecoveredSettings.Data = source;
            return RecoveredSettings;
        }
    #endregion
    #region Signatures
        private ItemUpdating SignatureUpdate(ref TemplateVersion Version, IList<Management.DTO_EditItem<Signature>> DTO_Signatures)
        {
            if (DTO_Signatures == null)
                DTO_Signatures = new List<Management.DTO_EditItem<Signature>>();

            if (Version == null)
                return ItemUpdating.Error;
            
            IList<Signature> OldSignatures;
            if (Version.Signatures != null)
                OldSignatures = Version.Signatures.Where(sg => (sg.Deleted == BaseStatusDeleted.None && sg.IsActive == true)).ToList();
            else
            {
                Version.Signatures = new List<Signature>();
                OldSignatures = new List<Signature>();
            }
                

            IList<Int64> RemovedSignaturesId = new List<Int64>();

            

            //SE DTO_Signatures non contiene Signature, sono TUTTE da "cancellare"!
            if (DTO_Signatures.Count() <= 0) //DTO_Signatures == null || 
            {
                foreach (Signature OldSgn in OldSignatures)
                {
                    RemovedSignaturesId.Add(OldSgn.Id);
                }
            }
            else { 
                //ALTRIMENTI

                // ADD PRE-FERIE:
                // - Da cancellare:
                //      Tutti quelli presenti in Old_Sgn, ma non in New_Sgn
                // - Da aggiungere:
                //      Tutti quelli presenti in New_Sgn, ma non in Old_Sgn (ZERO!)
                //      Tutti quelli in New_Sgn con un NUOVO ID (-1)
                // - Da MODIFICARE:
                //      Tutti gli ID presenti sia in Old_Sgn che in New_Sgn, che hanno ANCHE "HasUpdate"
                // - Inalterati:
                //      Tutti gli ID presenti sia in New_Sgn che in Old_Sgn, che NON hanno "HasUpadate"

                foreach (Management.DTO_EditItem<Signature> NewSgn in DTO_Signatures)
                {
                    //Per oggetti in liste, la "version" è totalmente inutile,
                    //lasciata a fini di debug.
                    NewSgn.Data.SubVersion = 1;

                    Boolean IsToAdd = true;

                    if (NewSgn.Data.Id > 0)
                    {
                        foreach (Signature OldSgn in OldSignatures)
                        {
                            //SE il DTO in arrivo è nella lista dei precedenti
                            if (OldSgn.Id == NewSgn.Data.Id)
                            {
                                IsToAdd = false;
                                //Controllo se necessita aggiornamenti, ed "aggiorno"
                                if (SignatureNeedUpdate(OldSgn, NewSgn.Data))
                                {
                                    //Controllo se l'immagine è "temporanea"
                                    if (NewSgn.Data.HasImage && !String.IsNullOrEmpty(NewSgn.Data.Path) && NewSgn.Data.Path.StartsWith("#"))
                                    {
                                        NewSgn.Data.Path = NewSgn.Data.Path.Remove(0, 1);
                                        ImageMoveTemp(NewSgn.Data.Path, Version.Template.Id, Version.Id);
                                    }

                                    //Imposto la vecchia Signature come "da cancellare"
                                    OldSgn.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                                    OldSgn.Deleted = BaseStatusDeleted.Automatic;
                                    OldSgn.IsActive = false;
                                    NewSgn.Data.SubVersion = Version.SubVersion; // OldSgn.Version + 1;
                                    //Aggiungo l'ID della signature sorgente nella liste di quelle da cancellare
                                    RemovedSignaturesId.Add(OldSgn.Id);

                                    IsToAdd = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Controllo se l'immagine è "temporanea"
                        if (NewSgn.Data.HasImage && !String.IsNullOrEmpty(NewSgn.Data.Path) && NewSgn.Data.Path.StartsWith("#"))
                        {
                            NewSgn.Data.Path = NewSgn.Data.Path.Remove(0, 1);
                            ImageMoveTemp(NewSgn.Data.Path, Version.Template.Id, Version.Id);
                        }
                    }

                    if (IsToAdd)
                    {
                        NewSgn.Data.Id = 0;
                        NewSgn.Data.IsActive = true;
                        NewSgn.Data.Deleted = BaseStatusDeleted.None;
                        NewSgn.Data.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                        NewSgn.Data.TemplateVersion = Version;
                        NewSgn.Data.SubVersion = Version.SubVersion;
                        Version.Signatures.Add(NewSgn.Data);
                    }

                }
            }

            //ID attivi su dB
            IList<Int64> OldSignIds = (from Signature sgn in OldSignatures select sgn.Id).ToList();
            //ID che dovranno essere attivati
            IList<Int64> NewSignIds = (from Management.DTO_EditItem<Signature> dto_sgn in DTO_Signatures select dto_sgn.Data.Id).ToList();
            //Cerco gli ID che dovranno essere cancellati
            foreach(Int64 oldId in OldSignIds)
            {
                if (!NewSignIds.Contains(oldId) && !RemovedSignaturesId.Contains(oldId))
                {
                    RemovedSignaturesId.Add(oldId);
                }
            }


            foreach (Signature remsgn in Version.Signatures.Where(
                sg => (sg.Id > 0
                    && sg.Deleted == BaseStatusDeleted.None
                    && sg.IsActive == true
                    && RemovedSignaturesId.Contains(sg.Id))))   //C'era un "not" prima di rem...countains... TOLTO!
            {
                remsgn.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                remsgn.Deleted = BaseStatusDeleted.Automatic;
                remsgn.IsActive = false;
            }
            Version.HasSignatures = (Version.Signatures.Where(sg => (sg.IsActive == true && sg.Deleted == BaseStatusDeleted.None)).Count() > 0);
            
            return ItemUpdating.Updated;
        }

        private Boolean SignatureNeedUpdate(Signature OldSign, Signature NewSign)
        {

            if(OldSign.HasImage != NewSign.HasImage)
                return true;
            if(OldSign.HasPDFPositioning != NewSign.HasPDFPositioning)
                return true;
            if (OldSign.Height != NewSign.Height)
                return true;
            if (OldSign.IsHTML != NewSign.IsHTML)
                return true;
            if(OldSign.Path != NewSign.Path)
                return true;
            if(OldSign.PosBottom != NewSign.PosBottom)
                return true;
            if(OldSign.Position != NewSign.Position)
                return true;
            if(OldSign.PosLeft != NewSign.PosLeft)
                return true;
            if(OldSign.Text != NewSign.Text)
                return true;
            if(OldSign.Width != NewSign.Width)
                return true;
            if (OldSign.PagePlacingMask != NewSign.PagePlacingMask)
                return true;
            if(OldSign.PagePlacingRange != NewSign.PagePlacingRange)
                return true;


            return false;
        }

        /// <summary>
        /// Cancellazione FISICA di una firma
        /// </summary>
        /// <param name="VersionId">Id della relativa Versione</param>
        /// <param name="ElementId">Id dell'elemento da cancellare</param>
        /// <remarks>Verranno controllati PERMESSI e LOGICHE di BUSINESS</remarks>
        public Boolean SignatureDeletePhisical(Int64 TemplateId, Int64 VersionId, Int64 SignatureId)
        {
            Boolean IsDeleted = false;
            if (Permission.EditTemplates || Permission.DeleteTemplates)
            {
                Signature Sign = Manager.Get<Signature>(SignatureId);

                if (Sign.IsActive == false && Sign.Deleted != BaseStatusDeleted.None)
                {

                    if (Sign.HasImage)
                    { ImageDelete(Sign.Path, TemplateId, VersionId); }

                    try
                    {
                        Manager.BeginTransaction();
                        Manager.DeletePhysical<Signature>(Sign);
                        IsDeleted = true;
                        Manager.Commit();
                        IsDeleted = true;
                    }
                    catch (Exception ex)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        IsDeleted = false;
                    }
                }
            }
            return IsDeleted;
        }
        /// <summary>
        /// Recupera un dato elemento e lo attiva
        /// </summary>
        /// <param name="VersionId">Id della relativa verisone</param>
        /// <param name="ElementId">Id dell'elemento da recuperare</param>
        /// <remarks>
        /// Verranno controllati PERMESSI e LOGICHE di BUSINESS
        /// VERSIONING
        /// </remarks>
        public Int64 SignatureRecoverSave(Int64 SignatureId)
        {
            Int64 NewId = 0;
            if (Permission.EditTemplates)
            {
                Domain.Core.dtoFileCopyBlock dtoFiles = new Domain.Core.dtoFileCopyBlock();

                Manager.BeginTransaction();

                Signature OldSign = Manager.Get<Signature>(SignatureId);
                Signature NewSign = new Signature();

                OldSign.TemplateVersion.SubVersion += 1;

                NewSign.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                NewSign.HasImage = OldSign.HasImage;
                NewSign.HasPDFPositioning = OldSign.HasPDFPositioning;
                NewSign.Height = OldSign.Height;
                NewSign.IsActive = true;
                NewSign.IsHTML = OldSign.IsHTML;
                NewSign.Path = dtoFiles.AddFile(OldSign.Path); //this.ImageCopy(OldSign.Path);
                NewSign.PosBottom = OldSign.PosBottom;
                NewSign.Position = OldSign.Position;
                NewSign.PosLeft = OldSign.PosLeft;
                NewSign.TemplateVersion = OldSign.TemplateVersion;
                NewSign.Text = OldSign.Text;
                NewSign.SubVersion = OldSign.TemplateVersion.SubVersion;//OldSign.Version + 1;
                NewSign.Width = OldSign.Width;
                OldSign.TemplateVersion.Signatures.Add(NewSign);

                dtoFiles.OldTemplateId = OldSign.TemplateVersion.Template.Id;
                dtoFiles.NewTemplateId = dtoFiles.OldTemplateId;
                dtoFiles.OldVersionId = OldSign.TemplateVersion.Id;
                dtoFiles.NewVersionId = dtoFiles.OldVersionId;

                ImageCopyBlock(dtoFiles);

                try
                {
                    Manager.SaveOrUpdate<Signature>(NewSign);
                    NewId = NewSign.Id;
                }
                catch
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();

                    NewId = -1;
                }
            }

            return NewId;
        }

        public Management.DTO_EditItem<Signature> SignatureGetRecover(Int64 SignatureId, Int64 TemplateId, Int64 VersionId)//, Boolean WithPrevVersion)
        {

            if (SignatureId <= 0 || !Permission.EditTemplates)
                return null; // new Management.DTO_EditItem<Settings> { Error = Management.VersionEditError.NoPremission };

            Signature source = Manager.Get<Signature>(SignatureId);
            Manager.Detach<Signature>(source);  //Per sicurezza...

            if (source == null)
                return null;

            if(source.HasImage && !string.IsNullOrEmpty(source.Path))
            {
                source.Path = RecoverCopy(source.Path, TemplateId, VersionId);
            }


            Management.DTO_EditItem<Signature> RecoveredSignature = new Management.DTO_EditItem<Signature>();


            //if (WithPrevVersion == true)
            //{
            //    RecoveredSignature.PreviousVersion = (from Signature sgn in Manager.GetAll<Signature>()
            //                                         where sgn.Id != SignatureId
            //                                         && sgn.TemplateVersion.Id == source.TemplateVersion.Id
            //                                         && sgn.IsActive == false
            //                                         orderby sgn.SubVersion descending
            //                                         select new Management.DTO_EditPreviousVersion { Id = sgn.Id, Lastmodify = sgn.ModifiedOn }
            //                    ).ToList();
            //}
            //else
            //{
            RecoveredSignature.PreviousVersion = null;
            //}

            //RecoveredSettings.Permissions = Permission;

            RecoveredSignature.Data = source;
            return RecoveredSignature;
        }
    #endregion
        // TUTTA LA PARTE RELATIVA AI MODULE
        // Al momento sono state corrette le varie funzioni, ma sono DI FATTO INUTILI:
        // i SERVICE sono aggiunti in fase di CREAZIONE e non più modificati.
    #region Services
        /// <summary>
        /// Intanto la lascio, ma successivamente i SERVICE verranno aggiunti SOLO in fase di CREAZIONE del TEMPLATE e non più modificati!!!
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="Contents"></param>
        /// <returns></returns>
        private ItemUpdating ServiceUpdate(ref Template Template, IList<Management.DTO_EditItem<ServiceContent>> Contents)
        {
            if (Template == null || Contents == null)
                return ItemUpdating.Error;

            if (Template.Services == null)
                Template.Services = new List<ServiceContent>();

            IList<ServiceContent> OldServices = Template.Services.Where(sg => (sg.Deleted == BaseStatusDeleted.None && sg.IsActive == true)).ToList();

            IList<Int64> RemovedServiceId = new List<Int64>();

            //.Where(sg => (sg.Deleted == BaseStatusDeleted.None && sg.IsActive == true);

            foreach (Management.DTO_EditItem<ServiceContent> NewSrv in Contents)
            {
                //Per oggetti in liste, la "version" è totalmente inutile,
                //lasciata a fini di debug.
                NewSrv.Data.Version = 0;

                Boolean IsToAdd = true;

                foreach (ServiceContent OldSrv in OldServices)
                {

                    if (OldSrv.Id == NewSrv.Data.Id)
                    {
                        IsToAdd = false;

                        if (ServiceIsUpdate(OldSrv, NewSrv.Data))
                        {
                            OldSrv.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                            OldSrv.Deleted = BaseStatusDeleted.Automatic;
                            OldSrv.IsActive = false;
                            NewSrv.Data.Version = OldSrv.Version + 1;

                            RemovedServiceId.Add(OldSrv.Id);

                            IsToAdd = true;
                            break;
                        }
                    }
                }

                if (IsToAdd)
                {
                    NewSrv.Data.Id = 0;
                    NewSrv.Data.IsActive = true;
                    NewSrv.Data.Deleted = BaseStatusDeleted.None;
                    NewSrv.Data.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                    NewSrv.Data.Template = Template;
                    Template.Services.Add(NewSrv.Data);
                }

            }

            foreach (ServiceContent remsrv in Template.Services.Where(
                sg => (sg.Id > 0
                    && sg.Deleted == BaseStatusDeleted.None
                    && sg.IsActive == true
                    && !RemovedServiceId.Contains(sg.Id))))
            {
                remsrv.SetDeleteMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);
                remsrv.Deleted = BaseStatusDeleted.Automatic;
                remsrv.IsActive = false;
            }

            return ItemUpdating.Updated;
        }

        private Boolean ServiceIsUpdate(ServiceContent OldSign, ServiceContent NewSign)
        {
            if (OldSign.ModuleCode != NewSign.ModuleCode)
                return true;
            if(OldSign.ModuleId != OldSign.ModuleId)
                return true;
            if(OldSign.ModuleName != OldSign.ModuleName)
                return true;
            
            return false;
        }

        public Boolean ServiceDeletePhisical(Int64 ServiceId)
        {
            Boolean Deleted = false;
            if (Permission.EditTemplates)
            {
                ServiceContent Sc = Manager.Get<ServiceContent>(ServiceId);
                if (Sc.Deleted != BaseStatusDeleted.None && Sc.IsActive)
                {
                    Manager.BeginTransaction();

                    try
                    {
                        Manager.DeletePhysical<ServiceContent>(Sc);
                        Manager.Commit();
                    }
                    catch
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();

                        Deleted = false;
                    }
                }
            }
            return Deleted;
        }

        public Int64 ServiceRecover(Int64 TemplateId, Int64 ServiceId)
        {
            Int64 NewId = 0;
            if (Permission.EditTemplates)
            {
                Manager.BeginTransaction();

                ServiceContent OldService = Manager.Get<ServiceContent>(ServiceId);
                ServiceContent NewService = new ServiceContent();

                OldService.CreateMetaInfo(CurrentUser, this.UC.IpAddress, this.UC.ProxyIpAddress);

                NewService.IsActive = true;
                NewService.ModuleCode = OldService.ModuleCode;
                NewService.ModuleId = OldService.ModuleId;
                NewService.ModuleName = OldService.ModuleName;
                NewService.Version = OldService.Version + 1;
                
                OldService.Template.Services.Add(NewService);

                try
                {
                    Manager.SaveOrUpdate<ServiceContent>(NewService);
                    NewId = NewService.Id;
                }
                catch
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();

                    NewId = -1;
                }
            }

            return NewId;
        }

    #endregion

        private String ImageCopy(String SourceFile, Int64 srcTemplateId, Int64 srcVersionId, Int64 dstTemplateId, Int64 dstVersionId) //, String DestPath)
        {
            int DotPos = SourceFile.LastIndexOf(".");
            String NewFile = System.Guid.NewGuid().ToString() + SourceFile.Remove(0, DotPos);

            bool HasCopied = false;

            if(FileCopy != null)
            {
                HasCopied = FileCopy(this, new CopyEventArgs(SourceFile, srcTemplateId, srcVersionId, NewFile, dstTemplateId, dstVersionId));
            }
            else
            {
                throw new NotImplementedException("File COPY Event Handler not found!");
            }

            return NewFile;
        }

        private String RecoverCopy(String Sourcefile, Int64 TemplateId, Int64 VersionId)
        {
            int DotPos = Sourcefile.LastIndexOf(".");
            String NewFile = System.Guid.NewGuid().ToString() + Sourcefile.Remove(0, DotPos);

            bool HasCopied = false;

            if (FileRecCopy != null)
            {
                HasCopied = FileRecCopy(this, new CopyEventArgs(Sourcefile, TemplateId, VersionId, NewFile, TemplateId, VersionId));
            }
            else
            {
                throw new NotImplementedException("File COPY Event Handler not found!");
            }

            return "#" + NewFile;
        }

        private Boolean ImageDelete(String SourceFile, Int64 TemplateId, Int64 VersionId)
        {
            if (FileDelete != null)
            {
                return FileDelete(this, new DeleteRemTmpEventArgs(new Domain.Core.dtoFileDeleteRemTmp(SourceFile, TemplateId, VersionId)));
            }
            else
            {
                throw new NotImplementedException("File DELETE Event Handler not found!");
            }
            return false;
        }

        private Boolean ImageMoveTemp(String SourceFile, Int64 TemplateId, Int64 VersionId)
        {
            if (FileRemTmp != null)
            {
                return FileRemTmp(this, new DeleteRemTmpEventArgs(new Domain.Core.dtoFileDeleteRemTmp(SourceFile, TemplateId, VersionId)));
            }
            else
            {
                throw new NotImplementedException("File Remove Temp Event Handler not found!");
            }
            return false;
        }

        //The metod fire the event
        private int ImageCopyBlock(Domain.Core.dtoFileCopyBlock Files)
        {
            int sourcefiles = Files.Files.Count();
            if (sourcefiles <= 0)
            {
                return 0;
            }

            int copiedfiles = 0;

            if (FileCopyBlock != null)
            {
                copiedfiles = FileCopyBlock(this, new FileBlockEventArgs(Files));
            }
            else
            {
                throw new NotImplementedException("File CopyBlock Event Handler not found!");
            }

            return sourcefiles - copiedfiles;
        }

        public delegate bool FileCopyEventHandler(object sender, CopyEventArgs CopyEA);
        public event FileCopyEventHandler FileCopy;

        public delegate bool FileDeleteEventHandler(object sender, DeleteRemTmpEventArgs DelEA);
        public event FileDeleteEventHandler FileDelete;

        // Delegate
        public delegate int FileCopyBlockEventHandler(object sender, lm.Comol.Core.DomainModel.DocTemplateVers.Business.FileBlockEventArgs FilesEA);
        // The event
        public event FileCopyBlockEventHandler FileCopyBlock;

        public delegate bool FileRemTmpEventHandler(object sender, DeleteRemTmpEventArgs DelEA);
        public event FileRemTmpEventHandler FileRemTmp;

        public delegate bool FileRecoverCopyEventHandler(object sender, CopyEventArgs RecEA);
        public event FileRecoverCopyEventHandler FileRecCopy;
        
        //Da "esternalizzare"
        //public abstract Boolean Filecopy(String SourcePath, String DestPath);
        //{
        //    return true;
        //}

        //public abstract Boolean ImageDelete(String SourcePath);
        //{
        //    return true;
        //}
#endregion

    }

    public class FileBlockEventArgs : EventArgs
    {
        public FileBlockEventArgs(Domain.Core.dtoFileCopyBlock Files)
        {
            msg = Files;
        }
        private Domain.Core.dtoFileCopyBlock msg;
        public Domain.Core.dtoFileCopyBlock Message
        {
            get { return msg; }
        }
    }

    public class DeleteRemTmpEventArgs : EventArgs
    {
        public DeleteRemTmpEventArgs(Domain.Core.dtoFileDeleteRemTmp value)
        {
            msg = value;
        }
        private Domain.Core.dtoFileDeleteRemTmp msg;
        public Domain.Core.dtoFileDeleteRemTmp Message
        {
            get { return msg; }
        }
    }

    public class CopyEventArgs : EventArgs
    {
        public CopyEventArgs(String sourceFile,  Int64 srcTempalteId, Int64 srcVersionId, String destFile, Int64 dstTemplateId, Int64 dstVersionId)
        {
            msg = new Domain.Core.dtoFileCopy(sourceFile, srcTempalteId, srcVersionId, destFile, dstTemplateId, dstVersionId);
        }
        private Domain.Core.dtoFileCopy msg;
        public Domain.Core.dtoFileCopy Message
        {
            get { return msg; }
        }
    }

}
