using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.File;

namespace lm.Comol.Core.BaseModules.Repository.Business
{

    public class ServiceCommunityRepository : CoreServices
    {
        protected const int maxItemsForQuery = 100;
        private const string UniqueCode = "SRVMATER";
        //private BaseModuleManager Manager { get; set; }
        //private iUserContext UC { set; get; }
        #region initClass
        public ServiceCommunityRepository() { }
        public ServiceCommunityRepository(iApplicationContext oContext)
        {
            this.Manager = new BaseModuleManager(oContext.DataContext);
            this.UC = oContext.UserContext;
        }
        public ServiceCommunityRepository(iDataContext oDC)
        {
            this.Manager = new BaseModuleManager(oDC);
            this.UC = null;
        }


        #endregion
        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(UniqueCode);
        }

        public CoreModuleRepository ServicePermission(int idPerson, int idCommunity)
        {
            CoreModuleRepository module = new CoreModuleRepository();
            Person person = Manager.GetPerson(idPerson);
            if (person == null)
                person = (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();

            if (idCommunity == 0)
                module = CoreModuleRepository.CreatePortalmodule(person.TypeID);
            else
                module = new CoreModuleRepository(Manager.GetModulePermission(idPerson, idCommunity, ServiceModuleID()));
            return module;
        }


        #region "ModuleLink"
            public List<ModuleLink> GetFileLinks(List<long> items, Int32 permission, Int32 idModule){
                List<ModuleLink> links = new  List<ModuleLink>();
                try{
                    List<BaseCommunityFile> files = (from f in Manager.GetIQ<BaseCommunityFile>()
                                                     where f.isDeleted == false && f.isFile && items.Contains(f.Id)
                                                     select f).ToList();
                    links = (from f in files
                             select new ModuleLink(permission, (Int32)GetDefaultAction(f.RepositoryItemType)) { DestinationItem= CreateModuleObject(f,idModule), EditEnabled=false}).ToList();

                }
                catch(Exception ex){
                
                }
                return links;
            }
            public List<ModuleActionLink> GetModuleActionLinkItems(List<long> items, Int32 permission, Int32 idModule){
                List<ModuleActionLink> links = new  List<ModuleActionLink>();
                try{
                    List<BaseCommunityFile> files = (from f in Manager.GetIQ<BaseCommunityFile>()
                                                     where f.isDeleted == false && f.isFile && items.Contains(f.Id)
                                                     select f).ToList();
                    links = (from f in files
                             select new ModuleActionLink(permission, (Int32)GetDefaultAction(f.RepositoryItemType)) { ModuleObject= CreateModuleObject(f,idModule), EditEnabled=false}).ToList();

                }
                catch(Exception ex){
                
                }
                return links;
            }
        //  Public Function GetFilesForModuleLinkDefaultAction(ByVal oList As List(Of Long), ByVal Permission As Integer, ByVal ModuleID As Integer) As List(Of ModuleLink)
        //    Dim oLinks As New List(Of ModuleLink)
        //    Try
        //        Dim oQuery = (From file In DC.GetCurrentSession.Linq(Of CommunityFile)() _
        //                      Where file.isFile AndAlso oList.Contains(file.Id) _
        //                      Select file)

        //        oLinks = (From item In oQuery.ToList Select New ModuleLink(Permission, GetDefaultAction(item.RepositoryItemType)) With {.DestinationItem = CreateModuleObject(item, ModuleID), .EditEnabled = False}).ToList

        //    Catch ex As Exception
        //    End Try

        //    Return oLinks
        //End Function

            public ModuleLink GetModuleLink(long IdLink)
            {
                return Manager.Get<ModuleLink>(IdLink);
            }
            public ModuleLink CreateModuleLink(ModuleObject source, BaseCommunityFile file, int permission, int moduleId)
            {
                return new ModuleLink(permission, ItemDefaultAction(file)) { DestinationItem = CreateModuleObject(file, moduleId), SourceItem = source };
            }
            public static ModuleLink CreateModuleLink(ModuleObject source, BaseCommunityFile file, int permission, int action, int moduleId)
            {
                return new ModuleLink(permission, action) { DestinationItem = CreateModuleObject(file, moduleId), SourceItem = source };
            }
            public static ModuleLink CreateModuleLink(BaseCommunityFile file, int permission, int action, int moduleId)
            {
                return new ModuleLink(permission, action) { DestinationItem = CreateModuleObject(file, moduleId) };
            }
            public ModuleLink CreateModuleLink(BaseCommunityFile file, int permission, int idModule)
            {
                return new ModuleLink(permission, ItemDefaultAction(file)) { DestinationItem = CreateModuleObject(file, idModule) };
            }
            public static ModuleActionLink CreateModuleActionLink(BaseCommunityFile file, int permission, int action, int moduleId)
            {
                return new ModuleActionLink(permission, action) { ModuleObject = CreateModuleObject(file, moduleId) };
            }
            public static ModuleActionLink CreateModuleActionLink(ModuleInternalFile file, int permission, int action, int moduleId)
            {
                return new ModuleActionLink(permission, action) { ModuleObject = CreateModuleObject(file, moduleId), EditEnabled = false };
            }
            public ModuleActionLink CreateModuleActionLink(BaseCommunityFile file, int permission, int idModule)
            {
                return new ModuleActionLink(permission, ItemDefaultAction(file)) { ModuleObject = CreateModuleObject(file, idModule), EditEnabled = false };
            }
            private static ModuleObject CreateModuleObject(BaseCommunityFile file, int moduleId)
            {
                ModuleObject item = new ModuleObject();
                item.FQN = file.GetType().FullName;
                item.ObjectLongID = file.Id;
                item.ObjectOwner = file;

                if (file.isFile)
                    item.ObjectTypeID = (int)file.RepositoryItemType;
                else
                    item.ObjectTypeID = (int)CoreModuleRepository.ObjectType.Folder;

                item.ServiceCode = CoreModuleRepository.UniqueID;
                item.ServiceID = moduleId;
                if (file.CommunityOwner == null)
                    item.CommunityID = 0;
                else
                    item.CommunityID = file.CommunityOwner.Id;

                return item;
            }
            
            private CoreModuleRepository.ActionType GetDefaultAction (RepositoryItemType type){
                switch(type){
                    case DomainModel.Repository.RepositoryItemType.FileStandard:
                        return DomainModel.CoreModuleRepository.ActionType.DownloadFile;
                    case DomainModel.Repository.RepositoryItemType.Multimedia:
                        return DomainModel.CoreModuleRepository.ActionType.PlayFile;
                    case DomainModel.Repository.RepositoryItemType.ScormPackage:
                        return DomainModel.CoreModuleRepository.ActionType.PlayFile;
                    case DomainModel.Repository.RepositoryItemType.VideoStreaming:
                        return DomainModel.CoreModuleRepository.ActionType.PlayFile;
                    default:
                        return DomainModel.CoreModuleRepository.ActionType.None;
                }
            }
            private Int32 ItemDefaultAction(BaseCommunityFile file)
            {
                if (file.RepositoryItemType == RepositoryItemType.FileStandard || file.RepositoryItemType== RepositoryItemType.Folder || file.RepositoryItemType== RepositoryItemType.None)
                    return (int)CoreModuleRepository.ActionType.DownloadFile;
                else
                    return (int)CoreModuleRepository.ActionType.PlayFile;
            }

            public Boolean UpdateModuleInternalFile(List<ModuleLink> items, FileRepositoryType internalType){
                Boolean executed = true;
                try{
                    Manager.BeginTransaction();
                    foreach (ModuleLink item in items){
                        long idItem = item.DestinationItem.ObjectLongID;
                        switch(internalType){
                            case FileRepositoryType.InternalGuid:
                                ModuleGuidInternalFile gFile = Manager.Get<ModuleGuidInternalFile>(idItem);
                                gFile.ServiceOwner = item.SourceItem.ServiceCode;
                                gFile.ObjectTypeID = item.SourceItem.ObjectTypeID;
                                gFile.ObjectOwner = item.SourceItem.ObjectOwner;
                                Manager.SaveOrUpdate(gFile);
                                break;
                            case FileRepositoryType.InternalLong:
                                ModuleLongInternalFile lFile = Manager.Get<ModuleLongInternalFile>(idItem);
                                lFile.ServiceOwner = item.SourceItem.ServiceCode;
                                lFile.ObjectTypeID = item.SourceItem.ObjectTypeID;
                                lFile.ObjectOwner = item.SourceItem.ObjectOwner;
                                Manager.SaveOrUpdate(lFile);
                                break;
                        }
                    }
                    Manager.Commit();
                }
                catch (Exception ex){
                    Manager.RollBack();
                }
                return executed;
            }
        #endregion

        #region "Generic statistics"
            public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(int IdCommunity, int IdUser, long objectId, int objectTypeId, Dictionary<int, string> translations)
            {
                StatTreeNode<StatFileTreeLeaf> node = null;
                Person person = Manager.Get<Person>(IdUser);
                long IdFolder = 0;
                //CommunityFile item = Manager.Get<CommunityFile>(objectId);
                //if (item != null && !item.IsInternal && item.CommunityOwner != null)
                //    IdCommunity = item.CommunityOwner.Id;

                //if (objectTypeId == (int)CoreModuleRepository.ObjectType.Folder && item != null && !item.isFile && !item.IsInternal && objectId > 0)
                //    IdFolder = objectId;
                //else if ((objectTypeId == (int)CoreModuleRepository.ObjectType.File || objectTypeId == (int)CoreModuleRepository.ObjectType.FileScorm) && item != null && item.isFile && !item.IsInternal)
                //{
                //    IdFolder = item.FolderId;
                //}
                //else
                //    item = null;
                CoreModuleRepository repository = ServicePermission(IdUser, IdCommunity);
                if (repository.Administration || repository.ListFiles || repository.DownLoad || repository.UploadFile)
                {
                    Boolean viewAdvanced = (repository.Administration || repository.Edit);
                    node = LoadItemFilesForStatistics(Manager.Get<Community>(IdCommunity), person, null, IdFolder, viewAdvanced, repository.Administration, repository.Administration);
                    //if (item == null) { 

                    //}
                    //else if (item.isFile) { 
                    //    node.Id = item.FolderId;
                    //    node.isVisible = item.isVisible;
                    //    node.Name = "";
                    //    node.Leaves.Add(CreateStatFileTreeLeaf(person,item,viewAdvanced));
                    //}
                }
                return node;
            }
            private StatTreeNode<StatFileTreeLeaf> LoadItemFilesForStatistics(Community community, Person person, CommunityFile item, long IdFolder, Boolean viewAdvanced, Boolean showAlsoHiddenItems, Boolean AdminPurpose)
            {
                List<CommunityFile> items = GetCommunityItems(community, person, IdFolder, showAlsoHiddenItems, AdminPurpose);
                StatTreeNode<StatFileTreeLeaf> rootNode = CreateStatTreeNode(item);
                foreach (CommunityFile folder in (from f in items where !f.isFile && !f.IsInternal orderby f.Name select f).ToList())
                {
                    StatTreeNode<StatFileTreeLeaf> folderNode = CreateStatTreeNode(folder);
                    RecursivelyLoadItemFilesForStatistics(folderNode, community, person, folder, folder.Id, viewAdvanced, showAlsoHiddenItems, AdminPurpose);
                    rootNode.Nodes.Add(folderNode);
                }

                rootNode.Leaves = (from f in items where f.isFile && !f.IsInternal orderby f.DisplayName select CreateStatFileTreeLeaf(person, f, viewAdvanced)).ToList();
                return rootNode;
            }
            private void RecursivelyLoadItemFilesForStatistics(StatTreeNode<StatFileTreeLeaf> fatherNode, Community community, Person person, CommunityFile item, long IdFolder, Boolean viewAdvanced, Boolean showAlsoHiddenItems, Boolean AdminPurpose)
            {
                List<CommunityFile> items = GetCommunityItems(community, person, IdFolder, showAlsoHiddenItems, AdminPurpose);
                foreach (CommunityFile folder in (from f in items where !f.isFile && !f.IsInternal orderby f.Name select f).ToList())
                {
                    StatTreeNode<StatFileTreeLeaf> folderNode = CreateStatTreeNode(folder);
                    RecursivelyLoadItemFilesForStatistics(folderNode, community, person, folder, folder.Id, viewAdvanced, showAlsoHiddenItems, AdminPurpose);
                    fatherNode.Nodes.Add(folderNode);
                }

                fatherNode.Leaves = (from f in items where f.isFile && !f.IsInternal orderby f.DisplayName select CreateStatFileTreeLeaf(person, f, viewAdvanced)).ToList();
            }
            private StatTreeNode<StatFileTreeLeaf> CreateStatTreeNode(CommunityFile item)
            {
                StatTreeNode<StatFileTreeLeaf> leaf = new StatTreeNode<StatFileTreeLeaf>();
                if (item == null)
                {
                    leaf.Id = 0;
                    leaf.isVisible = true;
                    leaf.Name = "";
                }
                else
                {
                    leaf.Id = item.Id;
                    leaf.isVisible = item.isVisible;
                    leaf.Name = item.Name;
                    leaf.ToolTip = "";
                };
                return leaf;
            }
            private StatFileTreeLeaf CreateStatFileTreeLeaf(Person person, CommunityFile item, Boolean viewAdvanced)
            {
                StatFileTreeLeaf leaf = new StatFileTreeLeaf()
                {
                    Id = item.Id,
                    Extension = item.Extension,
                    isVisible = item.isVisible,
                    LinkId = 0,
                    Name = item.DisplayName,
                    ToolTip = "",
                    Type = StatTreeLeafType.Personal,
                    isScorm = item.isSCORM,
                    UniqueID = item.UniqueID,
                    DownloadCount = item.Downloads
                };
                if ((viewAdvanced || item.Owner == person))
                    leaf.Type |= StatTreeLeafType.Advanced;
                return leaf;
            }
        #endregion

        public List<CommunityFile> GetCommunityItems(Community community, Person person, long IdFolder, Boolean showAlsoHiddenItems, Boolean AdminPurpose)
        {
            Role role = (from Subscription s in Manager.GetIQ<Subscription>()
                         where s.Community == community && s.Person == person
                         select (Role)s.Role).Skip(0).Take(0).ToList().FirstOrDefault();
            return GetCommunityItems(community, person, IdFolder, showAlsoHiddenItems, AdminPurpose, role);
        }
        public List<CommunityFile> GetCommunityItems(Community community, Person person, long IdFolder, Boolean showAlsoHiddenItems, Boolean AdminPurpose, Role role)
        {
            List<CommunityFile> items = new List<CommunityFile>();
            try
            {
                Manager.BeginTransaction();
                if (AdminPurpose)
                    items = (from f in Manager.GetIQ<CommunityFile>()
                             where f.CommunityOwner == community && f.FolderId == IdFolder && (showAlsoHiddenItems || (f.isVisible || f.Owner == person))
                             select f).ToList();
                else
                {
                    List<long> IdItems = (from f in Manager.GetIQ<CommunityFile>()
                                          where f.CommunityOwner == community && f.FolderId == IdFolder
                                              && (showAlsoHiddenItems || (f.isVisible || f.Owner == person))
                                          select f.Id).ToList();
                    items = (from fa in Manager.GetIQ<CommunityFileCommunityAssignment>() where IdItems.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == community && !fa.Deny select fa.File).ToList();
                    items.AddRange((from fa in Manager.GetIQ<CommunityFileRoleAssignment>()
                                    where IdItems.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == role && !fa.Deny && !items.Contains(fa.File)
                                    select fa.File).ToList());
                    items.AddRange((from fa in Manager.GetIQ<CommunityFilePersonAssignment>()
                                    where IdItems.Except((from f in items select f.Id).ToList()).Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == person && !fa.Deny && !items.Contains(fa.File)
                                    select fa.File).ToList());
                }

                //if (notWaiting)
                //{
                //    List<CommunityFile> wFiles = items.Where(i => i.RepositoryItemType == RepositoryItemType.Multimedia || i.RepositoryItemType == RepositoryItemType.ScormPackage).ToList();
                //    List<long> wFilesId = wFiles.Select(f => f.Id).ToList();
                //    List<FileTransfer> wItems = GetWaitingFiles(wFiles.Select(f => f.Id).ToList());
                //    if (wItems.Any()){
                //        wFilesId = wItems.Select(f=> f.File.Id).ToList();
                //        items = items.Where(f => !wFilesId.Contains(f.Id)).ToList();
                //    }
                //}
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return items;
        }
        public List<long> GetIdWaitingFiles(List<long> idFiles)
        {
            if (idFiles==null || !idFiles.Any())
                return new List<long>();
            else{
                List<FileTransfer> wItems = GetWaitingFiles(idFiles);
                if (wItems.Any())
                    return wItems.Select(f => f.File.Id).ToList();
                else
                    return new List<long>();
            }
        }
        private List<FileTransfer> GetWaitingFiles(List<long> idFiles)
        {
            List<FileTransfer> items = new List<FileTransfer>();
            Int32 pageIndex = 0;
            var idQuery = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            while (idQuery.Any())
            {
                items.AddRange((from ft in Manager.GetIQ<FileTransfer>()
                               where ft.File!=null && idQuery.Contains(ft.File.Id ) && ft.Status!= TransferStatus.Completed
                               select ft).ToList());
                pageIndex++;
                idQuery = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
            }
            return items;
        }

        #region "Files"
            public CommunityFile AddFile(CommunityFile file, String communityPath, long permission, ref ItemRepositoryStatus status){
                CommunityFile item = null;
                try{
                    Manager.BeginTransaction();
                    VerifyAndUpdateItemName(file);
                    List<String> fileNames = (from f in Manager.GetIQ<CommunityFile>()
                                              where f.CommunityOwner== file.CommunityOwner && f.FolderId== file.FolderId && f.isFile
                                              && f.DisplayName.StartsWith(file.DisplayName, StringComparison.InvariantCultureIgnoreCase) select f.Name).ToList();

                    if (fileNames.Any()){
                        item = null;
                        status= DomainModel.ItemRepositoryStatus.FileExist;
                    }
                    else{
                        item = file;
                        Manager.SaveOrUpdate(item);
                        AddCommunityAssignmentToItem(item.CreatedOn, item.Owner, item.Id, false, permission, false);
                        if (item.FolderId == 0)
                            AddCommunityAssignmentToItem(item.CreatedOn, item.Owner, item.Id, false, permission, true);
                        else
                            ApplyInheritedAssignment(item.CreatedOn, item.Owner, item, (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File.Id == file.FolderId && fa.Inherited select fa).ToList(), false, permission);
                        if (file.isFile && file.RepositoryItemType != RepositoryItemType.FileStandard)
                            AddFileForTransfer(file, communityPath);                   
                        status= DomainModel.ItemRepositoryStatus.FileUploaded;
                    }
                    Manager.Commit();
                }
                catch(Exception ex){
                    Manager.RollBack();
                    item = null;
                    if (status != DomainModel.ItemRepositoryStatus.FileExist)
                        status = (file.isFile) ? DomainModel.ItemRepositoryStatus.UploadError : DomainModel.ItemRepositoryStatus.CreationError;
                }
                return item;
            }
            public CommunityFile AddFile(CommunityFile file,Person owner, String communityPath, long permission, ref ItemRepositoryStatus status){
                CommunityFile item = null;
                try{
                    Manager.BeginTransaction();
                    VerifyAndUpdateItemName(file);
                    List<String> fileNames = (from f in Manager.GetIQ<CommunityFile>()
                                              where f.CommunityOwner== file.CommunityOwner && f.FolderId== file.FolderId && f.isFile
                                              && f.DisplayName.StartsWith(file.DisplayName, StringComparison.InvariantCultureIgnoreCase) select f.Name).ToList();
                    if (!fileNames.Any()){
                        Manager.SaveOrUpdate(file);
                        AddPersonAssignmentToItem(file.CreatedOn, file.Owner, file, file.Owner, false, permission, false);
                        if (file.FolderId==0)
                            AddPersonAssignmentToItem(file.CreatedOn, file.Owner, file, file.Owner, false, permission, true);
                        else
                            ApplyInheritedAssignment(file.CreatedOn, file.Owner, file, (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File.Id == file.FolderId && fa.Inherited select fa).ToList(), false, permission);

                        if (file.isFile && file.RepositoryItemType != DomainModel.Repository.RepositoryItemType.FileStandard)
                            AddFileForTransfer(file, communityPath);
                        status = ItemRepositoryStatus.FileUploaded;
                        item = file;
                    }
                    else{
                        status = ItemRepositoryStatus.FileExist;
                        item = file;
                    }
                    Manager.Commit();
                }
                catch( Exception ex){
                    Manager.RollBack();
                    item = null;
                    if (status != DomainModel.ItemRepositoryStatus.FileExist)
                        status = DomainModel.ItemRepositoryStatus.CreationError;
                }
                return item;
            }
        #endregion

        #region "Folders"
            public String GetFolderPathName(long idFolder){
                String path = "/";
                if (idFolder > 0){
                    try{
                        Manager.BeginTransaction();
                        BaseCommunityFile folder = (from f in Manager.GetIQ<BaseCommunityFile>()
                                               where f.isFile == false && f.Id== idFolder select f).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (folder!=null){
                            if (folder.FolderId==0)
                                path += folder.Name;
                            else
                                path += GetInternalFolderPathName(folder.Id) + "/" +folder.Name;
                        }
                            
                        Manager.Commit();
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        path = "";
                    }
                }
                return path;
            }
            private String GetInternalFolderPathName(long idFolder){
                String path = "/";
                if (idFolder > 0){
                    try{
                        Manager.BeginTransaction();
                        BaseCommunityFile folder = (from f in Manager.GetIQ<BaseCommunityFile>()
                                               where f.isFile == false && f.Id== idFolder select f).Skip(0).Take(1).ToList().FirstOrDefault();

                        Manager.Commit();
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        path = "";
                    }
                }
                return path;
            }
            public String GetFolderName(long idFolder){
                String path = "/";
                if (idFolder > 0){
                    try{
                        Manager.BeginTransaction();
                        path = (from f in Manager.GetIQ<BaseCommunityFile>()
                                               where f.isFile == false && f.Id== idFolder select f.Name).Skip(0).Take(1).ToList().FirstOrDefault();

                        Manager.Commit();
                    }
                    catch(Exception ex){
                        Manager.RollBack();
                        path = "";
                    }
                }
                return path;
            }
            public Boolean CommunityHasFolders(Int32 idCommunity){
                Boolean found = false;
                try{
                    Manager.BeginTransaction();
                    if (idCommunity == 0)
                        found = (from f in Manager.GetIQ<CommunityFile>() where f.CommunityOwner == null && f.isFile == false select f.Id).Any();
                    else
                        found = (from f in Manager.GetIQ<CommunityFile>() where f.CommunityOwner != null && f.CommunityOwner.Id == idCommunity && f.isFile == false select f.Id).Any();
                    Manager.Commit();
                }
                catch(Exception ex){
                    Manager.RollBack();
                }
                return found;
            }

        #endregion
        #region "Delete"
        //    public Boolean DeleteFiles(List<long> files, String communityPath){
        //        Boolean result = false;
        //        try{
                
        //        }
        //        catch(Exception ex){
                
        //        }
        //        return false;
        //    }
        // Public Function DeleteFiles(ByVal oFileID As List(Of Long), ByVal CommunityPath As String) As Boolean
        //    Dim oResponse As Boolean = False
        //    Try
        //        Dim oFiles As List(Of BaseCommunityFile) = (From f In Me.DC.GetCurrentSession.Linq(Of BaseCommunityFile)() _
        //                        Where oFileID.Contains(f.Id) Select f).ToList
        //        If oFiles.Count > 0 Then
        //            Dim FilesName As List(Of String) = (From f In oFiles Select CommunityPath & f.UniqueID.ToString & ".stored").ToList
        //            DC.BeginTransaction()

        //            DeleteFilesTransfer((From f In oFiles Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f).ToList())
        //            For Each oItem As BaseCommunityFile In oFiles
        //                Dim oFile As BaseCommunityFile = oItem
        //                If TypeOf oItem Is CommunityFile Then
        //                    Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

        //                    For Each oDiaryFile In oDiaryFiles
        //                        DC.GetCurrentSession.Delete(oDiaryFile)
        //                    Next

        //                    Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

        //                    For Each oWorkBookFile In oWorkBookFiles
        //                        DC.GetCurrentSession.Delete(oWorkBookFile)
        //                    Next
        //                End If
        //                DC.Delete(oFile)
        //            Next

        //            Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oFiles.Contains(fa.File) Select fa).ToList
        //            For Each Assigment In oAssigments
        //                DC.GetCurrentSession.Delete(Assigment)
        //            Next

        //            DC.Commit()

        //            Delete.Files(FilesName)
        //        End If
        //        oResponse = True
        //    Catch ex As Exception
        //        oResponse = Nothing
        //        If DC.isInTransaction Then
        //            DC.Rollback()
        //        End If
        //    End Try
        //    Return oResponse
        //End Function
        //Public Function DeleteFolder(ByVal oFolder As CommunityFile, ByVal CommunityPath As String) As Boolean
        //    Dim oResponse As Boolean = False
        //    Try
        //        DC.BeginTransaction()
        //        Dim oFileNames As New List(Of String)
        //        DeleteFolderContent(oFolder, CommunityPath, oFileNames)

        //        Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where fa.File Is oFolder Select fa).ToList
        //        For Each Assigment In oAssigments
        //            DC.GetCurrentSession.Delete(Assigment)
        //        Next
        //        DC.GetCurrentSession.Delete(oFolder)
        //        DC.Commit()
        //        Delete.Files(oFileNames)
        //        oResponse = True
        //    Catch ex As Exception
        //        oResponse = Nothing
        //        If DC.isInTransaction Then
        //            DC.Rollback()
        //        End If
        //    End Try
        //    Return oResponse
        //End Function
        //Private Sub DeleteFolderContent(ByVal oFolder As CommunityFile, ByVal CommunityPath As String, ByVal FileName As List(Of String))
        //    Dim oItems As List(Of CommunityFile)
        //    oItems = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.FolderId = oFolder.Id AndAlso f.isPersonal = oFolder.isPersonal AndAlso f.CommunityOwner Is oFolder.CommunityOwner Select f).ToList

        //    Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oItems.Contains(fa.File) Select fa).ToList
        //    For Each Assigment In oAssigments
        //        DC.GetCurrentSession.Delete(Assigment)
        //    Next

        //    DeleteFilesTransfer((From f In oItems Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f).ToList())

        //    For Each oItem In (From f In oItems Where f.isFile Select f).ToList
        //        Dim oFile As CommunityFile = oItem
        //        FileName.Add(CommunityPath & oFile.UniqueID.ToString & ".stored")
        //        'If oFile.isSCORM Then
        //        '    Dim oScorm As ScormFile = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() Where sf.File Is oFile AndAlso sf.FileUniqueID = oFile.UniqueID Select sf).FirstOrDefault
        //        '    If Not IsNothing(oScorm) Then
        //        '        oScorm.Status = ScormStatus.Deleting
        //        '        DC.GetCurrentSession.SaveOrUpdate(oScorm)
        //        '    End If
        //        'End If

        //        Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

        //        For Each oDiaryFile In oDiaryFiles
        //            DC.GetCurrentSession.Delete(oDiaryFile)
        //        Next

        //        Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where ef.FileCommunity Is oFile Select ef).ToList

        //        For Each oWorkBookFile In oWorkBookFiles
        //            DC.GetCurrentSession.Delete(oWorkBookFile)
        //        Next

        //        DC.GetCurrentSession.Delete(oFile)
        //    Next

        //    For Each oFolder In (From f In oItems Where Not f.isFile Select f).ToList
        //        DeleteFolderContent(oFolder, CommunityPath, FileName)
        //        DC.GetCurrentSession.Delete(oFolder)
        //    Next

        //End Sub

        //Public Function DeleteRepository(ByVal isPersonal As Boolean, ByVal oCommunity As Community, ByVal CommunityPath As String) As List(Of dtoDeletedItem)
        //    Dim oResponse As New List(Of dtoDeletedItem)
        //    Try
        //        DC.BeginTransaction()
        //        oResponse = DeleteCommunityRepository(isPersonal, oCommunity, CommunityPath)
        //    Catch ex As Exception
        //        oResponse.Clear()
        //        If DC.isInTransaction Then
        //            DC.Rollback()
        //        End If
        //    End Try
        //    Return oResponse
        //End Function
        //Private Function DeleteCommunityRepository(ByVal isPersonal As Boolean, ByVal oCommunity As Community, ByVal CommunityPath As String) As List(Of dtoDeletedItem)
        //    Dim oResponse As New List(Of dtoDeletedItem)
        //    Try
        //        Dim oItems As List(Of CommunityFile)
        //        oItems = (From f In DC.GetCurrentSession.Linq(Of CommunityFile)() Where f.CommunityOwner Is oCommunity AndAlso f.isPersonal = isPersonal Select f).ToList

        //        Dim FilesName As List(Of String) = (From f In oItems Where f.isFile Select CommunityPath & f.UniqueID.ToString & ".stored").ToList

        //        Dim list As List(Of System.Guid) = (From f In oItems Where f.isFile AndAlso f.RepositoryItemType <> RepositoryItemType.FileStandard Select f.UniqueID).ToList
        //        'Dim Query = (From sf In DC.GetCurrentSession.Linq(Of ScormFile)() _
        //        '             Where ScormsID.Contains(sf.Id) Select sf)

        //        oResponse.AddRange((From f In oItems Where f.FolderId = 0 Select New dtoDeletedItem(f.Id, f.DisplayName, f.isFile, f.FolderId, "", f.RepositoryItemType)).ToList)
        //        DeleteFilesTransfer(list)
        //        'For Each oItem As ScormFile In Query.ToList
        //        '    oItem.Status = ScormStatus.Deleting
        //        '    DC.GetCurrentSession.SaveOrUpdate(oItem)
        //        'Next

        //        Dim oAssigments As List(Of CommunityFileAssignment) = (From fa In DC.GetCurrentSession().Linq(Of CommunityFileAssignment)() Where oItems.Contains(fa.File) Select fa).ToList
        //        For Each Assigment In oAssigments
        //            DC.GetCurrentSession.Delete(Assigment)
        //        Next

        //        Dim FileItems As List(Of CommunityFile) = (From f In oItems Where f.isFile Select f).ToList
        //        Dim oDiaryFiles As List(Of EventCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of EventCommunityFile)() Where FileItems.Contains(ef.FileCommunity) Select ef).ToList
        //        For Each oDiaryFile In oDiaryFiles
        //            DC.GetCurrentSession.Delete(oDiaryFile)
        //        Next

        //        Dim oWorkBookFiles As List(Of WorkBookCommunityFile) = (From ef In DC.GetCurrentSession().Linq(Of WorkBookCommunityFile)() Where FileItems.Contains(ef.FileCommunity) Select ef).ToList
        //        For Each oWorkBookFile In oWorkBookFiles
        //            DC.GetCurrentSession.Delete(oWorkBookFile)
        //        Next

        //        For Each oItem As CommunityFile In oItems
        //            DC.GetCurrentSession.Delete(oItem)
        //        Next
        //        DC.Commit()

        //        Delete.Files(FilesName)
        //    Catch ex As Exception
        //        Throw New Exception
        //    End Try
        //    Return oResponse
        //End Function
        #endregion

        #region "Publish From Module"
        public List<BaseCommunityFile> GetRepositoryItems(List<long> IdFiles)
        {
            return GetRepositoryItemsById(IdFiles, false);
        }
        public List<BaseCommunityFile> GetModuleFiles(List<long> IdFiles)
        {
            return GetRepositoryItemsById(IdFiles, true);
        }
        private List<BaseCommunityFile> GetRepositoryItemsById(List<long> IdFiles, Boolean forModule)
        {
            return (from BaseCommunityFile item in Manager.GetAll<BaseCommunityFile>(item => item.IsInternal == forModule && IdFiles.Contains(item.Id))
                    select item).ToList();
        }
        public List<BaseCommunityFile> GetFilesFromModuleLinks(List<long> IdModuleLinks)
        {
            List<ModuleLink> links = (from ml in Manager.GetAll<ModuleLink>(ml => IdModuleLinks.Contains(ml.Id)
                && (ml.DestinationItem.FQN == typeof(ModuleLongInternalFile).FullName
                || ml.DestinationItem.FQN == typeof(ModuleGuidInternalFile).FullName))
                                      select ml).ToList();

            List<long> IdFiles = (from l in links select l.DestinationItem.ObjectLongID).ToList();
            return (from BaseCommunityFile mif in Manager.GetAll<BaseCommunityFile>(mif => IdFiles.Contains(mif.Id))
                    select mif).ToList();
        }
        public List<dtoFileExist<long>> PublishFilesIntoRepository(String BaseFilePath, int IdCategory, Community community, long IdFolder, List<dtoModuleFileToPublish> dtoFiles, ref List<BaseCommunityFile> addedFiles)
        {
            List<dtoFileExist<long>> filesToRename = new List<dtoFileExist<long>>();
            List<long> IdFiles = (from d in dtoFiles select d.FileID).ToList();
            List<BaseCommunityFile> moduleFiles = (from f in Manager.GetAll<BaseCommunityFile>(f => IdFiles.Contains(f.Id)) select f).ToList();
            if (!BaseFilePath.EndsWith("\\"))
                BaseFilePath += "\\";

            String DestinationFolderPath = BaseFilePath + (community == null ? 0 : community.Id).ToString() + "\\";
            foreach (BaseCommunityFile moduleFile in moduleFiles)
            {
                dtoModuleFileToPublish dto = (from d in dtoFiles where d.FileID == moduleFile.Id select d).FirstOrDefault();
                if (FindRepositoryFile(community, IdFolder, dto.FileName, moduleFile.Extension) == null)
                {
                    addedFiles.Add(PublishFileIntoRepository(moduleFile, community, IdFolder, dto, BaseFilePath, DestinationFolderPath, IdCategory));
                }
                else
                    filesToRename.Add(new dtoFileExist<long>() { Id = moduleFile.Id, ExistFileName = dto.FileName, Extension = dto.Extension, ProposedFileName = ProposedRepositoryItemName(moduleFile, community, IdFolder) });
            }
            return filesToRename;
        }

        private BaseCommunityFile PublishFileIntoRepository(BaseCommunityFile moduleFile, Community community, long IdFolder, dtoModuleFileToPublish dto, String BaseFilePath, String DestinationFolderPath, int IdCategory)
        {
            BaseCommunityFile importedFile = null;
            CommunityFile fileToSave = CreateRepositoryItem(moduleFile, community, IdFolder, dto.FileName, Manager.GetPerson(UC.CurrentUserID), dto.IsVisible);

            String sourcefile = BaseFilePath + (moduleFile.CommunityOwner == null ? 0 : moduleFile.CommunityOwner.Id).ToString() + "\\" + moduleFile.UniqueID.ToString() + ".stored";
            String destinationFile = DestinationFolderPath + fileToSave.UniqueID.ToString() + ".stored";
                            Create.Directory (DestinationFolderPath);
            if (Exists.File(sourcefile))
            {
                Create.CopyFile (sourcefile, destinationFile);
                ItemRepositoryStatus status = ItemRepositoryStatus.None;
                importedFile = AddFileToCommunityRepository(fileToSave, DestinationFolderPath, (long)CoreModuleRepository.Base2Permission.DownloadFile, ref status);
                if (importedFile == null)
                    Delete.File(destinationFile);
            }
            return importedFile;
        }

        #endregion

        #region "Search Files"
            public CommunityFile FindDuplicatedRepositoryItem(Community community, long IdFolder, BaseCommunityFile item)
            {
                return FindRepositoryItem(community, IdFolder, item.Name, item.Extension, item.isFile);
            }
            public CommunityFile FindRepositoryFile(Community community, long IdFolder, String name, String extension)
            {
                return FindRepositoryItem(community, IdFolder, name, extension, true);
            }
            public CommunityFile FindRepositoryFolder(Community community, long IdFolder, String name)
            {
                return FindRepositoryItem(community, IdFolder, name, "", false);
            }
            private CommunityFile FindRepositoryItem(Community community, long IdFolder, String name, String extension, Boolean isFile)
            {
                var query = (from f in Manager.GetIQ<CommunityFile>() where f.CommunityOwner == community && f.FolderId == IdFolder && f.isFile == isFile select f);

                return (from f in query.ToList()
                        where f.Name.ToLowerInvariant() == name.ToLowerInvariant() && (!isFile || (isFile && extension.ToLowerInvariant() == f.Extension.ToLowerInvariant()))
                        select f).FirstOrDefault();
            }
            public String ProposedRepositoryItemName(BaseCommunityFile item, Community community, long IdFolder)
            {
                String proposedName = item.Name;
                var query = (from f in Manager.GetIQ<CommunityFile>() where f.CommunityOwner == community && f.FolderId == IdFolder && f.isFile == item.isFile select f);
                List<String> names = (from f in query.ToList()
                                      where f.Name.ToLowerInvariant() == item.Name.ToLowerInvariant() && (!item.isFile || (item.isFile && item.Extension.ToLowerInvariant() == f.Extension.ToLowerInvariant()))
                                      select f.Name).ToList();

                int i = 1;
                while (names.Contains(proposedName))
                {
                    proposedName = item.Name + " [" + i.ToString() + "]";
                    i += 1;
                }
                return proposedName;
            }
            public void VerifyAndUpdateItemName(BaseCommunityFile item)
            {
                String proposedName = item.Name;
                var query = (from f in Manager.GetIQ<CommunityFile>() where f.CommunityOwner == item.CommunityOwner && f.FolderId == item.FolderId && f.IsInternal == item.IsInternal && f.isFile == item.isFile select f);
                List<String> names = (from f in query.ToList()
                                      where f.Name.ToLowerInvariant() == item.Name.ToLowerInvariant() && (!item.isFile || (item.isFile && item.Extension.ToLowerInvariant() == f.Extension.ToLowerInvariant()))
                                      select f.Name).ToList();

                int i = 1;
                while (names.Contains(proposedName))
                {
                    proposedName = item.Name + " [" + i.ToString() + "]";
                    i += 1;
                }
                item.Name = proposedName;
            }

            public Boolean HasItemType(Int32 idCommunity, Int32 idPerson,  Boolean showAlsoHiddenItems, Boolean adminPurpose, RepositoryItemType type) {
                Boolean result = false;
                try
                {
                    Manager.BeginTransaction();
                    Community community = Manager.GetCommunity(idCommunity);
                    Person person = Manager.GetPerson(idPerson);
                    if (person != null) {
                        Role role = null;
                        if (community !=null && idCommunity>0)
                            role = (from Subscription s in Manager.GetIQ<Subscription>()
                                     where s.Community == community && s.Person == person
                                     select (Role)s.Role).Skip(0).Take(0).ToList().FirstOrDefault();

                        if (adminPurpose)
                            result = (from f in Manager.GetIQ<CommunityFile>()
                                      where ((f.CommunityOwner == community && idCommunity > 1) || (idCommunity == 0 && f.CommunityOwner == null)) && (showAlsoHiddenItems || (f.isVisible || f.Owner == person))
                                      && f.RepositoryItemType == type
                                      select f.Id).Any();
                        else
                            result = HasItemType(community, person, showAlsoHiddenItems, adminPurpose, type, role);
                    }

                    Manager.Commit();
                }
                catch (Exception ex) { 
                
                }
                return result;
            }

            private Boolean HasItemType(Community community, Person person, Boolean showAlsoHiddenItems, Boolean adminPurpose, RepositoryItemType type, Role role)
            {
                Boolean result = false;
                Int32 personIdType = person.TypeID;

                List<long> idItems = (from f in Manager.GetIQ<CommunityFile>()
                                        where f.CommunityOwner == community && f.RepositoryItemType==type
                                            && (showAlsoHiddenItems || (f.isVisible || f.Owner == person))
                                        select f.Id).ToList();
                Int32 index= 0;
                List<long> tmp = idItems.Skip(index++ *100).Take(100).ToList();
                while (tmp!=null && tmp.Any()){
                    result =(from fa in Manager.GetIQ<CommunityFileCommunityAssignment>() 
                             where tmp.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == community && !fa.Deny select fa.Id).Any();
                    if (!result & role !=null){
                        result = (from fa in Manager.GetIQ<CommunityFileRoleAssignment>()
                                where tmp.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == role && !fa.Deny 
                               select fa.Id).Any();
                    }
                    if (!result & role ==null){
                        result = (from fa in Manager.GetIQ<CommunityFilePersonTypeAssignment>()
                                where tmp.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == personIdType && !fa.Deny 
                               select fa.Id).Any();
                    }
                    if (!result)
                        result = (from fa in Manager.GetIQ<CommunityFilePersonAssignment>()
                                where tmp.Contains(fa.File.Id) && fa.Inherited && fa.AssignedTo == person && !fa.Deny 
                               select fa.Id).Any();
                    if (!result)
                        tmp = idItems.Skip(index++ *100).Take(100).ToList();
                    else
                        tmp = null;
                }
               return result;
            }
        #endregion

        #region "Generic"
            public CommunityFile AddFileToCommunityRepository(CommunityFile file, String communityPath, long permission, ref ItemRepositoryStatus status)
            {
                return AddFileToCommunityRepository(file, communityPath, permission, true, ref status);
            }
            public CommunityFile AddFileToCommunityRepository(CommunityFile file, String communityPath, long permission, Boolean isInTransaction, ref ItemRepositoryStatus status)
            {
                CommunityFile created = null;
                try
                {
                    if (isInTransaction)
                        Manager.BeginTransaction();
                    VerifyAndUpdateItemName(file);
                    Manager.SaveOrUpdate(file);
                    AddCommunityAssignmentToItem(file.CreatedOn, file.Owner, file.Id, false, permission, false);
                    if (file.FolderId == 0)
                        AddCommunityAssignmentToItem(file.CreatedOn, file.Owner, file.Id, false, permission, true);
                    else
                        ApplyInheritedAssignment(file.CreatedOn, file.Owner, file, (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File.Id == file.FolderId && fa.Inherited select fa).ToList(), false, permission);

                    if (file.isFile && file.RepositoryItemType != DomainModel.Repository.RepositoryItemType.FileStandard)
                        AddFileForTransfer(file, communityPath);
                    created = file;
                    status = ItemRepositoryStatus.FileUploaded;
                    if (isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    created = null;
                    if (isInTransaction)
                        Manager.RollBack();
                    if (status != ItemRepositoryStatus.FileExist)
                        status = ItemRepositoryStatus.CreationError;
                }
                return created;
            }

            private void AddFileForTransfer(BaseCommunityFile file, String communityPath)
            {

                FileTransfer fileTransfer = new FileTransfer();
                fileTransfer.FileUniqueID = file.UniqueID;
                fileTransfer.File = file;
                fileTransfer.CloneId = file.CloneUniqueID;
                if (communityPath.EndsWith("\\"))
                    fileTransfer.Path = communityPath;
                else
                    fileTransfer.Path = communityPath + "\\";
                fileTransfer.Filename = file.UniqueID.ToString() + ".stored";

                switch (file.RepositoryItemType)
                {
                    case DomainModel.Repository.RepositoryItemType.ScormPackage:
                        ScormFileTransfer scorm = new ScormFileTransfer(fileTransfer);
                        scorm.Decompress = true;
                        scorm.isCompleted = false;
                        Manager.SaveOrUpdate(scorm);
                        break;
                    case DomainModel.Repository.RepositoryItemType.Multimedia:
                        MultimediaFileTransfer multimedia = new MultimediaFileTransfer(fileTransfer);
                        multimedia.Decompress = true;
                        multimedia.isCompleted = false;
                        Manager.SaveOrUpdate(multimedia);
                        break;
                }
            }
            private CommunityFile CreateRepositoryItem(BaseCommunityFile moduleFile, Community community, long IdFolder, String NewName, Person owner, Boolean isVisible)
            {
                CommunityFile file = new CommunityFile();
                file.FolderId = IdFolder;
                file.CreatedOn = DateTime.Now;
                file.Description = "";
                file.Extension = moduleFile.Extension;
                file.isFile = moduleFile.isFile;
                file.IsInternal = false;
                file.CloneID = moduleFile.Id;
                file.CloneUniqueID = moduleFile.UniqueID;
                file.CommunityOwner = community;
                file.ContentType = moduleFile.ContentType;
                file.isSCORM = moduleFile.isSCORM;
                file.isVideocast = moduleFile.isVideocast;
                file.isVirtual = moduleFile.isVirtual;
                file.isVisible = isVisible;
                file.ModifiedBy = owner;
                file.ModifiedOn = file.CreatedOn;
                file.RepositoryItemType = moduleFile.RepositoryItemType;
                file.Name = NewName;
                file.Owner = owner;
                file.Size = moduleFile.Size;
                file.UniqueID = System.Guid.NewGuid();
                file.IsDownloadable = moduleFile.IsDownloadable;
                file.FileCategoryID = moduleFile.FileCategoryID;
                file.Downloads = 0;
                return file;
            }
            
            public BaseCommunityFile CloneFile(object owner,Int32 idCommunityOwner, BaseCommunityFile sourceFile,String sourcePath,Person person){
                Boolean isInTransaction = Manager.IsInTransaction();
                BaseCommunityFile dFile = null;
                String dFilePath = "";
                String sFilePath = "";
                try{
                    if (!isInTransaction)
                        Manager.BeginTransaction();

                    Int32 idCommunity = 0;
                    String communityPath = sourcePath;
                    if (!sourcePath.EndsWith("\\"))
                        sourcePath +="\\";
                    if (sourceFile.IsInternal){
                        dFile = new ModuleLongInternalFile();
                        dFile.CloneID = sourceFile.Id;
                        dFile.ContentType = sourceFile.ContentType;
                        dFile.Description =  sourceFile.Description;
                        dFile.Downloads=0;
                        dFile.Description =  sourceFile.Description;
                        dFile.FileCategoryID =  sourceFile.FileCategoryID;
                        dFile.FolderId =  sourceFile.FolderId;
                        dFile.isDeleted =  sourceFile.isDeleted;
                        dFile.isPersonal =  sourceFile.isPersonal;
                        dFile.isSCORM =  sourceFile.isSCORM;
                        dFile.isVirtual =  sourceFile.isVirtual;
                        dFile.isVideocast =  sourceFile.isVideocast;
                        dFile.RepositoryItemType =  sourceFile.RepositoryItemType;
                        dFile.IsDownloadable =  sourceFile.IsDownloadable;
                        dFile.isVisible =  sourceFile.isVisible;
                        dFile.isFile =  sourceFile.isFile;
                        dFile.Name =  sourceFile.Name;
                        dFile.Size =  sourceFile.Size;
                        dFile.UniqueID = System.Guid.NewGuid();
                        dFile.Extension =  sourceFile.Extension;
                        dFile.IsInternal =  sourceFile.IsInternal;
                        dFile.Owner = person;
                        dFile.CreatedOn= DateTime.Now;
                        dFile.ModifiedOn= dFile.CreatedOn;
                        dFile.ModifiedBy= person;
                        dFile.CommunityOwner = Manager.GetCommunity(idCommunityOwner);
                        ((ModuleLongInternalFile)dFile).ServiceActionAjax = ((ModuleLongInternalFile)sourceFile).ServiceActionAjax;
                        ((ModuleLongInternalFile)dFile).ServiceOwner = ((ModuleLongInternalFile)sourceFile).ServiceOwner;
                        ((ModuleLongInternalFile)dFile).ObjectTypeID = ((ModuleLongInternalFile)sourceFile).ObjectTypeID;
                        ((ModuleLongInternalFile)dFile).ObjectOwner = owner;
                        idCommunity = (sourceFile.CommunityOwner== null || sourceFile.CommunityOwner.Id<1) ? 0 : sourceFile.CommunityOwner.Id;

                        sFilePath = sourcePath + idCommunity.ToString() + "\\" + sourceFile.UniqueID.ToString() + ".stored";
                        dFilePath = sourcePath + idCommunityOwner.ToString() + "\\" + dFile.UniqueID.ToString() + ".stored";
                        if (lm.Comol.Core.File.Exists.File(sFilePath))
                        {
                            if (!lm.Comol.Core.File.Create.CopyFile(sFilePath, dFilePath))
                                dFile = null;
                            else
                                Manager.SaveOrUpdate(dFile);
                        }
                        else
                            dFile = null;
                    }
                    
                    if (dFile!=null && dFile.RepositoryItemType != DomainModel.Repository.RepositoryItemType.Folder && dFile.RepositoryItemType != DomainModel.Repository.RepositoryItemType.FileStandard){
                        AddFileForTransfer(dFile,sourcePath + idCommunityOwner.ToString() + "\\");
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
               
                catch(Exception ex){
                    if (!isInTransaction && Manager.IsInTransaction())
                        Manager.RollBack();
                    dFile = null;
                }
                if (dFilePath != "" && dFile == null)
                    lm.Comol.Core.File.Delete.File(dFilePath);
                return dFile;
            }

        //Private Function AddFileTocommunityRepository(ByVal oFile As BaseFile, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal FileName As String, ByVal isVisible As Boolean, ByVal UserRepositoryFilePath As String, ByVal DefaultFileCategoryID As Integer) As Boolean
        //    Dim oCommunityFile As CommunityFile = Me.CreateCommunityFile(oFile, FolderID, CommunityID, FileName, DefaultFileCategoryID)
        //    Try
        //        Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
        //        oCommunityFile.isVisible = isVisible
        //        AddedFile = Me.RepositoryManager.AddFile(oCommunityFile, UserRepositoryFilePath & oFile.Id.ToString & ".stored", UCServices.Services_File.Base2Permission.DownloadFile, fileStatus)
        //        If Not IsNothing(AddedFile) Then
        //            Dim ParentFolder As String = Me.View.BaseFolder
        //            If AddedFile.FolderId > 0 Then
        //                ParentFolder = Me.RepositoryManager.GetFolderName(AddedFile.FolderId)
        //            End If
        //            Me.View.Notify(CommunityID, Me.ModuleID, AddedFile, ParentFolder)
        //        End If
        //End Function

        //Private Function CreateCommunityFile(ByVal oFile As BaseFile, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal FileName As String, ByVal CategoryID As Integer) As CommunityFile
        //    Dim oCommunityFile As New CommunityFile
        //    With oCommunityFile
        //        .CloneID = 0
        //        .ContentType = oFile.ContentType
        //        .Description = oFile.Description
        //        .DisplayOrder = 1
        //        .Downloads = 0
        //        .FileCategoryID = CategoryID
        //        .FolderId = FolderID
        //        .isDeleted = False
        //        .isPersonal = False
        //        .isSCORM = False
        //        .isVirtual = False
        //        .isVideocast = False
        //        .IsDownloadable = True
        //        .isVisible = False
        //        .isFile = True
        //        .Level = 0
        //        .Name = FileName
        //        .Size = oFile.Size
        //        .UniqueID = System.Guid.NewGuid
        //        .Extension = oFile.Extension
        //        .CommunityOwner = Me.CommonManager.GetCommunity(CommunityID)
        //        If Not String.IsNullOrEmpty(.Extension) Then
        //            .Extension = .Extension.ToLower
        //        End If
        //    End With
        //    Return oCommunityFile
        //End Function
        #endregion
        public Boolean UpdateFolderPermission(int userId, CommunityFile item, Boolean ToSubitems, long permission)
        {
            Boolean response = false;
            try
            {
                Manager.BeginTransaction();
                DateTime editOn = DateTime.Now;
                Person editBy = Manager.GetPerson(userId);
                if (editBy != null)
                    response = UpdateFoldersPermission(editOn, editBy, item, ToSubitems, permission);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return response;
        }
        public Boolean UpdateFoldersPermission(DateTime editOn, Person person, CommunityFile item, Boolean ToSubitems, long permission)
        {
            Boolean response = false;
            try
            {
                List<CommunityFileAssignment> assigments = (from CommunityFileAssignment f in Manager.Linq<CommunityFileAssignment>()
                                                            where f.File == item
                                                            select f).ToList();
                foreach (CommunityFileAssignment assigment in assigments)
                {
                    assigment.Permission = permission;
                    assigment.ModifiedOn = editOn;
                    assigment.ModifiedBy = person;
                    Manager.SaveOrUpdate(assigment);
                }
                response = true;
                if (ToSubitems)
                {
                    List<CommunityFile> items = (from f in Manager.Linq<CommunityFile>() where f.FolderId == item.Id select f).ToList();
                    foreach (CommunityFile file in items)
                    {
                        response = response && UpdateFoldersPermission(editOn, person, file, ToSubitems, permission);
                    }
                }
            }
            catch (Exception ex)
            {
                response = false;
            }
            return response;
        }

        #region "Add Assignment"
        public bool SetOtherAssignmentToItems(int ByUserID, List<long> ItemsId, List<int> RolesID, List<int> MembersID, Boolean Deny, Boolean ForPortal, Boolean ToSubitems, long Permission)
        {
            Boolean iResponse = false;
            try
            {
                Manager.BeginTransaction();

                DateTime CreatedOn = DateTime.Now;
                Person CreatedBy = Manager.GetPerson(ByUserID);
                List<CommunityFile> items = (from f in Manager.GetIQ<CommunityFile>() where ItemsId.Contains(f.Id) select f).ToList();
                if ((CreatedBy != null))
                {
                    foreach (CommunityFile item in items)
                    {
                        CommunityFile oFileItem = item;
                        ApplyAssignmentToOthers(CreatedOn, CreatedBy, item.FolderId, item, RolesID, MembersID, Deny, ToSubitems, Permission);

                        long FolderID = item.FolderId;
                        List<CommunityFileAssignment> oAssignments = null;

                        if (FolderID == 0)
                        {
                            oAssignments = (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File == oFileItem && !fa.Inherited select fa).ToList();
                        }
                        else
                        {
                            oAssignments = (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File.Id == FolderID && fa.Inherited select fa).ToList();
                        }
                        ApplyInheritedAssignment(CreatedOn, CreatedBy, item, oAssignments, Deny, Permission);
                    }
                }
                Manager.Commit();
                iResponse = true;
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();

            }
            return iResponse;
        }
        private void ApplyAssignmentToOthers(DateTime CreatedOn, Person CreatedBy, long FatherID, CommunityFile item, List<int> RolesID, List<int> MembersID, bool Deny, bool ToSubitems, long Permission)
        {
            try
            {
                List<CommunityFileAssignment> OldAssignments = (from fa in Manager.GetIQ<CommunityFileAssignment>()
                                                                where fa.File == item && (fa.Deny == Deny || fa.CreatedBy == null || fa.ModifiedBy == null)
                                                                select fa).ToList();
                Manager.DeletePhysicalList(OldAssignments);

                if (item.CommunityOwner == null)
                {
                    this.AddPersonTypesAssignmentToItem(CreatedOn, CreatedBy, item.Id, RolesID, Deny, Permission, false);
                }
                else
                {
                    this.AddRolesAssignmentToItem(CreatedOn, CreatedBy, item.Id, RolesID, Deny, Permission, false);
                }
                this.AddPersonsAssignmentToItem(CreatedOn, CreatedBy, item.Id, MembersID, Deny, Permission, false);

                List<CommunityFile> oItemsToApply = new List<CommunityFile>();
                if (!item.isFile && ToSubitems)
                {
                    oItemsToApply = (from f in Manager.GetIQ<CommunityFile>() where f.FolderId == item.Id select f).ToList();
                    if (oItemsToApply.Count > 0)
                    {
                        RecursiveApplyAssignmentToOther(CreatedOn, CreatedBy, item.Id, oItemsToApply, RolesID, MembersID, Deny, ToSubitems, Permission);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        private void RecursiveApplyAssignmentToOther(DateTime CreatedOn, Person CreatedBy, long FatherID, List<CommunityFile> Items, List<int> RolesID, List<int> MembersID, bool Deny, bool ToSubitems, long Permission)
        {
            try
            {
                foreach (CommunityFile item in Items)
                {
                    this.ApplyAssignmentToOthers(CreatedOn, CreatedBy, FatherID, item, RolesID, MembersID, Deny, ToSubitems, Permission);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        private List<CommunityFileCommunityAssignment> AddCommunityAssignmentToItems(DateTime CreatedOn, Person CreatedBy, List<long> ItemsId, bool Deny, long Permission, bool Inherited)
        {
            List<CommunityFileCommunityAssignment> iResponse = new List<CommunityFileCommunityAssignment>();
            try
            {
                foreach (long IdItem in ItemsId)
                {
                    CommunityFile oFile = Manager.Get<CommunityFile>(IdItem);
                    if ((oFile != null))
                    {
                        CommunityFileCommunityAssignment oAssignment = new CommunityFileCommunityAssignment();

                        oAssignment.AssignedTo = oFile.CommunityOwner;
                        oAssignment.CreatedBy = CreatedBy;
                        oAssignment.CreatedOn = CreatedOn;
                        oAssignment.Deny = Deny;
                        oAssignment.File = oFile;
                        oAssignment.ModifiedBy = CreatedBy;
                        oAssignment.ModifiedOn = CreatedOn;
                        oAssignment.Permission = Permission;
                        oAssignment.Inherited = Inherited;
                        Manager.SaveOrUpdate(oAssignment);
                        iResponse.Add(oAssignment);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }
        private List<CommunityFileCommunityAssignment> AddCommunityAssignmentToItem(DateTime CreatedOn, Person CreatedBy, long ItemId, bool Deny, long Permission, bool Inherited)
        {
            List<CommunityFileCommunityAssignment> iResponse = new List<CommunityFileCommunityAssignment>();
            try
            {
                CommunityFile oFile = Manager.Get<CommunityFile>(ItemId);
                if ((oFile != null))
                {
                    CommunityFileCommunityAssignment oAssignment = new CommunityFileCommunityAssignment();

                    oAssignment.AssignedTo = oFile.CommunityOwner;
                    oAssignment.CreatedBy = CreatedBy;
                    oAssignment.CreatedOn = CreatedOn;
                    oAssignment.Deny = Deny;
                    oAssignment.File = oFile;
                    oAssignment.ModifiedBy = CreatedBy;
                    oAssignment.ModifiedOn = CreatedOn;
                    oAssignment.Permission = Permission;
                    oAssignment.Inherited = Inherited;
                    Manager.SaveOrUpdate(oAssignment);
                    iResponse.Add(oAssignment);
                }
            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }
        private List<CommunityFileRoleAssignment> AddRolesAssignmentToItem(DateTime CreatedOn, Person CreatedBy, long ItemID, List<int> RolesID, bool Deny, long Permission, bool Inherited)
        {
            List<CommunityFileRoleAssignment> iResponse = new List<CommunityFileRoleAssignment>();
            try
            {
                CommunityFile oFile = Manager.Get<CommunityFile>(ItemID);
                if ((oFile != null))
                {
                    foreach (int IdRole in RolesID)
                    {
                        CommunityFileRoleAssignment oAssignment = new CommunityFileRoleAssignment();
                        Role oRole = Manager.Get<Role>(IdRole);
                        oAssignment.AssignedTo = oRole;
                        oAssignment.CreatedBy = CreatedBy;
                        oAssignment.CreatedOn = CreatedOn;
                        oAssignment.Deny = Deny;
                        oAssignment.File = oFile;
                        oAssignment.ModifiedBy = CreatedBy;
                        oAssignment.ModifiedOn = CreatedOn;
                        oAssignment.Permission = Permission;
                        oAssignment.Inherited = Inherited;
                        Manager.SaveOrUpdate(oAssignment);
                        iResponse.Add(oAssignment);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }
        private CommunityFilePersonAssignment AddPersonAssignmentToItem(DateTime CreatedOn, Person CreatedBy, CommunityFile oFile, Person oPerson, bool Deny, long Permission, bool Inherited)
        {
            CommunityFilePersonAssignment oAssignment = new CommunityFilePersonAssignment();
            oAssignment.AssignedTo = oPerson;
            oAssignment.CreatedBy = CreatedBy;
            oAssignment.CreatedOn = CreatedOn;
            oAssignment.Deny = Deny;
            oAssignment.File = oFile;
            oAssignment.ModifiedBy = CreatedBy;
            oAssignment.ModifiedOn = CreatedOn;
            oAssignment.Permission = Permission;
            oAssignment.Inherited = Inherited;
            Manager.SaveOrUpdate(oAssignment);

            return oAssignment;
        }
        private List<CommunityFilePersonAssignment> AddPersonsAssignmentToItem(DateTime CreatedOn, Person CreatedBy, long ItemId, List<int> MembersID, bool Deny, long Permission, bool Inherited)
        {
            List<CommunityFilePersonAssignment> iResponse = new List<CommunityFilePersonAssignment>();
            try
            {
                CommunityFile oFile = Manager.Get<CommunityFile>(ItemId);
                if ((oFile != null))
                {
                    foreach (int IdPerson in MembersID)
                    {
                        CommunityFilePersonAssignment oAssignment = new CommunityFilePersonAssignment();
                        Person oPerson = Manager.GetPerson(IdPerson);
                        oAssignment.AssignedTo = oPerson;
                        oAssignment.CreatedBy = CreatedBy;
                        oAssignment.CreatedOn = CreatedOn;
                        oAssignment.Deny = Deny;
                        oAssignment.File = oFile;
                        oAssignment.ModifiedBy = CreatedBy;
                        oAssignment.ModifiedOn = CreatedOn;
                        oAssignment.Permission = Permission;
                        oAssignment.Inherited = Inherited;
                        Manager.SaveOrUpdate(oAssignment);
                        iResponse.Add(oAssignment);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return iResponse;
        }
        private List<CommunityFilePersonTypeAssignment> AddPersonTypesAssignmentToItem(DateTime CreatedOn, Person CreatedBy, long ItemId, List<int> TypesID, bool Deny, long Permission, bool Inherited)
        {
            List<CommunityFilePersonTypeAssignment> iResponse = new List<CommunityFilePersonTypeAssignment>();
            try
            {
                CommunityFile oFile = Manager.Get<CommunityFile>(ItemId);
                if ((oFile != null))
                {
                    foreach (int IdUserType in TypesID)
                    {
                        CommunityFilePersonTypeAssignment oAssignment = new CommunityFilePersonTypeAssignment();
                        oAssignment.AssignedTo = IdUserType;
                        oAssignment.CreatedBy = CreatedBy;
                        oAssignment.CreatedOn = CreatedOn;
                        oAssignment.Deny = Deny;
                        oAssignment.File = oFile;
                        oAssignment.ModifiedBy = CreatedBy;
                        oAssignment.ModifiedOn = CreatedOn;
                        oAssignment.Permission = Permission;
                        oAssignment.Inherited = Inherited;
                        Manager.SaveOrUpdate(oAssignment);
                        iResponse.Add(oAssignment);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }

        private void ApplyInheritedAssignment(DateTime CreatedOn, Person CreatedBy, CommunityFile oItem, List<CommunityFileAssignment> oFatherAssignments, bool Deny, long Permission)
        {
            try
            {
                List<CommunityFileAssignment> OldAssignments = (from fa in Manager.GetIQ<CommunityFileAssignment>()
                                                                where fa.File == oItem && fa.Deny == Deny && fa.Inherited
                                                                select fa).ToList();
                Manager.DeletePhysicalList(OldAssignments);

                List<CommunityFileAssignment> oAssociated = (from fa in Manager.GetIQ<CommunityFileAssignment>()
                                                             where fa.File == oItem && fa.Deny == Deny && !fa.Inherited
                                                             select fa).ToList();

                bool ItemAllowCommunity = (from a in oAssociated where typeof(CommunityFileCommunityAssignment) == a.GetType() select a).Any();
                bool FatherAllowCommunity = (from a in oFatherAssignments where typeof(CommunityFileCommunityAssignment) == a.GetType() select a).Any();


                if (ItemAllowCommunity && FatherAllowCommunity)
                    AddCommunityAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, Deny, Permission, true);
                else
                {
                    List<int> FatherRoles = (from a in oFatherAssignments
                                             where a.GetType() == typeof(CommunityFileRoleAssignment)
                                             select ((CommunityFileRoleAssignment)a).AssignedTo.Id).ToList();
                    List<int> FatherPersons = (from a in oFatherAssignments
                                               where a.GetType() == typeof(CommunityFilePersonAssignment)
                                               select ((CommunityFilePersonAssignment)a).AssignedTo.Id).ToList();
                    List<int> FatherPersonTypes = (from a in oFatherAssignments
                                                   where a.GetType() == typeof(CommunityFilePersonTypeAssignment)
                                                   select ((CommunityFilePersonTypeAssignment)a).AssignedTo).ToList();

                    if (!ItemAllowCommunity)
                    {
                        var QueryRoles = (from a in oAssociated
                                          where a.GetType() == typeof(CommunityFileRoleAssignment)
                                          select ((CommunityFileRoleAssignment)a).AssignedTo.Id);
                        var QueryPersons = (from a in oAssociated
                                            where a.GetType() == typeof(CommunityFilePersonAssignment)
                                            select ((CommunityFilePersonAssignment)a).AssignedTo.Id);
                        var QueryTypes = (from a in oAssociated
                                          where a.GetType() == typeof(CommunityFilePersonTypeAssignment)
                                          select ((CommunityFilePersonTypeAssignment)a).AssignedTo);

                        if (FatherAllowCommunity)
                        {
                            FatherRoles = QueryRoles.ToList();
                            FatherPersonTypes = QueryTypes.ToList();
                            FatherPersons = QueryPersons.ToList();
                        }
                        else
                        {
                            FatherRoles = (from r in FatherRoles where (QueryRoles.ToList().Contains(r)) select r).ToList();
                            FatherPersonTypes = (from t in FatherPersonTypes where (QueryTypes.ToList().Contains(t)) select t).ToList();

                            List<int> ExludedUserFromItem = (from p in QueryPersons where !(FatherPersons.Contains(p)) select p).ToList();
                            List<int> ExludedUserFromFather = (from p in FatherPersons where !(QueryPersons.Contains(p)) select p).ToList();
                            FatherPersons = (from p in FatherPersons where (QueryPersons.ToList().Contains(p)) select p).ToList();

                            if (oItem.CommunityOwner == null)
                            {
                                var queryTypePerson = (from s in Manager.GetIQ<Person>() where (ExludedUserFromItem.Contains(s.Id)) select new { UserID = s.Id, TypeId = s.TypeID }).ToList();
                                FatherPersons.AddRange((from o in queryTypePerson join typeID in FatherPersonTypes on o.TypeId equals typeID select o.UserID).ToList());

                                queryTypePerson = (from s in Manager.GetIQ<Person>() where (ExludedUserFromFather.Contains(s.Id)) select new { UserID = s.Id, TypeId = s.TypeID }).ToList();
                                FatherPersons.AddRange((from o in queryTypePerson join typeID in QueryTypes on o.TypeId equals typeID select o.UserID).ToList());
                            }
                            else
                            {
                                var queryRolesPerson = (from s in Manager.GetIQ<Subscription>() where object.ReferenceEquals(s.Community, oItem.CommunityOwner) && (ExludedUserFromItem.Contains(s.Person.Id)) select new { UserID = s.Person.Id, RoleId = s.Role.Id }).ToList();
                                FatherPersons.AddRange((from o in queryRolesPerson join role in FatherRoles on o.RoleId equals role select o.UserID).ToList());

                                queryRolesPerson = (from s in Manager.GetIQ<Subscription>()
                                                    where s.Community == oItem.CommunityOwner && ExludedUserFromFather.Contains(s.Person.Id)
                                                    select new { UserID = s.Person.Id, RoleId = s.Role.Id }).ToList();
                                FatherPersons.AddRange((from o in queryRolesPerson join role in QueryRoles on o.RoleId equals role select o.UserID).ToList());
                            }
                        }
                    }
                    if (FatherPersons.Count > 0)
                    {
                        this.AddPersonsAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherPersons, Deny, Permission, true);
                    }
                    if (FatherPersonTypes.Count > 0)
                    {
                        this.AddPersonTypesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherPersonTypes, Deny, Permission, true);
                    }
                    if (FatherRoles.Count > 0)
                    {
                        this.AddRolesAssignmentToItem(CreatedOn, CreatedBy, oItem.Id, FatherRoles, Deny, Permission, true);
                    }

                }

                if (!oItem.isFile)
                {
                    List<CommunityFileAssignment> oUpdatedAssociated = (from fa in Manager.GetIQ<CommunityFileAssignment>() where fa.File == oItem && fa.Inherited select fa).ToList();
                    List<CommunityFile> oSubItems = (from f in Manager.GetIQ<CommunityFile>() where f.FolderId == oItem.Id select f).ToList();
                    foreach (CommunityFile subItem in oSubItems)
                    {
                        this.ApplyInheritedAssignment(CreatedOn, CreatedBy, subItem, oUpdatedAssociated, Deny, Permission);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        #endregion

        public BaseCommunityFile GetItem(long IdItem)
        {
            return Manager.Get<BaseCommunityFile>(IdItem);
        }
        
        #region "Transfer"
            public TransferStatus GetItemTransferStatus(System.Guid uniqueID)
            {
                try
                {
                    return (from f in Manager.GetIQ<FileTransfer>() where f.FileUniqueID == uniqueID select f.Status).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return TransferStatus.Error;
                }
            }
            public void SaveUserAccessToFile(String playSessionId, int idUser, BaseCommunityFile item, long IdLink)
            {
                try
                {
                    FilePlayInfo info = new FilePlayInfo();
                    Person person = Manager.GetPerson(idUser);
                    if (person != null)
                    {
                        info.WorkingSessionID = playSessionId;
                        info.Owner = person;
                        info.FileUniqueID = item.UniqueID;
                        info.File = item;
                        info.CreatedOn = DateTime.Now;
                        info.CommunityOwner = item.CommunityOwner;
                        info.DateZone = 1;
                        info.IdAction = 1;
                        info.RepositoryItemType = item.RepositoryItemType;
                        Manager.SaveOrUpdate(info);

                        if (item.RepositoryItemType == RepositoryItemType.ScormPackage)
                        {
                            ScormPackageToEvaluate evaluation = (from e in Manager.GetIQ<ScormPackageToEvaluate>()
                                                                 where e.FileUniqueID == item.UniqueID && e.IdPerson == idUser && e.IdLink == IdLink
                                                                 select e).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (evaluation == null)
                            {
                                evaluation = new ScormPackageToEvaluate();
                                evaluation.FileUniqueID = item.UniqueID;
                                evaluation.IdFile = item.Id;
                                evaluation.IdLink = IdLink;
                                evaluation.IsPlaying = false;
                                evaluation.ToUpdate = false;
                                evaluation.Deleted = BaseStatusDeleted.None;
                                evaluation.ModifiedOn = DateTime.Now;
                                evaluation.IdPerson = idUser;
                                Manager.SaveOrUpdate(evaluation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            public MultimediaFileTransfer GetMultimediaFileTransfer(BaseCommunityFile item)
            {
                MultimediaFileTransfer result = null;
                try
                {
                    result = (from mf in Manager.GetIQ<MultimediaFileTransfer>()
                              where mf.File == item
                              select mf).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public MultimediaFileTransfer GetMultimediaFileTransfer(long idItem)
            {
                MultimediaFileTransfer result = null;
                try
                {
                    result = (from mf in Manager.GetIQ<MultimediaFileTransfer>()
                              where mf.File != null && mf.File.Id == idItem
                              select mf).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            public MultimediaFileIndex SetMultimediaFileDefaultIndex(long IdFile, String path)
            {
                MultimediaFileIndex result = null;
                try
                {
                    BaseCommunityFile item = GetItem(IdFile);
                    MultimediaFileTransfer multimedia = GetMultimediaFileTransfer(item);
                    if (multimedia != null && item != null)
                    {
                        path = (item.UniqueID.ToString() + path).Replace("/", "\\");
                        result = (from m in multimedia.MultimediaIndexes
                                  where m.MultimediaFile == multimedia && m.Fullname == path
                                  select m).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (result != null)
                        {
                            Manager.BeginTransaction();
                            multimedia.DefaultDocument = result;
                            multimedia.DefaultDocumentPath = result.Fullname;
                            Manager.SaveOrUpdate(multimedia);
                            Manager.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                    result = null;
                }
                return result;
            }
        #endregion
    }
}