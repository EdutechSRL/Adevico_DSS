using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Utilizzata per calcoli permessi DI PIATTAFORMA!!!
    /// EVENTUALENTE aggiungere logiche DAL READ-ONLY!!! per permessi su singoli oggetti...
    /// </summary>
    public class WbServiceGeneric : WbService
    {

#region Costruttori

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="SysParameter">Parametro non utilizzato. Passare null.</param>
        /// <param name="oContext"></param>
        public WbServiceGeneric(WbSystemParameter SysParameter, iApplicationContext oContext)
            : base(SysParameter, oContext)
        {   
            this.DAL = new DAL.WbGenericDAL(oContext);
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="SysParameter">Parametro non utilizzato. Passare null.</param>
        /// <param name="oDC"></param>
        public WbServiceGeneric(WbSystemParameter SysParameter, iDataContext oDC)
            : base(SysParameter, oDC)
        {
            this.DAL = new DAL.WbGenericDAL(oDC);
        }

#endregion

#region Implemented void

    #region System/Generics

        /// <summary>
        /// Controlla lo stato del server
        /// </summary>
        /// <returns>
        /// False: server non raggiungibile o errore server (controllare configurazione e stato server)
        /// </returns>
        public override bool ServerCheck()
        {
            return false;
        }

        /// <summary>
        /// Recupera i dati avanzati di una stanza in base al tipo della stessa
        /// </summary>
        /// <param name="Type">Tipo di stanza</param>
        /// <returns>Dati avanzati TIPIZZATI sul sistema in uso</returns>
        public override WbRoomParameter ParameterGetByType(RoomType Type)
        {
            return null;
        }

        /// <summary>
        /// Controlla la possibilità di aggiungere una mail ad una determinata stanza
        /// </summary>
        /// <param name="Mail">Mail da aggiungere</param>
        /// <param name="RoomKey">Chiave stanza</param>
        /// <returns></returns>
        public override MailCheck MailServiceCheck(string Mail, string RoomKey)
        {
            return MailCheck.ParameterError;
        }

        /// <summary>
        /// Recupera l'indirizzo per l'accesso alla stanza.
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Id Utente stanza</param>
        /// <returns>
        /// Stringa vuota: non è possibile accesere
        /// URL accesso alla stanza
        /// </returns>
        /// <remarks>
        /// Per alcune implementazioni è necessario generare l'url di volta in volta ad ogni accesso.
        /// Le "vecchie" logiche sul "pubblica" sono state eliminate.
        /// </remarks>
        public override string AccessUrlExternalGet(long RoomId, long UserId)
        {
            return "";
        }

    #endregion

    #region Room management

        /// <summary>
        /// Crea una stanza
        /// </summary>
        /// <returns>Id della nuova stanza</returns>
        public override long RoomCreate(WbRoom Room, bool SysHasIdInName)
        {
            return -1;
        }

        /// <summary>
        /// Modifica una stanza
        /// </summary>
        /// <param name="User">L'utente per cui creare la stanza</param>
        /// <returns>true = updated</returns>
        public override bool RoomUpdate(WbRoom Room)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Filter">
        ///     User        required
        ///     Community   required
        ///     Other parameter:
        ///     Time range      available in time range
        ///     Invitation      le sessioni a cui l'utente è stato invitato
        ///     Open            le sessioni disponibili a tutti
        ///     ...
        /// </param>
        /// <returns></returns>
        public override IList<WbRoom> RoomsGet(Boolean IsForAdmin, int CommunityId, Int32 UserId, DTO.DTO_RoomListFilter filters, int PageIndex, int PageSize, ref int PageCount)
        {
            return null;
        }

        /// <summary>
        /// Cancella una stanza
        /// </summary>
        /// <param name="RoomId">ID della stanza da cancellare</param>
        /// <returns>True se cancellata</returns>
        public override bool RoomDelete(long RoomId)
        {
            return false;
        }

        /// <summary>
        /// Recupera stanza con tutti i dati
        /// </summary>
        /// <param name="RoomId">Id della stanza</param>
        /// <returns>Oggetto WbRoom</returns>
        public override WbRoom RoomGet(long RoomId)
        {
            return null;
        }

        /// <summary>
        /// Aggiorna i dati di una stanza
        /// </summary>
        /// <param name="RoomId">ID stanza</param>
        /// <param name="Data">Dati stanza (generici)</param>
        /// <param name="Parameters">Parametri stanza (dati avanzati, basati su integrazione)</param>
        /// <returns>La stanza con i dati aggiornati</returns>
        public override WbRoom RoomUpdateData(long RoomId, DTO.DTO_GenericRoomData Data, WbRoomParameter Parameters, bool HasIdInName)
        {
            return null;
        }

    #endregion

    #region User management

        /// <summary>
        /// Aggiunge un utente
        /// </summary>
        /// <param name="Users">Dati utente</param>
        /// <param name="RoomId">IdStanza</param>        
        public override void UsersAdd(IList<WbUser> Users, long RoomId)
        {}

        /// <summary>
        /// Recupera gli iscritti ad una stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>Lista di utente iscritti alla stanza</returns>
        public override IList<WbUser> UsersGet(long RoomId, DTO.DTO_UserListFilters Filters, int PageIndex, int PageSize, ref int PageCount)
        {
            //DA TESTARE!!! 
            return DAL.UsersGet(RoomId, Filters, PageIndex, PageSize, ref PageCount);
        }

        /// <summary>
        /// Abilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente abilitato
        /// False: errore, utente non abilitato
        /// </returns>
        public override bool UserEnable(long UserId, long RoomId)
        {
            return false;
        }

        /// <summary>
        /// Disabilita utente
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente abilitato
        /// False: errore interno, utente non abilitato
        /// </returns>
        public override bool UserDisable(int UserId, long RoomId)
        {
            return false;
        }

        /// <summary>
        /// Cancella utente dalla stanza
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="RoomId">Id Stanza</param>
        /// <returns>
        /// True: utente cancellato
        /// False: errore interno, utente non cancellato
        /// </returns>
        public override bool UserDelete(int UserId, long RoomId)
        {
            return false;
        }

        /// <summary>
        /// Aggiorna dati utente
        /// </summary>
        /// <param name="Users">Lista dati utente</param>
        /// <param name="RoomId">Stanza</param>
        /// <returns>
        /// True:   utenti aggiornati
        /// False:  errore aggiornamento
        /// </returns>
        public override bool UsersUpdate(IList<WbUser> Users, long RoomId)
        {
            return false;
        }

        /// <summary>
        /// Aggiunge un utente GIA' in COMOL al sistema esterno e restituisce il relativo ID.
        /// Se stringa vuota, l'utente non è stato inserito!
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public override void UserAddToExternalSystem(ref WbUser User)
        { }

    #endregion


#endregion


        public override void RoomRecordingUpdate()
        {
            throw new NotImplementedException();
        }

        public override void UserUpateInternal(long UserId, string Name, string SName, string Mail)
        {
            throw new NotImplementedException();
        }

        public override bool RoomNameExternalUpdate(string RoomExternalId, string NewName)
        {
            //Farà un ROLLBACK nel caso in cui venga chiamato!
            return false;
        }
    }
}