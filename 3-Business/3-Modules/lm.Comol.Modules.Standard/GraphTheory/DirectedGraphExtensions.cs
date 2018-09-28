using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.GraphTheory
{
    public static class DirectedGraphExtensions
    {
        public static void SetReference(this IEnumerable<DirectedGraphNode> nodes)
        {
            foreach (var node in nodes)
            {
                DirectedGraphNode a = node;
                foreach (var id in a.EdgesId)
                {
                    DirectedGraphNode b = nodes.Where(x => x.Id == id).FirstOrDefault();
                    DirectedGraphEdge edge = new DirectedGraphEdge(a, b);
                }
            }
        }

        public static void ClearReference(this IEnumerable<DirectedGraphNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Edges = new List<DirectedGraphEdge>();
            }
        }

        public static IList<DirectedGraphNode> ToNodeList<T>(this IEnumerable<T> items, Func<T, Int64> id, Func<T, IEnumerable<Int64>> edges)
        {
            IList<DirectedGraphNode> nodes = new List<DirectedGraphNode>();

            nodes.AddNodes<T>(items, id, edges);

            return nodes;
        }

        public static void AddNode<T>(this IList<DirectedGraphNode> nodes, T item, Func<T, Int64> id, Func<T, IEnumerable<Int64>> edges)
        {
            nodes.Add(new DirectedGraphNode(id(item), edges(item)));
        }

        public static void AddNodes<T>(this IList<DirectedGraphNode> nodes, IEnumerable<T> elements, Func<T, Int64> id, Func<T, IEnumerable<Int64>> edges)
        {
            foreach (T item in elements)
            {
                nodes.Add(new DirectedGraphNode(id(item), edges(item)));
            }
        }

        public static void AddNewNode(this IList<DirectedGraphNode> nodes, Int64 id, IEnumerable<Int64> edges)
        {
            nodes.Add(new DirectedGraphNode(id, edges));
        }

        public static void AddNewNode(this IList<DirectedGraphNode> nodes, Int64 id, params Int64[] edges)
        {
            nodes.Add(new DirectedGraphNode(id, edges));
        }

        public static Boolean isDisconnectedGraph(this IEnumerable<DirectedGraphNode> nodes)
        {

            return nodes.DisconnectedIds().Count() > 0;
        }

        public static IList<DirectedGraphNode> DisconnectedNodes(this IEnumerable<DirectedGraphNode> nodes)
        {
            

            var ids = nodes.Select(x => x.Id).ToList();
            var edges = nodes.SelectMany(x => x.EdgesId).Distinct().ToList();

            var disconnected = ids.Where(x => !edges.Contains(x)).ToList();

            return nodes.Where(x => disconnected.Contains(x.Id) && x != nodes.First()).ToList();
        }

        public static IList<Int64> DisconnectedIds(this IEnumerable<DirectedGraphNode> nodes)
        {

            var ids = nodes.Select(x => x.Id).ToList();
            var edges = nodes.SelectMany(x => x.EdgesId).Distinct().ToList();

            return ids.Where(x => !edges.Contains(x) && x!=nodes.First().Id).ToList();
        }

        public static IList<Int64> RootsId(this IEnumerable<DirectedGraphNode> nodes)
        {

            var ids = nodes.Select(x => x.Id).ToList();
            var edges = nodes.SelectMany(x => x.EdgesId).Distinct().ToList();

            return ids.Where(x => !edges.Contains(x)).ToList();
        }

        public static IList<DirectedGraphNode> Roots(this IEnumerable<DirectedGraphNode> nodes)
        {

            var rootsid = nodes.RootsId();


            return nodes.Where(x => rootsid.Contains(x.Id)).ToList();
        }

        public static IList<DirectedGraphNode> SubTreeRoots(this IEnumerable<DirectedGraphNode> nodes)
        {

            var roots = nodes.Roots();


            return roots.Where(x => x.EdgesId.Count > 0).ToList();
        }

        public static IEnumerable<DirectedGraphCycle> FindAllCyclesForestGraph(this IEnumerable<DirectedGraphNode> nodes)
        {
            nodes.ClearReference();
            nodes.SetReference();
            var roots = nodes.SubTreeRoots();

            HashSet<DirectedGraphNode> alreadyVisited = new HashSet<DirectedGraphNode>();

            IEnumerable<DirectedGraphCycle> all=new List<DirectedGraphCycle>();

            foreach (var item in roots)
            {
                alreadyVisited.Add(item);

                var currs = FindAllCycles(alreadyVisited, item).ToList();

                ReduceCycles(currs);


                all = all.Concat(currs);
            }

            return all;

            //return all;
        }

        private static void ReduceCycles(IList<DirectedGraphCycle> currs)
        {
            foreach (var curr in currs)
            {
                while (curr.Members.Count()>0 && curr.Members.First().A != curr.Members.Last().B)
                {
                    curr.Members.Remove(curr.Members.Last());
                }                
            }

            currs = currs.Distinct().ToList();
        }

        public static IEnumerable<DirectedGraphNode> FindAutoCycleNodesDisconnectedGraph(this IEnumerable<DirectedGraphNode> nodes)
        {
            nodes.ClearReference();
            nodes.SetReference();

            //HashSet<DirectedGraphNode> alreadyVisited = new HashSet<DirectedGraphNode>();

            

            return nodes.Where(x=>x.EdgesId.Contains(x.Id));
        }

        public static IEnumerable<DirectedGraphCycle> FindAllCyclesDisconnectedGraph(this IEnumerable<DirectedGraphNode> nodes)
        {
            nodes.ClearReference();
            //nodes.SetReference();
            HashSet<DirectedGraphNode> alreadyVisited = new HashSet<DirectedGraphNode>();

            IEnumerable<DirectedGraphCycle> all;


            IList<DirectedGraphNode> mynodes = nodes.ToList();


            if (nodes.isDisconnectedGraph())
            {

                //foreach (var item in mynodes)
                //{
                //    item.EdgesId.Add(-1);
                //}

                //mynodes.AddNewNode(-1);

                mynodes.SetReference();

                //alreadyVisited.Add(mynodes.FirstOrDefault());
                //return FindAllCycles(alreadyVisited, mynodes.FirstOrDefault());

                alreadyVisited.Add(mynodes.FirstOrDefault());
                

                var currs1  = FindAllCycles(alreadyVisited, mynodes.FirstOrDefault()).ToList();

                ReduceCycles(currs1);

                all = currs1;


                foreach (var item in mynodes.DisconnectedNodes())
                {
                    HashSet<DirectedGraphNode> forest = new HashSet<DirectedGraphNode>();
                    forest.Add(item);
                    alreadyVisited.Add(item);

                    var currs = FindAllCycles(forest, item).ToList();

                    ReduceCycles(currs);

                    all = all.Concat(currs);

                }

                //all = all.Where(x => x.Members.Count > 1 || x.isAutoCycle).ToList(); //attempt to avoid 1member cycle;                  

                return all;
            }
            else
            {
                mynodes.SetReference();

                // mynodes = mynodes.OrderBy(x => x.Id).ToList();




                alreadyVisited.Add(mynodes.FirstOrDefault());
                var currs = FindAllCycles(alreadyVisited, mynodes.FirstOrDefault()).ToList();

                ReduceCycles(currs);

                return currs;
            }

            

            //if (nodes.isDisconnectedGraph())
            //{
            //    mynodes.AddNewNode(-1, nodes.DisconnectedIds());

            //    //mynodes = mynodes.OrderBy(x => x.Id).ToList();

            //    //alreadyVisited.Add(mynodes.FirstOrDefault());
            //    //all = FindAllCycles(alreadyVisited, mynodes.FirstOrDefault()).ToList();


            //    //foreach (var item in nodes.DisconnectedNodes())
            //    //{
            //    //    alreadyVisited.Add(item);

            //    //    all = all.Concat(FindAllCycles(alreadyVisited, item)).ToList();
            //    //}

            //    //return all;
            //}

            
            

            //foreach (var item in connected)
            //{
            //    if (!alreadyVisited.Contains(item))
            //    {
            //        alreadyVisited.Add(item);
            //        all = all.Concat(FindAllCycles(alreadyVisited, item).ToList()).ToList();
            //    }
            //}


            //all = all.Where(x => x.Members.Count > 1 || x.isAutoCycle).ToList(); //attempt to avoid 1member cycle;            


            //return all;
        }

        public static IEnumerable<DirectedGraphCycle> FindAllCyclesConnectedGraph(this IEnumerable<DirectedGraphNode> nodes)
        {
            nodes.ClearReference();
            nodes.SetReference();
            HashSet<DirectedGraphNode> alreadyVisited = new HashSet<DirectedGraphNode>();
            //IList<DirectedGraphCycle> all = new List<DirectedGraphCycle>();

            //IList<DirectedGraphNode> connected = nodes.Where(x => x.Edges.Count > 0).ToList();

            alreadyVisited.Add(nodes.FirstOrDefault());

            var x = FindAllCycles(alreadyVisited, nodes.FirstOrDefault()).ToList();

            foreach (var item in x)
            {

                while (item.Members.First().A != item.Members.Last().B)
                {
                    item.Members.Remove(item.Members.Last());
                }
            }

            return x;

            //foreach (var item in connected)
            //{
            //    if (!alreadyVisited.Contains(item))
            //    {
            //        alreadyVisited.Add(item);
            //        all = all.Concat(FindAllCycles(alreadyVisited, item).ToList()).ToList();
            //    }
            //}


            //all = all.Where(x => x.Members.Count > 1 || x.isAutoCycle ).ToList(); //attempt to avoid 1member cycle;            


            //return all;
        }

        private static IEnumerable<DirectedGraphCycle> FindAllCycles(HashSet<DirectedGraphNode> alreadyVisited, DirectedGraphNode a, Boolean issub=false)
        {
            for (int i = 0; i < a.Edges.Count; i++)
            {
                DirectedGraphEdge e = a.Edges[i];
                if (alreadyVisited.Contains(e.B))
                {
                    yield return new DirectedGraphCycle(e);
                }
                else
                {
                    HashSet<DirectedGraphNode> newSet = i == a.Edges.Count - 1 ? alreadyVisited : new HashSet<DirectedGraphNode>(alreadyVisited);
                    newSet.Add(e.B);
                    foreach (DirectedGraphCycle c in FindAllCycles(newSet, e.B))
                    {
                        c.Build(e);
                        yield return c;
                    }
                }
            }
        }

        private static IEnumerable<DirectedGraphCycle> FindAllCyclesUnoptimized(HashSet<DirectedGraphNode> alreadyVisited, DirectedGraphNode a)
        {
            foreach (DirectedGraphEdge e in a.Edges)
                if (alreadyVisited.Contains(e.B))
                    yield return new DirectedGraphCycle(e);
                else
                {
                    HashSet<DirectedGraphNode> newSet = new HashSet<DirectedGraphNode>(alreadyVisited);
                    newSet.Add(e.B);//EDIT: thnx dhsto
                    foreach (DirectedGraphCycle c in FindAllCyclesUnoptimized(newSet, e.B))
                    {
                        c.Build(e);
                        yield return c;
                    }
                }
        }
    }
}
