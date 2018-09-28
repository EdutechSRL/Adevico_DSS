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
    public class GlossaryViewPresenter : DomainPresenter
    {
        private const String AllChar = " ";

        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;
            var pageIndex = View.PreloadPageIndex;

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
                View.IdGlossary = idGlossary;
                var module = Service.GetPermissions(View.IdCommunity, litePerson);
                // Verifico Permessi modulo
                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                {
                    var glossary = Service.GetDTO_Glossary(View.IdGlossary, View.IdCommunity);
                    Service.UpdateGlossaryPermission(module, glossary, litePerson, View.IdCommunity);
                    // Verifico permessi specifici del glossario
                    if (glossary.Permission.ViewTerm)
                    {
                        Dictionary<Char, UInt16> wordUsingDictionary;
                        //var words = Service.GetTermUsedLetters(idGlossary, out wordUsingDictionary);
                        var usedLetters = Service.GetGlossaryUsedLetters(View.IdGlossary);

                        View.LoadViewData(glossary, usedLetters, View.IdCommunity, (module.Administration || module.ManageGlossary), View.PreloadForManagement, View.LoadfromCookies, View.IdCookies, pageIndex);

                        return;
                    }
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
            Dictionary<string, CharInfo> usedLetters;
            var currentLetterTermList = Service.GetDTO_TermListFromliteTerm(f => f.IdGlossary == View.IdGlossary && f.Deleted == BaseStatusDeleted.None, f => f.Name, currentPage, currentChar, pageSize, filter, out records, out usedLetters).ToList();
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

            View.BindRepeaterList(filteredTerms, usedLetters, currentChar, records, currentPage, letterList);
        }

        public Boolean VirtualDeleteTerm(Int64 idTerm, out String errors)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            errors = String.Empty;
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();

                return false;
            }

            var module = Service.GetPermissions(View.IdCommunity, litePerson);
            if (module.DeleteTerm || module.Administration || module.ManageGlossary)
            {
                var result = Service.DeleteVirtualTerm(idTerm, out errors);
                if (result)
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), idTerm, ModuleGlossaryNew.ObjectType.Term, ModuleGlossaryNew.ActionType.VirtualDeleteTerm);
                return result;
            }
            View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
            View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());


            return false;
        }

        #region Initialize

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewGlossaryView View
        {
            get { return (IViewGlossaryView) base.View; }
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

        public GlossaryViewPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryViewPresenter(iApplicationContext oContext, IViewGlossaryView view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}