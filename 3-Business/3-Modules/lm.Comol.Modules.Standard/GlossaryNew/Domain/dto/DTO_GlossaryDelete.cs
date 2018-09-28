using System;
using System.Text;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_GlossaryDelete
    {
        private liteGlossary item;

        public DTO_GlossaryDelete(Glossary item, Int32 idCommunity, Int64 idShare)
        {
            Id = item.Id;
            IdShare = idShare;
            Name = item.Name;
            if (item.ModifiedOn.HasValue)
                DeletedOn = item.ModifiedOn.Value;
            if (item.ModifiedBy != null)
                DeletedBy = item.ModifiedBy.Name;
            TermsCount = item.TermsCount;

            var sb = new StringBuilder();

            if (item.IdCommunity == idCommunity)
                sb.Append("internal ");
            else
                sb.Append("external ");

            if (!item.IsPublished)
                sb.Append(" unpublished ");

            //if (item.IsDefault)
            //    sb.Append(" default ");
            //else if (item.IsDefault)
            //    sb.Append(" public ");
            //else
            sb.Append(" normal ");

            //if (!item.Shared)
            //    sb.Append(" shared ");

            Type = sb.ToString();
        }

        public Int64 Id { get; set; }
        public Int64 IdShare { get; set; }
        public String Name { get; set; }
        public String DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public Int32 TermsCount { get; set; }
        public String Type { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0} Name: {1}", Id, Name);
        }
    }
}