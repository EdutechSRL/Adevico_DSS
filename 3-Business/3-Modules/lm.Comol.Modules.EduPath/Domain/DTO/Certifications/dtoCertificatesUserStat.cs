using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoCertificatesUserStat
    {
        public Int32 IdPerson { get; set; }
        public litePerson Person { get; set; }
        public Int64 PathId { get; set; }
        public String PathName { get; set; }

        public IList<dtoCertificateUserStat> Certificates { get; set; }
    }

    public class dtoCertificateUserStat
    {
        public Int64 PathId { get; set; }
        public String PathName { get; set; }
        public Int64 ActivityId { get; set; }
        public Int64 SubActivityId { get; set; }
        public Int32 CommunityId { get; set; }

        public Int64 CertificateId { get; set; }
        public Int64 CertificateVersion { get; set; }
        public String CertificateName { get; set; }

        public DateTime CompletedOn { get; set; }

        public dtoSubActivity SubActivity { get; set; }
    }
}
