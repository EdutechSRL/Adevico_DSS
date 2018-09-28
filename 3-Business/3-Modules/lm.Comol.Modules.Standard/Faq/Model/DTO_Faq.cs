using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Faq
{
    [Serializable]
    public class DTO_Faq
    {
        public Int64 ID { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }
        public List<DTO_Category> Categories { get; set; }
        public int? Order { get; set; }

        public DTO_Faq()
        {
            ID = 0;
            Question = "";
            Answer = "";
            Categories = new List<DTO_Category>();
            Order = null;
        }
    }
}
