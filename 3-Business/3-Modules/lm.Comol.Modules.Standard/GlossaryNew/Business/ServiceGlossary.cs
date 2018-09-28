using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.Wizard;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using ItemStatus = lm.Comol.Modules.Standard.GlossaryNew.Domain.ItemStatus;

//using System.Web;

namespace lm.Comol.Modules.Standard.GlossaryNew.Business
{
    public partial class ServiceGlossary : BaseCoreServices
    {
        public Dictionary<int, string> GetLanguageDictionaryCodes()
        {
            //var result = Manager.GetIQ<Language>().Select(f => new KeyValuePair<Int32, String>(f.Id, f.Code.Split('-')[0].ToUpper())).ToDictionary<Int32, String>();
            var result = Manager.GetIQ<Language>().ToDictionary(mc => mc.Id, mc => mc.Code.Split('-')[0].ToUpper());
            return result;
        }

        public Dictionary<int, string> GetLanguageDictionaryDescriptions()
        {
            //var result = Manager.GetIQ<Language>().Select(f => new KeyValuePair<Int32, String>(f.Id, f.Code.Split('-')[0].ToUpper())).ToDictionary<Int32, String>();
            var result = Manager.GetIQ<Language>().ToDictionary(mc => mc.Id, mc => mc.Name);
            return result;
        }

        public List<NavigableWizardItem<int>> GetAvailableSteps(GlossaryStep currentStep, Boolean isAdd)
        {
            var result = new List<NavigableWizardItem<Int32>>();

            var stepGeneralSetting = new NavigableWizardItem<Int32>
            {
                DisplayOrderDetail = DisplayOrderEnum.first,
                Id = (Int32)GlossaryStep.Settings,
                AutoPostBack = false,
                Active = currentStep == GlossaryStep.Settings
            };

            result.Add(stepGeneralSetting);

            var stepShared = new NavigableWizardItem<Int32>
            {
                DisplayOrderDetail = DisplayOrderEnum.none,
                Id = (Int32)GlossaryStep.Share,
                AutoPostBack = false,
                Active = !isAdd
            };

            result.Add(stepShared);

            var stepImportTerms = new NavigableWizardItem<Int32>
            {
                DisplayOrderDetail = DisplayOrderEnum.last,
                Id = (Int32)GlossaryStep.Import,
                AutoPostBack = false,
                Active = !isAdd
            };

            result.Add(stepImportTerms);

            return result;
        }

        internal List<DTO_ImportCommunity> GetCommunityGlossaryTerms(Int64 currentGlossary, List<long> listShared, List<long> listPublic)
        {
            listShared.AddRange(listPublic);
            listShared.Remove(currentGlossary);

            var glossaryList = Manager.GetIQ<liteGlossary>().Where(f => listShared.Contains(f.Id)).Select(f => new { f.Id, f.IdCommunity, f.Name }).ToList();
            var termList = Manager.GetIQ<Term>().Where(f => listShared.Contains(f.IdGlossary)).Select(f => new { f.Id, f.IdGlossary, f.Name }).ToList();
            var communityList = glossaryList.Select(f => f.IdCommunity).Distinct().ToList();
            var result = new List<DTO_ImportCommunity>();

            foreach (var itemCommunity in communityList)
            {
                var community = new DTO_ImportCommunity();
                community.IdCommunity = itemCommunity;
                community.Name = string.Format("Comunità {0}", itemCommunity);

                foreach (var itemGlossary in glossaryList.Where(f => f.IdCommunity == itemCommunity))
                {
                    var glossary = new DTO_ImportGlossary();
                    glossary.Id = itemGlossary.Id;
                    glossary.Name = itemGlossary.Name;
                    glossary.TermList = termList.Where(f => f.IdGlossary == itemGlossary.Id).Select(f => new DTO_ImportTerm { Id = f.Id, Name = f.Name }).OrderBy(f => f.Name).ToList();
                    community.GlossaryList.Add(glossary);
                }

                community.GlossaryList = community.GlossaryList.OrderBy(f => f.Name).ToList();
                if (community.GlossaryList.Count > 0)
                    result.Add(community);
            }

            return result;
        }

        internal List<DTO_ImportCommunity> GetCommunityGlossary(List<Int32> communityList)
        {
            var glossaryList = Manager.GetIQ<liteGlossary>().Where(f => communityList.Contains(f.IdCommunity)).Select(f => new { f.Id, f.IdCommunity, f.Name }).ToList();
            var result = new List<DTO_ImportCommunity>();

            foreach (var itemCommunity in communityList.Distinct())
            {
                var community = new DTO_ImportCommunity { IdCommunity = itemCommunity, Name = Manager.GetCommunityName(itemCommunity) };
                foreach (var itemGlossary in glossaryList.Where(f => f.IdCommunity == itemCommunity).ToList())
                {
                    //   if (itemGlossary.Id != idGlossary && community.GlossaryList.All(f => f.Id == itemGlossary.Id))
                    {
                        var glossary = new DTO_ImportGlossary();
                        glossary.Id = itemGlossary.Id;
                        glossary.Name = itemGlossary.Name;
                        community.GlossaryList.Add(glossary);
                    }
                }

                community.GlossaryList = community.GlossaryList.OrderBy(f => f.Name).ToList();

                if (community.GlossaryList.Count > 0)
                    result.Add(community);
            }

            return result;
        }

        internal List<DTO_ImportCommunity> GetCommunityGlossaryTerms(Int64 idGlossary, List<Int32> communityList)
        {
            var glossaryList = Manager.GetIQ<liteGlossary>().Where(f => communityList.Contains(f.IdCommunity)).Select(f => new { f.Id, f.IdCommunity, f.Name }).ToList();
            var termList = Manager.GetIQ<Term>().Where(f => communityList.Contains(f.IdCommunity)).Select(f => new { f.Id, f.IdGlossary, f.Name }).ToList();
            var result = new List<DTO_ImportCommunity>();

            foreach (var itemCommunity in communityList.Distinct())
            {
                var community = new DTO_ImportCommunity();
                community.IdCommunity = itemCommunity;
                community.Name = Manager.GetCommunityName(itemCommunity);

                foreach (var itemGlossary in glossaryList.Where(f => f.IdCommunity == itemCommunity))
                {
                    if (itemGlossary.Id != idGlossary)
                    {
                        var glossary = new DTO_ImportGlossary();
                        glossary.Id = itemGlossary.Id;
                        glossary.Name = itemGlossary.Name;
                        glossary.TermList = termList.Where(f => f.IdGlossary == itemGlossary.Id).Select(f => new DTO_ImportTerm { Id = f.Id, Name = f.Name }).OrderBy(f => f.Name).ToList();
                        community.GlossaryList.Add(glossary);
                    }
                }

                community.GlossaryList = community.GlossaryList.OrderBy(f => f.Name).ToList();

                if (community.GlossaryList.Count > 0)
                    result.Add(community);
            }

            return result;
        }

