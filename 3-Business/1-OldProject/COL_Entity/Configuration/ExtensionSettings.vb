Imports Comol.Entity.File
Imports Comol.Entity.Configuration.Components


Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class ExtensionSettings

#Region "Private property"
		Private _DefaultIcon As String
		Private _ExtensionToShow As String
		Private _Icons As List(Of IconElement)
		Private _MimeTypes As List(Of MimeType)
#End Region

#Region "Public property"
		Public ReadOnly Property ExtensionToShow() As String
			Get
				Return _ExtensionToShow
			End Get
		End Property

		Public ReadOnly Property DefaultIcon() As String
			Get
				Return _DefaultIcon
			End Get
		End Property
		Public ReadOnly Property Icons() As List(Of IconElement)
			Get
				Return _Icons
			End Get
		End Property
		Public ReadOnly Property MimeTypes() As List(Of MimeType)
			Get
				Return _MimeTypes
			End Get
		End Property
#End Region

		Public Sub New()

		End Sub
		Sub New(ByVal iDefaultIcon As String, ByVal iExtensionToShow As String)
			Me._DefaultIcon = iDefaultIcon
			Me._ExtensionToShow = iExtensionToShow
			_MimeTypes = New List(Of MimeType)
			_Icons = New List(Of IconElement)
		End Sub
		Sub New(ByVal iDefaultIcon As String, ByVal iExtensionToShow As String, ByVal iMimeTypes As List(Of MimeType), ByVal iIconElements As List(Of IconElement))
			Me._DefaultIcon = iDefaultIcon
			Me._ExtensionToShow = iExtensionToShow
			_MimeTypes = iMimeTypes
			_Icons = iIconElements
		End Sub


		Public Function FindIconImage(ByVal Extension As String) As String
			Dim oIcon As IconElement = (From o As IconElement In _Icons Where o.Extension = Extension).FirstOrDefault
			If IsNothing(oIcon) Then
				Return _DefaultIcon
			Else
				Return oIcon.Icon
			End If
		End Function

		Public Function FindMimeType(ByVal Extension As String) As String
			Dim oMimeType As MimeType = (From o As MimeType In MimeTypes Where o.Extension = Extension).FirstOrDefault
			If IsNothing(oMimeType) Then
				Return ""
			Else
				Return oMimeType.Type
			End If
		End Function
	End Class
End Namespace