using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewViewSubmission : IViewViewBaseSubmission
    {
        Boolean PreloadAscending { get; }
        SubmissionFilterStatus PreloadFilterSubmission { get; }
        SubmissionsOrder PreloadOrderSubmission { get; }
        Boolean PreloadFromManagement { get; }
        int PreloadPageIndex { get; }
        int PreloadPageSize { get; }

        Boolean ShowAdministrationTools { get; set; }
        Boolean AllowRevisionRequest { get; set; }


        void LoadAvailableStatus(List<SubmissionStatus> items);
        void UpdateStatus(SubmissionStatus status);

        long IdPendingRevision { get; set; }
        void LoadAvailableRevision(List<dtoRevision> revisions, long idSelected);
        void InitializeRevisionRequest(RevisionType type);
        void DisplayRevisionInfo(dtoRevision revision);
        void DisplaySelfPendingRequest(dtoRevisionRequest revision, String url);
        void DisplayUserPendingRequest(dtoRevisionRequest revision, String url);
        void DisplayPendingRevision(dtoRevisionRequest revision, String url);
        void DisplayManagePendingRevision(dtoRevisionRequest revision, String url);
        List<dtoRevisionItem> GetFieldsToReview();


        //Nasconde le "SIGN", nel caso il bando non le preveda (DEFAULT)
        void HideSignSubmission();

        //MOstra che non è stata caricata la controfirma
        void ShowSignNotSubmitted();

        void ShowSendInfo(bool sended);

        //Inizializza l'UC per il download dell'allegato
        void InitializeDownloadSign(ModuleLink link);

        //Inizializza il controllo per l'upload
        void InitSignSubmission(int idCommunity);

        //Carica il file
        Core.DomainModel.ModuleActionLink AddInternalFile(
            Revision revision,
            String moduleCode,
            int idModule,
            int moduleAction,
            int objectType);

        //Addendum per Advanced
        Boolean IsAdvance { get; set; }
        long CommissionId { get; set; }

        void ShowHideSendIntegration(bool Show);

        void SendUserAction(int idCommunity, int idModule, Int64 idCall, ModuleCallForPaper.ActionType action);


        /// <summary>
        /// Invia TRAP di corretto caricamento controfirma
        /// </summary>
        void SendSuccessTrap();
    }
}
