using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class GlossarySearchPresenter : DomainPresenter
    {
        private const String AllChar = " ";

        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;

            View.SetTitle((idCommunity > 0) ? CurrentManager.GetCommunityName(idCommunity) : "");
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.IdCommunity = idCommunity;
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                View.IdCommunity = idCommunity;
                var module = Service.GetPermissions(View.IdCommunity, litePerson);
                // Verifico Permessi modulo
                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                {
                    // var usedLetters = Service.GetGlossaryUsedLetters(GetIDGlossary());
                    var filter = new GlossaryFilter {SearchString = View.SearchString, LemmaString = View.LemmaString, LemmaContentString = View.LemmaContentString, LemmaSearchType = (FilterTypeEnum) View.LemmaSearchType, LemmaVisibilityType = (FilterVisibilityTypeEnum) View.LemmaVisibilityType};
                    View.LoadViewData(View.IdCommunity, filter);
                    ChangeLetter(String.Empty, 0, 50, filter);
                    return;
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void ChangeLetter(String letter, Int32 currentPage, Int32 pageSize, GlossaryFilter filter)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }

            if (String.IsNullOrEmpty(letter))
                letter = AllChar;

            if (pageSize == 0)
                pageSize = 10;

            Int32 records;
            var currentChar = letter[0];
            var listValidId = GetIDGlossary();

            //var currentLetterTermListAll = Service.GetDTO_TermListFromliteTerm(f => f.Deleted == BaseStatusDeleted.None , f => f.Name, currentPage, currentChar, pageSize, filter, out records).ToList();
            Dictionary<string, CharInfo> words;
            var currentLetterTermList = Service.GetDTO_TermListFromliteTerm(f => f.Deleted == BaseStatusDeleted.None && listValidId.Contains(f.IdGlossary), f => f.Name, currentPage, currentChar, pageSize, filter, out records, out words).ToList();

            var filteredTerms = new List<DTO_Term>();
            var usedletterList = new List<String>();
            var letterList = new List<CharInfo>();
            var startIndex = currentPage == 0 ? 0 : 1;
            var endIndex = currentLetterTermList.Count;
            var lastChildLastPage = currentPage > 0 && currentLetterTermList.Count > 0 ? currentLetterTermList[0] : null;
            DTO_Term firstChildNextPage = null;
            var lastPageIndex = records%pageSize == 0 ? (records/pageSize) - 1 : (records/pageSize);

            if (currentLetterTermList.Count == pageSize + 1)
                firstChildNextPage = currentLetterTermList[currentLetterTermList.Count - 1];
            else if (currentLetterTermList.Count == pageSize + 2)
                firstChildNextPage = currentLetterTermList[currentLetterTermList.Count - 1];

            for (var index = startIndex; index < endIndex; index++)
            {
                var currentTerm = currentLetterTermList[index];

                if (filteredTerms.Count >= pageSize)
                    break;

                filteredTerms.Add(currentTerm);

                if (!usedletterList.Contains(currentTerm.FirstLetter.ToString()))
                {
                    usedletterList.Add(currentTerm.FirstLetter.ToString());
                    var wordInPreviousPage = lastChildLastPage != null && lastChildLastPage.FirstLetter == currentTerm.FirstLetter;
                    var wordInNextPage = firstChildNextPage != null && firstChildNextPage.FirstLetter == currentTerm.FirstLetter;
                    if (lastPageIndex == currentPage || records < pageSize)
                        wordInNextPage = false;

                    letterList.Add(new CharInfo(currentTerm.FirstLetter.ToString(), wordInPreviousPage, wordInNextPage));
                }
            }
            View.BindRepeaterList(filteredTerms, currentChar, records, currentPage, letterList);
        }

        public List<Int64> GetIDGlossary()
        {
            var listValidId = new List<Int64>();
            var glossaryList = Service.GetDTO_GlossaryListOrdered(View.IdCommunity, false);

            var person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            var module = Service.GetPermissions(View.IdCommunity, person);

            // Calcolo i permessi di tutti i glossari
            foreach (var dtoGlossary in glossaryList)
                Service.UpdateGlossaryPermission(module, dtoGlossary, person, View.IdCommunity);

            // Carico tutti i glossari pubblici, pubblicati e non della mia comunità
            var publicGlossaryList = Service.GetDTO_GlossaryListFromliteGlossary(f => f.IdCommunity != View.IdCommunity && f.Deleted == BaseStatusDeleted.None && f.IsPublic && f.IsPublished);

            // li metto in sola visualizzazione
            // non verifico al momento i permessi che ho nella comunità di origine del glossario
            foreach (var dtoGlossary in publicGlossaryList)
                dtoGlossary.Permission.ViewTerm = module.ViewTerm;

            // Dovrei aver concatenato tutti i permessi e quindi
            // invio dati alla vista solo con i glossari visibili

            listValidId.AddRange(glossaryList.Where(f => f.Permission.ViewTerm).Select(f => f.Id).ToList());
            listValidId.AddRange(publicGlossaryList.Select(f => f.Id).ToList());
            return listValidId;
        }

        public Boolean VirtualDeleteTerm(Int64 idTerm, out String errors)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                errors = String.Empty;
                return false;
            }
            var result = Service.DeleteVirtualTerm(idTerm, out errors);
            if (result)
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), idTerm, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.VirtualDeleteTerm);
            return result;
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossarySearch View
        {
            get { return (IViewGlossarySearch) base.View; }
        }

        private ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        private Int32 CurrentIdModule
        {
            get
            {
                if (currentIdModule == 0)
                    currentIdModule = CurrentManager.GetModuleID(ModuleGlossaryNew.UniqueCode);
                return currentIdModule;
            }
        }

        public GlossarySearchPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossarySearchPresenter(iApplicationContext oContext, IViewGlossarySearch view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}