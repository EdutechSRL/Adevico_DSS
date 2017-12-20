using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewAddField : IViewBase
    {
        long IdCall { get; set; }
        FieldType CurrentType { get; set; }
        DisclaimerType CurrentDisclaimerType { get; set; }
        List<dtoCallField> GetFieldsToCreate();
        void InitializeControl(long idCall);
        //void LoadParents(List<dtoCallSection<dtoCallField>> sections);
        void LoadAvailableTypes(List<DisplayFieldType> items);
        void LoadFields(List<dtoSubmissionValueField> fields);
        List<FieldDefinition> CreateFields(List<dtoCallSection<dtoCallField>> sections, long idSection);
    }
}