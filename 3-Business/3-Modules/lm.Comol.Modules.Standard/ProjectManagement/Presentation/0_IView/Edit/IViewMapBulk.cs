using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewMapBulk : IViewBaseProjectMap
    {
   
        #region "Context"
            Boolean AllowSave { get; set; }
        #endregion

        DateTime? PreviousStartDate { get; }
        DateTime? PreviousDeadline { get; }
        void DisplayErrorSavingDates();
        void DisplaySavedDates(dtoField<DateTime?> startDate, dtoField<DateTime?> endDate, dtoField<DateTime?> deadLine);
    }
}