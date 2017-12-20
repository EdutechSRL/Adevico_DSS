using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using NHibernate.Mapping;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.BaseModules.Tickets.Domain.DTO;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        #region Repository permissions

            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository _ServiceRepository;
            private lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository ServiceRepository
            {
                get
                {
                    if (_ServiceRepository == null)
                        _ServiceRepository = new lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(_Context);
                    return _ServiceRepository;
                }
            }
            public lm.Comol.Core.FileRepository.Domain.ModuleRepository GetRepositoryPermissions(Int32 idCommunity)
            {
                return GetRepositoryPermissions(idCommunity, UC.CurrentUserID);
            }

            public lm.Comol.Core.FileRepository.Domain.ModuleRepository GetRepositoryPermissions(Int32 idCommunity, Int32 idPerson)
            {
                lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier = lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create((idCommunity > 0 ? lm.Comol.Core.FileRepository.Domain.RepositoryType.Community : lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal), idCommunity);
                return ServiceRepository.GetPermissions(identifier, idPerson);
            }

        #endregion

        #region New methods.......... .. ..
            public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles(long idMessage, Boolean loadDeleted = false)
            {
                List<RepositoryItemLinkBase<long>> items = new List<RepositoryItemLinkBase<long>>();
                if (idMessage > 0)
                {
                    try
                    {
                        List<TicketFile> attachments = QueryAttachments(i => i.Deleted == BaseStatusDeleted.None && i.Message != null && i.Message.Id == idMessage).ToList();

                        attachments.ForEach(a => items.Add(new RepositoryItemLinkBase<long>()
                        {
                            Deleted = a.Deleted,
                            File = new RepositoryItemObject(a.Item),
                            IdObjectLink = a.Id,
                            AlwaysLastVersion = true,
                            IsVisible = (a.Deleted== BaseStatusDeleted.None && a.Item !=null && a.Item.IsVisible) // (a.Visibility == Domain.Enums.FileVisibility.visible)
                        }));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return items;
            }

            public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles(IList<DTO_AttachmentItem> attachments, Boolean loadDeleted = false)
            {
                List<RepositoryItemLinkBase<long>> items = new List<RepositoryItemLinkBase<long>>();
                if (attachments != null && attachments.Any())
                {
                    try
                    {
                        attachments.ToList().ForEach(a => items.Add(new RepositoryItemLinkBase<long>()
                        {
                            Deleted = a.Deleted,
                            File = new RepositoryItemObject(a.Item),
                            IdObjectLink = a.IdAttachment,
                            AlwaysLastVersion = true,
                            IsVisible = (a.Deleted == BaseStatusDeleted.None && a.Item != null && a.Item.IsVisible) // (a.Visibility == Domain.Enums.FileVisibility.visible)
                        }));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return items;
            }
            public List<RepositoryItemLinkBase<long>> AttachmentsGetBaseLinkedFiles(IList<TicketFile> attachments, Boolean loadDeleted = false)
            {
                List<RepositoryItemLinkBase<long>> items = new List<RepositoryItemLinkBase<long>>();
                if (attachments != null && attachments.Any())
                {
                    try
                    {
                        attachments.ToList().ForEach(a => items.Add(new RepositoryItemLinkBase<long>()
                        {
                            Deleted = a.Deleted,
                            File = new RepositoryItemObject(a.Item),
                            IdObjectLink = a.Id,
                            AlwaysLastVersion = true,
                            IsVisible = (a.Deleted == BaseStatusDeleted.None && a.Item != null && a.Item.IsVisible) // (a.Visibility == Domain.Enums.FileVisibility.visible)
                        }));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return items;
            }
            private IQueryable<TicketFile> QueryAttachments(Expression<Func<TicketFile, bool>> conditions)
            {
                return (from i in Manager.GetIQ<TicketFile>() select i).Where(conditions);
            }

            public List<TicketFile> AttachmentsAddFiles(long idMessage, List<dtoModuleUploadedItem> items)
            {
                return AttachmentsAddFiles(Manager.Get<Message>(idMessage), UserGetfromPerson(UC.CurrentUserID), items);
            }
            public List<TicketFile> AttachmentsAddFiles(long idMessage, long idTicketUser, List<dtoModuleUploadedItem> items)
            {
                return AttachmentsAddFiles(Manager.Get<Message>(idMessage), Manager.Get<TicketUser>(idTicketUser), items);
            }
            public List<TicketFile> AttachmentsAddFiles(Message message, List<dtoModuleUploadedItem> items)
            {
                return AttachmentsAddFiles(message, UserGetfromPerson(UC.CurrentUserID), items);
            }
            public List<TicketFile> AttachmentsAddFiles(Message message, long idTicketUser, List<dtoModuleUploadedItem> items)
            {
                return AttachmentsAddFiles(message, Manager.Get<TicketUser>(idTicketUser), items);
            }
            public List<TicketFile> AttachmentsAddFiles(Message message, TicketUser user, List<dtoModuleUploadedItem> items)
            {
                List<TicketFile> attachments = null;
                if (message == null || message.Ticket == null || user == null || items== null || !items.Any())
                    return null;
                if (message.Creator == null || message.Creator.Id != user.Id)
                    return null;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    litePerson person = (user.Person ?? Manager.GetLiteUnknownUser());
                    if (items.Any() && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        attachments = new List<TicketFile>();
                        DateTime date = DateTime.Now;
                        foreach (dtoModuleUploadedItem item in items)
                        {
                            TicketFile attachment = new TicketFile();
                            attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                            attachment.Item = item.ItemAdded;
                            attachment.Message = message;
                            attachment.TicketId = ((message != null && message.Ticket != null) ? message.Ticket.Id : 0);
                            attachment.Name = attachment.Item.DisplayName;
                            attachment.Visibility = Domain.Enums.FileVisibility.visible;
                            Manager.SaveOrUpdate(attachment);
                            liteModuleLink link = new liteModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                            link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                            link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                            link.AutoEvaluable = false;
                            link.SourceItem = ModuleObject.CreateLongObject(message.Id, message, (int)ModuleTicket.ObjectType.Message, 0, ModuleTicket.UniqueCode, ServiceModuleID());
                            Manager.SaveOrUpdate(link);
                            attachment.Link = link;

                            if (item.ItemAdded.IsInternal)
                            {
                                if (item.ItemAdded.Module == null)
                                    item.ItemAdded.Module = new lm.Comol.Core.FileRepository.Domain.ItemModuleSettings();
                                item.ItemAdded.Module.IdObject = message.Id;
                                item.ItemAdded.Module.IdObjectType = (int)ModuleTicket.ObjectType.Message;
                                Manager.SaveOrUpdate(item.ItemAdded);
                            }

                            Manager.SaveOrUpdate(attachment);
                            attachments.Add(attachment);
                        }
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }
            public List<TicketFile> AttachmentsLinkFiles(long idMessage, List<ModuleActionLink> links)
            {
                return AttachmentsLinkFiles(Manager.Get<Message>(idMessage), links);
            }
            public List<TicketFile> AttachmentsLinkFiles(Message message, List<ModuleActionLink> links)
            {
                List<TicketFile> attachments = null;

                if (message == null || links == null || !links.Any())
                    return null;

                TicketUser usr = this.UserGetfromPerson(UC.CurrentUserID);
                if (message.Creator == null || message.Creator.Id != usr.Id)
                    return null;

                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (!isInTransaction)
                        Manager.BeginTransaction();
                    litePerson person = CurrentLitePerson;

                    if (person != null && person.Id>0)
                    {
                        DateTime date = DateTime.Now;
                        attachments = new List<TicketFile>();
                        foreach (ModuleActionLink link in links)
                        {
                            TicketFile attachment = QueryAttachments(a => a.Deleted == BaseStatusDeleted.None && a.Message!= null && a.Message.Id == message.Id ).ToList().Where(a => a.Item == (liteRepositoryItem)link.ModuleObject.ObjectOwner).Skip(0).Take(1).ToList().FirstOrDefault();
                            if (attachment == null)
                            {
                                attachment = new TicketFile();
                                attachment.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                                attachment.Item = (liteRepositoryItem)link.ModuleObject.ObjectOwner;
                                attachment.Message = message;
                                attachment.TicketId = ((message != null && message.Ticket != null) ? message.Ticket.Id : 0);
                                attachment.Name = attachment.Item.DisplayName;
                                attachment.Visibility = Domain.Enums.FileVisibility.visible;

                                Manager.SaveOrUpdate(attachment);
                                

                                liteModuleLink mLink = new liteModuleLink(link.Description, link.Permission, link.Action);
                                mLink.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                mLink.DestinationItem = (ModuleObject)link.ModuleObject;
                                mLink.AutoEvaluable = false;
                                mLink.SourceItem = ModuleObject.CreateLongObject(message.Id, message, (int)ModuleTicket.ObjectType.Message,0, ModuleTicket.UniqueCode, ServiceModuleID());
                                Manager.SaveOrUpdate(mLink);
                                attachment.Link = mLink;
                                Manager.SaveOrUpdate(attachment);
                            }
                            else if (attachment.Deleted != BaseStatusDeleted.None)
                            {
                                attachment.RecoverMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress, date);
                                if (attachment.Item != null && attachment.Item.IsInternal && attachment.Item.Deleted != BaseStatusDeleted.None)
                                {
                                    attachment.Item.Deleted = BaseStatusDeleted.None;
                                    attachment.Item.UpdateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                                    Manager.SaveOrUpdate(attachment.Item);
                                }
                                Manager.SaveOrUpdate(attachment);
                            }
                            attachments.Add(attachment);
                        }
                    }
                    if (!isInTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                    attachments = null;
                }
                return attachments;

            }

            #region Manage attachments
                public Boolean AttachmentEditVisibility(Int64 idAttachment, Boolean hide)
                {
                    Domain.TicketFile attachment = Manager.Get<Domain.TicketFile>(idAttachment);
                    if (attachment == null)
                        return false;
                    if (attachment.Visibility == Domain.Enums.FileVisibility.hiddenMessage)
                        return false;

                    attachment.Visibility = (hide) ?
                        Domain.Enums.FileVisibility.hidden :
                        Domain.Enums.FileVisibility.visible;

                    attachment.UpdateMetaInfo(CurrentLitePerson, UC.IpAddress, UC.ProxyIpAddress);

                    Manager.SaveOrUpdate(attachment);
                    return true;
                }

                public Boolean AttachmentDelete(Int64 idAttachment, String baseFilePath, String baseThumbnailPath)
                {
                    return (idAttachment <= 0 ? false : AttachmentDelete(idAttachment, UserGetfromPerson(UC.CurrentUserID).Id, baseFilePath, baseThumbnailPath));
                }
                public Boolean AttachmentDelete(Int64 idAttachment, Int64 TkUserId, String baseFilePath, String baseThumbnailPath)
                {
                    Boolean deleted = true;

                    if (idAttachment < 0)
                        return false;
                    try
                    {
                        Manager.BeginTransaction();
                        Domain.TicketFile attachment = Manager.Get<Domain.TicketFile>(idAttachment);
                        if (attachment == null)
                            return true;
                        if (attachment.Message == null || attachment.Message.Ticket == null
                            || !(attachment.Message.IsDraft || attachment.Message.Ticket.IsDraft)
                            || attachment.Message.Creator == null
                            || attachment.Message.Creator.Id != TkUserId)
                            return false;

                        List<String> filesToRemove = new List<string>();

                        if (attachment.Item != null && attachment.Item.IsInternal)
                        {
                            filesToRemove.Add(ServiceRepository.GetItemDiskFullPath(baseFilePath, attachment.Item));
                            filesToRemove.Add(ServiceRepository.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                            Manager.DeletePhysical(attachment.Item);
                            if (attachment.Version != null)
                                Manager.DeletePhysical(attachment.Version);

                            if (attachment.Link != null)
                                Manager.DeletePhysical(attachment.Link);
                        }
                        else if (attachment.Link != null)
                            Manager.DeletePhysical(attachment.Link);

                        Manager.DeletePhysical(attachment);
                        Manager.Commit();

                        if (filesToRemove.Any(f=> !String.IsNullOrEmpty(f)))
                            lm.Comol.Core.File.Delete.Files(filesToRemove.Where(f => !String.IsNullOrEmpty(f)).ToList());
                    }
                    catch (Exception ex)
                    {
                        deleted = false;
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                    return deleted;
                }

                private Domain.Enums.TicketDraftDeleteError AttachmentDeleteAll( Int64 TkDraftId, Int64 TkUserID, String baseFilePath, String baseThumbnailPath)
                {
                    if (TkDraftId < 0)
                        return Domain.Enums.TicketDraftDeleteError.TicketNotFound;

                    liteDraftTicket drftTk = Manager.Get<liteDraftTicket>(TkDraftId);
                    if (drftTk == null)
                        return Domain.Enums.TicketDraftDeleteError.TicketNotFound;
                    if (!drftTk.IsDraft)
                        return Domain.Enums.TicketDraftDeleteError.TicketNotInDraft;
                    if (drftTk.CreatorId != TkUserID)
                        return Domain.Enums.TicketDraftDeleteError.TicketNotMine;


                    IList<TicketFile> attachments = Manager.GetAll<TicketFile>(tf => tf.TicketId == TkDraftId);

                    if (attachments == null || attachments.Count <= 0)
                        return Domain.Enums.TicketDraftDeleteError.none;

                    try
                    {
                        //Manager.BeginTransaction();
                        foreach (TicketFile attachment in attachments)
                        {
                            if (!(attachment == null
                                || attachment.Id <= 0
                                || attachment.Message == null || attachment.Message.Ticket == null
                                || !(attachment.Message.IsDraft || attachment.Message.Ticket.IsDraft)
                                || attachment.Message.Creator == null
                                || attachment.Message.Creator.Id != TkUserID))
                            {

                                List<String> filesToRemove = new List<string>();
                                if (attachment.Item != null && attachment.Item.IsInternal)
                                {
                                    filesToRemove.Add(ServiceRepository.GetItemDiskFullPath(baseFilePath, attachment.Item));
                                    filesToRemove.Add(ServiceRepository.GetItemThumbnailFullPath(baseThumbnailPath, attachment.Item));
                                    Manager.DeletePhysical(attachment.Item);
                                    if (attachment.Version != null)
                                        Manager.DeletePhysical(attachment.Version);

                                    if (attachment.Link != null)
                                        Manager.DeletePhysical(attachment.Link);
                                }
                                else if (attachment.Link != null)
                                    Manager.DeletePhysical(attachment.Link);

                                Manager.DeletePhysical(attachment);
                                Manager.Commit();

                                if (filesToRemove.Any(f => !String.IsNullOrEmpty(f)))
                                    lm.Comol.Core.File.Delete.Files(filesToRemove.Where(f => !String.IsNullOrEmpty(f)).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //deleted = false;
                        //if (Manager.IsInTransaction())
                        //    Manager.RollBack();

                        return Domain.Enums.TicketDraftDeleteError.FileError;
                    }

                    //if (Manager.IsInTransaction())
                    //    Manager.Commit();
                    //Manager.Commit();

                    //IList<Int64> Deleted = from Manager.GetIQ<Attribute
                    return Domain.Enums.TicketDraftDeleteError.none;
                }
            #endregion
        #endregion

        
        ///// <summary>
        ///// Solo il creatore può allegare file ad un messaggio.
        ///// Qualunque UTENTE può allegare file ad un messaggio.
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //private List<Domain.TicketFile> AttachmentsAddFiles(Message msg, MultipleUploadResult<dtoModuleUploadedFile> items, TicketUser usr)
        //{
        //    if (msg == null || msg.Ticket == null || usr == null)
        //        return null;

        //    List<Domain.TicketFile> Files = new List<Domain.TicketFile>();

        //    //Person person = Manager.GetPerson();

        //    //TicketUser usr = 
        //    if (msg.Creator == null || msg.Creator.Id != usr.Id)
        //        return null;

        //    //Boolean isInTransaction = Manager.IsInTransaction();
            
        //    try
        //    {
        //        if (Manager.IsInTransaction())
        //            Manager.Commit();

        //        Manager.BeginTransaction();
                
        //        //&& person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
        //        if (items.UploadedFile.Any())
        //        {
                    

        //            foreach (dtoModuleUploadedFile item in items.UploadedFile)
        //            {
        //                Domain.TicketFile file = new Domain.TicketFile();
        //                file.Message = msg;
        //                file.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //                file.File = item.File;
        //                file.Visibility = Domain.Enums.FileVisibility.visible;
        //                file.TicketId = msg.Ticket.Id;
        //                file.Name = item.File.DisplayName;
        //                msg.Attachments.Add(file);
                        
        //                ModuleLink link = new ModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);

        //                link.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //                link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
        //                link.AutoEvaluable = false;

        //                link.SourceItem = ModuleObject.CreateLongObject(msg.Id, 
        //                    msg, 
        //                    (int) Tickets.ModuleTicket.ObjectType.Message,
        //                    0,
        //                    Tickets.ModuleTicket.UniqueCode,
        //                    ServiceModuleID());

        //                Manager.SaveOrUpdate<ModuleLink>(link);

        //                file.Link = link;

        //                Manager.SaveOrUpdate<Domain.TicketFile>(file);
        //                Manager.SaveOrUpdate(msg);

        //                Files.Add(file);
        //            }

        //        }
        //        if (Manager.IsInTransaction())
        //            Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Manager.IsInTransaction())
        //            Manager.RollBack();
        //        Files = null;
        //    }

        //    return Files;

        //}

        //public List<Domain.TicketFile> AttachmentsAddFilesToCommunity(Message msg, MultipleUploadResult<dtoUploadedFile> items)
        //{
        //    if (msg == null || msg.Ticket == null)
        //        return null;

        //    List<Domain.TicketFile> Files = new List<Domain.TicketFile>();

        //    //Person person = Manager.GetPerson();

        //    TicketUser usr = this.UserGetfromPerson(UC.CurrentUserID);
        //    if (usr == null)// || msg.Creator.Id != usr.Id)
        //        return null;

        //    //Boolean isInTransaction = Manager.IsInTransaction();

        //    try
        //    {
        //        //if (!isInTransaction)
        //        //    Manager.BeginTransaction();

        //        //&& person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser)
        //        if (items.UploadedFile.Any())
        //        {


        //            foreach (dtoUploadedFile item in items.UploadedFile)
        //            {
        //                Domain.TicketFile file = new Domain.TicketFile();
        //                file.Message = msg;
        //                file.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //                file.File = item.File;
        //                file.Visibility = Domain.Enums.FileVisibility.visible;
        //                file.TicketId = msg.Ticket.Id;
        //                file.Name = item.File.DisplayName;

        //                ModuleLink link = new ModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);

        //                link.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
        //                link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
        //                link.AutoEvaluable = false;

        //                link.SourceItem = ModuleObject.CreateLongObject(msg.Id,
        //                    msg,
        //                    (int)Tickets.ModuleTicket.ObjectType.Message,
        //                    0,
        //                    Tickets.ModuleTicket.UniqueCode,
        //                    ServiceModuleID());

        //                Manager.SaveOrUpdate<ModuleLink>(link);

        //                file.Link = link;

        //                Manager.SaveOrUpdate<Domain.TicketFile>(file);

        //                Files.Add(file);
        //            }

        //        }
        //        //if (isInTransaction)
        //        //    Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        //if (isInTransaction)
        //        //    Manager.RollBack();
        //        Files = null;
        //    }

        //    return Files;

        //}
      
        //public Domain.Enums.FileDeleteResponse DeleteAttachment(Int64 TkFileId, Int64 TkUserId, ref String ToRemove)
        //{
        //    ToRemove = "";

        //    if(TkFileId < 0)
        //        return Domain.Enums.FileDeleteResponse.NotFound;

        //    Domain.TicketFile Attachment = Manager.Get<Domain.TicketFile>(TkFileId);
        //    if (Attachment == null || Attachment.Message == null)
        //        return Domain.Enums.FileDeleteResponse.NotFound;

        //    if (!Attachment.Message.IsDraft)
        //        return Domain.Enums.FileDeleteResponse.NotDraft;

        //    if (Attachment.Message.Creator == null || Attachment.Message.Creator.Id != TkUserId)
        //        return Domain.Enums.FileDeleteResponse.NoPermission;

        //    if (Attachment.File.GetType() == typeof(ModuleLongInternalFile))
        //    {
        //        ToRemove = Attachment.File.UniqueID.ToString() + ".stored";
        //        Manager.DeletePhysical(Attachment.Link);
        //        Manager.DeletePhysical(Attachment.File);
        //    }
        //    else
        //    {
        //        Manager.DeletePhysical(Attachment.Link);
        //    }

        //    Manager.DeletePhysical(Attachment);

        //    return Domain.Enums.FileDeleteResponse.deleted;
        //}
    }
}