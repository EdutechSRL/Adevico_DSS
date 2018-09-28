using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    public class WbCreatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region costruttori/view/module
        
        public WbCreatePresenter(iApplicationContext oContext)
            : base(oContext)
        {

        }

        public WbCreatePresenter(iApplicationContext oContext, View.iViewWbCreate view)
                : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        protected virtual View.iViewWbCreate View
        {
            get { return (View.iViewWbCreate)base.View; }
        }

        private Domain.ModuleWebConferencing _module;
        private Domain.ModuleWebConferencing Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    _module = Service.GetServicePermission(idUser, this.CurrentCommunityId);
                }
                return _module;
            }
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

        #endregion

        /// <summary>
        /// Inizializza la View (Page Load)
        /// </summary>
        public void InitView()
        {
            if (UserContext.isAnonymous)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoUser);
                View.DisplaySessionTimeout();
            }
            else if (!ServerStatus())
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoServer);
                View.DisplayNoServer();
            }
            else
            {
                if (Module.ManageRoom)
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomAdd);
                    View.DisplayBallot();
                }
                else if (Module.AddChatRoom)
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomAdd);
                    View.DisplayOnlyChat();
                }
                else
                {
                    this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                    View.DisplayNoPermission();
                }
            }
        }

        /// <summary>
        /// Imposta la view in base alla scelta del tipo stanza
        /// </summary>
        /// <param name="RoomType">Tipo di stanza</param>
        public void SetdefParameter(Domain.RoomType RoomType)
        {
            // Dati generici (nome, descrizione, inizio/fine, numero utenti)
            Domain.DTO.DTO_GenericRoomData GenericData = Service.RoomGetGenericData(RoomType);

            // Paramteri (in base all'istanza)
            Domain.WbRoomParameter DefRoomParam = Service.ParameterGetByType(RoomType);

            // Parametri disponibili per ogni istanza. (Se altra istanza, passo null!)
            Domain.eWorks.DTO.DTOAvailableParameters eWDefParameter = null; 
            
            if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.eWorks)
            {
                Domain.eWorks.eWSystemParameter eWSysParam = (Domain.eWorks.eWSystemParameter)View.SysParameter;

                eWDefParameter = Domain.eWorks.eWAPIConnector.getAvailableParameters(
                    eWSysParam.BaseUrl, eWSysParam.ProxyUrl, eWSysParam.MainUserId, eWSysParam.MainUserPwd, eWSysParam.MainUserId);
            }
            else if (View.SysParameter.CurrentSystem == Domain.wBImplementedSystem.OpenMeetings)
            {
                //nessun parametro avanzato per OpenMeetings
            }

            this.View.Init(GenericData, DefRoomParam, eWDefParameter);
        }
        
        /// <summary>
        /// Crea la stanza vera e propria
        /// </summary>
        /// <param name="UserAudio">Attiva audio utenti selezionati</param>
        /// <param name="UserVideo">Attiva video utenti selezionati</param>
        /// <param name="UserChat">Attiva chat utenti selezionati</param>
        /// <param name="UserAdmin">Utenti selezionati saranno amministratori</param>
        /// <param name="PersonsId">ID Person selezionate</param>
        /// <returns>ID della stanza creata</returns>
        public Int64 CreateRoom(Boolean UserAudio, Boolean UserVideo, Boolean UserChat, Boolean UserHost, Boolean UserCtrl, IList<int> PersonsId, bool HasIdInName)
        {
            if (UserContext.isAnonymous)
            {
                this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoUser);
                View.DisplaySessionTimeout();
                return -1;
            }
            else
            {
                if (Module.ManageRoom || Module.AddChatRoom)
                {
                   Domain.WbRoom Room = new Domain.WbRoom();

                   Room.hasIdInName = HasIdInName;

                    Room.Name = View.GenericData.Name;
                    
                    Room.Description = View.GenericData.Description;
                    Room.Duration = View.GenericData.Duration;
                    Room.EndDate = View.GenericData.EndDate;
                    Room.MaxAllowUsers = View.GenericData.MaxAllowUsers;
                    
                    Room.Public = View.GenericData.Public;
                    Room.StartDate = View.GenericData.StartDate;
                    Room.Type = View.RoomType;

                    Room.Parameter = View.Parameters;

                    Room.CommunityId = this.CurrentCommunityId;

                    Int64 NewId = Service.RoomCreate(Room, HasIdInName);

                    if (NewId > 0)
                    {
                        
                        IList<int> CurUsrIds = new List<int>();
                        CurUsrIds.Add(base.UserContext.CurrentUserID);
                        Service.UserPersonAddIds(CurUsrIds, NewId, true, true, true, true, true);
                     
                        if (PersonsId != null || PersonsId.Count > 0)
                            Service.UserPersonAddIds(PersonsId, NewId, UserAudio, UserVideo, UserChat, UserHost ,UserCtrl);

                        this.SendUserAction(Domain.ModuleWebConferencing.ActionType.RoomAdd);
                        return NewId;
                    }
                    else
                    {
                        this.SendUserAction(Domain.ModuleWebConferencing.ActionType.NoPermission);
                        View.DisplayNoPermission();
                        return -1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Controllo dello stato del server
        /// </summary>
        /// <returns>
        /// True: server ok
        /// False: server irraggiungibile
        /// </returns>
        public Boolean ServerStatus()
        {
            return this.Service.ServerCheck();
        }

        /// <summary>
        /// Helper invio azioni utente
        /// </summary>
        /// <param name="Action">Azione utente</param>
        private void SendUserAction(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType Action)
        {
            View.SendUserAction(UserContext.CurrentCommunityID, Service.ServiceModuleID(), Action);
        }

        /// <summary>
        /// Comunità corrente da incrocio URL/ViewState/Comunità corrente utente
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
    }
}
