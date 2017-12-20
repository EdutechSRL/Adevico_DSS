using System;
namespace lm.Comol.Core.DomainModel.Common
{
	[Serializable(), CLSCompliant(true)]
	public class DomainPresenter : iDomainPresenter
	{
		/// <summary>
		/// View, la pagina web o lo user control che usa il presenter
		/// </summary>
		private iDomainView _View;

		/// <summary>
		/// Application Context. 
		/// Contiene le informazioni generiche del sistema (stringhe di connessione, parametri di configurazione, etc...)
		/// e le informazioni sull'utente corrente (dati, ruolo, comunità corrente, etc...)
		/// </summary>
		private iApplicationContext _AppContext;
		/// <summary>
		/// Manager (DAL).
		/// Accesso ai dati su db e funzioni per l'accesso ad oggetti di Core (Person, community, etc...)
		/// </summary>
		protected iDomainManager _CurrentManager;
		#region "Public Person"
		/// <summary>
		/// Application Context. 
		/// </summary>
		public iApplicationContext AppContext {
			get { return _AppContext; }
		}
		/// <summary>
		/// Data context (proprietà di AppContext)
		/// </summary>
		public iDataContext DataContext {
			get { return _AppContext.DataContext; }
		}
		/// <summary>
		/// User Context (proprità di AppContext)
		/// </summary>
		public iUserContext UserContext {
			get { return _AppContext.UserContext; }
		}
		/// <summary>
		/// View
		/// </summary>
		public iDomainView View {
			get { return _View; }
		}
		/// <summary>
		/// Manager
		/// </summary>
		public iDomainManager CurrentManager {
			get { return _CurrentManager; }
			set { _CurrentManager = value; }
		}
		#endregion

		/// <summary>
		/// Costruttore generico
		/// </summary>
		/// <param name="oContext">Application Context</param>
		public DomainPresenter(iApplicationContext oContext)
		{
			this._AppContext = oContext;
		}
		/// <summary>
		/// Costruttore generico
		/// </summary>
		/// <param name="oContext">Application Context</param>
		/// <param name="view">Pagina</param>
		/// <remarks>Usare questo costruttore per associare la pagina al presenter</remarks>
		public DomainPresenter(iApplicationContext oContext, iDomainView view)
		{
			this._View = view;
			this._AppContext = oContext;
		}

	}
}