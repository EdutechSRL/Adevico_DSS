using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public enum ObjectStatus
	{
		Deleted,
		Active,
		All,
		AllbutOnlyMyDeleted
	}
}