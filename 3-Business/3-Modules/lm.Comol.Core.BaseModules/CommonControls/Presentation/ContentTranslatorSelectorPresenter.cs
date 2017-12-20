using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
namespace lm.Comol.Core.BaseModules.CommonControls.Presentation
{
    public class ContentTranslatorSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _ModuleID;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewContentTranslatorSelector View
            {
                get { return (IViewContentTranslatorSelector)base.View; }
            }
            public ContentTranslatorSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ContentTranslatorSelectorPresenter(iApplicationContext oContext, IViewContentTranslatorSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(List<BaseLanguageItem> availableLanguages)
        {
            InitView(availableLanguages, new List<LanguageItem>(), null);
        }
        public void InitView(List<lm.Comol.Core.DomainModel.Language> availableLanguages, Boolean alsoMultilanguage)
        {
            List<BaseLanguageItem> items = (from l in availableLanguages select new BaseLanguageItem(l) ).ToList();
            if (alsoMultilanguage)
                items.Add(View.GetMultiLanguageItem());

            InitView(items , new List<LanguageItem>(), null);
        }
        public void InitView(List<lm.Comol.Core.DomainModel.Language> availableLanguages, List<Int32> inUseItems, Boolean alsoMultilanguage = false, Boolean selectMultilanguage = false, Int32 idSelected = -1)
        {
            List<BaseLanguageItem> items = (from l in availableLanguages select new BaseLanguageItem(l)).ToList();
            if (alsoMultilanguage)
                items.Add(View.GetMultiLanguageItem());
            List<LanguageItem> inUse = items.Where(i=> inUseItems.Contains(i.Id) || (alsoMultilanguage && i.IsMultiLanguage) ).Select(i=>new LanguageItem(i)).ToList();
            InitView(items, inUse, (selectMultilanguage) ? inUse.Where(i=>i.IsMultiLanguage).FirstOrDefault() : inUse.Where(i=>i.Id== idSelected).FirstOrDefault());
        }
        public void InitView(List<lm.Comol.Core.DomainModel.Language> availableLanguages, List<String> inUseItems, Boolean alsoMultilanguage = false, Boolean selectMultilanguage = false, String selectedCode = "")
        {
            List<BaseLanguageItem> items = (from l in availableLanguages select new BaseLanguageItem(l)).ToList();
            if (alsoMultilanguage)
                items.Add(View.GetMultiLanguageItem());
            List<LanguageItem> inUse = items.Where(i => inUseItems.Contains(i.Code) || (alsoMultilanguage && i.IsMultiLanguage)).Select(i => new LanguageItem(i)).ToList();
            InitView(items, inUse, (selectMultilanguage) ? inUse.Where(i => i.IsMultiLanguage).FirstOrDefault() : inUse.Where(i => i.Code == selectedCode).FirstOrDefault());
        }
        public void InitView(List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUseItems, LanguageItem current)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.IsInitialized = true;
                if (inUseItems.Where(i => i.IsMultiLanguage && (String.IsNullOrEmpty(i.Name) || String.IsNullOrEmpty(i.ToolTip))).Any()) {
                    LanguageItem multi = inUseItems.Where(i => i.IsMultiLanguage && (String.IsNullOrEmpty(i.Name) || String.IsNullOrEmpty(i.ToolTip))).FirstOrDefault();
                    multi.Name = View.MultiName;
                    multi.ToolTip = View.MultiToolTip;
                }
                View.FirstItemToLoad = null;
                LoadItems(availableLanguages, inUseItems, current);
            }
        }
        public BaseLanguageItem AddLanguage(Int32 idLanguage, String codeLanguage)
        {
            BaseLanguageItem item = null;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<BaseLanguageItem> languages = View.AvailableLanguages;
                List<LanguageItem> inUseItems = View.InUseLanguages;
                item = languages.Where(l => l.Id == idLanguage && l.Code == codeLanguage).FirstOrDefault();
                if (item != null)
                {
                    LanguageItem current = new LanguageItem(item);
                    current.IsSelected = true;
                    if (!inUseItems.Where(i=> i.Id==current.Id && i.IsMultiLanguage== current.IsMultiLanguage && i.Code== current.Code).Any())
                        inUseItems.Add(current);
                    LoadItems(languages, inUseItems, current);
                }  
            }
            return item;
        }

        public LanguageItem SelectLanguage(Int32 idLanguage, String codeLanguage)
        {
            LanguageItem item = null;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<LanguageItem> inUseItems = View.InUseLanguages;
                item = inUseItems.Where(l => l.Id == idLanguage && l.Code == codeLanguage).FirstOrDefault();
                if (item != null)
                {
                    inUseItems.ForEach(i => i.IsSelected = false);
                    LanguageItem current = item;
                    current.IsSelected = true;
                    inUseItems = inUseItems.OrderByDescending(i => i.IsMultiLanguage).ThenBy(i => i.Name).ToList();
                    View.SelectedItem = current;
                    View.InUseLanguages = inUseItems;
                    View.LoadItems(inUseItems);
                }  
            }
            return item;
        }
        private void LoadItems(List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUseItems, LanguageItem current)
        {
            BaseLanguageItem firstItemToLoad = View.FirstItemToLoad;
            List<BaseLanguageItem> toLoad = new List<BaseLanguageItem>();
            View.SelectedItem = current;
            View.AvailableLanguages = availableLanguages;
            if (availableLanguages.Any())
            {
                toLoad = availableLanguages.Where(l=> (inUseItems==null || !inUseItems.Where(u=>u.Id== l.Id && u.Code==l.Code).Any())).OrderByDescending(i => i.IsMultiLanguage).ThenBy(i => i.Name).ToList();
                if (toLoad.Count == 1)
                {
                    toLoad[0].DisplayAs = (ItemDisplayAs.first | ItemDisplayAs.last);
                    toLoad[0].IsSelected = true;
                }
                else if (toLoad.Count > 1)
                {
                    toLoad.First().DisplayAs = ItemDisplayAs.first;
                    toLoad.Last().DisplayAs = ItemDisplayAs.last;
                }
            }

            if (inUseItems.Any())
            {
                inUseItems = inUseItems.OrderByDescending(i => i.IsMultiLanguage).ThenBy(i => i.Name).ToList();
                if (inUseItems.Count == 1)
                    inUseItems[0].DisplayAs = (ItemDisplayAs.first | ItemDisplayAs.last);
                else
                {
                    inUseItems.First().DisplayAs = ItemDisplayAs.first;
                    inUseItems.Last().DisplayAs = ItemDisplayAs.last;
                }
                inUseItems.ForEach(i => i.IsSelected = (current != null && current.Id == i.Id && current.Code == i.Code));
            }
            BaseLanguageItem item = null;
            if (firstItemToLoad == null) { 
                item = (toLoad.Where(t=>t.IsDefault).Any() ? toLoad.Where(t=>t.IsDefault).FirstOrDefault() :toLoad.Where(t=>t.Code== UserContext.Language.Code).FirstOrDefault());
                if (item != null) {
                    toLoad.ForEach(i => i.IsSelected = false);
                    item.IsSelected = true;
                    View.FirstItemToLoad = item;
                }
            }
            else if (firstItemToLoad.IsDefault && !toLoad.Where(t => t.IsDefault).Any() && toLoad.Where(t => t.Code == UserContext.Language.Code).Any()) {
                item = toLoad.Where(t => t.Code == UserContext.Language.Code).FirstOrDefault();
                if (item != null)
                {
                    toLoad.ForEach(i => i.IsSelected = false);
                    item.IsSelected = true;
                    View.FirstItemToLoad = item;
                }
            }

            //if (selected !=null)
            //    selected.s

            //IsSelected
            //String dCode = (toLoad.Where(t=>t.IsDefault).Any() ? toLoad.Where(t=>t.IsDefault).FirstOrDefault().Code,
            View.InUseLanguages = inUseItems;
            View.LoadItems(toLoad, inUseItems, current);
        }

        public LanguageItem RemoveCurrent(LanguageItem current)
        {
            List<BaseLanguageItem> availableLanguages = View.AvailableLanguages;
            List<LanguageItem> inUseItems = View.InUseLanguages;
            Int32 index = (inUseItems==null) ? -1 : inUseItems.Select(i=>i.Code).ToList().IndexOf(current.Code);

            if (index == 0 && inUseItems.Count == 1)
            {
                current = null;
                inUseItems = new List<LanguageItem>();
            }
            else if (index == 0 && inUseItems.Count > 1) {
                current = inUseItems[1];
                inUseItems.RemoveAt(0);
            }
            else if (inUseItems.Count > 1 && index >-1)
            {
                current = inUseItems[index - 1];
                inUseItems.RemoveAt(index);
            }

            LoadItems(availableLanguages, inUseItems, current);
            return current;
        }
    }
}