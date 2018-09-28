using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.Business;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Cfg;

namespace lm.Comol.Modules.Standard.Faq
{
    public class FAQService
    {
        private FAQDal FAQ_Dal { get;set;}
        
        private iUserContext UC { set; get; }
        private BaseManager Manager { get; set; }
        //private iDataContext DC { set; get; }

        #region initClass
        //public FAQManager(ISession ComolSession)
        //{
        //    FAQ_Dal = new FAQDal(ComolSession);
        //}

        public FAQService() { }
        public FAQService(iApplicationContext oContext, ISession ComolSession)
        {
            this.Manager = new BaseManager(oContext.DataContext);

            this.FAQ_Dal = new FAQDal(ComolSession);

            //DC = oContext.DataContext;
            this.UC = oContext.UserContext;

        }
        //public FAQService(iDataContext oDC)
        //{
        //    DC = oDC;
        //    //this.Manager = new BaseManager(oDC);
        //    FAQ_Dal = new FAQDal(ComolSession);
        //    this.UC = null;
        //}

        #endregion

        #region Read
        //Per Web Service

        /// <summary>
        /// Recupera una singola FAQ, controllando la comunità.
        /// </summary>
        /// <param name="FaqId">L'ID della FAQ da recuperare</param>
        /// <returns>La FAQ o una FAQ VUOTA se non viene trovata.</returns>
        public DTO_Faq GetFaq(Int64 FaqId)
        {
            Faq SourceFaq = FAQ_Dal.GetFaq(FaqId, UC.CurrentCommunityID);

            DTO_Faq outFaq = new DTO_Faq();
            //CurrentContext.UserContext.CurrentCommunityID
            if (SourceFaq != null) // && SourceFaq.CommunityId == UC.CurrentCommunityID)
            {
                outFaq.ID = SourceFaq.Id;
                outFaq.Question = SourceFaq.Question;
                outFaq.Answer = SourceFaq.Answer;

                outFaq.Categories = new List<DTO_Category>();

                if (SourceFaq.onCategories != null && SourceFaq.onCategories.Count() > 0)
                {

                    foreach (Category Cat in SourceFaq.onCategories)
                    {
                        outFaq.Categories.Add(new DTO_Category() { Name = Cat.Name, ID = Cat.Id });
                    }

                }
            }
            //else
            //{
            //    outFaq.Categories = new List<String>();
            //}

            return outFaq;
        }

        /// <summary>
        /// Recupera l'elenco delle faq disponibili in visualizzazione
        /// </summary>
        /// <param name="CategoryID">La categoria con cui filtrarle. Se minore di 0, vengono recuperate TUTTE le faq di quella comunità</param>
        /// <returns>Una lista di FAQ o una lista vuota se non ci sono FAQ.</returns>
        public List<DTO_Faq> GetFaqList(Int64 CategoryID)
        {
            List<DTO_Faq> OutFaqs = new List<DTO_Faq>();

            IList<Faq> srcFaqs = new List<Faq>();

            if (CategoryID > 0)
            {
                srcFaqs = FAQ_Dal.GetFaqs(UC.CurrentCommunityID, CategoryID);
            }
            else
            {
                srcFaqs = FAQ_Dal.GetFaqs(UC.CurrentCommunityID);
            }

            if (srcFaqs != null && srcFaqs.Count() > 0)
            {
                foreach (Faq fq in srcFaqs)
                {
                    DTO_Faq outfq = new DTO_Faq();
                    outfq.ID = fq.Id;
                    outfq.Question = fq.Question;
                    outfq.Answer = fq.Answer;
                    if (fq.onCategories != null && fq.onCategories.Count() > 0)
                    {
                        foreach (Category Cat in fq.onCategories)
                        {
                            outfq.Categories.Add(new DTO_Category() { Name= Cat.Name, ID= Cat.Id});
                        }
                    }
                    OutFaqs.Add(outfq);
                }
            }
            
            return OutFaqs;
        }
        
