using System.Runtime.Serialization;
using System;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class StatBaseTreeNode : iStatBaseTreeNode
	{

	    public long  Id {get; set;}
        public string  Name {get; set;}
        public string  ToolTip {get; set;}
        public bool  isVisible {get; set;}
        public bool  Selected {get; set;}
        public int  NodeObjectTypeId {get; set;}
    }
}