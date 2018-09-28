using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertificationUser : IViewBaseCertification
    {

        long PreloadIdPath { get; }
        long CurrentIdPath { get; set; }

        long PreloadIdCertificate { get; }
        long CurrentIdCertificate { get; set; }

        Boolean isPathManager { get; set; }
        Int32 PreloadIdCommunity { get; }
        Int32 CurrentPathIdCommunity { get; set; }

        Int32 PreloadIdUser { get; }
        Int32 CurrentIdUser { get; set; }

        void InitializeFilters();

        Boolean Empty { get; set; }

        //Boolean AllStatistics { get; set; }

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        dtoCertificatesUserStat CertificatesStats { get; set; }

    }
    
}
