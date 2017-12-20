
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class CompanyUser : Person
	{
   
        public virtual CompanyInfo CompanyInfo { get; set; }

        public CompanyUser() :base()
		{
            CompanyInfo = new CompanyInfo();
            PersonInfo = new PersonInfo();
		}
	}
}