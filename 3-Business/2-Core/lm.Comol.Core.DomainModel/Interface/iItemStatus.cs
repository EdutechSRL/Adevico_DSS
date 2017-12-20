using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iItemStatus : lm.Comol.Core.DomainModel.iDomainObject<Guid>
    {
		MetaApprovationStatus Approvation { get; set; }
	}
}