using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertificationList : IViewBaseCertification
    {
        IList<dtoCertificateStat> CertificateStats { get; set; }

        IList<SubActivity> Certificates { get; set; }

        IList<Role> Roles { get; set; }

        Int64 FilteredCertificate { get; set; }
        Int32 FilteredRole { get; set; }

        Boolean AllStatistics { get; set; }

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }        
                
        long PreloadIdPath { get; }                
        long CurrentIdPath { get; set; }
        
        Int32 PreloadIdCommunity { get; }
        Int32 CurrentPathIdCommunity { get; set; }

        void InitializeFilters();

        Boolean Empty { get; set; }
        Boolean OnlyOne { get; set; }
    }

    
}
