using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertification : IViewBaseCertification
    {
        IList<SubActivity> Certificates { get; set; }

        dtoCertificateStat CertificateStat { get; set; }

        IList<Role> Roles { get; set; }

        Int64 FilteredCertificate { get; set; }
        Int32 FilteredRole { get; set; }
        String FilteredUser { get; set; }

        Boolean AllStatistics { get; set; }
        Boolean IsPathManager { get; set; }

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        long PreloadIdPath { get; }
        long CurrentIdPath { get; set; }

        Int32 PreloadIdCommunity { get; }
        Int32 CurrentPathIdCommunity { get; set; }

        long PreloadIdCertificate { get; }
        long CurrentPathIdCertificate { get; set; }

        void InitializeFilters();

        Boolean Empty { get; set; }
    }
}
