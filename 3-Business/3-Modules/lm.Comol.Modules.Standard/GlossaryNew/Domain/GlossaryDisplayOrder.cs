using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    [Serializable]
    public class GlossaryDisplayOrder : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Glossary Glossary { get; set; }
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual bool IsDefault { get; set; }

        public override string ToString()
        {
            long idGlossary = -1;
            var nameGlossary = String.Empty;
            if (Glossary != null)
            {
                idGlossary = Glossary.Id;
                nameGlossary = Glossary.Name;
            }
            return string.Format("DisplayOrder: {0} IdGlossary:{1} Name:{2} IdCommunity: {3} ", DisplayOrder, idGlossary, nameGlossary, IdCommunity);
        }
    }
}