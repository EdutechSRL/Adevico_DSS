using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class liteCommunityFile : liteBaseCommunityFile
	{
        public virtual int Level { get; set; }
        public virtual int DisplayOrder { get; set; }
        public liteCommunityFile()
            : base()
		{
			DisplayOrder = 1;
		}
	}
}