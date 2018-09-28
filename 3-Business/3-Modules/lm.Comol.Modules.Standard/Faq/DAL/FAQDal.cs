using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Cfg;

namespace lm.Comol.Modules.Standard.Faq
{
    public class FAQDal
    {
        private ISession Session_Col { get; set; }


        public FAQDal(ISession ComolSession)
        {
            Session_Col = ComolSession;
        }

        #region Category
            public IList<Category> GetCategoriesList(Int32 CommunityId)
            {
                IList<Category> Cats;

                Cats = (
                    from Category cat
                    in this.Session_Col.Linq<Category>()
                    where cat.CommunityId == CommunityId
                    select cat
                    ).ToList();

                return Cats;

                //Ritorno una lista di categorie
                //con le relative FAQ
                //Utilizzato sia in visualizzazione che in modifica.
                
            }

            public IList<Category> GetCategoriesList(IList<Int64> CategoriesId)
            {
                IList<Category> Cats;

                Cats = (
                    from Category cat
                    in this.Session_Col.Linq<Category>()
                    where CategoriesId.Contains(cat.Id)
                    select cat
                    ).ToList();

                return Cats;

                //Ritorno una lista di categorie
                //con le relative FAQ
                //Utilizzato sia in visualizzazione che in modifica.

            }

            public Category GetCategory(Int32 CommunityId, Int64 CategoryId)
            {
                Category Cat;

                Cat = (
                    from Category cat
                    in this.Session_Col.Linq<Category>()
                    where 
                        cat.CommunityId == CommunityId &&
                        cat.Id == CategoryId
                    select cat
                    ).First();

                return Cat;

                //Ritorno una lista di categorie
                //con le relative FAQ
                //Utilizzato sia in visualizzazione che in modifica.

            }

            public void UpdateCategory(Category Category)
            { 
                //Per modificare SOLO il testo della Category.
                //Verranno utilizzati SOLO ID e NOME, il resto me lo aspetto a NULL
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                this.Session_Col.Update(Category);
                tx.Commit();
            }

            public void AddCategory(Category Category)
            {
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                this.Session_Col.Save(Category);
                tx.Commit();
                //Verranno utilizzati SOLO ID e NOME, il resto me lo aspetto a NULL
            }

            public void DeleteCategory(Category cat)
            {
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();

                try
                {
                    IList<FaqOnCategory> FOCs = (from FaqOnCategory FOC
                        in this.Session_Col.Linq<FaqOnCategory>()
                        where FOC.CatId == cat.Id
                        select FOC).ToList();
                    
                    foreach(FaqOnCategory foc in FOCs)
                    {
                        this.Session_Col.Delete(foc);    
                    };

                    
                    this.Session_Col.Delete(cat);
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {

                }
                //Cancella la Categoria e TUTTE le FAQ che non sono associate ad altre categorie...
                //Rivedere in ottica LINQ
            }

        #endregion
        #region FAQ
            public Faq GetFaq(Int64 FaqId, Int32 CommunityID)
            {
                Faq Faq = new Faq();

                try
                {
                    Faq = (
                    from Faq faq
                    in this.Session_Col.Linq<Faq>()
                    where faq.Id == FaqId && faq.CommunityId == CommunityID
                    orderby faq.Order, faq.Id
                    select faq
                    ).First();
                }
                catch {
                    Faq.Id = -1;
                    Faq.Answer = "";
                    Faq.Question = "";
                }
                
                return Faq;
            }

            public IList<Faq> GetFaqs(Int32 CommunityId)
            {
                IList<Faq> Faqs;

                Faqs = (
                    from Faq faq
                    in this.Session_Col.Linq<Faq>()
                    where faq.CommunityId == CommunityId
                    orderby faq.Order, faq.Id
                    select faq
                    ).ToList();

                return Faqs;
            }

            public IList<Faq> GetFaqs(Int32 CommunityId, Int64 CategoryId)
            {
                IList<Faq> Faqs;

                Category Cat;

                try
                {
                    Cat = (
                        from Category cat
                        in this.Session_Col.Linq<Category>()
                        where
                            cat.CommunityId == CommunityId &&
                            cat.Id == CategoryId
                        select cat
                        ).First();
                }
                catch {
                    Cat = new Category();
                    Cat.Id = -1;
                }

                if (Cat.Id > 0)
                {
                    Faqs = (
                        from Faq faq
                        in this.Session_Col.Linq<Faq>()
                        where ((faq.CommunityId == CommunityId) && faq.onCategories.Contains(Cat))
                        orderby faq.Order, faq.Id
                        select faq).ToList();
                }
                else {
                    Faqs = (
                        from Faq faq
                        in this.Session_Col.Linq<Faq>()
                        where (faq.CommunityId == CommunityId)
                        orderby faq.Order, faq.Id
                        select faq).ToList();
                }
                return Faqs;
            }

            public void UpdateFaq(Faq Faq)
            { 
                //Aggiorna una Faq.
                //Verranno modificati sia i dati relativi alla FAQ
                //che l'associazione con le categorie

                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                this.Session_Col.Update(Faq);
                tx.Commit();
            }

            public void AddFaq(Faq Faq)
            {
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                this.Session_Col.Save(Faq);
                tx.Commit();
                //Aggiorna una Faq.
                //Verranno modificati sia i dati relativi alla FAQ
                //che l'associazione con le categorie
            }

            public void DeleteFaq(Int64 FaqID)
            { 
                //Cancella la FAQ specificata
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                Faq faq = (from Faq fq
                   in this.Session_Col.Linq<Faq>()
                           where fq.Id == FaqID
                           select fq).First();

                this.Session_Col.Delete(faq);
                tx.Commit();
            }

            public void DeleteFaq(Faq faq)
            {
                //Cancella la FAQ specificata
                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                this.Session_Col.Delete(faq);
                tx.Commit();
            }

            //Utilizzare UNA delle due seguenti a seconda delle logiche nHybernate
            //public void DeleteOrphanFaq()
            //{ 
            //    //Cancella le FAQ che non sono associate a nessuna categoria
            //}

            public void DeleteFaqOnCategory(Int64 CategoryId)
            {


                ITransaction tx;
                tx = this.Session_Col.BeginTransaction();
                
                tx.Commit();
            }
        #endregion

    }
}