        /// <summary>
        /// Recupera l'elenco di categorie disponibili per la comunità corrente
        /// </summary>
        /// <returns>
        ///     L'elenco delle categorie o un elenco vuoto se non ci sono categorie.
        /// </returns>
        public List<DTO_Category> GetCategories()
        {
            List<DTO_Category> OutCats = new List<DTO_Category>();

            IList<Category> srcCats = FAQ_Dal.GetCategoriesList(UC.CurrentCommunityID);

            if (srcCats != null && srcCats.Count() > 0)
            {
                foreach (Category srcCt in srcCats)
                {
                    DTO_Category outCat = new DTO_Category();
                    outCat.ID = srcCt.Id;
                    outCat.Name = srcCt.Name;
                    OutCats.Add(outCat);
                }
            }

            return OutCats;
        }

        public UserDataModel GetUserDataModel(Int32 CommunityId)
        {
            UserDataModel UDM = new UserDataModel();
            UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId);
            UDM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            UDM.Category.Add(DefaultCategory());
            UDM.CurrentCategory = DefaultCategory();
            return UDM;
        }

        public UserDataModel GetUserDataModel(Int32 CommunityId, Int64 CategoryId)
        {
            
            UserDataModel UDM = new UserDataModel();
            if (CategoryId > 0)
            { 
                UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId, CategoryId);
                UDM.CurrentCategory = FAQ_Dal.GetCategory(CommunityId, CategoryId);
            }
            else {
                UDM.Faqs = FAQ_Dal.GetFaqs(CommunityId);
                UDM.CurrentCategory = DefaultCategory();
            }
            
            UDM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            //UDM.Category.Add(DefaultCategory());


            
            return UDM;
        }

        public EditFaqModel GetEditFaqModel(Int64 FaqId, Int32 CommunityId)
        {
            EditFaqModel EFM = new EditFaqModel();
            EFM.Faq = FAQ_Dal.GetFaq(FaqId, CommunityId);
            EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            return EFM;
        }

        public EditFaqModel GetNewFaqModel(Int32 CommunityId)
        {
            EditFaqModel EFM = new EditFaqModel();
            Faq NewFaq = new Faq();
            NewFaq.Question = "";
            NewFaq.Answer = "";
            NewFaq.CommunityId = CommunityId;
            NewFaq.onCategories = new List<Category>();
            EFM.Faq = NewFaq;

            EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            //EFM.Faq = FAQ_Dal.GetFaq(FaqId);
            //EFM.Category = FAQ_Dal.GetCategoriesList(CommunityId);
            return EFM;
        }

        
        public EditCategoryModel GetEditCategoryModel(Int32 CommunityId)
        {
            EditCategoryModel ECM = new EditCategoryModel();
            ECM.Categories = FAQ_Dal.GetCategoriesList(CommunityId);

            foreach (Category cat in ECM.Categories)
            {
                cat.Elements = (from Faq faq in FAQ_Dal.GetFaqs(CommunityId, cat.Id) select faq).Count();
            }

            return ECM;
        }

        public EditCategoryModel GetEditCategoryModel(Int32 CommunityId, Int64 CategoryId)
        {
            EditCategoryModel ECM = new EditCategoryModel();
            ECM.Categories = FAQ_Dal.GetCategoriesList(CommunityId);

            foreach (Category cat in ECM.Categories)
            {
                cat.Elements = (from Faq faq in FAQ_Dal.GetFaqs(CommunityId, cat.Id) select faq).Count();
            }

            ECM.CategoryForModify = FAQ_Dal.GetCategory(CommunityId, CategoryId);

            return ECM;
        }

