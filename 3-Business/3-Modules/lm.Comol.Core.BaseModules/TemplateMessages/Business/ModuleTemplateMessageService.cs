using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using NHibernate.Linq;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Business
{
    public class ModuleTemplateMessageService : lm.Comol.Core.Mail.Messages.MailMessagesService  
    {
        #region initClass
            public ModuleTemplateMessageService() :base() { }
            public ModuleTemplateMessageService(iApplicationContext oContext) :base(oContext) {
            }
            public ModuleTemplateMessageService(iDataContext oDC)
                : base(oDC)
            {

            }
        #endregion

            public List<dtoGenericModuleMessageRecipient> GetAvailableRecipientsForObject(String unknownUser, String anonymousUser, ModuleObject obj, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, Boolean loadAllInfo = true)
            {
                List<dtoGenericModuleMessageRecipient> items = new List<dtoGenericModuleMessageRecipient>();
                try
                {
                    List<MailRecipient> mRecipients = GetRecipientsQuery(filter.IdCommunity, false, obj).ToList();
                    if (mRecipients.Any()) {
                        List<dtoModuleRecipientMessages> pRecipients = GetParsedRecipients(obj,mRecipients,true);
                        items = pRecipients.Select(r => new dtoGenericModuleMessageRecipient(r)).ToList();
                        List<Int32> idUsers = items.Where(r => r.IsInternal).Select(r => r.IdPerson).Distinct().ToList();

                        if (idUsers.Any())
                        {
                            List<Person> users = new List<Person>();
                            if (idUsers.Count <= maxItemsForQuery)
                                users.AddRange((from p in Manager.GetIQ<Person>() where idUsers.Contains(p.Id) select p).ToList());
                            else
                            {
                                Int32 pageIndex = 0;
                                List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                while (idPagedUsers.Any())
                                {
                                    users.AddRange((from p in Manager.GetIQ<Person>() where idPagedUsers.Contains(p.Id) select p).ToList());
                                    pageIndex++;
                                    idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                                }
                            }
                            items.Where(r => r.IsInternal).ToList().ForEach(i => i.UpdatePersonInfo(users.Where(u => u.Id == i.IdPerson).FirstOrDefault(), unknownUser));
                        }
                        if (filter.IdCommunity > 0 && items.Any()) { 
                            idUsers = items.Select(i=>i.IdPerson).ToList();
                            List<LazySubscription> subscriptions = (idUsers.Count <= maxItemsForQuery) ? (from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity== filter.IdCommunity && idUsers.Contains(s.IdPerson) select s).ToList() : (from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity== filter.IdCommunity   select s).ToList().Where(s=>idUsers.Contains(s.IdPerson)).ToList();
                            items.Where(i=>i.IdPerson>0).ToList().ForEach(i => i.IdRole = subscriptions.Where(s => s.IdPerson == i.IdPerson).Select(s => s.IdRole).FirstOrDefault());
                        }
                        var query = (from r in items
                                     where (filter.IdProfileType < 1 || (filter.IdProfileType > 0 && r.IdProfileType == filter.IdProfileType))
                                         && (filter.IdRole < 1 || (filter.IdRole > 0 && r.IdRole == filter.IdRole))
                                     select r);

                        
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
                            if (loadAllInfo || filter.OrderBy == MailSender.UserByMessagesOrder.ByAgency)
                            {
                                Dictionary<long, String> agencyName = pService.GetAgenciesName(agencyInfos.Keys.ToList());
                                foreach (var i in agencyInfos)
                                {
                                    query.Where(r => r.IsInternal && i.Value.Contains(r.IdPerson)).ToList().ForEach(r => r.UpdateAgencyInfo(i.Key, (agencyName.ContainsKey(i.Key) ? agencyName[i.Key] : "")));
                                }
                            }
                        }
                        items = query.ToList();
                        items.Where(i => String.IsNullOrEmpty(i.DisplayName) && !String.IsNullOrEmpty(i.Name) && !String.IsNullOrEmpty(i.Surname)).ToList().ForEach(r => r.DisplayName = r.Surname + " " + r.Name);
                        items.Where(i => String.IsNullOrEmpty(i.DisplayName) && String.IsNullOrEmpty(i.Name) && String.IsNullOrEmpty(i.Surname)).ToList().ForEach(r => r.DisplayName = r.MailAddress);
                    }
                }
                catch (Exception ex)
                {

                }
                return items;
            }
        public List<dtoGenericModuleMessageRecipient> GetParsedUsersForMessages(List<dtoGenericModuleMessageRecipient> recipients, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
        {
            var query = (from r in recipients select r);
            if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
            {
                switch (filter.SearchBy)
                {
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                        List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                        if (values.Any() && values.Count == 1)
                            query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                        else if (values.Any() && values.Count > 1)
                            // values.Any(r.Name.ToLower().Contains) && values.Any(r.Surname.ToLower().Contains) || 
                            query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains)) || values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                        query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                        query = query.Where(r => !String.IsNullOrEmpty(r.Name) && r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                        query = query.Where(r => !String.IsNullOrEmpty(r.Name) && r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
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
            switch (filter.OrderBy)
            {
                case MailSender.UserByMessagesOrder.ByMessageNumber:
                    if (filter.Ascending)
                        query = query.OrderBy(r => r.MessageNumber);
                    else
                        query = query.OrderByDescending(r => r.MessageNumber);
                    break;
                case MailSender.UserByMessagesOrder.ByProfileType:
                    if (filter.Ascending)
                        query = query.OrderBy(r => filter.ProfyleTypeTranslations[r.IdProfileType]);
                    else
                        query = query.OrderByDescending(r => filter.ProfyleTypeTranslations[r.IdProfileType]);
                    break;
                case MailSender.UserByMessagesOrder.ByRole:
                    if (filter.Ascending)
                        query = query.OrderBy(r => filter.RoleTranslations[r.IdRole]);
                    else
                        query = query.OrderByDescending(r => filter.RoleTranslations[r.IdRole]);
                    break;
                case MailSender.UserByMessagesOrder.ByInternal:
                    if (filter.Ascending)
                        query = query.OrderByDescending(r => r.IsInternal).ThenBy(r => r.DisplayName);
                    else
                        query = query.OrderBy(r => r.IsInternal).ThenBy(r => r.DisplayName);
                    break;
                case MailSender.UserByMessagesOrder.ByUser:
                    if (filter.Ascending)
                        query = query.OrderBy(r => r.DisplayName);
                    else
                        query = query.OrderByDescending(r => r.DisplayName);
                    break;
                case MailSender.UserByMessagesOrder.ByAgency:
                    if (filter.Ascending)
                        query = query.OrderBy(r => r.AgencyName);
                    else
                        query = query.OrderByDescending(r => r.AgencyName);
                    break;
            }

            return query.ToList();
        }
        public List<dtoFilteredDisplayMessage> GetObjectMessages(Int32 idUser,ModuleObject obj, Int32 idCommunity = -1, Boolean isPortal = false, Int32 idModule = 0, String moduleCode = "")
        {
            dtoOwnership ownership = new dtoOwnership();
            ownership.ModuleObject = obj;
            if (obj != null && obj.ServiceID > 0 && String.IsNullOrEmpty(obj.ServiceCode))
                ownership.ModuleObject.ServiceCode = Manager.GetModuleCode(obj.ServiceID);
            else if (obj != null && obj.ServiceID <1 && !String.IsNullOrEmpty(obj.ServiceCode))
                ownership.ModuleObject.ServiceID = Manager.GetModuleID(obj.ServiceCode);
            ownership.IdCommunity = idCommunity;
            ownership.IsPortal = isPortal || (ownership.IdCommunity == 0);
            ownership.IdModule = idModule;
            ownership.ModuleCode = moduleCode;
            //ownership.IdCommunity = idCommunity;
            //if (ownership.IdCommunity == -1 && ownership.ModuleObject != null)
            //    ownership.IdCommunity = ownership.ModuleObject.CommunityID;
            //ownership.IsPortal = isPortal || (ownership.IdCommunity == 0);
            //ownership.ModuleObject = obj;
            //ownership.IdModule = idModule;
            //ownership.ModuleCode = moduleCode;
            //if (ownership.IdModule > 0 && String.IsNullOrEmpty(ownership.ModuleCode))
            //    ownership.ModuleCode = Manager.GetModuleCode(ownership.IdModule);
            //else if (ownership.IdModule == 0 && !String.IsNullOrEmpty(ownership.ModuleCode))
            //    ownership.IdModule = Manager.GetModuleID(ownership.ModuleCode);
            //else if (obj != null && obj.ServiceID > 0 && String.IsNullOrEmpty(obj.ServiceCode))
            //    ownership.ModuleCode = Manager.GetModuleCode(obj.ServiceID);
            //else if (obj != null && ownership.IdModule == 0 && !String.IsNullOrEmpty(obj.ServiceCode))
            //    ownership.IdModule = Manager.GetModuleID(obj.ServiceCode);
            //else if (obj != null) {
            //    ownership.IdModule = obj.ServiceID;
            //    ownership.ModuleCode = obj.ServiceCode;
            //}
            return GetDisplayMessages(ownership, idUser).Select(m => new dtoFilteredDisplayMessage(m)).ToList();
        }
        public List<dtoFilteredDisplayMessage> ParseObjectMessages(List<dtoFilteredDisplayMessage> messages, String searchBy, String startWith, MessageOrder orderBy, Boolean ascending,lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
        {
            var query = (from m in messages select m);
            if (!string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchBy.Trim()) == false)
            {
                List<String> values = searchBy.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                if (values.Any() && values.Count == 1)
                    query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(searchBy.ToLower()));
                else if (values.Any() && values.Count > 1)
                    // values.Any(r.Name.ToLower().Contains) && values.Any(r.Surname.ToLower().Contains) || 
                    query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && values.Any(r.DisplayName.ToLower().Contains));
            }
             if (!String.IsNullOrEmpty(startWith)){
                if (startWith != "#")
                    query = query.Where(r => r.FirstLetter == startWith.ToLower());
                else
                    query = query.Where(r => pService.DefaultOtherChars().Contains(r.FirstLetter));
                }
            switch (orderBy)
            {
                case MessageOrder.ByDate:
                    if (ascending)
                        query = query.OrderBy(m=>m.CreatedOn);
                    else
                        query = query.OrderByDescending(m=>m.CreatedOn);
                    break;
                case  MessageOrder.ByName:
                    if (ascending)
                        query = query.OrderBy(m=> m.DisplayName);
                    else
                        query = query.OrderByDescending(m => m.DisplayName);
                    break;
            }

            return query.ToList();
        }

        public List<dtoModuleRecipientMessage> GetRecipientsForMessage(long idMessage, String unknownUser, String anonymousUser, ModuleObject obj, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService, Boolean loadAllInfo = true)
        {
            List<dtoModuleRecipientMessage> items = new List<dtoModuleRecipientMessage>();
            try
            {
                List<MailRecipient> mRecipients = GetRecipientsQuery(filter.IdCommunity, false, obj,"",0, idMessage).ToList();
                if (mRecipients.Any())
                {
                    items = GetParsedMessageRecipients(obj, mRecipients);
                    List<Int32> idUsers = items.Where(r => r.IsInternal).Select(r => r.IdPerson).Distinct().ToList();

                    if (idUsers.Any())
                    {
                        List<Person> users = new List<Person>();
                        if (idUsers.Count <= maxItemsForQuery)
                            users.AddRange((from p in Manager.GetIQ<Person>() where idUsers.Contains(p.Id) select p).ToList());
                        else
                        {
                            Int32 pageIndex = 0;
                            List<Int32> idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            while (idPagedUsers.Any())
                            {
                                users.AddRange((from p in Manager.GetIQ<Person>() where idPagedUsers.Contains(p.Id) select p).ToList());
                                pageIndex++;
                                idPagedUsers = idUsers.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                            }
                        }
                        items.Where(r => r.IsInternal).ToList().ForEach(i => i.UpdatePersonInfo(users.Where(u => u.Id == i.IdPerson).FirstOrDefault(), unknownUser));
                    }
                    if (filter.IdCommunity > 0 && items.Any())
                    {
                        idUsers = items.Select(i => i.IdPerson).ToList();
                        List<LazySubscription> subscriptions = (idUsers.Count <= maxItemsForQuery) ? (from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity == filter.IdCommunity && idUsers.Contains(s.IdPerson) select s).ToList() : (from s in Manager.GetIQ<LazySubscription>() where s.IdCommunity == filter.IdCommunity select s).ToList().Where(s => idUsers.Contains(s.IdPerson)).ToList();
                        items.Where(i => i.IdPerson > 0).ToList().ForEach(i => i.IdRole = subscriptions.Where(s => s.IdPerson == i.IdPerson).Select(s => s.IdRole).FirstOrDefault());
                    }
                    var query = (from r in items
                                 where (filter.IdProfileType < 1 || (filter.IdProfileType > 0 && r.IdProfileType == filter.IdProfileType))
                                     && (filter.IdRole < 1 || (filter.IdRole > 0 && r.IdRole == filter.IdRole))
                                 select r);


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
                        if (loadAllInfo || filter.OrderBy == MailSender.UserByMessagesOrder.ByAgency)
                        {
                            Dictionary<long, String> agencyName = pService.GetAgenciesName(agencyInfos.Keys.ToList());
                            foreach (var i in agencyInfos)
                            {
                                query.Where(r => r.IsInternal && i.Value.Contains(r.IdPerson)).ToList().ForEach(r => r.UpdateAgencyInfo(i.Key, (agencyName.ContainsKey(i.Key) ? agencyName[i.Key] : "")));
                            }
                        }
                    }
                    items = query.ToList();
                    items.Where(i => String.IsNullOrEmpty(i.DisplayName) && !String.IsNullOrEmpty(i.Name) && !String.IsNullOrEmpty(i.Surname)).ToList().ForEach(r => r.DisplayName = r.Surname + " " + r.Name);
                    items.Where(i => String.IsNullOrEmpty(i.DisplayName) && String.IsNullOrEmpty(i.Name) && String.IsNullOrEmpty(i.Surname)).ToList().ForEach(r => r.DisplayName = r.MailAddress);
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }

        public List<dtoModuleRecipientMessage> GetParsedMessageRecipients(ModuleObject obj, List<MailRecipient> recipients, Boolean alsoSubject= false)
        {
            List<dtoModuleRecipientMessage> results = recipients.Select(r =>
                                                        new dtoModuleRecipientMessage() { 
                                                            IdPerson = r.IdPerson,
                                                            IdRecipient = r.Id,
                                                            DisplayName = r.DisplayName,
                                                            MailAddress = r.MailAddress ,
                                                            IdUserModule = r.IdUserModule
                                                        }
                                                    ).ToList();


            //foreach (var item in recipients.GroupBy(r => r.IdPerson))
            //{
            //    if (item.Key > 0)
            //    {
            //        results.Add(new dtoModuleRecipientMessage()
            //        {
            //            IdPerson = item.Key,
            //            IdUserModule = item.FirstOrDefault().IdUserModule,
            //            ModuleCode = obj.ServiceCode,
            //            MessageNumber = item.Count(),
            //            Messages = item.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType, Subject = (alsoSubject) ? m.Item.Subject : "" }).ToList()
            //        });
            //    }
            //    else if (item.Where(i => i.IdUserModule > 0).Any())
            //    {
            //        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdUserModule > 0 && r.IdPerson < 1).ToList().GroupBy(r => r.IdUserModule).ToList().Select(i => new dtoModuleRecipientMessages()
            //        {
            //            IdUserModule = i.Key,
            //            ModuleCode = obj.ServiceCode,
            //            MessageNumber = i.Count(),
            //            MailAddress = i.OrderByDescending(it => it.Id).ToList().Select(it => it.MailAddress).FirstOrDefault(),
            //            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType, Subject = (alsoSubject) ? m.Item.Subject : "" }).ToList()
            //        }).ToList());
            //    }
            //    else
            //    {
            //        results.AddRange(recipients.Where(r => r.IsFromModule && r.IdPerson == 0 && r.IdUserModule == 0 && !String.IsNullOrEmpty(r.MailAddress)).GroupBy(r => r.MailAddress.ToLower()).ToList().Select(i => new dtoModuleRecipientMessages()
            //        {
            //            IdUserModule = 0,
            //            ModuleCode = obj.ServiceCode,
            //            MailAddress = i.Key,
            //            MessageNumber = i.Count(),
            //            Messages = i.ToList().Select(m => new dtoUserMessageInfo() { Id = m.Id, IdLanguage = m.IdLanguage, LanguageCode = m.LanguageCode, SentOn = m.Item.CreatedOn, Sent = m.IsMailSent, IdModuleObject = m.IdModuleObject, IdModuleType = m.IdModuleType, Subject = (alsoSubject) ? m.Item.Subject : "" }).ToList()
            //        }).ToList());
            //    }
            //}
            return results;
        }
        public List<dtoModuleRecipientMessage> GetParsedUsersForMessage(List<dtoModuleRecipientMessage> recipients, lm.Comol.Core.BaseModules.MailSender.dtoUsersByMessageFilters filter, lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pService)
        {
            var query = (from r in recipients select r);
            if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Value.Trim()) == false)
            {
                switch (filter.SearchBy)
                {
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Contains:
                        List<String> values = filter.Value.Split(' ').ToList().Where(f => !String.IsNullOrEmpty(f)).Select(f => f.ToLower()).ToList();
                        if (values.Any() && values.Count == 1)
                            query = query.Where(r => !String.IsNullOrEmpty(r.DisplayName) && r.DisplayName.ToLower().Contains(filter.Value.ToLower()));
                        else if (values.Any() && values.Count > 1)
                            // values.Any(r.Name.ToLower().Contains) && values.Any(r.Surname.ToLower().Contains) || 
                            query = query.Where(r => (!String.IsNullOrEmpty(r.Name) && values.Any(r.Name.ToLower().Contains)) || (!String.IsNullOrEmpty(r.Surname) && values.Any(r.Surname.ToLower().Contains)) || values.Any(r.MailAddress.ToLower().Contains) || values.Any(r.DisplayName.ToLower().Contains));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Mail:
                        query = query.Where(r => r.MailAddress.ToLower().Contains(filter.Value.ToLower()));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Name:
                        query = query.Where(r => !String.IsNullOrEmpty(r.Name) && r.Name.ToLower().StartsWith(filter.Value.ToLower()));
                        break;
                    case Core.BaseModules.ProfileManagement.SearchProfilesBy.Surname:
                        query = query.Where(r => !String.IsNullOrEmpty(r.Name) && r.Surname.ToLower().StartsWith(filter.Value.ToLower()));
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
            switch (filter.OrderBy)
            {
                case MailSender.UserByMessagesOrder.ByProfileType:
                    if (filter.Ascending)
                        query = query.OrderBy(r => filter.ProfyleTypeTranslations[r.IdProfileType]);
                    else
                        query = query.OrderByDescending(r => filter.ProfyleTypeTranslations[r.IdProfileType]);
                    break;
                case MailSender.UserByMessagesOrder.ByRole:
                    if (filter.Ascending)
                        query = query.OrderBy(r => filter.RoleTranslations[r.IdRole]);
                    else
                        query = query.OrderByDescending(r => filter.RoleTranslations[r.IdRole]);
                    break;
                case MailSender.UserByMessagesOrder.ByInternal:
                    if (filter.Ascending)
                        query = query.OrderByDescending(r => r.IsInternal).ThenBy(r => r.DisplayName);
                    else
                        query = query.OrderBy(r => r.IsInternal).ThenBy(r => r.DisplayName);
                    break;
                case MailSender.UserByMessagesOrder.ByUser:
                    if (filter.Ascending)
                        query = query.OrderBy(r => r.DisplayName);
                    else
                        query = query.OrderByDescending(r => r.DisplayName);
                    break;
                case MailSender.UserByMessagesOrder.ByAgency:
                    if (filter.Ascending)
                        query = query.OrderBy(r => r.AgencyName);
                    else
                        query = query.OrderByDescending(r => r.AgencyName);
                    break;
            }

            return query.ToList();
        }

        public List<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings> SaveNotificationSettings(TemplateLevel level, List<lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent> settings, Boolean forPortal, Int32 idCommunity, Int32 idOrganization, ModuleObject obj = null, lm.Comol.Core.Notification.Domain.NotificationChannel channel = lm.Comol.Core.Notification.Domain.NotificationChannel.Mail, lm.Comol.Core.Notification.Domain.NotificationMode mode = lm.Comol.Core.Notification.Domain.NotificationMode.Automatic)
        {
            List<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings> items = new List<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings>();
            try
            {
                Manager.BeginTransaction();
                litePerson p = Manager.GetLitePerson(UC.CurrentUserID);
                if (p != null && p.TypeID != (int)UserTypeStandard.Guest) {
                    List<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings> inUse = null;
                    if (obj == null)
                        inUse = (from n in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings>()
                                 where n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != lm.Comol.Core.Notification.Domain.NotificationActionType.Ignore && n.Settings.ActionType != lm.Comol.Core.Notification.Domain.NotificationActionType.None
                                    && (n.ObjectOwner == null || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || (idOrganization != -1 && idOrganization == n.IdOrganization) || n.IsForPortal)
                                    select n).ToList();
                    else
                        inUse = (from n in Manager.GetIQ<lm.Comol.Core.TemplateMessages.Domain.CommonNotificationSettings>()
                                 where n.Settings.Channel == channel && n.Settings.Mode == mode && n.Settings.ActionType != lm.Comol.Core.Notification.Domain.NotificationActionType.Ignore && n.Settings.ActionType != lm.Comol.Core.Notification.Domain.NotificationActionType.None
                                    && (
                                    (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == obj.ObjectLongID && n.ObjectOwner.ObjectTypeID == obj.ObjectTypeID && n.ObjectOwner.ServiceCode == obj.ServiceCode)
                                    || (idCommunity != -1 && (idCommunity == n.IdCommunity)) || (idOrganization != -1 && idOrganization == n.IdOrganization) || n.IsForPortal
                                    )
                                    select n).ToList();
                    Dictionary<String, Int32> mInfo= Manager.GetIdModules(settings.Select(s=> s.ModuleCode).Distinct().ToList());
                    foreach( lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoModuleEvent setting in settings){
                        CommonNotificationSettings current = null;
                        var query = inUse.Where(n => n.Settings.IdModuleAction == setting.IdEvent && n.Settings.ModuleCode == setting.ModuleCode);
                        if (obj == null)
                            query = query.Where(n=>n.ObjectOwner == null);
                        else
                            query = query.Where(n => (n.ObjectOwner != null && n.ObjectOwner.ObjectLongID == obj.ObjectLongID && n.ObjectOwner.ObjectTypeID == obj.ObjectTypeID && n.ObjectOwner.ServiceCode == obj.ServiceCode));
                        switch (level) { 
                            case TemplateLevel.Portal:
                                #region "Portal Saving Settings"
                                current = query.Where(n=>n.IsForPortal && n.ObjectOwner==null).FirstOrDefault();
                                if (current == null && setting.IdTemplate>0)
                                {
                                    current = new CommonNotificationSettings();
                                    current.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                    current.IsEnabled = true;
                                    current.IsForPortal = true;
                                    current.Settings.ActionType = lm.Comol.Core.Notification.Domain.NotificationActionType.BySystem;
                                    current.Settings.Channel = channel;
                                    current.Settings.IdModule= (mInfo.ContainsKey(setting.ModuleCode) ? mInfo[setting.ModuleCode]: 0);
                                    current.Settings.IdModuleAction= setting.IdEvent;
                                    current.Settings.Mode= mode;
                                    current.Settings.ModuleCode= setting.ModuleCode;
                                }
                                else if (current != null && (current.Deleted== BaseStatusDeleted.None || (setting.IdTemplate>0 && current.Deleted!= BaseStatusDeleted.None)))
                                    current.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                if (current != null) {
                                    current.Deleted =(setting.IdTemplate>0) ?  BaseStatusDeleted.None: BaseStatusDeleted.Manual;
                                    if (setting.IdTemplate>0){
                                        current.AlwaysLastVersion = (setting.IdVersion == 0);
                                        current.Template = Manager.Get<TemplateDefinition>(setting.IdTemplate);
                                        current.Version = (setting.IdVersion == 0) ? null : Manager.Get<TemplateDefinitionVersion>(setting.IdVersion);
                                    }
                                    Manager.SaveOrUpdate(current);
                                    items.Add(current);
                                }
                                #endregion
                                break;
                            case TemplateLevel.Organization:
                                //#region "Organization Saving Settings"
                                //if (setting.IdTemplate > 0) { 
                                //    if (setting.ItemLevel== TemplateLevel.Organization)

                                //}
                                //if (setting.IdTemplate<=0){
                                //    // rimuovo 
                                //}
                                //current = query.Where(n=>!n.IsForPortal && && n.ObjectOwner==null).FirstOrDefault();
                                //if (current == null && setting.IdTemplate>0)
                                //{
                                //    current = new CommonNotificationSettings();
                                //    current.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                //    current.IsEnabled = true;
                                //    current.IsForPortal = true;
                                //    current.Settings.ActionType = NotificationActionType.BySystem;
                                //    current.Settings.Channel = channel;
                                //    current.Settings.IdModule= (mInfo.ContainsKey(setting.ModuleCode) ? mInfo[setting.ModuleCode]: 0);
                                //    current.Settings.IdModuleAction= setting.IdEvent;
                                //    current.Settings.Mode= mode;
                                //    current.Settings.ModuleCode= setting.ModuleCode;
                                //}
                                //else if (current != null && (current.Deleted== BaseStatusDeleted.None || (setting.IdTemplate>0 && current.Deleted!= BaseStatusDeleted.None)))
                                //    current.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress);
                                //if (current != null) {
                                //    current.Deleted =(setting.IdTemplate>0) ?  BaseStatusDeleted.None: BaseStatusDeleted.Manual;
                                //    if (setting.IdTemplate>0){
                                //        current.AlwaysLastVersion = (setting.IdVersion == 0);
                                //        current.Template = Manager.Get<TemplateDefinition>(setting.IdTemplate);
                                //        current.Version = (setting.IdVersion == 0) ? null : Manager.Get<TemplateDefinitionVersion>(setting.IdVersion);
                                //    }
                                //    Manager.SaveOrUpdate(current);
                                //    items.Add(current);
                                //}
                                //#endregion
                                break;
                        }
                    }
                }
                else
                    items = null;
                Manager.Commit();
            }
            catch (Exception ex) {
                Manager.RollBack();
                items = null;
            }
            return items;
        }

    }
}