using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Business
{
    public class CoreServices: BaseCoreServices
    {

        #region "PermissionRepository - OLD Version"

        public IList<iCoreItemFileLinkPermission<long>> GetCoreItemFileLinkPermission(iCoreItemPermission itemPermissions, IList<iCoreItemFileLink<long>> links, IList<TranslatedItem<long>> statusList, Person person)
        {
            IList<iCoreItemFileLinkPermission<long>> list = (from l in links
                                                             select (iCoreItemFileLinkPermission<long>)new dtoCoreItemFilePermission<long>()
                                                             {
                                                                 AvailableStatus = statusList,
                                                                 ItemFileLink = l,
                                                                 ItemFileLinkId = l.ItemFileLinkId,
                                                                 Permission = GetCoreFilePermission(itemPermissions, l, person)
                                                             }).ToList();
            return list;
        }
        public iCoreFilePermission GetCoreFilePermission(iCoreItemPermission itemPermissions, iCoreItemFileLink<long> itemFileLink, Person person)
        {
            CoreModuleRepository repository = null;
            CoreFilePermission permission = new CoreFilePermission();
            if (itemPermissions != null && itemFileLink.File != null && itemFileLink.File.Id > 0)
            {
                Boolean isMultimedia = (itemFileLink.File.isFile && itemFileLink.File.RepositoryItemType != DomainModel.Repository.RepositoryItemType.FileStandard && itemFileLink.File.RepositoryItemType != DomainModel.Repository.RepositoryItemType.Folder && itemFileLink.File.RepositoryItemType != DomainModel.Repository.RepositoryItemType.None);
                permission.Download = itemFileLink.File.IsDownloadable && itemPermissions.AllowViewFiles;
                permission.Play = isMultimedia && itemPermissions.AllowViewFiles;
                //  permission.EditRepositoryVisibility = false;
                permission.EditStatus = itemPermissions.AllowEdit;
                permission.EditVisibility = itemPermissions.AllowEdit;


                permission.Publish = itemPermissions.AllowFilePublish && itemFileLink.File.IsInternal && itemFileLink.Deleted == BaseStatusDeleted.None;
                permission.Unlink = itemPermissions.AllowEdit && !itemFileLink.File.IsInternal;



                permission.ViewPersonalStatistics = itemFileLink.File.isSCORM && itemPermissions.AllowViewFiles;
                permission.ViewStatistics = itemFileLink.File.isSCORM && itemPermissions.AllowViewStatistics;
                permission.Delete = itemFileLink.File.IsInternal && itemPermissions.AllowDelete && (itemFileLink.Deleted != BaseStatusDeleted.None);
                permission.UnDelete = itemFileLink.File.IsInternal && (itemFileLink.Deleted != BaseStatusDeleted.None) && itemPermissions.AllowUnDelete;
                permission.VirtualDelete = itemFileLink.File.IsInternal && (itemFileLink.Deleted == BaseStatusDeleted.None) && itemPermissions.AllowUnDelete;
                permission.EditMetadata = isMultimedia && ((itemFileLink.File.IsInternal && (itemFileLink.Deleted == BaseStatusDeleted.None) && itemPermissions.AllowEdit));

                BaseCommunityFile file = itemFileLink.File;
                if (!file.IsInternal)
                {
                    if (file.CommunityOwner == null)
                        repository = CoreModuleRepository.CreatePortalmodule(UC.UserTypeID);
                    else
                        repository = new CoreModuleRepository(Manager.GetModulePermission(person.Id, file.CommunityOwner.Id, Manager.GetModuleID(CoreModuleRepository.UniqueID)));
                    if (HasPermissionToSeeRepositoryItem(file.Id, repository.Administration, repository.Administration, UC.CurrentUserID))
                    {
                        Boolean itemOwner = (person == file.Owner);

                        // ATTENZIONE: !! DOVREBBE ANDARE QUESTO MA CREA PROBLEMI AL WCF SERVICES
                        //if (permission.Download==false && itemPermissions.AllowViewFiles ==true){
                        //   permission.Download = (repository.Administration || (file.IsDownloadable && repository.DownLoad));
                        //}

                        permission.EditRepositoryVisibility = repository.Administration || repository.Edit || (repository.UploadFile && itemOwner);
                        permission.EditRepositoryPermission = repository.Administration || repository.Edit || (repository.UploadFile && itemOwner);
                        permission.EditMetadata = isMultimedia && ((itemFileLink.Deleted == BaseStatusDeleted.None) && itemPermissions.AllowEdit && permission.EditRepositoryPermission);

                        permission.ViewPermission = repository.Administration || repository.Edit || (repository.UploadFile && itemOwner);
                        //   _Permission.Delete = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.DeleteMyFile AndAlso ItemOwner)
                        //  _Permission.UnDelete = False
                        //_Permission.VirtualDelete = False
                        // _Permission.Edit = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner)
                        // _Permission.EditPermission = oPermission.Administration OrElse oPermission.Edit OrElse (oPermission.UploadFile AndAlso ItemOwner)
                        permission.EditRepositoryPermission = false;
                    }
                }

                permission.Link = false;
            }
            else if (itemPermissions != null && itemFileLink.File == null || itemFileLink.File.Id == 0)
            {
                permission.Delete = itemPermissions.AllowDelete && (itemFileLink.Deleted != BaseStatusDeleted.None);
                permission.VirtualDelete = (itemFileLink.Deleted == BaseStatusDeleted.None) && itemPermissions.AllowUnDelete;
                permission.EditVisibility = itemPermissions.AllowEdit;
            }
            return (iCoreFilePermission)permission;
        }
        public bool HasPermissionToSeeRepositoryItem(long itemId, bool ShowHiddenItems, bool adminPurpose, int personID)
        {
            return HasPermissionToSeeRepositoryItem(itemId, ShowHiddenItems, adminPurpose, Manager.GetPerson(personID));
        }
        public bool HasPermissionToSeeRepositoryItem(long ItemId, bool ShowHiddenItems, bool AdminPurpose, Person oPerson)
        {
            Boolean iResponse = false;
            try
            {
                Manager.BeginTransaction();
                CommunityFile oCommunityFile = Manager.Get<CommunityFile>(ItemId);
                if ((oCommunityFile != null))
                {
                    if (AdminPurpose)
                    {
                        iResponse = ShowHiddenItems || (oCommunityFile.isVisible) || oCommunityFile.Owner == oPerson;
                    }
                    else
                    {
                        bool HasPersonAssignment = false;
                        bool HasRoleAssignment = false;
                        bool HasCommunityAssignment = false;
                        bool HasDenyPerson = false;
                        bool HasDenyRole = false;
                        bool HasDenyCommunity = false;

                        if (oCommunityFile.CommunityOwner == null)
                        {
                            var QueryRole = (from fa in Manager.Linq<CommunityFilePersonTypeAssignment>() where fa.File == oCommunityFile && fa.Inherited && fa.AssignedTo == oPerson.TypeID && (ShowHiddenItems || fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == oPerson)) select fa);
                            HasRoleAssignment = (from CommunityFilePersonTypeAssignment q in QueryRole where !q.Deny select q).Any();
                            HasDenyRole = (from CommunityFilePersonTypeAssignment q in QueryRole where q.Deny select q).Any();

                        }
                        else
                        {
                            Role oRole = (from s in Manager.Linq<Subscription>() where s.Community == oCommunityFile.CommunityOwner && s.Person == oPerson && s.Accepted && s.Enabled select (Role)s.Role).FirstOrDefault();

                            var QueryRole = (from fa in Manager.Linq<CommunityFileRoleAssignment>() where fa.File == oCommunityFile && fa.AssignedTo == oRole && fa.Inherited && (ShowHiddenItems || fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == oPerson)) select fa);
                            HasRoleAssignment = (from q in QueryRole where !q.Deny select q).Any();
                            HasDenyRole = (from q in QueryRole where q.Deny select q).Any();
                        }
                        var QueryCommunity = (from fa in Manager.Linq<CommunityFileCommunityAssignment>() where fa.File == oCommunityFile && fa.AssignedTo == oCommunityFile.CommunityOwner && fa.Inherited && (ShowHiddenItems || fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == oPerson)) select fa);

                        HasCommunityAssignment = (from q in QueryCommunity where !q.Deny select q).Any();
                        HasDenyCommunity = (from q in QueryCommunity where q.Deny select q).Any();

                        var QueryPerson = (from fa in Manager.Linq<CommunityFilePersonAssignment>() where fa.File == oCommunityFile && fa.AssignedTo == oPerson && fa.Inherited && (ShowHiddenItems || fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == oPerson)) select fa);
                        HasPersonAssignment = (from q in QueryPerson where !q.Deny select q).Any();
                        HasDenyPerson = (from q in QueryPerson where q.Deny select q).Any();

                        iResponse = (HasCommunityAssignment || HasRoleAssignment || HasPersonAssignment);

                        if (iResponse)
                        {
                            iResponse = (HasCommunityAssignment && !HasDenyPerson && !HasDenyRole) || (HasRoleAssignment && !HasDenyPerson) || HasPersonAssignment;
                        }

                    }
                }
                Manager.Commit();
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                {
                    Manager.RollBack();
                }
            }
            return iResponse;
        }
        public bool HasPermissionToUploadIntoRepositoryFolder(long folderId, int personId, CoreModuleRepository module)
        {
            if (module.Administration || module.UploadFile)
                return true;
            else
                return this.HasPermissionToUploadIntoRepositoryFolder(folderId, Manager.GetPerson(personId), module);
        }
        public bool HasPermissionToUploadIntoRepositoryFolder(long folderId, Person person, CoreModuleRepository module)
        {
            bool iResponse = module.Administration || module.UploadFile;
            if (!iResponse)
            {
                try
                {
                    Manager.BeginTransaction();
                    CommunityFile oFolder = Manager.Get<CommunityFile>(folderId);
                    if ((oFolder != null))
                    {
                        long AssignedPermission = 0;

                        Community community = oFolder.CommunityOwner;
                        if (community == null)
                        {
                            AssignedPermission = (from fa in Manager.Linq<CommunityFilePersonTypeAssignment>() where fa.File == oFolder && fa.Inherited && fa.AssignedTo == person.TypeID && (fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == person)) && fa.Deny == false select fa.Permission).FirstOrDefault();
                        }
                        else
                        {
                            Role oRole = (from s in Manager.Linq<Subscription>() where s.Community == community && s.Person == person && s.Accepted && s.Enabled select (Role)s.Role).FirstOrDefault();

                            AssignedPermission = (from fa in Manager.Linq<CommunityFileRoleAssignment>() where fa.File == oFolder && fa.AssignedTo == oRole && fa.Inherited && (fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == person)) && fa.Deny == false select fa.Permission).FirstOrDefault();
                        }
                        if (!((AssignedPermission & (long)CoreModuleRepository.Base2Permission.UploadFile) > 0))
                        {
                            AssignedPermission = AssignedPermission | (from fa in Manager.Linq<CommunityFileCommunityAssignment>() where fa.File == oFolder && fa.AssignedTo == community && fa.Inherited && (fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == person)) && fa.Deny == false select fa.Permission).FirstOrDefault();

                        }

                        if (!((AssignedPermission & (long)CoreModuleRepository.Base2Permission.UploadFile) > 0))
                        {
                            AssignedPermission = AssignedPermission | (from fa in Manager.Linq<CommunityFilePersonAssignment>() where fa.File == oFolder && fa.AssignedTo == person && fa.Inherited && (fa.File.isVisible || (!fa.File.isVisible && fa.File.Owner == person)) && fa.Deny == false select fa.Permission).FirstOrDefault();

                        }
                        iResponse = ((AssignedPermission & (long)CoreModuleRepository.Base2Permission.UploadFile) > 0);
                    }
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return iResponse;
        }
       
        #endregion

        #region "REpository utility"
            public string GetFolderName_OLD(long IdFolder){
                String PathName = "";
                if (IdFolder>0){
                    CommunityFile folder = (from f in Manager.GetIQ<CommunityFile>() where f.Id == IdFolder && !f.isFile select new CommunityFile() { Id = f.Id, Name = f.Name }).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (folder != null)
                        PathName = folder.Name;
                }
                else
                    PathName = "/";

                return PathName;
            }
        #endregion 

        public CoreServices():base() { }
        public CoreServices(iApplicationContext oContext) : base( oContext)
        {
        }
        public CoreServices(iDataContext oDC)
            : base(oDC)
        {
        }

        public liteSubscriptionInfo GetSubscriptionInfo(Int32 idPerson, Int32 idCommunity)
        {
            liteSubscriptionInfo result = null;
            try
            {
                result = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdPerson == idPerson && s.IdCommunity == idCommunity select s).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch(Exception ex)
            {
                result = null;
            }
            return result;
        }
        protected List<string> DefaultOtherChars()
        {
            List<string> chars = new List<string>();
            for (int i = 32; i <= 47; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 58; i <= 64; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 91; i <= 96; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            for (int i = 123; i <= 126; i++)
            {
                chars.Add(Char.ConvertFromUtf32(i));
            }
            return chars;
        }
    }
}