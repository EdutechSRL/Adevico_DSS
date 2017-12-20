   using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    namespace lm.Comol.Core.DomainModel
    {
        [Serializable(), CLSCompliant(true)]
        public class FilterParameter
        {
            public Int32 DisplayOrder
            {
                get { return m_DisplayOrder; }
                set { m_DisplayOrder = value; }
            }
            private Int32 m_DisplayOrder;
            public string PropertyName
            {
                get { return m_PropertyName; }
                set { m_PropertyName = value; }
            }
            private string m_PropertyName;
            public string DisplayAs
            {
                get { return m_DisplayAs; }
                set { m_DisplayAs = value; }
            }
            private string m_DisplayAs;
            public string DefaultValue
            {
                get { return m_DefaultValue; }
                set { m_DefaultValue = value; }
            }

            private string m_DefaultValue;
            protected string Container()
            {
                return "<li class=\"filter\" id=\"filter_" + Convert.ToString(DisplayOrder) + "\">{0}</li>";
            }

            protected string Label()
            {
                return "<span class=\"filterLabel\">{0}</span>";
            }

            public virtual string GenerateHTML()
            {
                return String.Format(Container(), String.Format(Label(), this.DisplayAs) + String.Format("<br /><input class=\"filterInput\" type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", this.DisplayAs, this.DefaultValue));
            }

            public override string ToString()
            {
                return this.GenerateHTML();
            }
        }


        public class CheckBoxFilterParameter : FilterParameter
        {
            public override string GenerateHTML()
            {
                return base.GenerateHTML();
            }
        }

        public class ValuesFilterParameter : FilterParameter
        {
            public IList<KeyValuePair<String, String>> Values
            {
                get { return m_Values; }
                set { m_Values = value; }
            }

            private IList<KeyValuePair<String, String>> m_Values;
            public override string GenerateHTML()
            {
                return base.GenerateHTML();
            }
        }

        public class RadiobuttonListFilterParameter : ValuesFilterParameter
        {
            public override string GenerateHTML()
            {
                return base.GenerateHTML();
            }
        }

        public class DropdownFilterParameter : ValuesFilterParameter
        {
            public override string GenerateHTML()
            {
                return base.GenerateHTML();
            }
        }

    }
