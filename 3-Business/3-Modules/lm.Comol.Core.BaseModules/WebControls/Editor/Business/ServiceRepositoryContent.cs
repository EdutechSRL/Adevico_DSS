using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;
using System.Web;

namespace lm.Comol.Core.BaseModules.Editor.Business
{
    public class ServiceRepositoryContent : CoreServices
    {
        protected iApplicationContext _Context;
        private String UserFolderName { get; set; }
        private String CommunityFolderName { get; set; }
        private const char pathSeparator = '/';
        private Int32 IdUser  { get; set; }
        private Int32 IdCommunity { get; set; }

        #region initClass
        //public ServiceRepositoryContent() : base() { }
        //public ServiceRepositoryContent(iApplicationContext oContext)
        //    : base(oContext.DataContext)
        //{
        //    _Context = oContext;
        //    this.UC = oContext.UserContext;
        //}
        //public ServiceRepositoryContent(iDataContext oDC) : base(oDC) { }
        public ServiceRepositoryContent(iApplicationContext oContext, Int32 idUser, Int32 idCommunity, String uFolder, String cFolder)
            : base(oContext.DataContext)
        {
            _Context = oContext;
            this.UC = oContext.UserContext;
            UserFolderName = uFolder;
            CommunityFolderName = cFolder;
            IdUser = idUser;
            IdCommunity = idCommunity;
        }
        #endregion
        public Int32 GetIdDefaultLanguage()
        {
            Language l = Manager.GetDefaultLanguage();
            return (l==null) ? Manager.GetAllLanguages().FirstOrDefault().Id : l.Id;
        }

        public String[] GetBaseFolders()
        {
            String[] result = null;
            try
            {
                List<EditorRepositoryItem> folders = (from f in Manager.GetIQ<EditorRepositoryItem>()
                                                      where f.Deleted == BaseStatusDeleted.None && (f.IdOwner == IdUser && IdUser > 0)
                                                      && f.Folder==null && f.IsDirectory 
                                                      select f).ToList();
                result = folders.Select(f => f.DisplayName).OrderBy(f => f).ToArray();
            }
            catch (Exception ex) {
                result = new String[] { "" }; 
            }
            return result;
        }

        public void UpdateItem(String path, String newPath)
        {
            String oldName = this.GetName(path);
            String newName = this.GetName(newPath);


            long? idItem = GetItemIdFromPath(path);

            if (idItem.HasValue && idItem.Value>0) {
                try
                {
                    Manager.BeginTransaction();
                    EditorRepositoryItem item = Manager.Get<EditorRepositoryItem>(idItem.Value);
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    if (p != null && item != null)
                    {
                        item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        if (oldName == newName)
                        {
                            EditorRepositoryItem newParent = this.GetParentFromPath(newPath);
                            if (newParent != null && newParent.Id > 0)
                                item.Folder = newParent;
                            else
                                item.Folder = null;
                        }
                        else
                            item.Name = newName;
                        Manager.SaveOrUpdate(item);
                    }
                    Manager.Commit();
                }
                catch (Exception ex) {
                    Manager.RollBack();
                }
            }
        }

