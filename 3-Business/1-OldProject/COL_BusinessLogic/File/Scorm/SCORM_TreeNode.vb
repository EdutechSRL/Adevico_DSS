Namespace Comol.Materiale.Scorm
	Public Class SCORM_TreeNode
		Inherits GenericTreeNode

		Private _NodeType As SCORM_TreeNodeType
		Private _ChildNodes As List(Of SCORM_TreeNode)

		Public ReadOnly Property NodeType() As SCORM_TreeNodeType
			Get
				Return _NodeType
			End Get
		End Property
		Public Overloads Property ChildNodes() As List(Of SCORM_TreeNode)
			Get
				Return _ChildNodes
			End Get
			Set(ByVal value As List(Of SCORM_TreeNode))
				_ChildNodes = value
			End Set
		End Property

		Sub New()
			MyBase.New()
			Me._ChildNodes = New List(Of SCORM_TreeNode)
			Me._NodeType = SCORM_TreeNodeType.Unknow
		End Sub
		Sub New(ByVal oType As SCORM_TreeNodeType)
			MyBase.New()
			Me._ChildNodes = New List(Of SCORM_TreeNode)
			Me._NodeType = oType
		End Sub
		Sub New(ByVal oType As SCORM_TreeNodeType, ByVal iText As String, ByVal iValue As String, Optional ByVal isSistemNode As Boolean = False)
			MyBase.New(iText, iValue, isSistemNode)
			Me._ChildNodes = New List(Of SCORM_TreeNode)
			Me._NodeType = oType
		End Sub
	End Class
End Namespace