
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iCoreFilePermission
	{
		bool UnDelete { get; set; }
		bool Delete { get; set; }
		bool VirtualDelete { get; set; }
		bool Unlink { get; set; }
		bool Link { get; set; }
		bool Publish { get; set; }

		bool EditStatus { get; set; }
		bool Download { get; set; }
		bool Play { get; set; }
		bool ViewPersonalStatistics { get; set; }
		bool ViewStatistics { get; set; }
		bool EditVisibility { get; set; }
		bool EditRepositoryVisibility { get; set; }
		bool EditRepositoryPermission { get; set; }
		bool ViewPermission { get; set; }
		bool EditMetadata { get; set; }
	}
}