Imports Comol.Entity.Configuration

Namespace File
	<Serializable(), CLSCompliant(True)> Public Class ObjectFilePath

#Region "Private properties"
		Private _VirtualPath As String
		Private _DrivePath As String
		Private _SharePath As String
		Private _RewritePath As String
		Private _isSharePath As Boolean
		Private _isOnOtherIIS As Boolean
#End Region

#Region "Public properties"
		Public Property Virtual() As String
			Get
				Virtual = _VirtualPath
			End Get
			Set(ByVal value As String)
				_VirtualPath = value
			End Set
		End Property
		Public Property Drive() As String
			Get
				Drive = _DrivePath
			End Get
			Set(ByVal value As String)
				_DrivePath = value
			End Set
		End Property
		Public Property isOnShare() As Boolean
			Get
				isOnShare = _isSharePath
			End Get
			Set(ByVal value As Boolean)
				_isSharePath = value
			End Set
		End Property
		Public ReadOnly Property isOnIIS() As Boolean
			Get
				isOnIIS = CBool(_VirtualPath <> "")
			End Get
		End Property
		Public Property SharePath() As String
			Get
				SharePath = _SharePath
			End Get
			Set(ByVal value As String)
				_SharePath = value
			End Set
		End Property
		Public Property RewritePath() As String
			Get
				RewritePath = _RewritePath
			End Get
			Set(ByVal value As String)
				_RewritePath = value
			End Set
		End Property
#End Region

		Sub New(ByVal isOnSharePath As Boolean)
			_isSharePath = isOnSharePath
		End Sub


		Public Shared Function CreateByConfigPath(ByVal oPath As Components.ConfigurationPath, ByVal UrlPath As String, ByVal DrivePath As String) As ObjectFilePath
			Dim oObjectPath As New ObjectFilePath(oPath.isOnThisServer)

			Try
				If oPath.isOnThisServer Then
					If oPath.VirtualPath <> "" Then
						oObjectPath.Virtual = UrlPath & "/" & oPath.VirtualPath
						oObjectPath.Virtual = Replace(oObjectPath.Virtual, "//", "/")
					End If
					If oPath.DrivePath <> "" Then
						oObjectPath.Drive = oPath.DrivePath	'Me.BaseUrlDrivePath & oPath.DrivePath
					ElseIf oPath.VirtualPath <> "" Then
						oObjectPath.Drive = DrivePath & oPath.VirtualPath
					End If
					oObjectPath.Drive = Replace(oObjectPath.Drive, "/", "\")
					oObjectPath.Drive = Replace(oObjectPath.Drive, "\\", "\")
				Else
					If oPath.ServerVirtualPath <> "" Then
						oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath
					End If
					If oPath.DrivePath <> "" Then
						oObjectPath.Drive = oPath.ServerPath & oPath.DrivePath
					Else
						oObjectPath.Drive = oPath.ServerPath & oPath.VirtualPath
					End If
					oObjectPath.SharePath = oPath.ServerPath
				End If
				oObjectPath.RewritePath = oPath.RewritePath
			Catch ex As Exception

			End Try
			Return oObjectPath
		End Function
	End Class
End Namespace