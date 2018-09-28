using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.GraphTheory
{
    [Serializable]
    public class DirectedGraphCycle:IEquatable<DirectedGraphCycle>
    {
        public IList<DirectedGraphEdge> Members { get; set; }
        public Boolean IsComplete { get; set; }

        public String CycleId
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in this.Members)
                {
                    sb.Append(item.A.Id.ToString() + "->" + item.B.Id.ToString());
                    if (item != this.Members.Last())
                    {
                        sb.Append(";");
                    }
                }

                return sb.ToString();
            }
        }

        public Boolean isAutoCycle
        {
            get
            {
                return (Members.Count == 1) && (Members[0].A.Id == Members[0].B.Id);
            }
        }

        internal void Build(DirectedGraphEdge member)
        {
            if (!IsComplete)
            {
                if (member == Members[0])
                    IsComplete = true;
                else
                    this.Members.Add(member);
            }
        }

        public DirectedGraphCycle()
        {
            this.Members = new List<DirectedGraphEdge>();
        }

        public DirectedGraphCycle(DirectedGraphEdge firstMember)
            :this()
        {            
            this.Members.Add(firstMember);            
        }
        public List<long> GetIdentifyers() {
            return Members.Select(m => m.A.Id).Union(Members.Select(m => m.B.Id)).Distinct().ToList();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in this.Members)
            {
                sb.Append(item.ToString());
                if (item != this.Members.Last())
                {
                    sb.Append("; ");
                }
            }

            return sb.ToString();
        }


        public bool Equals(DirectedGraphCycle other)
        {
            return this.CycleId.Equals(other.CycleId);
        }
    }
}
