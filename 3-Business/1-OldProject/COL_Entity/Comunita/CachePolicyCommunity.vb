Public Class CachePolicyCommunity

#Region "Const"
	Private Const _Organization As String = "Organization{0}"
	Private Const _CommunityType As String = "CommunityType{0}"
	Private Const _DegreeType As String = "DegreeType{0}"
	Private Const _Degree As String = "Degree{0}"
	Private Const _AccademicYear As String = "AccademicYear"
	Private Const _Periodo As String = "Periodo{0}"
	Private Const _All As String = "CommunityAll{0}"
#End Region

#Region "Subscription"
	Public Shared Function CommunityAll() As String
		Return String.Format(_All, "")
	End Function
	Public Shared Function Organization() As String
		Return String.Format(_Organization, "")
	End Function
	Public Shared Function Organization(ByVal LanguageID As Integer) As String
		Return String.Format(_Organization, "_" & LanguageID)
	End Function
	Public Shared Function CommunityType() As String
		Return String.Format(_CommunityType, "")
	End Function
	Public Shared Function CommunityType(ByVal LanguageID As Integer) As String
		Return String.Format(_CommunityType, "_" & LanguageID)
	End Function
	Public Shared Function DegreeType() As String
		Return String.Format(_DegreeType, "")
	End Function
	Public Shared Function DegreeType(ByVal LanguageID As Integer) As String
		Return String.Format(_DegreeType, "_" & LanguageID)
	End Function
	Public Shared Function AccademicYear() As String
		Return String.Format(_AccademicYear, "")
	End Function
	Public Shared Function Periodo() As String
		Return String.Format(_Periodo, "")
	End Function
	Public Shared Function Periodo(ByVal LanguageID As Integer) As String
		Return String.Format(_Periodo, "_" & LanguageID)
	End Function
#End Region
End Class