using System;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class StatTreeLeaf : StatBaseTreeNode, iStatTreeLeaf
    {
        public StatTreeLeafType Type { get; set; }
        public long LinkId { get; set; }
    }
}