Namespace Comol.Materiale.Scorm
	Public Class SCORM_Package
		Inherits ObjectBase

#Region "Private Property"
		Private _MetaData As SCORM_MetaData
		Private _Organizations As List(Of SCORM_Organization)
		Private _Resources As List(Of SCORM_Resource)
		Private _Sequencing As List(Of SCORM_Sequencing)
		Private _UrlBasePackage As String
		Private _PackagePath As String
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property MetaData() As SCORM_MetaData
			Get
				Return _MetaData
			End Get
			Set(ByVal value As SCORM_MetaData)
				_MetaData = value
			End Set
		End Property
		Public Property Resources() As List(Of SCORM_Resource)
			Get
				Return _Resources
			End Get
			Set(ByVal value As List(Of SCORM_Resource))
				_Resources = value
			End Set
		End Property
		Public Property Organizations() As List(Of SCORM_Organization)
			Get
				Return _Organizations
			End Get
			Set(ByVal value As List(Of SCORM_Organization))
				_Organizations = value
			End Set
		End Property
		Public Property Sequencing() As List(Of SCORM_Sequencing)
			Get
				Return _Sequencing
			End Get
			Set(ByVal value As List(Of SCORM_Sequencing))
				_Sequencing = value
			End Set
		End Property
		Public ReadOnly Property UrlBase() As String
			Get
				Return _UrlBasePackage
			End Get
		End Property
		Public ReadOnly Property PackagePath() As String
			Get
				Return _PackagePath
			End Get
		End Property
		Public Property IsValid() As Boolean
			Get
				Return _isValid
			End Get
			Set(ByVal value As Boolean)
				_isValid = value
			End Set
		End Property
