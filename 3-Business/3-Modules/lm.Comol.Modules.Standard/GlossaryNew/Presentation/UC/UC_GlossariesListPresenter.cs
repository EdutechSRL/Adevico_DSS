using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.Standard.GlossaryNew.Business;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;
using lm.Comol.Modules.Standard.GlossaryNew.Domain.dto;
using lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC
{
    public class UC_GlossariesListPresenter : DomainPresenter
    {
        public void InitView(Boolean forManagement)
        {
            var idCommunity = View.PreloadIdCommunity;

            if (idCommunity <= 0)
                idCommunity = UserContext.CurrentCommunityID;

            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
            }
            else
            {
                var module = Service.GetPermissions(idCommunity, litePerson);
                View.IdCommunity = idCommunity;
                View.ForManagement = forManagement && (module.Administration || module.ManageGlossary);

                if (module.ViewTerm || module.Administration || module.ManageGlossary)
                    UpdateView(litePerson, module);
                else
                {
                    View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                    View.DisplayNoPermission(View.IdCommunity, Service.GetServiceIdModule());
                }
            }
        }

        public void ShowFilterResult(GlossaryFilter searchFilter)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            if (searchFilter == null)
            {
                InitView(View.ForManagement);
            }
            else
            {
                View.GoToGlossarySearch();
            }
        }

        public String GetLanguageCode(Int32 idLanguage, String defaultValue)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return String.Empty;
            }
            if (LanguageDictionaryType == null)
                LanguageDictionaryType = Service.GetLanguageDictionaryCodes();

            if (LanguageDictionaryType.ContainsKey(idLanguage))
                return LanguageDictionaryType[idLanguage];
            return defaultValue;
        }

        public string GetLanguageDescription(int idLanguage, String defaultValue)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return String.Empty;
            }
            if (LanguageDictionaryTypeDescription == null)
                LanguageDictionaryTypeDescription = Service.GetLanguageDictionaryDescriptions();

            if (LanguageDictionaryTypeDescription.ContainsKey(idLanguage))
                return LanguageDictionaryTypeDescription[idLanguage];
            return defaultValue;
        }

        public void UpdateView(litePerson person = null, ModuleGlossaryNew module = null)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            if (person == null)
                person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (module == null)
                module = Service.GetPermissions(View.IdCommunity, person);

            // Creo dizionario contentente idLingua - Code es 1-IT 2-EN 3-DE ...
            LanguageDictionaryType = Service.GetLanguageDictionaryCodes();
            LanguageDictionaryTypeDescription = Service.GetLanguageDictionaryDescriptions();

            // Carico tutti i glossari dalla tabella Glossary Display Order quindi nell'ordine corretto, senza verifica dei permessi e distinzione fra condivisi o meno
            // Se per gestione mostro anche i non pubblicati altrimenti solo i pubblicati
            List<DTO_Glossary> glossaryList = null;


            //if (View.ForManagement)
            //glossaryList = Service.GetActiveDTO_GlossaryListOrdered(f => f.IdCommunity == View.IdCommunity && f.Deleted == BaseStatusDeleted.None && f.Glossary.Deleted == BaseStatusDeleted.None);
            glossaryList = Service.GetDTO_GlossaryListOrdered(View.IdCommunity, View.ForManagement);

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
            View.LoadViewData(glossaryList.Where(f => f.Permission.ViewTerm).OrderBy(f => f.DisplayOrder).ToList(), publicGlossaryList);
        }

        public void ShowError(String error)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            View.SetErrorMessage(MessageType.alert, error);
        }

        public void VirtualDeleteGlossary(Int64 idGlossary)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            String errors;
            if (Service.DeleteVirtualGlossaryDisplayOrder(idGlossary, out errors))
            {
                UpdateView();
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), idGlossary, ModuleGlossaryNew.ActionType.VirtualDeleteGlossary);
                View.SetErrorMessage(MessageType.success, "Glossary.deleted", Service.GetGlossaryName(idGlossary));
            }
            else
                View.SetErrorMessage(MessageType.alert, errors);
        }

        public void Sort(List<DTO_Glossary> glossaryList, List<DTO_Glossary> publicGlossaryList, SortObject sortObject, String commandName)
        {
            var litePerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
            if (UserContext.isAnonymous || litePerson == null)
            {
                View.SendUserAction(View.IdCommunity, Service.GetServiceIdModule(), ModuleGlossaryNew.ActionType.NoPermission);
                View.DisplaySessionTimeout();
                return;
            }
            if (commandName == "Community")
            {
                if (sortObject.Key == "default")
                    glossaryList = glossaryList.OrderBy(f => f.DisplayOrder).ToList();
                else if (sortObject.Key == "alphabetical")
                    glossaryList = glossaryList.OrderBy(f => f.Name).ToList();
                else
                    glossaryList = glossaryList.OrderByDescending(f => f.LastUpdate).ToList();
            }
            else
            {
                if (sortObject.Key == "default")
                    publicGlossaryList = publicGlossaryList.OrderBy(f => f.DisplayOrder).ToList();
                else if (sortObject.Key == "alphabetical")
                    publicGlossaryList = publicGlossaryList.OrderBy(f => f.Name).ToList();
                else
                    publicGlossaryList = publicGlossaryList.OrderByDescending(f => f.LastUpdate).ToList();
            }


            View.LoadViewData(glossaryList, publicGlossaryList);
        }

        #region "Properties"

        private ServiceGlossary service;

        public virtual BaseModuleManager CurrentManager { get; set; }
        private Int32 currentIdModule;

        protected virtual IViewUC_GlossariesList View
        {
            get { return (IViewUC_GlossariesList) base.View; }
        }

        public ServiceGlossary Service
        {
            get
            {
                if (service == null)
                    service = new ServiceGlossary(AppContext);
                return service;
            }
        }

        public UC_GlossariesListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public UC_GlossariesListPresenter(iApplicationContext oContext, IViewUC_GlossariesList view)
            : base(oContext, view)
        {
            CurrentManager = new BaseModuleManager(oContext);
        }

        public Dictionary<Int32, String> LanguageDictionaryType { get; set; }
        public Dictionary<Int32, String> LanguageDictionaryTypeDescription { get; set; }

        #endregion
    }
}

[Serializable]
public class SortObject
{
    public SortObject()
    {
    }

    public SortObject(String key, Func<DTO_Glossary, String> sortProperty)
    {
        Key = key;
        SortProperty = sortProperty;
    }

    public String Key { get; set; }
    public Func<DTO_Glossary, String> SortProperty { get; set; }
}