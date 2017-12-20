
using System.Runtime.Serialization;
using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class StatFileTreeLeaf : StatTreeLeaf
	{
		public string Extension;
		public System.Guid UniqueID;
		public bool isScorm;
		public long DownloadCount;
	}
}