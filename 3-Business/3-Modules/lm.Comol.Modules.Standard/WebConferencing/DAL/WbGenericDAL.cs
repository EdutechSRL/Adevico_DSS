using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.WebConferencing.Domain;

namespace lm.Comol.Modules.Standard.WebConferencing.DAL
{

    /// <summary>
    /// Solo accesso ai dati su dB.
    /// I permessi e le logiche di business sono gestite in modo AUTONOMO dai relativi SERVICE!
    /// </summary>
    public class WbGenericDAL : lm.Comol.Core.Business.BaseCoreServices
    {
       
#region "Init class"

        public WbGenericDAL():base() {
        
        }
        public WbGenericDAL(iApplicationContext oContext) : base(oContext)
        {

        }
        public WbGenericDAL(iDataContext oDC)
            : base(oDC)
        {

        }

#endregion
        
#region Private
        protected const int maxItemsForQuery = 100;
        /// <summary>
        /// Person corrente (per created/modify by)
        /// </summary>
        private lm.Comol.Core.DomainModel.Person CurrentPerson
        {
            get
            {
                Person user = Manager.Get<lm.Comol.Core.DomainModel.Person>(UC.CurrentUserID);
                if (user == null)
                {
                    user = Manager.GetUnknownUser();
                }
                return user;
            }
        }
#endregion

#region Room

    #region Create/Update

        /// <summary>
        /// Crea stanza
        /// </summary>
        /// <param name="Room">Parametri stanza</param>
        /// <returns>ID nuova stanza</returns>
        public Int64 RoomCreate(WbRoom Room)
        {
            if (Room == null)
            {
                throw new ArgumentNullException("Room", "Cannot be null.");
            }

            if (string.IsNullOrEmpty(Room.ExternalId))
            {
                throw new ArgumentNullException("Room.ExternalId", "Cannot be null.");
            }

            Int64 NewId = 0;

            Room.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<WbRoom>(Room);
                Manager.Commit();
                NewId = Room.Id;
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                NewId = -1;
                throw ex;
            }

            return NewId;
        }

        /// <summary>
        /// Aggiorna stanza
        /// </summary>
        /// <param name="Room">Parametri stanza</param>
        /// <returns>La stanza aggiornata</returns>
        public WbRoom RoomUpdate(WbRoom Room)
        {
            if (Room == null)
            {
                throw new ArgumentNullException("Room", "Cannot be null.");
            }

            if (string.IsNullOrEmpty(Room.ExternalId))
            {
                throw new ArgumentNullException("Room.ExternalId", "Cannot be null.");
            }

            WbRoom OldRoom = Manager.Get<WbRoom>(Room.Id);

            OldRoom.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (!string.IsNullOrEmpty(Room.Name))
                OldRoom.Name = Room.Name;

            OldRoom.Recording = Room.Recording;

            OldRoom.Description = Room.Description;

            OldRoom.MaxAllowUsers = Room.MaxAllowUsers;
            OldRoom.Public = Room.Public;

            OldRoom.StartDate = Room.StartDate;

            OldRoom.NotificationEnableUsr = Room.NotificationEnableUsr;
            OldRoom.NotificationDisableUsr = Room.NotificationDisableUsr;

            if (Room.EndDate == null)
            {
                if (Room.Duration > 0 && Room.StartDate != null)
                {
                    DateTime StartDT = Room.StartDate ?? DateTime.Now;
                    OldRoom.EndDate = StartDT.AddMinutes(Room.Duration);
                }
                else
                {
                    OldRoom.EndDate = null;
                }

            }
            else
            {

                OldRoom.EndDate = Room.EndDate;

                if (Room.StartDate != null)
                {
                    TimeSpan ts = (Room.EndDate ?? DateTime.Now) - (Room.StartDate ?? DateTime.Now);
                    try
                    {
                        OldRoom.Duration = (int)ts.TotalMinutes;
                    }
                    catch
                    {
                        OldRoom.Duration = -1;
                    }
                }
                else
                {
                    OldRoom.Duration = 0;
                }
            }

            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<WbRoom>(OldRoom);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                OldRoom.Id = -1;
            }

