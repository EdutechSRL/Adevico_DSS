using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.FileRepository.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewEditAttachments : IViewBaseEditCall
    {
        
        List<dtoSubmitterType> Availablesubmitters { get; set; }
        //void LoadSubmitters(List<dtoSubmitterTypePermission> items);
        //void DisplayError(SubmitterTypeError errors);
        //void DisplayErrors(Dictionary<long,SubmitterTypeError> errors);
        List<dtoAttachmentFile> GetAttachments();
        void LoadAttachments(List<dtoAttachmentFilePermission> items);
        void LoadSubmitterTypes(List<dtoSubmitterType> submitters);
        void DisplaySavingError();
        void DisplayDeletingError();
        void DisplayNoAttachments();
        void DisplaySettingsSaved();
        void HideErrorMessages();
        void InitializeAttachmentsControl(long idCall, CallForPaperType type, RepositoryIdentifier identifier, List<lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions> actions, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions dAction = Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleCallForPaper.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idCall, ModuleRequestForMembership.ActionType action);
        
    }
}