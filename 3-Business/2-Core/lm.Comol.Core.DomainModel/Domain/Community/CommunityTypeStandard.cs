using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum CommunityTypeStandard : int
	{
		Organization = 0,
		UniversityCourse = 1,
		Degree = 6,
        Thesis = 3,
        WorkGroup = 5,
	    ThesisWorkGroup = 2,
	}
}