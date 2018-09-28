using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.GraphTheory
{
     [Serializable]
    public class DirectedGraphNode
    {
        public Int64 Id { get; set; }

        public IList<DirectedGraphEdge> Edges { get; set; }

        public IList<Int64> EdgesId { get; set; }

        public DirectedGraphNode()
        {
            Edges = new List<DirectedGraphEdge>();
            EdgesId = new List<Int64>();
        }

        public DirectedGraphNode(Int64 id)
            :this()
        {
            this.Id = id;
        }

        public DirectedGraphNode(Int64 id, IEnumerable<Int64> edges)
            : this(id)
        {
            foreach (var item in edges)
            {
                this.EdgesId.Add(item);
            }
        }

        public DirectedGraphNode(Int64 id, params Int64[] edges)
            : this(id)
        {
            foreach (var item in edges)
            {
                this.EdgesId.Add(item);
            }
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