            return OldRoom;
        }

        /// <summary>
        /// Aggiorna stanza
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="RoomData">DTO con dati stanza</param>
        /// <returns>Stanza aggiornata</returns>
        public WbRoom RoomUpdate(Int64 RoomId, Domain.DTO.DTO_GenericRoomData RoomData)
        {
            if (RoomData == null)
            {
                throw new ArgumentNullException("RoomData", "Cannot be null.");
            }

            WbRoom OldRoom = Manager.Get<WbRoom>(RoomId);

            OldRoom.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (!string.IsNullOrEmpty(RoomData.Name))
                OldRoom.Name = RoomData.Name;

            OldRoom.Recording = RoomData.Recording;
            OldRoom.Description = RoomData.Description;

            OldRoom.MaxAllowUsers = RoomData.MaxAllowUsers;
            OldRoom.Public = RoomData.Public;

            OldRoom.SubCommunity = RoomData.SubCommunity;
            OldRoom.SubExternal = RoomData.SubExternal;
            OldRoom.SubSystem = RoomData.SubSystem;

            OldRoom.StartDate = RoomData.StartDate;

            OldRoom.NotificationEnableUsr = RoomData.NotificationEnableUsr;
            OldRoom.NotificationDisableUsr = RoomData.NotificationDisableUsr;

            //OldRoom.TemplateId = RoomData.TemplateId;

            if (RoomData.EndDate == null)
            {
                if (RoomData.Duration > 0 && RoomData.StartDate != null)
                {
                    DateTime StartDT = RoomData.StartDate ?? DateTime.Now;
                    OldRoom.EndDate = StartDT.AddMinutes(RoomData.Duration);
                }
                else
                {
                    OldRoom.EndDate = null;
                }

            }
            else
            {

                OldRoom.EndDate = RoomData.EndDate;

                if (RoomData.StartDate != null)
                {
                    TimeSpan ts = (RoomData.EndDate ?? DateTime.Now) - (RoomData.StartDate ?? DateTime.Now);
                    try
                    {
                        OldRoom.Duration = (int)ts.TotalMinutes;
                    }
                    catch
                    {
                        OldRoom.Duration = -1;
                    }
                }
                else
                {
                    OldRoom.Duration = 0;
                }
            }

            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<WbRoom>(OldRoom);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                OldRoom.Id = -1;
            }

            return OldRoom;
        }

        /// <summary>
        /// Aggiorna il numero di utenti recuperando il numero reale degli stessi
        /// </summary>
        /// <param name="RoomId">L'id della stanza</param>
        public void RoomUpdateUserNumber(Int64 RoomId)
        {
            int UserNumber = (from WbUser usr in Manager.GetIQ<WbUser>() where usr.RoomId == RoomId select usr.Id).Count();
            RoomUpdateUserNumber(RoomId, UserNumber);
        }

        /// <summary>
        /// Aggiorna il numero di utenti, impostandolo al numero indicato
        /// </summary>
        /// <param name="RoomId">Id stanza</param>
        /// <param name="UserNumber">Numero utente</param>
        /// <remarks>
        /// Usato internamente o nel caso in cui il numero venga fornito dal sistema esterno
        /// </remarks>
        public void RoomUpdateUserNumber(Int64 RoomId, int UserNumber)
        {
            WbRoom oRoom = Manager.Get<WbRoom>(RoomId);
            oRoom.CurrentUsersNumber = UserNumber;

            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<WbRoom>(oRoom);
                Manager.Commit();

            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
            }
        }

    #endregion

    #region Get

        /// <summary>
        /// Recupera una stanza
        /// </summary>
        /// <param name="RoomId">L'Id della stanza (COL)</param>
        /// <returns></returns>
        public WbRoom RoomGet(Int64 RoomId)
        {
            return Manager.Get<WbRoom>(RoomId);
        }

        /// <summary>
        /// Recupera ID sistema esterno
        /// </summary>
        /// <param name="RoomId">Id stanza (interno)</param>
        /// <returns>ID esterno stanza</returns>
        public String RoomGetExternaId(Int64 RoomId)
        {
            WbRoom room = Manager.Get<WbRoom>(RoomId);
            if (room != null)
            {
                return room.ExternalId;
            }
            else
            {
                return "";
            }
        }

        /// Recupera una stanza
        /// </summary>
        /// <param name="RoomId">L'Id della stanza (COL)</param>
        /// <returns></returns>
        public IList<WbRoom> RoomsGetAll()
        {
            return Manager.GetAll<WbRoom>().ToList();
        }
        /// <summary>
        /// Recupera lista stanze
        /// </summary>
        /// <param name="CommunityId">ID Comunità</param>
        /// <param name="PersonId">Id persona</param>
        /// <returns>Lista stanze in cui l'utente è abilitato per quella comunità</returns>
        /// <remarks>
        /// Manca paginazione!
        /// </remarks>
        public IList<WbRoom> RoomsGet(Boolean IsForAdmin, int CommunityId, Int32 PersonId, Domain.DTO.DTO_RoomListFilter Filters, int PageIndex, int PageSize, ref int PageCount)
        {
            List<Int64> RoomsId = new List<Int64>();
            

            //IEnumerable<WbUser> query = from WbUser u in Manager.GetIQ<WbUser>() where u.RoomId == RoomId select u;

            switch(Filters.Access)
            {
                case RoomListFilterAccess.access:   //ok - V
                    //Tutte quelle a cui ho accesso

                    //Tutte iscritto non bloccato
                    var UsrNotLocked = (from WbUser usr in Manager.GetIQ<WbUser>()
                                        where usr.PersonID == PersonId && usr.Enabled == true
                                        select usr.RoomId).ToList();

                    //Tutte di Comunita
                    var CommunityALL = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                         where rm.CommunityId == CommunityId
                                         select rm.Id).ToList();

                    //Tutte di comunità, iscritto non bloccato
                    var ComUsrSubscribedNotLocked = UsrNotLocked.Intersect(CommunityALL).ToList();




                    //Tutte di comunità a cui mi posso iscrivere
                    var ComCanSubscribe = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                          where rm.CommunityId == CommunityId && (rm.SubCommunity != SubscriptionType.None)
                          select rm.Id).ToList();

                    //Tutte bloccato
                    var UsrLocked = (from WbUser usr in Manager.GetIQ<WbUser>() 
                               where usr.PersonID == PersonId && usr.Enabled == false
                                        select usr.RoomId).ToList();

                    //Tutte di comunità a cui mi posso iscrivere e NON sono bloccato
                    var UserComCanSubscribeUnlocked = ComCanSubscribe.Except(UsrLocked).ToList();

                    //Iscritto non bloccato +
                    //Posso iscrivermi non bloccato
                    //Distinct per evitare doppioni
                    RoomsId = ComUsrSubscribedNotLocked.Union(UserComCanSubscribeUnlocked).Distinct().ToList();
                    //Union               <- System.NotImplementedException !!!

                    break;

                case RoomListFilterAccess.created:  //ok
                    RoomsId = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                            where rm.CommunityId == CommunityId && rm.CreatedBy.Id == PersonId
                            select rm.Id).ToList();
                    break;

                case RoomListFilterAccess.subscribed:   //ok
                    //Tutte di comunità
                    var ComRid = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                      where rm.CommunityId == CommunityId
                                      select rm.Id).ToList();

                    //Tutte iscritto
                    var SubsRid = (from WbUser usr in Manager.GetIQ<WbUser>()
                                         where usr.PersonID == PersonId && usr.Enabled == true
                                         select usr.RoomId);

                    RoomsId = ComRid.Intersect(SubsRid).ToList();
                    break;

                case RoomListFilterAccess.all:  //ok?
                    //                      Tutte quelle a cui ho accesso

                    if (IsForAdmin)
                    {
                        //var all_CommunityALL =
                        RoomsId = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                   where rm.CommunityId == CommunityId
                                   select rm.Id).ToList();
                    }
                    else { 

                        //Tutte iscritto non bloccato
                        var all_UsrNotLocked = (from WbUser usr in Manager.GetIQ<WbUser>()
                                            where usr.PersonID == PersonId && usr.Enabled == true
                                            select usr.RoomId).ToList();

                        //Tutte di Comunita
                        var all_CommunityALL = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                            where rm.CommunityId == CommunityId
                                            select rm.Id).ToList();

                        //Tutte di comunità, iscritto non bloccato
                        var all_ComUsrSubscribedNotLocked = all_UsrNotLocked.Intersect(all_CommunityALL).ToList();

                        //Tutte di comunità a cui mi posso iscrivere
                        var all_ComCanSubscribe = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                               where rm.CommunityId == CommunityId && (rm.SubCommunity != SubscriptionType.None)
                                               select rm.Id).ToList();

                        //Tutte bloccato
                        var all_UsrLocked = (from WbUser usr in Manager.GetIQ<WbUser>()
                                         where usr.PersonID == PersonId && usr.Enabled == false
                                         select usr.RoomId).ToList();

                        //Tutte di comunità a cui mi posso iscrivere e NON sono bloccato
                        var all_UserComCanSubscribeUnlocked = all_ComCanSubscribe.Except(all_UsrLocked).ToList();


                        var all_Access = all_ComUsrSubscribedNotLocked.Union(all_UserComCanSubscribeUnlocked).Distinct().ToList();

                        // Tutte create in comunità
                        var all_Created = (from WbRoom rm in Manager.GetIQ<WbRoom>()
                                where rm.CommunityId == CommunityId && rm.CreatedBy.Id == PersonId
                                select rm.Id).ToList();

                        RoomsId = all_Access.Union(all_Created).Distinct().ToList();
                    }

                    break;
            }

            //

            ////Recupero le Room, secondo i filtri impostati

            //IEnumerable<WbRoom> Rooms = Manager.GetAll<WbRoom>(rm => RoomsId.Contains(rm.Id));
            IEnumerable<WbRoom> Rooms = from WbRoom rm in Manager.GetIQ<WbRoom>()
                                        where RoomsId.Contains(rm.Id) && rm.Deleted == BaseStatusDeleted.None
                                        select rm;

            if (Filters.Visibility != RoomListFilterVisibility.all)
            {
                Boolean filtPublic = true;

                if (Filters.Visibility == RoomListFilterVisibility.Private)
                    filtPublic = false;

                Rooms = Rooms.Where(rm => rm.Public == filtPublic);
            }

            if (Filters.Type != RoomListFilterType.all)
            {
                Rooms = Rooms.Where(rm => rm.Type == ConvType(Filters.Type));
            }





            //Riordino stanze.
            if (Filters.OrderDir)
            {
                switch (Filters.OrderBy)
                {
                    case RoomListOrder.Name:
                        Rooms = Rooms.OrderBy(rm => rm.Name);
                        break;
                    case RoomListOrder.Start:
                        Rooms = Rooms.OrderBy(rm => rm.StartDate);
                        break;
                    case RoomListOrder.Status:
                        Rooms = Rooms.OrderBy(rm => rm.Public);
                        break;
                    case RoomListOrder.Type:
                        Rooms = Rooms.OrderBy(rm => rm.Type);
                        break;
                }
            }
            else
            {
                switch (Filters.OrderBy)
                {
                    case RoomListOrder.Name:
                        Rooms = Rooms.OrderByDescending(rm => rm.Name);
                        break;
                    case RoomListOrder.Start:
                        Rooms = Rooms.OrderByDescending(rm => rm.StartDate);
                        break;
                    case RoomListOrder.Status:
                        Rooms = Rooms.OrderByDescending(rm => rm.Public);
                        break;
                    case RoomListOrder.Type:
                        Rooms = Rooms.OrderByDescending(rm => rm.Type);
                        break;
                }
            }


            // PAGINAZIONE:


            // !! ATTENZIONE !!
            PageCount = (from WbRoom rm in Rooms select rm.Id).ToList().Count();

            return Rooms.Skip(PageIndex * PageSize).Take(PageSize).ToList();

           
            //if (Filters.Type == RoomListFilterType.all)
            //{
            //    if (Filters.Visibility == RoomListFilterVisibility.all)
            //    {
            //        Rooms = Rooms.Where(rm => RoomsId.Contains(rm.Id));
            //        //Manager.GetAll<WbRoom>(rm => RoomsId.Contains(rm.Id)).ToList();

            //        Pager.Count = RoomsId.Count();
            //    }
            //    else
            //    {
            //        Boolean filtPublic = true;
            //        if(Filters.Visibility == RoomListFilterVisibility.Private)
            //        {
            //            filtPublic = false;
            //        }

            //        Rooms = Manager.GetAll<WbRoom>(rm => 
            //            RoomsId.Contains(rm.Id) &&
            //            rm.Public == filtPublic
            //            ).ToList();
            //    }
            //}
            //else
            //{
            //    if (Filters.Visibility == RoomListFilterVisibility.all)
            //    {
            //        Rooms = Manager.GetAll<WbRoom>(rm => 
            //            RoomsId.Contains(rm.Id) &&
            //            rm.Type == ConvType(Filters.Type)
            //            ).ToList();
            //    }
            //    else
            //    {
            //        Boolean filtPublic = true;
            //        if (Filters.Visibility == RoomListFilterVisibility.Private)
            //        {
            //            filtPublic = false;
            //        }

            //        Rooms = Manager.GetAll<WbRoom>(rm =>
            //            rm.Type == ConvType(Filters.Type) &&
            //            rm.Public == filtPublic &&
            //            RoomsId.Contains(rm.Id)
            //            ).ToList();
            //    }
            //}







            //OLD CODE!
            ////Stanza in cui l'utente è abilitato
            //List<Int64> RoomsId = (from WbUser usr in Manager.GetIQ<WbUser>()
            //                       where usr.PersonID == UserId && usr.Enabled == true
            //                       select usr.RoomId).ToList();

            //List<Int64> DisabledRoomsId = (from WbUser usr
            //                                    in Manager.GetIQ<WbUser>()
            //                             where usr.PersonID == UserId && usr.Enabled == false
            //                             select usr.RoomId).ToList();

            //List<WbRoom> Rooms = (from WbRoom room
            //                        in Manager.GetAll<WbRoom>(rm =>
            //                            rm.CommunityId == CommunityId && (rm.CreatedBy.Id == UserId || RoomsId.Contains(rm.Id) || (rm.Public && !DisabledRoomsId.Contains(rm.Id))))
            //                      select room).ToList();

            //return Rooms;
        }

        private RoomType ConvType(RoomListFilterType fType)
        {
            switch(fType)
            {
                case RoomListFilterType.Chat:
                    return RoomType.VideoChat;
                case RoomListFilterType.Conference:
                    return RoomType.Conference;
                case RoomListFilterType.Custom:
                    return RoomType.Custom;
                case RoomListFilterType.Lesson:
                    return RoomType.Lesson;
                case RoomListFilterType.Meetings:
                    return RoomType.Meeting;
                default:
                    return RoomType.Meeting;
            }
        }
    #endregion

    #region Delete

        /// <summary>
        /// Cancella stanza
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <returns>True: stanza cancellata, False: errore</returns>
        public Boolean RoomDelete(Int64 RoomId)
        {
            WbRoom Room = Manager.Get<WbRoom>(RoomId);
            return RoomDelete(Room);
        }

        /// <summary>
        /// Cancella stanza
        /// </summary>
        /// <param name="Room">Oggetto stanza da cancellare</param>
        /// <returns>True: stanza cancellata, False: errore</returns>
        public Boolean RoomDelete(WbRoom Room)
        {

            IList<Int64> UsersId = (from WbUser usr in Manager.GetAll<WbUser>(u => u.RoomId == Room.Id) select usr.Id).ToList();

            try
            {
                Manager.BeginTransaction();
                Manager.DeletePhysicalList<WbUser>(UsersId);
                Manager.DeletePhysical<WbRoom>(Room);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }

            return true;
        }

    #endregion

    #region RoomCode
    
        public void RoomCodeGenerate(Int64 RoomId)  // String Code)
        {
            WbAccessCode WbCode = new WbAccessCode();
            if (RoomId > 0)
                WbCode = Manager.GetAll<WbAccessCode>(c => c.RoomId == RoomId).FirstOrDefault();

            if (RoomId < 0 || WbCode == null)
            {
                WbCode = new WbAccessCode();
                WbCode.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                WbCode.RoomId = RoomId;

                Manager.SaveOrUpdate<WbAccessCode>(WbCode);
            }
            else
                WbCode.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            WbCode.UrlCode = CodeHelper.GenerateCode(WbCode.Id);
            Manager.SaveOrUpdate<WbAccessCode>(WbCode);
        }

        public void RoomCodeDelete(Int64 RoomId)
        {
            WbAccessCode WbCode = new WbAccessCode();
            if (RoomId > 0)
                WbCode = Manager.GetAll<WbAccessCode>(c => c.RoomId == RoomId).FirstOrDefault(); // && cod.UserId <= 0

            if (RoomId < 0 || WbCode == null)
            {
                WbCode = new WbAccessCode();
                WbCode.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                WbCode.RoomId = RoomId;

                Manager.SaveOrUpdate<WbAccessCode>(WbCode);
            }
            else
                WbCode.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            WbCode.UrlCode = "";
            Manager.SaveOrUpdate<WbAccessCode>(WbCode);
        }

        public String RoomCodeGet(Int64 RoomId)
        {
            String Code = (from WbAccessCode cod in Manager.GetAll<WbAccessCode>(c => c.RoomId == RoomId)
                           select cod.UrlCode).FirstOrDefault();
            return Code;
        }
    #endregion

        //public void RoomTemplateUpdate(Int64 RoomId, Int64 TemplateId)
        //{
        //    WbRoom oRoom = Manager.Get<WbRoom>(RoomId);
            
        //    if (oRoom == null)
        //        return;

        //    //oRoom.TemplateId = TemplateId;
        //    oRoom.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    Manager.SaveOrUpdate<WbRoom>(oRoom);
        //}
