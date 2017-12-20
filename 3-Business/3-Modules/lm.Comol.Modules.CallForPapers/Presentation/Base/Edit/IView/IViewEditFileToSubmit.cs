using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditFileToSubmit : IViewBaseEditCall
    {
        List<dtoSubmitterType> Availablesubmitters { get; set; }
        List<dtoCallRequestedFile> GetRequestedFiles();
        dtoCallRequestedFile GetDefaultRequestedFile();

        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void LoadFiles(List<dtoRequestedFilePermission> items);
        void DisplayNoFileToSubmit();
        void DisplaySettingsSaved();
        void DisplayError(EditorErrors errors);
        void HideErrorMessages();
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
    }
}