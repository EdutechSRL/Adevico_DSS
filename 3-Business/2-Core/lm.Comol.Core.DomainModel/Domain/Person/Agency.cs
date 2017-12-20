using System;
using System.Collections.Generic;
using System.Linq;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class Agency : DomainBaseObjectMetaInfo<long>
	{
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String TaxCode { get; set; }
        public virtual String ExternalCode { get; set; }
        public virtual String NationalCode { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsEmpty { get; set; }
        public virtual Boolean IsEditable { get; set; }
        public virtual Boolean AlwaysAvailable { get; set; }
        public virtual IList<OrganizationAffiliation> OrganizationAffiliations { get; set; }
        public Agency()
            : base()
		{
            IsDefault = false;
            IsEmpty = false;
            IsEditable = true;
            AlwaysAvailable = true;
            OrganizationAffiliations = new List<OrganizationAffiliation>();
		}

        public virtual List<OrganizationAffiliation> ActiveAffiliations()
        {
            return (OrganizationAffiliations == null || !OrganizationAffiliations.Any()) ? new List<OrganizationAffiliation>() : OrganizationAffiliations.Where(a => a.Deleted == BaseStatusDeleted.None).ToList();
        }
    }
}