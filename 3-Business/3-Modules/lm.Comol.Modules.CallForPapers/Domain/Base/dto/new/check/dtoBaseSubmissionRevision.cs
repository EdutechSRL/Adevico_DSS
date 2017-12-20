using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    //[Serializable]
    //public class dtoBaseSubmissionRevision :dtoBaseSubmission 
    //{
    //    public virtual List<dtoRevision> Revisions { get; set; }
    //    //public virtual long IdRevision { get { return (Revision == null) ? 0 : Revision.Id; } }
    //    //public virtual dtoRevision Revision { get; set; }
    //    public dtoBaseSubmissionRevision() :base() {
    //        Status = SubmissionStatus.none;
    //    }

    //    public dtoBaseSubmissionRevision(long id)
    //        : base(id)
    //    {
    //        Status = SubmissionStatus.none;
    //    }
    //    //public void InitializeRevision(Revision revision, Boolean full)
    //    //{
    //    //    if (revision != null)
    //    //    {
    //    //        if (revision is SubmissionRevision)
    //    //            Revision = dtoRevisionSubmission.Initialize((SubmissionRevision)revision, full);
    //    //        else
    //    //            Revision = dtoRevision.Initialize(revision);
    //    //    }
    //    //}
    //}
}