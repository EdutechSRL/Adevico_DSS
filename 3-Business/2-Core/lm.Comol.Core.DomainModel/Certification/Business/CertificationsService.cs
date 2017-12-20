using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.Certifications.Business
{
    public class CertificationsService : CoreServices
    {
        private const string UniqueCode = "COREauth";
        private iApplicationContext _Context;

        #region initClass
            public CertificationsService() { }
            public CertificationsService(iApplicationContext oContext)
            {
                this.Manager = new BaseModuleManager(oContext.DataContext);
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public CertificationsService(iDataContext oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext();
                _Context.DataContext = oDC;
                this.UC = null;
            }
        #endregion

            public List<Certification> GetCertifications(ModuleObject source) {
                List<Certification> items = null;
                try
                {
                    Manager.BeginTransaction();
                    items = (from c in Manager.GetIQ<Certification>()
                             where c.Deleted == BaseStatusDeleted.None && c.SourceItem.Equals(source)
                             select c).ToList();

                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    items = new List<Certification>();
                }
                return items;
            }
            public List<Certification> GetUserCertifications(ModuleObject source, Int32 idUser, CertificationType type) {
                return GetUserCertifications(source, idUser, false, type);
            }
            public List<Certification> GetUserCertifications(ModuleObject source, Int32 idUser, Boolean all, CertificationType type = CertificationType.AutoProduced)
            {
                List<Certification> items = null;
                try
                {
                    Manager.BeginTransaction();
                    items = (from c in Manager.GetIQ<Certification>()
                            where c.Deleted == BaseStatusDeleted.None && c.SourceItem.Equals(source) && c.Owner.Id == idUser
                                && c.Status == CertificationStatus.Valid && (all || c.Type == type)
                            select c).ToList().OrderByDescending(c=> c.CreatedOn).ToList();

                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    items = new List<Certification>();
                }
                return items;
            }
            public Certification SaveUserCertification(dtoCertification dto)
            {
                Certification certification = null;
                try
                {
                    Manager.BeginTransaction();
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    Person owner = Manager.GetPerson(dto.IdOwner);
                    if (owner!=null && owner.TypeID != (int)UserTypeStandard.Guest  && person !=null){

                        List<Certification> items = (from c in Manager.GetIQ<Certification>()
                                                     where c.Deleted == BaseStatusDeleted.None && c.SourceItem.Equals(dto.SourceItem) && c.Owner.Id == dto.IdOwner
                                     && c.Type == dto.Type
                                 select c).ToList().OrderByDescending(c => c.CreatedOn).ToList();
                        Guid uId = Guid.NewGuid();
                        if (items.Count > 0) {
                            uId = items.Select(c => c.UniqueId).FirstOrDefault();
                            items.Where(i=>i.Status== CertificationStatus.Valid).ToList().ForEach(i => i.Status = CertificationStatus.OverWritten);
                        }

                        certification = new Certification();
                        certification.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        certification.IssuedOn =  DateTime.Now;
                        certification.SourceItem = dto.SourceItem;
                        certification.Status = dto.Status;
                        certification.Type = dto.Type;
                        certification.Owner = owner;
                        certification.SourceIdContainer = dto.IdContainer;
                        certification.UniqueId = uId;
                        certification.Name = dto.Name;
                        certification.Description = dto.Description;
                        certification.FileUniqueId=dto.UniqueIdGeneratedFile;
                        certification.Community = Manager.GetCommunity(dto.IdCommunity);
                        certification.IdTemplate = dto.IdTemplate;
                        certification.IdTemplateVersion = dto.IdTemplateVersion;
                        certification.FileExtension = dto.FileExtension;
                        certification.WithEmptyPlaceHolders = dto.WithEmptyPlaceHolders;
                        Manager.SaveOrUpdate(certification);
                    }
                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                    certification = null;
                }
                return certification;
            }


            public Boolean DeleteItemCertifications(ModuleObject source, CertificationType type)
            {
                return DeleteItemCertifications(source, false, type);
            }
            public Boolean DeleteItemCertifications(ModuleObject source, Boolean all, CertificationType type = CertificationType.AutoProduced)
            {
                return DeleteItemCertifications(source, -1, all, type);
            }
            public Boolean DeleteItemCertifications(ModuleObject source, Int32 idUser, CertificationType type){
                return DeleteItemCertifications(source, idUser, false, type);
            }
            public Boolean DeleteItemCertifications(ModuleObject source, Int32 idUser, Boolean all , CertificationType type = CertificationType.AutoProduced)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    if (p != null)
                    {
                        List<Certification> items = (from c in Manager.GetIQ<Certification>()
                                 where c.Deleted == BaseStatusDeleted.None && c.SourceItem.Equals(source) && (idUser==-1 || c.Owner.Id == idUser)
                                     && c.Status == CertificationStatus.Valid && (all || c.Type ==type)
                                 select c).ToList().OrderByDescending(c => c.CreatedOn).ToList();

                        items.ForEach(i => i.Status = CertificationStatus.Ignore);
                        items.ForEach(i => i.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress));
                    }
                    Manager.Commit();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }

            /// <summary>
            /// Delete users certifications for other module
            /// </summary>
            /// <param name="idContainer">module object identifier</param>
            /// <param name="idModule">identifier module</param>
            /// <param name="moduleCode">module code</param>
            /// <param name="type">type of certification to delete</param>
            /// <returns></returns>
            public Boolean DeleteUsersCertifications(long idContainer, Int32 idModule, String moduleCode, CertificationType type)
            {
                return DeleteUsersCertifications(idContainer, idModule, moduleCode, false, type);
            }
            /// <summary>
            /// Delete users certifications for other module
            /// </summary>
            /// <param name="idContainer">module object identifier</param>
            /// <param name="idModule">identifier module</param>
            /// <param name="moduleCode">module code</param>
            /// <param name="all">if true delete certification withot user check</param>
            /// <param name="type"></param>
            /// <returns></returns>
            public Boolean DeleteUsersCertifications(long idContainer, Int32 idModule, String moduleCode, Boolean all , CertificationType type = CertificationType.AutoProduced)
            {
                return DeleteUsersCertifications(idContainer, idModule, moduleCode, -1, all, type);
            }
            public Boolean DeleteUsersCertifications(long idContainer, Int32 idModule, String moduleCode, Int32 idUser, Boolean all, CertificationType type = CertificationType.AutoProduced)
            {
                Boolean deleted = false;
                try
                {
                    Manager.BeginTransaction();
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    if (p != null) { 
                        List<Certification> items = (from c in Manager.GetIQ<Certification>()
                                                 where c.Deleted == BaseStatusDeleted.None && c.SourceIdContainer == idContainer && c.SourceItem.ServiceCode == moduleCode && c.SourceItem.ServiceID == idModule && (idUser == -1 || c.Owner.Id == idUser)
                                 && c.Status == CertificationStatus.Valid && (all || c.Type == type)
                             select c).ToList().OrderByDescending(c => c.CreatedOn).ToList();
                        items.ForEach(i => i.Status= CertificationStatus.Ignore);
                        items.ForEach(i => i.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress));
                    }
                    Manager.Commit();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    deleted = false;
                }
                return deleted;
            }
            //public ModuleLongInternalFile CreateInternalFile(Person person, Certification certification, String fileName, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType type, Guid uniqueId)
            //{
            //    ModuleLongInternalFile intFile = new ModuleLongInternalFile();
            //    intFile.CloneID = 0;
            //    intFile.Extension = "." + type.ToString();
            //    intFile.Name = fileName;
            //    switch (type)
            //    {
            //        case  DomainModel.Helpers.Export.ExportFileType.pdf:
            //            intFile.ContentType = "text/pdf";
            //            break;
            //        case  DomainModel.Helpers.Export.ExportFileType.rtf:
            //            intFile.ContentType = "text/rtf";
            //            break;
            //        default:
            //            intFile.ContentType = "text/pdf";
            //            break;
            //    }

            //    intFile.Description = "";
            //    intFile.Downloads = 0;
            //    intFile.FileCategoryID = 0;
            //    intFile.FolderId = 0;
            //    intFile.isDeleted = false;
            //    intFile.isPersonal = true;
            //    intFile.isSCORM = false;
            //    intFile.isVirtual = false;
            //    intFile.isVideocast = false;
            //    intFile.IsDownloadable = true;
            //    intFile.isVisible = true;
            //    intFile.isFile = true;
            //    intFile.UniqueID = uniqueId;
            //    intFile.IsInternal = true;
            //    intFile.ObjectOwner = certification;
            //    intFile.ObjectTypeID = (int)ModuleCertifications.ObjectType.Certification;
            //    intFile.Owner = person;
            //    intFile.CreatedOn = DateTime.Now;
            //    intFile.ModifiedOn = intFile.CreatedOn;
            //    intFile.ModifiedBy = person;
            //    intFile.ServiceActionAjax = (int)ModuleCertifications.ActionType.DownloadFile;
            //    intFile.ServiceOwner = ModuleCertifications.UniqueCode;
            //    return intFile;
            //}
    }
}