using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class WaitingActivationPerson : DomainObject<int>
	{
        public virtual Person WaitingProfile { get; set; }
        public virtual String Code { get; set; }
        public virtual String WorkingSessionID { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
		public WaitingActivationPerson()
		{
		}
	}
}