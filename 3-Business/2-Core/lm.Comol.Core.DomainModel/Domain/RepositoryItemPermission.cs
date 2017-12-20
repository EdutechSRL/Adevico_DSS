
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class RepositoryItemPermission
	{
        public virtual bool UnDelete { get; set; }
        public virtual bool Delete { get; set; }
		public virtual bool Download { get; set; }
		public virtual bool VirtualDelete { get; set; }
		public virtual bool Link  { get; set; }
		public virtual bool Play  { get; set; }
		public virtual bool ViewBaseStatistics  { get; set; }
		public virtual bool ViewAdvancedStatistics  { get; set; }
		public virtual bool ViewPermission { get; set; }
		public virtual bool Edit  { get; set; }
        public virtual bool EditPermission { get; set; }
        public virtual bool EditSettings { get; set; }
		public RepositoryItemPermission()
		{
		}
	}
}