Namespace File
	Public Class CachePolicyRepository
		Private Const _LabelCommunity As String = "LabelCommunity{0}"
		Private Const _LabelUser As String = "LabelUser{0}"
		Private Const _FileUser As String = "FileUser{0}"
		Private Const _FileCommunity As String = "FileCommunity{0}"

#Region "Generici"
		Public Shared Function LabelCommunity() As String
			Return String.Format(_LabelCommunity, "")
		End Function
		Public Shared Function LabelCommunity(ByVal CommunityID As Int64) As String
			Return String.Format(_LabelCommunity, "_" & CommunityID.ToString)
		End Function
		Public Shared Function LabelUser() As String
			Return String.Format(_LabelUser, "")
		End Function
		Public Shared Function LabelUser(ByVal UserID As Int64) As String
			Return String.Format(_LabelUser, "_" & UserID.ToString)
		End Function
		Public Shared Function FileUser() As String
			Return String.Format(_FileUser, "")
		End Function
		Public Shared Function FileUser(ByVal UserID As Int64) As String
			Return String.Format(_FileUser, "_" & UserID.ToString)
		End Function
		Public Shared Function FileCommunity() As String
			Return String.Format(_FileCommunity, "")
		End Function
		Public Shared Function FileCommunity(ByVal CommunityID As Int64) As String
			Return String.Format(_FileCommunity, "_" & CommunityID.ToString)
		End Function

#End Region

	End Class
End Namespace