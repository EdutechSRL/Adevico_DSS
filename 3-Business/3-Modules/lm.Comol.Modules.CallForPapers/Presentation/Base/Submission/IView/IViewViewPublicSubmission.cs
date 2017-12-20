using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewViewPublicSubmission : IViewViewBaseSubmission
    {
        Boolean FromPublicList { get; }
        void InitializeView(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext skin);
    }
}