#endregion

        #region Update/Save

        /// <summary>
        /// Utilizzato sia per modificare Domanda/risposta
        /// che per aggiornare la lista di categorie associate.
        /// </summary>
        /// <param name="Faq"></param>
        /// <remarks>
        /// Verificare la struttura in relazione ad nHibernate, Id ed affini...
        /// </remarks>
        public Enum.ErrorCode UpdateFaq(Faq Faq)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;
            if (Faq.onCategories.Count > 0)
            {
                Person user = Manager.Get<Person>(UC.CurrentUserID);
                Faq.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);


                FAQ_Dal.UpdateFaq(Faq);
            } else
            {
                ErrorCode = Enum.ErrorCode.NoCategory;
            }

            return ErrorCode;
        }

        public Enum.ErrorCode UpdateFaq(Int64 Id, String Question, String Answer)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;

            if ((Question == null) || (Question == "") || (Answer == null) || (Answer == ""))
            {
                ErrorCode = Enum.ErrorCode.NoData;
            }
            else
            {
                Faq Faq = Manager.Get<Faq>(Id);// FAQ_Dal.GetFaq(Id);
                Faq.Question = Question;
                Faq.Answer = Answer;
                Person user = Manager.Get<Person>(UC.CurrentUserID);
                Faq.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                Manager.SaveOrUpdate<Faq>(Faq);

                //FAQ_Dal.UpdateFaq(Faq);
            }

            return ErrorCode;
        }

        public Enum.ErrorCode UpdateFaq(Int64 Id, String Question, String Answer, IList<Int64> CategoriesId, Int32? Order = null)
        {
            Enum.ErrorCode ErrorCode = Enum.ErrorCode.none;

            if ((Question == null) || (Question == "") || (Answer == null) || (Answer == ""))
            {
                ErrorCode = Enum.ErrorCode.NoData;
            }
            else
            {
                Faq Faq = FAQ_Dal.GetFaq(Id, UC.CurrentCommunityID);
                Faq.Question = Question;
                Faq.Answer = Answer;
                Faq.onCategories = FAQ_Dal.GetCategoriesList(CategoriesId);
                Person user = Manager.Get<Person>(UC.CurrentUserID);
                Faq.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);
                Faq.Order = Order;

                FAQ_Dal.UpdateFaq(Faq);
            }

            return ErrorCode;
        }


        public Faq CreateFaq(Int32 CommunityId, String Question, String Answer, IList<Int64> CategoriesId)
        {
            Faq newFaq = new Faq();

            newFaq.CommunityId = CommunityId;
            newFaq.Question = Question;
            newFaq.Answer = Answer;

            if (CategoriesId.Count > 0)
            {
                newFaq.onCategories = FAQ_Dal.GetCategoriesList(CategoriesId);
            }
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            newFaq.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            FAQ_Dal.AddFaq(newFaq);
            return newFaq;
        }

        public void UpdateCategory(Category Category)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            Category.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            FAQ_Dal.UpdateCategory(Category);
        }

        public void UpdateCategory(Int64 CategoryId, String CategoryName, Int32 CommunityId)
        {
            Category cat = FAQ_Dal.GetCategory(CommunityId, CategoryId);
            cat.Name = CategoryName;

            Person user = Manager.Get<Person>(UC.CurrentUserID);
            cat.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            FAQ_Dal.UpdateCategory(cat);
        }

        public void CreateCategory(Category Category)
        {
            Person user = Manager.Get<Person>(UC.CurrentUserID);
            Category.CreateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

            FAQ_Dal.AddCategory(Category);
        }

        public Enum.ErrorCode CreateCategory(String CategoryName, Int32 CommunityId)
        {
            if (CategoryName == "")
            {
                return Enum.ErrorCode.NoData;
            }
            else
            {
                Category Cat = new Category();
                Cat.Name = CategoryName;
                Cat.CommunityId = CommunityId;

                Person user = Manager.Get<Person>(UC.CurrentUserID);
                Cat.UpdateMetaInfo(user, UC.IpAddress, UC.ProxyIpAddress);

                FAQ_Dal.AddCategory(Cat);
                return Enum.ErrorCode.none;
            }
        }

        #endregion
#region Delete
        public void DeleteFaq(Int64 FaqId)
        {
            FAQ_Dal.DeleteFaq(FaqId);
        }

        public void DeleteFaq(Faq faq)
        {
            FAQ_Dal.DeleteFaq(faq);
        }

        public Enum.ErrorCode DeleteCategory(Int32 CommunityId, Int64 CategoryId)
        {
            Category Cat = FAQ_Dal.GetCategory(CommunityId, CategoryId);

            if (Cat != null)
            {
                //FAQ_Dal.DeleteFaqOnCategory(CategoryId);
                //Cat.Faqs = FAQ_Dal.GetFaqs(CommunityId, CategoryId);

                //if (Cat.Faqs.Count > 0)
                //{
                //    return Enum.ErrorCode.CategoryWithFaq;
                //}
                //else
                //{
                    FAQ_Dal.DeleteCategory(Cat);
                //    FAQ_Dal.DeleteOrphanFaq(CategoryId);
                //}
            }
            else 
            {
                return Enum.ErrorCode.NoData;
            }

            return Enum.ErrorCode.none;
            
        }

#endregion

        private Category DefaultCategory()
        {
            Category cat = new Category();
            cat.Name = "All";
            cat.Id = -1;
            return cat;
        }
    }
}
