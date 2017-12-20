using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using lm.Comol.Core.BaseModules.Tickets.Domain.Enums;
using NHibernate.Mapping;

using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.ScormStat;


namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        #region Save/Update

        /// <summary>
        /// Creazione di una category.
        /// </summary>
        /// <param name="Name">Nome generico, associato ANCHE a "multi"</param>
        /// <param name="Description">Descrioption generica, associato ANCHE a "multi"</param>
        /// <param name="Type"></param>
        /// <returns></returns>
        /// <remarks>Unica funzione realmente utilizzata</remarks>
        public Int64 CategoryCreateSimple(
            String Name,
            String Description,
            Domain.Enums.CategoryType Type)
        {
            Category cat = new Category();
            cat.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            cat.Name = Name;
            cat.Description = Description;
            cat.Type = Type;
            cat.IdCommunity = UC.CurrentCommunityID;

            cat.Translations = new List<CategoryTranslation>();
            CategoryTranslation ct = new CategoryTranslation();
            ct.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            ct.Category = cat;
            ct.Description = Description;
            ct.LanguageId = 0;
            ct.Name = Name;
            ct.LanguageName = LangMultiCODE;
            ct.LanguageId = LangMultiID;
            ct.LanguageCode = LangMultiCODE;
            
            cat.Translations.Add(ct);
            cat.Order = CategoryGetUndeletedOrder();

            Manager.SaveOrUpdate<Category>(cat);

            CategoriesCacheSet();

            return cat.Id;
        }

        /// <summary>
        /// Modifica dati categoria
        /// </summary>
        /// <param name="CategoryId">ID categoria da modificare</param>
        /// <param name="Name">Nome, nella lingua indicata</param>
        /// <param name="Description">Description, nella lingua indicata</param>
        /// <param name="Type">Tipo categoria</param>
        /// <param name="Lang">Linga usata per Name e Description</param>
        /// <returns></returns>
        /// <remarks>
        /// Se lingua = MULTI, Name e Description saranno applicati ANCHE all'oggetto stesso.
        /// </remarks>
        public Domain.Enums.CategoryAddModifyError CategoryModify(
            Int64 CategoryId,
            String Name,
            String Description,
            Domain.Enums.CategoryType Type,
            lm.Comol.Core.DomainModel.Languages.LanguageItem Lang
            )
        {

            if (String.IsNullOrEmpty(Name))
                return Domain.Enums.CategoryAddModifyError.noName;

            if (CategoryId <= 0)
            {
                return Domain.Enums.CategoryAddModifyError.noData;
            }

            Category UpdateCategory = Manager.Get<Category>(CategoryId);
            if (UpdateCategory == null)
                return Domain.Enums.CategoryAddModifyError.noData;

            if (UpdateCategory.IsDefault && Type != Domain.Enums.CategoryType.Public)
            {
                //return Domain.Enums.CategoryAddModifyError.isDefault;
                Type = Domain.Enums.CategoryType.Public;
            }

            UpdateCategory.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if (Lang.Id == LangMultiID)
            {
                UpdateCategory.Name = Name;
                UpdateCategory.Description = Description;
            }
            UpdateCategory.Type = Type;

            CategoryTranslation UpdateTranslation = (from CategoryTranslation ct in UpdateCategory.Translations where ct.LanguageId == Lang.Id select ct).FirstOrDefault();

            if (UpdateTranslation == null)
            {
                UpdateTranslation = new CategoryTranslation()
                {
                    Category = UpdateCategory,
                    LanguageId = Lang.Id,
                    LanguageCode = Lang.Code,
                    LanguageName = Lang.Name,
                    Description = Description,
                    Name = Name
                };

                UpdateTranslation.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                UpdateCategory.Translations.Add(UpdateTranslation);
            }
            else
            {
                UpdateTranslation.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                UpdateTranslation.Name = Name;
                UpdateTranslation.Description = Description;
            }

            Manager.SaveOrUpdate<Category>(UpdateCategory);

            CategoriesCacheSet();

            return Domain.Enums.CategoryAddModifyError.none;
        }

        #endregion

        #region Get - USER

        /// <summary>
        /// Recupera l'elenco PLAIN di categorie DELLA Comunità Corrente (solo quelle!)
        /// DALLA CATEGORIE INDICATA (indicata + sotto-categorie)
        /// per LISTA GESTIONE!
        /// </summary>
        /// <param name="Translate">Se tradurre o meno i vari termini</param>
        /// <param name="ShowDeleted">Se mostrare ANCHE le cancellate...</param>
        /// <returns>Lista di DTO con Id, Id Padre, etc...</returns>
        public IList<Domain.DTO.DTO_CategoryList> CategoriesGetFromCategory(bool Translate, int CommunityId, Int64 CategoryId)
        {
            if (CommunityId < 0)
                CommunityId = UC.CurrentCommunityID;

            Category Cate = Manager.Get<Category>(CategoryId);
            if (Cate == null || Cate.IdCommunity != CommunityId)
                return null;


            IList<Domain.DTO.DTO_CategoryList> dtoCats = new List<Domain.DTO.DTO_CategoryList>();

            CategoriesGetPlain(ref dtoCats, Cate, false);

            return dtoCats;
        }


        /// <summary>
        /// Recupera l'elenco PLAIN di categorie DELLA Comunità Corrente (solo quelle!)
        /// per LISTA GESTIONE!
        /// </summary>
        /// <param name="Translate">Se tradurre o meno i vari termini</param>
        /// <param name="ShowDeleted">Se mostrare ANCHE le cancellate...</param>
        /// <returns>Lista di DTO con Id, Id Padre, etc...</returns>
        public IList<Domain.DTO.DTO_CategoryList> CategoriesGetCommunity(bool Translate, bool ShowDeleted, int CommunityId)
        {
            if (CommunityId < 0)
                CommunityId = UC.CurrentCommunityID;

            IList<Category> Categories = new List<Category>();
            IList<Category> CategoriesDeleted = new List<Category>();

            if (ShowDeleted)
            {
                //IList<Category> CategoriesAll = Manager.GetAll<Category>(ct => ct.IdCommunity == CommunityId && ((ct.Father == null && ct.Deleted != BaseStatusDeleted.None) || ct.Deleted != null)).OrderBy(c => c.Deleted).ThenBy(c => c.Order).ToList();
                //Categories = CategoriesAll.Where(ct => ct.Deleted == BaseStatusDeleted.None && ct.Father == null).ToList();
                //CategoriesDeleted = CategoriesAll.Where(ct => ct.Deleted != BaseStatusDeleted.None && ct.Father == null).ToList();

                Categories = Manager.GetAll<Category>(ct => ct.IdCommunity == CommunityId && ct.Father == null).OrderBy(c => c.Deleted).ThenBy(c => c.Order).ToList();
                
            }   
            else
                Categories = Manager.GetAll<Category>(ct => ct.IdCommunity == CommunityId && ct.Deleted == BaseStatusDeleted.None && ct.Father == null).OrderBy(c => c.Order).ToList();

            IList<Domain.DTO.DTO_CategoryList> dtoCats = new List<Domain.DTO.DTO_CategoryList>();
            //IList<Domain.DTO.DTO_CategoryList> dtoCatsDel = new List<Domain.DTO.DTO_CategoryList>();

            foreach (Category ct in Categories)
            {
                CategoriesGetPlain(ref dtoCats, ct, ShowDeleted);
            }

            //if(ShowDeleted && CategoriesDeleted != null && CategoriesDeleted.Any())
            //foreach (Category ct in CategoriesDeleted)
            //{
            //    CategoriesGetPlain(ref dtoCats, ct, true, false);
            //}

            return dtoCats;
        }

        /// <summary>
        /// X Ricorsione - SEMPRE tradotte
        /// </summary>
        /// <param name="dtoCats"></param>
        /// <param name="MainCat"></param>
        /// <param name="ShowDeleted"></param>
        /// <returns></returns>
        private IList<Domain.DTO.DTO_CategoryList> CategoriesGetPlain(
            ref IList<Domain.DTO.DTO_CategoryList> dtoCats, Category MainCat, bool ShowDeleted, bool recursive = true)
        {
            if (dtoCats == null)
            {
                dtoCats = new List<Domain.DTO.DTO_CategoryList>();
            }

            //Ora, le cancellate sono "doppie", presenti SIA nell'albero che in coda.
            //NECESSARIO togliere quelle in coda!

            if (MainCat != null)
            {
                Domain.DTO.DTO_CategoryList dtocat = new Domain.DTO.DTO_CategoryList();

                Category TransCat = CategoryTranslate(MainCat, this.LanguageIdCurrentUser);

                dtocat.Name = TransCat.Name;
                dtocat.Description = TransCat.Description;
                dtocat.Order = TransCat.Order;

                if (MainCat.Father != null)
                {
                    dtocat.FatherId = MainCat.Father.Id;
                }
                else
                {
                    dtocat.FatherId = 0;
                }
                dtocat.Id = MainCat.Id;
                dtocat.IsDeleted = (MainCat.Deleted != BaseStatusDeleted.None);

                dtocat.Users = new List<Domain.DTO.DTO_CategoryListUser>();

                //Controllare il caricamento di UserRoles, altrimenti caricarli...
                MainCat.UserRoles = Manager.GetAll<Domain.LK_UserCategory>(lkuc => lkuc.Category.Id == MainCat.Id);

                foreach (Domain.LK_UserCategory uc in MainCat.UserRoles)
                {
                    if (uc.User != null && uc.User.Person != null)
                    {
                        Domain.DTO.DTO_CategoryListUser clu = new Domain.DTO.DTO_CategoryListUser();
                        clu.DisplayName = uc.User.Person.SurnameAndName;
                        clu.UserId = uc.Id;
                        clu.IsManager = uc.IsManager;
                        if (uc.IsManager)
                            dtocat.ManagerNum++;
                        else
                            dtocat.ResolverNum++;

                        dtocat.Users.Add(clu);
                    }
                }
                //Aggiungo il dtoCat corrente
                dtoCats.Add(dtocat);

                //Poi faccio lo stesso per i vari figli
                if (recursive && !dtocat.IsDeleted && MainCat.Children != null && MainCat.Children.Count > 0)
                {
                    MainCat.Children = MainCat.Children.OrderBy(c => c.Deleted).ThenBy(c => c.Order).ToList();

                    foreach (Category cCat in MainCat.Children)
                    {
                        if (ShowDeleted | (cCat.Deleted == BaseStatusDeleted.None))
                            CategoriesGetPlain(ref dtoCats, cCat, ShowDeleted);
                    }
                }
            }
            return dtoCats.OrderByDescending(c => c.IsDeleted).ToList();
        }

        /// <summary>
        /// Recupera DTO ricorsivi per TREE Ordinamento
        /// </summary>
        /// <param name="Translate">SE tradurre i termini</param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoriesGetCommunityTree(bool Translate, Int32 CommunityId)
        {
            IList<Category> Categories;

            Categories = Manager.GetAll<Category>(ct => ct.IdCommunity == CommunityId && ct.Father == null && ct.Deleted == BaseStatusDeleted.None); // 

            IList<Domain.DTO.DTO_CategoryTree> dtoCats = new List<Domain.DTO.DTO_CategoryTree>();

            foreach (Category ct in Categories.Where(c=> c.Deleted == BaseStatusDeleted.None))
            {
                Domain.DTO.DTO_CategoryTree catTree = CategoriesGetDTOTree(ct, Translate);
                if(catTree != null)
                    dtoCats.Add(catTree);
            }

            return dtoCats;
        }

        /// <summary>
        /// Ricorsione per albero categorie
        /// </summary>
        /// <param name="Category">Categoria di partenza</param>
        /// <param name="Translate">Se tradurre i vari DTO</param>
        /// <returns></returns>
        private Domain.DTO.DTO_CategoryTree CategoriesGetDTOTree(Category Category, bool Translate)
        {
            if (Category.Deleted != BaseStatusDeleted.None)
                return null;

            Domain.DTO.DTO_CategoryTree CT = new Domain.DTO.DTO_CategoryTree();
            CT.Id = Category.Id;
            CT.Order = Category.Order;
            CT.IsDefault = Category.IsDefault;
            //CT.IsDeleted = (Category.Deleted == BaseStatusDeleted.None) ? false : true;

            if (Translate)
            {
                Category TransCat = CategoryTranslate(Category, LanguageIdCurrentUser);
                CT.Name = TransCat.Name;
                CT.Description = TransCat.Description;
            }
            else
            {
                CT.Name = Category.Name;
                CT.Description = Category.Description;
            }

            if (Category.Children != null && Category.Children.Count() > 0)
            {
                Category.Children = Category.Children.OrderBy(c => c.Order).ToList();

                foreach (Domain.Category cat in Category.Children)
                {
                    Domain.DTO.DTO_CategoryTree childTree = CategoriesGetDTOTree(cat, Translate);
                    if(childTree != null)
                        CT.Children.Add(childTree);
                }
            }

            return CT;
        }

        /// <summary>
        /// Recupera categorieper DDL da una singola categoria (x riassegnazione)
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryDDLItem> CategoryGetDDL(Int64 CategoryId)
        {
            IList<Domain.DTO.DTO_CategoryDDLItem> dtoCats = new List<Domain.DTO.DTO_CategoryDDLItem>();


            Category Category = this.CategoryGetDetached(CategoryId, Domain.Enums.RolesLoad.none, true);

            if (Category != null && Category.Id <= 0)
            {
                dtoCats = CategoriesGetPlainDDL(ref dtoCats, Category);
            }

            return dtoCats;
        }

        /// <summary>
        /// Recupera l'elenco categorie per DDL escluse quelle indicate (x riassegnazione)
        /// </summary>
        /// <param name="ExcludedCategory"></param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryDDLItem> CategoryGetCommunityDDL(Int64 ExcludedCategoryId)
        {
            IList<Domain.DTO.DTO_CategoryDDLItem> dtoCats = new List<Domain.DTO.DTO_CategoryDDLItem>();

            IList<Category> Categories = Manager.GetAll<Category>(ct => ct.IdCommunity == UC.CurrentCommunityID && ct.Deleted == BaseStatusDeleted.None && ct.Father == null);

            foreach (Category ct in Categories)
            {
                if (ct.Id != ExcludedCategoryId)
                    dtoCats = CategoriesGetPlainDDL(ref dtoCats, ct);
            }

            return dtoCats;
        }

        /// <summary>
        /// Ricorsione per DDL categorie x cancellazione
        /// </summary>
        /// <param name="dtoCats"></param>
        /// <param name="MainCat"></param>
        /// <returns></returns>
        private IList<Domain.DTO.DTO_CategoryDDLItem> CategoriesGetPlainDDL(
                ref IList<Domain.DTO.DTO_CategoryDDLItem> dtoCats, Category MainCat)
        {
            if (dtoCats == null)
            {
                dtoCats = new List<Domain.DTO.DTO_CategoryDDLItem>();
            }

            if (MainCat != null)
            {
                Category ctrans = this.CategoryTranslate(MainCat, this.LanguageIdCurrentUser);

                dtoCats.Add(new Domain.DTO.DTO_CategoryDDLItem() { Id = MainCat.Id, Name = ctrans.Name, Description = ctrans.Description, FatherId = 0 });
            }

            foreach (Category ct in MainCat.Children.OrderBy(c => c.Order))
            {
                dtoCats = CategoriesGetPlainDDL(ref dtoCats, ct);
            }

            return dtoCats;
        }

        /// <summary>
        /// Recupera un singolo elemento DTO_CategoryTree, SENZA FIGLI
        /// </summary>
        /// <param name="CommunityId"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public Domain.DTO.DTO_CategoryTree CategoryGetTreeDLLSingle(Int32 CommunityId, Int64 CategoryId)
        {
            liteCategoryTreeItem lCat = (from liteCategoryTreeItem lcti in this.CategoriesCachedGet()
                                         where lcti.IdCommunity == CommunityId
                                         && lcti.Id == CategoryId
                                         select lcti).FirstOrDefault();

            if (lCat == null || lCat.Id <= 0)
                return null;

            Domain.DTO.DTO_CategoryTree CatDDL = new Domain.DTO.DTO_CategoryTree();

            CatDDL.Id = lCat.Id;
            CatDDL.Order = lCat.Order;
            CatDDL.Name = lCat.GetTranslatedName(this.LanguageIdCurrentUser); //ctrans.Name;
            CatDDL.Description = lCat.GetTranslatedDescription(this.LanguageIdCurrentUser);
            CatDDL.IsSelectable = true;
            CatDDL.Children = new List<Domain.DTO.DTO_CategoryTree>();

            return CatDDL;
        }

        /// <summary>
        /// Recupera un elenco DTO_CategoryTree con i relativi figli
        /// </summary>
        /// <param name="CommunityId"></param>
        /// <param name="CategoryId"></param>
        /// <param name="Recursive"></param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoriesGetTreeDLLForDelete(Int32 CommunityId, Int64 CategoryId, bool Recursive)
        {
            if (CommunityId < 0)
                CommunityId = UC.CurrentCommunityID;

            IList<Int64> CategoriesIds = new List<Int64>();
            CategoriesIds.Add(CategoryId);

            if (Recursive)
            {
                CategoriesIds = CategoryGetFamilyIds(CategoriesIds);
            }

            //Necessario recuperare:
            //      - TUTTE le categorie PUBBLICHE
            //      - TUTTE le categorie "TICKET" se COMUNITA' TICKET
            //      - TUTTE le categorie della COMUNITA' CORRENTE
            //      - L'albero "completo" delle categorie di cui sono MANAGER (da MANAGER in GIU, SELEZIONABILI)
            //      - L'albero fino alle foglie delle categorie di cui sono RESOLVER (selezionabili solo RESOLVER)

            IList<Domain.DTO.DTO_CategoryTree> CatDDL = new List<Domain.DTO.DTO_CategoryTree>();


            IList<liteCategoryTreeItem> lCategories = (from liteCategoryTreeItem lcti in this.CategoriesCachedGet()
                                                       where lcti.IdCommunity == CommunityId
                                                       && !CategoriesIds.Contains(lcti.Id)
                                                       select lcti).ToList();


            if (lCategories != null && lCategories.Count() > 0)
            {
                foreach (liteCategoryTreeItem cat in lCategories)
                {
                    Domain.DTO.DTO_CategoryTree ct = null;
                    if (!CategoriesIds.Contains(cat.Id))
                        ct = CategoryGetDDLTreeItemDel(cat, CommunityId, CategoriesIds);

                    if (ct != null)
                    {
                        CatDDL.Add(ct);
                    }
                }
            }
            return CatDDL;

        }

        /// <summary>
        /// Per ricorsione su categorie
        /// </summary>
        /// <param name="lCat">Categoria corrente</param>
        /// <param name="LoadTicketCat">SE la comunità corrente PUO' vedere le categorie di tipo Ticket</param>
        /// <param name="ComId">ID comunità corrente</param>
        /// <param name="FatherManager">SE l'utente è MANAGER del padre (i figli verranno caricati senza alcun controllo)</param>
        /// <returns></returns>
        private Domain.DTO.DTO_CategoryTree CategoryGetDDLTreeItemDel(Domain.liteCategoryTreeItem lCat, Int32 ComId, IList<Int64> UnselectableIds)
        {

            Domain.DTO.DTO_CategoryTree CatDDL = new Domain.DTO.DTO_CategoryTree();

            CatDDL.Id = lCat.Id;
            CatDDL.Order = lCat.Order;
            CatDDL.Name = lCat.GetTranslatedName(this.LanguageIdCurrentUser); //ctrans.Name;
            CatDDL.Description = lCat.GetTranslatedDescription(this.LanguageIdCurrentUser);
            CatDDL.IsSelectable = true;
            CatDDL.Children = new List<Domain.DTO.DTO_CategoryTree>();

            if (lCat.Children != null && lCat.Children.Count() > 0)
            {
                foreach (liteCategoryTreeItem lchild in lCat.Children)
                {
                    if (!UnselectableIds.Contains(lchild.Id))
                    {
                        Domain.DTO.DTO_CategoryTree ct = CategoryGetDDLTreeItemDel(lchild, ComId, UnselectableIds);
                        if (ct != null)
                        {
                            CatDDL.Children.Add(ct);
                        }
                    }
                }
            }

            return CatDDL;
        }

        /// <summary>
        /// Recupera DTO per DDL categorie dalla categoria indicata con le sotto-categorie
        /// </summary>
        /// <param name="CommunityId"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoriesGetTreeDLL(Int32 CommunityId, Int64 CategoryId, Domain.Enums.CategoryTREEgetType getTreeType)
        {
            if (CommunityId < 0)
                CommunityId = UC.CurrentCommunityID;

            if (CategoryId <= 0)
                return CategoriesGetTreeDLL(CommunityId, getTreeType);

            //Necessario recuperare:
            //      - TUTTE le categorie PUBBLICHE
            //      - TUTTE le categorie "TICKET" se COMUNITA' TICKET
            //      - TUTTE le categorie della COMUNITA' CORRENTE
            //      - L'albero "completo" delle categorie di cui sono MANAGER (da MANAGER in GIU, SELEZIONABILI)
            //      - L'albero fino alle foglie delle categorie di cui sono RESOLVER (selezionabili solo RESOLVER)

            IList<Domain.DTO.DTO_CategoryTree> CatDDL = new List<Domain.DTO.DTO_CategoryTree>();

            Boolean IsTicketCommunity = CommunityViewTicket(CommunityId);

            liteCategoryTreeItem lCategory = this.CategoriesCachedGet().Where(lcc => lcc.Id == CategoryId).FirstOrDefault();

            if (lCategory != null)
            {
                if (lCategory.IdCommunity != CommunityId)
                    return null;

                Domain.DTO.DTO_CategoryTree ct = CategoryGetDdlTreeItem(lCategory, IsTicketCommunity, CommunityId, false, getTreeType);
                if (ct != null)
                {
                    CatDDL.Add(ct);
                }
            }
            return CatDDL;
        }

        /// <summary>
        /// Recupera DTO per DDL categorie
        /// </summary>
        /// <param name="CommunityId">ID Comunità. NOTA: -1 = comunità corrente, 0 portale?</param>
        /// <param name="OnlySystem">Indica di caricare "solo" gli elementi di sistema, ignorando le categoria in cui sono manager o resolver: per la creazione o le impostazioni di Sistema.</param>
        /// <returns>Lista ad albero per DDL categorie</returns>
        /// <remarks>
        /// Verranno recuperate TUTTE le categorie pubbliche,
        /// quelle della comunità indicata
        /// e tutte quelle in cui l'utente è Manager o Resolver e relativi rami
        /// </remarks>
        public IList<Domain.DTO.DTO_CategoryTree> CategoriesGetTreeDLL(Int32 CommunityId, Domain.Enums.CategoryTREEgetType getTreeType)
        {
            if (CommunityId < 0)
                CommunityId = UC.CurrentCommunityID;

            //Necessario recuperare:
            //      - TUTTE le categorie PUBBLICHE
            //      - TUTTE le categorie "TICKET" se COMUNITA' TICKET
            //      - TUTTE le categorie della COMUNITA' CORRENTE
            //      - L'albero "completo" delle categorie di cui sono MANAGER (da MANAGER in GIU, SELEZIONABILI)
            //      - L'albero fino alle foglie delle categorie di cui sono RESOLVER (selezionabili solo RESOLVER)
            bool IsTicketCommunity = false;


            //if (!OnlySystem && CommunityId > 0)
            if (getTreeType != CategoryTREEgetType.System && CommunityId > 0)
            {
                IsTicketCommunity = this.CommunityViewTicket(CommunityId);
            }

            IList<Domain.DTO.DTO_CategoryTree> CatDDL = new List<Domain.DTO.DTO_CategoryTree>();

            IList<liteCategoryTreeItem> lCategories = this.CategoriesCachedGet();

            if (lCategories != null && lCategories.Count() > 0)
            {
                foreach (liteCategoryTreeItem cat in lCategories)
                {
                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDdlTreeItem(cat, IsTicketCommunity, CommunityId, false, getTreeType);
                    if (ct != null)
                    {
                        CatDDL.Add(ct);
                    }

                }
            }
            return CatDDL;
        }

        /// <summary>
        /// Per ricorsione su categorie
        /// </summary>
        /// <param name="lCat">Categoria corrente</param>
        /// <param name="LoadTicketCat">SE la comunità corrente PUO' vedere le categorie di tipo Ticket</param>
        /// <param name="ComId">ID comunità corrente</param>
        /// <param name="FatherManager">SE l'utente è MANAGER del padre (i figli verranno caricati senza alcun controllo)</param>
        /// <param name="getTreeType">Indica l'ambito in cui mi serve caricare le categorie</param>
        /// <returns></returns>
        private Domain.DTO.DTO_CategoryTree CategoryGetDdlTreeItem(Domain.liteCategoryTreeItem lCat, Boolean LoadTicketCat, Int32 ComId, Boolean FatherManager, Domain.Enums.CategoryTREEgetType getTreeType)
        {
            if (lCat.Deleted != BaseStatusDeleted.None)
                return null;

            //Se NON E' comunità corrente
            //              E
            // Non devo caricare Ticket ed è tipo Ticket 
            //              E
            // NON SONO Manager/Resolver
            //              E
            // NON CI SONO FIGLI da controllare:
            //
            // NON restituisco nulla!!!

            //Boolean CheckChilden = false;   // <-- Mi dice che NON avrei pemessi per accedere alla categoria, MA è necessario controllare i FIGLI per la SOLA visualizzazione!

            Boolean IsManager = false;
            Boolean IsManRes = false; //

            //PEr filtri Manager/REsolver, controllo se lo sono
            if (getTreeType == CategoryTREEgetType.FilterManager)
            {
                if (this.CategoryIdManRes.ContainsKey(lCat.Id))
                {
                    IsManager = this.CategoryIdManRes[lCat.Id];
                    IsManRes = true;
                }
            }


            Boolean Selectable = IsManRes || FatherManager;
            Boolean Visible = IsManRes || FatherManager;

            //Non sono MANAGER di questa categoria o di un suo padre (o sys o creazione)
            if (!(Selectable))
            {
                if (lCat.Type == Domain.Enums.CategoryType.Public)
                {
                    Visible = true;
                    Selectable = true;
                }
                else if ((lCat.Type == Domain.Enums.CategoryType.Current && lCat.IdCommunity == ComId) ||
                    (lCat.Type == Domain.Enums.CategoryType.Ticket && LoadTicketCat))
                {
                    Visible = true;
                    Selectable = true;
                }
            }
            
            Domain.DTO.DTO_CategoryTree CatDDL = new Domain.DTO.DTO_CategoryTree();

            CatDDL.Id = lCat.Id;
            CatDDL.Order = lCat.Order;
            CatDDL.Name = lCat.GetTranslatedName(this.LanguageIdCurrentUser);
            CatDDL.Description = lCat.GetTranslatedDescription(this.LanguageIdCurrentUser);
            //CatDDL.IsSelectable = true;
            CatDDL.Children = new List<Domain.DTO.DTO_CategoryTree>();

            if (lCat.Children != null && lCat.Children.Count() > 0)
            {
                foreach (liteCategoryTreeItem lchild in lCat.Children)
                {
                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDdlTreeItem(lchild, LoadTicketCat, ComId, FatherManager || IsManager, getTreeType);
                    if (ct != null)
                    {
                        CatDDL.Children.Add(ct);
                    }
                }
            }

            if (CatDDL.Children != null && CatDDL.Children.Any())
            {
                Visible = true;
            }


            if (!Visible)
                return null;

            CatDDL.IsSelectable = Selectable;

            return CatDDL;
        }

        /// <summary>
        /// Recupera una CATEGORY - Detached -> NON TRADUCE I FIGLI!!!!
        /// Per modifica singola categoria
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LoadRoles"></param>
        /// <param name="Translate"></param>
        /// <returns></returns>
        public Domain.Category CategoryGetDetached(Int64 Id, Domain.Enums.RolesLoad LoadRoles, Boolean Translate)
        {
            Domain.Category category = new Category();


            if (Id != null && Id >= 0)
            {
                category = Manager.Get<Domain.Category>(Id);
                Manager.Detach<Category>(category);
            }

            if (category != null && category.Id > 0)
            {
                switch (LoadRoles)
                {
                    case Domain.Enums.RolesLoad.none:
                        category.UserRoles = null;
                        break;
                    case Domain.Enums.RolesLoad.all:
                        category.UserRoles = (from LK_UserCategory roles in Manager.GetAll<LK_UserCategory>(Lk => Lk.Category.Id == category.Id) orderby roles.User.Sname select roles).ToList();
                        break;
                    case Domain.Enums.RolesLoad.Manager:
                        category.UserRoles = (from LK_UserCategory roles in Manager.GetAll<LK_UserCategory>(Lk => Lk.Category.Id == category.Id && Lk.IsManager == true) orderby roles.User.Sname select roles).ToList();
                        break;
                    case Domain.Enums.RolesLoad.Resolver:
                        category.UserRoles = (from LK_UserCategory roles in Manager.GetAll<LK_UserCategory>(Lk => Lk.Category.Id == category.Id && Lk.IsManager == false) orderby roles.User.Sname select roles).ToList();
                        break;
                }

                if (Translate)
                {
                    category = CategoryTranslate(category, LanguageIdCurrentUser);
                }

            }
            else
                category = new Category();

            return category;
        }

        /// <summary>
        /// Recupera l'elenco categorie PUBBLICHE: per VISUALIZZAZIONE in GLOBAL ADMIN
        /// </summary>
        /// <returns></returns>
        public IList<Category> CategoriesGetSystem()
        {
            IList<Category> AllCategories = (from Category cat in Manager.GetIQ<Category>()
                                             where cat.Type == Domain.Enums.CategoryType.Public
                                             && cat.Father == null
                                             select cat).ToList();
            return AllCategories;
        }

        #endregion

        #region Get - Manager/Resolver

        /// <summary>
        /// Recupera oggetto per Tree categorie
        /// </summary>
        /// <param name="CurrentCateId">Id Categoria </param>
        /// <returns></returns>
        public Domain.DTO.DTO_CategoryTree CategoryGetDTOCatTree(Int64 CurrentCateId)
        {

            Domain.DTO.DTO_CategoryTree DTOct = new Domain.DTO.DTO_CategoryTree();
            DTOct.Id = -1;

            if (CurrentCateId > 0)
            {
                Category CurCate = Manager.Get<Category>(CurrentCateId);

                if (CurCate != null)
                {
                    DTOct.Description = CurCate.GetTranslatedDescription(UC.Language.Code);
                    DTOct.Id = CurCate.Id;
                    DTOct.Order = CurCate.Order;
                    DTOct.IsSelectable = true;
                    DTOct.Name = CurCate.GetTranslatedName(UC.Language.Code);
                    DTOct.Order = -1;
                }
            }

            return DTOct;
        }

        /// <summary>
        /// Recupera albero categorie della COMUNITA' CORRENTE (non usato!)
        /// </summary>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoryGetDDLManRes_ComCurrent()
        {
            return CategoryGetDDLManRes(UC.CurrentCommunityID, CurrentUser.Id);
        }

        /// <summary>
        /// Recupera albero categorie del PORTALE (non usato!)
        /// </summary>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoryGetDDLManRes_Portal()
        {
            return CategoryGetDDLManRes(0, CurrentUser.Id);
        }

        /// <summary>
        /// Recupera albero categorie dell'UTENTE CORRENTE
        /// </summary>
        /// <param name="CommunityId">Comunità Id (0 o meno per PORTAL)</param>
        /// <returns></returns>
        public IList<Domain.DTO.DTO_CategoryTree> CategoryGetDDLFilter_ManResCurrent(int CommunityId)
        {
            return CategoryGetDDLManRes(CommunityId, CurrentUser.Id);
        }

        public IList<Domain.DTO.DTO_CategoryTree> CategoryGetDDLManRes_Filters()
        {
            IList<Domain.DTO.DTO_CategoryTree> dtoCats = new List<Domain.DTO.DTO_CategoryTree>();

            //Categorie associate all'utente
            IList<Int64> CatIds = (from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                                   where lkuc.User != null && lkuc.User.Id == CurrentUser.Id && lkuc.Category != null
                    select lkuc.Category.Id).Distinct().ToList();

            //Comunità di quelle categorie
            IList<Int32> CommunityCatIds = (from Category cat in Manager.GetIQ<Category>()
                where CatIds.Contains(cat.Id)
                select cat.IdCommunity).Distinct().ToList();

            //bool comIsViewTicket = false;

            //foreach (Int32 ComId in CommunityCatId)
            //{
                
            //}

            

            IList<liteCategoryTreeItem> Categories = CategoriesCachedGet().Where(ct => CommunityCatIds.Contains(ct.IdCommunity)).ToList();

            if (Categories != null && Categories.Count() > 0)
            {
                foreach (liteCategoryTreeItem cat in Categories)
                {
                    //Boolean LaodChilds = CategoryIdManRes.ContainsKey(cat.Id) && CategoryIdManRes[cat.Id];

                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDDLTreeItemManRes_Filters(cat);
                    if (ct != null)
                    {
                        dtoCats.Add(ct);
                    }
                }
            }
            return dtoCats;
        }

        /// <summary>
        /// Recupera DDL categorie per Admin/Resolver - Usata internamente
        /// </summary>
        /// <param name="CommunityId">Comunità Id (0 o meno per PORTAL)</param>
        /// <param name="UserId">Id Utente. Se minore di 0, non ritorno nulla.</param>
        /// <returns></returns>
        /// <remarks> CommunityId -1 corrisponde a PORTAL!</remarks>
        public IList<Domain.DTO.DTO_CategoryTree> CategoryGetDDLManRes(int CommunityId, Int64 UserId)
        {
            IList<Domain.DTO.DTO_CategoryTree> dtoCats = new List<Domain.DTO.DTO_CategoryTree>();
            IList<liteCategoryTreeItem> Categories = CategoriesCachedGet();

            Boolean comIsViewTicket = CommunityViewTicket(CommunityId);

            if (Categories != null && Categories.Count() > 0)
            {
                foreach (liteCategoryTreeItem cat in Categories)
                {
                    Boolean LaodChilds = CategoryIdManRes.ContainsKey(cat.Id) && CategoryIdManRes[cat.Id];

                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDDLTreeItemManRes(cat, comIsViewTicket, CommunityId, LaodChilds);
                    if (ct != null)
                    {
                        dtoCats.Add(ct);
                    }
                }
            }
            return dtoCats;
        }

        /// <summary>
        /// Ricorsione recupero DDL categorie per manager/Resolver
        /// </summary>
        /// <param name="Cat"></param>
        /// <param name="LoadTicketCat">Carica ANCHE le categorie di tipo Ticket</param>
        /// <param name="ComId"></param>
        /// <returns></returns>
        private Domain.DTO.DTO_CategoryTree CategoryGetDDLTreeItemManRes(Domain.liteCategoryTreeItem Cat, Boolean LoadTicketCat, Int32 ComId, Boolean LaodChilds)
        {

            if (Cat.Deleted != BaseStatusDeleted.None)
                return null;

            Domain.DTO.DTO_CategoryTree CTOut = new Domain.DTO.DTO_CategoryTree();
            CTOut.IsSelectable = true;

            CTOut.Children = new List<Domain.DTO.DTO_CategoryTree>();

            //Se nei padri o in quella corrente sono MANGER
            LaodChilds = LaodChilds || (CategoryIdManRes.ContainsKey(Cat.Id) && CategoryIdManRes[Cat.Id]);

            if (Cat.Children != null && Cat.Children.Count() > 0)
            {
                foreach (liteCategoryTreeItem child in Cat.Children)
                {
                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDDLTreeItemManRes(child, LoadTicketCat, ComId, LaodChilds);
                    if (ct != null)
                    {
                        CTOut.Children.Add(ct);
                    }
                }
            }

            //SE non ho figli e non sono manres della cate corrente, ritorno null!!!
            if (CategoryIdManRes.ContainsKey(Cat.Id)
                || !LoadTicketCat && Cat.Type == Domain.Enums.CategoryType.Ticket
                || (Cat.Type == Domain.Enums.CategoryType.Current && Cat.IdCommunity != ComId)
                )
            {
                if (!CTOut.Children.Any())
                {
                    return null;
                }
                else
                {
                    CTOut.IsSelectable = false;
                }
            }   

            //Altrimenti imposto i parametri per il corrente e lo ritorno.
            CTOut.Id = Cat.Id;
            CTOut.Order = Cat.Order;
            CTOut.Name = Cat.GetTranslatedName(LanguageIdCurrentUser);
            CTOut.Description = Cat.GetTranslatedDescription(LanguageIdCurrentUser);


            return CTOut;
        }

        private Domain.DTO.DTO_CategoryTree CategoryGetDDLTreeItemManRes_Filters(Domain.liteCategoryTreeItem Cat)
        {

            if (Cat.Deleted != BaseStatusDeleted.None)
                return null;

            Domain.DTO.DTO_CategoryTree CTOut = new Domain.DTO.DTO_CategoryTree();
            CTOut.IsSelectable = true;

            CTOut.Children = new List<Domain.DTO.DTO_CategoryTree>();

            //Se nei padri o in quella corrente sono MANGER
            //LaodChilds = LaodChilds || (CategoryIdManRes.ContainsKey(Cat.Id) && CategoryIdManRes[Cat.Id]);

            if (Cat.Children != null && Cat.Children.Count() > 0)
            {
                foreach (liteCategoryTreeItem child in Cat.Children)
                {
                    Domain.DTO.DTO_CategoryTree ct = CategoryGetDDLTreeItemManRes_Filters(child);//, LaodChilds);
                    if (ct != null)
                    {
                        CTOut.Children.Add(ct);
                    }
                }
            }

            //SE non ho figli e non sono manres della cate corrente, ritorno null!!!
            if (!CategoryIdManRes.ContainsKey(Cat.Id))
            {
                if (!CTOut.Children.Any())
                {
                    return null;
                }
                else
                {
                    CTOut.IsSelectable = false;
                }
            }

            //Altrimenti imposto i parametri per il corrente e lo ritorno.
            CTOut.Id = Cat.Id;
            CTOut.Order = Cat.Order;
            CTOut.Name = Cat.GetTranslatedName(LanguageIdCurrentUser);
            CTOut.Description = Cat.GetTranslatedDescription(LanguageIdCurrentUser);


            return CTOut;
        }


        /// <summary>
        /// Recupera un dictionary con chiave Id Categoria e valore True per Manager, False per Resolver
        /// </summary>
        /// <returns>IL DICTIONARY</returns>
        /// <remarks>
        /// NOTA: SU QUESTO OGGETTO SI BASANO TUTTE LE FUNZIONI:
        /// - Permessi accesso ai ticket
        /// - Recupero elenchi di categorie per i vari utenti: creazione, filtri liste ticket!
        /// </remarks>
        public IDictionary<Int64, Boolean> CategoryIdManRes
        {
            get
            {
                if (_CategoryIdManRes != null)
                {
                    return _CategoryIdManRes;
                }

                IList<Int64> CatIdMan = CategoriesIdGetManager(CurrentUser.Id);
                IDictionary<Int64, bool> DICT_TkIdMan = CatIdMan.Distinct().ToDictionary(k => k, v => true);

                IList<Int64> CatIdRes = CategoriesIdGetResolver(CurrentUser.Id).Except(CatIdMan).ToList();

                foreach (Int64 id in CatIdRes)
                {
                    DICT_TkIdMan.Add(id, false);
                }

                _CategoryIdManRes = DICT_TkIdMan;

                return _CategoryIdManRes;
            }
        }
        private IDictionary<Int64, Boolean> _CategoryIdManRes;

        /// <summary>
        /// Recupera gli Id Categoria con Sottocategorie PLAIN di cui User sono manager
        /// </summary>
        /// <param name="UserId">Id dell'utente</param>
        /// <returns></returns>
        private List<long> CategoriesIdGetManager(Int64 UserId)
        {
            if (_TmpCategoriesIdGetManagerALL == null)
            {
                _TmpCategoriesIdGetManagerALL = CategoriesIdGetManager(UserId, -1);
            }
            return _TmpCategoriesIdGetManagerALL;
        }
        private List<long> _TmpCategoriesIdGetManagerALL;


        private Int64 _CurSelCate = -10;

        /// <summary>
        /// Recupera gli Id Categoria con Sottocategorie PLAIN di cui User sono manager dalla categoria indicata
        /// </summary>
        /// <param name="UserId">Id Utente</param>
        /// <param name="CategoryId">Id Categoria partenza (0 o meno = TUTTE)</param>
        /// <returns></returns>
        /// <remarks>
        /// 2" in meno... Si potrebbe fare meglio...
        /// METTERE IN CACHE!
        /// </remarks>
        private List<long> CategoriesIdGetManager(Int64 UserId, Int64 CategoryId)
        {
            if(_CategoriesIdGetManager == null || _CurSelCate != CategoryId)
            {
                _CurSelCate = CategoryId;
                List<Int64> ManagerCategoriesId = new List<long>();

                IList<Int64> MANCateID;

                if (CategoryId > 0)
                {
                    //MANCateID =
                    //(from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                    // where lkuc.User != null && lkuc.User.Id == UserId &&
                    // lkuc.IsManager == true && lkuc.Category != null && lkuc.Category.Id == CategoryId
                    // select lkuc.Category.Id).ToList();

                    //bool IsCatMang = Manager.GetAll<liteLK_UserCategory>(llk => llk.IdCategory != null && llk.IdCategory == CategoryId).Any();

                    bool IsCatMng = (from liteLK_UserCategory llk in Manager.GetIQ<liteLK_UserCategory>() where llk.IdCategory != null && llk.IdCategory == CategoryId select llk.Id).Any();

                    if(IsCatMng)
                    {
                        MANCateID = new List<Int64>();
                        MANCateID.Add(CategoryId);
                    }
                    else
                    {
                        MANCateID = null;
                    }
                    
                
                }
                else
                {
                    //MANCateID =
                    //(from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                    // where lkuc.User != null && lkuc.User.Id == UserId &&
                    //  lkuc.IsManager == true && lkuc.Category != null
                    // select lkuc.Category.Id).Distinct().ToList();

                    MANCateID =
                    (from liteLK_UserCategory lkuc in CategoryGETALLlk_UserCategory()
                     where lkuc.IdUser != null && (Int64)lkuc.IdUser == UserId
                        && lkuc.IsManager == true
                        && lkuc.IdCategory != null
                     select (Int64)lkuc.IdCategory).Distinct().ToList();
                
                }

                //DateTime TEST_Start = DateTime.Now;

            
                if (MANCateID != null && MANCateID.Count() > 0)
                {
                    foreach (Int64 ctId in MANCateID)
                    {

                        //liteCategoryId ct = Manager.Get<liteCategoryId>(ctId);
                        liteCategoryId ct = CategoryGetAll_LiteCategoryId().Where(c => c.Id == ctId).FirstOrDefault();
                        CategoriesIdGetFlat(ref ManagerCategoriesId, ct);
                    }
                }

                //TimeSpan TestDuration = DateTime.Now - TEST_Start;
                //TOTALE: 2.8" (Mille "GetSingoli" ricorsivi)       OTTIMIZZARE!!!
                //Totale: 0.8" (Con get unico di tutto, una sola volta) no ottimale, ma meglio...


                if (ManagerCategoriesId != null && ManagerCategoriesId.Count() > 0)
                    ManagerCategoriesId = ManagerCategoriesId.Distinct().ToList();
                else if (ManagerCategoriesId == null)
                    ManagerCategoriesId = new List<long>();

                _CategoriesIdGetManager = ManagerCategoriesId;
            }

            return _CategoriesIdGetManager;
        }

        /// <summary>
        /// Categorie di cui l'utente è manager...
        /// ...eventualmente mettere in cache con chiave sull'utente.
        /// </summary>
        private List<long> _CategoriesIdGetManager { get; set; }

        private IList<liteCategoryId> _CategoryAll_LiteCategoryId { get; set; }
        private IList<liteCategoryId> CategoryGetAll_LiteCategoryId()
        {
            if(_CategoryAll_LiteCategoryId == null)
            {
                _CategoryAll_LiteCategoryId = Manager.GetAll<liteCategoryId>();
            }

            return _CategoryAll_LiteCategoryId;
        }

        private IList<liteLK_UserCategory> _CategoryALLlk_UserCategory { get; set; }
        private IList<liteLK_UserCategory> CategoryGETALLlk_UserCategory()
        {

            if (_CategoryALLlk_UserCategory == null)
            {
                _CategoryALLlk_UserCategory = Manager.GetAll<liteLK_UserCategory>();
            }
            return _CategoryALLlk_UserCategory;
        }

        /// <summary>
        /// Recupera gli id di tutte le categorie a cui un utente è assegnato come Resolver
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private List<long> CategoriesIdGetResolver(Int64 UserId)
        {
            return CategoriesIdGetResolver(-1, UserId);
        }

        /// <summary>
        /// Recupera gli id della categoria indicata, se l'utente ne è resolver...
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private List<long> CategoriesIdGetResolver(Int64 CategoryId, Int64 UserId)
        {
            IList<Int64> ResCate;

            if (CategoryId > 0)
            {
                ResCate =
                    (from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                     where
                         lkuc.User != null
                         && lkuc.User.Id == UserId
                         && lkuc.IsManager == false
                         && lkuc.Category != null
                         && lkuc.Category.Id == CategoryId
                     select lkuc.Category.Id
                        ).ToList();
            }
            else
            {
                ResCate =
                    (from LK_UserCategory lkuc in Manager.GetIQ<LK_UserCategory>()
                     where
                         lkuc.User != null
                         && lkuc.User.Id == UserId
                         && lkuc.IsManager == false
                         && lkuc.Category != null
                     select lkuc.Category.Id).ToList();
            }

            return ResCate.Distinct().ToList();
        }

        /// <summary>
        /// Recupera Id sottocategorie (RICORSIVA)
        /// </summary>
        /// <param name="OutCate">ref Id categorie a cui aggiungere gli id categoria correnti</param>
        /// <param name="InCate">Categorie corrente di cui analizzare le sottocategorie</param>
        private void CategoriesIdGetFlat(
            ref List<Int64> OutCate,
            liteCategoryId InCate
            )
        {

            if (OutCate == null)
                OutCate = new List<Int64>();

            if (InCate != null)
            {
                OutCate.Add(InCate.Id);

                if (InCate.Childrens != null)
                {
                    foreach (liteCategoryId ct in InCate.Childrens)
                    {
                        CategoriesIdGetFlat(ref OutCate, ct);
                    }
                }
            }
        }

        /// <summary>
        /// Riordino categorie
        /// </summary>
        /// <param name="CategoriesReorder"></param>
        /// <returns></returns>
        public Domain.Enums.CategoryReorderResponse CategoryReorder(IList<liteCategoryReorderItem> CategoriesReorder)
        {
            Domain.Enums.CategoryReorderResponse response = Domain.Enums.CategoryReorderResponse.Error;

            bool hasDefaultReordered = false;
            int CommunityOrder = CategoryGetCommunityOrder();

            Int64 DefCategoryId = this.CategoryDefaultGetID();

            CategoriesReorder = CategoriesReorderCheck(CategoriesReorder, DefCategoryId, ref hasDefaultReordered);

            IList<liteCategoryReorderItem> Categories = new List<liteCategoryReorderItem>();

            foreach (liteCategoryReorderItem CatItem in CategoriesReorder)
            {
                liteCategoryReorderItem ltCat = Manager.Get<liteCategoryReorderItem>(CatItem.Id);
                if (ltCat != null)// && ltCat.Id != DefCategoryId)
                {
                    ltCat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                    ltCat.Order = CatItem.Order + ((CatItem.DefCateFamily) ? 0 : CommunityOrder);
                    ltCat.FatherId = CatItem.FatherId;
                    Categories.Add(ltCat);
                } 
            }

            if (Categories != null && Categories.Count > 0)
            {
                Manager.SaveOrUpdateList<liteCategoryReorderItem>(Categories);
                response = Domain.Enums.CategoryReorderResponse.Success;
            }

            if (hasDefaultReordered)
            {
                response = Domain.Enums.CategoryReorderResponse.NoDefaultReorder;

            }

            return response;
        }

        private IList<liteCategoryReorderItem> CategoriesReorderCheck(IList<liteCategoryReorderItem> CategoriesOrdItems, Int64 DefCateId, ref bool Changed)
        {
            Changed = false;

            liteCategoryReorderItem Def_lcri = (from liteCategoryReorderItem lcri in CategoriesOrdItems where lcri.Id == DefCateId select lcri).FirstOrDefault();

            if(Def_lcri == null)
                return CategoriesOrdItems;


            if (Def_lcri.Order != 1)
                Changed = true;
            //    return CategoriesOrdItems;


            IList<liteCategoryReorderItem> DefFamilyOrder = new List<liteCategoryReorderItem>();
            IList<liteCategoryReorderItem> OtherCategoryOrder = new List<liteCategoryReorderItem>();

            //int order = 1;
            IList<Int64> DefFamilyId = new List<Int64>();
            DefFamilyId.Add(DefCateId);

            foreach(liteCategoryReorderItem _lcri in CategoriesOrdItems)
            {
                if(_lcri.Id == DefCateId)
                {
                    _lcri.DefCateFamily = true;
                    DefFamilyOrder.Add(_lcri);
                    
                }
                else if (_lcri.FatherId != null && DefFamilyId.Contains((Int64)_lcri.FatherId))
                {
                    _lcri.DefCateFamily = true;
                    DefFamilyOrder.Add(_lcri);
                    DefFamilyId.Add(_lcri.Id);
                }
                else
                {
                    _lcri.DefCateFamily = false;
                    OtherCategoryOrder.Add(_lcri);
                }
            }

            CategoriesOrdItems = DefFamilyOrder.Union(OtherCategoryOrder).ToList();

            int i = 1;
            foreach (liteCategoryReorderItem _lcri in CategoriesOrdItems)
            {
                _lcri.Order = i++;
            }

            return CategoriesOrdItems;

        }


        #endregion

        #region Category Default set/reset/get ID

        public bool CategoryDefaultSet(Int64 CategoryId)
        {
            
            if (CategoryId == CategoryDefaultGetID())
                return true;

            if (!Manager.IsInTransaction())
                Manager.BeginTransaction();

            Category OldCat = (from Category ct in Manager.GetIQ<Category>() where ct.IsDefault == true select ct).FirstOrDefault();

            if (OldCat != null && OldCat.Id == CategoryId)
                return true;    //E' già categoria di default

            Category NewCat = Manager.Get<Category>(CategoryId);

            //La categoria non esiste
            if (NewCat == null)
                return false;

            
            NewCat.Father = null;
            NewCat.IsDefault = true;
            CategorySetChildenOrder(NewCat, CategoryGetCommunityOrder(NewCat.IdCommunity) * -1);


            //NewCat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            
            //if (NewCat.Order > 0)
            //    NewCat.Order = NewCat.Order * -1;
            //Manager.SaveOrUpdate<Category>(NewCat);

            if (OldCat != null)
            {
                //int ComCatOrder = CategoryGetCommunityOrder(OldCat.IdCommunity);

                //OldCat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                OldCat.IsDefault = false;

                CategorySetChildenOrder(OldCat, CategoryGetCommunityOrder(OldCat.IdCommunity));
                
            }

            try
            {
                Manager.Commit();
            }
            catch(Exception ex)
            {
                if(Manager.IsInTransaction())
                    Manager.RollBack();
                return false;
            }
            
            _CategoryDefaultId = CategoryId;
            CategoriesCacheSet();

            return true;

        }

        private void CategorySetChildenOrder(Category cat, int ComCatOrder)
        {
            if (cat == null)
                return;

            //if (cat.IsDefault)
            //{
            //    cat.Order = 0;
            //    //cat.IsDefault = false;
            //} 
            //else
            //{
            cat.Order = cat.Order + ComCatOrder;//
            //}
            
            cat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            if(cat.Children != null && cat.Children.Any())
            {
                foreach(Category CatChild in cat.Children)
                {
                    this.CategorySetChildenOrder(CatChild, ComCatOrder);
                }
            }

            Manager.SaveOrUpdate<Category>(cat);
        }


        public bool CategoryDefaultRemove()
        {
            Category Cat = (from Category ct in Manager.GetIQ<Category>() where ct.IsDefault == true select ct).FirstOrDefault();

            if (Cat == null)
                return true;


            Cat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            Cat.IsDefault = false;

            CategorySetChildenOrder(Cat, CategoryGetCommunityOrder(Cat.IdCommunity));
            
            //if (Cat.Order < 0)
            //    Cat.Order = Cat.Order * -1;

            //Manager.SaveOrUpdate<Category>(Cat);

            CategoriesCacheSet();

            return true;
        }

        #endregion

        #region Cancellazione/Recupero/Riordino

        ///// <summary>
        ///// Cancella categoria: SOLO LOGICA!
        ///// </summary>
        ///// <param name="CategoryId">Id categoria</param>
        ///// <returns></returns>
        ///// <remarks>
        ///// Usare funzioni complete, comprensive di riassegnazioni, invio messaggi, etc...
        ///// </remarks>
        //public Boolean CategoryDelete(Int64 CategoryId)
        //{
        //    Boolean Response = true;

        //    Category cat = Manager.Get<Category>(CategoryId);

        //    if (cat == null || cat.IsDefault)
        //        return false;

        //    cat.Order = CategoryGetDeletedOrder();
        //    cat.Deleted = BaseStatusDeleted.Manual;
        //    cat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

        //    Manager.SaveOrUpdate<Category>(cat);
        //    CategoriesCacheSet();

        //    return Response;
        //}

        /// <summary>
        /// Recupera categoria cancellata
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public Boolean CategoryRecover(Int64 CategoryId)
        {
            Boolean Response = true;

            if (!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }
            Category cat = Manager.Get<Category>(CategoryId);

            if (cat == null)
                return false;

            //SE recupero una categoria cancellata manualmente, la imposto in coda all'elenco.
            if (cat.Deleted == BaseStatusDeleted.Manual)
                cat.Order = CategoryGetUndeletedOrder();

            cat.Deleted = BaseStatusDeleted.None;
            CategoryRecoverRecursive(cat.Children);

            //cat.Order = this.CategoryGetUndeletedOrder();

            //if (CheckFatherDelete(cat))
            //{
            //    cat.Father = null;
            //}
            

            //if (cat.Father == null)
            //{
            //    cat.Order =
            //        Manager.GetAll<Domain.Category>(
            //            c =>
            //                c.IdCommunity == cat.IdCommunity
            //                && c.Father == null
            //                && c.Deleted == BaseStatusDeleted.None).Count();
            //}
            //else
            //{
            //    cat.Order =
            //        Manager.GetAll<Domain.Category>(
            //            c =>
            //                c.IdCommunity == cat.IdCommunity
            //                && c.Father != null
            //                && c.Father.Id == cat.Father.Id
            //                && c.Deleted == BaseStatusDeleted.None).Count();
            //}

            cat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            Manager.SaveOrUpdate<Category>(cat);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                Response = false;
            }


            CategoriesCacheSet();

            return Response;
        }

        private void CategoryRecoverRecursive(IList<Category> childrenCategories)
        {
            if (childrenCategories != null && childrenCategories.Any())
            {
                foreach(Category cat in childrenCategories)
                {
                    if (cat.Deleted != BaseStatusDeleted.Manual)
                    {
                        cat.Deleted = BaseStatusDeleted.None;
                        CategoryRecoverRecursive(cat.Children);
                    }
                }
            }
        }


        private bool CheckFatherDelete(Category Cat)
        {
            if (Cat.Father == null)
            {
                return false;
            } else if (Cat.Father.Deleted != BaseStatusDeleted.None)
            {
                return true;
            }
            else
            {
                return CheckFatherDelete(Cat.Father);
            }
        }

        /// <summary>
        /// Cancella la categoria spostando su di un livello le relative figlie
        /// </summary>
        /// <param name="CategoryId">ID Categoria da cancellare</param>
        /// <returns></returns>
        public Boolean CategoryDeletePutUp(Int64 CategoryId)
        {
            if (CategoryId == CategoryDefaultGetID())
                return false;

            return CategoryDeletePutUp(CategoryId, -1, "");
        }

        /// <summary>
        /// Cancella la categoria spostando su di un livello le relative figlie
        /// </summary>
        /// <param name="CategoryId">Id Categoria da cancellare</param>
        /// <param name="ReassignCategoryId">Categoria a cui riassegnare i ticket assegnati alla categoria da cancellare</param>
        /// <param name="Message">Messaggio di riassegnazione</param>
        /// <returns></returns>
        public Boolean CategoryDeletePutUp(Int64 CategoryId, Int64 ReassignCategoryId, String Message)
        {
            if (CategoryId == CategoryDefaultGetID())
                return false;

            //lista di figli "lite" (ID al posto dell'oggetto padre)
            IList<liteCategoryReorderItem> llCRI = Manager.GetAll<liteCategoryReorderItem>(cri => cri.FatherId == CategoryId);

            //ID padre della categoria corrente
            Int64? NewFatherId = Manager.Get<liteCategoryReorderItem>(CategoryId).FatherId;


            if (llCRI != null && llCRI.Any())
            {
                //a TUTTI i FIGLI assegno il nuovo padre - Funzione simile al riordino
                foreach (liteCategoryReorderItem lCri in llCRI)
                {
                    lCri.FatherId = NewFatherId;
                    lCri.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                }
                // TOCCA FORZARE IL SALVATAGGIO!
                Manager.SaveOrUpdateList<liteCategoryReorderItem>(llCRI);
                Manager.Flush();
            }

            //CONTROLLO CHE NON CI SIANO SOTTO-CATEGORIE, altrimenti finisco per essere cancellate!
            Category DelCat = Manager.Get<Category>(CategoryId);
            if (DelCat.Children.Any())
                throw new Exception("Error 001: tentativo cancellazione categoria con figlie che dovrebbero essere state automaticamente riassegnate.");

            Dictionary<Int64, Int64> ReassignIds = new Dictionary<long, long>();
            if (ReassignCategoryId > 0)
                ReassignIds.Add(CategoryId, ReassignCategoryId);

            return CategoryDeleteAll(DelCat, new Dictionary<Int64, Int64>(), Message);
        }

        /// <summary>
        /// Cancella la categorie e TUTTE le sotto-categorie (ricorsivamente, senza riassegnazione!)
        /// </summary>
        /// <param name="CategoryId">Id CAtegoria da cancellare</param>
        /// <returns></returns>
        public Boolean CategoryDeleteAll(Int64 CategoryId)
        {
            if (CategoryId == CategoryDefaultGetID())
                return false;

            return CategoryDeleteAll(CategoryId, -1, "");
        }

        /// <summary>
        /// Cancella la categorie e TUTTE le sotto-categorie (ricorsivamente)
        /// </summary>
        /// <param name="CategoryId">Id Categoria da cancellare</param>
        /// <param name="ReassignIds">Id Categoria a cui riassegnare TUTTE le categorie</param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public Boolean CategoryDeleteAll(Int64 CategoryId, Int64 ReassignId, String Message)
        {
            if (CategoryId == CategoryDefaultGetID())
                return false;

            Dictionary<Int64, Int64> Reassignments = new Dictionary<long, long>();

            if (ReassignId > 0)
            {
                Reassignments.Add(-1, ReassignId);
            }

            return CategoryDeleteAll(CategoryId, Reassignments, Message);
        }

        /// <summary>
        /// RICORSIVO! Cancella la categoria e tutti i suoi figli, EVENTUALMENTE riassegnando gli stessi
        /// </summary>
        /// <param name="CategoryId">ID Categoria da cancellare</param>
        /// <param name="ReassignmentsIds">Riassegnazione: IdCategoria da cancellare, Id nuova categoria</param>
        /// <param name="Message">Messaggio di cancellazione (dB)</param>
        /// <returns></returns>
        public Boolean CategoryDeleteAll(Int64 CategoryId, IDictionary<Int64, Int64> Reassignments, String Message)
        {
            if (CategoryId == CategoryDefaultGetID())
                return false;

            return CategoryDeleteAll(Manager.Get<Category>(CategoryId), Reassignments, Message);
        }

        /// <summary>
        /// RICORSIVO! Cancella la categoria e tutti i suoi figli, EVENTUALMENTE riassegnando gli stessi
        /// </summary>
        /// <param name="Category">Categoria da cancellare</param>
        /// <param name="ReassignmentsIds">Riassegnazione: IdCategoria da cancellare, Id nuova categoria</param>
        /// <param name="Message">Messaggio di cancellazione (dB)</param>
        /// <returns></returns>
        public Boolean CategoryDeleteAll(Category Category, IDictionary<Int64, Int64> ReassignmentsIds, string Message)
        {

            if (Category == null || Category.Id <= 0 || Category.Id == CategoryDefaultGetID())
                return false;

            if (!Manager.IsInTransaction())
            {
                Manager.BeginTransaction();
            }

            IList<Int64> ReassignedID = ReassignmentsIds.Keys.ToList();


            if (ReassignedID != null || ReassignedID.Count() > 0)
            {
                IList<Assignment> OldAssignments = Manager.GetAll<Assignment>(a =>
                    a.AssignedCategory != null
                    && a.IsCurrent == true
                    && ReassignedID.Contains(a.AssignedCategory.Id));

                List<Ticket> Tickets = new List<Ticket>();

                if (OldAssignments != null && OldAssignments.Count() > 0)
                {
                    IList<Assignment> ReAssignments = new List<Assignment>();
                    IList<Message> NewMessages = new List<Message>();

                    foreach (Assignment oldass in OldAssignments)
                    {
                        Int64 CategoryId = -1; //NON RIASSEGNARE!

                        if (ReassignmentsIds.ContainsKey(oldass.AssignedCategory.Id))
                        {
                            CategoryId = ReassignmentsIds[oldass.AssignedCategory.Id];
                        }
                        else if (ReassignmentsIds.ContainsKey(-1))
                        {
                            CategoryId = ReassignmentsIds[-1];
                        }

                        Category NewCategory = null;
                        if (CategoryId > 0)
                        {
                            NewCategory = Manager.Get<Category>(CategoryId);
                        }

                        if (NewCategory != null)
                        {
                            Ticket tk = oldass.Ticket;

                            oldass.IsCurrent = false;
                            tk.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

                            Assignment NewAss = new Assignment();
                            NewAss.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                            NewAss.Ticket = tk;
                            NewAss.AssignedCategory = NewCategory;
                            NewAss.IsCurrent = true;
                            NewAss.Type = Domain.Enums.AssignmentType.Category;

                            ReAssignments.Add(NewAss);

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
                            msg.UserType = Domain.Enums.MessageUserType.System;
                            msg.Visibility = true;

                            msg.Action = Domain.Enums.MessageActionType.riassignedToCategory;
                            msg.ToStatus = oldass.Ticket.Status;
                            msg.ToUser = null;

                            msg.ToCategory = NewCategory;

                            NewMessages.Add(msg);

                            Tickets.Add(tk);
                        }
                    }

                    Manager.SaveOrUpdateList<Assignment>(OldAssignments);
                    Manager.SaveOrUpdateList<Assignment>(ReAssignments);
                    Manager.SaveOrUpdateList<Message>(NewMessages);
                    Manager.SaveOrUpdateList<Ticket>(Tickets);

                }
            }

            Category DelCategory = Manager.Get<Category>(Category.Id);
            DelCategory.Deleted = BaseStatusDeleted.Manual;
            //DelCategory.Order = this.CategoryGetDeletedOrder(); //TRY THIS! (SOLO questo).
            CategoryDeleteRecoursive(DelCategory.Children);

            DelCategory.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);

            Manager.SaveOrUpdate<Category>(DelCategory);

            try
            {
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
                return false;
            }

            CategoriesCacheSet();

            return true;
        }

        /// <summary>
        /// Ricorsione per cancellazione categorie e figlie
        /// </summary>
        /// <param name="OldCategory"></param>
        private void CategoryDeleteRecoursive(IList<Category> children)
        {

            if (children != null && children.Any())
            {
                foreach (Category Child in children)
                {
                    if (Child.Deleted == BaseStatusDeleted.None)
                    {
                        CategoryDeleteRecoursive(Child.Children);
                        Child.Deleted = BaseStatusDeleted.Cascade;
                        
                        Child.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                    }
                }
            }

            //OldCategory.Order = CategoryGetDeletedOrder();
            

            

            //CategoriesCacheSet();
        }

        /// <summary>
        /// Controlla se la categoria ha figli o meno
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public bool CategoryHasChildren(Int64 CategoryId)
        {
            bool HasChild = (from Category cate in Manager.GetIQ<Category>()
                             where cate.Id == CategoryId
                             && cate.Children.Any()
                             select cate).Any();

            return HasChild;


        }

        /// <summary>
        /// Controlla quanti Ticket sono associati alla categoria ed ai suoi figli
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="IsRecursive"></param>
        /// <returns></returns>
        public int CategoryGetAssociatedTicketNum(Int64 CategoryId, Boolean IsRecursive)
        {
            if (IsRecursive)
                return CategoryGetAssociatedTicketNumRecursive(CategoryId);

            return CategoryGetAssociatedTicketNumSingle(CategoryId);
        }

        /// <summary>
        /// Controlla i ticket associati alla categoria, ignorandone le figlie
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        private int CategoryGetAssociatedTicketNumSingle(Int64 CategoryId)
        {
            IEnumerable<Int64> TicketIds = from Assignment ass in Manager.GetIQ<Assignment>()
                                           where ass.IsCurrent == true
                                           && ass.AssignedCategory != null
                                           && ass.AssignedCategory.Id == CategoryId
                                           select ass.Ticket.Id;

            int TkCount = TicketIds.Distinct().Count();

            return TkCount;
        }

        /// <summary>
        /// Recupera il numero di Ticket associati alla categoria ed ai suoi figli
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        private int CategoryGetAssociatedTicketNumRecursive(Int64 CategoryId)
        {
            IList<Int64> tmpCatsIds = new List<Int64>();
            tmpCatsIds.Add(CategoryId);

            IList<Int64> CompletecateIds = CategoryGetFamilyIds(tmpCatsIds);


            //IEnumerable<Int64> TicketIds = from Assignment ass in Manager.GetIQ<Assignment>()
            //                               where ass.IsCurrent == true
            //                               && ass.AssignedCategory != null
            //                               && CompletecateIds.Contains(ass.AssignedCategory.Id)
            //                                select ass.Ticket.Id;
            int TkCount = 0;
            try
            {
                TkCount = (from Assignment ass in Manager.GetIQ<Assignment>()
                                           where ass.IsCurrent == true
                                           && ass.AssignedCategory != null
                                           && CompletecateIds.Contains(ass.AssignedCategory.Id)
                                            select ass.Ticket.Id).Distinct().Count();
            }
            catch (Exception)
            {
                
            }
            

            return TkCount;
        }

        /// <summary>
        /// Recupera l'elenco di ID delle categorie indicate e dei relativi figli. Ricorsiva.
        /// </summary>
        /// <param name="CategoriesIds"></param>
        /// <returns></returns>
        private IList<Int64> CategoryGetFamilyIds(IList<Int64> CategoriesIds)
        {
            IList<Category> Categories = Manager.GetAll<Category>(c => c.Deleted == BaseStatusDeleted.None && c.Children.Any() && CategoriesIds.Contains(c.Id));

            IList<Int64> ChildCats = new List<Int64>();

            foreach (Category cat in Categories)
            {
                if (cat.Children != null && cat.Children.Any())
                {
                    ChildCats = ChildCats.Union(
                        (from chcat in cat.Children select chcat.Id).ToList()
                        ).Distinct().ToList();
                }
            }

            //= (from Category childcat in

            //                        select childcat.Id).ToList();

            IList<Int64> SubChildrenIds = new List<Int64>();

            if (ChildCats != null && ChildCats.Count > 0)
            {
                IEnumerable<Int64> AllIds = CategoriesIds.Union(ChildCats);

                SubChildrenIds = CategoryGetFamilyIds(ChildCats);

                if (SubChildrenIds != null && SubChildrenIds.Count() > 0)
                {
                    AllIds = AllIds.Union(SubChildrenIds);
                }

                return AllIds.ToList();
            }
            else
            {
                return CategoriesIds;
            }
        }

        Int64 _CategoryDefaultId = -1;
        public Int64 CategoryDefaultGetID()
        {
            //int ManNum = (from LK_UserCategory lu in Manager.GetIQ<LK_UserCategory>() where lu.Category.Id == CategoryId && lu.IsManager == true select lu.Id).Count();

            if (_CategoryDefaultId <= 0)
            {

                _CategoryDefaultId = (from Category cat in Manager.GetIQ<Category>()
                            where cat.IsDefault == true
                            select cat.Id).FirstOrDefault();

                if (_CategoryDefaultId == null)
                    _CategoryDefaultId = 0;

                
            }

            return _CategoryDefaultId;
        }

        public Category CategoryDefaultGet()
        {
            return (from Category cat in Manager.GetIQ<Category>()
                        where cat.IsDefault == true
                        select cat).FirstOrDefault();

        }
        #endregion

        #region Gestione ruoli Categorie

        /// <summary>
        /// Aggiunge una PERSON alla categoria
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="IsManger">SE è manager</param>
        /// <param name="PersonId">Id Person (COMOL)</param>
        /// <returns></returns>
        public Boolean CategoryPersonAdd(
            Int64 CategoryId,
            Boolean IsManger,
            Int32 PersonId)
        {
            Domain.TicketUser User = Manager.GetAll<Domain.TicketUser>(u => u.Person != null && u.Person.Id == PersonId).FirstOrDefault();

            if (User != null && User.Id > 0)
            {
                return CategoryUserAdd(CategoryId, IsManger, User);
            }
            else
            {
                Domain.Enums.MailSettings Settings = Domain.Enums.MailSettings.Default;


                Int64 UserId = UserCreateFromPerson(PersonId, Settings).Id;
                if (UserId > 0)
                {
                    return CategoryUserAdd(CategoryId, IsManger, UserId);
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Aggiunge più persone ad una category
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="IsManger">Se sono TUTTI manager o resolver</param>
        /// <param name="PersonsIds">Elenco di ID Person</param>
        /// <returns>Lista di ID non aggiunti</returns>
        public IList<int> CategoryPersonsAdd(
            Int64 CategoryId,
            Boolean IsManger,
            IList<int> PersonsIds)
        {

            IList<int> ErrorPersonsIs = new List<int>();

            foreach (int pid in PersonsIds)
            {
                if (!CategoryPersonAdd(CategoryId, IsManger, pid))
                    ErrorPersonsIs.Add(pid);
            }

            return ErrorPersonsIs;
        }

        /// <summary>
        /// Aggiunge PERSONE tramite DTO che indica per ogni ID Person il loro ruolo
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="Roles">Elenco coppie IdPERSON/IsManager</param>
        /// <returns></returns>
        public Domain.DTO.DTO_AddRolesResponse CategoryRolesAdd(
            Int64 CategoryId,
            IList<Domain.DTO.DTO_CategoryRole> Roles)
        {
            Domain.DTO.DTO_AddRolesResponse Response = new Domain.DTO.DTO_AddRolesResponse();

            int Managers = (from Domain.DTO.DTO_CategoryRole rl in Roles
                            where rl.IsManager == true && rl.PersonId >= 0
                            select rl).ToList().Count();

            if (Managers <= 0)
            {
                Response.NoManager = true;
            }

            foreach (Domain.DTO.DTO_CategoryRole rl in Roles)
            {
                if (rl.PersonId >= 0)
                {
                    if (!this.CategoryPersonAdd(CategoryId, rl.IsManager, rl.PersonId))
                    {
                        Response.UnAddedRoles.Add(rl);
                    }
                }
            }

            return Response;
        }

        /// <summary>
        /// Aggiunge uno USER (Ticket) ad una category
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="IsManger">SE è manager (altrimenti resolver)</param>
        /// <param name="UserId">Id USER (Ticket)</param>
        /// <returns></returns>
        public Boolean CategoryUserAdd(
            Int64 CategoryId,
            Boolean IsManger,
            Int64 UserId)
        {
            Domain.TicketUser User = Manager.Get<Domain.TicketUser>(UserId);

            if (User != null && User.Id > 0)
            {
                CategoryUserAdd(CategoryId, IsManger, User);
            }

            return false;
        }

        /// <summary>
        /// Aggiunge un utente (Ticket) ad una categoria
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="IsManger">SE è manager</param>
        /// <param name="User">UTENTE</param>
        /// <returns></returns>
        public Boolean CategoryUserAdd(
            Int64 CategoryId,
            Boolean IsManger,
            Domain.TicketUser User)
        {

            Domain.Category cat = Manager.Get<Domain.Category>(CategoryId);

            if (cat != null && cat.Id > 0 && User != null && User.Id > 0 && User.Person != null)
            {
                Domain.LK_UserCategory lkUC = Manager.GetAll<Domain.LK_UserCategory>(lk => lk.User != null && lk.User.Id == User.Id && lk.Category != null && lk.Category.Id == CategoryId).FirstOrDefault();
                if (lkUC != null)
                {
                    return false;
                }

                lkUC = new LK_UserCategory();
                lkUC.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                lkUC.Category = cat;
                lkUC.User = User;
                lkUC.IsManager = IsManger;

                Manager.SaveOrUpdate<LK_UserCategory>(lkUC);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Rimuove uno USER dalla categoy
        /// </summary>
        /// <param name="CategoryId">ID Categoria</param>
        /// <param name="UserId">ID Utente (Ticket)</param>
        public bool CategoryUserRemove(Int64 CategoryId, Int64 UserId)
        {

            int ManNum = (from LK_UserCategory lu in Manager.GetIQ<LK_UserCategory>() where lu.Category.Id == CategoryId && lu.IsManager == true select lu.Id).Count();


            LK_UserCategory lkuc = Manager.GetAll<LK_UserCategory>(lu => lu.Category.Id == CategoryId && lu.User.Id == UserId).FirstOrDefault();

            if (lkuc != null && ((!lkuc.IsManager && ManNum > 0) || lkuc.IsManager && ManNum > 1))
            {
                Manager.DeletePhysical<LK_UserCategory>(lkuc);
                //Manager.Commit();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Aggiorna utenti (ruoli di una categoria)
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="UsersRoles">IDictionary che usa come chiave il Ticket User ID e come valore TRUE se Manager, False se Resolver</param>
        public Domain.Enums.CategoryAssignersError CategoryUserUpdate(Int64 CategoryId, IDictionary<Int64, Boolean> UsersRoles)
        {
            if (UsersRoles == null)
                return Domain.Enums.CategoryAssignersError.noManager;

            Dictionary<Int64, bool> _usrRole = new Dictionary<Int64, bool>(UsersRoles);
               

            bool HasManager = _usrRole.ContainsValue(true);
            
            if(HasManager)
            {
                IList<LK_UserCategory> UsrCats = Manager.GetAll<LK_UserCategory>(luk => luk.Category.Id == CategoryId).ToList();

                foreach (LK_UserCategory ct in UsrCats)
                {
                    ct.IsManager = _usrRole[ct.User.Id];
                    //HasRealManager = HasRealManager || ct.IsManager;
                }

                Manager.SaveOrUpdateList<LK_UserCategory>(UsrCats);
                return Domain.Enums.CategoryAssignersError.none;
            }

            return Domain.Enums.CategoryAssignersError.noManager;
        }

        /// <summary>
        /// Controllare ed imposta un manager per la categoria indicata
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        /// <remarks>
        ///  >0: id USER assegnatario
        ///  =0: User corrente
        ///  -1: manager presente
        /// -10: no category
        /// -11: no user (current)
        /// </remarks>
        public Domain.DTO.DTO_CategoryCheckResponse CategoryUsersCheck(Int64 CategoryId)
        {
            Domain.DTO.DTO_CategoryCheckResponse resp = new Domain.DTO.DTO_CategoryCheckResponse();


            Boolean HasManager = (from liteLK_UserCategory liteLK in Manager.GetIQ<liteLK_UserCategory>()
                                  where liteLK.IdCategory == CategoryId && liteLK.IsManager == true
                                  select liteLK.Id).Any();

            if (HasManager)
            {
                resp.PreviousAssigned = true;
                return resp;
            }   

            LK_UserCategory LKuc = (from LK_UserCategory lk in Manager.GetIQ<LK_UserCategory>()
                                    where lk.Category != null && lk.Category.Id == CategoryId
                                    orderby lk.CreatedOn
                                    select lk).Skip(0).Take(1).FirstOrDefault();

            Int64 SelectedUserId = 0;

            if (LKuc == null)
            {
                LKuc = new LK_UserCategory();
                LKuc.Category = Manager.Get<Category>(CategoryId);
                if (LKuc.Category == null)
                {
                    resp.NoCategory = true;
                    return resp;
                }

                LKuc.User = CurrentUser;
                if(LKuc.User == null || LKuc.User.Id <= 0)
                {
                    resp.NoUser = true;
                    return resp;
                }
                
                resp.IsCurrentUser = true;

                LKuc.CreateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                LKuc.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
            }

            LKuc.IsManager = true;
            SelectedUserId = LKuc.User.Id;
            Manager.SaveOrUpdate<LK_UserCategory>(LKuc);

            resp.UserDisplayName = (LKuc.User.Person == null) ?
                LKuc.User.Sname + " " + LKuc.User.Name :
                LKuc.User.Person.SurnameAndName;

            return resp;
        }

        #endregion

        #region Translation - Rivedere in favore di funzioni interne.

        /// <summary>
        /// Fa il DETACH dell'oggetto Category e ne internazionalizza Nome e Descrizione
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public Category CategoryTranslate(Category Category, int LanguageId)
        {
            if (LanguageId > 0)
            {

                CategoryTranslation Translated = (from CategoryTranslation ct in Category.Translations where ct.LanguageId == LanguageId select ct).FirstOrDefault();

                Manager.Detach<Category>(Category);

                if (Translated == null)
                {
                    Translated = (from CategoryTranslation ct in Category.Translations where ct.LanguageId == LanguageIdSystem select ct).FirstOrDefault();
                }

                if (Translated != null)
                {
                    Category.Name = Translated.Name;
                    Category.Description = Translated.Description;
                }
            }
            return Category;
        }

        /// <summary>
        /// Recupera il SOLO nome internazionalizzato della categoria corrente - NO DETACH!
        /// </summary>
        /// <param name="Category">Categoria</param>
        /// <param name="LanguageId">ID lingua</param>
        /// <returns></returns>
        public String CategoryTranslateName(Category Category, int LanguageId)
        {
            if (Category == null)
                return "";

            String CatName = Category.Name;

            if (LanguageId > 0)
            {

                CategoryTranslation Translated = (from CategoryTranslation ct in Category.Translations where ct.LanguageId == LanguageId select ct).FirstOrDefault();

                if (Translated == null)
                {
                    Translated = (from CategoryTranslation ct in Category.Translations where ct.LanguageId == LanguageIdSystem select ct).FirstOrDefault();
                }

                if (Translated != null)
                {
                    CatName = Translated.Name;
                }
            }
            return CatName;
        }

        /// <summary>
        /// Elimina una traduzione
        /// </summary>
        /// <param name="CategoryId">Id Categoria</param>
        /// <param name="LanguageId">Id Lingua</param>
        public void CategoryLanguageDelete(Int64 CategoryId, Int64 LanguageId)
        {
            Domain.Category cat = Manager.Get<Category>(CategoryId);

            Domain.CategoryTranslation trn = (from CategoryTranslation ct in cat.Translations where ct.LanguageId == LanguageId select ct).FirstOrDefault();

            if (trn != null && trn.LanguageCode != LangMultiCODE)
            {
                cat.UpdateMetaInfo(CurrentPerson, UC.IpAddress, UC.ProxyIpAddress);
                cat.Translations.Remove(trn);
                Manager.DeletePhysical<Domain.CategoryTranslation>(trn);

                Manager.SaveOrUpdate<Category>(cat);

                CategoriesCacheSet();
            }


        }

        #endregion

        /// <summary>
        /// Recupera i "permessi" di creazione di una categoria, relativamente ad una comunità
        /// </summary>
        /// <param name="CommunityID">Id Comunità: SE minore o uguale a 0: nessun permesso (Vale anche per il PORTALE!)</param>
        /// <returns>Permessi</returns>
        public Domain.DTO.DTO_CategoryTypeComPermission CategoryTypeGetPermission(int CommunityID)
        {
            Domain.DTO.DTO_CategoryTypeComPermission Permission = new Domain.DTO.DTO_CategoryTypeComPermission();
            if (CommunityID <= 0)
                return Permission;

            int ComTypeId = -1;

            try
            {
                ComTypeId = (from Community Com in Manager.GetIQ<Community>() where Com.Id == CommunityID select Com.IdTypeOfCommunity).Skip(0).Take(1).FirstOrDefault();
            }
            catch { }

            Domain.SettingsComType ComTypeSett = new SettingsComType();

            if (ComTypeId >= 0)
            {
                ComTypeSett = Manager.GetAll<Domain.SettingsComType>(s => s.CommunityType.Id == ComTypeId).FirstOrDefault();

                if (ComTypeSett != null)
                {
                    Permission.CanPrivate = ComTypeSett.CreatePrivate;
                    Permission.CanPublic = ComTypeSett.CreatePublic;
                    Permission.CanTicket = ComTypeSett.CreateTicket;
                }
            }

            return Permission;
        }


        #region Caching - Category

        /// <summary>
        /// Imposta le categorie in cache, con TUTTE quelle presenti nel dB!!!
        /// </summary>
        private void CategoriesCacheSet()
        {
            IList<liteCategoryTreeItem> Cats = Manager.GetAll<liteCategoryTreeItem>(c => c.Father == null && c.Deleted == BaseStatusDeleted.None);
            
            Manager.DetachList<liteCategoryTreeItem>(Cats);

            CacheHelper.PurgeCacheItems(CacheKeyCategory);
            CacheHelper.AddToCache<IList<liteCategoryTreeItem>>(CacheKeyCategory, Cats, CacheExpiration);
        }

        /// <summary>
        /// Recupera le categorie in Cache. Se VUOTO, reimposta con TUTTE le categorie del dB.
        /// </summary>
        /// <returns></returns>
        private IList<liteCategoryTreeItem> CategoriesCachedGet()
        {
            IList<liteCategoryTreeItem> Cats = null;
            try
            {
                Cats = (IList<liteCategoryTreeItem>)CacheHelper.Cache.Get(CacheKeyCategory);
            }
            catch { }


            if (Cats == null || Cats.Count() <= 0)
            {
                CategoriesCacheSet();
                Cats = (IList<liteCategoryTreeItem>)CacheHelper.Cache.Get(CacheKeyCategory);

                //Per "sicurezza": se non riesce a mettere  e prendere dalla cache, recupera da dB.
                if (Cats == null)
                {
                    return Manager.GetAll<liteCategoryTreeItem>(c => c.Father == null);
                }
            }

            return Cats;
        }

        #endregion

        /// <summary>
        /// Recupera il numero di categorie per tipo presenti nel sistema
        /// </summary>
        /// <returns></returns>
        public Domain.DTO.DTO_SysCategoryInfo CategorySysInfoGet()
        {
            Domain.DTO.DTO_SysCategoryInfo ci = new Domain.DTO.DTO_SysCategoryInfo();

            ci.Public = (from Domain.Category ct in Manager.GetIQ<Domain.Category>() where ct.Type == Domain.Enums.CategoryType.Public select ct.Id).Count();

            ci.Ticket = (from Domain.Category ct in Manager.GetIQ<Domain.Category>() where ct.Type == Domain.Enums.CategoryType.Ticket select ct.Id).Count();

            ci.Community = (from Domain.Category ct in Manager.GetIQ<Domain.Category>() where ct.Type == Domain.Enums.CategoryType.Current select ct.Id).Count();

            return ci;

        }

        //public int CategoryGetDeletedOrder()
        //{
        //    return CategoryGetCommunityOrder() + maxCategoryPerCommunity - 1;
        //}

        public int CategoryGetUndeletedOrder()//Int64 CategoryId)
        {
            //Domain.Category category = Manager.Get<Category>(CategoryId);

            //if (category.Father == null)
            //{
            //    category.Order =
            //        Manager.GetAll<Domain.Category>(
            //            c =>
            //                c.IdCommunity == category.IdCommunity
            //                && c.Father == null
            //                && c.Deleted == BaseStatusDeleted.None).Count();
            //}
            //else
            //{
            //    category.Order =
            //        Manager.GetAll<Domain.Category>(
            //            c =>
            //                c.IdCommunity == category.IdCommunity
            //                &&c.Father != null
            //                && c.Father.Id == category.Father.Id
            //                && c.Deleted == BaseStatusDeleted.None).Count();
            //}


            return CategoryGetCommunityOrder() + maxCategoryPerCommunity - 1;
        }

        public int CategoryGetCommunityOrder()
        {
            //CommunityOrder 
            int COrder = -1;
            try
            {
                COrder = UC.CurrentCommunityID * maxCategoryPerCommunity;
            }
            catch { }

            if (COrder > int.MaxValue - maxCategoryPerCommunity || COrder < 0)
            {
                COrder = 0 - UC.CurrentCommunityID * maxCategoryPerCommunity;
            }
            return COrder;
        }

        public int CategoryGetCommunityOrder(int CommunityId)
        {
            //CommunityOrder 
            int COrder = -1;
            try
            {
                COrder = CommunityId * maxCategoryPerCommunity;
            }
            catch { }

            //if (COrder > int.MaxValue - maxCategoryPerCommunity || COrder < 0)
            //{
            //    COrder = 0 - CommunityId * maxCategoryPerCommunity;
            //}
            return COrder;
        }


        public IList<LK_UserCategory> CategoryGetLkucList(Int64 CategoryId)
        {
            IList<LK_UserCategory> lkUcList = new List<LK_UserCategory>();

            if (CategoryId > 0)
            {
                lkUcList =
                    Manager.GetAll<LK_UserCategory>(lkuc => lkuc.Category != null && lkuc.Category.Id == CategoryId && lkuc.Deleted == BaseStatusDeleted.None);
            }

            return lkUcList;
        }
    }
}
