using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.GraphTheory
{
     [Serializable]
    public class DirectedGraphEdge
    {
        public DirectedGraphNode A { get; set; }
        public DirectedGraphNode B { get; set; }

        public DirectedGraphEdge()
        {
        }

        public DirectedGraphEdge(DirectedGraphNode a)
        {
            this.A = a;
            this.A.Edges.Add(this);
        }

        public DirectedGraphEdge(DirectedGraphNode a, DirectedGraphNode b)
            : this(a)
        {
            this.B = b;
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", this.A, this.B);
        }
    }
}
