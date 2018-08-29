Namespace UCServices
	Namespace Abstract
		<Serializable(), CLSCompliant(True)> Public MustInherit Class BaseService
			Private _Permission As List(Of Integer)
			Private Const Codice As String = "SRVABSTRACT"

			Public Property PermissionList() As String
				Get
					Dim Permissions As String = ""
					For Each o In _Permission
						Permissions &= o.ToString
					Next
					Return Permissions
				End Get
				Set(ByVal Value As String)
					For Each Permission In Value.ToList
						Try
							Me._Permission.Add(Val(Permission))
						Catch ex As Exception
							Me._Permission.Add(0)
						End Try
					Next
				End Set
			End Property
			Public Enum PermissionType
				None = -1
				Read = 0
				Write = 1
				Change = 2
				Delete = 3
				Moderate = 4
				Grant = 5
				Admin = 6
				Send = 7
				Receive = 8
				Synchronize = 9
				Browse = 10
				Print = 11
				ChangeOwner = 12
				Add = 13
				ChangeStatus = 14
				DownLoad = 15
			End Enum
			Sub New()
				_Permission = New List(Of Integer)
				For i As Integer = 0 To 31 : _Permission.Add(0)

				Next
			End Sub
			Protected Overridable Function GetPermissionByPosition(ByVal Index As Integer) As Boolean
				If Index >= Me._Permission.Count Then
					Return False
				Else
					Return (_Permission(Index) = 1)
				End If
			End Function
			Protected Overridable Function GetPermissionValue(ByVal oType As PermissionType) As Boolean
				Dim iPosizione As Integer
				iPosizione = CType(oType, PermissionType)
				Return GetPermissionByPosition(iPosizione)
			End Function
			Protected Overridable Function SetPermissionByPosition(ByVal Index As Integer, ByVal oValue As Boolean) As Boolean
				If Index > Me._Permission.Count - 1 Then
					Return False
				Else
					_Permission(Index) = IIf(oValue, 1, 0)
				End If
				Return False
			End Function
		End Class
	End Namespace
End Namespace