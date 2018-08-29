Namespace Comol.Materiale.Scorm

	Public Class SCORM_Resource

#Region "Private Property"
		Private _ID As String	'Identificatore univoco risorsa
		Private _Type As String		'Type della risorsa
		Private _Href As String		'Path
		Private _ScormType As String	'SCO or Asset?
		Private _ResourceFiles As List(Of SCORM_ResourceFile)
		Private _ResourceDependency As List(Of SCORM_Resource)
		Private _Dependency As List(Of String)
		Private _isValid As Boolean
#End Region

#Region "Public Property"
		Public Property ID() As String
			Get
				ID = _ID
			End Get
			Set(ByVal value As String)
				_ID = value
			End Set
		End Property
		Public Property Href() As String
			Get
				Href = _Href
			End Get
			Set(ByVal value As String)
				_Href = value
			End Set
		End Property
		Public Property Type() As String
			Get
				Type = _Type
			End Get
			Set(ByVal value As String)
				_Type = value
			End Set
		End Property
		Public Property ScormType() As String
			Get
				ScormType = _ScormType
			End Get
			Set(ByVal value As String)
				_ScormType = value
			End Set
		End Property
		Public Property ResourceFiles() As List(Of SCORM_ResourceFile)
			Get
				Return _ResourceFiles
			End Get
			Set(ByVal value As List(Of SCORM_ResourceFile))
				_ResourceFiles = value
			End Set
		End Property
		Public ReadOnly Property ResourceDependency() As List(Of SCORM_Resource)
			Get
				Return _ResourceDependency
			End Get
		End Property
		Public ReadOnly Property DependencyList() As List(Of String)
			Get
				Return _Dependency
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

		Sub New()
			_isValid = True
			_ResourceFiles = New List(Of SCORM_ResourceFile)
			_Dependency = New List(Of String)
			_ResourceDependency = New List(Of SCORM_Resource)
		End Sub
		Public Sub New(ByVal ID As String, ByVal oType As String, ByVal oHref As String, ByVal oScormType As String)
			_ID = ID
			_Type = oType
			_Href = oHref
			_ScormType = oScormType
			_ResourceFiles = New List(Of SCORM_ResourceFile)
			_Dependency = New List(Of String)
			_ResourceDependency = New List(Of SCORM_Resource)
			Me._isValid = True
		End Sub
		Public Function Validate() As Boolean
			Return Me._isValid = False
		End Function
		Public Shared Function FindByValidita(ByVal item As SCORM_Resource, ByVal argument As Boolean) As Boolean
			Return item._isValid = argument
		End Function
		Public Sub AddDependency(ByVal oResource As SCORM_Resource)
			If Not IsNothing(oResource) Then
				If IsNothing(Me._Dependency) Then
					Me._Dependency.Add(oResource.ID)
				ElseIf IsNothing(Me._Dependency.Find(New GenericPredicate(Of String, String)(oResource.ID, AddressOf SCORM_Resource.FindDependencyIdentifier))) Then
					Me._Dependency.Add(oResource.ID)
				End If
				If IsNothing(Me._ResourceDependency) Then
					Me._ResourceDependency.Add(oResource)
				ElseIf IsNothing(Me._ResourceDependency.Find(New GenericPredicate(Of SCORM_Resource, String)(oResource.ID, AddressOf SCORM_Resource.FindResourceDependency))) Then
					Me._ResourceDependency.Add(oResource)
				End If
			End If
		End Sub

		Public Function GetAllFiles() As List(Of SCORM_ResourceFile)
			Dim oLista As New List(Of SCORM_ResourceFile)
			oLista = Me._ResourceFiles

			For Each oResource As SCORM_Resource In Me._ResourceDependency
				Dim oListaFile As List(Of SCORM_ResourceFile) = oResource.GetAllFiles()
				If Not IsNothing(oListaFile) Then
					oLista.AddRange(oListaFile)
				End If
			Next
			Return oLista
		End Function
		Public Shared Function FindDependencyIdentifier(ByVal item As String, ByVal argument As String) As Boolean
			Return item = argument
		End Function
		Public Shared Function FindResourceDependency(ByVal item As SCORM_Resource, ByVal argument As String) As Boolean
			Return item.ID = argument
		End Function
		Public Shared Function FindDependencyCostraints(ByVal item As SCORM_Resource, ByVal argument As Boolean) As Boolean
			Return (item.DependencyList.Count <> item.ResourceDependency.Count) = argument
		End Function
	End Class
End Namespace