        public EditorRepositoryItem GetItem(String path)
        {
            return this.GetItemRowFromPath(path);
        }
        public EditorRepositoryItem GetItem(System.Guid idItem)
        {
            EditorRepositoryItem item = null;
            try
            {
                item = (from f in Manager.GetIQ<EditorRepositoryItem>() where f.Identifyer == idItem select f).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            catch (Exception ex) { }
            return item;
        }

        //private void UpdateItem(long idItem, String[] fields, String[] values)
        //{
        //    if (fields.Length != values.Length)
        //        return;

        //    string updateCommandStr = "UPDATE [Items] SET";
        //    for (int i = 0; i < fields.Length; i++)
        //    {
        //        updateCommandStr += String.Format(" [{0}]='{1}'", fields[i], values[i]);
        //        if (i < fields.Length - 1)
        //            updateCommandStr += ",";
        //    }
        //    updateCommandStr += String.Format(" WHERE [ItemID] = {0}", itemId);

        //    SqlConnection connection = this.GetConnection(this.connectionString);

        //    using (connection)
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand(updateCommandStr, connection);
        //        command.ExecuteNonQuery();

        //        this._data = null; //force update
        //    }
        //}

        private EditorRepositoryItem GetParentFromPath(String path)
        {
            string parentPath = TrimSeparator(path);
            if (parentPath.Contains("/"))
            {
                parentPath = parentPath.Substring(0, parentPath.LastIndexOf(pathSeparator));
                return this.GetItemRowFromPath(parentPath);
            }
            else
                return new EditorRepositoryItem() { Id = 0, Name = "/", IsDirectory = true };
            
        }
        private long[] ConvertPathToIds(String path)
        {
            path = this.TrimSeparator(path);
            string[] names = path.Split('/');

            List<long> result = new List<long>(names.Length);

            long idItem = 0;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                List<EditorRepositoryItem> items = (from f in Manager.GetIQ<EditorRepositoryItem>()
                                                      where f.Deleted == BaseStatusDeleted.None && (f.IdOwner == IdUser  && IdUser>0)
                                                      && ((idItem == 0 && f.Folder == null) || (idItem > 0 && f.Folder != null && idItem == f.Folder.Id))
                                                    select f).ToList();

                items = items.Where(f => f.DisplayName == name).ToList();

                //&& f.Name == name && ((idFolder == 0 && f.Folder==null) || (idFolder > 0 && f.Folder !=null && idFolder == f.Folder.Id))
                if (items.Count > 0)
                {
                    result.Add(items[0].Id);
                    idItem = items[0].Id;
                }
            }

            return names.Length == result.Count ? result.ToArray() : null;
        }
        private EditorRepositoryItem GetItemRowFromPath(String path)
        {
            EditorRepositoryItem item = null;
            long? itemId = GetItemIdFromPath(path);
            if (itemId != null)
                item = Manager.Get<EditorRepositoryItem>(itemId.Value);

            return item;
        }
        private long? GetItemIdFromPath(String path)
        {
            long[] ancestors = this.ConvertPathToIds(path);

            return ancestors != null && ancestors.Length > 0 ? (long?)ancestors[ancestors.Length - 1] : null;
        }
        public void DeleteItem(String path)
        {
            try
            {
                Manager.BeginTransaction();
                Person p = Manager.GetPerson(UC.CurrentUserID);
                long? itemId = GetItemIdFromPath(path);
                if (itemId.HasValue && p != null)
                {
                    EditorRepositoryItem item = Manager.Get<EditorRepositoryItem>(itemId.Value);
                    if (item != null && item.IsDirectory)
                    {

                        item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        item.Deleted = BaseStatusDeleted.Manual;
                        Manager.SaveOrUpdate(item);
                    }
                    else if (item != null)
                    {
                        item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                        item.Deleted = BaseStatusDeleted.Manual;
                        Manager.SaveOrUpdate(item);
                    }
                }
                Manager.Commit();
            }
            catch (Exception ex) {
                Manager.RollBack();
            }
        }

        //private string AddItem(String name, int parentId, string mimeType, Boolean isDirectory, long size, byte[] content)
        //{
        //    try
        //    {
        //        SqlConnection connection = this.GetConnection(this.connectionString);

        //        SqlCommand command =
        //            new SqlCommand(
        //                "INSERT INTO Items ([Name], ParentId, MimeType, IsDirectory, [Size], Content) VALUES (@Name, @ParentId, @MimeType, @IsDirectory, @Size, @Content)", connection);
        //        command.Parameters.Add(new SqlParameter("@Name", name));
        //        command.Parameters.Add(new SqlParameter("@ParentId", parentId));
        //        command.Parameters.Add(new SqlParameter("@MimeType", mimeType));
        //        command.Parameters.Add(new SqlParameter("@IsDirectory", isDirectory));
        //        command.Parameters.Add(new SqlParameter("@Size", size));
        //        command.Parameters.Add(new SqlParameter("@Content", content));

        //        using (connection)
        //        {
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            this._data = null; //force update
        //        }

        //        return String.Empty;
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //}

