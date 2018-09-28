using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.Domain.DTO;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertificationRestore : IViewCertificationPageBase
    {
       
        void AddAutoRedirectoToDownloadPage(String url);
    }
}