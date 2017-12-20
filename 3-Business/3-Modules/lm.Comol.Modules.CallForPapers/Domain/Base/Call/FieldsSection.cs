using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class FieldsSection : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual int DisplayOrder {get;set;}
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual BaseForPaper Call { get; set; }
        public virtual IList<FieldDefinition> Fields { get; set; }

        public FieldsSection()
        {
            DisplayOrder = 1;
            Name = "";
            Description = "";
            Fields = new List<FieldDefinition>();
            Deleted = BaseStatusDeleted.None;
        }
        public FieldsSection(String name, BaseForPaper call)
        {
            DisplayOrder = 1;
            Name = name;
            Fields = new List<FieldDefinition>();
            Call = call;
            Deleted = BaseStatusDeleted.None;
        }
        public FieldsSection(String name, String description, BaseForPaper call)
        {
            DisplayOrder = 1;
            Name = name;
            Description = description;
            Fields = new List<FieldDefinition>();
            Call = call;
            Deleted = BaseStatusDeleted.None;
        }
        public FieldsSection(String name, BaseForPaper call, int displayOrder)
        {
            DisplayOrder = displayOrder;
            Name = name;
            Fields = new List<FieldDefinition>();
            Call = call;
            Deleted = BaseStatusDeleted.None;
        }
        public FieldsSection(String name, String description, BaseForPaper call, int displayOrder)
        {
            DisplayOrder = displayOrder;
            Name = name;
            Description = description; 
            Fields = new List<FieldDefinition>();
            Call = call;
            Deleted = BaseStatusDeleted.None;
        }
    }
}
