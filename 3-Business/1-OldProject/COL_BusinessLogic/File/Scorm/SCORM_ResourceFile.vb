Namespace Comol.Materiale.Scorm
	Public Class SCORM_ResourceFile

#Region "Private Property"
		Private _Name As String
		Private _Extension As String
		Private _FileName As String
		Private _Url As String
		Private _CompleteUrl As String
#End Region

#Region "Public Property"
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
			End Set
		End Property
		Public Property Url() As String
			Get
				Return _Url
			End Get
			Set(ByVal value As String)
				_Url = value
				_Extension = FindExtension()
				_FileName = FindFileName()
				If _FileName <> "" Then
					_FileName = Replace(_FileName, "." & _Extension, "")
				End If
			End Set
		End Property
		Public ReadOnly Property Extension() As String
			Get
				Return _Extension
			End Get
		End Property
		Public ReadOnly Property FileNameAndExtension() As String
			Get
				Return _FileName & "." & _Extension
			End Get
		End Property
		Public ReadOnly Property FileName() As String
			Get
				Return _FileName
			End Get
		End Property
#End Region

		Public Sub New(ByVal fileName As String, ByVal fileUrl As String)
			_Name = fileName
			_Url = fileUrl
			_Extension = FindExtension()
			_FileName = FindFileName()
			If _FileName <> "" Then
				_FileName = Replace(_FileName, "." & _Extension, "")
			End If
		End Sub

		Private Function FindExtension() As String
			If _Url <> "" AndAlso InStr(Me._Url, ".") > 0 Then
				Return Right(_Url, _Url.Length - InStrRev(Me._Url, "."))
			Else
				Return ""
			End If
		End Function
		Private Function FindFileName() As String
			If InStr(Me._Url, "/") > 0 Then
				Dim oSplit() As String
				oSplit = Me._Url.Split("/")
				Return oSplit(oSplit.GetUpperBound(0))
			Else
				Return _Url
			End If
		End Function
	End Class
End Namespace