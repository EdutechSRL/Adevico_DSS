
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CommunityFilePersonTypeAssignment : CommunityFileAssignment
	{


		private int _AssignedTo;
		public virtual int AssignedTo {
			get { return _AssignedTo; }
			set { _AssignedTo = value; }
		}


		public CommunityFilePersonTypeAssignment()
		{
		}
	}
}