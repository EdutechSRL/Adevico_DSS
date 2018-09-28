using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoCertificateStat
    {

        public Int64 PathId { get; set; }
        public Int64 ActivityId { get; set; }
        public Int64 SubActivityId { get; set; }
        public Int32 CommunityId { get; set; }

        public dtoSubActivity dtoSubActivity { get; set; }

        public Int64 CertificateId { get; set; }
        public Int64 CertificateVersion { get; set; }
        public String CertificateName { get; set; }

        public Int32 Obtained { get; set; }
        public Int32 Total { get; set; }

        public IList<dtoCertificateUsersStat> Users { get; set; }        

        public dtoCertificateStat()
        {
            Users = new List<dtoCertificateUsersStat>();
        }
    }

    public class dtoCertificateUsersStat
    {
        public dtoCertificateStat Parent { get; set; }

        public Int32 PersonId { get; set; }
        public litePerson Person { get; set; }

        public DateTime Date { get; set; }
    }
}
