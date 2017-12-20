using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Filters
{
    [Serializable]
    public class Filter
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string Placeholder { get; set; }

        public Int32 GridSize { get; set; }

        public string Type
        {
            get
            {
                return FilterType.ToString().ToLower();
            }
        }
        public FilterType FilterType { get; set; }

        public List<FilterListItem> Values { get; set; }

        public Boolean AutoUpdate { get; set; }

        public String Value { get; set; }

        public string CssClass { get; set; }

        public FilterListItem Selected { get; set; }

        public Int64 SelectedId { get; set; }

        public List<FilterListItem> SelectedIds { get; set; }


        public Int32 DisplayOrder { get; set; }

        public Filter()
        {
            GridSize = 6;
            Values = new List<FilterListItem>();
            SelectedIds = new List<FilterListItem>();
        }

        public Filter(String name, FilterListItem[] items)
        {
            GridSize = 6;
            Values = items.ToList();
            this.Name = name;
            SelectedIds = new List<FilterListItem>();
        }

        public Filter(params FilterListItem[] items)
        {
            GridSize = 6;
            SelectedIds = new List<FilterListItem>();
            Values = items.ToList();
        }
    }

    [Serializable]

    public class FilterListItem
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public Boolean Checked { get; set; }

        public Boolean Disabled { get; set; }
        public FilterListItem()
        {

        }
        public FilterListItem(Int64 id, String name, Boolean selected = false)
        {
            this.Id = id;
            this.Name = name;
            this.Checked = selected;
        }
    }

    [Serializable]
    public class KeyValue
    {
        public String key { get; set; }
        public String value { get; set; }
        public String title { get; set; }
    }

    public enum FilterType
    {
        Text = 0,
        Radio = 1,
        Checkbox = 2,
        Select = 3,
        TextSelect = 4,
        MultiSelect = 5,
        MaskedRadio = 6
    }
}
