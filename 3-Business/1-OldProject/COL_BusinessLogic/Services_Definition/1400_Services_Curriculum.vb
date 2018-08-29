Namespace UCServices
	Public Class Services_Curriculum
		Inherits Abstract.MyServices

		Const Codice = "SRVCRCLA"
		Public Overloads Shared ReadOnly Property Codex() As String
			Get
				Codex = Codice
			End Get
		End Property

#Region "Public Property"
		Public Property ListOther() As Boolean
			Get
				ListOther = MyBase.GetPermissionValue(PermissionType.Browse)
			End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Browse, IIf(Value, 1, 0))
			End Set
		End Property
		Public Property Read() As Boolean
			Get
				Read = MyBase.GetPermissionValue(PermissionType.Read)
			End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
			End Set
		End Property
		Public Property Admin() As Boolean
			Get
				Admin = MyBase.GetPermissionValue(PermissionType.Admin)
			End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
			End Set
		End Property
		Public Property GrantPermission() As Boolean
			Get
				GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
			End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
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

		Public Shared Function Create() As Services_Curriculum
			Return New Services_Curriculum("00000000000000000000000000000000")
		End Function

		Public Enum ActionType
			None = 0
			NoPermission = 1
			GenericError = 2
			CreateCurriculum = 14003
			EditCurriculum = 14004
			CreateEducation = 14005
			EditEducation = 14006
			CreateLanguage = 14007
			EditLanguage = 14008
			CreateWorkingExperience = 14009
			EditWorkingExperience = 14010
			SaveCurriculum = 14011
			ShowCurriculum = 14012
			'Show = 3
			'Create = 4
			'Edit = 5
			'VirtualDelete = 7
			'Undelete = 8
			'Delete = 9
		End Enum
		Public Enum ObjectType
			None = 0
			Curriculum = 1
			Education = 2
			Language = 3
			WorkingExperience = 4
			Competenza = 5
		End Enum
	End Class
End Namespace