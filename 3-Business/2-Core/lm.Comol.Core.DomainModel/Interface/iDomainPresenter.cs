
using lm.Comol.Core.DomainModel;
using System;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface iDomainPresenter
	{

		iApplicationContext AppContext { get; }
		iDataContext DataContext { get; }

		iUserContext UserContext { get; }
		iDomainView View { get; }
		iDomainManager CurrentManager { get; set; }
	}
}