using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation.Base.Controls.IView
{
    public interface IViewPrintDraft : IViewBase
    {
        long IdCall { get; set; }
        CallForPaperType  CallType { get; set; }
        String  CallName { get; set; }
        SubmitterType SubmissionType { get; set; }

        long IdRevision { get; set; }
        long IdSubmission { get; set; }
        
        //void InitView(long callId, long subTypeId);

        /// <summary>
        /// Solo per pagina di Edit per avere i settings aggiornati.
        /// </summary>
        /// <param name="settings"></param>
        /// <remarks>Per pagine normali, fare return di settings, per pagina di edit: raiseevent ed aggiornare il valore</remarks>
        void UpdateSettings(ref CallPrintSettings settings);
    }
}
