Imports System
Imports System.Collections.Generic 
Imports System.Text 
Imports System.Web 
Imports System.Configuration.Provider 
Imports System.Collections.Specialized 

	Public Class HistoryProvider
		Inherits StaticSiteMapProvider
		Private mRootNode As SiteMapNode = Nothing


		Public Sub New()
		End Sub

		' create the root node 
		Public Overloads Overrides Sub Initialize(ByVal name As String, ByVal attributes As NameValueCollection)
			MyBase.Initialize(name, attributes)
		'mRootNode = New SiteMapNode(Me, "Home", "Default.aspx", "Home")
		'AddNode(mRootNode)
		End Sub

	Public Sub AddRootNode(ByVal ChiaveRicerca As String, ByVal url As String, ByVal testo As String)
		mRootNode = New SiteMapNode(Me, ChiaveRicerca, url, testo)
		AddNode(mRootNode)
	End Sub

		Public Overloads Overrides Function BuildSiteMap() As SiteMapNode
			Return mRootNode
		End Function

		Public Overloads Overrides ReadOnly Property RootNode() As SiteMapNode
			Get
				Return mRootNode
			End Get
		End Property

		Protected Overloads Overrides Function GetRootNodeCore() As SiteMapNode
			Return RootNode
		End Function

		Public Overloads Overrides Function FindSiteMapNode(ByVal rawUrl As String) As SiteMapNode
			Return MyBase.FindSiteMapNode(rawUrl)
		End Function

		' stack a node under the root 
		Public Function Stack(ByVal title As String, ByVal uri As String) As SiteMapNode
			Return Stack(title, uri, mRootNode)
		End Function

		' stack a node under any other node 
		Public Function Stack(ByVal title As String, ByVal uri As String, ByVal parentnode As SiteMapNode) As SiteMapNode
			SyncLock Me
				Dim node As SiteMapNode = MyBase.FindSiteMapNodeFromKey(uri)

				If node Is Nothing Then
					node = New SiteMapNode(Me, uri, uri, title)
					node.ParentNode = (IIf((parentnode Is Nothing), mRootNode, parentnode))
					AddNode(node)
				ElseIf node.Title <> title Then
					node.Title = title
				End If

				Return node
			End SyncLock
		End Function

		Public Sub Stack(ByVal nodes As List(Of KeyValuePair(Of String, Uri)))
			Dim parent As SiteMapNode = RootNode
			For Each node As KeyValuePair(Of String, Uri) In nodes
				parent = Stack(node.Key, node.Value.PathAndQuery, parent)
			Next
		End Sub
End Class