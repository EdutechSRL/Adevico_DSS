using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        /// <summary>
        /// Aggiunge un messeggio DELL'UTENTE al Ticket indicato
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="Text">Testo messaggio</param>
        /// <param name="Preview">Eventuale anteprima</param>
        /// <param name="ChangeStatus">Indica se dopo l'invio il ticket dovrà modificare il suo stato (riapertura/chiusura)</param>
        /// <param name="NewStatus">Il nuovo stato (se va modificato)</param>
        /// <returns>Eventuali errori</returns>
        public Domain.Enums.TicketMessageSendError MessageSendUser(
            Int64 TicketId,
            String Text,
            String Preview,
            ref Int64 MessageId,
            Boolean IsDraft = false)
        {



            //Message ChkMsg = Manager.Get<Message>(DraftId);
            //if(!ChkMsg.IsDraft)
            //    return TicketMessageSendError.none;


            bool isTicketClosed = false;
            if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(Preview))
                return Domain.Enums.TicketMessageSendError.NoMessage;

            Ticket tk = Manager.Get<Ticket>(TicketId);

            if (tk == null)
                return Domain.Enums.TicketMessageSendError.TicketNotFound;

            if (tk.IsDraft)
                return Domain.Enums.TicketMessageSendError.DraftTicket;

            Domain.TicketUser Usr = this.CurrentUser;

            if (tk.Owner.Id != Usr.Id)
            {
                if (!tk.IsBehalf)
                    return Domain.Enums.TicketMessageSendError.NoPermission;

                if(!this.SettingPermissionGet(Domain.Enums.PermissionType.Behalf))
                    return Domain.Enums.TicketMessageSendError.NoPermission;
            }


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();


             //this.UserGetfromPerson(UC.CurrentUserID);


            if (Usr == null || Usr.Id <= 0)
                return Domain.Enums.TicketMessageSendError.NoPermission;

            bool IsNew = false;
            bool IsBehalf = false;

            Message msg = new Message();

            if (!tk.IsBehalf || tk.Owner.Id == Usr.Id)
            {
                msg = (from Message m in tk.Messages
                    where m.IsDraft == true &&
                          m.Creator != null && m.Creator.Id == Usr.Id
                    select m).FirstOrDefault();
            }
            else
            {
                msg = (from Message m in tk.Messages
                       where m.IsDraft == true &&
                             m.CreatedBy != null && m.CreatedBy.Id == this.CurrentPerson.Id
                       select m).FirstOrDefault();
                IsBehalf = true;
            }


            if (msg == null || msg.Id <= 0)
            {
                //msg = Manager.Get<Message>(DraftId);
                //if (!msg.IsDraft)
                //{
                //    return TicketMessageSendError.none;
                //}
                //else if (msg.Ticket == null || msg.Ticket.Id != TicketId)
                //{
                    msg = new Message();
                    IsNew = true;    
                //}

            }
            //else
            //{
               
            //    //    TEORICAMENTE NON SERVE, MA FORSE MEGLIO METTERCELO!!!
            //    if (DraftId != msg.Id)
            //    {
            //        //UPDATE FILE MESSAGE ID, FROM DraftId To msgId!!!
            //    }
            //}

            if (IsNew)
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            else
                msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            msg.Text = Text;
            msg.Preview = Preview;
            //msg.Creator = Usr;
            msg.Creator = tk.Owner;
            msg.DisplayName = "";
            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Ticket = tk;
            msg.Type = Domain.Enums.MessageType.Request;
            msg.UserType = Domain.Enums.MessageUserType.Partecipant;
            msg.Visibility = true;

            if (tk.Status == TicketStatus.closeSolved || tk.Status == TicketStatus.closeUnsolved)
            {
                msg.IsDraft = true;
                isTicketClosed = true;
            }
            else
            {
                msg.IsDraft = IsDraft;
            }

            msg.Action = Domain.Enums.MessageActionType.normal;
            msg.ToStatus = tk.Status;

            msg.IsBehalf = IsBehalf;

            Domain.TicketUser Asusr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msg.ToUser = Asusr;

            Domain.Category cat = (from Assignment asg in tk.Assignemts where asg.AssignedCategory != null orderby asg.CreatedOn select asg.AssignedCategory).FirstOrDefault();

            if (cat == null)
                cat = tk.CreationCategory;

            msg.ToCategory = cat;

            if (IsNew)
                tk.Messages.Add(msg);
            else
                Manager.SaveOrUpdate<Message>(msg);

            MessageId = msg.Id;

            //TOLTO:
            // Verrà visualizzato nella VIEW il messaggio "Creato da..." per TUTTI i messaggi in Behalf.
            // Il messaggio di sistema verrà utilizzato SOLO per indicare che TUTTO il Ticket è passato nello stato di BEHALF.

            //if (IsBehalf)
            //{
            //    Message msgBehalf = new Message();
            //    msgBehalf.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            //    //RIVEDERE!!!

            //    msgBehalf.Text = "Created by: " + msg.CreatedBy.SurnameAndName;
            //    msgBehalf.Preview = "";
            //    msgBehalf.Creator = this.CurrentUser;
            //    msgBehalf.DisplayName = "";
            //    msgBehalf.SendDate = DateTime.Now;
            //    msgBehalf.ShowRealName = true;
            //    msgBehalf.Ticket = tk;
            //    msgBehalf.Type = Domain.Enums.MessageType.System;
            //    msgBehalf.UserType = msg.UserType;
            //    msgBehalf.Visibility = true;

            //    msgBehalf.Action = Domain.Enums.MessageActionType.behalfMessage;
            //    msgBehalf.ToStatus = tk.Status;
            //    msgBehalf.ToUser = tk.Owner;
            //    msgBehalf.ToCategory = msg.ToCategory;
            //    msgBehalf.IsBehalf = true;
            //    //Manager.SaveOrUpdate<Message>(msgBehalf);
            //    tk.Messages.Add(msgBehalf);
            //}






            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (tk.Owner.Id == this.CurrentUser.Id)
            {
                tk.LastUserAccess = DateTime.Now;
            }
            else
            {
                tk.LastCreatorAccess = DateTime.Now;
            }

            
            Manager.SaveOrUpdate<Ticket>(tk);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
            }

            if(isTicketClosed) return TicketMessageSendError.TicketClosed;
            
            return Domain.Enums.TicketMessageSendError.none;
        }

        /// <summary>
        /// Aggiunge un messeggio DELL'UTENTE al Ticket indicato
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="Text">Testo messaggio</param>
        /// <param name="Preview">Eventuale anteprima</param>
        /// <param name="ChangeStatus">Indica se dopo l'invio il ticket dovrà modificare il suo stato (riapertura/chiusura)</param>
        /// <param name="NewStatus">Il nuovo stato (se va modificato)</param>
        /// <returns>Eventuali errori</returns>
        public Domain.Enums.TicketMessageSendError MessageSendUserExt(
            Int64 TicketId,
            String Text,
            String Preview,
            Domain.DTO.DTO_User User,
            ref Int64 MessageId,
            Int64 DraftId = -1,
            Boolean IsDraft = false)
        {
            bool ticketIsClose = false;

            if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(Preview))
                return Domain.Enums.TicketMessageSendError.NoMessage;

            Ticket tk = Manager.Get<Ticket>(TicketId);

            if (tk == null)
                return Domain.Enums.TicketMessageSendError.TicketNotFound;

            if (tk.IsDraft)
                return Domain.Enums.TicketMessageSendError.DraftTicket;

            if (tk.Owner != null && tk.Owner.Id != User.UserId)
                return Domain.Enums.TicketMessageSendError.NoPermission;

            bool IsNew = false;
            Message msg = (from Message m in tk.Messages where m.IsDraft == true && m.Creator != null && m.Creator.Id == User.UserId select m).FirstOrDefault();

            if (msg == null || msg.Id <= 0)
            {
                msg = new Message();
                IsNew = true;
            }
            else
            {
                //    TEORICAMENTE NON SERVE, MA FORSE MEGLIO METTERCELO!!!
                if (DraftId != msg.Id)
                {
                    //UPDATE FILE MESSAGE ID, FROM DraftId To msgId!!!
                }
            }

            if (IsNew)
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            else
                msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            msg.Text = Text;
            msg.Preview = Preview;
            msg.Creator = this.UserGet(User.UserId);
            msg.DisplayName = "";
            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Ticket = tk;
            msg.Type = Domain.Enums.MessageType.Request;
            msg.UserType = Domain.Enums.MessageUserType.Partecipant;
            msg.Visibility = true;

            if (tk.Status == TicketStatus.closeSolved || tk.Status == TicketStatus.closeUnsolved)
            {
                msg.IsDraft = true;
                ticketIsClose = true;
            }
            else
            {
                msg.IsDraft = IsDraft;
            }
            

            msg.Action = Domain.Enums.MessageActionType.normal;
            msg.ToStatus = tk.Status;

            Domain.TicketUser usr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msg.ToUser = usr;

            Domain.Category cat = (from Assignment asg in tk.Assignemts where asg.AssignedCategory != null orderby asg.CreatedOn select asg.AssignedCategory).FirstOrDefault();

            if (cat == null)
                cat = tk.CreationCategory;

            msg.ToCategory = cat;

            if (IsNew)
                tk.Messages.Add(msg);
            else
                Manager.SaveOrUpdate<Message>(msg);

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (tk.Owner.Id == this.CurrentUser.Id)
            {
                tk.LastUserAccess = DateTime.Now;   
            }   
            else
            {
                tk.LastCreatorAccess = DateTime.Now;
            }


            Manager.SaveOrUpdate<Ticket>(tk);

            if (ticketIsClose)
                return TicketMessageSendError.TicketClosed;

            return Domain.Enums.TicketMessageSendError.none;
        }

        /// <summary>
        /// Aggiunge un messeggio dal Manager/Resolver al Ticket indicato
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="Text">Testo messaggio</param>
        /// <param name="Preview">Eventuale anteprima</param>
        /// <param name="HideToUser">Se nascondere il messaggio all'utente</param>
        /// <param name="MessageType">Tipo di messaggio</param>
        /// <param name="UpdateAccess">SE aggiornare l'accesso dell'utente al Ticket</param>
        /// <param name="UserType">Tipo utente (per messaggio e controllo)</param>
        /// <returns>Eventuali errori</returns>
        public Domain.Enums.TicketMessageSendError MessageSendMan(
            Int64 TicketId,
            String Text,
            String Preview,
            Boolean HideToUser,
            Domain.Enums.MessageType MessageType,
            Domain.Enums.MessageUserType UserType,
            Boolean UpdateAccess,
            ref Int64 NewMessageId,
            Int64 DraftId = -1,
            Boolean IsDraft = false
            )
        {
            bool isTicketClosed = false;

            if (UserType == Domain.Enums.MessageUserType.none || UserType == Domain.Enums.MessageUserType.Partecipant
                || CurrentPerson.TypeID == (int)UserTypeStandard.Guest
                || CurrentPerson.TypeID == (int)UserTypeStandard.ExternalUser)
                return Domain.Enums.TicketMessageSendError.NoPermission;

            if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(Preview))
                return Domain.Enums.TicketMessageSendError.NoMessage;


            Domain.TicketUser Usr = this.UserGetfromPerson(UC.CurrentUserID);
            if (Usr == null || Usr.Id <= 0)
                return Domain.Enums.TicketMessageSendError.NoPermission;


            Ticket tk = Manager.Get<Ticket>(TicketId);

            if (tk == null)
                return Domain.Enums.TicketMessageSendError.TicketNotFound;

            if (tk.IsDraft)
                return Domain.Enums.TicketMessageSendError.DraftTicket;

            bool IsNew = false;
            Message msg = (from Message m in tk.Messages where m.IsDraft == true && m.Creator != null && m.Creator.Id == Usr.Id select m).FirstOrDefault();

            if (msg == null || msg.Id <= 0)
            {
                msg = new Message();
                IsNew = true;
            }
            else
            {
                //    TEORICAMENTE NON SERVE, MA FORSE MEGLIO METTERCELO!!!
                if (DraftId != msg.Id)
                {
                    //UPDATE FILE MESSAGE ID, FROM DraftId To msgId!!!
                }
            }

            if (IsNew)
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            else
                msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            msg.Text = Text;
            msg.Preview = Preview;
            msg.Creator = Usr;
            msg.DisplayName = "";
            msg.SendDate = DateTime.Now;
            msg.ShowRealName = true;
            msg.Ticket = tk;
            msg.Type = MessageType;
            msg.UserType = UserType;
            msg.Visibility = !HideToUser;

            if (tk.Status == TicketStatus.closeSolved || tk.Status == TicketStatus.closeUnsolved)
            {
                isTicketClosed = true;
                msg.IsDraft = true;
            }
            else
            {
                msg.IsDraft = IsDraft;    
            }

            msg.Action = Domain.Enums.MessageActionType.normal;
            msg.ToStatus = tk.Status;

            Domain.TicketUser usr = (from Assignment asg in tk.Assignemts where asg.AssignedTo != null orderby asg.CreatedOn select asg.AssignedTo).FirstOrDefault();

            msg.ToUser = usr;

            Domain.Category cat = (from Assignment asg in tk.Assignemts where asg.AssignedCategory != null orderby asg.CreatedOn select asg.AssignedCategory).FirstOrDefault();

            if (cat == null)
                cat = tk.CreationCategory;

            msg.ToCategory = cat;

            if (IsNew)
                tk.Messages.Add(msg);
            //else
            //    Manager.SaveOrUpdate<Message>(msg);

            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            Manager.SaveOrUpdate<Ticket>(tk);

            if (UpdateAccess)
                this.UserAccessUpdate(tk.Id);

            NewMessageId = msg.Id;

            if (isTicketClosed)
                return TicketMessageSendError.TicketClosed;

            return Domain.Enums.TicketMessageSendError.none;
        }

        /// <summary>
        /// Modifica visibilità messaggio
        /// </summary>
        /// <param name="TicketId">Id Ticket</param>
        /// <param name="MessageId">Id Messaggio</param>
        /// <param name="Isvisibile">Visibilità</param>
        /// <param name="UserType">Tipo utente</param>
        /// <returns>TRUE se la modifica è avvenuta</returns>
        public Boolean MessageChangeVisibility(
            Int64 TicketId,
            Int64 MessageId,
            Boolean Isvisibile,
            Domain.Enums.MessageUserType UserType)
        {
            if (UserType == Domain.Enums.MessageUserType.none || UserType == Domain.Enums.MessageUserType.Partecipant
                || CurrentPerson.TypeID == (int)UserTypeStandard.Guest
                || CurrentPerson.TypeID == (int)UserTypeStandard.ExternalUser)
                return false;

            Message msg = Manager.Get<Message>(MessageId);

            if (msg.UserType == Domain.Enums.MessageUserType.Partecipant)
                return false;

            if (msg.Ticket == null || msg.Ticket.Id != TicketId || msg.Visibility == Isvisibile)
                return false;

            msg.Visibility = Isvisibile;
            msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (msg.Attachments != null && msg.Attachments.Any())
            {
                foreach(TicketFile file in msg.Attachments)
                { 
                    if(Isvisibile && file.Visibility == Domain.Enums.FileVisibility.hiddenMessage)
                    {
                        file.Visibility = Domain.Enums.FileVisibility.visible;
                    } else if(!Isvisibile && file.Visibility == Domain.Enums.FileVisibility.visible)
                    {
                        file.Visibility = Domain.Enums.FileVisibility.hiddenMessage;
                    }
                        
                }
            }
            

            Manager.SaveOrUpdate<Message>(msg);

            this.UserAccessUpdate(TicketId);

            return true;
        }

        public Message MessageGet(Int64 Id)
        {
            return Manager.Get<Message>(Id);
        }

        public Message MessageGetFromTicketDraft(Int64 TicketId, Int64 UserId)
        {
            if(UserId <= 0)
            {
                UserId = this.CurrentUser.Id;
            }

            Ticket tk = Manager.Get<Ticket>(TicketId);

            if(tk == null || tk.Id < 0 || tk.Owner == null || tk.Owner.Id != UserId || tk.IsDraft == false || tk.Status != Domain.Enums.TicketStatus.draft)
                return null;

            return (from Message msg in tk.Messages

                        where msg.Creator != null 
                        && msg.Creator.Id == UserId
                        orderby msg.Id
                        select msg).FirstOrDefault();
        }

        public bool MessageDraftUpdate(
           String HtmlMessage,
           String Preview,
           Int64 TicketId,
           Int64 MessageId,
            Int64 UserId)
        {
            return MessageDraftUpdate(
                HtmlMessage,
                Preview,
                TicketId,
                MessageId,
                Manager.Get<Domain.TicketUser>(UserId));
        }
        
        public bool MessageDraftUpdate(
           String HtmlMessage,
           String Preview,
           Int64 TicketId,
           Int64 MessageId)
        { 
            return MessageDraftUpdate(
            HtmlMessage,
            Preview,
            TicketId,
            MessageId,
            this.CurrentUser);
        }

        private bool MessageDraftUpdate(
            String HtmlMessage,
            String Preview,
            Int64 TicketId,
            Int64 MessageId,
            TicketUser usr)
        {
            //if(!Manager.IsInTransaction())
            //{
            //    Manager.BeginTransaction();
            //}

            Message msg = Manager.Get<Message>(MessageId);
            

            if (
                usr == null ||
                msg == null ||
                msg.Ticket == null ||
                msg.Ticket.Id != TicketId ||
                msg.Creator == null ||
                msg.Creator.Id != usr.Id ||
                !msg.IsDraft ||
                msg.Deleted != BaseStatusDeleted.None)
                return false;

            msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            msg.Text = HtmlMessage;
            msg.Preview = Preview;

            Manager.SaveOrUpdate<Message>(msg);

            //Manager.Flush();

            return true;
        }



        //private bool MessageDraftUpdate(
        //    String HtmlMessage,
        //    String Preview,
        //    Int64 TicketId,
        //    Int64 MessageId,
        //    Int64 UserId)
        //{
        //    Message msg = Manager.Get<Message>(MessageId);
        //    TicketUser usr = Manager.Get<TicketUser>(UserId);

        //    if (
        //        usr != null ||
        //        msg == null ||
        //        msg.Ticket == null ||
        //        msg.Ticket.Id != TicketId ||
        //        msg.Creator == null ||
        //        msg.Creator.Id != usr.Id ||
        //        !msg.IsDraft ||
        //        msg.Deleted != BaseStatusDeleted.None)
        //        return false;

        //    msg.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //    msg.Text = HtmlMessage;
        //    msg.Preview = Preview;

        //    Manager.SaveOrUpdate<Message>(msg);



        //    return true;
        //}
        /// <summary>
        /// Dato un Ticket e l'utente recupera il relativo messaggio DRAFT.
        /// Nel caso non esista, viene creato.
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private Message MessageDraftGet(
            Ticket Ticket, TicketUser User, 
            Domain.Enums.MessageUserType UserType = Domain.Enums.MessageUserType.Partecipant,
            Domain.Enums.MessageType MessageType = Domain.Enums.MessageType.FeedBack
            )
        {
            //if(!Manager.IsInTransaction())
            //{
            //    Manager.BeginTransaction();
            //}
            

            Message msg = Manager.GetAll<Message>(m =>
                m.IsDraft == true && m.Deleted == BaseStatusDeleted.None &&
                m.Creator != null && m.Creator.Id == User.Id &&
                m.Ticket != null && m.Ticket.Id == Ticket.Id).FirstOrDefault();
            
            if(msg == null)
            {
                //if (!Manager.IsInTransaction())
                //    Manager.BeginTransaction();

                msg = new Message();
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                msg.Creator = User;
                msg.Action = Domain.Enums.MessageActionType.normal;
                msg.Attachments = new List<TicketFile>();
                msg.DisplayName = User.DisplayName;
                msg.IsDraft = true;
                msg.Preview = "";
                msg.ShowRealName = true;
                msg.Type = MessageType;
                msg.UserType = UserType;
                msg.Visibility = true;
                msg.Ticket = Ticket;
                msg.ToStatus = Ticket.Status;
                msg.ToCondition = Ticket.Condition;

                msg.ToCategory = (from Assignment ass in Ticket.Assignemts
                    where ass.AssignedCategory != null
                    orderby ass.CreatedOn
                    select ass.AssignedCategory).LastOrDefault()
                    ?? Ticket.CreationCategory;


                Manager.SaveOrUpdate<Message>(msg);
            }
            else
            {
                Manager.Refresh<Message>(msg);
            }

            //Manager.Flush();

            return msg;
        }

        private Message MessageDraftGet(
    Ticket Ticket, Int64 UserId,
    Domain.Enums.MessageUserType UserType = Domain.Enums.MessageUserType.Partecipant,
    Domain.Enums.MessageType MessageType = Domain.Enums.MessageType.FeedBack
    )
        {
            TicketUser User = Manager.Get<TicketUser>(UserId);

            Message msg = Manager.GetAll<Message>(m =>
                m.IsDraft == true && m.Deleted == BaseStatusDeleted.None &&
                m.Creator != null && m.Creator.Id == User.Id &&
                m.Ticket != null && m.Ticket.Id == Ticket.Id).FirstOrDefault();

            if (msg == null)
            {
                //if (!Manager.IsInTransaction())
                //    Manager.BeginTransaction();

                msg = new Message();
                msg.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                msg.Creator = User;
                msg.Action = Domain.Enums.MessageActionType.normal;
                msg.Attachments = new List<TicketFile>();
                msg.DisplayName = User.DisplayName;
                msg.IsDraft = true;
                msg.Preview = "";
                msg.ShowRealName = true;
                msg.Type = MessageType;
                msg.UserType = UserType;
                msg.Visibility = true;
                msg.Ticket = Ticket;

                msg.ToStatus = Ticket.Status;
                msg.ToCondition = Ticket.Condition;

                msg.ToCategory = (from Assignment ass in Ticket.Assignemts
                                  where ass.AssignedCategory != null
                                  orderby ass.CreatedOn
                                  select ass.AssignedCategory).LastOrDefault()
                    ?? Ticket.CreationCategory;

                Manager.SaveOrUpdate<Message>(msg);
            }

            return msg;
        }

        /// <summary>
        /// True: messaggio inesistente o NON in DRAFT = non inviabile!
        /// </summary>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public bool MessageCheckDraft(Int64 MsgId)
        {
            liteMessage chkmsg = Manager.Get<liteMessage>(MsgId);
            if (chkmsg == null || !chkmsg.IsDraft)
                return true;

            return false;
        }
    }
}
