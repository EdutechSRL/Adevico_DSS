using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoMacUrlUserAttribute
    {
        private String _QueryValue;
        public virtual long Id { get; set; }
        public virtual String QueryName { get; set; }
        public virtual String QueryValue { 
            get {
                if (Type == UrlMacAttributeType.compositeProfile)
                {
                    if (ComposedBy.Count == 1)
                        return ComposedBy[0].Attribute.QueryValue;
                    else if (ComposedBy.Count > 1)
                        return string.Join(CharComposer, ComposedBy.Where(a=>a.Attribute !=null).OrderBy(c => c.DisplayOrder).Select(c => c.Attribute.QueryValue).ToArray());
                    else
                        return "";
                }
                else
                    return _QueryValue;
            }
            set {
                _QueryValue = value;
            }
        }
        public virtual UrlMacAttributeType Type { get; set; }
        public virtual Boolean isEmpty { get { return String.IsNullOrEmpty(QueryValue); }}
        public virtual Boolean isIdentifier { get; set; }
        public virtual List<dtoComposerAttribute> ComposedBy { get; set; }
        public virtual String CharComposer{ get; set; }
        public dtoMacUrlUserAttribute()
        {
            ComposedBy = new List<dtoComposerAttribute>();
        }
    
    }
    [Serializable]
    public class dtoComposerAttribute
    {
        public virtual dtoMacUrlUserAttribute Attribute { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public dtoComposerAttribute()
        {
        }
    }
}