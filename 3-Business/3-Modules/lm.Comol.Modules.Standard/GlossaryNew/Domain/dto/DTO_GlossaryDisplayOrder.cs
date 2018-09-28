using System;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
{
    [Serializable]
    public class DTO_GlossaryDisplayOrder
    {
        public GlossaryPermission Permission;

        public DTO_GlossaryDisplayOrder(GlossaryDisplayOrder source)
        {
            Id = source.Id;
            IdCommunity = source.IdCommunity;
            DisplayOrder = source.DisplayOrder;
            IdGlossary = source.Glossary.Id;
            Glossary = new DTO_Glossary(source.Glossary);
            Permission = new GlossaryPermission();
            Permission.AddTerm = true;
            Permission.EditTerm = true;
            Permission.DeleteTerm = true;
        }

        public DTO_GlossaryDisplayOrder()
        {
        }

        public Int64 Id { get; set; }
        public Int64 IdGlossary { get; set; }
        public Int32 IdCommunity { get; set; }
        public Int32 DisplayOrder { get; set; }
        public DTO_Glossary Glossary { get; set; }
    }
}