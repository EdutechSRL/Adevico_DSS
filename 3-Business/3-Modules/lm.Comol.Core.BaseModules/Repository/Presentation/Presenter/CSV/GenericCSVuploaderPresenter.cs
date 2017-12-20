using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation

{
    public class GenericCSVuploaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
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
            protected virtual IViewGenericCSVuploader View
            {
                get { return (IViewGenericCSVuploader)base.View; }
            }
            public GenericCSVuploaderPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public GenericCSVuploaderPresenter(iApplicationContext oContext, IViewGenericCSVuploader view)
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
                dtoCsvSettings settings = new dtoCsvSettings() { ColumnDelimeter= View.DefaultDelimiter, FirstRowColumnNames= true, RowDelimeter= TextDelimiter.CrLf, RowsToSkip=0};
                if (!View.isInitialized && View.ClearPreviousFiles)
                    ClearPreviousFiles(filePath);
                View.InitializeControl(settings, ((View.isInitialized) ? GetCurrentFile(filePath) :null));
                View.isInitialized= true;
            }
        }

        public void UploadCSV(ref System.Web.HttpPostedFile file,String filePath) {
            if (file != null && file.ContentLength > 0)
            {
                Guid uniqueID = System.Guid.NewGuid();
                if (Create.Directory(filePath))
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

        private void ClearPreviousFiles(String filePath)
        {
            List<dtoFileSystemInfo> files = new List<dtoFileSystemInfo>();
            ContentOf.Directory(filePath, ref files, "*.csv.stored");
            foreach (dtoFileSystemInfo file in files) {
                Delete.File(file.FullName);
            }
        }
        public List<ExternalColumnComparer<String, Int32>> GetAvailableColumns(dtoCsvSettings settings, dtoCSVfile file)
        {
            List<ExternalColumnComparer<String, Int32>> columns = new List<ExternalColumnComparer<String, Int32>>();
            if (file != null)
            {
                CsvFile header = ContentOf.LoadCsvFile(file.RealName, settings,1);
                if (header!=null)
                    columns = (from c in header.ColumHeader
                               select new ExternalColumnComparer<String, Int32>
                               {
                                   SourceColumn = (c.Empty) ? c.Number.ToString() : c.Value,
                                   Number = c.Number,
                                   DestinationColumn = new DestinationItem<Int32>() { Id=-1, InputType = InputType.skip },
                                   InputType = InputType.skip
                               }).ToList();
            }
            return columns;
        }

        public ExternalResource GetFileContent(List<ExternalColumnComparer<String, Int32>> columns, dtoCsvSettings settings, dtoCSVfile file)
        {
           
            CsvFile csvFile = ContentOf.LoadCsvFile(file.RealName, settings);
            ExternalResource result = new ExternalResource(columns, csvFile);
            return result;
        }
    }
}