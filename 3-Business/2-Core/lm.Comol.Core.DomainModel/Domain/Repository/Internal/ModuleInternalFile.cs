
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class ModuleInternalFile : BaseCommunityFile
	{
		public virtual int ServiceActionAjax {get;set;}
		public virtual string ServiceOwner  {get;set;}
		protected virtual string FQN {get;set;}
		public virtual int ObjectTypeID {get;set;}
		public virtual object ObjectOwner {get;set;}

        public ModuleInternalFile()
		{
		}
	}
}