
using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Authentication;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class Employee : Person
	{
        //public virtual PersonInfo PersonInfo { get; set; }
        public virtual AgencyAffiliation CurrentAffiliation
        {
            get
            {
                return Affiliations.Where(a => a.IsEnabled).OrderBy(a => a.FromDate).ToList().FirstOrDefault();
            }
        }
        public virtual IList<AgencyAffiliation> Affiliations { get; set; }


        public Employee()
            : base()
		{
            Affiliations = new List<AgencyAffiliation>();
            //PersonInfo = new PersonInfo();
		}

        public virtual AgencyAffiliation GetAffiliation(DateTime date)
        {
            AgencyAffiliation result = null;
            if (Affiliations.Any()) {
                result = Affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                  (
                      (!a.ToDate.HasValue && a.FromDate <= date))
                      || (a.FromDate <= date && a.ToDate.HasValue && a.ToDate.Value >= date)
                  ).OrderByDescending(a=> a.FromDate).FirstOrDefault();

            }
          
            return result;
        }

        public virtual Boolean HasAffiliations(DateTime date)
        {
            Boolean result = false;
            result = (Affiliations.Any() && Affiliations.Where(a=> a.Deleted== BaseStatusDeleted.None && 
                (
                    (!a.ToDate.HasValue && a.FromDate<=date))
                    || (a.FromDate<= date && a.ToDate.HasValue && a.ToDate.Value >=date)
                ).Any());

            return result;
        }
        public virtual Boolean HasAffiliations(DateTime? fromDate, DateTime? toDate)
        {
            Boolean result = false;
            result = (Affiliations.Any() && Affiliations.Where(a => a.Deleted == BaseStatusDeleted.None &&
                    (
                        (!a.ToDate.HasValue && toDate.HasValue && HasAffiliations(toDate.Value))
                        ||
                        (fromDate.HasValue && HasAffiliations(fromDate.Value))
                    )
                
                ).Any());

            return result;
        }
        public static List<ProfileAttributeType> AvailableAttributes() {
            List<ProfileAttributeType> attributes = new List<ProfileAttributeType>();
            attributes.Add(ProfileAttributeType.agencyNationalCode);
            attributes.Add(ProfileAttributeType.agencyExternalCode);
            attributes.Add(ProfileAttributeType.agencyTaxCode);
            attributes.Add(ProfileAttributeType.agencyInternalCode);
            attributes.Add(ProfileAttributeType.agencyName);
            return attributes;
        }
        public static Dictionary<ProfileAttributeType, List<ProfileAttributeType>> AlternativeAttributes()
        {
            Dictionary<ProfileAttributeType, List<ProfileAttributeType>> results = new Dictionary<ProfileAttributeType, List<ProfileAttributeType>>();

            List<ProfileAttributeType> alternatives = new List<ProfileAttributeType>();
            alternatives.Add(ProfileAttributeType.agencyNationalCode);
            alternatives.Add(ProfileAttributeType.agencyExternalCode);
            alternatives.Add(ProfileAttributeType.agencyTaxCode);
            alternatives.Add(ProfileAttributeType.agencyInternalCode);
            results.Add(ProfileAttributeType.agencyNationalCode, alternatives.Where(a => a != ProfileAttributeType.agencyNationalCode).ToList());
            results.Add(ProfileAttributeType.agencyExternalCode, alternatives.Where(a => a != ProfileAttributeType.agencyExternalCode).ToList());
            results.Add(ProfileAttributeType.agencyTaxCode, alternatives.Where(a => a != ProfileAttributeType.agencyTaxCode).ToList());
            results.Add(ProfileAttributeType.agencyInternalCode, alternatives.Where(a => a != ProfileAttributeType.agencyInternalCode).ToList());

            return results;
        }
	}
}