        private EditorRepositoryItem AddDirectory(String name, long? idFolder)
        {
            EditorRepositoryItem folder = null;
            try
            {
                Manager.BeginTransaction();
                Person p = Manager.GetPerson(UC.CurrentUserID);
                if (p != null)
                {
                    folder = new EditorRepositoryItem();
                    folder.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    folder.IdOwner = IdUser;
                    folder.IdCommunity = IdCommunity;
                    folder.IsDirectory = true;
                    folder.Name = name;
                    folder.Identifyer = Guid.NewGuid();
                    if (idFolder.HasValue)
                        folder.Folder = Manager.Get<EditorRepositoryItem>(idFolder.Value);

                    Manager.SaveOrUpdate(folder);
                }
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return folder;
        }

        private EditorRepositoryItem AddFile(EditorRepositoryItem item, EditorRepositoryItem folder)
        {
            EditorRepositoryItem file = item;
            try
            {
                Manager.BeginTransaction();
                Person p = Manager.GetPerson(UC.CurrentUserID);
                if (p != null && item != null)
                {
                    file = new EditorRepositoryItem();
                    file.Name = item.Name;
                    file.Size = item.Size;
                    file.MimeType = item.MimeType;
                    file.Description = item.Description;
                    file.Extension = item.Extension;
                    file.Identifyer = item.Identifyer;
                    file.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                    file.IdOwner = IdUser;
                    file.IdCommunity = IdCommunity;
                    file.IsDirectory = false;
                    file.Folder = folder;
                    Manager.SaveOrUpdate(file);
                }
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                file = null;
            }
            return file;
        }

        private void CopyItemInternal(String path, String newPath, String repositoryPath)
        {
            EditorRepositoryItem item = this.GetItemRowFromPath(path);
            EditorRepositoryItem folder = this.GetParentFromPath(newPath);

            if (item.IsDirectory)
            {
                long idFolder =  (folder==null) ? 0 :  folder.Id;
                EditorRepositoryItem cDirectory = AddDirectory(item.Name, (folder==null) ? 0 : idFolder);

                List<EditorRepositoryItem> children = (from c in Manager.GetIQ<EditorRepositoryItem>() 
                                                       where c.Deleted== BaseStatusDeleted.None && ((idFolder==0 && c.Folder==null) || (idFolder>0 && c.Folder.Id==idFolder))
                                                       select c).ToList();
                foreach (EditorRepositoryItem child in children)
                {
                    CopyItemInternal(String.Format("{0}{1}{2}", this.TrimSeparator(path), pathSeparator, child.DisplayName), String.Format("{0}{1}{2}", newPath, pathSeparator, child.DisplayName), repositoryPath);
                }
            }
            else
            {
                EditorRepositoryItem clone = new EditorRepositoryItem();
                clone.IsDirectory = false;
                clone.Name = item.Name;
                clone.Size = item.Size;
                clone.Extension = item.Extension;
                clone.IdCommunity = item.IdCommunity;
                clone.IdOwner= item.IdOwner;
                clone.MimeType= item.MimeType;
                clone.Identifyer= Guid.NewGuid();
                lm.Comol.Core.File.Create.CopyFile_FM(repositoryPath + item.Identifyer.ToString() + ".stored", repositoryPath + clone.Identifyer.ToString() + ".stored");
                
                AddFile(clone,folder);
            }
        }

        public void CopyItem(String path, String newPath, String repositoryPath)
        {
            CopyItemInternal(path, newPath, repositoryPath);
        }

        public EditorRepositoryItem StoreFile(EditorRepositoryItem item, String location)
        {
            EditorRepositoryItem parent = this.GetItemRowFromPath(location);
            return AddFile(item, parent);
        }


        public String CreateDirectory(String name, String location)
        {
            EditorRepositoryItem folder = AddDirectory(name, this.GetItemIdFromPath(location));

            return (folder == null) ? String.Empty : name;
        }
        public byte[] GetItemContent(EditorRepositoryItem item, String storePath)
        {
            return item != null ? lm.Comol.Core.File.ContentOf.File_ToByteArray(storePath + item.Identifyer.ToString() + ".stored") : null;
        }

        public byte[] GetItemContent(String path,String storePath)
        {
            EditorRepositoryItem item = GetItemRowFromPath(path);

            return (item != null && !item.IsDirectory)  ? GetItemContent(item, storePath) : null;
        }

        public String GetPath(String path)
        {
            EditorRepositoryItem item = this.GetItemRowFromPath(path);
            if (item == null)
                item = GetParentFromPath(path);

            return item.IsDirectory ? GetFullPath(item) : GetLoaction(item);
        }

        public String GetLocation(String path)
        {
            EditorRepositoryItem item = this.GetItemRowFromPath(path);

            return this.GetLoaction(item);
        }
        private String GetLoaction(EditorRepositoryItem item)
        {
            if (item.Folder == null || item.DisplayName == "/")
                return String.Empty;
            else
            {
                EditorRepositoryItem parentFolder = (from f in Manager.GetIQ<EditorRepositoryItem>()
                                                     where f.IsDirectory && f.IdOwner == IdUser && IdUser > 0 && f.Deleted == BaseStatusDeleted.None
                                                     && f.Id == item.Folder.Id
                                                     select f).Skip(0).Take(1).ToList().FirstOrDefault();

                return (parentFolder != null) ? GetFullPath(parentFolder) : "";
            }
        }
        public Telerik.Web.UI.Widgets.FileItem GetFileItem(String path, String handlerPath)
        {
            EditorRepositoryItem item = this.GetItemRowFromPath(path);

            return this.CreateFileItem(item, handlerPath);
        }
        public Telerik.Web.UI.Widgets.DirectoryItem GetDirectoryItem(String path, Boolean includeSubfolders)
        {
            EditorRepositoryItem item = null;
            //long idFolder = -1;
            if (!String.IsNullOrEmpty(path) && path != "/")
            {
                item = GetItemRowFromPath(path);
                //  idFolder = (item != null) ? item.Id : -1;
                return (item != null && item.IsDirectory) ? CreateDirectoryItem(item, includeSubfolders) : null;
            }
            else {
                EditorRepositoryItem rFolder = new EditorRepositoryItem() { Id = 0, IdOwner = IdUser, IsDirectory = true, Name = "/" };
                return CreateDirectoryItem(rFolder, includeSubfolders);
            }

            //else
            //{
            //    List <EditorRepositoryItem> folders = (from f in Manager.GetIQ<EditorRepositoryItem>()
            //            where f.IsDirectory && f.IdOwner == IdUser && IdUser > 0 && f.Deleted == BaseStatusDeleted.None
            //            && f.Folder==null orderby f.Name select f).ToList();

            //    Telerik.Web.UI.Widgets.DirectoryItem directory = new Telerik.Web.UI.Widgets.DirectoryItem(item.Name,
            //                                          "",
            //                                          "",
            //                                          String.Empty,
            //                                          (UC.CurrentUserID == IdUser) ? Telerik.Web.UI.Widgets.PathPermissions.Read | Telerik.Web.UI.Widgets.PathPermissions.Delete | Telerik.Web.UI.Widgets.PathPermissions.Upload : Telerik.Web.UI.Widgets.PathPermissions.Read, //correct permissions should be applied from the content provider
            //                                          null, null
            //                                          );
            //    directory
            //}

        }
        private Telerik.Web.UI.Widgets.DirectoryItem CreateDirectoryItem(EditorRepositoryItem item, Boolean includeSubfolders)
        {
            Telerik.Web.UI.Widgets.DirectoryItem directory = new Telerik.Web.UI.Widgets.DirectoryItem(item.DisplayName,
                                                        this.GetLoaction(item),
                                                        this.GetFullPath(item),
                                                        String.Empty,
                                                        (item.IdOwner == IdUser) ? Telerik.Web.UI.Widgets.PathPermissions.Read | Telerik.Web.UI.Widgets.PathPermissions.Delete | Telerik.Web.UI.Widgets.PathPermissions.Upload : Telerik.Web.UI.Widgets.PathPermissions.Read, //correct permissions should be applied from the content provider
                                                         
                                                        null, null
                                                        );

            if (includeSubfolders)
            {
                List<EditorRepositoryItem> subDirItems = GetChildDirectories(item);
                List<Telerik.Web.UI.Widgets.DirectoryItem> subDirs = new List<Telerik.Web.UI.Widgets.DirectoryItem>();

                foreach (EditorRepositoryItem subDir in subDirItems)
                {
                    subDirs.Add(CreateDirectoryItem(subDir, false));
                }

                directory.Directories = subDirs.ToArray();
            }

            return directory;
        }
        private List<EditorRepositoryItem> GetChildDirectories(EditorRepositoryItem item)
        {
            return (from f in Manager.GetIQ<EditorRepositoryItem>()
                    where f.IsDirectory && f.IdOwner == IdUser && IdUser > 0 && f.Deleted == BaseStatusDeleted.None
                    && ((item.Id==0 && f.Folder==null) || (item.Id>0 && f.Folder != null && f.Folder.Id == item.Id))
                    select f).ToList().OrderBy(f=>f.DisplayName).ToList();
        }
        public Telerik.Web.UI.Widgets.FileItem[] GetChildFiles(String folderPath, String[] searchPatterns, String handlerPath)
        {
            EditorRepositoryItem parentFolder = GetItemRowFromPath(folderPath);

            List<EditorRepositoryItem> files = null;
            if (parentFolder !=null) 
                files = (from f in Manager.GetIQ<EditorRepositoryItem>()
                                                      where f.Deleted == BaseStatusDeleted.None && f.IdOwner == IdUser && !f.IsDirectory &&
                                                      f.Folder.Id == parentFolder.Id
                                                      select f).ToList();
            else
                files = (from f in Manager.GetIQ<EditorRepositoryItem>()
                         where f.Deleted == BaseStatusDeleted.None && f.IdOwner == IdUser && !f.IsDirectory && f.Folder == null
                         select f).ToList();
            if (searchPatterns.Length > 0 && Array.IndexOf(searchPatterns, "*.*") == -1) {
                files = files.Where(f => searchPatterns.Contains("*" + f.Extension)).ToList();
            }

            List<Telerik.Web.UI.Widgets.FileItem> result = new List<Telerik.Web.UI.Widgets.FileItem>(files.Count);
            foreach (EditorRepositoryItem f in files.OrderBy(f => f.DisplayName).ToList())
            {
                result.Add(this.CreateFileItem(f, handlerPath));
            }

            return result.ToArray();
        }

        //private String GetSearchPatternsFilter(String[] searchPatterns)
        //{
        //    if (Array.IndexOf(searchPatterns, "*.*") > -1)
        //        return String.Empty;


        //    String searchPatterntsFilterExpression = " AND (Name LIKE '%";
        //    for (int i = 0; i < searchPatterns.Length; i++)
        //    {
        //        searchPatterntsFilterExpression += searchPatterns[i].Substring(searchPatterns[i].LastIndexOf('.'));
        //        if (i < searchPatterns.Length - 1)
        //            searchPatterntsFilterExpression += "' OR Name LIKE '%";
        //        else
        //            searchPatterntsFilterExpression += "')";
        //    }

        //    return searchPatterntsFilterExpression;
        //}

        private Telerik.Web.UI.Widgets.FileItem CreateFileItem(EditorRepositoryItem item, String handlerPath)
        {
            string itemPath = this.GetFullPath(item);
            return new Telerik.Web.UI.Widgets.FileItem(item.DisplayName,
                                item.Extension,
                                item.Size,
                                itemPath,
                                GetItemUrl(itemPath, handlerPath, item.Identifyer),
                                String.Empty,
                                (item.IdOwner == IdUser) ? Telerik.Web.UI.Widgets.PathPermissions.Read | Telerik.Web.UI.Widgets.PathPermissions.Delete : Telerik.Web.UI.Widgets.PathPermissions.Read 
                                );
        }
        private String GetFullPath(EditorRepositoryItem item)
        {
            if (item.DisplayName != "/")
            {
                String path = item.DisplayName;
                if (item.IsDirectory && path != "" && path != "/") path += pathSeparator;

                do
                {
                    EditorRepositoryItem parentSearch = (item.Folder != null) ? item.Folder : new EditorRepositoryItem() { Name = "" };

                    if (parentSearch != null)
                    {
                        item = parentSearch;
                        path = String.Format("{0}{1}{2}", item.DisplayName, pathSeparator, path);
                    }
                } while (item.Folder != null);
                return (path.StartsWith("/") ? path : "/" + path);
            }
            else
                return "/";
        }
        private String GetItemUrl(String itemPath, String handlerPath, System.Guid identifyer)
        {
            String escapedPath = HttpUtility.UrlEncode(itemPath);
            return String.Format("{0}?path={1}", handlerPath, identifyer.ToString());
        }
        public Boolean ItemExists(String path)
        {
            EditorRepositoryItem item = this.GetItemRowFromPath(path);
            return !Object.Equals(item, null);
        }
        private String TrimSeparator(String path)
        {
            return path.Trim(pathSeparator);
        }
        private String GetName(String path)
        {
            String tmpPath = this.TrimSeparator(path);
            return tmpPath.Substring(tmpPath.LastIndexOf(pathSeparator) + 1);
        }


    //    Private Function GetPermission(ByVal oLabel As LabelTag) As PathPermissions
    //    If oLabel Is Nothing Then
    //        Return PathPermissions.Read
    //    ElseIf oLabel.ID < 0 And oLabel.isUserDefined Then
    //        Return PathPermissions.Read Or PathPermissions.Upload
    //    ElseIf oLabel.ID < 0 Then
    //        Return PathPermissions.Read
    //    ElseIf oLabel.isUserDefined And oLabel.CreatedBy.ID = Me._Person.ID Then
    //        Return PathPermissions.Read Or PathPermissions.Delete Or PathPermissions.Upload
    //    Else
    //        Return PathPermissions.Read Or PathPermissions.Delete Or PathPermissions.Upload
    //    End If
    //End Function

    }
}