using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
namespace lm.Comol.Core.BaseModules.CommonControls
{
    public class AlphabetSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewAlphabetSelector View
            {
                get { return (IViewAlphabetSelector)base.View; }
            }
            public AlphabetSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AlphabetSelectorPresenter(iApplicationContext oContext, IViewAlphabetSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(List<String> availableWords, String activeWord, AlphabetDisplayMode displayMode = AlphabetDisplayMode.none)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (displayMode == AlphabetDisplayMode.none)
                    displayMode = AlphabetDisplayMode.commonletters | AlphabetDisplayMode.allCharsItem | AlphabetDisplayMode.otherCharsItem | AlphabetDisplayMode.addUnmatchLetters;
                View.DisplayMode = displayMode;
                LoadItems(availableWords, activeWord, displayMode);
            }
        }


        private void LoadItems(List<String> availableWords, String activeWord, AlphabetDisplayMode displayMode)
        {
            Boolean hasOtherChars = false;
            List<AlphabetItem> items = new List<AlphabetItem>();
            List<AlphabetItem> otherChars = new List<AlphabetItem>();

            if (displayMode.IsFlagSet(AlphabetDisplayMode.commonletters) || displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                items = (from n in Enumerable.Range(97, 26) select new AlphabetItem() { isEnabled = false, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
            if (displayMode.IsFlagSet(AlphabetDisplayMode.extendedletters))
                items.AddRange(GetOtherAlphabetItems(false));
            if (displayMode.IsFlagSet(AlphabetDisplayMode.addUnmatchLetters))
                otherChars = GetOtherAlphabetItems(true);

            foreach (String l in availableWords)
            {
                if (items.Where(i => i.Value == l).Any())
                    items.Where(i => i.Value == l).FirstOrDefault().isEnabled = true;
                else if (System.Text.RegularExpressions.Regex.IsMatch(l, @"[^\w\.@-]", System.Text.RegularExpressions.RegexOptions.None) && displayMode.IsFlagSet(AlphabetDisplayMode.addUnmatchLetters))
                {
                    String upper = "";
                    try
                    {
                        upper = l.ToUpper();
                    }
                    catch (Exception ex)
                    {
                        upper = l;
                    }
                    items.Add(new AlphabetItem() { isEnabled = true, Value = l, DisplayName = upper });
                }
                else if (otherChars.Where(i => i.Value == l).Any())
                    items.AddRange(otherChars.Where(i => i.Value == l).ToList());
                else
                    hasOtherChars = true;
            }

            items = items.OrderBy(i => i.Value).ToList();

            if (displayMode.IsFlagSet(AlphabetDisplayMode.otherCharsItem))
                items.Insert(0, new AlphabetItem() { Type = AlphabetItemType.otherChars, isEnabled = hasOtherChars, Value = "#", DisplayName = "" });
            if (displayMode.IsFlagSet(AlphabetDisplayMode.allCharsItem))
                items.Insert(0, new AlphabetItem() { DisplayAs = AlphabetItem.AlphabetItemDisplayAs.first, isEnabled = true, Type = AlphabetItemType.all, Value="" });
            items.LastOrDefault().DisplayAs = AlphabetItem.AlphabetItemDisplayAs.last;

            if (activeWord != "-1")
            {
                if (items.Where(i => i.Value == activeWord && i.isEnabled).Any())
                    items.Where(i => i.Value == activeWord && i.isEnabled).FirstOrDefault().isSelected = true;
                else if (items.Where(i => i.Value == activeWord && !i.isEnabled).Any() && displayMode.IsFlagSet(AlphabetDisplayMode.allCharsItem))
                {
                    items.Where(i => i.Type == DomainModel.Helpers.AlphabetItemType.all).FirstOrDefault().isSelected = true;
                    activeWord = "";
                }
                else
                    activeWord = "";
                View.SelectedItem = activeWord;
            }

            View.LoadItems(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultEnable"></param>
        /// <returns></returns>
        private static List<AlphabetItem> GetOtherAlphabetItems(Boolean defaultEnable)
        {
            return (from n in Enumerable.Range(222, 34) select new AlphabetItem() { isEnabled = defaultEnable, Value = Char.ConvertFromUtf32(n), DisplayName = Char.ConvertFromUtf32(n).ToString().ToUpper(), isSelected = false }).ToList();
        }
    }
}