#endregion

#region User

    #region Create/Update

        /// <summary>
        /// Crea/aggiorna utente
        /// </summary>
        /// <param name="Usr">Utente da aggiornare</param>
        public void UserSaveOrUpdate(WbUser Usr)
        {
            if (Usr.Id <= 0)
                Usr.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            else
                Usr.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<WbUser>(Usr);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
            }
        }

    #endregion

    #region Get

        /// <summary>
        /// Recupera l'ID sistema esterno di una person
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="PersonId">ID Persona</param>
        /// <returns></returns>
        public String UserGetExternalIdFromPerson(Int64 RoomId, Int32 PersonId)
        {

            WbUser user = (from WbUser usr in Manager.GetAll<WbUser>(
                               u => u.PersonID == PersonId && u.RoomId == RoomId && (u.Enabled == true || u.MailChecked == true)
                               )
                           select usr).FirstOrDefault();
            //where usr.PersonID == PersonId && usr.RoomId == RoomId && (usr.Enabled == true || usr.MailChecked == true)

            if (user != null)
            {
                return user.ExternalID;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Recupera l'ID sistema esterno di un utente
        /// </summary>
        /// <param name="UserId">ID Utente stanza</param>
        /// <returns>ID esterno utente</returns>
        public String UserGetExternalIdFromUser(Int64 UserId)
        {
            WbUser user = Manager.Get<WbUser>(UserId);

            if (user != null)
            {
                return user.ExternalID;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// Recupera lista utenti stanza
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="Filters">Filtri. New Object: all</param>
        /// <param name="PageCount">REf. Numero pagine reali, dato pagesize</param>
        /// <param name="PageIndex">Indice corrente per paginazione. -1 = tutte</param>
        /// <param name="PageSize">Dimensione paginazione. -1 = tutte</param>
        /// <returns>Lista utenti</returns>
        public IList<WbUser> UsersGet(Int64 RoomId, Domain.DTO.DTO_UserListFilters Filters, int PageIndex, int PageSize, ref int PageCount)
        {
            if (Filters == null)
                Filters = new Domain.DTO.DTO_UserListFilters();

            //List<WbUser> UsersGet = new List<WbUser>();

            IEnumerable<WbUser> query = from WbUser u in Manager.GetIQ<WbUser>() where u.RoomId == RoomId select u;

            switch (Filters.UserType)
            {
                case UserListUsertype.external:
                    query = query.Where(u => u.PersonID <= 0);
                    break;
                case UserListUsertype.system:
                    query = query.Where(u => u.PersonID > 0);
                    break;
            }

            List<WbUser> TEst = query.ToList();
            
            if (!String.IsNullOrEmpty(Filters.SearchString) && !String.IsNullOrEmpty(Filters.SearchString.Trim()))
            {
                Filters.SearchString = Filters.SearchString.ToLower();

                switch(Filters.SearchBy)
                {
                    case UserListSearchBy.Mail:
                        query = query.Where(u => u.Mail.ToLower().Contains(Filters.SearchString));
                        break;
                    case UserListSearchBy.Name:
                        query = query.Where(u => !String.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(Filters.SearchString));
                        break;
                    case UserListSearchBy.Surename:
                        query = query.Where(u => !String.IsNullOrEmpty(u.SName) && u.SName.ToLower().Contains(Filters.SearchString));
                        break;
                }
            }

            switch (Filters.OrderBy)
            {
                case UserListOrderBy.Mail:
                    if (Filters.OrderDir)
                        query = query.OrderBy(u => u.Mail);
                    else
                        query = query.OrderByDescending(u => u.Mail);
                    break;

                case UserListOrderBy.Name:
                    if (Filters.OrderDir)
                        query = query.OrderBy(u => u.Name);
                    else
                        query = query.OrderByDescending(u => u.Name);
                    break;

                case UserListOrderBy.SureName:
                    if (Filters.OrderDir)
                        query = query.OrderBy(u => u.SName);
                    else
                        query = query.OrderByDescending(u => u.SName);
                    break;
            }


            // !! ATTENZIONE !!
            PageCount = (from WbUser usr in query select usr.Id).ToList().Count();

            if (PageCount < PageIndex)
                PageIndex = PageCount;
            
            if (PageIndex == -1 && PageSize == -1)
                return query.ToList();
            else
                return query.Skip(PageIndex * PageSize).Take(PageSize).ToList();
        }

        /// <summary>
        /// Recupera un utente IN UNA STANZA
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Person ID di COMOL</param>
        /// <returns></returns>
        public WbUser UserGetInRoomByPerson(Int32 PersonId, Int64 RoomId)
        {
            return Manager.GetAll<WbUser>(u => u.RoomId == RoomId && u.PersonID == PersonId).FirstOrDefault();
        }

        /// <summary>
        /// Recupera un utente IN UNA STANZA
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Person ID di COMOL</param>
        /// <returns></returns>
        public WbUser UserGetInRoom(Int64 UserId, Int64 RoomId)
        {
            return Manager.GetAll<WbUser>(u => u.Id == UserId && u.RoomId == RoomId).FirstOrDefault();
        }

        /// <summary>
        /// Recupera un utente dal SISTEMA, senza controllare che sia nella stanza.
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Person ID di COMOL</param>
        /// <returns></returns>
        /// <remarks>TUTTI i parametri utenti relativi alla stanza saranno ricontrollati ed aggiornati dal servizio! (Es: AVC, host, controller, etc...</remarks>
        public WbUser UserGetForRoomByPerson(Int32 PersonId, Int64 RoomId)
        {
            WbUser User = new WbUser();

            WbRoom Room = Manager.Get<WbRoom>(RoomId);
            Person Person = Manager.Get<Person>(PersonId);

            if (Room == null || Person == null)
                return null;

            User = new WbUser();

            User.Audio = false;
            User.Chat = false;
            //User.DisplayName = Person.SurnameAndName;
            User.Name = Person.Name;
            User.SName = Person.Surname;

            User.Enabled = true;
            User.ExternalRoomId = Room.ExternalId;
            User.IsController = false;
            User.IsHost = false;
            User.Mail = Person.Mail;
            User.MailChecked = true;
            User.PersonID = Person.Id;
            User.RoomId = Room.Id;
            User.SendedInvitation = 0;
            User.ShowMail = false;
            User.ShowStatus = false;
            User.Video = false;

            Language lang = Manager.Get<Language>(Person.LanguageID);
            User.LanguageCode = lang.Code;

            return User;
        }

        /// <summary>
        /// Recupera una lista di utenti per l'inserimento in un stanza da un elendo di ID Person
        /// </summary>
        /// <param name="PersonsIds">Lista di ID Person da aggiugnere</param>
        /// <param name="RoomId">Stanza</param>
        /// <param name="Audio">Abilita audio</param>
        /// <param name="Video">Abilita video</param>
        /// <param name="Chat">Abilia chat</param>
        /// <param name="IsAdmin">Iscrivi come amministratori</param>
        /// <returns></returns>
        public IList<WbUser> UsersGetForRoomByPersons(IList<Int32> PersonsIds, Int64 RoomId, Boolean Audio, Boolean Video, Boolean Chat, bool Host, bool Controller)
        {
            IList<WbUser> wUsers = new List<WbUser>();
            WbUser wUser = new WbUser();

            WbRoom Room = Manager.Get<WbRoom>(RoomId);

            IList<Person> Persons = Manager.GetAll<Person>(p => PersonsIds.Contains(p.Id)).ToList();

            if (Room == null || Persons == null || Persons.Count() == 0)

                return null;

            foreach (Person prs in Persons)
            {

                wUser = new WbUser();

                wUser.Audio = Audio;
                wUser.Chat = Chat;
                //wUser.DisplayName = prs.SurnameAndName;
                wUser.Name = prs.Name;
                wUser.SName = prs.Surname;

                wUser.Enabled = true;
                wUser.ExternalRoomId = Room.ExternalId;
                wUser.IsController = Controller;
                wUser.IsHost = Host;
                wUser.Mail = prs.Mail;
                wUser.MailChecked = true;
                wUser.PersonID = prs.Id;
                wUser.RoomId = Room.Id;
                wUser.SendedInvitation = 0;
                wUser.ShowMail = false;
                wUser.ShowStatus = false;
                wUser.Video = Video;

                Language lang = Manager.GetLanguage(prs.LanguageID);// .Get<Language>(prs.LanguageID);
                wUser.LanguageCode = lang.Code;

                wUsers.Add(wUser);
            }
            return wUsers;
        }

        /// <summary>
        /// Recupera un utente
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Person ID di COMOL</param>
        /// <returns></returns>
        public WbUser UserGet(Int64 UserId)
        {
            return Manager.Get<WbUser>(UserId);
        }

        /// <summary>
        /// Recupera un utente
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="Mail">Mail utente</param>
        /// <returns></returns>
        public WbUser UserGet(Int64 RoomId, String Mail)
        {
            WbUser User = Manager.GetAll<WbUser>(u => u.Mail == Mail && u.RoomId == RoomId).FirstOrDefault();

            if (User != null && User.Id <= 0)
                User = null;

            return User;
        }

    #endregion

    #region Delete

        /// <summary>
        /// Cancella un utente
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        /// <param name="UserId">Person ID di COMOL!</param>
        /// <returns>
        /// True if Ok, else false.
        /// </returns>
        public bool UserDelete(Int64 RoomId, Int32 UserId)
        {
            WbUser Usr = Manager.GetAll<WbUser>(u => u.RoomId == RoomId && u.PersonID == UserId).FirstOrDefault();

            if (Usr != null)
            {
                Manager.DeletePhysical<WbUser>(Usr);
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Cancella un utente
        /// </summary>
        /// <param name="Id">
        /// ID nella tabella "WM_RoomUser"!
        /// </param>
        /// <returns>
        /// True if Ok, else false.
        /// </returns>
        public bool UserDelete(Int64 Id)
        {
            WbUser usr = Manager.Get<WbUser>(Id);

            if (usr != null)
            {
                Manager.DeletePhysical<WbUser>(usr);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserDelete(Int64 RoomId, String Mail)
        {
            WbUser User = Manager.GetAll<WbUser>(u => u.Mail == Mail && u.RoomId == RoomId).FirstOrDefault();

            if(User != null)
                Manager.DeletePhysical<WbUser>(User);

            return true;
        }

        /// <summary>
        /// Cancella gli utenti da una lista di ID Esterni
        /// </summary>
        /// <param name="ExternalExtId"></param>
        public void UserDeleteExtIds(IList<String> ExternalExtId)
        {
            if (ExternalExtId != null && ExternalExtId.Count() > 0)
            {
                //IList<WbUser> ToDeleteUsr = (from WbUser usr in Manager.GetIQ<WbUser>() where ExternalExtId.Contains(usr.ExternalID) select usr).ToList();
                IList<WbUser> ToDeleteUsr = Manager.GetAll<WbUser>(usr => ExternalExtId.Contains(usr.ExternalID)).ToList();
                Manager.DeletePhysicalList<WbUser>(ToDeleteUsr);
                Manager.Commit();
                
            }
        }
    #endregion

    #region Vari

        /// <summary>
        /// Recupera una Person
        /// </summary>
        /// <param name="UserId">ID Utente</param>
        /// <returns></returns>
        public Person UserGetPerson(Int32 UserId)
        {
            return Manager.GetPerson(UserId);
        }

        /// <summary>
        /// Recupera una Person dalla mail
        /// </summary>
        /// <param name="Mail">Mail</param>
        /// <returns>Oggetto Person</returns>
        public Person UserGetPersonByMail(String Mail)
        {
            return Manager.GetAll<Person>(p => p.Mail == Mail).FirstOrDefault();
        }

        /// <summary>
        /// Genera chiave per un utente
        /// </summary>
        /// <param name="RoomId">ID Stanza</param>
        /// <param name="UserId">ID Utente</param>
        /// <remarks>Se esistente la chiave verrà cancellata</remarks>
        public void UserKeyGenerate(Int64 RoomId, Int64 UserId)
        {
            WbUser wbUser = Manager.Get<WbUser>(UserId);
            if (wbUser == null)
                return;

            wbUser.UserKey = CodeHelper.GenerateCode(wbUser.Id);    //Chiave richiesta all'utente!
            Manager.SaveOrUpdate<WbUser>(wbUser);
        }

        public int UserKeysGenerate(Int64 RoomId, IList<Int64> UsersId, Boolean Regenerate)
        {
            if (UsersId == null)
                return -1;

            int count = 0;

            IList<WbUser> Generated = new List<WbUser>();

            foreach(Int64 id in UsersId)
            {
                WbUser wbUser = Manager.Get<WbUser>(id);
                if (wbUser != null && wbUser.RoomId == RoomId && (Regenerate || String.IsNullOrEmpty(wbUser.UserKey)))
                {

                    wbUser.UserKey = CodeHelper.GenerateCode(wbUser.Id);    //Chiave richiesta all'utente!
                    wbUser.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                    Generated.Add(wbUser);
                    count++;
                }
            }

            if (Generated != null && Generated.Count > 0)
            {
                Manager.SaveOrUpdateList<WbUser>(Generated);
            }

            return count;
        }
    #endregion
        
    #region "Search users"
        /// <summary>
        /// Restituisce la lista degli stati relativi alla validazione o meno della mail degli utenti
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<MailStatus> GetAvailableMailStatus(long idRoom, UserTypeFilter userType, UserStatus status, List<long> removeUsers)
        {
            List<MailStatus> items = new List<MailStatus>();
            try
            {
                var query = (from u in Manager.GetIQ<WbUser>() where u.Deleted== BaseStatusDeleted.None && u.RoomId==idRoom && (status== UserStatus.All  || (status== UserStatus.Locked && !u.Enabled) || (status== UserStatus.Unlocked && u.Enabled)) select u);
                if (removeUsers != null && removeUsers.Any())
                {
                    if (removeUsers.Count <= maxItemsForQuery)
                        query = query.Where(u => !removeUsers.Contains(u.Id));
                    else
                        query = query.ToList().Where(u => !removeUsers.Contains(u.Id)).AsQueryable();
                }
                if (query.Where(u => !u.MailChecked).Any())
                    items.Add(MailStatus.WaitingConfirm);

                if (query.Where(u => u.MailChecked).Any() ||status== UserStatus.NotSubscribed || status== UserStatus.All || status== UserStatus.None)
                    items.Add(MailStatus.Confirmed);
               
            }
            catch (Exception ex) { 
            
            }
            return items;
        }
        /// <summary>
        /// Restituisce la lista degli stati relativi alla validazione o meno della mail degli utenti
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<UserStatus> GetAvailableUserStatus(long idRoom, UserTypeFilter userType, List<long> removeUsers)
        {
            List<UserStatus> items = new List<UserStatus>();
            try
            {
                WbRoom room = Manager.Get<WbRoom>(idRoom);
                if (room != null)
                {
                    var query = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == idRoom && (userType == UserTypeFilter.All || userType == UserTypeFilter.WithoutMembers || (userType == UserTypeFilter.Administrator && u.IsController) || (userType == UserTypeFilter.Participant && !u.IsController) || (userType == UserTypeFilter.InternalParticipant && u.PersonID >0  && !u.IsController) || (userType == UserTypeFilter.ExternalParticipant && u.PersonID < 1 && !u.IsController)) select u);
                    if (removeUsers != null && removeUsers.Any())
                    {
                        if (removeUsers.Count <= maxItemsForQuery)
                            query = query.Where(u => !removeUsers.Contains(u.Id));
                        else
                            query = (from u in query.ToList() where !removeUsers.Contains(u.Id) select u).AsQueryable();
                    }
                    if (query.Where(u => u.Enabled).Any())
                        items.Add(UserStatus.Unlocked);
                    if (query.Where(u => !u.Enabled).Any())
                        items.Add(UserStatus.Locked);
                    if ((userType == UserTypeFilter.All || userType== UserTypeFilter.GenericMail || userType == UserTypeFilter.None) && (room.SubSystem != SubscriptionType.None || room.SubCommunity != SubscriptionType.None))
                        items.Add(UserStatus.NotSubscribed);
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }
         /// <summary>
        /// Restituisce la lista degli stati relativi alla validazione o meno della mail degli utenti
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="removeUsers"></param>
        /// <returns></returns>
        public List<UserTypeFilter> GetAvailableUserTypes(ModuleObject obj, List<long> removeUsers, lm.Comol.Core.Mail.Messages.MailMessagesService service)
        {
            List<UserTypeFilter> items = new List<UserTypeFilter>();
            try
            {
                WbRoom room = Manager.Get<WbRoom>(obj.ObjectLongID);
                if (room != null)
                {
                    var query = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId ==room.Id select u);
                    if (removeUsers != null && removeUsers.Any())
                    {
                        if (removeUsers.Count <= maxItemsForQuery)
                            query = query.Where(u => !removeUsers.Contains(u.Id));
                        else
                            query = query.ToList().Where(u => !removeUsers.Contains(u.Id)).AsQueryable();
                    }
                    if (query.Where(u => u.IsController).Any())
                        items.Add(UserTypeFilter.Administrator);
                    if (query.Where(u => !u.IsController).Any())
                    {
                        items.Add(UserTypeFilter.Participant);
                        if (query.Where(u => !u.IsController && u.PersonID>0).Any() && query.Where(u => !u.IsController && u.PersonID < 0).Any()){
                            items.Add(UserTypeFilter.InternalParticipant);
                            items.Add(UserTypeFilter.ExternalParticipant);
                        }
                    }
                    if (service.HasUsersForMessage(obj, Core.Mail.Messages.MessageRecipientType.External))
                        items.Add(UserTypeFilter.GenericMail);
                    if (items.Any())
                        items.Insert(0,UserTypeFilter.WithoutMembers);
                    if (room.SubSystem!= SubscriptionType.None || room.SubCommunity != SubscriptionType.None)
                        items.Add(UserTypeFilter.None);
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        /// <summary>
        /// Mi dice se trova utenti con una determinata tipologia di profilo
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="idProfileType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Boolean HasUsersWithProfileType(long idRoom, int idProfileType, UserTypeFilter type,UserStatus status)
        {
            Boolean found = false;
            try
            {
                WbRoom room = Manager.Get<WbRoom>(idRoom);
                if (room != null) {
                    if (!found && (status== UserStatus.NotSubscribed || (status == UserStatus.All && (status== UserStatus.NotSubscribed || status== UserStatus.All))))
                    {
                        if (room.SubCommunity != SubscriptionType.None)
                            found = (from s in Manager.GetIQ<Subscription>()
                                     where s.Community != null && s.Community.Id == room.CommunityId && s.Accepted && s.Enabled
                                     select s.Person).ToList()
                                        .Where(p => p.TypeID == idProfileType).Select(p => p.Id).Any();
                        else if (room.SubSystem != SubscriptionType.None)
                            found = (from p in Manager.GetIQ<Person>()
                                     where p.TypeID == idProfileType
                                     select p.Id).Any();
                    }

                    if (!found && status!= UserStatus.NotSubscribed || (type!= UserTypeFilter.None)){
                        List<Int32> idUsers = (from u in Manager.GetIQ<WbUser>()
                                                where u.Deleted == BaseStatusDeleted.None && u.RoomId == idRoom && u.PersonID > 0 && ((status== UserStatus.Locked && u.Enabled) || (status== UserStatus.Unlocked && !u.Enabled ))
                                                select u.PersonID).ToList();
                        if (idUsers.Any()) {
                            if (idUsers.Count <= maxItemsForQuery)
                            {
                                found = (from p in Manager.GetIQ<Person>() where p.TypeID == idProfileType where idUsers.Contains(p.Id) select p.Id).Any();
                            }
                            else {
                                Int32 pageIndex = 0;
                                List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                while (idPagedUsers.Any())
                                {
                                    found = (from p in Manager.GetIQ<Person>() where p.TypeID == idProfileType where idPagedUsers.Contains(p.Id) select p.Id).Any();
                                    if (found)
                                        break;
                                    pageIndex++;
                                    idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return found;
        }

        /// <summary>
        /// Trova tutte le agenzie disponibili di una stanza
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Dictionary<long, String> GetAgenciesForUsers(long idRoom, UserTypeFilter type, UserStatus status)
        {
            Dictionary<long, String> list = new Dictionary<long, String>();
            try
            {
                List<Int32> idUsers = new List<Int32>();

                WbRoom room = Manager.Get<WbRoom>(idRoom);
                if (room != null)
                {
                    if (status == UserStatus.NotSubscribed || (status == UserStatus.All && (status== UserStatus.NotSubscribed || status== UserStatus.All)))
                    {
                        if (room.SubCommunity != SubscriptionType.None)
                            idUsers.AddRange((from s in Manager.GetIQ<Subscription>()
                                              where s.Community != null && s.Community.Id == room.CommunityId && s.Accepted && s.Enabled
                                              select s.Person).ToList()
                                        .Where(p => p.TypeID == (Int32)UserTypeStandard.Employee).Select(p => p.Id).ToList());
                        else if (room.SubSystem != SubscriptionType.None)
                            idUsers.AddRange((from p in Manager.GetIQ<Person>()
                                              where p.TypeID == (Int32)UserTypeStandard.Employee
                                     select p.Id).ToList());
                    }

                    if (status != UserStatus.NotSubscribed || (type != UserTypeFilter.None))
                    {
                        idUsers.AddRange((from u in Manager.GetIQ<WbUser>()
                                               where u.Deleted == BaseStatusDeleted.None && u.RoomId == idRoom && u.PersonID > 0 && ((status == UserStatus.Locked && u.Enabled) || (status == UserStatus.Unlocked && !u.Enabled))
                                               select u.PersonID).ToList());
                    }
                }

                list = GetAgenciesForUsers(idUsers.Distinct().ToList());
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// Recupera la lista delle agenzie data una lista di utenti
        /// </summary>
        /// <param name="idUsers"></param>
        /// <returns></returns>
        private Dictionary<long, String> GetAgenciesForUsers(List<Int32> idUsers)
        {
            Dictionary<long, String> list = new Dictionary<long, String>();
            try
            {
                List<Agency> agencies = (from a in Manager.GetIQ<Agency>() where a.Deleted == BaseStatusDeleted.None select a).ToList();
                List<long> idUserAgencies = new List<long>();
                if (idUsers.Count() <= maxItemsForQuery)
                    idUserAgencies = (from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && idUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList();
                else
                {
                    Int32 index = 0;
                    List<Int32> tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (tUsers.Any())
                    {
                        idUserAgencies.AddRange((from a in Manager.GetIQ<AgencyAffiliation>() where a.IsEnabled && a.Deleted == BaseStatusDeleted.None && tUsers.Contains(a.Employee.Id) select a.Agency.Id).Distinct().ToList());
                        index++;
                        tUsers = idUsers.Skip(index * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    idUserAgencies = idUserAgencies.Distinct().ToList();
                }
                list = agencies.Where(a => idUserAgencies.Contains(a.Id)).OrderBy(a => a.Name).ToDictionary(a => a.Id, a => a.Name);
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        private List<dtoWebConferenceMessageRecipient> GetInternalUsers(String unknownUser, String anonymousUser, ModuleObject obj, dtoUsersByMessageFilter filter, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, Boolean ignoreValues, List<Int32> removeUsers = null)
        {
            List<dtoWebConferenceMessageRecipient>  items = new List<dtoWebConferenceMessageRecipient> ();
            if (filter.IdCommunity > 0)
            {
                lm.Comol.Core.BaseModules.ProfileManagement.dtoUserFilters sFilter = new lm.Comol.Core.BaseModules.ProfileManagement.dtoUserFilters();
                sFilter.IdAgency = filter.IdAgency;
                sFilter.IdCommunities = new List<int>() { filter.IdCommunity };
                sFilter.IdRole = filter.IdRole;
                if (!ignoreValues)
                {
                    sFilter.SearchBy = filter.SearchBy;
                    sFilter.StartWith = filter.StartWith;
                    sFilter.Value = filter.Value;
                }
                sFilter.Status = SubscriptionStatus.activemember;

                var query = pService.GetSearchSubscriptionsQuery(sFilter);
                items.AddRange(query.ToList().Select(s => new dtoWebConferenceMessageRecipient(obj,(Person)s.Person, (Role)s.Role, ModuleWebConferencing.UniqueCode, anonymousUser)).ToList());
            }
            else {
                lm.Comol.Core.BaseModules.ProfileManagement.dtoFilters pFilter = new lm.Comol.Core.BaseModules.ProfileManagement.dtoFilters();
                pFilter.IdAgency = filter.IdAgency;
                pFilter.IdProfileType = filter.IdProfileType;
                if (!ignoreValues)
                {
                    pFilter.SearchBy = filter.SearchBy;
                    pFilter.StartWith = filter.StartWith;
                    pFilter.Value = filter.Value;
                }
                pFilter.Status = Core.BaseModules.ProfileManagement.StatusProfile.Active;
                var query = pService.GetSearchProfileQuery(pFilter);
                items.AddRange(query.ToList().Select(s => new dtoWebConferenceMessageRecipient(obj, s.Profile, ModuleWebConferencing.UniqueCode, anonymousUser)).ToList());
            }
            return items;
        }
      
        public List<dtoWebConferenceMessageRecipient> GetAvailableUsersForMessages(String unknownUser, String anonymousUser, ModuleObject obj, List<long> removeUsers, dtoUsersByMessageFilter filter,  lm.Comol.Core.Mail.Messages.MailMessagesService service, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, Boolean loadAllInfo = true)
        {
            List<dtoWebConferenceMessageRecipient> items = new List<dtoWebConferenceMessageRecipient>();
            try
            {
                WbRoom room = Manager.Get<WbRoom>(obj.ObjectLongID);
                if (room != null)
                {
                    List<dtoWebConferenceMessageRecipient> iUsers = new List<dtoWebConferenceMessageRecipient>();
                    List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages> recipients = (filter.IdMessages.Any() ? service.GetUsersForMessage(obj, filter.IdMessages) : new List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages>());
                    List<WbUser> cUsers = new List<WbUser>();
                    switch (filter.UserType)
                    {
                        case UserTypeFilter.All:
                            if (filter.MailStatus != MailStatus.WaitingConfirm && (filter.UserStatus == UserStatus.NotSubscribed || (filter.UserStatus == UserStatus.All && ((room.SubCommunity != SubscriptionType.None && room.CommunityId > 0) || room.SubSystem != SubscriptionType.None))))
                                iUsers.AddRange(GetInternalUsers(unknownUser, anonymousUser, obj, filter, pService, true));
                            if (filter.UserStatus != UserStatus.NotSubscribed)
                            {
                                var queryGP = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == room.Id && ((u.IsController && filter.UserType == UserTypeFilter.Administrator) 
                                                                                                                                                        || (u.IsController == false && (filter.UserType == UserTypeFilter.Participant || (filter.UserType == UserTypeFilter.InternalParticipant && u.PersonID>0)|| (filter.UserType == UserTypeFilter.ExternalParticipant && u.PersonID<1)))) select u);
                                queryGP = queryGP.Where(u => filter.MailStatus == MailStatus.All || (filter.MailStatus == MailStatus.Confirmed && u.MailChecked) || (!u.MailChecked && filter.MailStatus == MailStatus.WaitingConfirm));
                                cUsers.AddRange(queryGP.ToList());
                            }
                            else {
                                List<Int32> rUsers = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == room.Id && u.PersonID > 0 select u.PersonID).ToList();
                                iUsers = iUsers.Where(iu => !rUsers.Contains(iu.IdPerson)).ToList();
                            }
                            break;
                        case UserTypeFilter.WithoutMembers:
                            if (filter.UserStatus != UserStatus.NotSubscribed)
                            {
                                var queryGP = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == room.Id select u);
                                queryGP = queryGP.Where(u => filter.MailStatus == MailStatus.All || (filter.MailStatus == MailStatus.Confirmed && u.MailChecked) || (!u.MailChecked && filter.MailStatus == MailStatus.WaitingConfirm));
                                cUsers.AddRange(queryGP.ToList());
                            }
                            break;
                        case UserTypeFilter.Administrator:
                        case UserTypeFilter.Participant:
                        case UserTypeFilter.ExternalParticipant:
                        case UserTypeFilter.InternalParticipant:
                            var queryP = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == room.Id && ((u.IsController && filter.UserType == UserTypeFilter.Administrator) || (u.IsController == false && filter.UserType == UserTypeFilter.Participant) || (u.IsController == false && u.PersonID < 1 && filter.UserType == UserTypeFilter.ExternalParticipant) || (u.IsController == false && u.PersonID > 0 && filter.UserType == UserTypeFilter.InternalParticipant)) select u);
                            queryP = queryP.Where(u => filter.MailStatus == MailStatus.All || (filter.MailStatus == MailStatus.Confirmed && u.MailChecked) || (!u.MailChecked && filter.MailStatus == MailStatus.WaitingConfirm));
                            cUsers.AddRange(queryP.ToList());
                            break;
                        case UserTypeFilter.None:
                            if (filter.MailStatus != MailStatus.WaitingConfirm)
                            {
                                List<Int32> rUsers = (from u in Manager.GetIQ<WbUser>() where u.Deleted == BaseStatusDeleted.None && u.RoomId == room.Id && u.PersonID > 0 select u.PersonID).ToList();
                                iUsers.AddRange(GetInternalUsers(unknownUser, anonymousUser, obj, filter, pService, true).Where(iu => !rUsers.Contains(iu.IdPerson)).ToList());
                            }
                            break;
                    }
                    if (iUsers.Any())
                        items.AddRange(iUsers);
                    if (cUsers.Any()){
                        items.AddRange(cUsers.Where(wu => !items.Where(i => i.IdPerson == wu.PersonID).Any()).Select(wu => new dtoWebConferenceMessageRecipient(obj, wu, ModuleWebConferencing.UniqueCode)).ToList());
                        items.Where(i => cUsers.Where(wu => wu.PersonID == i.IdPerson || wu.Id == i.IdUserModule).Any()).ToList().ForEach(i => i.UpdateIdUserModule(cUsers.Where(wu => (wu.PersonID == i.IdPerson && i.IdPerson>0) || wu.Id == i.IdUserModule).FirstOrDefault()));
                    }
                    items.ForEach(i => i.MessageNumber = recipients.Where(r => r.IsFromModule && r.IdUserModule == i.IdUserModule).SelectMany(r => r.Messages.Where(m => m.IdModuleObject == i.IdModuleObject || m.IdModuleObject == 0)).Count());
                    items.ForEach(i => i.Messages = recipients.Where(r => r.IsFromModule && r.IdUserModule == i.IdUserModule).SelectMany(r => r.Messages.Where(m => m.IdModuleObject == i.IdModuleObject || m.IdModuleObject == 0)).ToList());

                    if (filter.UserType == UserTypeFilter.All || filter.UserType == UserTypeFilter.GenericMail || filter.UserType == UserTypeFilter.WithoutMembers){
                        items.AddRange((from r in recipients where r.IdUserModule <=0 select new dtoWebConferenceMessageRecipient(r)).ToList());
                        items.AddRange(recipients.Where(r => r.IsInternal && !items.Where(i => i.IdPerson == r.IdPerson).Any()).Select(r => new dtoWebConferenceMessageRecipient(r)).ToList());
                    }
                    if (items.Where(i => (i.IdLanguage == 0 && i.CodeLanguage != "multi" && i.IdUserModule > 0) || (i.IsInternal)).Any())
                    {
                        ParseInternalItems(room, filter,items.Where(i => (i.IdLanguage == 0 && i.CodeLanguage != "multi") || (i.IsInternal)).ToList());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        public List<dtoWebConferenceMessageRecipient> GetUsersForMessages(lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, List<dtoWebConferenceMessageRecipient> recipients, dtoUsersByMessageFilter filter, List<TranslatedItem<Int32>> profileTypes, List<TranslatedItem<Int32>> roles, Boolean loadAllInfo = true)
        {
            List<dtoWebConferenceMessageRecipient> items = new List<dtoWebConferenceMessageRecipient>();
            try
            {
                var query = (from r in recipients where (filter.IdProfileType <=0 || filter.IdProfileType== r.IdProfileType) && (filter.IdRole== r.IdRole || filter.IdRole==-1) select r);

                if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
                {
                    switch (filter.SearchBy)
                    {
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                            List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                            if (values.Any() && values.Count == 1)
                                query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                            else if (values.Any() && values.Count > 1)
                                query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains)) || values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                            query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                            query = query.Where(r => r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                            break;
                        case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                            query = query.Where(r => r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
                            break;
                    }
                }
                if ((filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Name || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.All || filter.SearchBy == Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains || string.IsNullOrEmpty(filter.Value)) && !string.IsNullOrEmpty(filter.StartWith))
                {
                    if (filter.StartWith != "#")
                        query = query.Where(r => r.FirstLetter == filter.StartWith.ToLower());
                    else
                        query = query.Where(r => pService.DefaultOtherChars().Contains(r.FirstLetter));
                }
                if (filter.IdAgency == -3)
                    query = query.Where(r => r.IdProfileType != (int)UserTypeStandard.Employee && (filter.IdProfileType <= 0 || filter.IdProfileType == r.IdProfileType));
                else
                {
                    Dictionary<long, List<Int32>> agencyInfos = pService.GetUsersWithAgencies(query.Where(r => r.IsInternal).Select(r => r.IdPerson).ToList().Distinct().ToList());
                    if (filter.IdAgency == -2)
                        query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee);
                    else if (agencyInfos.ContainsKey(filter.IdAgency))
                        query = query.Where(r => r.IdProfileType == (int)UserTypeStandard.Employee && agencyInfos[filter.IdAgency].Contains(r.IdPerson));
                    else if (filter.IdAgency > 0)
                        query = query.Where(r => 1 == 2);
                    if (loadAllInfo || filter.OrderBy == UserByMessagesOrder.ByAgency)
                    {
                        Dictionary<long, String> agencyName = pService.GetAgenciesName(agencyInfos.Keys.ToList());
                        foreach (var i in agencyInfos)
                        {
                            query.Where(r => r.IsInternal && i.Value.Contains(r.IdPerson)).ToList().ForEach(r => r.UpdateAgencyInfo(i.Key, (agencyName.ContainsKey(i.Key) ? agencyName[i.Key] : "")));
                        }
                    }
                }
                if (loadAllInfo) {
                    query.Where(r => r.IsInternal && r.IdProfileType > 0 && profileTypes.Where(p => p.Id == r.IdProfileType).Any()).ToList().ForEach(r => r.ProfileTypeName = profileTypes.Where(p => p.Id == r.IdProfileType).Select(p=>p.Translation).FirstOrDefault());
                    query.Where(r => r.IsInternal && r.IdRole > 0 && roles.Where(p => p.Id == r.IdRole).Any()).ToList().ForEach(r => r.RoleName = roles.Where(p => p.Id == r.IdRole).Select(p => p.Translation).FirstOrDefault());
                }

                switch (filter.OrderBy)
                {
                    case UserByMessagesOrder.ByMessageNumber:
                        if (filter.Ascending)
                            query = query.OrderBy(r => r.MessageNumber);
                        else
                            query = query.OrderByDescending(r => r.MessageNumber);
                        break;
                    case UserByMessagesOrder.ByStatus:
                        if (filter.Ascending)
                            query = query.OrderBy(r => filter.UserStatusTranslations[r.UserStatus]);
                        else
                            query = query.OrderByDescending(r => filter.UserStatusTranslations[r.UserStatus]);
                        break;
                    case UserByMessagesOrder.ByType:
                        if (filter.Ascending)
                            query = query.OrderBy(r => filter.TypeTranslations[r.UserType]);
                        else
                            query = query.OrderByDescending(r => filter.TypeTranslations[r.UserType]);
                        break;
                    case UserByMessagesOrder.ByName:
                        if (filter.Ascending)
                            query = query.OrderBy(r => r.Name).ThenBy(r => r.Surname);
                        else
                            query = query.OrderByDescending(r => r.Name).ThenByDescending(r => r.Surname);
                        break;
                    case UserByMessagesOrder.BySurname:
                        if (filter.Ascending)
                            query = query.OrderBy(r => r.Surname).ThenBy(r => r.Name);
                        else
                            query = query.OrderByDescending(r => r.Surname).ThenByDescending(r => r.Name);
                        break;
                    case UserByMessagesOrder.ByAgency:
                        if (filter.Ascending)
                            query = query.OrderBy(r => r.AgencyName);
                        else
                            query = query.OrderByDescending(r => r.AgencyName);
                        break;
                }

                items = query.ToList();
            }
            catch (Exception ex)
            {

            }
            return items;
        }
      
        private void ParseInternalItems(WbRoom room, dtoUsersByMessageFilter filter, List<dtoWebConferenceMessageRecipient> items)
        { 
            List<Int32> idUsers = items.Where(i => i.IsInternal && !i.IsAutoGeneratedPerson ).Select(i => i.IdPerson).Distinct().ToList();
            List<Person> persons = new List<Person>();
            if (idUsers.Count <= maxItemsForQuery)
                persons = (from p in Manager.GetIQ<Person>() where idUsers.Contains(p.Id) select p).ToList();
            else {
                Int32 pageIndex = 0;
                List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                while (idPagedUsers.Any())
                {
                    persons.AddRange((from p in Manager.GetIQ<Person>() where idPagedUsers.Contains(p.Id) select p).ToList());
                    pageIndex++;
                    idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                }
            }
            items.Where(i => i.IsInternal && !i.IsAutoGeneratedPerson ).ToList().ForEach(i => i.UpdatePersonInfo(persons.Where(p => p.Id== i.IdPerson).FirstOrDefault()));
            if (room.CommunityId >0) {
                idUsers = items.Where(i => i.IsInternal && i.IdRole==0).Select(i => i.IdPerson).Distinct().ToList();
                List<LazySubscription> subscriptions = new List<LazySubscription>();
                if (idUsers.Count <= maxItemsForQuery)
                    subscriptions = (from s in Manager.GetIQ<LazySubscription>() where idUsers.Contains(s.IdPerson) select s).ToList();
                else {
                    Int32 pageIndex = 0;
                    List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedUsers.Any())
                    {
                        subscriptions.AddRange((from s in Manager.GetIQ<LazySubscription>() where idUsers.Contains(s.IdPerson) select s).ToList());
                        pageIndex++;
                        idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                }
                items.Where(i => i.IsInternal && i.IdRole == 0).ToList().ForEach(i => i.IdRole = subscriptions.Where(s => s.IdPerson==i.IdPerson).Select(s => s.IdRole).FirstOrDefault());
            }
            List<Language> languages = Manager.GetAllLanguages().ToList();
            items.Where(i => !String.IsNullOrEmpty(i.CodeLanguage) && i.IdLanguage == 0 && i.CodeLanguage != "multi" && !i.IsInternal && i.IdUserModule > 0).ToList().ForEach(i => i.IdLanguage = languages.Where(l => l.Code.ToLower() == i.CodeLanguage.ToLower()).Select(l => l.Id).FirstOrDefault());
            items.Where(i => !String.IsNullOrEmpty(i.CodeLanguage) && i.IdLanguage > 0 && i.IdUserModule > 0 && !languages.Where(l => l.Id == i.IdLanguage && l.Code.ToLower() == i.CodeLanguage).Any()).ToList().ForEach(i => i.CodeLanguage = languages.Where(l => l.Id == i.IdLanguage).Select(l => l.Code).FirstOrDefault());
        }
    #endregion
#endregion

        #region UrlCode

        /// <summary>
        /// Recupera oggetto "AccessCode" (con Codice ed Id stanza) da un codice stanza
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public WbAccessCode CodeGet(String Code)
        {
            return Manager.GetAll<WbAccessCode>(c => c.UrlCode == Code).FirstOrDefault();
        }

#endregion

#region Languages

        /// <summary>
        /// Recupera dati lista dati lingua
        /// </summary>
        /// <param name="LanguagesId">ID Lingue di cui recuperare i dati</param>
        /// <returns>Lista di oggetti con dati lingua</returns>
        public IList<Domain.DTO.DTO_Language> LanguagesGet(IList<Int32> LanguagesId)
        {
            if (LanguagesId == null || LanguagesId.Count <= 0)
                return new List<Domain.DTO.DTO_Language>();

            IList<Domain.DTO.DTO_Language> WbLangs =
                (from lm.Comol.Core.DomainModel.Language lang
                in Manager.GetIQ<lm.Comol.Core.DomainModel.Language>()
                 where LanguagesId.Contains(lang.Id)
                 select new Domain.DTO.DTO_Language() { Code = lang.Code, Name = lang.Name , Icon = lang.Icon }).ToList();

            if (WbLangs == null || WbLangs.Count <= 0)
                WbLangs = new List<Domain.DTO.DTO_Language>();

            return WbLangs;
        }

        /// <summary>
        /// Controlla la presenza di una mail
        /// </summary>
        /// <param name="Mail">Mail da controllare</param>
        /// <param name="RoomId">ID Stanza</param>
        /// <returns>Esito controllo</returns>
        public MailCheck MailDoCheck(String Mail, Int64 RoomId)
        {
            int CurrentPage = 0;
            WbUser usr = (from WbUser rusr in this.UsersGet(RoomId, new Domain.DTO.DTO_UserListFilters(), -1, -1, ref CurrentPage) where rusr.Mail == Mail select rusr).FirstOrDefault();

            if (usr != null && usr.Id > 0)
                return MailCheck.MailInRoomdB;

            Person prsn = UserGetPersonByMail(Mail);

            if (prsn != null && prsn.Id > 0)
                return MailCheck.MailInSystem;

            return MailCheck.MailUnknow;
        }

#endregion


        public String GetCommunityNameByRoom(Int64 RoomId)
        {
            try
            {
                int comId = (from WbRoom r in Manager.GetIQ<WbRoom>() where r.Id == RoomId select r.CommunityId).FirstOrDefault();

                string Name = GetCommunityName(comId);

                if (!string.IsNullOrEmpty(Name))
                    return Name;
            }
            catch { }

            return "";
        }

        public String GetCommunityName(int CommunityId)
        {
            String name = "";
            try
            {
                
                if (CommunityId > 0)
                    name = (from Community c in Manager.GetIQ<Community>() where c.Id == CommunityId select c.Name).FirstOrDefault();
            }
            catch { }

            return name;
        }
    }
}