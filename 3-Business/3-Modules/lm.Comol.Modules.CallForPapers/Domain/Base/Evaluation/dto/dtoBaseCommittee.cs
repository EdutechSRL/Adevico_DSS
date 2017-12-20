using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
     [Serializable]
    public class dtoBaseCommittee:dtoBase 
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean ForAllSubmittersType { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual Boolean UseDss { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings MethodSettings { get; set; }
        public virtual lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings WeightSettings { get; set; }

        public virtual bool IsMaster { get; set; }

        public dtoBaseCommittee() {
            MethodSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings();
            WeightSettings = new lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightSettings();
        }

        public lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase ToWeightItem(Dictionary<long, String> weights, Boolean isFuzzy, Boolean isOrderedItem = false)
        {
            lm.Comol.Core.Dss.Domain.Templates.dtoItemWeightBase weight = new Core.Dss.Domain.Templates.dtoItemWeightBase();
            weight.IdObject = Id;
            weight.IsFuzzyValue = isFuzzy;
            weight.Name = Name;
            weight.OrderedItem = isOrderedItem;
            weight.Value = "";
            if (weights.ContainsKey(Id))
                weight.Value = weights[Id];
            return weight;
        }
    }
}