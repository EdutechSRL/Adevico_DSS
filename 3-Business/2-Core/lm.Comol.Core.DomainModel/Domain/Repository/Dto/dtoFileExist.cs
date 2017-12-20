
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoFileExist<T> : DomainObject<T>
	{
        public string ExistFileName { get; set; }
        public string ProposedFileName { get; set; }
        public string Extension { get; set; }
        public bool HighLight { get; set; }
	}
}