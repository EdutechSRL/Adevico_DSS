using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum UserTypeStandard : int
	{
		AllUserType = -2,
		AllWithoutGuestUser = -1,
		Undergraduate = 1,
		UniversityTeacher = 2,
		Tutor = 3,
		ExternalUser = 4,
		Other = 5,
		Administrative = 6,
		SysAdmin = 7,
		TypingOffice = 8,
		PHDstudent = 9,
		HighSchoolStudent = 10,
		HighSchoolTeacher = 11,
		Director = 12,
		Researcher = 13,
		Teacher = 14,
		Student = 15,
		ExStudent = 16,
		Technician = 17,
		Guest = 18,
		Administrator = 19,
        Company = 20,
        PublicUser = 21,
        Employee = 22
	}
}