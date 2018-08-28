using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
//using lm.Comol.Modules.ScormStat;
using NHibernate.Mapping;
using Message = lm.Comol.Core.BaseModules.Tickets.Domain.Message;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        /// <summary>
        /// Crea/Invia un Ticket
        /// </summary>
        /// <param name="NewTicket">Dati del ticket da creare/inviare</param>
        /// <returns>Eventuali errori</returns>
        public Domain.Enums.TicketCreateError TicketCreate(ref Domain.DTO.DTO_Ticket NewTicket)
        {
            //if (!PermissionTicketUsercanCreate())
            //    return Domain.Enums.TicketCreateError.ToMuchTicket;
            if (NewTicket.CategoryId <= 0)
                return Domain.Enums.TicketCreateError.NoCategory;

            Domain.Category cat = Manager.Get<Category>(NewTicket.CategoryId);
            if (cat == null)
                return Domain.Enums.TicketCreateError.NoCategory;

            if (String.IsNullOrEmpty(NewTicket.Text) && !NewTicket.IsDraft)
                return Domain.Enums.TicketCreateError.NoText;
            if (String.IsNullOrEmpty(NewTicket.Title))
                return Domain.Enums.TicketCreateError.NoTitle;


            TicketUser creator = this.UserGetfromPerson(CurrentPerson.Id);
            
            if (creator == null)
                return Domain.Enums.TicketCreateError.NoPermission;

            Domain.Ticket Tk = new Ticket();

            if (NewTicket.TicketId > 0)
                Tk = Manager.Get<Ticket>(NewTicket.TicketId);

            if (Tk != null && Tk.IsBehalf && (Tk.IsHideToOwner && Tk.CreatedBy.Id != UC.CurrentUserID))
            {
                return TicketCreateError.NoPermission;
            }
            
            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();



            if (Tk == null || Tk.Id <= 0 || (Tk.CreatedBy.Id != CurrentPerson.Id && !Tk.IsDraft))
            {
                Tk = new Ticket();
                Tk.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                Tk.Owner = creator;
            }
            else
            {
                Tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            //x action
            NewTicket.CreatorId = creator.Id;

            ////test
            //TicketUser DestUsr = Manager.Get<TicketUser>(3193);



            //if (Tk.IsBehalf)
            //{
                
            //}
            //else
            //{
            //    Tk.Owner = usr;
            //}

            
            Tk.Community = Manager.GetCommunity(NewTicket.CommunityId);
            Tk.CreationCategory = cat;
            Tk.LanguageCode = NewTicket.LanguageCode;
            Tk.OpenOn = DateTime.Now;
            Tk.Title = NewTicket.Title;
            Tk.IsHideToOwner = NewTicket.HideToOwner;

            ////da testare
            //if (Filters.ShowOnlyNews)
            //{
            //    Tickets = Tickets.Where(t => !t.IsDraft &&
            //        (t.Owner.Id == CurUsrId) ? t.LastUserAccess <= t.ModifiedOn :
            //        t.LastCreatorAccess <= t.ModifiedOn
            //        );
            //}
            if (Tk.Owner.Id == this.CurrentUser.Id)
            {
                Tk.LastUserAccess = DateTime.Now;
            }
            else
            {
                Tk.LastCreatorAccess = DateTime.Now;
            }
            
            if (NewTicket.IsDraft)
            {
                Tk.IsDraft = true;
                Tk.Status = Domain.Enums.TicketStatus.draft;
            }
            else
            {
                Tk.IsDraft = false;
                Tk.Status = Domain.Enums.TicketStatus.open;
            }

            if (String.IsNullOrEmpty(NewTicket.Preview))
                NewTicket.Preview = NewTicket.Text;

            if (NewTicket.Preview.Length > PreviewMaxLenght)
                Tk.Preview = NewTicket.Preview.Substring(0, PreviewMaxLenght) + "...";
            else
                Tk.Preview = NewTicket.Preview;

            Message msg = new Message();
            //Message lastMessage = new Message();

            bool msgIsNew = true;

            if (Tk.Messages.Count > 0)
            {
                msg = Tk.Messages.FirstOrDefault();
                msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                msgIsNew = false;
            }
            else
            {
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            

            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Text = NewTicket.Text;
            msg.Creator = Tk.Owner;
            msg.Ticket = Tk;
            msg.Visibility = true;
            msg.UserType = Domain.Enums.MessageUserType.Partecipant;
            msg.Type = Domain.Enums.MessageType.Request;

            msg.Action = Domain.Enums.MessageActionType.normal;
            msg.ToCategory = Tk.CreationCategory;
            msg.ToUser = null;
            msg.ToStatus = Tk.Status;
            msg.IsDraft = NewTicket.IsDraft;
            msg.IsBehalf = Tk.IsBehalf;

            if (msgIsNew)
                Tk.Messages.Add(msg);

         

            if (!Tk.IsDraft)
            {
                Domain.Assignment Ass = new Assignment();
                Ass.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                Ass.AssignedCategory = cat;
                Ass.IsCurrent = true;
                Ass.Type = Domain.Enums.AssignmentType.Category;
                Ass.Ticket = Tk;

                Tk.Assignemts.Add(Ass);
                Tk.LastAssignment = Ass;

                if (Tk.IsBehalf)
                {
                    Message msgBehalf = new Message();
                    msgBehalf.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                    //RIVEDERE!!!

                    msgBehalf.Text = "Created by: " + Tk.CreatedBy.SurnameAndName;
                    msgBehalf.Preview = "";
                    msgBehalf.Creator = this.CurrentUser;//msg.Creator; //creato da...
                    msgBehalf.DisplayName = "";
                    msgBehalf.SendDate = DateTime.Now;
                    msgBehalf.ShowRealName = true;
                    msgBehalf.Ticket = Tk;
                    msgBehalf.Type = Domain.Enums.MessageType.System;
                    msgBehalf.UserType = msg.UserType;
                    msgBehalf.Visibility = true;

                    msgBehalf.Action = Domain.Enums.MessageActionType.behalfSet;
                    msgBehalf.ToStatus = Tk.Status;
                    msgBehalf.ToUser = Tk.Owner;    //x conto di Tk.Owner
                    msgBehalf.ToCategory = msg.ToCategory;
                    //Manager.SaveOrUpdate<Message>(msgBehalf);
                    Tk.Messages.Add(msgBehalf);
                    //lastMessage = msgBehalf;

                    try
                    {
                        Manager.SaveOrUpdate<Message>(msgBehalf);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            try
            {
                Manager.SaveOrUpdate<Ticket>(Tk);
                NewTicket.TicketId = Tk.Id;

                if (Tk.Status == Domain.Enums.TicketStatus.open) //Ticket inviato
                {
                    var firstOrDefault = Tk.Messages.Where(m => m.IsDraft == false)
                        .OrderByDescending(m => m.CreatedOn)
                        .FirstOrDefault();
                    if (firstOrDefault != null)
                        NewTicket.DraftMsgId =
                            firstOrDefault
                                .Id;
                }

                Manager.Commit();
            }
            catch (Exception ex)
            {
                if(Manager.IsInTransaction())
                    Manager.RollBack();

                return Domain.Enums.TicketCreateError.dBUnknown;

            }

            //MailSettings
            //this.MailSettingsSetTicket(NewTicket.MailSettings, Tk);
            
            return Domain.Enums.TicketCreateError.none;
        }

        public Domain.Enums.TicketCreateError TicketCreateExternal(Domain.DTO.DTO_User User, ref Domain.DTO.DTO_Ticket NewTicket)
        {

            if (NewTicket.CategoryId <= 0)
                return Domain.Enums.TicketCreateError.NoCategory;

            Domain.Category cat = Manager.Get<Category>(NewTicket.CategoryId);
            if (cat == null)
                return Domain.Enums.TicketCreateError.NoCategory;

            if (String.IsNullOrEmpty(NewTicket.Text))
                return Domain.Enums.TicketCreateError.NoText;
            if (String.IsNullOrEmpty(NewTicket.Title))
                return Domain.Enums.TicketCreateError.NoTitle;

            TicketUser usr = this.UserGet(User.UserId);
            if (usr == null)
                return Domain.Enums.TicketCreateError.NoPermission;

            Domain.Ticket Tk = new Ticket();

            if (NewTicket.TicketId > 0)
                Tk = Manager.Get<Ticket>(NewTicket.TicketId);

            if (Tk == null || Tk.Id <= 0 || (Tk.CreatedBy.Id != CurrentPerson.Id && !Tk.IsDraft))
            {
                Tk = new Ticket();
                Tk.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                Tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            //x action
            NewTicket.CreatorId = usr.Id;

            Tk.Owner = usr;
            Tk.Community = Manager.GetCommunity(NewTicket.CommunityId);
            Tk.CreationCategory = cat;
            Tk.LanguageCode = NewTicket.LanguageCode;
            Tk.OpenOn = DateTime.Now;
            Tk.Title = NewTicket.Title;
            if (Tk.Owner.Id == this.CurrentUser.Id)
            {
                Tk.LastUserAccess = DateTime.Now;
            }
            else
            {
                Tk.LastCreatorAccess = DateTime.Now;
            }

            if (NewTicket.IsDraft)
            {
                Tk.IsDraft = true;
                Tk.Status = Domain.Enums.TicketStatus.draft;
            }
            else
            {
                Tk.IsDraft = false;
                Tk.Status = Domain.Enums.TicketStatus.open;
            }

            if (String.IsNullOrEmpty(NewTicket.Preview))
                NewTicket.Preview = NewTicket.Text;

            if (NewTicket.Preview.Length > PreviewMaxLenght)
                Tk.Preview = NewTicket.Preview.Substring(0, PreviewMaxLenght) + "...";
            else
                Tk.Preview = NewTicket.Preview;

            Message msg = new Message();
            bool msgIsNew = true;

            if (Tk.Messages.Count > 0)
            {
                msg = Tk.Messages.FirstOrDefault();
                msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                msgIsNew = false;
            }
            else
            {
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Text = NewTicket.Text;
            msg.Creator = usr;
            msg.Ticket = Tk;
            msg.Visibility = true;
            msg.UserType = Domain.Enums.MessageUserType.Partecipant;
            msg.Type = Domain.Enums.MessageType.Request;

            msg.Action = Domain.Enums.MessageActionType.normal;
            msg.ToCategory = Tk.CreationCategory;
            msg.ToUser = null;
            msg.ToStatus = Tk.Status;

            if (msgIsNew)
                Tk.Messages.Add(msg);

            if (!Tk.IsDraft)
            {
                Domain.Assignment Ass = new Assignment();
                Ass.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                Ass.AssignedCategory = cat;
                Ass.IsCurrent = true;
                Ass.Type = Domain.Enums.AssignmentType.Category;
                Ass.Ticket = Tk;

                Tk.Assignemts.Add(Ass);
                Tk.LastAssignment = Ass;
            }

            try
            {
                Manager.SaveOrUpdate<Ticket>(Tk);
                NewTicket.TicketId = Tk.Id;
                NewTicket.DraftMsgId =
                    (from Message ms in Tk.Messages orderby ms.SendDate select ms.Id).LastOrDefault();
            }
            catch (Exception ex)
            {
                return Domain.Enums.TicketCreateError.dBUnknown;
            }

            //MailSettings
            //this.MailSettingsSetTicket(NewTicket.MailSettings, Tk, User.UserId);

            

            return Domain.Enums.TicketCreateError.none;
        }
        /// <summary>
        /// Modifica lo stato di un ticket, se è da modificare.
        /// </summary>
        /// <param name="TicketId">ID Ticket</param>
        /// <param name="UserId">ID Utente</param>
        /// <param name="Status">Stato</param>
        /// <param name="Message">Messaggio che verrà salvato su dB. Nell'interfaccia non sarà visbile.</param>
        /// <param name="UpdateAccess">Se TRUE aggiorna l'accesso per l'utente corrente.</param>
        /// <returns></returns>
        /// <remarks>
        /// NO CONTROLLO PERMESSI!!!
        /// </remarks>
        public bool TicketStatusModify(
            Int64 TicketId,
            Domain.Enums.TicketStatus Status,
            String Message,
            Boolean UpdateAccess,
            Domain.Enums.MessageUserType UserType,
            ref Int64 newMessageId
            )
        {

            if (UserType == Domain.Enums.MessageUserType.none)
                return false;

            Ticket tk = Manager.Get<Ticket>(TicketId);
            if (tk == null)
                return false;

            if (tk.Status == Status)
            {
                return false;
            }

            TicketUser Usr = this.UserGetfromPerson(UC.CurrentUserID);
            if (Usr == null)
                return false;

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            tk.Status = Status;

            Message msgClose = new Message();
            msgClose.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            msgClose.Text = Message;
            msgClose.Preview = "";
            msgClose.Creator = Usr;
            msgClose.DisplayName = "";
            msgClose.SendDate = DateTime.Now;
            msgClose.ShowRealName = true;
            msgClose.Ticket = tk;
            msgClose.Type = Domain.Enums.MessageType.System;
            msgClose.UserType = UserType;
            msgClose.Visibility = true;

            msgClose.Action = Domain.Enums.MessageActionType.statusChanged;
            msgClose.ToStatus = tk.Status;

            Domain.TicketUser usr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msgClose.ToUser = usr;

            Domain.Category cat = (from Assignment asg in tk.Assignemts where asg.AssignedCategory != null orderby asg.CreatedOn select asg.AssignedCategory).FirstOrDefault();

            if (cat == null)
                cat = tk.CreationCategory;

            msgClose.ToCategory = cat;

            tk.Messages.Add(msgClose);

            if (UpdateAccess && UserType == Domain.Enums.MessageUserType.Partecipant)
                tk.LastUserAccess = DateTime.Now;

            Manager.SaveOrUpdate<Ticket>(tk);

            if (UpdateAccess
                && UserType == Domain.Enums.MessageUserType.CategoryManager
                && UserType == Domain.Enums.MessageUserType.CategoryResolver
                && UserType == Domain.Enums.MessageUserType.Manager
                && UserType == Domain.Enums.MessageUserType.Resolver)
            {
                this.UserAccessUpdate(tk.Id);
            }

            newMessageId = msgClose.Id;

            return true;
        }

        /// <summary>
        /// Modifica la condizione di un ticket.
        /// </summary>
        /// <param name="TicketId">ID Ticket</param>
        /// <param name="UserId">ID Utente</param>
        /// <param name="Condition">Nuova condizione (se permessa): attivo, segnalato, bloccato, deprecato</param>
        /// <param name="Message">Messaggio che verrà salvato su dB. Nell'interfaccia non sarà visbile.</param>
        /// <param name="UpdateAccess">Se TRUE aggiorna l'accesso per l'utente corrente.</param>
        /// <returns></returns>
        /// <remarks>
        /// NO CONTROLLO PERMESSI!!!
        /// </remarks>
        public bool TicketConditionModify(
            Int64 TicketId,
            Domain.Enums.TicketCondition Condition,
            bool status,
            String Message,
            Boolean UpdateAccess,
            Domain.Enums.MessageUserType UserType,
            ref Int64 messageId
            )
        {

            if (UserType == Domain.Enums.MessageUserType.none)
                return false;

            Ticket tk = Manager.Get<Ticket>(TicketId);
            if (tk == null)
                return false;

            TicketUser Usr = this.UserGetfromPerson(UC.CurrentUserID);
            if (Usr == null)
                return false;

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            if (status)
                tk.Condition = tk.Condition | Condition;
            else
            {
                tk.Condition = tk.Condition ^ Condition;
            }

            Message msgClose = new Message();
            msgClose.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            msgClose.Text = Message;
            msgClose.Preview = "";
            msgClose.Creator = Usr;
            msgClose.DisplayName = "";
            msgClose.SendDate = DateTime.Now;
            msgClose.ShowRealName = true;
            msgClose.Ticket = tk;
            msgClose.Type = Domain.Enums.MessageType.System;
            msgClose.UserType = UserType;
            msgClose.Visibility = true;

            msgClose.Action = Domain.Enums.MessageActionType.conditionChanged;
            msgClose.ToCondition = Condition;

            Domain.TicketUser usr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msgClose.ToUser = usr;

            Domain.Category cat = (from Assignment asg in tk.Assignemts where asg.AssignedCategory != null orderby asg.CreatedOn select asg.AssignedCategory).FirstOrDefault();

            if (cat == null)
                cat = tk.CreationCategory;

            msgClose.ToCategory = cat;

            tk.Messages.Add(msgClose);

            if (UpdateAccess && UserType == Domain.Enums.MessageUserType.Partecipant)
                tk.LastUserAccess = DateTime.Now;

            Manager.SaveOrUpdate<Ticket>(tk);

            messageId = msgClose.Id;

            if (UpdateAccess
                && UserType == Domain.Enums.MessageUserType.CategoryManager
                && UserType == Domain.Enums.MessageUserType.CategoryResolver
                && UserType == Domain.Enums.MessageUserType.Manager
                && UserType == Domain.Enums.MessageUserType.Resolver)
            {
                this.UserAccessUpdate(tk.Id);
            }

            return true;
        }

        #region Get - User

        /// <summary>
        /// Recupera un Ticket in BOZZA
        /// </summary>
        /// <param name="TicketId">Id del Ticket</param>
        /// <returns>Dati del Ticket</returns>
        /// <remarks>
        /// Ritorna un oggetto vuoto (new) SE non sono soddisfatti i seguenti requisiti:
        /// - Il Ticket non è in DRAFT o non esiste
        /// - L'utente NON è il CREATORE del Ticket (controllare per ESTERNI!)
        /// </remarks>
        public Domain.DTO.DTO_Ticket TicketGetDraft(Int64 TicketId, Int64 UserId = 0)
        {
            if (UserId <= 0)
                UserId = this.UserGetIdfromPerson(CurrentPerson.Id);

            Ticket Tk = Manager.Get<Ticket>(TicketId);
            Domain.DTO.DTO_Ticket dtoTK = new Domain.DTO.DTO_Ticket();

            if (Tk != null && (Tk.Owner.Id == UserId || Tk.CreatedBy.Id == CurrentPerson.Id) && Tk.IsDraft)  // 
            {
                dtoTK.CategoryId = (Tk.CreationCategory != null) ? Tk.CreationCategory.Id : 0;
                dtoTK.IsDraft = Tk.IsDraft;
                dtoTK.LanguageCode = Tk.LanguageCode;

                if (Tk.Messages != null && Tk.Messages.Count() > 0)
                {
                    //&& _msg.Creator.Id == UserId <- 
                    Message msg = (from Message _msg in Tk.Messages where _msg.Creator != null && _msg.Deleted == BaseStatusDeleted.None && _msg.IsDraft == true orderby _msg.CreatedOn descending select _msg).FirstOrDefault();
                    
                    if (msg != null)
                    {
                        dtoTK.Text = msg.Text;
                        dtoTK.DraftMsgId = msg.Id;

                        //TK.Domain.DTO.DTO_AttachmentItem
                        if(msg.Attachments != null && msg.Attachments.Any())
                        {
                            dtoTK.Attachments = (from Domain.TicketFile file 
                                           in msg.Attachments 
                                           where (file.Visibility == Domain.Enums.FileVisibility.visible 
                                           || msg.Creator.Id == UserId)
                                       select new Domain.DTO.DTO_AttachmentItem(file)
                                       ).ToList();
                        }
                    }
                }

                dtoTK.TicketId = Tk.Id;
                dtoTK.Title = Tk.Title;

                if (Tk.Community == null)
                    dtoTK.CommunityId = -1;
                else
                    dtoTK.CommunityId = Tk.Community.Id;

                dtoTK.OwnerId = Tk.Owner.Id;

                if (Tk.Owner.Person == null)
                {
                    dtoTK.OwnerName = Tk.Owner.Name;
                    dtoTK.OwnerSName = Tk.Owner.Sname;
                    dtoTK.OwnerMail = Tk.Owner.mail;
                }
                else
                {
                    dtoTK.OwnerName = Tk.Owner.Person.Name;
                    dtoTK.OwnerSName = Tk.Owner.Person.Surname;
                    dtoTK.OwnerMail = Tk.Owner.Person.Mail;
                }
            }

            //dtoTK.MailSettings = this.MailSettingsGet(this.CurrentUser.Id, dtoTK.TicketId); //UC.CurrentCommunityID,


            if (Tk.Owner.Id == UserId)
                dtoTK.IsOwner = true;
            else
                dtoTK.IsOwner = false;

            //User
            if (Tk.Owner.Person != null)
            {
                if (Tk.CreatedBy.Id != Tk.Owner.Person.Id)
                    dtoTK.IsBehalf = true;
                else
                    dtoTK.IsBehalf = false;

                dtoTK.Creator = Tk.CreatedBy.SurnameAndName;
                dtoTK.IsOwnerNotificationActive = Tk.Owner.IsNotificationActiveUser;

            }
            else
            {
                if(Tk.CreatedBy.Id != PersonExternal.Id)
                    dtoTK.IsBehalf = true;
                else
                    dtoTK.IsBehalf = false;
            }

            dtoTK.HideToOwner = Tk.IsHideToOwner;

            

            return dtoTK;
        }

        public Domain.DTO.DTO_UserModify TicketGetUserExt(
            Int64 TicketId, Int64 UserId
            )
        {
            return TicketGetUser(TicketId, Manager.Get<Domain.TicketUser>(UserId));
        }


        public Domain.DTO.DTO_UserModify TicketGetUser(
            Int64 TicketId
            )
        {
            return TicketGetUser(TicketId, this.UserGetfromPerson(UC.CurrentUserID));
        }

        /// <summary>
        /// Recupera un Ticket per la visualizzazione UTENTE
        /// </summary>
        /// <param name="TicketId">ID Ticket</param>
        /// <returns></returns>
        private Domain.DTO.DTO_UserModify TicketGetUser(
            Int64 TicketId, Domain.TicketUser User
            )
        {
            SettingsPortal settingsPortal = PortalSettingsGet();

            Domain.DTO.DTO_UserModify TicketData = new Domain.DTO.DTO_UserModify();
            //TicketData.IsReadOnly = true;

            if (!settingsPortal.CanShowTicket)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NoPermission;
                return TicketData;
            }

            if (User == null)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NoPermission;
                return TicketData;
            }

            Domain.Ticket Tk = Manager.Get<Domain.Ticket>(TicketId);

            if (Tk == null)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NotFound;
                return TicketData;
            }
            else if (Tk.IsDraft)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.IsDraft;
                TicketData.TicketId = TicketId;
                return TicketData;
            } else if (Tk.IsHideToOwner && Tk.Owner.Id == User.Id)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NotFound;
                return TicketData;
            }

            MessageUserType RealUserType = TicketUserTypeGet(TicketId, User.Id, CurrentPerson.Id);


            //RIVEDERE!
            //bool isOwner = Tk.Owner != null && Tk.Owner.Id == User.Id;
            TicketData.isOwner = Tk.Owner.Id == this.CurrentUser.Id;
            TicketData.IsBehalf = Tk.IsBehalf;
            TicketData.CategoryName = CategoryTranslateName(Tk.CreationCategory, this.LanguageIdCurrentUser);
            TicketData.TicketId = Tk.Id;
            TicketData.Code = Tk.Code;
            TicketData.Status = Tk.Status;
            TicketData.Title = Tk.Title;
            TicketData.IsHideToOwner = Tk.IsHideToOwner;
            TicketData.CurrentUserType = MessageUserType.Partecipant;


            
            //                                     TicketUserTypeGet

            //if
            //    (!isOwner
            //     && !(Tk.IsBehalf && Tk.CreatedBy.Id == this.CurrentPerson.Id)
            //    )
            //{
            //    if (currentUserType == MessageUserType.none)
            //    {
            
            //else
            //{
            //    TicketData.IsReadOnly = false;
            //}
            

            
            //Ho permessi di Behalf
            //    - Ticket IN BEHALF & NON ho accessi utenti   ->
            bool behalfPermission = this.SettingPermissionGet(this.CurrentUser.Id, this.CurrentPerson.TypeID,
                PermissionType.Behalf);

            if(behalfPermission && !Tk.IsBehalf)
            {
                behalfPermission = (Tk.CreatedBy.Id == UC.CurrentUserID);
            }


            DateTime? sendDate = (from Message m in Tk.Messages orderby m.CreatedOn ascending select m.CreatedOn).FirstOrDefault();

            //BEHALFER
            //bool showBehalf = behalfPermission;
            bool userAccessBhPermission = false;
            if (Tk.IsBehalf)
                userAccessBhPermission = (Tk.LastUserAccess == null || Tk.LastUserAccess <= sendDate);


            TicketData.ShowBehalf = behalfPermission 
                && userAccessBhPermission
                && settingsPortal.CanEditTicket;

            TicketData.CanRemoveBehalf = userAccessBhPermission
                                         && (Tk.CreatedBy.Id == UC.CurrentUserID)
                                         && TicketData.IsBehalf;

            //Sono il creatore, il Ticket è in behalf, ma non ho i permessi di Behalf.
            TicketData.BehalfRevoked = (Tk.CreatedBy.Id == UC.CurrentUserID)
                                         && TicketData.IsBehalf
                                         && !TicketData.isOwner
                                         && !behalfPermission;
          
            if (TicketData.isOwner)
            {
                TicketData.IsReadOnly = !settingsPortal.CanEditTicket;
            }
            else
            {
                TicketData.IsReadOnly = !(Tk.IsBehalf && (Tk.CreatedBy.Id == UC.CurrentUserID) && settingsPortal.CanEditTicket);    //showBehalf &&
            }
            
            TicketData.LastUserAccess = Tk.LastUserAccess;

            

            //if(!Manager.IsInTransaction())
            //    Manager.BeginTransaction();


            if (TicketData.isOwner)
            {
                Tk.LastUserAccess = DateTime.Now;
                //Manager.SaveOrUpdate<Ticket>(Tk);    
            }
            else if (Tk.CreatedBy.Id == this.CurrentPerson.Id)
            {
                Tk.LastCreatorAccess = DateTime.Now;
                //Manager.SaveOrUpdate<Ticket>(Tk);    
            }
            else
            {
                this.UserAccessUpdate(Tk.Id);
                //Manager.SaveOrUpdate<Ticket>(Tk);    
            }

            Manager.SaveOrUpdate<Ticket>(Tk);


            //TicketData.Notifications = null;
            //MessageUserType currentUserType = this.TicketUserTypeGet(Tk, User.Id, UC.CurrentUserID);
            if
                (!TicketData.isOwner
                 && !(Tk.IsBehalf && Tk.CreatedBy.Id == this.CurrentPerson.Id)
                && (RealUserType == MessageUserType.none)
                )
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NoPermission;
                return TicketData;
            }

            //PERMISSION (Management/)
            TicketData.ShowToBehalfList = behalfPermission;
            TicketData.IsManagerOrResolver = UserHasManResTicketPermission(RealUserType);
            TicketData.ShowToManagementList = TicketData.IsManagerOrResolver;
            //Reimposto partecipante (x messaggi e altro)
            


            TicketData.IsClosed = (Tk.Status == Domain.Enums.TicketStatus.closeUnsolved || Tk.Status == Domain.Enums.TicketStatus.closeSolved);


            TicketData.CurrentUserDisplayName = (Tk.Owner.Person == null)
                ? Tk.Owner.Sname + " " + Tk.Owner.Name
                : Tk.Owner.Person.SurnameAndName;

                //UC.CurrentUser.SurnameAndName;
           

           TicketData.Messages = (from Message msg in Tk.Messages
                                   where (msg.Visibility == true) 
                                   && msg.IsDraft == false
                                   orderby msg.SendDate
                                   select new Domain.DTO.DTO_UserModifyItem
                                   {
                                       MessageId = msg.Id,
                                       MessageText = msg.Text,
                                       MessagePreview = msg.Preview,
                                       SendedOn = msg.SendDate,
                                       UserDisplayName = (msg.ShowRealName) ?
                                       ((msg.Creator.Person == null) ? msg.Creator.DisplayName : msg.Creator.Person.SurnameAndName) :
                                       msg.DisplayName,
                                       MessageType = msg.Type,
                                       UserType = msg.UserType,
                                       IsCloseMessage = (msg.Type == Domain.Enums.MessageType.System && msg.Action == Domain.Enums.MessageActionType.statusChanged && (msg.ToStatus == Domain.Enums.TicketStatus.closeSolved || msg.ToStatus == Domain.Enums.TicketStatus.closeUnsolved)),
                                       Action = msg.Action,
                                       ToCategory = CategoryTranslateName(msg.ToCategory, this.LanguageIdCurrentUser),
                                       ToUser = (msg.ToUser == null) ? "" :
                                        (msg.ShowRealName && msg.ToUser.Person != null) ? msg.ToUser.Person.SurnameAndName : msg.ToUser.DisplayName,
                                       ToStatus = msg.ToStatus,
                                       ToCondition = msg.ToCondition,
                                       Attachments = (msg.Attachments != null)?
                        ((from TicketFile file
                            in msg.Attachments
                        where (file.Visibility == Domain.Enums.FileVisibility.visible || msg.Creator.Id == User.Id)
                        select new DTO_AttachmentItem(file)
                    ).ToList()):
                    (new List<DTO_AttachmentItem>()),
                                       IsBehalf = msg.IsBehalf,
                                       CreatorName = (msg.IsBehalf) ? msg.CreatedBy.SurnameAndName : ""
                                   }).ToList();

            if (TicketData.Messages != null && TicketData.Messages.Count > 0)
            {
                TicketData.Messages.First().IsFirst = true;
                TicketData.Messages.Last().IsLast = true;
            }

            
            
            //DraftMessage
            TicketData.DraftMessage = this.MessageDraftGet(Tk, User, TicketData.CurrentUserType, Domain.Enums.MessageType.FeedBack);

            TicketData.Condition = Tk.Condition;
            
            
            //if (currentUserType == MessageUserType.CategoryManager
            //    || currentUserType == MessageUserType.Manager
            //    || currentUserType == MessageUserType.CategoryResolver
            //    || currentUserType == MessageUserType.Resolver)
            //    TicketData.IsManagerOrResolver = true;
            //else
            //{
            //    TicketData.IsManagerOrResolver = false;
            //}
            //this.UserHasManResTicketPermission(TicketId, User.Id);
            //TicketData.IsManagerOrResolver;
            TicketData.ShowToUserList = TicketData.isOwner || (Tk.CreatedBy.Id == this.CurrentPerson.Id);

            //notification
            if (Tk.IsBehalf)
            {
                if (Tk.IsHideToOwner)
                {
                    bool isdefaultCreator = false;
                    TicketData.CreatorMailSettings = this.MailSettingsGet(User.Id, Tk.Id, false, ref isdefaultCreator);
                    TicketData.IsDefaultNotCreator = isdefaultCreator;
                    TicketData.OwnerMailSettings = MailSettings.none;
                    TicketData.IsDefaultNotOwner = true;

                }
                else
                {
                    bool isdefaultOwner = false;
                    bool isdefaultCreator = false;
                    TicketData.CreatorMailSettings = this.MailSettingsGet(User.Id, Tk.Id, false, ref isdefaultCreator);
                    TicketData.IsDefaultNotCreator = isdefaultCreator;
                    TicketData.OwnerMailSettings = this.MailSettingsGet(Tk.Owner.Id, Tk.Id, false, ref isdefaultOwner);
                    TicketData.IsDefaultNotOwner = isdefaultOwner;


                }

                TicketData.IsCreatorNotificationEnable = User.IsNotificationActiveUser;
                TicketData.IsOwnerNotificationEnable = Tk.Owner.IsNotificationActiveUser;

            }
            else
            {   
                //TicketData.OwnerMailSettings = MailSettings.none;
                //bool isdefaultCreator = false;
                //TicketData.CreatorMailSettings = this.MailSettingsGet(this.CurrentUser.Id, Tk.Id, false, ref isdefaultCreator);
                //TicketData.IsDefaultNotCreator = isdefaultCreator;
                //TicketData.IsDefaultNotOwner = true;

                //TicketData.IsCreatorNotificationEnable = User.IsNotificationActiveUser;
                TicketData.OwnerMailSettings = MailSettings.none;
                bool isdefaultCreator = false;
                TicketData.CreatorMailSettings = this.MailSettingsGet(this.CurrentUser.Id, Tk.Id, false, ref isdefaultCreator);
                TicketData.IsDefaultNotCreator = isdefaultCreator;
                TicketData.IsDefaultNotOwner = true;

                TicketData.IsOwnerNotificationEnable = false;
                TicketData.IsCreatorNotificationEnable = User.IsNotificationActiveUser;
            }
            
            return TicketData;
        }

        /// <summary>
        /// Recupera un Ticket per la visualizzazione UTENTE
        /// </summary>
        /// <param name="TicketId">ID Ticket</param>
        /// <returns></returns>
        public Domain.DTO.DTO_UserModify TicketGetUserExt(
            Int64 TicketId,
            Domain.DTO.DTO_User User
            )
        {

            Domain.DTO.DTO_UserModify TicketData = new Domain.DTO.DTO_UserModify();

            //Domain.TicketUser User = this.UserGetfromPerson(UC.CurrentUserID);
            if (User == null)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NoPermission;
                return TicketData;
            }

            Domain.Ticket Tk = Manager.Get<Domain.Ticket>(TicketId);

            if (Tk == null)
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NotFound;
            else if (Tk.Owner != null && Tk.Owner.Id != User.UserId)
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.NoPermission;
            else if (Tk.IsDraft)
            {
                TicketData.Errors = Domain.Enums.TicketEditUserErrors.IsDraft;
                TicketData.TicketId = TicketId;
            }
            if (TicketData.Errors != Domain.Enums.TicketEditUserErrors.none)
                return TicketData;

            //Aggiorno SOLO l'ultimo accesso utente: non ci sono modifiche attive, quindi non aggiorno le metainfo.
            TicketData.LastUserAccess = Tk.LastUserAccess;
            Tk.LastUserAccess = DateTime.Now;
            Manager.SaveOrUpdate<Ticket>(Tk);

            TicketData.CategoryName = CategoryTranslateName(Tk.CreationCategory, this.LanguageIdCurrentUser);

            TicketData.TicketId = Tk.Id;
            TicketData.Status = Tk.Status;
            TicketData.Title = Tk.Title;
            //TicketData.Notifications = null;
            TicketData.IsClosed = (Tk.Status == Domain.Enums.TicketStatus.closeUnsolved || Tk.Status == Domain.Enums.TicketStatus.closeSolved);

            TicketData.CurrentUserDisplayName = User.SName + " " + User.Name;
            TicketData.CurrentUserType = Domain.Enums.MessageUserType.Partecipant;

            TicketData.Messages = (from Message msg in Tk.Messages
                                   where (msg.Visibility == true) && msg.IsDraft == false
                                   orderby msg.SendDate
                                   select new Domain.DTO.DTO_UserModifyItem
                                   {
                                       MessageId = msg.Id,
                                       MessageText = msg.Text,
                                       MessagePreview = msg.Preview,
                                       SendedOn = msg.SendDate,
                                       UserDisplayName = (msg.ShowRealName) ?
                                       ((msg.Creator.Person == null) ? msg.Creator.DisplayName : msg.Creator.Person.SurnameAndName) :
                                       msg.DisplayName,
                                       MessageType = msg.Type,
                                       UserType = msg.UserType,
                                       IsCloseMessage = (msg.Type == Domain.Enums.MessageType.System && msg.Action == Domain.Enums.MessageActionType.statusChanged && (msg.ToStatus == Domain.Enums.TicketStatus.closeSolved || msg.ToStatus == Domain.Enums.TicketStatus.closeUnsolved)),
                                       Action = msg.Action,
                                       ToCategory = CategoryTranslateName(msg.ToCategory, this.LanguageIdCurrentUser),
                                       ToUser = (msg.ToUser == null) ? "" :
                                        (msg.ShowRealName && msg.ToUser.Person != null) ? msg.ToUser.Person.SurnameAndName : msg.ToUser.DisplayName,
                                       ToStatus = msg.ToStatus,
                                       ToCondition = msg.ToCondition,
                                       Attachments = (
                                       from Domain.TicketFile file
                                           in msg.Attachments
                                       where (file.Visibility == Domain.Enums.FileVisibility.visible || msg.Creator.Id == User.UserId)
                                       select new Domain.DTO.DTO_AttachmentItem(file)
                                       ).ToList(),
                                       IsBehalf = msg.IsBehalf
                                   }).ToList();

            if(TicketData.Messages != null && TicketData.Messages.Count >0)
            {
                TicketData.Messages.First().IsFirst = true;
                TicketData.Messages.Last().IsLast = true;
            }

            //DraftMessage
            TicketData.DraftMessage = this.MessageDraftGet(Tk, User.UserId, TicketData.CurrentUserType, Domain.Enums.MessageType.FeedBack);

            TicketData.Condition = Tk.Condition;


            //Impostazioni notifica
            //notification
            bool isNotifDef = false;
            TicketData.CreatorMailSettings = MailSettings.none;
            TicketData.OwnerMailSettings = this.MailSettingsGet(User.UserId, Tk.Id, false, ref isNotifDef);
            TicketData.IsDefaultNotOwner = isNotifDef;
            TicketData.IsBehalf = Tk.IsBehalf;

            //TicketData.isOwner = (Tk.Owner.Id == User.UserId) ;

            TicketData.IsCreatorNotificationEnable = User.IsOwnerNotificationEnable;

            return TicketData;
        }

        /// <summary>
        /// Recupera l'elenco di Ticket aperti dall'utente,
        /// in base ai filtri
        /// </summary>
        /// <param name="Filters">Filtri</param>
        /// <returns>Elenco di DTO appurtuni</returns>
        /// <remarks>Filters passato BYREF per l'aggionrmaneto della paginazione: elementi totali e pagina corrente</remarks>
        public List<Domain.DTO.DTO_TicketListItemUser> TicketGetListUsr(
    ref Domain.DTO.DTO_ListFilterUser Filters)
        {

            Int64 UserId = this.CurrentUser.Id;
            Int64 PersonId = this.CurrentPerson.Id;

            //IEnumerable<Ticket> Tickets = Manager.GetIQ<Domain.Ticket>().Where(t => t.CreatedBy.Id == this.CurrentPerson.Id);
            IEnumerable<Ticket> Tickets = Manager.GetIQ<Domain.Ticket>().Where(
                t => ((t.Owner.Id == this.CurrentUser.Id && 
                    (!(t.IsBehalf && t.IsDraft) && !t.IsHideToOwner))
                || (t.CreatedBy.Id == PersonId))
                );

            //Tickets = Tickets.Where(t =>
            //      (!(t.IsBehalf && t.IsDraft) && (t.Owner.Id == cUsrId))
            //      && !t.IsHideToOwner);




            return TicketFilterList(Tickets, ref Filters);

            //IList<Int64> CategoriesId = new List<Int64>();

            //Domain.DTO.DTO_ListFilterUser fltrs = Filters;

            //IEnumerable<Ticket> Tickets = Manager.GetIQ<Domain.Ticket>().Where(t => t.CreatedBy.Id == this.CurrentPerson.Id);

            //if (fltrs.ShowOnlyNews)
            //{
            //    Tickets = Tickets.Where(t => !t.IsDraft && t.LastUserAccess <= t.ModifiedOn);
            //}

            //// Creazione posso fare già qui. Ultima modifica post conversione in DTO!!!!
            //if (fltrs.DateField == Domain.Enums.TicketUserDateFilter.Creation)
            //{
            //    if (fltrs.DateEnd != null && Filters.DateStart != null)
            //    {
            //        DateTime Start = (DateTime)fltrs.DateStart;
            //        DateTime End = (DateTime)fltrs.DateEnd;
            //        Tickets = Tickets.Where(t => t.CreatedOn >= Start && t.CreatedOn <= End);
            //    }
            //    else if (fltrs.DateStart != null)
            //    {
            //        DateTime Start = (DateTime)fltrs.DateStart;
            //        Tickets = Tickets.Where(t => t.CreatedOn >= Start);
            //    }
            //    else if (fltrs.DateEnd != null)
            //    {
            //        DateTime End = (DateTime)fltrs.DateEnd;
            //        Tickets = Tickets.Where(t => t.CreatedOn <= End);
            //    }
            //}
            //else if (fltrs.DateField == Domain.Enums.TicketUserDateFilter.LastModify)
            //{
            //    if (fltrs.DateEnd != null && Filters.DateStart != null)
            //    {
            //        DateTime Start = (DateTime)fltrs.DateStart;
            //        DateTime End = (DateTime)fltrs.DateEnd;
            //        Tickets = Tickets.Where(t => t.ModifiedOn >= Start && t.ModifiedOn <= End);
            //    }
            //    else if (fltrs.DateStart != null)
            //    {
            //        DateTime Start = (DateTime)fltrs.DateStart;
            //        Tickets = Tickets.Where(t => t.ModifiedOn >= Start);
            //    }
            //    else if (fltrs.DateEnd != null)
            //    {
            //        DateTime End = (DateTime)fltrs.DateEnd;
            //        Tickets = Tickets.Where(t => t.ModifiedOn <= End);
            //    }
            //}

            //if (!Filters.ShowAllStatus)
            //{
            //    Tickets = Tickets.Where(t => t.Status == fltrs.Status);
            //}

            //if (!String.IsNullOrEmpty(Filters.Title))
            //{
            //    Tickets = Tickets.Where(t => t.Title.ToLower().Contains(fltrs.Title));
            //}

            //Filters.RecordTotal = Tickets.Count();

            //if (Filters.RecordTotal <= 0)
            //{
            //    Filters.PageIndex = 0;
            //    return new List<Domain.DTO.DTO_TicketListItemUser>();
            //}

            //IEnumerable<Domain.DTO.DTO_TicketListItemUser> TksDtoEn = from Ticket tk in Tickets.ToList()
            //        select
            //            new Domain.DTO.DTO_TicketListItemUser
            //            {
            //                Code = tk.Id,
            //                Title = tk.Title,
            //                CreationCategoryName = (tk.CreationCategory != null) ?
            //                    ((tk.CreationCategory.Translations.Where(t => t.LanguageCode == "").Any()) ?
            //                    tk.CreationCategory.Translations.Where(t => t.LanguageCode == "").FirstOrDefault().Name :
            //                    tk.CreationCategory.Name)
            //                : "",
            //                LifeTime = tk.OpenTime,
            //                SendOn = (!tk.IsDraft) ? tk.OpenOn : DateTime.Now,
            //                Status = tk.Status,
            //                IsDraft = tk.IsDraft,
            //                LastModify = tk.ModifiedOn,
            //                HasNews = (!tk.IsDraft && tk.LastUserAccess < tk.ModifiedOn) ? true : false,
            //                CreateOn = (tk.CreatedOn != null) ? (DateTime)tk.CreatedOn : DateTime.Now,
            //                FirstMessage = tk.Preview
            //            };

            ////Riordino
            //if (Filters.OrderAscending)
            //{
            //    switch (fltrs.OrderField)
            //    {
            //        case Domain.Enums.TicketOrderUser.code:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => tk.Code); //.ToList();
            //            break;
            //        case Domain.Enums.TicketOrderUser.subject:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => tk.Title);
            //            break;
            //        case Domain.Enums.TicketOrderUser.category:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => tk.CreationCategoryName);
            //            break;
            //        case Domain.Enums.TicketOrderUser.lifeTime:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => tk.LifeTime);
            //            break;
            //        case Domain.Enums.TicketOrderUser.lastModify:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => tk.LastModify);
            //            break;
            //        case Domain.Enums.TicketOrderUser.status:
            //            TksDtoEn = TksDtoEn.OrderBy(tk => (int)tk.Status);
            //            break;
            //    }
            //}
            //else
            //{
            //    switch (Filters.OrderField)
            //    {
            //        case Domain.Enums.TicketOrderUser.code:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.Code); //.ToList();
            //            break;
            //        case Domain.Enums.TicketOrderUser.subject:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.Title);
            //            break;
            //        case Domain.Enums.TicketOrderUser.category:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.CreationCategoryName);
            //            break;
            //        case Domain.Enums.TicketOrderUser.lifeTime:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.LifeTime);
            //            break;
            //        case Domain.Enums.TicketOrderUser.lastModify:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.LastModify);
            //            break;
            //        case Domain.Enums.TicketOrderUser.status:
            //            TksDtoEn = TksDtoEn.OrderByDescending(tk => (int)tk.Status);
            //            break;
            //    }
            //}

            //// PAGINAZIONE:
            //if (Filters.PageIndex * Filters.PageSize >= Filters.RecordTotal)
            //    Filters.PageIndex = 0;

            //return TksDtoEn.Skip(Filters.PageIndex * Filters.PageSize).Take(Filters.PageSize).ToList();
        }


        private List<Domain.DTO.DTO_TicketListItemUser> TicketFilterList(
            IEnumerable<Ticket> Tickets,
            ref Domain.DTO.DTO_ListFilterUser Filters)
        {
            Domain.DTO.DTO_ListFilterUser fltrs = Filters;

            Int64 CurUsrId = this.CurrentUser.Id;

            bool canBehalf = this.SettingPermissionGet(this.CurrentUser.Id, this.CurrentPerson.TypeID,
                Domain.Enums.PermissionType.Behalf);

            if (!canBehalf)
            {
                Filters.OwnerType = 0;
                Filters.OwnerName = "";
            }

            ////da testare
            //if (Filters.ShowOnlyNews)
            //{
            //    Tickets = Tickets.Where(t => !t.IsDraft &&
            //        t.ModifiedOn != null &&
            //        (t.Owner.Id == CurUsrId)? t.LastUserAccess <= t.ModifiedOn :
            //        t.LastCreatorAccess <= t.ModifiedOn
            //        );
            //}

            if (Filters.ShowOnlyNews)
            {
                Tickets = Tickets.Where(tk => !tk.IsDraft &&
                                              ((tk.Owner.Id == CurUsrId)
                                                  ? (((tk.LastUserAccess == null) || (tk.LastUserAccess < tk.ModifiedOn))
                                                      ? true
                                                      : false)
                                                  : (((tk.LastCreatorAccess == null) ||
                                                      (tk.LastCreatorAccess < tk.ModifiedOn))
                                                      ? true
                                                      : false)));
            }
            // Creazione posso fare già qui. Ultima modifica post conversione in DTO!!!!
            if (Filters.DateField == Domain.Enums.TicketUserDateFilter.Creation)
            {
                if (Filters.DateEnd != null && Filters.DateStart != null)
                {
                    DateTime Start = (DateTime)Filters.DateStart;
                    DateTime End = (DateTime)Filters.DateEnd;
                    Tickets = Tickets.Where(t => t.CreatedOn >= Start && t.CreatedOn <= End);
                }
                else if (Filters.DateStart != null)
                {
                    DateTime Start = (DateTime)Filters.DateStart;
                    Tickets = Tickets.Where(t => t.CreatedOn >= Start);
                }
                else if (Filters.DateEnd != null)
                {
                    DateTime End = (DateTime)Filters.DateEnd;
                    Tickets = Tickets.Where(t => t.CreatedOn <= End);
                }
            }
            else if (Filters.DateField == Domain.Enums.TicketUserDateFilter.LastModify)
            {
                if (Filters.DateEnd != null && Filters.DateStart != null)
                {
                    DateTime Start = (DateTime)Filters.DateStart;
                    DateTime End = (DateTime)Filters.DateEnd;
                    Tickets = Tickets.Where(t => t.ModifiedOn >= Start && t.ModifiedOn <= End);
                }
                else if (Filters.DateStart != null)
                {
                    DateTime Start = (DateTime)Filters.DateStart;
                    Tickets = Tickets.Where(t => t.ModifiedOn >= Start);
                }
                else if (Filters.DateEnd != null)
                {
                    DateTime End = (DateTime)Filters.DateEnd;
                    Tickets = Tickets.Where(t => t.ModifiedOn <= End);
                }
            }

            if (!Filters.ShowAllStatus)
            {
                Tickets = Tickets.Where(t => t.Status == fltrs.Status);
            }

            Int32 cPId = CurrentPerson.Id;
            Int64 cUsrId = CurrentUser.Id;

            if (canBehalf)
            {
                if (Filters.OwnerType != -1)
                {

                    if (Filters.OwnerType == 0)
                    {
                        Tickets = Tickets.Where(t => t.Owner.Person != null && t.Owner.Person.Id == cPId);
                    }
                    else
                    {
                        Tickets = Tickets.Where(t => t.Owner.Person != null && t.Owner.Person.Id != cPId);
                    }
                }


                if (!String.IsNullOrEmpty(Filters.OwnerName) && Filters.OwnerType != 0)
                {
                    Tickets = Tickets.Where(t =>
                        (t.Owner.Person != null
                         && (
                             t.Owner.Person.SurnameAndName.Contains(fltrs.OwnerName) ||
                             t.Owner.Person.SurnameAndName.Contains(fltrs.OwnerName)
                             )
                            ) ||
                        (t.Owner.Person == null
                         && (

                             (t.Owner.Name + " " + t.Owner.Sname).Contains(fltrs.OwnerName) ||
                             (t.Owner.Sname + " " + t.Owner.Name).Contains(fltrs.OwnerName)
                             )
                            )
                        );
                }

                if (!String.IsNullOrEmpty(Filters.Title))
                {
                    Tickets = Tickets.Where(t => t.Title.ToLower().Contains(fltrs.Title));
                }
            }
            //else
            //{
            //    Tickets = Tickets.Where(t =>
            //        (!(t.IsBehalf && t.IsDraft) && (t.Owner.Id == cUsrId))
            //        && !t.IsHideToOwner);
            //}

            Tickets = Tickets.ToList();

            Filters.RecordTotal = Tickets.Count(); //Andava SENZA ToList() fino a 15 minuti fa...

            if (Filters.RecordTotal <= 0)
            {
                Filters.PageIndex = 0;
                return new List<Domain.DTO.DTO_TicketListItemUser>();
            }

            IEnumerable<Domain.DTO.DTO_TicketListItemUser> TksDtoEn = from Ticket tk in Tickets //.ToList()
                select
                    new Domain.DTO.DTO_TicketListItemUser
                    {
                        Id = tk.Id,
                        Code = tk.Code,
                        Title = tk.Title,
                        CreationCategoryName = (tk.CreationCategory != null) ?
                            ((tk.CreationCategory.Translations.Any(t => t.LanguageCode == "")) ?
                            tk.CreationCategory.Translations.Where(t => t.LanguageCode == "").FirstOrDefault().Name :
                            tk.CreationCategory.Name)
                        : "",
                        LifeTime = tk.OpenTime,
                        SendOn = (!tk.IsDraft) ? tk.OpenOn : DateTime.Now,
                        Status = tk.Status,
                        IsDraft = tk.IsDraft,
                        LastModify = tk.ModifiedOn,
                        
                        HasNews = (!tk.IsDraft &&
                            ((tk.Owner.Id == cUsrId)?
                                (((tk.LastUserAccess == null) || (tk.LastUserAccess < tk.ModifiedOn)) ? true : false):
                                (((tk.LastCreatorAccess == null) || (tk.LastCreatorAccess < tk.ModifiedOn)) ? true : false))),
                        CreateOn = (tk.CreatedOn != null) ? (DateTime)tk.CreatedOn : DateTime.Now,
                        FirstMessage = tk.Preview,
                        Condition = tk.Condition,
                        IsBehalf = tk.IsBehalf,
                        OwnerName = (canBehalf)? 
                            ((tk.Owner.Person == null)? tk.Owner.Sname + " " + tk.Owner.Name : tk.Owner.Person.SurnameAndName) : "",
                        IsHideToOwner = tk.IsHideToOwner

                    };



            //Riordino
            if (Filters.OrderAscending)
            {
                switch (Filters.OrderField)
                {
                    case Domain.Enums.TicketOrderUser.code:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.Id); //.ToList();
                        break;
                    case Domain.Enums.TicketOrderUser.subject:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.Title);
                        break;
                    case Domain.Enums.TicketOrderUser.category:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.CreationCategoryName);
                        break;
                    case Domain.Enums.TicketOrderUser.lifeTime:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.IsDraft).ThenBy(tk => tk.LifeTime);
                        break;
                    case Domain.Enums.TicketOrderUser.lastModify:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.SendOn);
                        break;
                    case Domain.Enums.TicketOrderUser.status:
                        TksDtoEn = TksDtoEn.OrderBy(tk => (int)tk.Status);
                        break;
                    case Domain.Enums.TicketOrderUser.behalf:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.OwnerName);
                        break;
                }
            }
            else
            {
                switch (Filters.OrderField)
                {
                    case Domain.Enums.TicketOrderUser.code:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.Id); //.ToList();
                        break;
                    case Domain.Enums.TicketOrderUser.subject:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.Title);
                        break;
                    case Domain.Enums.TicketOrderUser.category:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.CreationCategoryName);
                        break;
                    case Domain.Enums.TicketOrderUser.lifeTime:
                        TksDtoEn = TksDtoEn.OrderBy(tk => tk.IsDraft).ThenByDescending(tk => tk.LifeTime);
                        //TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.LifeTime);
                        break;
                    case Domain.Enums.TicketOrderUser.lastModify:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.SendOn);
                        break;
                    case Domain.Enums.TicketOrderUser.status:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => (int)tk.Status);
                        break;
                    case Domain.Enums.TicketOrderUser.behalf:
                        TksDtoEn = TksDtoEn.OrderByDescending(tk => tk.OwnerName);
                        break;
                }
            }


            // PAGINAZIONE:
            if (Filters.PageIndex * Filters.PageSize >= Filters.RecordTotal)
                Filters.PageIndex = 0;

            return TksDtoEn.Skip(Filters.PageIndex * Filters.PageSize).Take(Filters.PageSize).ToList();
        }

        /// <summary>
        /// Recupera l'elenco di Ticket aperti dall'utente,
        /// in base ai filtri
        /// </summary>
        /// <param name="Filters">Filtri</param>
        /// <returns>Elenco di DTO appurtuni</returns>
        /// <remarks>Filters passato BYREF per l'aggionrmaneto della paginazione: elementi totali e pagina corrente</remarks>
        public List<Domain.DTO.DTO_TicketListItemUser> TicketGetListUsrExt(
            ref Domain.DTO.DTO_ListFilterUser Filters,
            Domain.DTO.DTO_User User)
        {
            //Di cui sono il PROPRIETARIO (TkUser), ma SE il Ticket è in Behalf deve anche essermi visibile.
            //I Ticket CREATI per conto di TERZI saranno gestiti SOLO internamente!

            IEnumerable<Ticket> Tickets = Manager.GetIQ<Domain.Ticket>()
                .Where(t => t.Owner.Id == User.UserId 
                            && (!t.IsBehalf 
                                || (t.IsBehalf && (!t.IsHideToOwner && !t.IsDraft))
                                )
                    );

            return TicketFilterList(Tickets, ref Filters);

        }
        #endregion

        #region Get - Manager

        /// <summary>
        /// Recupera UN Ticket per la visualizzazione Man/Res
        /// </summary>
        /// <param name="TicketId">ID Ticket</param>
        /// <param name="MassageFilter">Indica come filtrare i messaggi</param>
        /// <param name="MessagesOrder">Indica come ordinare i messaggi</param>
        /// <returns>DTO con tutti i dati necessari</returns>
        public Domain.DTO.DTO_ManagerModify TicketGetManager(
            Int64 TicketId,
            Domain.Enums.EditManResMessagesShow MassageFilter,
            Domain.Enums.EditManResMessagesOrder MessagesOrder
            )
        {
            //Manager.Refresh
            //if (!Manager.IsInTransaction())
            //    Manager.BeginTransaction();

            

            DTO_ManagerModify TicketData = new DTO_ManagerModify();

            TicketUser User = this.UserGetfromPerson(UC.CurrentUserID);

            if (User == null)
            {
                TicketData.Errors = TicketEditManErrors.NoPermission;
                return TicketData;
            }


            Ticket Tk = Manager.Get<Ticket>(TicketId);

            MessageUserType userType = this.TicketUserTypeGet(Tk, User.Id, UC.CurrentUserID);


            if (Tk == null)
                TicketData.Errors = TicketEditManErrors.NotFound;
            else if (Tk.IsDraft)
            {
                TicketData.Errors = TicketEditManErrors.IsDraft;
                TicketData.TicketId = TicketId;
            }

            TicketData.CurrentUserType = userType;

            //(TicketData.CurrentUserType == Domain.Enums.MessageUserType.Partecipant || TicketData.CurrentUserType == Domain.Enums.MessageUserType.none)

            if (!UserHasManResTicketPermission(TicketData.CurrentUserType))
            {
                TicketData.Errors = TicketEditManErrors.NoPermission;
                return TicketData;
            }



            //Se errori non proseguo e restituisco gli errori (Accesso al Ticket)
            if (TicketData.Errors != TicketEditManErrors.none)
                return TicketData;


            TicketData.CommunityName = (Tk.Community == null) ? ComPortalName : ((Tk.Community.Id <= 0) ? ComPortalName : Tk.Community.Name);

            TicketData.CategoryCreationName = CategoryTranslateName(Tk.CreationCategory, this.LanguageIdCurrentUser);

            TicketData.TicketId = Tk.Id;
            TicketData.Code = Tk.Code;
            TicketData.Status = Tk.Status;
            TicketData.Title = Tk.Title;

            TicketData.IsClosed = (Tk.Status == Domain.Enums.TicketStatus.closeUnsolved || Tk.Status == Domain.Enums.TicketStatus.closeSolved);

            Domain.Category Cate;

            if (Tk.LastAssignment.AssignedCategory != null)
                Cate = Tk.LastAssignment.AssignedCategory;
            else
            {
                Cate = (from Domain.Assignment ass in Tk.Assignemts
                        where ass.AssignedCategory != null
                        orderby ass.CreatedOn descending
                        select ass.AssignedCategory).FirstOrDefault();
            }

            if (Tk.LastAssignment != null)
            {
                TicketData.LastAssignmentId = Tk.LastAssignment.Id;
                if (Tk.LastAssignment.AssignedTo != null)
                {
                    TicketData.UserAssigned = (Tk.LastAssignment.AssignedTo.Person == null) ?
                        Tk.LastAssignment.AssignedTo.Sname + " " + Tk.LastAssignment.AssignedTo.Name :
                        Tk.LastAssignment.AssignedTo.Person.SurnameAndName;
                    
                    TicketData.UserAssignedId = Tk.LastAssignment.AssignedTo.Id;

                    TicketData.PersonAssignedId = (Tk.LastAssignment.AssignedTo.Person == null)
                        ? 0
                        : Tk.LastAssignment.AssignedTo.Person.Id;
                }
            }

            if (Cate == null)
                Cate = Tk.CreationCategory;

            TicketData.CategoryCurrentId = Cate.Id;
            TicketData.CategoryCurrentName = Cate.GetTranslatedName(UC.Language.Code);

            TicketData.CurrentUserDisplayName = UC.CurrentUser.SurnameAndName;
            
            //TEST


            //foreach(Message msg in Tk.Messages.ToList())
            //{
            //    Domain.DTO.DTO_UserModifyItem mymsg = new DTO_UserModifyItem();


            //    mymsg.MessageId = msg.Id;
            //    mymsg.MessageText = msg.Text;
            //    mymsg.MessagePreview = msg.Preview;
            //    mymsg.SendedOn = msg.SendDate;
            //    mymsg.UserDisplayName = (msg.ShowRealName) ?
            //                           ((msg.Creator.Person == null) ? (msg.Creator.Sname + " " + msg.Creator.Name) : msg.Creator.Person.SurnameAndName) :
            //                           msg.DisplayName;
            //    mymsg.MessageType = msg.Type;
            //    mymsg.UserType = msg.UserType;
            //    mymsg.IsCloseMessage = (msg.Type == Domain.Enums.MessageType.System &&
            //                            msg.Action == Domain.Enums.MessageActionType.statusChanged &&
            //                            (msg.ToStatus == Domain.Enums.TicketStatus.closeSolved ||
            //                             msg.ToStatus == Domain.Enums.TicketStatus.closeUnsolved));
            //    mymsg.Action = msg.Action;
            //    mymsg.ToCategory = CategoryTranslateName(msg.ToCategory, this.LanguageIdCurrentUser);
            //    mymsg.ToUser = (msg.ToUser == null)
            //        ? ""
            //        : (msg.ShowRealName && msg.ToUser.Person != null)
            //            ? msg.ToUser.Person.SurnameAndName
            //            : msg.ToUser.DisplayName;
            //    mymsg.ToStatus = msg.ToStatus;
            //    mymsg.ToCondition = msg.ToCondition;
            //    mymsg.IsVisible = msg.Visibility;
            //    mymsg.Attachments = (msg.Attachments != null)
            //        ? (from Domain.TicketFile file in msg.Attachments select new Domain.DTO.DTO_AttachmentItem(file))
            //            .ToList()
            //        : new List<Domain.DTO.DTO_AttachmentItem>();
            //    mymsg.IsBehalf = msg.IsBehalf;


            //}
                

            TicketData.Messages = (from Message msg in Tk.Messages
                                   where msg.IsDraft == false
                                   orderby msg.SendDate
                                   select new Domain.DTO.DTO_UserModifyItem
                                   {
                                       MessageId = msg.Id,
                                       MessageText = msg.Text,
                                       MessagePreview = msg.Preview,
                                       SendedOn = msg.SendDate,
                                       UserDisplayName = (msg.ShowRealName) ?
                                       ((msg.Creator.Person == null) ? (msg.Creator.Sname + " " + msg.Creator.Name) : msg.Creator.Person.SurnameAndName) :
                                       msg.DisplayName,
                                       MessageType = msg.Type,
                                       UserType = msg.UserType,
                                       IsCloseMessage = (msg.Type == Domain.Enums.MessageType.System && msg.Action == Domain.Enums.MessageActionType.statusChanged && (msg.ToStatus == Domain.Enums.TicketStatus.closeSolved || msg.ToStatus == Domain.Enums.TicketStatus.closeUnsolved)),
                                       Action = msg.Action,
                                       ToCategory = CategoryTranslateName(msg.ToCategory, this.LanguageIdCurrentUser),
                                       ToUser = (msg.ToUser == null) ? "" :
                                        (msg.ShowRealName && msg.ToUser.Person != null) ? msg.ToUser.Person.SurnameAndName : msg.ToUser.DisplayName,
                                       ToStatus = msg.ToStatus,
                                       ToCondition = msg.ToCondition,
                                       IsVisible = msg.Visibility,
                                       Attachments = (msg.Attachments != null)? (from Domain.TicketFile file in msg.Attachments select new Domain.DTO.DTO_AttachmentItem(file)).ToList() : new List<Domain.DTO.DTO_AttachmentItem>(),
                                       IsBehalf = msg.IsBehalf
                                   }).ToList();

            //SET di first e last
            if(TicketData.Messages != null && TicketData.Messages.Count > 0)
            {
                TicketData.Messages.First().IsFirst = true;
                TicketData.Messages.Last().IsLast = true;
            }


            switch (MassageFilter)
            {
                case Domain.Enums.EditManResMessagesShow.MessageOnly:
                    TicketData.Messages = TicketData.Messages.Where(m => m.MessageType != Domain.Enums.MessageType.System).ToList();
                    break;

                case Domain.Enums.EditManResMessagesShow.NotifiesOnly:
                    TicketData.Messages = TicketData.Messages.Where(m => m.MessageType == Domain.Enums.MessageType.System).ToList();
                    break;
            }

            if (MessagesOrder == Domain.Enums.EditManResMessagesOrder.oldertorecent)
                TicketData.Messages = TicketData.Messages.OrderBy(m => m.SendedOn).ToList();
            else
                TicketData.Messages = TicketData.Messages.OrderByDescending(m => m.SendedOn).ToList();

            //DraftMessage
            //!! Tengo l'oggetto MESSAGE, altrimenti non riesco ad utilizzarlo in fase di UPLOAD!

            TicketData.DraftMessage = this.MessageDraftGet(Tk, User, TicketData.CurrentUserType, Domain.Enums.MessageType.FeedBack);

            TicketData.LastUserAccess = this.UserAccessUpdate(TicketId);
            TicketData.Condition = Tk.Condition;

            //NOTIFICATION
            bool isDefault = true;
            TicketData.MailSettings = MailSettingsGet(User.Id, TicketId, true, ref isDefault);
            TicketData.IsDefault = isDefault;

            if (Tk.LastAssignment.AssignedTo != null)
            {
                TicketData.IsActiveUser = Tk.LastAssignment.AssignedTo.Id == User.Id;
            } 
            else if (Tk.LastAssignment.AssignedCategory != null)
            {
                TicketData.IsActiveUser = UserIsManagerOrResolverInCategory(Tk.LastAssignment.AssignedCategory.Id,
                    User.Id);
            }
            else
            {
                TicketData.IsActiveUser = UserIsManagerOrResolverInCategory(Tk.CreationCategory.Id,
                    User.Id);
            }

            TicketData.IsNotificationActive = User.IsNotificationActiveManager;

            return TicketData;
        }

        #endregion

        #region Assignment

        /// <summary>
        /// Assegna un ticket ad una categoria
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="CategoryId">Categoria</param>
        /// <returns></returns>
        public Domain.Enums.CategoryReassignError TicketAssignToCategory(
            Int64 TicketId,
            Int64 CategoryId,
            String Message,
            Domain.Enums.MessageUserType UserType,
            Boolean UpdateAccess,
            ref Int64 messageId)
        {

            if (UserType == Domain.Enums.MessageUserType.Partecipant
                || UserType == Domain.Enums.MessageUserType.none
                || CurrentPerson.TypeID == (int)UserTypeStandard.Guest
                || CurrentPerson.TypeID == (int)UserTypeStandard.ExternalUser)
                return Domain.Enums.CategoryReassignError.noPermission;

            Ticket tk = Manager.Get<Domain.Ticket>(TicketId);

            if (tk == null || tk.Status == TicketStatus.closeSolved || tk.Status == TicketStatus.closeUnsolved)
                return Domain.Enums.CategoryReassignError.invalidTicket;

            Int64 LastCategoryId = (from Domain.Assignment ass in Manager.GetIQ<Domain.Assignment>()
                                    where ass.Ticket.Id == TicketId
                                    && ass.AssignedCategory != null
                                    orderby ass.CreatedOn descending
                                    select ass.AssignedCategory.Id).Skip(0).Take(1).FirstOrDefault();

            if (LastCategoryId == CategoryId)
                return Domain.Enums.CategoryReassignError.noChange;

            Domain.Category NewCate = Manager.Get<Domain.Category>(CategoryId);

            if (NewCate == null)
                return Domain.Enums.CategoryReassignError.CategoryNotFound;

            IList<Domain.Assignment> OldLastAssignments = (from Domain.Assignment ass in Manager.GetIQ<Domain.Assignment>()
                                                           where ass.Ticket.Id == TicketId
                                                           && ass.IsCurrent == true
                                                           select ass).ToList();

            if (OldLastAssignments != null && OldLastAssignments.Any())
            {
                foreach (Assignment OldAss in OldLastAssignments)
                {
                    OldAss.IsCurrent = false;
                }

                Manager.SaveOrUpdateList<Domain.Assignment>(OldLastAssignments);
            }


            //Check per mantenere eventuale altro utente

            bool MantainOldUser = false;
            
            Domain.Assignment OldAssignment = OldLastAssignments.OrderByDescending(oa => oa.CreatedOn).FirstOrDefault();

            if(OldAssignment != null && OldAssignment.AssignedTo != null)
            {

                MantainOldUser = Manager.GetAll<LK_UserCategory>(uc => 
                    uc.Category.Id == NewCate.Id && 
                    uc.User.Id == OldAssignment.AssignedTo.Id).Any();
            }

            
            Domain.Assignment NewAssignment = new Assignment();
            NewAssignment.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            NewAssignment.IsCurrent = true;
            NewAssignment.Ticket = tk;
            NewAssignment.Type = Domain.Enums.AssignmentType.Category;
            NewAssignment.AssignedCategory = NewCate;
            NewAssignment.AssignedTo = (MantainOldUser)?
                OldAssignment.AssignedTo : null;

            tk.LastAssignment = NewAssignment;
            tk.Assignemts.Add(NewAssignment);

            Message msg = new Message();
            msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            msg.Text = Message;
            msg.Preview = "";
            msg.Creator = this.UserGetfromPerson(UC.CurrentUserID);
            msg.DisplayName = "";
            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Ticket = tk;
            msg.Type = Domain.Enums.MessageType.System;
            msg.UserType = UserType;
            msg.Visibility = true;

            msg.Action = Domain.Enums.MessageActionType.riassignedToCategory;
            msg.ToStatus = tk.Status;

            Domain.TicketUser usr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msg.ToUser = usr;
            msg.ToCategory = NewCate;

            tk.Messages.Add(msg);

            if (UpdateAccess)
            {
                if (tk.Owner.Id == this.CurrentUser.Id)
                {
                    tk.LastUserAccess = DateTime.Now;
                } 
                else if (tk.CreatedBy.Id == this.CurrentPerson.Id)
                {
                    tk.LastCreatorAccess = DateTime.Now;
                }
                else
                {
                    this.UserAccessUpdate(tk.Id);
                }
            }

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            Manager.SaveOrUpdate<Ticket>(tk);

            messageId = msg.Id;

            return Domain.Enums.CategoryReassignError.none;
        }

        /// <summary>
        /// Assegna un Ticket ad un utente specifico
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="UserId">Id Utente</param>
        /// <param name="AssignerUserId">Id assegnatario</param>
        /// <returns></returns>
        public bool TicketAssignToUser(
            Int64 TicketId,
            Domain.Enums.MessageUserType UserType,
            Int64 AssignerUserId,
            String Message,
            Boolean MessageIsVisible,
            Boolean IsManager,
            ref Int64 messageId
            )
        {
            TicketUser Assigner = UserGet(AssignerUserId);
            if (Assigner == null || Assigner.Id <= 0)
                return false;

            return TicketAssignToUser(TicketId, UserType, Assigner, Message, MessageIsVisible, IsManager, ref messageId);
        }

        /// <summary>
        /// Assegna un ticket ad una Person (utente di piattaforma)
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="UserId">Id Utente che compie l'azione</param>
        /// <param name="AssignerPersonId">ID Person a cui viene assegnato</param>
        /// <returns></returns>
        public bool TicketAssignToPerson(
            Int64 TicketId,
            Domain.Enums.MessageUserType UserType,
            int AssignerPersonId,
            String Message,
            Boolean MessageIsVisible,
            Boolean Ismanager,
            ref Int64 messageId
            )
        {
            TicketUser Assigner = UserGetfromPerson(AssignerPersonId);
            if (Assigner == null || Assigner.Id <= 0)
                return false;

            return TicketAssignToUser(TicketId, UserType, Assigner, Message, MessageIsVisible, Ismanager, ref messageId);
        }

        /// <summary>
        /// Assegna il ticket all'utente corrente
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="UserType"></param>
        /// <param name="Message"></param>
        /// <param name="MessageIsVisible"></param>
        /// <param name="IsManager"></param>
        /// <returns></returns>
        public bool TicketAssignToCurrent(
            Int64 TicketId,
            Domain.Enums.MessageUserType UserType,
            String Message,
            Boolean MessageIsVisible,
            Boolean IsManager,
            ref Int64 messageId
            )
        {
            return TicketAssignToUser(TicketId, UserType, CurrentUser, Message, MessageIsVisible, IsManager, ref messageId);
        }

        /// <summary>
        /// Assegna il Ticket as un Utente specifico
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="UserType"></param>
        /// <param name="AssignerUser"></param>
        /// <param name="Message"></param>
        /// <param name="MessageIsVisible"></param>
        /// <param name="IsManager"></param>
        /// <returns></returns>
        public bool TicketAssignToUser(
            Int64 TicketId,
            Domain.Enums.MessageUserType UserType,
            TicketUser AssignerUser,
            String Message,
            Boolean MessageIsVisible,
            Boolean IsManager,
            ref Int64 messageId
            )
        {
            if (UserType == Domain.Enums.MessageUserType.none
                || UserType == Domain.Enums.MessageUserType.Partecipant
                || CurrentPerson.TypeID == (int)UserTypeStandard.Guest
                || CurrentPerson.TypeID == (int)UserTypeStandard.ExternalUser)
                return false;

            Ticket CurTk = Manager.Get<Ticket>(TicketId);

            if (CurTk == null || CurTk.Status == TicketStatus.closeSolved || CurTk.Status == TicketStatus.closeUnsolved)
                return false;

            //IList<Assignment> TkAssignment = Manager.GetAll<Assignment>(a => a.Ticket != null && a.Ticket.Id == TicketId);
            Assignment NewAssignment = new Assignment();

            if (CurTk.Assignemts != null)
            {

                Assignment curAssignment = (from Assignment asg in CurTk.Assignemts where asg.IsCurrent && asg.AssignedTo != null select asg).
            ToList().FirstOrDefault();

                if (curAssignment != null && curAssignment.AssignedTo != null &&
                    curAssignment.AssignedTo.Id == AssignerUser.Id)
                    return false;

                Category LastCate = (from Assignment ass in CurTk.Assignemts where ass.AssignedCategory != null orderby ass.CreatedOn select ass.AssignedCategory).LastOrDefault();

                if (LastCate != null)
                    NewAssignment.AssignedCategory = LastCate;
                else
                {
                    NewAssignment.AssignedCategory = CurTk.CreationCategory;
                }

                IList<Assignment> CurrentAss = (from Assignment ass in CurTk.Assignemts where ass.IsCurrent == true select ass).ToList();

                foreach (Assignment ass in CurrentAss)
                {
                    ass.IsCurrent = false;
                    ass.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                }
            }
            NewAssignment.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            NewAssignment.AssignedTo = AssignerUser;
            NewAssignment.IsCurrent = true;
            NewAssignment.Ticket = CurTk;
            if (IsManager)
                NewAssignment.Type = Domain.Enums.AssignmentType.Manager;
            else
                NewAssignment.Type = Domain.Enums.AssignmentType.Resolver;

            CurTk.Assignemts.Add(NewAssignment);
            CurTk.LastAssignment = NewAssignment;
            CurTk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            //Message

            Message msg = new Message();
            msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (String.IsNullOrEmpty(Message))
                msg.Text = "Reassigned to: " + AssignerUser.Sname + " " + AssignerUser.Name;
            else if (Message.Contains(MessageUserPlaceHolder))
                msg.Text = Message.Replace(MessageUserPlaceHolder, AssignerUser.Sname + " " + AssignerUser.Name);
            else
                msg.Text = Message;

            //msg.Text = Message.Replace(MessageUserPlaceHolder, AssignerUser.Sname + " " + AssignerUser.Name);
            msg.Preview = "";
            msg.Creator = this.UserGetfromPerson(UC.CurrentUserID);
            msg.DisplayName = "";
            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Ticket = CurTk;
            msg.Type = Domain.Enums.MessageType.System;
            msg.UserType = UserType;
            msg.Visibility = MessageIsVisible;
            msg.Action = Domain.Enums.MessageActionType.riassignedToUser;
            msg.ToStatus = CurTk.Status;
            msg.ToUser = AssignerUser;
            msg.ToCategory = NewAssignment.AssignedCategory;

            CurTk.Messages.Add(msg);

            Manager.SaveOrUpdate<Ticket>(CurTk);

            messageId = msg.Id;

            return false;
        }

        #endregion

        #region Infos

        /// <summary>
        /// Recupera le INFO sul numero di Ticket (x UTENTE)
        /// </summary>
        /// <returns></returns>
        public Domain.DTO.DTO_ListInfo TicketListInfoGetUser()
        {
            Domain.DTO.DTO_ListInfo LI = new Domain.DTO.DTO_ListInfo();

            LI.DisplayUserName = UC.CurrentUser.SurnameAndName;

            IEnumerable<Domain.liteTicketUserInfo> TicketsUI = Manager.GetAll<Domain.liteTicketUserInfo>(t => t.OwnerId == this.CurrentUser.Id && !t.IsHide 
                && !(t.IsBehalf && t.IsDraft));

                LI.Draft = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.draft);
                LI.Open = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.open);
                LI.SolvedClosed = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.closeSolved);
                LI.UnSolvedClosed = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.closeUnsolved);
                LI.Waiting = TicketsUI.Count(t => t.Status == TicketStatus.userRequest);
                LI.WorkingOn = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.inProcess);
 

            return LI;
        }

        public Domain.DTO.DTO_ListInfo TicketListInfoGetBehalfer()
        {
            Domain.DTO.DTO_ListInfo LI = new Domain.DTO.DTO_ListInfo();

            LI.DisplayUserName = UC.CurrentUser.SurnameAndName;

            

            //IEnumerable<Domain.liteTicketUserInfo> TicketsUI = Manager.GetAll<Domain.liteTicketUserInfo>(t => t._CreatedById == CreatorId);
            IList<Domain.liteTicketUserInfo> TicketsUI = Manager.GetAll<Domain.liteTicketUserInfo>(t => t._CreatedById == this.CurrentPerson.Id || (t.OwnerId == this.CurrentUser.Id && !t.IsHide
                && !(t.IsBehalf && t.IsDraft)));


            //IList<Domain.liteTicketUserInfo> TicketsUI =
            //    Manager.GetAll<Domain.liteTicketUserInfo>(t =>
            //        t._CreatedById == this.CurrentPerson.Id
            //        || (t.OwnerId == this.CurrentUser.Id &&
            //            (!t.IsDraft && !t.IsHide)
            //            || (t.IsDraft && !t.IsBehalf)
            //        ));

            var TicketsUI_NotBh = TicketsUI.Where(t => t.IsBehalf == false);

            LI.Draft = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.draft);
            LI.Open = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.open);
            LI.SolvedClosed = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.closeSolved);
            LI.UnSolvedClosed = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.closeUnsolved);
            LI.Waiting = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.userRequest);
            LI.WorkingOn = TicketsUI_NotBh.Count(t => t.Status == Domain.Enums.TicketStatus.inProcess);

            var TicketsUI_Bh = TicketsUI.Where(t => t.IsBehalf == true);

            LI.BHDraft = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.draft);
            LI.BHOpen = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.open);
            LI.BHSolvedClosed = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.closeSolved);
            LI.BHUnSolvedClosed = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.closeUnsolved);
            LI.BHWaiting = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.userRequest);
            LI.BHWorkingOn = TicketsUI_Bh.Count(t => t.Status == Domain.Enums.TicketStatus.inProcess);
            

            return LI;
        }

        /// <summary>
        /// Recupera le INFO sul numero di Ticket (x UTENTE)
        /// </summary>
        /// <returns></returns>
        public Domain.DTO.DTO_ListInfo TicketListInfoGetUser(Domain.DTO.DTO_User User)
        {
            Domain.DTO.DTO_ListInfo LI = new Domain.DTO.DTO_ListInfo();

            LI.DisplayUserName = UC.CurrentUser.SurnameAndName;

            IEnumerable<Domain.liteTicketUserInfo> TicketsUI = Manager.GetAll<Domain.liteTicketUserInfo>(t => t.OwnerId == User.UserId && (!t.IsBehalf || (t.IsBehalf && (!t.IsHide && !t.IsDraft))));
             

            LI.Draft = TicketsUI.Count(t => t.Status == TicketStatus.draft);
            LI.Open = TicketsUI.Count(t => t.Status == TicketStatus.open);
            LI.SolvedClosed = TicketsUI.Count(t => t.Status == TicketStatus.closeSolved);
            LI.UnSolvedClosed = TicketsUI.Count(t => t.Status == TicketStatus.closeUnsolved);
            LI.Waiting = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.userRequest);
            LI.WorkingOn = TicketsUI.Count(t => t.Status == Domain.Enums.TicketStatus.inProcess);

            return LI;

            //Domain.DTO.DTO_ListInfo LI = new Domain.DTO.DTO_ListInfo();

            //LI.DisplayUserName = User.SName + " " + User.Name;

            //IEnumerable<Ticket> Tickets = Manager.GetAll<Domain.Ticket>(t => t.Creator.Id == User.UserId);

            //LI.Draft = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.draft).Count();
            //LI.Open = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.open).Count();
            //LI.SolvedClosed = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.closeSolved).Count();
            //LI.UnSolvedClosed = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.closeUnsolved).Count();
            //LI.Waiting = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.userRequest).Count();
            //LI.WorkingOn = Tickets.Where(t => t.Status == Domain.Enums.TicketStatus.inProcess).Count();

            //return LI;
        }

        /// <summary>
        /// Recupera le INFO sulla lista per Manager/Resolver
        /// </summary>
        /// <returns></returns>
        public Domain.DTO.DTO_ListInfo TicketListInfoGetManRes()
        {
            Domain.DTO.DTO_ListInfo infos = new Domain.DTO.DTO_ListInfo();

            Int64 UserId = CurrentUser.Id;

            if (UserId <= 0)
                return infos;

            Boolean GetAll = true;  //Se TRUE recupera ANCHE lo storico, altrimenti solo ultima assegnazione...

            IList<Int64> AllManagerCategoriesId = CategoriesIdGetManager(CurrentUser.Id);
            IList<Int64> ManagerTicketIdAll = ManResTicketId(UserId, GetAll, AllManagerCategoriesId, false);
            IList<Int64> ResolverTicketIdAll = ResolverTicketID(UserId, GetAll, null, AllManagerCategoriesId, ManagerTicketIdAll);

            IList<Int64> ALLTicketId = ManagerTicketIdAll.Union(ResolverTicketIdAll).ToList();

            int ALLCount = ALLTicketId.Count();

            List<List<Int64>> ALLTkIdList = new List<List<long>>();
            if (ALLCount > maxItemsForQuery)
            {
                int iter = (ALLCount / maxItemsForQuery);

                int i = 0;

                for (i = 0; i <= iter; i++)
                {
                    ALLTkIdList.Add(ALLTicketId.Skip(i * maxItemsForQuery).Take(maxItemsForQuery).ToList());
                }

                int last = ALLCount % maxItemsForQuery;
                if (last > 0)
                    ALLTkIdList.Add(ALLTicketId.Skip(i * maxItemsForQuery).Take(last).ToList());
            }
            else
            {
                ALLTkIdList.Add(ALLTicketId.ToList());
            }

            //INFO
            if (infos == null)
                infos = new Domain.DTO.DTO_ListInfo();

            foreach (IList<Int64> TkIds in ALLTkIdList)
            {
                IEnumerable<liteTicket> TkMan = (
                from liteTicket tk in Manager.GetIQ<liteTicket>()
                where TkIds.Contains(tk.Id) && tk.Status != Domain.Enums.TicketStatus.draft
                select tk
                );

                infos.Open += (from liteTicket tk in TkMan where tk.Status == Domain.Enums.TicketStatus.open select tk.Id).Count();
                infos.SolvedClosed += (from liteTicket tk in TkMan where tk.Status == Domain.Enums.TicketStatus.closeSolved select tk.Id).Count();
                infos.UnSolvedClosed += (from liteTicket tk in TkMan where tk.Status == Domain.Enums.TicketStatus.closeUnsolved select tk.Id).Count();
                infos.Waiting += (from liteTicket tk in TkMan where tk.Status == Domain.Enums.TicketStatus.userRequest select tk.Id).Count();
                infos.WorkingOn += (from liteTicket tk in TkMan where tk.Status == Domain.Enums.TicketStatus.inProcess select tk.Id).Count();
            }

            return infos;
        }

        #endregion


        #region Ticket - GET ManRes

        /// <summary>
        /// Recupera l'elenco ticket in base ai filtri impostati
        /// </summary>
        /// <param name="TicketsId"></param>
        /// <param name="filters"></param>
        /// <param name="Managers"></param>
        /// <returns></returns>
        private IList<Domain.DTO.DTO_TicketListItemManager> TicketsGetFilterManager(
            IList<Int64> TicketsId,
            Domain.DTO.DTO_ListFilterManager filters,
            IDictionary<Int64, bool> Managers
            )
        {

            bool filterCategory = (filters.CategoryId > 0);

            filters.Title = filters.Title.ToLower();
            


            IEnumerable<liteTicket> TkMan = (
                from liteTicket tk in Manager.GetIQ<liteTicket>()
                where TicketsId.Contains(tk.Id) && tk.Status != Domain.Enums.TicketStatus.draft
                select tk
                );

            if (!filters.ShowAllStatus)
                TkMan = TkMan.Where(t => t.Status == filters.Status);

            if (!String.IsNullOrEmpty(filters.Title))
            {
                TkMan = TkMan.Where(t => t.Title.ToLower().Contains(filters.Title));
            }

            //if ((filters.OnlyTicket & Domain.Enums.TicketManagerListOnly.noanswers) == Domain.Enums.TicketManagerListOnly.noanswers)
            //{
            //    TkMan = TkMan.Where(t => !t.Messages.Any(m => 
            //             (m.Type == MessageType.FeedBack ||
            //                m.Type == MessageType.PersonalFeedBack)
            //                && !m.IsDraft
            //                && m.Visibility));
            //}

            if ((filters.OnlyTicket & Domain.Enums.TicketManagerListOnly.notassigned) == Domain.Enums.TicketManagerListOnly.notassigned)
            {
                TkMan = TkMan.Where(t => !t.Assignments.Any(a => a.IsCurrent && t.LastAssignment.AssignedTo != null));
                //&& a.Type != AssignmentType.Category 
            }

            

            if (String.IsNullOrEmpty(filters.LanguageCode) || filters.LanguageCode.ToLower() == "all")
                filters.LanguageCode = "";

            if (!String.IsNullOrEmpty(filters.LanguageCode))
                TkMan = TkMan.Where(t => t.LanguageCode == filters.LanguageCode);

            if (filters.DateStart != null || filters.DateEnd != null)
            {
                switch (filters.DateField)
                {
                    case Domain.Enums.TicketManagerDateFilter.Creation:
                        if (filters.DateStart != null)
                        {
                            DateTime dt = (DateTime)filters.DateStart;
                            TkMan = TkMan.Where(t => t.OpenOn >= dt);
                        }
                        if (filters.DateEnd != null)
                        {
                            DateTime dt = (DateTime)filters.DateEnd;
                            TkMan = TkMan.Where(t => t.OpenOn <= dt);
                        }
                        break;
                    case Domain.Enums.TicketManagerDateFilter.LastMessage:
                        if (filters.DateStart != null)
                        {
                            DateTime dt = (DateTime)filters.DateStart;

                            TkMan = TkMan.Where(t => t.Messages.Where(m => !m.IsDraft && m.Type != MessageType.System)
                                .OrderByDescending(m => m.CreatedOn).Skip(0).Take(1).ToList().FirstOrDefault().CreatedOn >= dt);
                        }
                        if (filters.DateEnd != null)
                        {
                            DateTime dt = (DateTime)filters.DateEnd;
                            TkMan = TkMan.Where(t => t.Messages.Where(m => !m.IsDraft && m.Type != MessageType.System)
                                .OrderByDescending(m => m.CreatedOn).Skip(0).Take(1).ToList().FirstOrDefault().CreatedOn <= dt);

                        }
                        break;
                    case Domain.Enums.TicketManagerDateFilter.LastAssignment:
                        if (filters.DateStart != null)
                        {
                            DateTime dt = (DateTime)filters.DateStart;
                            TkMan = TkMan.Where(t => t.LastAssignment.CreatedOn >= dt && t.LastAssignment.Type != AssignmentType.Category);

                        }
                        if (filters.DateEnd != null)
                        {
                            DateTime dt = (DateTime)filters.DateEnd;
                            TkMan = TkMan.Where(t => t.LastAssignment.CreatedOn <= dt && t.LastAssignment.Type != AssignmentType.Category);

                        }
                        break;
                    case Domain.Enums.TicketManagerDateFilter.LastModify:
                        if (filters.DateStart != null)
                        {
                            DateTime dt = (DateTime)filters.DateStart;
                            TkMan = TkMan.Where(t => (t.ModifiedOn ?? t.OpenOn) <= dt);
                        }
                        if (filters.DateEnd != null)
                        {
                            DateTime dt = (DateTime)filters.DateEnd;
                            TkMan = TkMan.Where(t => (t.ModifiedOn ?? t.OpenOn) >= dt);
                        }
                        break;
                }
            }

            string LangCode = "";

            LangCode = (UC.Language != null) ? UC.Language.Code : "";

            if (String.IsNullOrEmpty(LangCode))
                LangCode = LangMultiCODE;

            //DateTime Start = DateTime.Now;
            
            IList<Domain.DTO.DTO_TicketListItemManager> TicketsManager =
                (from liteTicket tk in TkMan
                 select new Domain.DTO.DTO_TicketListItemManager
                 {
                     Id = tk.Id,
                     Code = tk.Code,
                     CreationCategoryName = tk.CreationCategory.GetTranslatedName(LangCode),
                     LastAssignment = tk.LastAssignment,
                     LangCode = tk.LanguageCode,
                     HasNews = (tk.ModifiedOn ?? tk.OpenOn) > this.UserAccessGet(tk.Id),
                     IsForManager = Managers.ContainsKey(tk.Id),
                     SendedOn = tk.OpenOn,
                     Status = tk.Status,
                     Title = tk.Title,
                     ModifiedOn = (tk.ModifiedOn != null) ? (DateTime)tk.ModifiedOn : DateTime.MinValue,
                     CommunityName = (tk.Community != null) ? tk.Community.Name : "",
                     IsAsgnToMe = (tk.LastAssignment.AssignedTo != null && tk.LastAssignment.AssignedTo.Person != null && tk.LastAssignment.AssignedTo.Person.Id == this.CurrentPerson.Id) ? true : false,
                     HasNoAnswers = !tk.Messages.Any(m => 
                         (m.Type == MessageType.FeedBack ||
                            m.Type == MessageType.PersonalFeedBack)
                            && !m.IsDraft
                            && m.Visibility),
                     NumAttachments = 0,
                     Condition = tk.Condition,
                     CategoryFilter = 
                        (filters.CategoryId <= 0 )? Domain.Enums.CategoryFilterStatus.None :
                            (tk.LastAssignment.AssignedCategory != null && tk.LastAssignment.AssignedCategory.Id == filters.CategoryId)? Domain.Enums.CategoryFilterStatus.Current :
                                ((tk.CreationCategory.Id == filters.CategoryId)? Domain.Enums.CategoryFilterStatus.Creation : CategoryFilterStatus.History)
                 }
            ).ToList();

            //Usa una funzione che "prima" del .ToList() pare non funzioni...

            

            if ((filters.OnlyTicket & Domain.Enums.TicketManagerListOnly.withnews) == Domain.Enums.TicketManagerListOnly.withnews)
            {
                TicketsManager = TicketsManager.Where(tm => tm.HasNews).ToList();
            }

            if ((filters.OnlyTicket & Domain.Enums.TicketManagerListOnly.noanswers) == Domain.Enums.TicketManagerListOnly.noanswers)
            {
                TicketsManager = TicketsManager.Where(tm => tm.HasNoAnswers).ToList();
            }

            //if ((filters.OnlyTicket & Domain.Enums.TicketManagerListOnly.notassigned) == Domain.Enums.TicketManagerListOnly.notassigned)
            //{
            //    FiltTkManager = TicketsManager.Where(tm => tm.LastAssignment != null || tm.LastAssignment.Id <= 0); //.ToList();
            //}

          
            //TimeSpan Duration = DateTime.Now - Start;

            return TicketsManager;
        }

        /// <summary>
        /// Recupera elenco Ticket per Manager/Resolver
        /// </summary>
        /// <param name="filters">Filtri vari</param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_TicketListItemManager> TicketsGetManRes(
            ref Domain.DTO.DTO_ListFilterManager filters
            )
        {
            List<Int64> AllManagerCategoriesId = new List<Int64>();
            IList<Int64> ManagerTicketIdAll = new List<Int64>();
            IList<Int64> ResolverTicketIdAll = new List<Int64>();
            Int64 UserId = CurrentUser.Id;

            Boolean GetAll = true;
            
            if (UserId <= 0)
                return null;

            if (filters.CategoryId > 0)
            {
                IList<Int64> ManagerCategoriesId = CategoriesIdGetManager(CurrentUser.Id, filters.CategoryId);

                if (ManagerCategoriesId != null && ManagerCategoriesId.Any())
                    ManagerTicketIdAll = ManResTicketId(UserId, GetAll, ManagerCategoriesId, true);
                else
                {
                    IList<Int64> ResCateIds = new List<Int64>();
                    ResCateIds.Add(filters.CategoryId);
                    ResolverTicketIdAll = ResolverTicketID(UserId, GetAll, ResCateIds, null, ManagerTicketIdAll);
                }

            }
            else
            {
                AllManagerCategoriesId = CategoriesIdGetManager(CurrentUser.Id);
                ManagerTicketIdAll = ManResTicketId(UserId, GetAll, AllManagerCategoriesId, false);
                ResolverTicketIdAll = ResolverTicketID(UserId, GetAll, null, AllManagerCategoriesId, ManagerTicketIdAll);
            }

            //Creo un dictionary che mi servirà successivamente per identificare i Ticket di cui sono MANAGER
            IDictionary<Int64, bool> DICT_TkIdManRes = ManagerTicketIdAll.Distinct().ToDictionary(k => k, v => true);

            IList<Int64> ALLTicketId = ManagerTicketIdAll.Union(ResolverTicketIdAll).ToList();

            int ALLCount = ALLTicketId.Count();

            List<List<Int64>> ALLTkIdList = new List<List<long>>();
            if (ALLCount > maxItemsForQuery)
            {
                int iter = (ALLCount / maxItemsForQuery);

                int i = 0;

                for (i = 0; i <= iter; i++)
                {
                    ALLTkIdList.Add(ALLTicketId.Skip(i * maxItemsForQuery).Take(maxItemsForQuery).ToList());
                }

                int last = ALLCount % maxItemsForQuery;
                if (last > 0)
                    ALLTkIdList.Add(ALLTicketId.Skip(i * maxItemsForQuery).Take(last).ToList());
            }
            else
            {
                ALLTkIdList.Add(ALLTicketId.ToList());
            }

            IList<Domain.DTO.DTO_TicketListItemManager> ALLdtoTicket = new List<Domain.DTO.DTO_TicketListItemManager>();

            //Per evitare query troppo grandi, viene diviso in più "sotto-query"...
            foreach (IList<Int64> TkIds in ALLTkIdList)
            {
                IList<Domain.DTO.DTO_TicketListItemManager> tmpdtoTicket = TicketsGetFilterManager(
                TkIds,
                filters,
                DICT_TkIdManRes);

                if (tmpdtoTicket != null && tmpdtoTicket.Any())
                    ALLdtoTicket = ALLdtoTicket.Union(tmpdtoTicket).ToList();
            }

            int totalTicketCount = 0;
            
            if(ALLdtoTicket != null && ALLdtoTicket.Any())
                totalTicketCount = ALLdtoTicket.Count;

            

            //Ordinamenti
            switch (filters.OrderField)
            {
                case Domain.Enums.TicketOrderManRes.code:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.Id).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.Id).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.subject:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.Title).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.Title).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.language:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.LangCode).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.LangCode).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.community:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.CommunityName).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.CommunityName).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.association:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.LastAssignment.AssignetToSurNameAndName).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.LastAssignment.AssignetToSurNameAndName).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.category:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.CreationCategoryName).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.CreationCategoryName).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.lifeTime:
                    if (filters.OrderAscending)
                        
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.SendedOn).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.SendedOn).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.lastModify:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.ModifiedOn).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.ModifiedOn).ToList();
                    break;
                case Domain.Enums.TicketOrderManRes.status:
                    if (filters.OrderAscending)
                        ALLdtoTicket = ALLdtoTicket.OrderBy(t => t.Status).ToList();
                    else
                        ALLdtoTicket = ALLdtoTicket.OrderByDescending(t => t.Status).ToList();
                    break;

            }

            //Paginazioni
            filters.RecordTotal = totalTicketCount; 
            if (ALLCount <= filters.PageSize)
                filters.PageIndex = 0;
            else
            {
                if (filters.PageIndex * filters.PageSize >= ALLCount)
                {
                    filters.PageIndex = 0;
                }
            }


            return ALLdtoTicket.Skip(filters.PageIndex * filters.PageSize).Take(filters.PageSize).ToList();


        }

        /// <summary>
        /// Recupera un elenco di ID Ticket a cui l'utente indicato ha accesso come MANAGER
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="GetAll">Se recuperare SOLO dalle assegnazioni correnti o anche dallo storico.</param>
        /// <param name="AllManagerCategoriesId">Id delle categorie di cui sono manager (completa, flat)</param>
        /// <returns></returns>
        private IList<Int64> ManResTicketId(
            Int64 UserId,
            Boolean GetAll,
            IList<Int64> AllManagerCategoriesId,
            bool FilterForCategories)
        {
            //Associazioni legate a quelle categorie

            //var CatManagerQuery = Manager.GetIQ<Assignment>()
            //    .Where(a => a.Type == Domain.Enums.AssignmentType.Category &&
            //    AllManagerCategoriesId.Contains(a.AssignedCategory.Id));


            IList<Int64> ManTkId = new List<Int64>();
            

            if(AllManagerCategoriesId != null && AllManagerCategoriesId.Any())
            {
                var CatManagerQuery = Manager.GetIQ<liteTinyAssignment>();

                CatManagerQuery = CatManagerQuery.Where(a => a.Type == Domain.Enums.AssignmentType.Category
                    && a.CategoryId != null
                    && AllManagerCategoriesId.Contains((Int64)a.CategoryId));

                if (!GetAll)
                CatManagerQuery = CatManagerQuery.Where(a => a.IsCurrent == true);

                ManTkId = (from liteTinyAssignment ass in CatManagerQuery
                                    where ass.TicketId != null
                                    select (Int64)ass.TicketId).ToList();


            }

            if (ManTkId == null)
            {
                ManTkId = new List<Int64>();
            }

            //Ticket delle associazioni legate alle categorie in cui sono Manager


            //where ass.Ticket != null 
            // SE viene cancellato un Ticket, ma non gli assignment! (dB ONLY!)
             

            //Associazioni in cui sono inserito come AMMINISTRATORE
            if (!FilterForCategories)
            {
                var UsrManagerQuery = (from liteTinyAssignment ass 
                                       in Manager.GetIQ<liteTinyAssignment>()
                             .Where(a =>
                                 a.Type == Domain.Enums.AssignmentType.Manager
                                 && a.TicketId != null
                                 && a.UserId != null && (Int64)a.UserId == UserId)
                                   select ass);

                if (!GetAll)
                    UsrManagerQuery = UsrManagerQuery.Where(a => a.IsCurrent == true);

                ManTkId = (from liteTinyAssignment ass in UsrManagerQuery
                       select (Int64)ass.TicketId).ToList().Union(ManTkId).Distinct().ToList();
            }
            return ManTkId;
        }

        /// <summary>
        /// Recupera gli ID dei Ticket di cui l'utente è Resolver
        /// </summary>
        /// <param name="UserId">ID Utente</param>
        /// <param name="GetAll">Se recuperare tutti (true) o SOLO da associazioni correnti (false)</param>
        /// <param name="ExcludedCategoryIds">Id Categorie da escludere (di cui User è MANAGER)</param>
        /// <param name="ExcludedTicktIds">Id Ticket da escludere (di cui è MANAGER)</param>
        /// <returns></returns>
        private IList<Int64> ResolverTicketID(
            Int64 UserId, Boolean GetAll,
            IList<Int64> IncludedCategoryIds,
            IList<Int64> ExcludedCategoryIds,
            IList<Int64> ExcludedTicktIds)
        {
            //Id Categorie di cui l'utente è RESOLVER
            if (!(IncludedCategoryIds != null && IncludedCategoryIds.Any()))
            {

                IncludedCategoryIds = (from liteLK_UserCategory lkuc 
                                           in Manager.GetIQ<liteLK_UserCategory>()
                                       where lkuc.IdUser != null && lkuc.IdUser == UserId 
                                       && lkuc.IsManager == false
                                       && lkuc.IdCategory != null
                                       select (Int64)lkuc.IdCategory).ToList();
            }

            //Se ci sono Id Categorie da togliere, le tolgo
            if (ExcludedCategoryIds != null && ExcludedCategoryIds.Any())
                IncludedCategoryIds = IncludedCategoryIds.Except(ExcludedCategoryIds).Distinct().ToList();

            //Assignmente legati alle categorie o all'utente
            var ResQUERY = (from liteTinyAssignment ass in Manager.GetIQ<liteTinyAssignment>()
                            where ass.Type != Domain.Enums.AssignmentType.Manager
                            && ass.TicketId != null
                            &&  (
                                (ass.CategoryId != null && IncludedCategoryIds.Contains((Int64)ass.CategoryId)) 
                                ||
                                (ass.UserId != null && ass.UserId == UserId))
                            select ass);

            //Se solo correnti, prendo solo quelle
            if (!GetAll)
                ResQUERY = ResQUERY.Where(a => a.IsCurrent == true);

            //Recupero i Ticket di quegli assignment
            IList<Int64> ResTkId = (from liteTinyAssignment ass in ResQUERY
                                    select (Int64)ass.TicketId).Distinct().ToList();

            //Se ci sono Ticket da escludere, li escludo
            if (ExcludedTicktIds != null && ExcludedTicktIds.Any())
                ResTkId = ResTkId.Except(ExcludedTicktIds).Distinct().ToList();

            return ResTkId;
        }

        #endregion



        public Domain.DTO.DTO_Ticket TicketCreateNewDraft(Int64 UserId, int CommunityId, String DefTitle, String DefPreview = "")
        {
            if (String.IsNullOrEmpty(DefTitle))
                DefTitle = "New Draft";

            if (UserId <= 0)
                UserId = this.UserGetIdfromPerson(CurrentPerson.Id);

            if (UserId <= 0)
                return null;

            TicketUser Usr = Manager.Get<TicketUser>(UserId);

            if (Usr == null || Usr.Id <= 0)
                return null;

            //No, vengono messa nella view quelle "di default",
            //e successivamente utilizzate in cascata.
            //Domain.Enums.MailSettings MailSett = this.MailSettingsGet(Usr.Id);

            Category DefaultCategory = this.CategoryDefaultGet();
            
            if (DefaultCategory == null)
                return null;
            
            Ticket Tk = new Ticket();
            Tk.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            Tk.IsDraft = true;
            Tk.LanguageCode = Usr.LanguageCode;
            Tk.Community = Manager.GetCommunity(CommunityId);
            Tk.Condition = Domain.Enums.TicketCondition.active;
            Tk.Owner = Usr;
            Tk.LastUserAccess = DateTime.Now;
            Tk.OpenOn = DateTime.Now;
            Tk.Preview = DefPreview;
            Tk.Status = Domain.Enums.TicketStatus.draft;
            Tk.Title = DefTitle;
            Tk.CreationCategory = DefaultCategory;
            
            Assignment DefCateAss = new Assignment();
            DefCateAss.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            DefCateAss.AssignedCategory = DefaultCategory;
            DefCateAss.IsCurrent = true;
            DefCateAss.Ticket = Tk;
            DefCateAss.Type = Domain.Enums.AssignmentType.Category;
            Tk.Assignemts = new List<Assignment>();
            Tk.Assignemts.Add(DefCateAss);

            Message Msg = new Message();
            Msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            Msg.Action = Domain.Enums.MessageActionType.normal;
            Msg.Creator = Usr;
            Msg.IsDraft = true;
            Msg.Preview = DefPreview;
            Msg.ShowRealName = true;
            Msg.Text = DefPreview;
            Msg.Ticket = Tk;
            Msg.ToCategory = DefaultCategory;
            Msg.ToStatus = Domain.Enums.TicketStatus.userRequest;
            Msg.Type = Domain.Enums.MessageType.Request;

            Tk.Messages.Add(Msg);

            Manager.SaveOrUpdate<Ticket>(Tk);

            Domain.DTO.DTO_Ticket dtoTK = new Domain.DTO.DTO_Ticket();
            
            if (Tk != null && Tk.Owner.Id == UserId && Tk.IsDraft)
            {
                dtoTK.CategoryId = Tk.CreationCategory.Id;
               
                dtoTK.IsDraft = Tk.IsDraft;
                dtoTK.LanguageCode = Tk.LanguageCode;
                dtoTK.DraftMsgId = Msg.Id;

                if (Tk.Messages != null && Tk.Messages.Count() > 0)
                {
                    dtoTK.Text = (from Message msg in Tk.Messages where msg.Creator != null && msg.Creator.Id == UserId orderby msg.CreatedOn descending select msg.Text).FirstOrDefault();
                }

                dtoTK.TicketId = Tk.Id;
                dtoTK.Code = Tk.Code;
                dtoTK.Title = Tk.Title;

                if (Tk.Community == null)
                    dtoTK.CommunityId = -1;
                else
                    dtoTK.CommunityId = Tk.Community.Id;

                if (Tk.Owner.Person != null)
                {
                    dtoTK.OwnerName = Tk.Owner.Person.Name;
                    dtoTK.OwnerSName = Tk.Owner.Person.Surname;
                    dtoTK.OwnerMail = Tk.Owner.Person.Mail;
                }
                else
                {
                    dtoTK.OwnerName = Tk.Owner.Name;
                    dtoTK.OwnerSName = Tk.Owner.Sname;
                    dtoTK.OwnerMail = Tk.Owner.mail;    
                }

                
            }

            //dtoTK.MailSettings = this.MailSettingsGet(this.CurrentUser.Id, dtoTK.TicketId); //UC.CurrentCommunityID,

            dtoTK.IsBehalf = false;
            dtoTK.IsOwnerNotificationActive = true;

            return dtoTK;
        }

        int _numDraft = -1;

        public int TicketGetNumDraft(Int64 UserID)
        {
            if (_numDraft < 0)
            {
                _numDraft = (from Domain.liteDraftTicket tk in Manager.GetIQ<Domain.liteDraftTicket>()
                    where tk.IsDraft == true && tk.Deleted == BaseStatusDeleted.None && tk.CreatorId == UserID
                       select tk.Id).Count();

                _hasDraft = (_numDraft > 0)? true : false;
                _draftCalculated = true;
            }

            return _numDraft;
        }

        public int TicketGetNumOpen(Int64 UserID)
        {
            return (from Ticket tk in Manager.GetIQ<Ticket>()
                    where tk.IsDraft == false && tk.Deleted == BaseStatusDeleted.None
                    && (tk.Status == Domain.Enums.TicketStatus.inProcess ||
                        tk.Status == Domain.Enums.TicketStatus.open ||
                        tk.Status == Domain.Enums.TicketStatus.userRequest)
                    && tk.Owner != null && tk.Owner.Id == UserID
                    select tk.Id).Count();
        }

        bool _hasDraft = false;
        bool _draftCalculated = false;

        public bool TicketUserHasDraft()
        {
            if(_draftCalculated == false)
            {
                _hasDraft = (from Domain.liteDraftTicket tk in Manager.GetIQ<Domain.liteDraftTicket>() where tk.IsDraft == true && tk.CreatorId == this.CurrentUser.Id select tk.Id).Any();
                _draftCalculated = true;
            }
            
            return _hasDraft;
            
        }


        public Domain.Enums.TicketDraftDeleteError TicketDeleteDraft(Int64 TkDraftId, String baseFilePath, String baseThumbnailPath)
        {
            return TicketDeleteDraft(TkDraftId, this.UserGetfromPerson(UC.CurrentUserID).Id, baseFilePath, baseThumbnailPath);

        }

        public Domain.Enums.TicketDraftDeleteError TicketDeleteDraft(Int64 TkDraftId, Int64 TkUserId, String baseFilePath, String baseThumbnailPath)
        {

            if(Manager.IsInTransaction())
            {
                try
                {
                    Manager.Commit();
                }
                catch {}
            }


            if(!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }

            Domain.Enums.TicketDraftDeleteError response =
                AttachmentDeleteAll(TkDraftId, TkUserId, baseFilePath, baseThumbnailPath);

            if (response == Domain.Enums.TicketDraftDeleteError.none)
            {
                Ticket Tk = Manager.Get<Ticket>(TkDraftId);

                if (Tk == null)
                    return Domain.Enums.TicketDraftDeleteError.TicketNotFound;
                if (!Tk.IsDraft)
                    return Domain.Enums.TicketDraftDeleteError.TicketNotInDraft;
                if (Tk.Owner == null || Tk.Owner.Id != TkUserId)
                    return Domain.Enums.TicketDraftDeleteError.TicketNotMine;

                IList<Domain.liteManResTicketAccess> Access = Manager.GetAll<Domain.liteManResTicketAccess>(ta => ta.TicketId == TkDraftId);

                try
                {
                    Manager.DeletePhysicalList<Domain.liteManResTicketAccess>(Access);
                    //Manager.SaveOrUpdateList<Domain.Assignment>(Tk.Assignemts);
                    //Manager.DeletePhysicalList<Domain.Assignment>(Tk.Assignemts);
                    //Manager.DeletePhysicalList<Domain.Message>(Tk.Messages);
                    Manager.DeletePhysical<Ticket>(Tk);
                } catch
                {
                    response = Domain.Enums.TicketDraftDeleteError.dBError;
                }

                if(Manager.IsInTransaction())
                {
                    try
                    {
                        Manager.Commit();
                    } catch
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }


                    try
                    {
                        Manager.Flush();
                    } catch(Exception ex)
                    {
                        String err = ex.ToString();
                    }
                    
                }
            }

            return response;
            //return Domain.Enums.TicketDraftDeleteError.none;

        }

        public bool TicketSetBehalfUser(Int64 TicketId, Int64 UserId, bool HideToOwner, ref Int64 messageId)
        {
            bool output = true;

            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            TicketSetBehalf(TicketId, Manager.Get<TicketUser>(UserId), HideToOwner, ref messageId);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                output = false;
            }

            return output;
        }

        /// <summary>
        /// Imposta l'Owner di un Ticket
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="PersonId">Owner</param>
        /// <param name="HideToOwner">Se nascosto all'owner</param>
        /// <param name="messageId">Messaggio di riferimento x NOTIFICHE</param>
        /// <returns></returns>
        public bool TicketSetBehalfPerson(
            Int64 TicketId, 
            Int32 PersonId, 
            bool HideToOwner,
            ref Int64 messageId)
        {
            //bool output = true;

            //if (!Manager.IsInTransaction())
            //    Manager.BeginTransaction();


            return TicketSetBehalf(TicketId, this.UserGetfromPerson(PersonId), HideToOwner, ref messageId);

            //try
            //{
            //    Manager.Commit();
            //}
            //catch (Exception)
            //{
            //    Manager.RollBack();
            //    output = false;
            //}

            //return output;
        }

     public bool TicketSetBehalfCurrent(Int64 TicketId, ref Int64 messageId)
        {
            //bool output = true;

            //if (!Manager.IsInTransaction())
            //    Manager.BeginTransaction();

            return TicketSetBehalf(TicketId, this.UserGetfromPerson(UC.CurrentUserID), false, ref messageId);

            //try
            //{
            //    Manager.Commit();
            //}
            //catch (Exception)
            //{
            //    Manager.RollBack();
            //    output = false;
            //}

            //return output;
        }


        //Imposta il 
        private bool TicketSetBehalf(Int64 TicketId, TicketUser User, bool HideToOwner, ref Int64 messageId)
        {
            bool output = true;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            Ticket tk = Manager.Get<Ticket>(TicketId);
            //if (!tk.IsDraft)
            //    return;

            if (tk.IsBehalf && tk.Owner.Id == User.Id)
                return false;
            //Il Ticket è già assegnato all'utente (Es: REFRESH PAGINA)


            TicketUser OldOwner = tk.Owner;

            tk.Owner = User;

            tk.IsBehalf = (tk.Owner.Person != null && tk.Owner.Person.Id != tk.CreatedBy.Id) ? true : false;

            try
            {
                tk.Messages.Where(m => m.Creator.Id == OldOwner.Id && m.Type != MessageType.System && m.UserType == MessageUserType.Partecipant)
                    .ToList()
                .ForEach(m =>
                {
                    m.Creator = User;
                    m.IsBehalf = tk.IsBehalf;
                });
            }
            catch (Exception)
            {
                
                throw;
            }

            tk.IsHideToOwner = HideToOwner;
                

            //if (tk.IsBehalf)
            //{
            Message msg = tk.Messages.OrderByDescending(m => m.CreatedOn).FirstOrDefault();
            if (msg == null)
            {
                msg = new Message();
            }

            tk.LastUserAccess = null;
            tk.LastCreatorAccess = DateTime.Now;

            Message msgBehalf = null;
            if(!tk.IsDraft)
            {
                msgBehalf = new Message();
                msgBehalf.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                //RIVEDERE!!!

                msgBehalf.Text = "Created by: " + this.CurrentPerson.SurnameAndName; //msg.CreatedBy.SurnameAndName;
                msgBehalf.Preview = "";
                msgBehalf.Creator = this.CurrentUser;
                msgBehalf.DisplayName = "";
                msgBehalf.SendDate = DateTime.Now;
                msgBehalf.ShowRealName = true;
                msgBehalf.Ticket = tk;
                msgBehalf.Type = Domain.Enums.MessageType.System;
                msgBehalf.UserType = MessageUserType.Partecipant;
                msgBehalf.Visibility = true;
                if (tk.IsBehalf)
                {
                    msgBehalf.Action = Domain.Enums.MessageActionType.behalfSet;   
                }
                else
                {
                    msgBehalf.Action = Domain.Enums.MessageActionType.behalfRemove;   
                }

                msgBehalf.ToStatus = tk.Status;
                msgBehalf.ToUser = tk.Owner;
                msgBehalf.ToCategory = msg.ToCategory;
                msgBehalf.IsBehalf = true;
                //Manager.SaveOrUpdate<Message>(msgBehalf);
                tk.Messages.Add(msgBehalf);
            }
            //}
            
            
            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            Manager.SaveOrUpdate<Ticket>(tk);

            // Impostazioni notifiche: rivedere...

            Domain.MailNotification notSetting =
                (Manager.GetAll<Domain.MailNotification>(s => s.Ticket != null && s.Ticket.Id == tk.Id && s.IsPortal ==
                            false && s.User != null && s.User.Id == OldOwner.Id)).FirstOrDefault();

            if (notSetting != null)
            {
                if (notSetting.IsDefaultManager)
                {
                    Manager.DeletePhysical(notSetting);
                }
                else
                {
                    notSetting.IsDefaultUser = true;
                    notSetting.Settings |= MailSettings.UserDefault;
                    Manager.SaveOrUpdate(notSetting);
                }

            }

            if (msgBehalf != null)
                messageId = msgBehalf.Id;
            else
            {
                messageId = -1;
            }

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                output = false;
            }

            return output;
        }

        /// <summary>
        /// Recupera l'ID PERSON dell'OWNER del Ticket.
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns>ID PERSON. SE -1 è Utente TICKET!</returns>
        public Int32 TicketGetOwnerPersonId(Int64 TicketId)
        {
            Ticket tk = Manager.Get<Ticket>(TicketId);

            return ((tk.Owner.Person != null)? tk.Owner.Person.Id : -1);
        }

        public bool TicketVisibilitySet(Int64 TicketId, bool HideToUser)
        {
            bool success = true;
            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();


            Ticket tk = Manager.Get<Ticket>(TicketId);

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            tk.IsHideToOwner = HideToUser;

            Manager.SaveOrUpdate<Ticket>(tk);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                success = false;
                //throw;
            }

            return success;
        }
    }
}
