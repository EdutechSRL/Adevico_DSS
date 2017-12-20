using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iLanguage : iDomainObject<int>
	{

		string Name { get; set; }
		string Code { get; set; }
		bool isDefault { get; set; }
		string Icon { get; set; }
        string ShortCode { get;  }
		//Person Sign() As String 'Sigla?????

	}
}