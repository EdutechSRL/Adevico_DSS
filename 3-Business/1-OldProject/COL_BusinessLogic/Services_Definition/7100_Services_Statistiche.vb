Namespace UCServices
	Public Class Services_Statistiche
		Inherits UCServices.Abstract.MyServices

		Private Const Codice As String = "SRVSTAT"

		Public Overloads Shared ReadOnly Property Codex() As String
			Get
				Codex = Codice
			End Get
		End Property

#Region "Public Property"
		Public Property List() As Boolean
			Get
				List = MyBase.GetPermissionValue(PermissionType.Read)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					MyBase.SetPermissionByPosition(PermissionType.Read, 1)
				Else
					MyBase.SetPermissionByPosition(PermissionType.Read, 0)
				End If
			End Set
		End Property
		Public Property Management() As Boolean
			Get
				Management = MyBase.GetPermissionValue(PermissionType.Moderate)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					MyBase.SetPermissionByPosition(PermissionType.Moderate, 1)
				Else
					MyBase.SetPermissionByPosition(PermissionType.Moderate, 0)
				End If
			End Set
		End Property
		Public Property GrantPermission() As Boolean
			Get
				GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					MyBase.SetPermissionByPosition(PermissionType.Grant, 1)
				Else
					MyBase.SetPermissionByPosition(PermissionType.Grant, 0)
				End If
			End Set
		End Property
		Public Property Admin() As Boolean
			Get
				Admin = MyBase.GetPermissionValue(PermissionType.Admin)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					MyBase.SetPermissionByPosition(PermissionType.Admin, 1)
				Else
					MyBase.SetPermissionByPosition(PermissionType.Admin, 0)
				End If
			End Set
		End Property
		Public Property Export() As Boolean
			Get
				Export = MyBase.GetPermissionValue(PermissionType.Receive)
			End Get
			Set(ByVal Value As Boolean)
				If Value Then
					MyBase.SetPermissionByPosition(PermissionType.Receive, 1)
				Else
					MyBase.SetPermissionByPosition(PermissionType.Receive, 0)
				End If
			End Set
		End Property
#End Region

		Sub New()
			MyBase.New()
		End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub

		Public Shared Function Create() As Services_Statistiche
			Return New Services_Statistiche("00000000000000000000000000000000")
		End Function

		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			ListGlobalPortal = 71003
			ListGlobalCommunity = 71004
			ViewGenericDetails = 71005
			ViewModuleDetails = 71006
		End Enum
		Public Enum ObjectType
			None = 0
		End Enum
	End Class
End Namespace