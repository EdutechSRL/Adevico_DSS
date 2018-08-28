Public Class CachePolicyEvent

#Region "Const"
	Private Const _Events As String = "Events{0}{1}"
    Private Const _Esse3Events As String = "Esse3Events{0}{1}"
    Private Const _Libreria As String = "Libreria{0}{1}"
#End Region

#Region "Subscription"
	Public Shared Function Esse3Events() As String
		Return String.Format(_Esse3Events, "", "")
	End Function
	Public Shared Function Esse3Events(ByVal CommunityID As Integer) As String
		Return String.Format(_Esse3Events, "_" & CommunityID.ToString, "")
	End Function
	Public Shared Function Esse3Events(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As String
		Return String.Format(_Esse3Events, "_" & CommunityID.ToString, "_" & LanguageID.ToString)
	End Function
	
	Public Shared Function CommunityEvents() As String
		Return String.Format(_Events, "", "")
    End Function

	Public Shared Function CommunityEvents(ByVal CommunityID As Integer) As String
		Return String.Format(_Events, "_" & CommunityID.ToString, "")
    End Function

	Public Shared Function CommunityEvents(ByVal CommunityID As Integer, ByVal LanguageID As Integer) As String
		Return String.Format(_Events, "_" & CommunityID.ToString, "_" & LanguageID.ToString)
    End Function

    Public Shared Function Libreria() As String
        Return String.Format(_Libreria, "", "")
    End Function

    Public Shared Function Libreria(ByVal idLibreria As Integer) As String
        Return String.Format(_Events, "_" & idLibreria.ToString, "")
    End Function

    Public Shared Function Libreria(ByVal idLibreria As Integer, ByVal LanguageID As Integer) As String
        Return String.Format(_Libreria, "_" & idLibreria.ToString, "_" & LanguageID.ToString)
    End Function
#End Region
End Class