using System;
namespace lm.Comol.Core.DomainModel
{
	/// <summary>
	/// Base Object di dominio con dati "lite".
	/// </summary>
	/// <typeparam name="T">TIPO dell'identificativo (ID). Di norma Int64.</typeparam>
	/// <remarks>
	///     Per la creazione e la modifica delle informazioni degli oggetti base, utilizzare sempre le funzioni "CreateMetaInfo" ed "UpdateMetaInfo" in fase di creazione e modifica degli oggetti.
	///     Le informazioni sono contenute nello UserContext.
	/// </remarks>
	[Serializable()]
	public class DomainBaseObjectLiteMetaInfo<T> : DomainBaseObject<T>
	{
		/// <summary>
		/// Creatore dell'oggetto
		/// </summary>
		public virtual litePerson CreatedBy { get; set; }
		/// <summary>
		/// Data di creazione dell'oggetto
		/// </summary>
		public virtual DateTime? CreatedOn { get; set; }
		/// <summary>
		/// Indirizzo IP del proxy individuato per il creatore dell'oggetto
		/// </summary>
		public virtual String CreatorProxyIpAddress { get; set; }
		/// <summary>
		/// Indirizzo IP individuato per il creatore dell'oggetto
		/// </summary>
		public virtual String CreatorIpAddress { get; set; }
		/// <summary>
		/// Che ha eseguito l'ultima modifica sull'oggetto
		/// </summary>
		public virtual litePerson ModifiedBy { get; set; }
		/// <summary>
		/// Data di ultima modifica, se è stato modificato
		/// </summary>
		public virtual DateTime? ModifiedOn { get; set; }
		/// <summary>
		/// Indirizzo IP del proxy individuato per chi ha eseguito l'ultima modifica sull'oggetto
		/// </summary>
		public virtual String ModifiedProxyIpAddress { get; set; }
		/// <summary>
		/// Indirizzo IP individuato per chi ha eseguito l'ultima modifica sull'oggetto
		/// </summary>
		public virtual String ModifiedIpAddress { get; set; }

		/// <summary>
		/// Costruttore vuoto
		/// </summary>
		public DomainBaseObjectLiteMetaInfo()
		{
			Deleted = DomainModel.BaseStatusDeleted.None;
		}
		/// <summary>
		/// Inizializza i parametri standard
		/// </summary>
		/// <param name="person">Oggetto LitePerson corrente, che identifica il creatore.</param>
		public virtual void CreateMetaInfo(litePerson person)
		{
			CreatedBy = person;
			CreatedOn = DateTime.Now;
			UpdateMetaInfo(person, BaseStatusDeleted.None);
		}

		/// <summary>
		/// Inizializza i parametri del domain Object
		/// </summary>
		/// <param name="person">Oggetto LitePerson corrente, che identifica il creatore.</param>
		/// <param name="IpAddress">Indirizzo IP dell'utente corrente</param>
		/// <param name="ProxyIpAddress">Indirizzo IP del proxy dell'utente corrente</param>
		/// <param name="date"></param>
		/// <remarks>
		/// Utilizzare sempre questa funzione prima del salvataggio in fase di creazione dell'oggetto
		/// </remarks>
		public virtual void CreateMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
		{
			CreatedBy = person;
			CreatedOn = (date.HasValue)? date:DateTime.Now;
			CreatorIpAddress = IpAddress;
			CreatorProxyIpAddress = ProxyIpAddress;
			UpdateMetaInfo(person, BaseStatusDeleted.None);
		}
		/// <summary>
		/// Aggiorna i parametri per la modifica
		/// </summary>
		/// <param name="user">Utente corrente che effettua la modifica</param>
		/// <param name="date">Data ultima modifica</param>
		public virtual void UpdateMetaInfo(litePerson user, DateTime? date = null) 
		{
			ModifiedBy = user;
			ModifiedOn = (date.HasValue) ? date.Value : DateTime.Now;
		}
		/// <summary>
		/// Aggiorna i parametri per la modifica
		/// </summary>
		/// <param name="user">Utente corrente che effettua la modifica</param>
		/// <param name="delete">Stato cancellazione (vedi enum)</param>
		/// <param name="date">Data modifica</param>
		public virtual void UpdateMetaInfo(litePerson user, BaseStatusDeleted delete, DateTime? date = null)
		{
			UpdateMetaInfo(user);
			Deleted = delete;
		}

		/// <summary>
		/// Aggiorna i parametri per la modifica
		/// </summary>
		/// <param name="user">Utente che effettua la modifica</param>
		/// <param name="IpAddress">Indirizzo IP dell'utente corrente</param>
		/// <param name="ProxyIpAddress">Indirizzo IP del Proxy dell'utente corrente</param>
		/// <remarks>
		/// Di norma utilizzare questa funzione.
		/// </remarks>
		public virtual void UpdateMetaInfo(litePerson user, string IpAddress, string ProxyIpAddress)
		{
			UpdateMetaInfo(user);
			ModifiedIpAddress = IpAddress;
			ModifiedProxyIpAddress = ProxyIpAddress;
		}
		/// <summary>
		/// Aggiorna i parametri per la modifica
		/// </summary>
		/// <param name="user">Utente che effettua la modifica</param>
		/// <param name="IpAddress">Indirizzo IP dell'utente corrente</param>
		/// <param name="ProxyIpAddress">Indirizzo IP del Proxy dell'utente corrente</param>
		/// <param name="date">Data modifica (da usare quando diversa da quella corrente)</param>
		public virtual void UpdateMetaInfo(litePerson user, string IpAddress, string ProxyIpAddress,DateTime date)
		{
			UpdateMetaInfo(user, date);
			ModifiedIpAddress = IpAddress;
			ModifiedProxyIpAddress = ProxyIpAddress;
		}
		/// <summary>
		/// Aggiorna i parametri per la cancellazione logica
		/// </summary>
		/// <param name="person">Utente che effettua la modifica</param>
		/// <param name="IpAddress">Indirizzo IP dell'utente corrente</param>
		/// <param name="ProxyIpAddress">Indirizzo IP del Proxy dell'utente corrente</param>
		/// <param name="date">Data cancellazione</param>
		public virtual void SetDeleteMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
		{
			ModifiedIpAddress = IpAddress;
			ModifiedProxyIpAddress = ProxyIpAddress;
			UpdateMetaInfo(person, BaseStatusDeleted.Manual, date);
		}
		/// <summary>
		/// Aggiorna i dati per il recupero di un oggetto con cancellazione logica.
		/// </summary>
		/// <param name="person">Utente che effettua la modifica</param>
		/// <param name="IpAddress">Indirizzo IP dell'utente corrente</param>
		/// <param name="ProxyIpAddress">Indirizzo IP del Proxy dell'utente corrente</param>
		/// <param name="date">Data cancellazione</param>
		public virtual void RecoverMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null )
		{
			ModifiedIpAddress = IpAddress;
			ModifiedProxyIpAddress = ProxyIpAddress;
			UpdateMetaInfo(person, BaseStatusDeleted.None, date);
		}
	}
}