#End Region

		Public Sub New(Optional ByVal isValid As Boolean = True)
			_Organizations = New List(Of SCORM_Organization)
			_Resources = New List(Of SCORM_Resource)
			_Sequencing = New List(Of SCORM_Sequencing)
			Me._isValid = isValid
		End Sub
		Public Sub New(ByVal PackagePath As String, Optional ByVal isValid As Boolean = True)
			_PackagePath = PackagePath
			_UrlBasePackage = "" ' PackagePath
			_Organizations = New List(Of SCORM_Organization)
			_Resources = New List(Of SCORM_Resource)
			_Sequencing = New List(Of SCORM_Sequencing)
			Me._isValid = isValid
		End Sub

		Public Sub ChangeUrlBase(ByVal url As String)
			If url <> "" Then
				url = Replace(url, "/", "\")
				If Not (url.EndsWith("\")) Then
					url &= "\"
				End If
			End If
			Me._UrlBasePackage = url
		End Sub

		Public Function Validate() As Boolean
			If Me._isValid = False Then
				Return False
			Else
				Return Me.ValidateOrganizations AndAlso Me.ValidateResources
			End If
		End Function


		Public Sub RebuildResourceDependency()
			Me._Resources.Sort(New GenericComparer(Of SCORM_Resource)("DependencyList.Count"))

			For Each oResource As SCORM_Resource In Me._Resources
				If oResource.DependencyList.Count > 0 Then
					For Each oIdentifier As String In oResource.DependencyList
						Dim oDependency As SCORM_Resource = Me._Resources.Find(New GenericPredicate(Of SCORM_Resource, String)(oIdentifier, AddressOf SCORM_Resource.FindResourceDependency))
						If Not IsNothing(oDependency) Then
							oResource.AddDependency(oDependency)
						End If
					Next
				End If
			Next
		End Sub

		Private Function ValidateOrganizations() As Boolean
			Return IsNothing(Me._Organizations.Find(New GenericPredicate(Of SCORM_Organization, Boolean)(False, AddressOf SCORM_Organization.FindByValidita)))
		End Function
		Private Function ValidateResources() As Boolean
			Return IsNothing(Me._Resources.Find(New GenericPredicate(Of SCORM_Resource, Boolean)(False, AddressOf SCORM_Resource.FindByValidita)))
		End Function

		Public Function GetWebTreeNode(ByVal LinguaCode As String) As SCORM_TreeNode

			If Me.IsValid Then
				Dim oNode As New SCORM_TreeNode(SCORM_TreeNodeType.Package, "", 0, True)
				oNode.Expanded = True
				If Not IsNothing(Me._MetaData) Then
					Dim oMetaDataNode As SCORM_TreeNode
					oMetaDataNode = LoadMetaDataTree(Me._MetaData, LinguaCode)
					If Not IsNothing(oMetaDataNode) Then
						oNode.ChildNodes.Add(oMetaDataNode)
					End If
					oNode.Text = Me._MetaData.General.Identifier.Catalog
				End If
				If Me._Organizations.Count = 1 Then
					Dim oItemNodes As List(Of SCORM_TreeNode)
					oItemNodes = LoadItems(Me._Organizations(0).Items)
					If Not IsNothing(oItemNodes) Then
						oNode.ChildNodes.AddRange(oItemNodes)
					End If
					If IsNothing(Me._MetaData) Or oNode.Text = "" Then
						oNode.Text = Me._Organizations(0).Title
					End If

				ElseIf Me._Organizations.Count > 1 Then
					Dim oOrganizationNodes As List(Of SCORM_TreeNode)
					oOrganizationNodes = LoadOrganization(Me._Organizations)
					If Not IsNothing(oOrganizationNodes) Then
						oNode.ChildNodes.AddRange(oOrganizationNodes)
					End If
				End If
				Return oNode
			Else
				Return New SCORM_TreeNode(SCORM_TreeNodeType.InvalidPackage, "", -1, True)
			End If
			Return Nothing
		End Function
		Public Function FindResource(ByVal Identifier As String) As SCORM_Resource
			Dim oResource As SCORM_Resource
			oResource = Me._Resources.Find(New GenericPredicate(Of SCORM_Resource, String)(Identifier, AddressOf SCORM_Resource.FindResourceDependency))
			Return oResource
		End Function

		Private Function LoadMetaDataTree(ByVal oMetadata As SCORM_MetaData, ByVal LinguaCode As String) As SCORM_TreeNode
			Dim oMetaDataNode As New SCORM_TreeNode(SCORM_TreeNodeType.InfoManifest, "", 0, False)

			'		oMetadata.General.Title()

			If oMetaDataNode.ChildNodes.Count = 0 Then
				Return Nothing
			Else
				Return oMetaDataNode
			End If
		End Function
		Private Function LoadOrganization(ByVal oOrganizations As List(Of SCORM_Organization)) As List(Of SCORM_TreeNode)
			Dim oList As New List(Of SCORM_TreeNode)

			For Each oItem As SCORM_Organization In oOrganizations
				If oItem.isVisible Then
					Dim oNode As New SCORM_TreeNode(SCORM_TreeNodeType.Organization, oItem.Title, oItem.ID, True)
					If oItem.Items.Count > 0 Then
						oNode.ChildNodes.AddRange(Me.LoadItems(oItem.Items))
					End If
					oList.Add(oNode)
				End If
			Next
			If oList.Count = 0 Then
				Return Nothing
			Else
				Return oList
			End If
		End Function
		Private Function LoadItems(ByVal oItems As List(Of SCORM_Item)) As List(Of SCORM_TreeNode)
			Dim oList As New List(Of SCORM_TreeNode)

			For Each oItem As SCORM_Item In oItems
				If oItem.isVisible Then
					Dim oNode As New SCORM_TreeNode(SCORM_TreeNodeType.Item, oItem.Title, oItem.IDresource, False)
					If oItem.Items.Count > 0 Then
						oNode.ChildNodes.AddRange(Me.LoadItems(oItem.Items))
					End If
					oList.Add(oNode)
				End If
			Next
			If oList.Count = 0 Then
				Return Nothing
			Else
				Return oList
			End If
		End Function
	End Class
End Namespace