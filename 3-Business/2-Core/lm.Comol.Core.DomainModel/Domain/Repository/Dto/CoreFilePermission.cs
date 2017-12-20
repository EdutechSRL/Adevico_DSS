
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class CoreFilePermission : iCoreFilePermission
	{
		public virtual bool UnDelete {get;set;}
		public virtual bool Delete {get;set;}
		public virtual bool VirtualDelete  {get;set;}
		public virtual bool Unlink {get;set;}
		public virtual bool Link {get;set;}
		public bool Publish {get;set;}
		public virtual bool EditStatus  {get;set;}
		public virtual bool Download  {get;set;}
		public virtual bool Play  {get;set;}
		public bool ViewPersonalStatistics  {get;set;}
		public bool ViewStatistics  {get;set;}
		public bool EditRepositoryVisibility  {get;set;}
		public bool EditVisibility  {get;set;}
		public bool EditMetadata {get;set;}
		public bool EditRepositoryPermission  {get;set;}
        public bool ViewPermission { get; set; }

		public CoreFilePermission()
		{
		}

	}
}