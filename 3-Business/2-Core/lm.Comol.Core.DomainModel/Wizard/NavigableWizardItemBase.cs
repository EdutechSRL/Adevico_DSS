using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Wizard
{
    [Serializable]
    public class NavigableWizardItemBase<T>
    {
        const String SPACE = " ";

        /// <summary>
        ///     Element Id
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        ///     Position into the list
        /// </summary>
        public Int32 DisplayOrder { get; set; }

        /// <summary>
        ///     Render Visibility
        /// </summary>
        public Boolean Visible { get; set; }

        /// <summary>
        ///     Extra CssClass if needed
        /// </summary>
        public String CssClass { get; set; }

        /// <summary>
        ///     CssClass for emulate CSS3 (:first/:last) ex: "first" "" "last" (in future, "odd", "even", "last odd", "last even")
        /// </summary>
        public DisplayOrderEnum DisplayOrderDetail { get; set; }

        /// <summary>
        ///     Status icon enum
        /// </summary>
        public WizardItemStatus Status { get; set; }

        public NavigableWizardItemBase()
        {
            DisplayOrderDetail = DisplayOrderEnum.none;
        }

        public String Css
        {
            get
            {
                String retval = "";

                retval = "navigationitem" + SPACE;

                if (this is NavigableWizardItem<T>)
                {
                    NavigableWizardItem<T> nvitem = (this as NavigableWizardItem<T>);

                    retval += nvitem.Status.ToString() + SPACE;

                    retval += nvitem.Active ? "active" + SPACE : "";

                    //retval += nvitem.Status == WizardItemStatus.disabled ? nvitem.Status.ToString() + SPACE : "";
                }
                else if (this is NavigableWizardItemSeparator<T>)
                {
                    retval += "separator" + SPACE;
                }

                switch (DisplayOrderDetail) { 
                    case DisplayOrderEnum.first:
                    case DisplayOrderEnum.last:
                        retval += this.DisplayOrderDetail.ToString() + SPACE;
                        break;
                    case DisplayOrderEnum.none:
                        break;
                    default:
                        retval += DisplayOrderEnum.first.ToString() + SPACE + DisplayOrderEnum.last.ToString() + SPACE;
                        break;
                }
            
                retval = retval + SPACE + this.CssClass + SPACE;

                // Trimming (removing double spaces, and start/end spaces)
                retval = retval.Replace(SPACE + SPACE, SPACE).Trim();

                return retval;
            }
        }
    }

    [Serializable]
    public class NavigableWizardItemSeparator <T> : NavigableWizardItemBase<T>
    {
    }

    [Serializable]
    public class NavigableWizardItem<T> : NavigableWizardItemBase<T>
    {
        /// <summary>
        ///     Visible title
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Browser tooltip
        /// </summary>
        public String Tooltip { get; set; }

        /// <summary>
        ///     Status Message
        /// </summary>
        public String Message { get; set; }

       

        /// <summary>
        ///     Current step
        /// </summary>
        public Boolean Active { get; set; }

        /// <summary>
        ///     Item url
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        ///     Allow AutoPostBack
        /// </summary>
        public Boolean AutoPostBack { get; set; }
    }
    [Flags]
    public enum WizardItemStatus
    {
        none = 0,
        valid = 1,
        error = 2,
        warning = 4,
        disabled = 8
    }

    public enum DisplayOrderEnum
    {
        none = 0,
        first = 1,
        last = 2
    }

}