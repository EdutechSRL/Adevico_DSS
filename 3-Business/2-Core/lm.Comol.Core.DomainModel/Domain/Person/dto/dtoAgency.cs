
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class dtoAgency : DomainBaseObject<long>
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
        public virtual List<dtoOrganizationAffiliation> Organizations { get; set; }
        public virtual Int32 EmployeeNumber { get; set; }
        public dtoAgency()
            : base()
		{
            IsDefault = false;
            IsEditable = true;
            AlwaysAvailable = true;
            Organizations = new List<dtoOrganizationAffiliation>();
		}
	}
}