        internal List<DTO_ImportCommunity> GetCommunityGlossaryTerms(string fileName)
        {
            var result = new List<DTO_ImportCommunity>();
            var item = new XmlRepository<GlossaryContainer>();
            var glossaryContainer = item.Deserialize(fileName);
            if (glossaryContainer.Glossaries != null && glossaryContainer.Terms != null)
                foreach (var g in glossaryContainer.Glossaries)
                {
                    var community = result.FirstOrDefault(f => f.IdCommunity == g.IdCommunity);

                    if (community == null)
                    {
                        community = new DTO_ImportCommunity { IdCommunity = g.IdCommunity, Name = g.Community, GlossaryList = new List<DTO_ImportGlossary>() };
                        result.Add(community);
                    }

                    var glossary = new DTO_ImportGlossary { Id = g.Id, Name = g.Name, TermList = new List<DTO_ImportTerm>() };

                    foreach (var term in glossaryContainer.Terms.OrderBy(f => f.Name).Where(f => f.IdGlossary == g.Id).Select(t => new DTO_ImportTerm { Id = t.Id, Name = t.Name }))
                        glossary.TermList.Add(term);

                    community.GlossaryList.Add(glossary);
                }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="idCommunity"></param>
        /// <param name="idGlossary"></param>
        /// <param name="listId"></param>
        /// <param name="selectedIndex">0-Tutti pubblicati; 1-Tutti bozza; 2-Bozza solo duplicati</param>
        public Boolean ImportTerms(int idCommunity, long idGlossary, List<long> listId, int selectedIndex)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            foreach (var idTerm in listId)
            {
                var term = GetTerm(idTerm);
                var newTerm = CloneTerm(idCommunity, idGlossary, term, selectedIndex);

                if (Manager.IsInTransaction())
                {
                    try
                    {
                        Manager.SaveOrUpdate(newTerm);
                    }
                    catch (Exception e1)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        return false;
                    }
                }
            }

            try
            {
                // esegue anche il commit
                UpdateGlossaryTerms(idGlossary);
            }
            catch (Exception e1)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="idCommunity"></param>
        /// <param name="glossary"></param>
        /// <param name="selectedIndex">0 - tutti pubblici; 1 - tutti non pubblici; 2 - i rinominati non pubblici</param>
        /// <returns></returns>
        private Domain.Glossary CloneGlossary(int idCommunity, Domain.Glossary glossary, int selectedIndex)
        {
            var item = new Domain.Glossary
            {
                IdCommunity = idCommunity,
                Status = ItemStatus.Available,
                DisplayMode = DisplayMode.AllDefinition
            };
            item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            var hasSameName = communityHasGlossaryWithName(glossary.Name, idCommunity);

            item.Name = hasSameName ? string.Format("{0}_{1}", glossary.Name, DateTime.Now.ToString("dd/MM/yy H:mm:ss")) : glossary.Name;

            item.Description = glossary.Description;
            //item.IsDefault = false;
            item.DisplayOrder = glossary.DisplayOrder;
            item.TermsArePaged = glossary.TermsArePaged;
            item.TermsPerPage = glossary.TermsPerPage;
            item.IdLanguage = glossary.IdLanguage;
            item.IsPublished = glossary.IsPublished;

            if (selectedIndex == 0)
                item.IsPublished = true;
            else if (selectedIndex == 1)
                item.IsPublished = false;
            else if (selectedIndex == 2)
                item.IsPublished = !hasSameName;
            else
                item.IsPublished = glossary.IsPublished;


            return item;
        }

        private Term CloneTerm(int idCommunity, long idGlossary, Term term, int selectedIndex = -1)
        {
            var item = new Term();
            item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            item.IdCommunity = idCommunity;
            item.IdGlossary = idGlossary;

            //Modifica parametri
            item.Name = term.Name;
            item.Description = term.Description;
            item.DescriptionText = term.DescriptionText;
            if (selectedIndex == 0)
                item.IsPublished = true;
            else if (selectedIndex == 1)
                item.IsPublished = false;
            else if (selectedIndex == 2)
                item.IsPublished = !glossaryHasTermWithName(term.Name, idGlossary);
            else
                item.IsPublished = term.IsPublished;

            if (!String.IsNullOrWhiteSpace(item.Name))
            {
                var currentFirstLetter = item.Name.ToLower()[0];
                if (char.IsLetter(currentFirstLetter))
                    item.FirstLetter = currentFirstLetter;
                else if (char.IsDigit(currentFirstLetter))
                    item.FirstLetter = '#';
                else
                    item.FirstLetter = '_';
            }
            return item;
        }

        private Boolean glossaryHasTermWithName(String name, long idGlossary)
        {
            return Manager.GetIQ<Term>().Any(item => item.IdGlossary == idGlossary && item.Name == name);
        }

        private Boolean communityHasGlossaryWithName(String name, long idCommunity)
        {
            return Manager.GetIQ<Domain.Glossary>().Any(item => item.IdCommunity == idCommunity && item.Name == name);
        }

        /// <summary>
        /// </summary>
        /// <param name="idCommunity"></param>
        /// <param name="listId"></param>
        /// <param name="selectedIndex">0 - tutti pubblici; 1 - tutti non pubblici; 2 - i rinominati non pubblici</param>
        public Boolean ImportGlossaries(int idCommunity, List<long> listId, int selectedIndex = -1)
        {
            foreach (var idGlossary in listId)
            {
                var glossary = GetGlossary(idGlossary);
                var newGlossary = CloneGlossary(idCommunity, glossary, selectedIndex);
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();


                if (Manager.IsInTransaction())
                {
                    try
                    {
                        Manager.SaveOrUpdate(newGlossary);


                        var itemDisplayOrder = new GlossaryDisplayOrder();
                        itemDisplayOrder.Glossary = newGlossary;
                        itemDisplayOrder.IdCommunity = idCommunity;
                        var maxValue = Manager.GetIQ<GlossaryDisplayOrder>().Where(f => f.IdCommunity == idCommunity).Max(f => f.DisplayOrder);
                        itemDisplayOrder.DisplayOrder = maxValue + 1;
                        itemDisplayOrder.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        Manager.SaveOrUpdate(itemDisplayOrder);

                        var termList = Manager.GetIQ<Term>().Where(f => f.IdGlossary == idGlossary).ToList();

                        foreach (var term in termList)
                        {
                            var newTerm = CloneTerm(idCommunity, newGlossary.Id, term);
                            Manager.SaveOrUpdate(newTerm);
                        }
                        UpdateGlossaryTerms(newGlossary.Id);
                    }
                    catch (Exception e1)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                        return false;
                    }
                }
            }
            return true;
        }

        public GlossaryContainer RetriveGlossaryData(IEnumerable<long> selectedTerms)
        {
            var terms = new List<TermSerialized>();

            var currentIndex = 0;
            const int page = 500;

            var currentPage = currentIndex * page;

            while (currentPage < selectedTerms.Count())
            {
                terms.AddRange(Manager.GetIQ<TermSerialized>().Where(f => selectedTerms.Contains(f.Id)).Skip(currentPage).Take(page).ToList());
                currentIndex++;
                currentPage = currentIndex * page;
            }

            var selectedGlossaries = terms.Select(f => f.IdGlossary).Distinct().ToList();
            var glossaries = Manager.GetIQ<GlossarySerialized>().Where(f => selectedGlossaries.Contains(f.Id)).ToList();
            foreach (var glossarySerialized in glossaries)
                glossarySerialized.Community = Manager.GetCommunityName(glossarySerialized.IdCommunity);

            var result = new GlossaryContainer { Terms = terms, Glossaries = glossaries };
            return result;
        }

