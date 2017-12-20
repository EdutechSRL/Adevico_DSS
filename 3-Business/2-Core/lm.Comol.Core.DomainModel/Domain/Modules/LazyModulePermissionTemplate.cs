using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class LazyModulePermissionTemplate : DomainObject<int>
	{
        public virtual int IdModule { get; set; }
        public virtual int IdCommunityType { get; set; }
        public virtual int IdOrganization { get; set; }
        public virtual int IdRole { get; set; }
        public virtual string Permission { get; set; }
		public LazyModulePermissionTemplate()
		{

		}
	}
}