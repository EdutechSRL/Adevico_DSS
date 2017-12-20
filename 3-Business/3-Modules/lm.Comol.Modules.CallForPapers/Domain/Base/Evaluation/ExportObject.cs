using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    //[Serializable]
    //public enum ExportObject
    //{ 
    //    SubmissionEvaluations = 0,
    //    CommitteesEvaluations = 1,
    //    CommitteeEvaluations = 2
    //}

    [Serializable]
    public enum ItemsToExport
    {
        All = 0,
        Filtered = 1
    }

    [Serializable]
    public enum ExportData
    {
        DisplayData = 0,
        Fulldata = 1,
        FulldataToAnalyze = 2
    }
}