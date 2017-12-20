
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class TimeZone : lm.Comol.Core.DomainModel.DomainObject<Guid>, iTimeZone
	{

		public virtual string FullDescription {
            get { return string.Format("(GMT {0} {1}:{2}) {3}", (Hours >= 0 ? "+" : "-"), Hours, Minutes.ToString("dd"), Description); }
		}
        public virtual int Hours { get; set; }
        public virtual int Minutes { get; set; }
        public virtual string Description { get; set; }

		public TimeZone()
		{
            Hours = 0;
            Minutes = 0;
            Description = string.Empty;
		}
	}
}