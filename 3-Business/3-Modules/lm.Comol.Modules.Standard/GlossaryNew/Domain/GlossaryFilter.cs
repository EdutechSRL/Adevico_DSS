using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class GlossaryFilter
    {
        private string _lemmaContentString;
        private FilterTypeEnum _lemmaSearchType;
        private string _lemmaString;
        private FilterVisibilityTypeEnum _lemmaVisibilityType;
        private string _searchString;

        public GlossaryFilter()
        {
            LemmaString = String.Empty;
            LemmaContentString = String.Empty;
            LemmaSearchType = FilterTypeEnum.Contains;
            LemmaVisibilityType = FilterVisibilityTypeEnum.Published;
        }

        public String SearchString
        {
            get
            {
                if (_searchString != null) return _searchString.ToLower();
                return String.Empty;
            }
            set { _searchString = value; }
        }

        public String LemmaString
        {
            get
            {
                if (_lemmaString != null) return _lemmaString.ToLower();
                return String.Empty;
            }
            set { _lemmaString = value; }
        }

        public String LemmaContentString
        {
            get
            {
                if (_lemmaContentString != null) return _lemmaContentString.ToLower();
                return String.Empty;
            }
            set { _lemmaContentString = value; }
        }

        public FilterTypeEnum LemmaSearchType
        {
            get { return _lemmaSearchType; }
            set { _lemmaSearchType = value; }
        }

        public FilterVisibilityTypeEnum LemmaVisibilityType
        {
            get { return _lemmaVisibilityType; }
            set { _lemmaVisibilityType = value; }
        }
    }
}