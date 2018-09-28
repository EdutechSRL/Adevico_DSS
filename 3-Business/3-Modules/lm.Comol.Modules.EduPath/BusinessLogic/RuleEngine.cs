using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class RuleEngine<Telement> where Telement : IRuleElement
    {
        public IList<RuleBase<Telement>> Rules { get; set; }        

        public RuleEngine()
        {
            Rules = new List<RuleBase<Telement>>();
        }

        public void AddRule(RuleBase<Telement> rule)
        {
            Rules.Add(rule); 
        }

        public void AddRules(params RuleBase<Telement>[] rules)
        {
            //foreach (var rule in rules)
            //{ 
            //    Rules.Add(rule);
            //}

            Rules = Rules.Concat(rules).ToList<RuleBase<Telement>>();
        }

        public void AddRulesRange(IList<RuleBase<Telement>> rules)
        {
            Rules = Rules.Concat(rules).ToList<RuleBase<Telement>>();
        }

        public RuleEngineResult<Telement> ExecuteStepTopDown(Telement item)
        {
            RuleEngineResult<Telement> result = new RuleEngineResult<Telement>();            
            IList<RuleBase<Telement>> appliableRules = (from rule in Rules where rule.Source!=null && rule.Source.Id.Equals(item.Id) select rule).ToList<RuleBase<Telement>>();

            result.isValid = (appliableRules.Count == 0);

            foreach (var rule in appliableRules)
            {
                Boolean res = false;

                if (rule.Source.OverrideRules)
                {
                    res = true;
                }
                else
                {
                    res = rule.Execute();
                }

                result.isValid = result.isValid || res;

                if (!res)
                {
                    result.ViolatedRules.Add(rule);
                }
                else
                {
                    result.ValidRules.Add(rule);
                }
            }
              

            return result;
        }

        IList<Telement> visited { get; set; }

        public IList<KeyValuePair<Telement, RuleEngineResult<Telement>>> ExecuteFromTop(Telement item, IList<KeyValuePair<Telement, RuleEngineResult<Telement>>> previous = null)
        {

            IList<KeyValuePair<Telement, RuleEngineResult<Telement>>> result = previous;

            if (result == null)
            {
                result = new List<KeyValuePair<Telement, RuleEngineResult<Telement>>>();
                visited = new List<Telement>();
            }

            if (!visited.Contains(item))
            {
                visited.Add(item);
                RuleEngineResult<Telement> res = ExecuteStepTopDown(item);

                result.Add(new KeyValuePair<Telement, RuleEngineResult<Telement>>(item, res));

                foreach (var rule in res.ValidRules)
                {
                    ExecuteFromTop(rule.Destination, result);
                }

            } 
            return result;
        }

        public RuleEngineResult<Telement> ExecuteStep(Telement item)
        {
            RuleEngineResult<Telement> result = new RuleEngineResult<Telement>();

            IList<RuleBase<Telement>> appliableRules = (from rule in Rules where rule.Destination.Id.Equals(item.Id) select rule).ToList<RuleBase<Telement>>();

            result.isValid = (appliableRules.Count == 0);

            foreach (var rule in appliableRules)
            {
                Boolean res = false;

                if (rule.Source.OverrideRules)
                {
                    res = true;
                }
                else
                {
                    res = rule.Execute();
                }

                result.isValid = result.isValid || res;

                if (!res)
                {
                    result.ViolatedRules.Add(rule);
                }
                else
                {
                    result.ValidRules.Add(rule);
                }
            }

            return result;
        }

        public RuleEngineResult<Telement> ExecuteStep(Telement prev, Telement next)
        {
            RuleEngineResult<Telement> result = new RuleEngineResult<Telement>();

            IList<RuleBase<Telement>> appliableRules = (from rule in Rules where rule.Source.Id.Equals(prev.Id) && rule.Destination.Id.Equals(next.Id) select rule).ToList<RuleBase<Telement>>();

            result.isValid = (appliableRules.Count == 0);

            foreach (var rule in appliableRules)
            {
                Boolean res = false;

                if (rule.Source.OverrideRules)
                {
                    res = true;
                }
                else
                {
                    res = rule.Execute();
                }

                result.isValid = result.isValid || res;

                if (!res)
                {
                    result.ViolatedRules.Add(rule);
                }
                else
                {
                    result.ValidRules.Add(rule);
                }
            }

            return result;
        }

        public IList<KeyValuePair<Telement, RuleEngineResult<Telement>>> ExecuteFromTopByRoots(IEnumerable<Telement> roots)
        {
            IList<KeyValuePair<Telement, RuleEngineResult<Telement>>> result = null;

            foreach (var item in roots)
            {
                result = ExecuteFromTop(item, result);
            }

            return result;
        }
    }
}
