using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class dtoSubmissionValueField : dtoBase 
    {
        public virtual long IdRevision { get; set; }
        public virtual long IdField {
            get{
                return (Field == null) ? 0 : Field.Id;  
            }
        }
        public virtual Boolean Mandatory
        {
            get
            {
                return (Field == null) ? false : Field.Mandatory;
            }
        }
        public virtual long IdValueField { get; set; }
        public virtual dtoCallField Field { get; set; }
        public virtual long IdOption { get; set; }
        public virtual dtoValueField Value { get; set; }
        public virtual FieldError FieldError { get; set; }
        public virtual long RevisionsCount { get; set; }
        public virtual Boolean AllowSelection { get; set; }
        public virtual Boolean IsValid { get; set; }
        
        public dtoSubmissionValueField()
            : base()
        {
            FieldError = Domain.FieldError.None;
            Value = new dtoValueField("");
        }
        public dtoSubmissionValueField(FieldDefinition definition)
            : base()
        {
            Field = new dtoCallField(definition);
            FieldError = Domain.FieldError.None;
            Value = new dtoValueField("");
        }

        public dtoSubmissionValueField(FieldDefinition definition, SubmissionFieldBaseValue valueItem, long revNumber)
            : this(definition)
        {
            if (valueItem != null)
            {
                if (valueItem is SubmissionFieldStringValue)
                    Value = new dtoValueField(((SubmissionFieldStringValue)valueItem).Value,((SubmissionFieldStringValue)valueItem).UserValue);
                else if (valueItem is SubmissionFieldFileValue)
                {
                    SubmissionFieldFileValue fileValue = (SubmissionFieldFileValue)valueItem;
                    Value = new dtoValueField(fileValue.Link);
                }
                IdValueField = valueItem.Id;
            }
            RevisionsCount = revNumber;
            //else if (Mandatory)
            //    FieldError = FieldError.Mandatory;
        }
        public void SetError(Dictionary<long, FieldError> errors)
        {
            if (errors == null)
                FieldError = Domain.FieldError.None;
            else if (errors.ContainsKey(IdField))
                FieldError = errors[IdField];
            else
                FieldError = Domain.FieldError.None;
        }
        public void SetValue(SubmissionFieldBaseValue valueItem)
        {
            if (valueItem != null)
            {
                if (valueItem is SubmissionFieldStringValue)
                    Value = new dtoValueField(((SubmissionFieldStringValue)valueItem).Value, ((SubmissionFieldStringValue)valueItem).UserValue);
                else if (valueItem is SubmissionFieldFileValue)
                {
                    SubmissionFieldFileValue fileValue = (SubmissionFieldFileValue)valueItem;
                    Value = new dtoValueField(fileValue.Link);
                }
                IdValueField = valueItem.Id;
            }
        }

        public dtoValueField GetValue()
        {
            switch(Field.Type){
                case  FieldType.Note:
                    return new dtoValueField("");
                case FieldType.DropDownList:
                case FieldType.RadioButtonList:
                case FieldType.CheckboxList:
                case FieldType.Disclaimer:
                    String rValue = "";
                    Boolean hasUserValue = false;
                    if (Value != null && !String.IsNullOrEmpty(Value.Text))
                    {
                        List<String> mValue = Value.Text.Split('|').ToList();
                        hasUserValue = Field.Options.Where(o => o.IsFreeValue && mValue.Contains(o.Id.ToString())).Any();
                        foreach (dtoFieldOption option in Field.Options.Where(o => mValue.Contains(o.Id.ToString())).ToList())
                        {
                            rValue += ", " + option.Name;
                        }
                        if (!String.IsNullOrEmpty(rValue))
                            rValue = rValue.Remove(0, 2);
                    }
                    return new dtoValueField(rValue, (Value != null && hasUserValue) ? Value.FreeText : "");
                case FieldType.FileInput:
                    return Value;
                default :
                    return new dtoValueField("");
            }
        }
    }

    [Serializable]
    public class dtoValueField
    {
        public virtual String Text { get; set; }
        public virtual String FreeText { get; set; }
        public virtual long IdLink { get; set; }
        public virtual liteModuleLink Link { get; set; }

        public dtoValueField(String value, String userValue="") {
            FreeText = userValue;
            Text = value;
            IdLink = 0;
            Link = null;
        }
        public dtoValueField(liteModuleLink link) {
            Link = link;
            IdLink = (link == null) ? 0 : link.Id;
        }
    }
}