using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class AlphabetItem
    {
        public String DisplayName { get; set; }
        public String Value { get; set; }
        public Boolean isEnabled { get; set; }     
        public Boolean isSelected { get; set; }
        public AlphabetItemType Type { get; set; }
        public AlphabetItemDisplayAs DisplayAs {get;set;}

        public AlphabetItem() {
            DisplayAs = AlphabetItemDisplayAs.item;
            isSelected = false;
            Type = AlphabetItemType.letter;
        }
        [Serializable,Flags]
        public enum AlphabetItemDisplayAs 
        {
           first = 1,
           item = 2,
           last = 3
        }
       
    }
    [Serializable]
    public enum AlphabetItemType
    {
        letter = 1,
        number = 2,
        all = 3,
        otherChars = 4,
    }
}