        public bool ImportGlossaryData(Int32 idCommunity, GlossaryContainer item, List<long> idTermList, int selectedIndex)
        {
            var result = false;
            if (item != null)
            {
                var validGlossary = new List<long>();

                foreach (var termSerialized in item.Terms.ToList())
                {
                    if (!idTermList.Contains(termSerialized.Id))
                        item.Terms.Remove(termSerialized);
                    else
                    {
                        if (!validGlossary.Contains(termSerialized.IdGlossary))
                            validGlossary.Add(termSerialized.IdGlossary);
                    }
                }

                result = true;
                foreach (var glossary in item.Glossaries.Where(f => validGlossary.Contains(f.Id)))
                {
                    var idCommunityItem = idCommunity > 0 ? idCommunity : glossary.IdCommunity;
                    var newGlossary = CloneGlossarySerialized(idCommunityItem, glossary, selectedIndex);
                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    if (Manager.IsInTransaction())
                    {
                        try
                        {
                            Manager.SaveOrUpdate(newGlossary);

                            var itemDisplayOrder = new GlossaryDisplayOrder();
                            itemDisplayOrder.Glossary = newGlossary;
                            itemDisplayOrder.IdCommunity = idCommunityItem;
                            var maxValue = Manager.GetIQ<GlossaryDisplayOrder>().Where(f => f.IdCommunity == idCommunityItem).Max(f => f.DisplayOrder);
                            itemDisplayOrder.DisplayOrder = maxValue + 1;
                            itemDisplayOrder.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                            Manager.SaveOrUpdate(itemDisplayOrder);

                            var termList = item.Terms.Where(f => f.IdGlossary == glossary.Id);

                            foreach (var term in termList)
                            {
                                var newTerm = CloneTermSerialized(idCommunityItem, newGlossary.Id, term);
                                Manager.SaveOrUpdate(newTerm);
                            }

                            UpdateGlossaryTerms(newGlossary.Id);
                        }
                        catch (Exception e1)
                        {
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                            result = false;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        private Domain.Glossary CloneGlossarySerialized(int idCommunity, GlossarySerialized glossary, int selectedIndex)
        {
            var item = new Domain.Glossary
            {
                IdCommunity = idCommunity,
                Status = ItemStatus.Available,
                DisplayMode = DisplayMode.AllDefinition
            };
            item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            var hasSameName = communityHasGlossaryWithName(glossary.Name, idCommunity);
            item.Name = hasSameName ? string.Format("{0}_{1}", glossary.Name, DateTime.Now.ToString("dd/MM/yy H:mm:ss")) : glossary.Name;

            item.Description = glossary.Description;

            item.DisplayOrder = glossary.DisplayOrder;
            item.TermsArePaged = glossary.TermsArePaged;
            item.TermsPerPage = glossary.TermsPerPage;
            item.IdLanguage = glossary.IdLanguage;

            if (selectedIndex == 0)
                item.IsPublished = true;
            else if (selectedIndex == 1)
                item.IsPublished = false;
            else if (selectedIndex == 2)
                item.IsPublished = !hasSameName;
            else
                item.IsPublished = glossary.IsPublished;

            return item;
        }

        private Term CloneTermSerialized(int idCommunity, long idGlossary, TermSerialized term)
        {
            var item = new Term();
            item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            item.IdCommunity = idCommunity;
            item.IdGlossary = idGlossary;

            //Modifica parametri
            item.Name = term.Name;
            item.Description = term.Description;
            item.DescriptionText = term.DescriptionText;

            item.IsPublished = term.IsPublished;

            if (!String.IsNullOrWhiteSpace(item.Name))
            {
                var currentFirstLetter = item.Name.ToLower()[0];
                if (char.IsLetter(currentFirstLetter))
                    item.FirstLetter = currentFirstLetter;
                else if (char.IsDigit(currentFirstLetter))
                    item.FirstLetter = '#';
                else
                    item.FirstLetter = '_';
            }
            return item;
        }

        public String GetGlossaryName(long id)
        {
            var nameGlossary = String.Empty;
            if (id > 0)
            {
                var glossary = Manager.GetIQ<Domain.Glossary>().FirstOrDefault(f => f.Id == id);
                if (glossary != null)
                    nameGlossary = glossary.Name;
            }
            return nameGlossary;
        }

        public List<int> GetIdCommunityWithoutGlossaries(Int32 idCommunity = -1)
        {
            var idListCommunityGlossaryList = Manager.GetIQ<Domain.Glossary>().Where(f => f.TermsCount > 0).Select(f => f.IdCommunity).Distinct().ToList();
            var allCommunityIds = Manager.GetIQ<Community>().Select(f => f.Id).ToList();
            var result = allCommunityIds.Except(idListCommunityGlossaryList).ToList();
            if (idCommunity > 0)
                result.Add(idCommunity);
            return result;
        }

        internal string GetCommunityName(int idCommunity)
        {
            return Manager.GetCommunityName(idCommunity);
        }

        #region Stats

        public void StatAddTermView(int idCommunity, long idGlossary, long idTerm, int idPerson)
        {
            var stat = new Statistics { IdCommunity = idCommunity, IdGlossary = idGlossary, IdTerm = idTerm, IdPerson = idPerson, ViewdOn = DateTime.Now };
            Manager.SaveOrUpdate(stat);
        }

        #endregion

        public DTO_TermEmbed GetDTO_TermEmbed(long idTerm)
        {
            var liteTerm = Manager.GetIQ<liteTerm>().FirstOrDefault(itm => itm.Id == idTerm && itm.Deleted == BaseStatusDeleted.None);
            if (liteTerm != null)
                return new DTO_TermEmbed(liteTerm);
            return null;
        }

        public DTO_TermsEmbed GetDTO_TermsEmbed(int idCommunity, long idGlossary, List<long> idTermList)
        {
            var result = new DTO_TermsEmbed { IdCommunity = idCommunity, IdGlossary = idGlossary, GlossaryName = GetGlossaryName(idGlossary) };
            foreach (var idTerm in idTermList)
                result.TermEmbeds.Add(GetDTO_TermEmbed(idTerm));
            return result;
        }

        public DTO_TermsEmbed GetDTO_TermsEmbed(int idCommunity, long idGlossary)
        {
            var idTermList = Manager.GetIQ<liteTerm>().Where(f => f.IdGlossary == idGlossary && f.Deleted == BaseStatusDeleted.None).OrderBy(f => f.Name).Select(f => f.Id).ToList();
            return GetDTO_TermsEmbed(idCommunity, idGlossary, idTermList);
        }

        #region Share

        public Share GetShare(Int64 id)
        {
            return Manager.Get<Share>(id);
        }

        internal void AddGlossaryShareList(List<Int32> communityList, Int64 idGlossary)
        {
            var glossary = GetGlossary(idGlossary);
            foreach (var idCommunity in communityList)
            {
                if (Manager.GetIQ<Share>().Any(f => f.IdGlossary == idGlossary && f.IdCommunity == idCommunity))
                    continue;

                var item = new Share();
                item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                item.IdCommunity = idCommunity;
                item.IdGlossary = idGlossary;
                item.Status = ShareStatusEnum.Pending;
                item.Permissions = SharePermissionEnum.None;
                item.Type = ShareTypeEnum.Glossary;

                var displayOrder = new GlossaryDisplayOrder();
                displayOrder.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                displayOrder.DisplayOrder = Int32.MaxValue;
                displayOrder.IdCommunity = idCommunity;
                displayOrder.Glossary = glossary;

                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                if (Manager.IsInTransaction())
                {
                    try
                    {
                        var createDisplayOrder = false;
                        Manager.SaveOrUpdate(item);
                        Manager.SaveOrUpdate(displayOrder);
                        Manager.Commit();
                    }
                    catch (Exception e1)
                    {
                        if (Manager.IsInTransaction())
                            Manager.RollBack();
                    }
                }
            }
        }

        public Boolean DeleteVirtualShare(long idShare, out String errors)
        {
            errors = String.Empty;
            Share item;
            if (idShare <= 0)
            {
                item = new Share();
                item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                return true;
            }
            item = GetShare(idShare);
            item.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            var glossary = GetGlossary(item.IdGlossary);

            var displayOrder = Manager.GetIQ<GlossaryDisplayOrder>().FirstOrDefault(f => f.IdCommunity == item.IdCommunity && f.Glossary == glossary);
            if (displayOrder != null)
                displayOrder.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    if (displayOrder != null)
                        Manager.SaveOrUpdate(displayOrder);

                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        internal List<DTO_Share> GetDTO_GlossaryShareList(Int64 idGlossary)
        {
            IEnumerable<Share> list = Manager.GetIQ<Share>().Where(f => f.IdGlossary == idGlossary && f.Deleted == BaseStatusDeleted.None);
            var result = list.Select(f => new DTO_Share(f)).ToList();

            foreach (var dtoShare in result)
                dtoShare.FromCommunityName = Manager.GetCommunityName(dtoShare.IdCommunity);


            return result;
        }

        public List<int> GetSharedIdList(Int64 idGlossary)
        {
            IEnumerable<Share> list = Manager.GetIQ<Share>().Where(f => f.IdGlossary == idGlossary);
            return list.Select(f => f.IdCommunity).ToList();
        }

        internal DTO_Share GetDTO_GlossaryShare(Int64 idGlossary, Int32 idCommunity)
        {
            var share = Manager.GetIQ<Share>().FirstOrDefault(f => f.IdGlossary == idGlossary && f.IdCommunity == idCommunity && f.Deleted == BaseStatusDeleted.None);
            // non è condiviso
            if (share == null)
                return null;

            var glossary = GetDTO_Glossary(idGlossary, idCommunity);
            var dto = new DTO_Share(share);
            dto.IdFromCommunity = glossary.IdCommunity;
            dto.FromCommunityName = Manager.GetCommunityName(glossary.IdCommunity);
            var listGlossary = (from item in Manager.GetIQ<Domain.Glossary>()
                                where item.Id == idGlossary
                                select item.Description).ToList();
            if (listGlossary.Count > 0)
                dto.GlossaryDescription = listGlossary[0];

            return dto;
        }

        public void ChangeShareStatus(long idGlossary, int idCommunity, ShareStatusEnum state)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var share = Manager.GetIQ<Share>().FirstOrDefault(f => f.IdGlossary == idGlossary && f.IdCommunity == idCommunity);
                    if (share != null)
                    {
                        share.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        share.Status = state;
                        Manager.SaveOrUpdate(share);
                    }

                    Manager.Commit();
                }
                catch (Exception e1)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
        }

        public void ChangeShareVisibility(long idGlossary, int idCommunity, Boolean visible)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var share = Manager.GetIQ<Share>().FirstOrDefault(f => f.IdGlossary == idGlossary && f.IdCommunity == idCommunity);
                    if (share != null)
                    {
                        share.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        share.Visible = visible;
                        Manager.SaveOrUpdate(share);
                    }

                    Manager.Commit();
                }
                catch (Exception e1)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
        }

        #endregion

        #region Permission

        /// <summary>
        ///     Restituisce i permessi che ha l'utente, sul glossario condiviso sommando quelli della comunità d'origine del
        ///     glossario, i permessi dei vari share
        /// </summary>
        /// <param name="person"></param>
        /// <param name="dto_glossary"></param>
        /// <param name="idCommunity"></param>
        /// <param name="permissionDictionary"></param>
        /// <returns></returns>
        public GlossaryPermission GetSharedGlossaryPermission(litePerson person, DTO_Glossary dto_glossary, int idCommunity)
        {
            GlossaryPermission result;

            // controllo i permessi per utente nella comunità di origine del glossario
            result = new GlossaryPermission(GetPermissions(dto_glossary.IdCommunity, person));

            // Ho tutti i permessi (verificare se sono solo questi) e quindi non controllo altro
            if (result.AddTerm && result.EditTerm && result.DeleteTerm)
            {
                result.ViewTerm = true;
                return result;
            }

            // prelevo lista di tutte le condivisioni attive del glossario
            var listCommunityShare = Manager.GetIQ<Share>().Where(f => f.IdGlossary == dto_glossary.Id && f.Type == ShareTypeEnum.Glossary &&
                                                                       (f.Status == ShareStatusEnum.Active || f.Status == ShareStatusEnum.ForceActive || f.Status == ShareStatusEnum.Pending)
                                                                       && f.Deleted == BaseStatusDeleted.None).ToList();


            // aggiungo i permessi dalle condivisioni, al momento ho solo i permessi dalla comunità di origine
            foreach (var communityShare in listCommunityShare)
            {
                // Per ogni share verifico se nella comunità ho almeno la possibilità di vedere i glossari per capire se sono iscritto con un minimo di permessi
                var communityViewPermission = GetPermissions(communityShare.IdCommunity, person).ViewTerm;

                if (communityViewPermission)
                {
                    //E' una comunità in cui ho permessi e sono quindi sono di conseguenza iscritto
                    //Carico i permessi (Aggiunta/Modifica/Eliminazione Termine) della condivisione e li aggiungo
                    result.AddPermissions(communityShare.Permissions);
                    result.ViewTerm = true;
                }

                // Ho tutti i permessi (verificare se sono solo questi) e quindi non controllo altro
                if (result.AddTerm && result.EditTerm && result.DeleteTerm)
                {
                    result.ViewTerm = true;
                    return result;
                }
            }

            return result;
        }

        public ShareStatusEnum GetGlossaryHasShare(long idGlossary, Int32 idCommunity)
        {
            var share = Manager.GetIQ<Share>().FirstOrDefault(f => f.IdGlossary == idGlossary && f.IdCommunity == idCommunity && f.Deleted == BaseStatusDeleted.None);
            if (share != null)
                return share.Status;
            return ShareStatusEnum.None;
        }

        public Int32 HasPendingShare(long idGlossary, int idCommunity)
        {
            return Manager.GetIQ<Share>().Count(f => f.IdGlossary == idGlossary && f.IdCommunity != idCommunity && f.Deleted == BaseStatusDeleted.None && f.Status == ShareStatusEnum.Pending);
        }

        /// <summary>
        ///     Calcolo Permessi Glossario, in base alla comunità attuale, utente, permessi modulo/comunità
        /// </summary>
        /// <param name="glossaryModule">modulo glossario corrente</param>
        /// <param name="dtoGlossary">glossario</param>
        /// <param name="person">utente corrente</param>
        /// <param name="idCommunity">comunità corrente</param>
        /// <param name="permissionDictionary"></param>
        public void UpdateGlossaryPermission(ModuleGlossaryNew glossaryModule, DTO_Glossary dtoGlossary, litePerson person, int idCommunity)
        {
            // Se glossario di comunità aggiorno da permessi modulo/comunità
            if (dtoGlossary.IdCommunity == idCommunity)
            {
                dtoGlossary.Permission.ViewTerm = glossaryModule.ViewTerm;
                dtoGlossary.Permission.EditGlossary = glossaryModule.EditGlossary;
                dtoGlossary.Permission.DeleteGlossary = glossaryModule.DeleteGlossary;
                dtoGlossary.Permission.AddTerm = glossaryModule.AddTerm;
                dtoGlossary.Permission.EditTerm = glossaryModule.EditTerm;
                dtoGlossary.Permission.DeleteTerm = glossaryModule.DeleteTerm;
                dtoGlossary.Permission.ViewStat = glossaryModule.ViewStat;

                // Verifico se è condiviso
                dtoGlossary.Shared = GetGlossaryHasShare(dtoGlossary.Id, idCommunity);
            }
            else
            {
                if (dtoGlossary.IsPublic)
                    dtoGlossary.Permission.ViewTerm = true;
                else
                {
                    // E' sempre condiviso, non occorre il controllo
                    dtoGlossary.Shared = GetGlossaryHasShare(dtoGlossary.Id, idCommunity);

                    //Calcolo dei permessi sommando i permessi che ha l'utente anche sulle altre condivisioni del glossario
                    dtoGlossary.SetPermission(GetSharedGlossaryPermission(person, dtoGlossary, idCommunity));
                }
            }
        }

        #endregion

        #region Base

        protected const int maxItemsForQuery = 100;
        protected iApplicationContext _Context;

        /// <summary>
        ///     Person corrente (per created/modify by)
        /// </summary>
        public litePerson CurrentPerson
        {
            get
            {
                if (_currentPerson == null)
                {
                    _currentPerson = Manager.Get<litePerson>(UC.CurrentUserID);
                    if (_currentPerson == null)
                    {
                        //_currentPerson = Manager.GetUnknownUser();
                    }
                }
                return _currentPerson;
            }
        }

        private litePerson _currentPerson { get; set; }

        public int GetServiceIdModule()
        {
            return Manager.GetModuleID(ModuleGlossaryNew.UniqueCode);
        }

        public ModuleGlossaryNew GetPermissions(Int32 idCommunity, litePerson p)
        {
            return (idCommunity < 1)
                ? ModuleGlossaryNew.CreatePortalmodule(p.TypeID)
                : new ModuleGlossaryNew(Manager.GetModulePermission(p.Id, idCommunity, GetServiceIdModule()));
        }

        public List<BaseLanguageItem> GetAvailableLanguages()
        {
            var items = new List<BaseLanguageItem>();

            try
            {
                var languages = Manager.GetAllLanguages().ToList();
                items.AddRange(languages.Select(l => new BaseLanguageItem(l)).ToList());
            }
            catch (Exception ex)
            {
            }

            return items;
        }

        public long GetGlossaryDefaultId(int idCommunity)
        {
            //ricerca glossario default
            var defaultGlossaryId = Manager.GetIQ<GlossaryDisplayOrder>().Where(item => item.IdCommunity == idCommunity && item.Deleted == BaseStatusDeleted.None && item.IsDefault).Select(f => f.Glossary.Id).FirstOrDefault();

            if (defaultGlossaryId > 0)
                return defaultGlossaryId;

            //ricerca se ho solo un glossario
            var glossaryList = Manager.GetIQ<Domain.Glossary>().Where(item => item.IdCommunity == idCommunity && item.Deleted == BaseStatusDeleted.None).Select(f => f.Id).ToList();

            if (glossaryList.Count() == 1)
                return glossaryList[0];

            //nessun glossario default trovato
            return -1;
        }

        #endregion

        #region Glossary

        public Domain.Glossary GetGlossary(Int64 idGlossary)
        {
            return Manager.Get<Domain.Glossary>(idGlossary);
        }

        public liteGlossary GetliteGlossary(Int64 idGlossary)
        {
            return Manager.Get<liteGlossary>(idGlossary);
        }

        public Boolean IsDefaultGlossary(Int32 idcommunity, Int64 idGlossary)
        {
            var item = Manager.GetIQ<GlossaryDisplayOrder>().FirstOrDefault(f => f.IdCommunity == idcommunity && f.Glossary.Id == idGlossary);
            if (item != null)
                return item.IsDefault;
            return false;
        }

        public DTO_Glossary GetDTO_Glossary(Int64 id, Int32 idCommunity)
        {
            try
            {
                if (id > 0)
                {
                    var currentGlossary = GetliteGlossary(id);
                    if (currentGlossary != null)
                    {
                        var isdefault = IsDefaultGlossary(idCommunity, currentGlossary.Id);
                        return new DTO_Glossary(currentGlossary, isdefault);
                    }
                }
                return new DTO_Glossary();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<String> GetGlossaryUsedLetters(Int32 idCommunity)
        {
            var list = Manager.GetIQ<Domain.Glossary>().Where(item => item.IdCommunity == idCommunity && item.Deleted == BaseStatusDeleted.None).Select(f => f.Name).ToList();
            var result = list.Select(item => item.Substring(0, 1).ToLower()).ToList();
            return result;
        }

        public List<string> GetTermUsedLetters(Int64 idGlossary, out Dictionary<Char, UInt16> wordUsingDictionary)
        {
            var list = Manager.GetIQ<Term>().Where(itm => itm.IdGlossary == idGlossary && itm.Deleted == BaseStatusDeleted.None).OrderBy(f => f.Name).Select(n => n.FirstLetter);
            var result = list.ToList().Select(item => item.ToString()).ToList();

            wordUsingDictionary = new Dictionary<char, ushort>();

            foreach (var item in result)
            {
                var currentKey = item[0];

                if (char.IsDigit(currentKey))
                    currentKey = '#';
                else if (!char.IsLetter(currentKey))
                    currentKey = '_';

                if (!wordUsingDictionary.ContainsKey(currentKey))
                    wordUsingDictionary.Add(currentKey, 1);
                else
                    wordUsingDictionary[currentKey] += 1;
            }

            return result;
        }

        /// <summary>
        ///     Metodo che restituisce la lista dei glossari ordinata con permessi, info condivisione ecc...
        /// </summary>
        /// <param name="idCommunity"></param>
        /// <param name="forManagement"></param>
        /// <returns></returns>
        public List<DTO_Glossary> GetDTO_GlossaryListOrdered(Int32 idCommunity, Boolean forManagement)
        {
            var glossaryDisplayOrderList = Manager.GetIQ<GlossaryDisplayOrder>().Where(f => f.IdCommunity == idCommunity && f.Deleted == BaseStatusDeleted.None && f.Glossary.Deleted == BaseStatusDeleted.None).OrderBy(f => f.DisplayOrder).ToList();
            var glossaryShare = Manager.GetIQ<Share>().Where(f => f.IdCommunity == idCommunity && f.Deleted == BaseStatusDeleted.None).ToList();
            if (!forManagement)
                glossaryShare = glossaryShare.Where(f => f.Visible && (f.Status == ShareStatusEnum.Active || f.Status == ShareStatusEnum.ForceActive)).ToList();
            else
                glossaryShare = glossaryShare.Where(f => f.Status != ShareStatusEnum.Refused).ToList();

            var result = new List<DTO_Glossary>();
            foreach (var item in glossaryDisplayOrderList)
            {
                var share = glossaryShare.FirstOrDefault(f => f.IdGlossary == item.Glossary.Id && item.Glossary.IsShared);
                if (idCommunity == item.Glossary.IdCommunity || share != null)
                {
                    if (share != null)
                    {
                        if (share.Status == ShareStatusEnum.Disabled || share.Status == ShareStatusEnum.None || share.Status == ShareStatusEnum.Refused)
                        {
                            //Debug.WriteLine("Glossario {0} {1} condiviso NON disponibile", item.Glossary.Id, item.Glossary.Name);
                        }
                        else
                        {
                            //Debug.WriteLine("Glossario {0} {1} condiviso disponibile", item.Glossary.Id, item.Glossary.Name);
                            if (forManagement || share.Visible)
                            {
                                var dto = new DTO_Glossary(item, share);
                                result.Add(dto);
                            }
                        }
                    }
                    else
                    {
                        //Debug.WriteLine("Glossario {0} {1} disponibile", item.Glossary.Id, item.Glossary.Name);
                        var dto = new DTO_Glossary(item, share);
                        result.Add(dto);
                    }
                }
            }

            return result;
        }

        public List<DTO_Glossary> GetDTO_GlossaryListFromliteGlossary(Expression<Func<liteGlossary, bool>> filter)
        {
            IEnumerable<liteGlossary> list = Manager.GetIQ<liteGlossary>().Where(filter).OrderBy(f => f.DisplayOrder);
            var isdefault = false;
            var result = list.Select(f => new DTO_Glossary(f, false)).ToList();
            return result;
        }

        public List<DTO_GlossaryDelete> GetDTO_GlossaryDelete(Expression<Func<GlossaryDisplayOrder, bool>> filter, Int32 idCommunity)
        {
            var result = Manager.GetIQ<GlossaryDisplayOrder>().Where(filter).ToList().Select(item => new DTO_GlossaryDelete(item.Glossary, idCommunity, item.Id)).ToList();
            return result;
        }

        //public List<DTO_Glossary> GetGlossaryListFromBin(Expression<Func<liteGlossary, bool>> filter, Int32 idLanguage, Func<liteGlossary, Object> orderby = null, Int32 take = 0, Int32 skip = 0)
        //{
        //    IEnumerable<liteGlossary> list = Manager.GetIQ<liteGlossary>().Where(filter);
        //    if (orderby != null)
        //        list = list.OrderBy(orderby);
        //    else
        //        list = list.OrderBy(f => f.DisplayOrder);

        //    if (skip > 0)
        //        list = list.Skip(skip);

        //    if (take > 0)
        //        list = list.Take(take).ToList();

        //    var result = list.Select(f => new DTO_Glossary(f, idLanguage)).ToList();
        //    return result;
        //}

        public List<liteGlossary> GetliteGlossaryList(Expression<Func<liteGlossary, bool>> filter, Func<liteGlossary, Object> orderby)
        {
            var result = Manager.GetIQ<liteGlossary>().Where(filter).OrderBy(orderby).Select(n => new liteGlossary { Id = n.Id, IdCommunity = n.IdCommunity, Name = n.Name, DisplayOrder = n.DisplayOrder }).ToList();
            return result;
        }

        public bool DeleteVirtualGlossaryDisplayOrder(Int64 idShare, out String errors)
        {
            errors = String.Empty;
            if (idShare > 0)
            {
                var item = Manager.Get<GlossaryDisplayOrder>(idShare);
                if (item != null)
                {
                    item.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                    if (item.IdCommunity == item.Glossary.IdCommunity)
                        item.Glossary.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                    if (!Manager.IsInTransaction())
                        Manager.BeginTransaction();

                    if (Manager.IsInTransaction())
                    {
                        try
                        {
                            Manager.SaveOrUpdate(item);
                            Manager.Commit();
                            return true;
                        }
                        catch (Exception e1)
                        {
                            errors = e1.Message;
                            if (Manager.IsInTransaction())
                                Manager.RollBack();
                        }
                    }
                }
            }
            return false;
        }

        public bool DeleteVirtualGlossary(Int64 idGlossary, out String errors)
        {
            errors = String.Empty;
            if (idGlossary <= 0)
                return true;
            var item = GetGlossary(idGlossary);
            item.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);

                    var termList = Manager.GetAll<Term>(f => f.IdGlossary == idGlossary && f.Deleted == BaseStatusDeleted.None);
                    foreach (var term in termList)
                    {
                        item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        item.Deleted = BaseStatusDeleted.Cascade;
                        Manager.SaveOrUpdate(term);
                    }

                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public bool DeleteGlossary(Int64 idGlossary, out String errors)
        {
            errors = String.Empty;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var shared = Manager.GetAll<GlossaryDisplayOrder>(f => f.Glossary.Id == idGlossary);
                    var terms = Manager.GetAll<Term>(f => f.IdGlossary == idGlossary);
                    var glossary = GetGlossary(idGlossary);

                    Manager.DeletePhysicalList(shared);
                    Manager.DeletePhysicalList(terms);
                    Manager.DeletePhysical(glossary);

                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public bool RestoreVirtualGlossary(Int64 idGlossary, out String errors)
        {
            errors = String.Empty;
            if (idGlossary <= 0)
                return false;
            var item = GetGlossary(idGlossary);

            item.SetDeleteMetaInfo(null, String.Empty, String.Empty, null);
            item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            item.UpdateMetaInfo(CurrentPerson, BaseStatusDeleted.None);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);

                    var termList = Manager.GetAll<Term>(f => f.IdGlossary == idGlossary && f.Deleted == BaseStatusDeleted.Cascade);
                    foreach (var term in termList)
                    {
                        item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        item.Deleted = BaseStatusDeleted.None;
                        Manager.SaveOrUpdate(term);
                    }

                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public bool RestoreVirtualGlossaryDisplayOrder(Int64 idShare, out String errors)
        {
            errors = String.Empty;
            if (idShare <= 0)
                return false;
            var item = Manager.Get<GlossaryDisplayOrder>(idShare);

            item.SetDeleteMetaInfo(null, String.Empty, String.Empty, null);
            item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            item.UpdateMetaInfo(CurrentPerson, BaseStatusDeleted.None);

            if (item.IdCommunity == item.Glossary.IdCommunity)
            {
                item.Glossary.SetDeleteMetaInfo(null, String.Empty, String.Empty, null);
                item.Glossary.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                item.Glossary.UpdateMetaInfo(CurrentPerson, BaseStatusDeleted.None);
            }


            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public SaveInfo SaveOrUpdateGlossary(DTO_Glossary glossaryDto)
        {
            var result = new SaveInfo { SaveState = SaveStateEnum.None, Id = glossaryDto.Id };
            Domain.Glossary item;
            if (glossaryDto.Id <= 0)
            {
                item = new Domain.Glossary
                {
                    IdCommunity = glossaryDto.IdCommunity,
                    Status = ItemStatus.Available,
                    DisplayMode = DisplayMode.AllDefinition
                };
                item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                item = GetGlossary(glossaryDto.Id);
                item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            // Verifico se è cambiato il glossario predefinito da !default a default
            if (glossaryDto.IsDefault)
            {
                var oldDefaultGlossary = Manager.GetAll<GlossaryDisplayOrder>(f => f.IdCommunity == glossaryDto.IdCommunity && f.IsDefault && f.Glossary.Id != glossaryDto.Id).FirstOrDefault();
                if (oldDefaultGlossary != null)
                {
                    oldDefaultGlossary.IsDefault = false;
                    oldDefaultGlossary.DisplayOrder = 0;
                    Manager.SaveOrUpdate(oldDefaultGlossary);
                }
                var newDefaultGlossary = Manager.GetAll<GlossaryDisplayOrder>(f => f.IdCommunity == glossaryDto.IdCommunity && f.Glossary.Id == glossaryDto.Id).FirstOrDefault();
                if (newDefaultGlossary != null)
                {
                    newDefaultGlossary.IsDefault = true;
                    newDefaultGlossary.DisplayOrder = -1;
                    Manager.SaveOrUpdate(newDefaultGlossary);
                }

                if (oldDefaultGlossary != null)
                {
                    Manager.SaveOrUpdate(oldDefaultGlossary);
                }

                if (newDefaultGlossary != null)
                {
                    Manager.SaveOrUpdate(newDefaultGlossary);
                }

            }

            // Modifica parametri
            item.Name = glossaryDto.Name;
            item.Description = glossaryDto.Description;
            //item.IsDefault = glossaryDto.IsDefault;
            item.DisplayOrder = glossaryDto.DisplayOrder;
            item.TermsArePaged = glossaryDto.TermsArePaged;
            item.TermsPerPage = glossaryDto.TermsPerPage;
            item.IdLanguage = glossaryDto.IdLanguage;
            item.IsPublished = glossaryDto.IsPublished;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var createDisplayOrder = false;
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();

                    if (result.Id != item.Id)
                    {
                        result.Id = item.Id;
                        // Nuovo Glossario creo riga nella tabella GlossaryDisplayOrder
                        createDisplayOrder = true;
                    }
                    else
                    {
                        // Glossario esistente creo riga nella tabella GlossaryDisplayOrder Verificare IdCommunity
                        createDisplayOrder = !Manager.GetIQ<GlossaryDisplayOrder>().Any(f => f.IdCommunity == item.IdCommunity && f.Glossary.Id == item.Id);
                    }
                    if (createDisplayOrder)
                    {
                        var itemDisplayOrder = new GlossaryDisplayOrder();
                        itemDisplayOrder.Glossary = item;
                        itemDisplayOrder.IdCommunity = item.IdCommunity;
                        var maxValue = Manager.GetIQ<GlossaryDisplayOrder>().Where(f => f.IdCommunity == item.IdCommunity).Max(f => f.DisplayOrder);
                        itemDisplayOrder.DisplayOrder = maxValue + 1;
                        itemDisplayOrder.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        if (!Manager.IsInTransaction())
                            Manager.BeginTransaction();
                        Manager.SaveOrUpdate(itemDisplayOrder);
                        Manager.Commit();
                    }
                    result.SaveState = SaveStateEnum.Saved;
                }
                catch (Exception e1)
                {
                    result.Exception = e1;
                    result.SaveState = SaveStateEnum.DbError;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return result;
        }

        public bool ReorderGlossary(int idCommunity, List<long> idGlossaryList, long defaultId)
        {
            try
            {
                if (!Manager.IsInTransaction())
                    Manager.BeginTransaction();

                var i = 1;

                foreach (var idGlossary in idGlossaryList)
                {
                    var glossaryDisplayOrder = Manager.GetIQ<GlossaryDisplayOrder>().FirstOrDefault(f => f.IdCommunity == idCommunity && f.Glossary.Id == idGlossary);
                    if (glossaryDisplayOrder != null)
                    {
                        glossaryDisplayOrder.DisplayOrder = defaultId == glossaryDisplayOrder.Glossary.Id ? Int32.MinValue : i;
                        Manager.SaveOrUpdate(glossaryDisplayOrder);
                    }
                    i++;
                }
                Manager.Commit();
                return true;
            }
            catch (Exception e)
            {
                ;
            }
            return false;
        }

        public SaveInfo SaveOrUpdateGlossaryShare(DTO_Glossary glossaryDto, List<DTO_Share> dtoShares)
        {
            var result = new SaveInfo { SaveState = SaveStateEnum.None, Id = glossaryDto.Id };

            if (glossaryDto.Id <= 0)
            {
                result.SaveState = SaveStateEnum.GlossaryNotExist;
                return result;
            }

            var item = GetGlossary(glossaryDto.Id);
            item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            // Modifica parametri solo parametri di share 
            item.IsShared = glossaryDto.IsShared;
            item.IsPublic = glossaryDto.IsPublic;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);

                    foreach (var dtoShare in dtoShares)
                    {
                        Share share;

                        if (dtoShare.Id == 0)
                        {
                            share = new Share { IdCommunity = dtoShare.IdCommunity, IdGlossary = dtoShare.IdGlossary };
                            item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        else
                        {
                            share = Manager.Get<Share>(dtoShare.Id);
                            share.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                        }

                        share.Permissions = dtoShare.Permissions;

                        if (dtoShare.Status == ShareStatusEnum.ForceActive)
                            share.Status = dtoShare.Status;
                        else
                        {
                            if (share.Status == ShareStatusEnum.ForceActive)
                                share.Status = ShareStatusEnum.Active;
                        }

                        Manager.SaveOrUpdate(share);
                    }

                    Manager.Commit();
                    result.SaveState = SaveStateEnum.Saved;
                }
                catch (Exception e1)
                {
                    result.Exception = e1;
                    result.SaveState = SaveStateEnum.DbError;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return result;
        }

        #endregion

        #region Term

        public Term GetTerm(Int64 id)
        {
            return Manager.Get<Term>(id);
        }

        public DTO_Term GetTermDTO(Int64 id)
        {
            var liteTerm = Manager.GetIQ<liteTerm>().FirstOrDefault(itm => itm.Id == id && itm.Deleted == BaseStatusDeleted.None);
            if (liteTerm != null)
                return new DTO_Term(liteTerm);
            return null;
        }

        public Dictionary<String, CharInfo> GetGlossaryUsedLetters(Int64 idGlossary)
        {
            var list = Manager.GetIQ<Term>().Where(itm => itm.IdGlossary == idGlossary && itm.Deleted == BaseStatusDeleted.None).OrderBy(f => f.Name).Select(n => n.FirstLetter);
            var temp = list.ToList().Select(item => item.ToString()).Distinct().ToList();
            var result = new Dictionary<String, CharInfo>();
            foreach (var letter in temp)
                if (!result.ContainsKey(letter))
                    result.Add(letter, new CharInfo());
            return result;
        }

        //public Dictionary<String, CharInfo> GetGlossaryUsedLetters(List<Int64> idList)
        //{
        //    var list = Manager.GetIQ<Term>().Where(itm => idList.Contains(itm.IdGlossary) && itm.Deleted == BaseStatusDeleted.None).OrderBy(f => f.Name).Select(n => n.FirstLetter);
        //    var temp = list.ToList().Select(item => item.ToString()).Distinct().ToList();
        //    var result = new Dictionary<String, CharInfo>();
        //    foreach (var letter in temp)
        //        if (!result.ContainsKey(letter))
        //            result.Add(letter, new CharInfo());
        //    return result;
        //}

        //public List<liteTerm> GetTermList(Expression<Func<liteTerm, bool>> filter, Func<liteTerm, Object> orderby, Int32 take, Int32 skip = 0)
        //{
        //    var result = Manager.GetIQ<liteTerm>().Where(filter).OrderBy(orderby);
        //    var resultList = take > 0 ? result.Skip(skip).Take(take).ToList() : result.Skip(skip).ToList();
        //    return resultList;
        //}

        public IEnumerable<DTO_Term> GetDTO_TermListFromliteTerm(Expression<Func<liteTermSearch, bool>> filter, Func<liteTermSearch, object> @orderby, int currentPage, char firstLetter, int pageSize, GlossaryFilter filterSearch, out int records, out Dictionary<string, CharInfo> words)
        {
            IEnumerable<liteTermSearch> list = Manager.GetIQ<liteTermSearch>().Where(filter).OrderBy(@orderby);

            //if (String.IsNullOrWhiteSpace(firstLetter.ToString()))
            //    list = Manager.GetIQ<liteTermSearch>().Where(filter).OrderBy(@orderby);
            //else
            //    list = Manager.GetIQ<liteTermSearch>().Where(filter).Where(f => f.FirstLetter == firstLetter).OrderBy(@orderby);

            if (filterSearch != null)
            {
                if (String.IsNullOrWhiteSpace(filterSearch.LemmaString) && !String.IsNullOrWhiteSpace(filterSearch.SearchString))
                    filterSearch.LemmaString = filterSearch.SearchString;

                if (!String.IsNullOrWhiteSpace(filterSearch.LemmaString))
                {
                    switch (filterSearch.LemmaSearchType)
                    {
                        case FilterTypeEnum.Contains:
                            list = list.Where(f => f.Name.ToLower().Contains(filterSearch.LemmaString));
                            break;
                        case FilterTypeEnum.StartWith:
                            list = list.Where(f => f.Name.ToLower().StartsWith(filterSearch.LemmaString));
                            break;
                        case FilterTypeEnum.EndWith:
                            list = list.Where(f => f.Name.ToLower().EndsWith(filterSearch.LemmaString));
                            break;
                    }
                }
                if (!String.IsNullOrWhiteSpace(filterSearch.LemmaContentString))
                    list = list.Where(f => f.Description.ToLower().Contains(filterSearch.LemmaContentString));

                switch (filterSearch.LemmaVisibilityType)
                {
                    case FilterVisibilityTypeEnum.Published:
                        list = list.Where(f => f.IsPublished);
                        break;
                    case FilterVisibilityTypeEnum.Unpublished:
                        list = list.Where(f => !f.IsPublished);
                        break;
                }
            }

            //  records = String.IsNullOrWhiteSpace(firstLetter.ToString()) ? Manager.GetIQ<liteTerm>().Count(filter) : Manager.GetIQ<liteTerm>().Where(filter).Count(f => f.FirstLetter == firstLetter);
            records = list.Count();
            var listFirstLetters = list.Select(n => n.FirstLetter).Distinct().ToList();
            var temp = listFirstLetters.ToList().Select(item => item.ToString()).Distinct().ToList();
            words = new Dictionary<String, CharInfo>();
            foreach (var letter in temp)
                if (!words.ContainsKey(letter))
                    words.Add(letter, new CharInfo());

            if (!String.IsNullOrWhiteSpace(firstLetter.ToString()))
                list = list.Where(f => f.FirstLetter == firstLetter);

            if (currentPage > 0)
            {
                list = list.Skip(currentPage * pageSize - 1).ToList();
                if (pageSize > 0)
                    list = list.Take(pageSize + 2).ToList();
            }
            else if (pageSize > 0)
                list = list.Take(pageSize + 1).ToList();

            var dicGlossary = new Dictionary<Int64, String>();
            foreach (var idGlossary in list.Select(f => f.IdGlossary).Distinct())
            {
                try
                {
                    var gl = Manager.GetIQ<liteGlossary>().FirstOrDefault(f => f.Id == idGlossary);
                    dicGlossary.Add(idGlossary, gl.Name);
                }
                catch (Exception)
                {
                    dicGlossary.Add(idGlossary, String.Empty);
                }
            }

            var result = list.ToList().Select(f => new DTO_Term(f) { GlossaryName = dicGlossary[f.IdGlossary] });
            return result;
        }

        public IEnumerable<DTO_TermMap> GetDTO_TermListMapFromliteTerm(Expression<Func<liteTermMap, bool>> filter, Func<liteTermMap, object> @orderby, char firstLetter, GlossaryFilter filterSearch, out List<string> wordAlls)
        {
            var list = Manager.GetIQ<liteTermMap>().Where(filter).OrderBy(@orderby).ToList();

            //if (String.IsNullOrWhiteSpace(firstLetter.ToString()))
            //    list = Manager.GetIQ<liteTermMap>().Where(filter).OrderBy(@orderby).ToList();
            //else 
            //    list = Manager.GetIQ<liteTermMap>().Where(filter).Where(f => f.FirstLetter == firstLetter).OrderBy(@orderby).ToList();

            if (filterSearch != null)
            {
                if (!String.IsNullOrWhiteSpace(filterSearch.LemmaString))
                {
                    switch (filterSearch.LemmaSearchType)
                    {
                        case FilterTypeEnum.Contains:
                            list = list.Where(f => f.Name.ToLower().Contains(filterSearch.LemmaString)).ToList();
                            break;
                        case FilterTypeEnum.StartWith:
                            list = list.Where(f => f.Name.ToLower().StartsWith(filterSearch.LemmaString)).ToList();
                            break;
                        case FilterTypeEnum.EndWith:
                            list = list.Where(f => f.Name.ToLower().EndsWith(filterSearch.LemmaString)).ToList();
                            break;
                    }
                }

                //if (!String.IsNullOrWhiteSpace(filterSearch.LemmaContentString))
                //    list = list.Where(f => f.Description.ToLower() == filterSearch.LemmaContentString).ToList();

                switch (filterSearch.LemmaVisibilityType)
                {
                    case FilterVisibilityTypeEnum.Published:
                        list = list.Where(f => f.IsPublished).ToList();
                        break;
                    case FilterVisibilityTypeEnum.Unpublished:
                        list = list.Where(f => !f.IsPublished).ToList();
                        break;
                }
            }

            wordAlls = new List<string>();
            foreach (var letter in list.Select(f => f.FirstLetter))
                if (!wordAlls.Contains(letter.ToString()))
                    wordAlls.Add(letter.ToString());


            if (!String.IsNullOrWhiteSpace(firstLetter.ToString()))
                list = list.Where(f => f.FirstLetter == firstLetter).ToList();

            var result = list.Select(f => new DTO_TermMap(f));
            return result;
        }

        public IEnumerable<DTO_TermDelete> GetDTO_TermDelete(Expression<Func<liteTerm, bool>> filter)
        {
            var result = Manager.GetIQ<liteTerm>().Where(filter).ToList();
            var result2 = result.Select(item => new DTO_TermDelete(item.Id, item.Name, item.ModifiedOn, item.ModifiedBy)).ToList();
            return result2;
        }

        //public IOrderedEnumerable<liteTerm> GetTermListOE(Expression<Func<liteTerm, bool>> filter, Func<liteTerm, Object> orderby)
        //{
        //    var result = Manager.GetIQ<liteTerm>().Where(filter).OrderBy(orderby);
        //    return result;
        //}

        public bool DeleteVirtualTerm(Int64 idTerm, out String errors)
        {
            errors = String.Empty;
            Term item;
            if (idTerm <= 0)
            {
                item = new Term();
                item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                return true;
            }
            item = GetTerm(idTerm);
            item.SetDeleteMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    UpdateGlossaryTerms(item.IdGlossary);
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public bool DeleteTerm(Int64 idTerm, out String errors)
        {
            errors = String.Empty;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var term = GetTerm(idTerm);
                    Manager.DeletePhysical(term);

                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }

        public bool RestoreVirtualTerm(long idTerm, out string errors)
        {
            errors = String.Empty;
            if (idTerm <= 0)
                return false;
            var item = GetTerm(idTerm);

            item.SetDeleteMetaInfo(null, String.Empty, String.Empty, null);
            item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            item.UpdateMetaInfo(CurrentPerson, BaseStatusDeleted.None);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    return true;
                }
                catch (Exception e1)
                {
                    errors = e1.Message;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return false;
        }


        public void ChangePublishStateTerm(long idTerm, bool published)
        {
            Term item;
            if (idTerm <= 0)
                return;
            item = GetTerm(idTerm);
            item.IsPublished = published;
            item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    UpdateGlossaryTerms(item.IdGlossary);
                }
                catch (Exception e1)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
        }

        public SaveInfo SaveOrUpdateTerm(DTO_Term term)
        {
            var result = new SaveInfo { SaveState = SaveStateEnum.None, Id = term.Id };
            Term item;
            if (term.Id <= 0)
            {
                item = new Term();
                item.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                item.IdGlossary = term.IdGlossary;
            }
            else
            {
                item = GetTerm(term.Id);
                item.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            //Controllo per warning nome già inserito
            var counter = Manager.GetAll<liteTerm>(f => f.IdGlossary == term.IdGlossary && f.Id != term.Id && f.Name == term.Name && f.Deleted == BaseStatusDeleted.None).Count();
            result.ElementsWithSameName = counter;

            if (counter > 0)
                result.SaveState = SaveStateEnum.TermNameAlreadyExists;

            //Modifica parametri
            item.Name = term.Name;
            item.Description = term.Description;
            item.DescriptionText = term.DescriptionText;
            item.IdCommunity = term.IdCommunity;
            item.IsPublished = term.IsPublished;

            if (!String.IsNullOrWhiteSpace(item.Name))
            {
                //var letters = new List<char>();
                //for (var i = 97; i <= 122; i++)
                //    letters.Add((char)i);
                var currentFirstLetter = item.Name.ToLower()[0];
                if (char.IsLetter(currentFirstLetter))
                    item.FirstLetter = currentFirstLetter;
                else if (char.IsDigit(currentFirstLetter))
                    item.FirstLetter = '#';
                else
                    item.FirstLetter = '_';
            }

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                    UpdateGlossaryTerms(term.IdGlossary);
                    result.Id = item.Id;
                    result.SaveState = SaveStateEnum.Saved;
                    return result;
                }
                catch (Exception e1)
                {
                    result.Exception = e1;
                    result.SaveState |= SaveStateEnum.DbError;
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
            return result;
        }

        private void UpdateGlossaryTerms(long idGlossary)
        {
            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            if (Manager.IsInTransaction())
            {
                try
                {
                    var item = GetGlossary(idGlossary);

                    item.TermsCount = Manager.GetIQ<Term>().Count(f => f.IdGlossary == idGlossary && f.Deleted == BaseStatusDeleted.None);

                    Manager.SaveOrUpdate(item);
                    Manager.Commit();
                }
                catch (Exception e1)
                {
                    if (Manager.IsInTransaction())
                        Manager.RollBack();
                }
            }
        }

        #endregion

        #region initClass

        private Int32 idModule;

        public ServiceGlossary()
        {
        }

        public ServiceGlossary(iApplicationContext oContext)
            : base(oContext.DataContext)
        {
            _Context = oContext;
            Manager = new BaseModuleManager(oContext.DataContext);
            UC = oContext.UserContext;
        }

        public ServiceGlossary(iDataContext oDC)
            : base(oDC)
        {
            Manager = new BaseModuleManager(oDC);
            _Context = new ApplicationContext { DataContext = oDC };
        }

        #endregion
    }
}