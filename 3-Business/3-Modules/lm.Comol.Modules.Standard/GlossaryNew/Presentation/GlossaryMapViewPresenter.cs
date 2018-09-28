using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation
{
    public class GlossaryMapViewPresenter : DomainPresenter
    {
        public void InitView()
        {
            var idCommunity = View.PreloadIdCommunity;
            var idGlossary = View.PreloadIdGlossary;

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
                        var words = Service.GetTermUsedLetters(View.IdGlossary, out wordUsingDictionary);
                        View.LoadViewData(glossary, words, View.IdCommunity, (module.Administration || module.ManageGlossary), View.PreloadForManagement, View.LoadfromCookies, View.IdCookies);
                        return;
                    }
                }
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
            }
        }

        public void ChangeLetter(String letter, GlossaryFilter filter)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            if (String.IsNullOrEmpty(letter))
                letter = " ";

            var words = new List<String>();
            var currentChar = letter[0];
            var wordAlls = new List<String>();
            var currentLetterTermList = Service.GetDTO_TermListMapFromliteTerm(f => f.IdGlossary == View.IdGlossary && f.Deleted == BaseStatusDeleted.None, f => f.Name, currentChar, filter, out wordAlls);
            if (!String.IsNullOrWhiteSpace(letter))
                words.Add(letter);
            else
                words.AddRange(currentLetterTermList.Select(dtoTermMap => dtoTermMap.FirstLetter.ToString()));

            View.BindRepeaterList(words, wordAlls, currentLetterTermList, currentChar);
        }

        public Boolean VirtualDeleteTerm(Int64 idTerm, out String errors)
        {
            errors = String.Empty;

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
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

        protected virtual IViewGlossaryMapView View
        {
            get { return (IViewGlossaryMapView) base.View; }
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

        public GlossaryMapViewPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public GlossaryMapViewPresenter(iApplicationContext oContext, IViewGlossaryMapView view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        #endregion
    }
}