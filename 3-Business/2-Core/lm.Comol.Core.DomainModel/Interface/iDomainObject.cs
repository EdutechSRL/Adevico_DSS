using System;
namespace lm.Comol.Core.DomainModel
{

	[CLSCompliant(true)]
	public interface iDomainObject<T>
	{

		T Id { get; set; }
	}

}