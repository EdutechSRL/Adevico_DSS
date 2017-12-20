
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class StatTreeNode<T> : StatBaseTreeNode
	{
		public IList<StatTreeNode<T>> Nodes {get;set;}
		public IList<T> Leaves {get;set;}
		public StatTreeNode()
		{
			Nodes = new List<StatTreeNode<T>>();
			Leaves = new List<T>();
		}

		public List<T> GetLeavesByType(StatTreeLeafType type)
		{
			List<T> oList = (from l in Leaves where ((l as iStatTreeLeaf).Type & type) >= 0 select l).ToList();

			foreach (StatTreeNode<T> node in Nodes) {
				oList.AddRange(node.GetLeavesByType(type));
			}
			if ((oList == null)) {
				oList = new List<T>();
			}
			return oList;
		}

    
        //Public Function GetLeavesByType(ByVal type As StatTreeLeafType) As List(Of T)
        //    Dim oList As List(Of T) = (From l In Leaves Where (TryCast(l, iStatTreeLeaf).Type And type) >= 0).ToList

        //    For Each node As StatTreeNode(Of T) In Nodes
        //        oList.AddRange(node.GetLeavesByType(type))
        //    Next
        //    If IsNothing(oList) Then
        //        oList = New List(Of T)
        //    End If
        //    Return oList
        //End Function
	}
}