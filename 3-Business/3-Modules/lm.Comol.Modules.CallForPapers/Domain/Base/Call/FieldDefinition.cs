using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public class FieldDefinition : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual String Name {get;set;}
        public virtual String Description {get;set;}
        public virtual int MaxLength {get;set;}
        public virtual FieldType Type {get;set;}
        public virtual FieldsSection Section {get;set;}
        public virtual int DisplayOrder {get;set;}
        public virtual Boolean Mandatory {get;set;}
        public virtual BaseForPaper Call { get; set; }
        public virtual String ToolTip { get; set; }
        public virtual String RegularExpression { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MinOption { get; set; }
        public virtual IList<FieldOption> Options { get; set; }
        public virtual DisclaimerType DisclaimerType { get; set; }

        public virtual String TableCols { get; set; }
        public virtual Int32 TableMaxRows { get; set; }

        public virtual double TableMaxTotal { get; set; }

        public virtual String Tags { get; set; }

        public FieldDefinition()
        {
            DisplayOrder = 1;
            Name = "";
            Description = "";
            Mandatory = true;
            MaxLength = -1;
            Deleted = BaseStatusDeleted.None;
            Options = new List<FieldOption>();
            TableCols = "";
            TableMaxRows = 50;
            TableMaxTotal = 0;
        }
        public FieldDefinition(String name,String description, int maxLength, FieldType type, int displayOrder, Boolean mandatory,FieldsSection section)
        {
            Name = name;
            Description=description;
            MaxLength=maxLength;
            Type=type;
            DisplayOrder = displayOrder;
            Mandatory=mandatory;
            Section=section;
            Call = section.Call;
            Deleted = BaseStatusDeleted.None;
            Options = new List<FieldOption>();
        }
    }
}