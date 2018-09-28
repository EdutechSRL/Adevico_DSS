using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    public class WbListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region costruttori/view/service/module

        public WbListPresenter(iApplicationContext oContext)
            : base(oContext)
        {}

         public WbListPresenter(iApplicationContext oContext, View.iViewWbList view)
                : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        
        protected virtual View.iViewWbList View
        {
            get { return (View.iViewWbList)base.View; }
        }

        private Domain.WbService _Service;
        private Domain.WbService Service
        {
            get
            {
                if (_Service == null)
                {
                    if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.eWorks)
                    {
                        Domain.eWorks.eWSystemParameter param = (Domain.eWorks.eWSystemParameter)View.SysParameter;
                        _Service = new Domain.eWorks.eWService(param, this.AppContext);
                    }
                    else if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.OpenMeetings)
                    {
                        Domain.OpenMeetings.oMSystemParameter param = (Domain.OpenMeetings.oMSystemParameter)View.SysParameter;
                        _Service = new Domain.OpenMeetings.oMService(param, this.AppContext);
                    }
                }

                return _Service;
            }
        }

        private Domain.ModuleWebConferencing _module;
        private Domain.ModuleWebConferencing Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    _module = Service.GetServicePermission(idUser, CurrentCommunityId);
                }
                return _module;
            }
        }

        #endregion

        /// <summary>
        /// Inizializza View (PageLoad)
        /// </summary>
        public void InitView(int currentPageIndex, int currentPageSize)
        {
            //View.ShowUpdateRoomRecording = false;

            if (UserContext.isAnonymous)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoUser);
                View.DisplaySessionTimeout();
            }
            else if (!ServerStatus())
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoServer);
                View.ShowErrorOnPage(Domain.ErrorListMessage.NoServer, true);
            }
            else
            {
                if (true) //Permessi: OK così. S'arrangia a recuperarsi le stanze corrette e mostrare o meno l'edit, etc...
                {
                    
                    int PageCount = 0;

                    IList<Domain.WbRoom> Rooms = Service.RoomsGet(Module.ManageRoom, CurrentCommunityId, UserContext.CurrentUserID, View.Filters, currentPageIndex, currentPageSize, ref PageCount);

                    PagerBase pager = new PagerBase();
                    pager.PageSize = currentPageSize;//Me.View.CurrentPageSize
                    pager.Count = PageCount;
                    pager.PageIndex = currentPageIndex;// Me.View.CurrentPageIndex
                    View.Pager = pager;

                    Boolean CanAdd = (Module.AddChatRoom || Module.ManageRoom);
                    Boolean CanModify = (Module.ManageRoom);
                    Int32 CurrentUserId = UserContext.CurrentUserID;
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomList);

                    View.BindList(Rooms, CanAdd, CanModify, CurrentUserId);

                    //if (Rooms != null && Rooms.Count > 0)
                    //{
                    //    View.BindList(Rooms, CanAdd, CanModify, CurrentUserId);
                    //}
                    //else
                    //{
                    //    View.BindList(null, CanAdd, CanModify, CurrentUserId);
                    //    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomList);
                    //    View.ShowErrorOnPage(Domain.ErrorListMessage.NoItem, false);
                    //}

                }
                else
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.ShowErrorOnPage(Domain.ErrorListMessage.NoPermission, true);
                }

                if (UserContext.CurrentUser.TypeID == 0)
                {

                }
            }
        }

        /// <summary>
        /// Cancella stanza
        /// </summary>
        /// <param name="RoomId">Id Stanza</param>
        public void DeleteRoom(Int64 RoomId)
        {
            Boolean Success = false;
            try
            {
                //PERMESSI?!
                Success = Service.RoomDelete(RoomId);
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomDelete);
            }
            catch
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.GenericError);
                Success = false;
            }

            this.InitView(View.Pager.PageIndex, View.Pager.PageSize);   // ??? RICONTROLLARE !!!

        }

        /// <summary>
        /// Stato del server
        /// </summary>
        /// <returns>
        /// True: server OK
        /// False: server non raggiungibile
        /// </returns>
        public Boolean ServerStatus()
        {
            return this.Service.ServerCheck();
        }

        /// <summary>
        /// Invia azione utente
        /// </summary>
        /// <param name="Action">Azione utente</param>
        private void SendUserAction(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType Action)
        {
            View.SendUserAction(CurrentCommunityId, Service.ServiceModuleID(), Action);
        }

        /// <summary>
        /// ID comunità corrente (tiene conto dell'URL o della sessione utente)
        /// </summary>
        public Int32 CurrentCommunityId
        {
            get
            {
                Int32 VComId = View.ViewCommunityId;
                if (VComId > 0)
                {
                    return VComId;
                }
                else
                {
                    Int32 SysComId = UserContext.CurrentCommunityID;
                    View.ViewCommunityId = SysComId;
                    return SysComId;
                }
            }
        }

        public void UpdateRoomRecording()
        {
            this.Service.RoomRecordingUpdate();
        }
    }
}