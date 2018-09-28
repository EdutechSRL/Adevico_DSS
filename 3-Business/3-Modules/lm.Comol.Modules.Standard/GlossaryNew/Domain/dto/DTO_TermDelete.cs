using System;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_TermDelete
    {
        public DTO_TermDelete(Int64 id, String name, DateTime? deleteOn, litePerson deleteBy)
        {
            Id = id;
            Name = name;
            if (deleteOn.HasValue)
                DeletedOn = deleteOn.Value;
            if (deleteBy != null)
                DeletedBy = deleteBy.Name;
        }

        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0} Name: {1}", Id, Name);
        }
    }
}