using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.File;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.BaseModules.Repository;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class SourceCsvPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            private ProfileManagementService _Service;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewIMsourceCSV View
            {
                get { return (IViewIMsourceCSV)base.View; }
            }
            private ProfileManagementService Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ProfileManagementService(AppContext);
                    return _Service;
                }
            }
            public SourceCsvPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SourceCsvPresenter(iApplicationContext oContext, IViewIMsourceCSV view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(String filePath)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                dtoCsvSettings settings = new dtoCsvSettings() { ColumnDelimeter= TextDelimiter.Semicolon, FirstRowColumnNames= true, RowDelimeter= TextDelimiter.CrLf, RowsToSkip=0};
                //List<dtoFileSystemInfo> files = new List<dtoFileSystemInfo>();
                //ContentOf.Directory(filePath, files, "*.csv.stored");

                //dtoFileSystemInfo LastFile = files.OrderByDescending(f => f.CreationTime).Where(f => f.Exists).FirstOrDefault();

                View.InitializeControl(settings, GetCurrentFile(filePath));
            }
        }

        public void UploadCSV(ref System.Web.HttpPostedFile file,String filePath) {
            Guid uniqueID = System.Guid.NewGuid();
            if (Create.Directory(filePath) && file.ContentLength>0)
            {
                String fileName = filePath + uniqueID.ToString() + "_" + file.FileName + ".stored";
                FileMessage result = Create.UploadFile(ref file, fileName);
                if (result == FileMessage.FileCreated)
                    View.LoadUploadedFile(GetUploadedFile(fileName));
                else
                    View.DisplayUploadError();
            }
            else
                View.DisplayUploadError();
        }
        public void RemoveFile(dtoCSVfile file) {
            if (file != null)
                Delete.File(file.RealName);
            View.DisplayFileToUpload();
        }

        public void Preview(dtoCsvSettings settings, dtoCSVfile file)
        {
            if (file != null)
                View.PreviewRows(ContentOf.LoadCsvFile(file.RealName, settings, 8));
            else
                View.PreviewRows(null);
        }

        private dtoCSVfile GetCurrentFile(String filePath)
        {
            List<dtoFileSystemInfo> files = new List<dtoFileSystemInfo>();
            ContentOf.Directory(filePath, ref files, "*.csv.stored");

            dtoFileSystemInfo LastFile = files.OrderByDescending(f => f.CreationTime).Where(f => f.Exists).FirstOrDefault();
            return GetCsvFile(LastFile);
        }
        private dtoCSVfile GetUploadedFile(String filePath)
        {
            dtoFileSystemInfo uploaded = ContentOf.File_dtoInfo(filePath);
            return GetCsvFile(uploaded);
        }
        private dtoCSVfile GetCsvFile(dtoFileSystemInfo lastFile){
            dtoCSVfile file = null;
            if (lastFile != null) {
                file = new dtoCSVfile();
                String fileName = lastFile.Name;


                file.RealName = lastFile.FullName;
                file.Size = lastFile.Length;
                file.UploadedOn = lastFile.CreationTime;
                try{
                    file.Id= new Guid((fileName.Split('_'))[0]);
                }
                catch(Exception ex){}
                file.Name = fileName.Replace(file.Id.ToString() + "_", "").Replace(".stored","");
            }
            return file;
        }

        public List<ProfileColumnComparer<String>> GetAvailableColumns(dtoCsvSettings settings, dtoCSVfile file)
        {
            List<ProfileColumnComparer<String>> columns = new List<ProfileColumnComparer<String>>();
            if (file != null)
            {
                CsvFile header = ContentOf.LoadCsvFile(file.RealName, settings,1);
                if (header != null)
                {
                    Dictionary<ProfileAttributeType,String> attributes = new Dictionary<ProfileAttributeType,String>();
                    foreach(ProfileAttributeType att in Enum.GetValues(typeof(ProfileAttributeType))){
                        attributes.Add(att, att.ToString().ToLower());
                    }
                    columns = (from c in header.ColumHeader
                               select new ProfileColumnComparer<String>
                               {
                                   SourceColumn = (c.Empty) ? c.Number.ToString() : c.Value,
                                   Number = c.Number,
                                   DestinationColumn = (c.Empty || !attributes.Values.Contains(c.Value.ToLower())) ? Authentication.ProfileAttributeType.skip : attributes.ToList().Where(a=>a.Value== c.Value.ToLower()).Select(a=>a.Key).FirstOrDefault()
                               }).ToList();
                }
            }
            return columns;
        }

        public ProfileExternalResource GetFileContent(List<ProfileColumnComparer<String>> columns, dtoCsvSettings settings, dtoCSVfile file)
        {
            CsvFile csvFile = ContentOf.LoadCsvFile(file.RealName, settings);
            ProfileExternalResource result = new ProfileExternalResource(columns,csvFile);
            return result;
        }
    }
}