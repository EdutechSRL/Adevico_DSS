using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class MessageHelper
    {
        public static string SetCssClass(String currentClass, MessageType type)
        {
            if (!currentClass.Contains(type.GetStringValue())){
                if (!String.IsNullOrEmpty(currentClass) && (Enum.GetValues(typeof(MessageType)).OfType<MessageType>().Where(t=>t != MessageType.none).Select(t=> t.GetStringValue()).ToList().Where(c=> currentClass.Contains(c)).Any()))
                {
                    foreach (MessageType t in Enum.GetValues(typeof(MessageType)).OfType<MessageType>().Where(t => t != MessageType.none && currentClass.Contains(t.GetStringValue())).ToList())
                    {
                        if (currentClass.Contains(t.GetStringValue()))
                            currentClass = currentClass.Replace(t.GetStringValue(), "");
                    }
                }
                currentClass += " " + type.GetStringValue();
                currentClass = currentClass.Replace("  ", " ");
            }
            return currentClass;
        }
    }
    public enum MessageType
    {
        [StringValue("")]
        none = 0,
        //[Description("16x16")]
        [StringValue("alert")]
        alert = 1,
        [StringValue("info")]
        info = 2,
        [StringValue("ok")]
        success = 3,
        [StringValue("error")]
        error = 4,
        [StringValue("norecords")]
        norecords = 5,
    }


    [Serializable]
    public class dtoMessage
    {
        public MessageType Type { get; set; }
        public String Text { get; set; }
        public String ConfirmButton { get; set; }
        public String ConfirmButtonValue { get; set; }
        public String CancelButton { get; set; }
        public String CancelButtonValue { get; set; }
    }
}