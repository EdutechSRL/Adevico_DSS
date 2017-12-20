using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewRenderField : IViewBase
    {
        Boolean Selected { get; set; }
        Boolean AllowRevisionCheck { get; set; }
        Boolean ShowFieldChecked { get; set; }
        long IdField { get; set; }
        long RevisionCount { get; set; }
        FieldType FieldType { get; set; }
        FieldError CurrentError { get; set; }
        Boolean Disabled { get; set; }
        Int32 MaxChars { get; set; }
        Int32 MaxOptions { get; set; }
        Int32 MinOptions { get; set; }

        void InitializeControl(dtoSubmissionValueField field, Boolean disabled, Boolean isPublic, Boolean allowRevisionCheck);
        void InitializeControl(dtoSubmissionValueField field, Boolean disabled, Boolean isPublic, FieldError err, Boolean allowRevisionCheck);
        void SetupView(dtoSubmissionValueField field, Boolean isPublic);
        void DisplayEmptyField();
    }
}