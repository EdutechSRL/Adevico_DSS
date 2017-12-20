using System;
using System.Linq;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class Language : DomainObject<int>, iLanguage
	{

        public virtual string Code { get; set; }
        public virtual string Icon { get; set; }
        public virtual bool isDefault { get; set; }
        public virtual string Name { get; set; }

        public virtual String ShortCode { get { return (String.IsNullOrEmpty(Code) ? "" : (Code.Contains("-") ? Code.Split('-').FirstOrDefault().ToUpper() : Code.ToUpper())); } }
	}
}