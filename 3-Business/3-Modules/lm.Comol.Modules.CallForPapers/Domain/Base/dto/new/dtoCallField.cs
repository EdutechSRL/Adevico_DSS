using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoCallField :dtoBase 
    {
        public virtual long IdSection { get; set; }
        public virtual String Name { get; set; }
        //public virtual String Text { get; set; }
        public virtual String Description { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int MaxLength { get; set; }
        public virtual FieldType Type { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual List<long> Submitters { get; set; }
        public virtual List<dtoFieldOption> Options { get; set; }
        public virtual Boolean isMultiple { get { return (Options != null); } }
        public virtual String ToolTip { get; set; }
        public virtual String RegularExpression { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MinOption { get; set; }
        public virtual DisclaimerType DisclaimerType { get; set; }
        public virtual dtoCallTableField TableFieldSetting { get; set; }

        public virtual string Tags { get; set; }

        public virtual Boolean HasFreeOption {get { return isMultiple && Options.Where(o=> o.Deleted == BaseStatusDeleted.None && o.IsFreeValue ).Any();}}
        public dtoCallField()
            : base()
        {
        }

        public dtoCallField(long id, String name, int display, string tags)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
            Tags = tags;
        }
        public dtoCallField(FieldDefinition definition)
            : base()
        {
            CreateDto(definition);
            Submitters = new List<long>();
        }

        public dtoCallField(FieldDefinition definition, List<long> idSubmitters)
            : base()
        {
            CreateDto(definition);
            Submitters = idSubmitters;
        }

        private void CreateDto(FieldDefinition definition)
        {
            Id = definition.Id;
            if (definition.Section!=null)
                IdSection = definition.Section.Id;
            Deleted = definition.Deleted;
            DisplayOrder = definition.DisplayOrder;
            Name = definition.Name;
            Description = definition.Description;
            MaxLength = definition.MaxLength;
            Type = definition.Type;
            Mandatory = definition.Mandatory;
            ToolTip = definition.ToolTip;
            RegularExpression = definition.RegularExpression;
            Options = new List<dtoFieldOption>();
            Tags = definition.Tags;

            switch (definition.Type) { 
                case FieldType.DropDownList:
                case FieldType.CheckboxList:
                case FieldType.RadioButtonList:
                    Options = definition.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).OrderBy(o=>o.DisplayOrder).ThenBy(o=>o.Name).Select(o => new dtoFieldOption(o)).ToList();
                    MaxOption = definition.MaxOption;
                    MinOption = definition.MinOption;
                    break;
                case FieldType.Disclaimer:
                    Options = definition.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).OrderBy(o=>o.DisplayOrder).ThenBy(o=>o.Name).Select(o => new dtoFieldOption(o)).ToList();
                    MaxOption = definition.MaxOption;
                    MinOption = definition.MinOption;
                    DisclaimerType = definition.DisclaimerType;
                    break;
                case FieldType.TableReport:
                case FieldType.TableSimple:
                    TableFieldSetting = new dtoCallTableField();
                    TableFieldSetting.MaxRows = definition.TableMaxRows;
                    TableFieldSetting.Cols = definition.TableCols;
                    TableFieldSetting.MaxTotal = definition.TableMaxTotal;
                    break;
            }
        }
    }

    [Serializable]
    public class dtoCallTableField
    {
        public string Cols { get; set; }
        //public int MaxCols { get; set; }
        //public int Rows { get; set; }
        public int MaxRows { get; set; }

        public double MaxTotal { get; set; }

        public dtoCallTableField()
        {
            Cols = "";
            MaxRows = 50;
            MaxTotal = 0;
        }
        
    }

}