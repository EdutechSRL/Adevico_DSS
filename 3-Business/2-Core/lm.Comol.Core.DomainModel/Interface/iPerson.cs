using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iPerson : iDomainObject<int>
	{

		string Name { get; set; }
		string Surname { get; set; }
		string Login { get; set; }
		string Password { get; set; }
		string Mail { get; set; }
        string FirstLetter { get; set; }
		bool isDisabled { get; set; }
        string SurnameAndName { get; set; }
		int TypeID { get; set; }
		int AuthenticationTypeID { get; set; }
		string TaxCode { get; set; }
		int LanguageID { get; set; }

        DateTime? LastAccessOn { get; set; }
        Boolean AcceptPolicy { get; set; }
        DateTime? AcceptPolicyOn { get; set; }
	}
}