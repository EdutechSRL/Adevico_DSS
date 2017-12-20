using System;
namespace lm.Comol.Core.DomainModel
{
	/// <summary>
	/// Stato cancellazione logica
	/// </summary>
	[Serializable(), Flags(), CLSCompliant(true)]
	public enum BaseStatusDeleted //specifica come è stato cancellato il file
	{
		/// <summary>
		/// Oggetto non cancellato
		/// </summary>
		None = 0,
		/// <summary>
		/// Cancellazione manuale (da azione utente diretta)
		/// </summary>
		Manual = 1,
		/// <summary>
		/// Cancellazione automatica (logiche di sistema)
		/// </summary>
		Automatic = 2,
		/// <summary>
		/// Cancellazione in cascata.
		/// Da utilizzare nel caso in cui l'oggetto padre abbia cancellazione manuale.
		/// In questo modo, il recupero dell'oggetto padre permette di distiguere quali oggetti figli siano stati cancellati
		/// da un azione utente (Manual) o in seguito alla cancellazione del padre (Cascade).
		/// </summary>
		Cascade = 4
	}

	/// <summary>
	/// Oggetto di dominio base
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable(), CLSCompliant(true)]
	public class DomainBaseObject<T> : iDomainBaseObject<T>
	{
		/// <summary>
		/// ID dell'oggetto.
		/// T è il tipo, di norma Int64. I Guid sono utilizzati quando necessario.
		/// </summary>
		public virtual T Id { get; set; }
		/// <summary>
		/// Stato cancellazione logica
		/// </summary>
		public virtual BaseStatusDeleted Deleted { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public virtual byte[] TimeStamp { get; set; }
		/// <summary>
		/// Costruttore.
		/// </summary>
		public DomainBaseObject() {
			Deleted = BaseStatusDeleted.None;
		}
	}
}