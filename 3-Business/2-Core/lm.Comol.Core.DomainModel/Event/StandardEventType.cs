
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public enum StandardEventType : int
	{
		All = -1,
		Reminder = 0,
		Lesson = 1,
		Exam = 2,
		SessioneLaurea = 3,
		Vacanze = 4,
		ConsiglioRiunione = 5,
		Other = 6
	}
}