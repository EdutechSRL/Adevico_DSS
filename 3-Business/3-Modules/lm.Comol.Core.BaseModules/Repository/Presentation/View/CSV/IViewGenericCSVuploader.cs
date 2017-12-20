using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewGenericCSVuploader : lm.Comol.Core.DomainModel.Common.iDomainView 
    {

        dtoCsvSettings CurrentSettings { get; set; }
        dtoCSVfile CurrentFile { get; set; }
        Boolean AllowPreview { set; }
        Boolean ClearPreviousFiles { get; set; }
        String DefaultCSVfolder { get; set; }
        TextDelimiter DefaultDelimiter { get; set; }
        Boolean isInitialized { get; set; }
        Boolean isValid { get; set; }
        List<ExternalColumnComparer<String, Int32>> AvailableColumns { get; set; }

       
        void InitializeControl();
        void InitializeControl(dtoCsvSettings settings, dtoCSVfile file);
        void LoadUploadedFile(dtoCSVfile file);
        void DisplayFileToUpload();
        void DisplayUploadError();
        void PreviewRows(CsvFile file);

        CsvFile RetrieveFile();
        List<ExternalColumnComparer<String, Int32>> GetAvailableColumns();
        ExternalResource GetFileContent(List<ExternalColumnComparer<String, Int32>> columns);
        void DisplaySessionTimeout();
    }
}