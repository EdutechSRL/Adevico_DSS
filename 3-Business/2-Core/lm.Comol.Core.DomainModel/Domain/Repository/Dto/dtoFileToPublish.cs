
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoModuleFileToPublish
	{
		public long FileID  {get;set;}
		public string FileName {get;set;}
		public string Extension  {get;set;}
		public int CategoryID  {get;set;}
        public bool IsVisible { get; set; }
	}
}