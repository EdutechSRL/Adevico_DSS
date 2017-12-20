using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Subscriptions;
using lm.Comol.Core.Communities;
using lm.Comol.Core.BaseModules.CommunityManagement;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewIMsourceCSV : lm.Comol.Core.DomainModel.Common.iDomainView 
    {

        dtoCsvSettings CurrentSettings { get; set; }
        lm.Comol.Core.BaseModules.Repository.dtoCSVfile CurrentFile { get; set; }
        Boolean AllowPreview { set; }
        String CSVfolder { get; set; }
        Boolean isInitialized { get; set; }
        Boolean isValid { get; set; }
        List<ProfileColumnComparer<String>> AvailableColumns { get; set; }

       
        void InitializeControl();
        void InitializeControl(dtoCsvSettings settings, lm.Comol.Core.BaseModules.Repository.dtoCSVfile file);
        void LoadUploadedFile(lm.Comol.Core.BaseModules.Repository.dtoCSVfile file);
        void DisplayFileToUpload();
        void DisplayUploadError();
        void PreviewRows(CsvFile file);

        CsvFile RetrieveFile();
        List<ProfileColumnComparer<String>> GetAvailableColumns();
        ProfileExternalResource GetFileContent(List<ProfileColumnComparer<String>> columns);
        void DisplaySessionTimeout();
    }
}