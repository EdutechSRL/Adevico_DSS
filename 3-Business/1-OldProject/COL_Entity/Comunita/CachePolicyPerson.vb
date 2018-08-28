Public Class CachePolicyPerson

#Region "Const"
	Private Const _Subscription As String = "Subscription{0}{1}{2}"
	Private Const _SubscriptionByService As String = "SubscriptionByService{0}{1}{2}"
	Private Const _Organization As String = "SubscriptionOrganization{0}{1}{2}"
	Private Const _CommunityType As String = "SubscriptionCommunityType{0}{1}{2}{3}{4}"
	Private Const _DegreeType As String = "SubscriptionDegreeType{0}{1}{2}{3}{4}"
	Private Const _Degree As String = "SubscriptionDegree{0}"
	Private Const _AccademicYear As String = "SubscriptionAccademicYear{0}{1}{2}{3}"
	Private Const _Periodo As String = "SubscriptionPeriodo{0}{1}{2}{3}{4}{5}"
	Private Const _Status As String = "SubscriptionStatus{0}{1}"
	Private Const _ServicesAvailable As String = "ServicesAvailable{0}"
#End Region

#Region "Subscription"
	Public Shared Function ServicesAvailable() As String
		Return String.Format(_ServicesAvailable, "", "")
	End Function
	Public Shared Function ServicesAvailable(ByVal PersonId As Integer) As String
		Return String.Format(_ServicesAvailable, "_" & PersonId, "")
	End Function
	Public Shared Function ServicesAvailable(ByVal PersonId As Integer, ByVal ServiceCode As String) As String
		Return String.Format(_ServicesAvailable, "_" & PersonId, "_" & ServiceCode)
	End Function
	Public Shared Function Subscription() As String
		Return String.Format(_Subscription, "", "", "")
	End Function
	Public Shared Function Subscription(ByVal PersonId As Integer) As String
		Return String.Format(_Subscription, "_" & PersonId, "", "")
	End Function
	Public Shared Function Subscription(ByVal PersonId As Integer, ByVal isLazy As Boolean) As String
		Return String.Format(_Subscription, "_" & PersonId, "_" & isLazy.ToString, "")
	End Function
	Public Shared Function Subscription(ByVal PersonId As Integer, ByVal isLazy As Boolean, ByVal ServiceCode As String) As String
		Return String.Format(_Subscription, "_" & PersonId, "_" & isLazy.ToString, "_" & ServiceCode)
	End Function
	'Public Shared Function SubscriptionByService() As String
	'	Return String.Format(_SubscriptionByService, "", "", "")
	'End Function
	'Public Shared Function SubscriptionByService(ByVal PersonId As Integer) As String
	'	Return String.Format(_SubscriptionByService, "_" & PersonId, "", "")
	'End Function
	'Public Shared Function SubscriptionByService(ByVal PersonId As Integer, ByVal isLazy As Boolean) As String
	'	Return String.Format(_SubscriptionByService, "_" & PersonId, "_" & isLazy.ToString, "")
	'End Function
	'Public Shared Function SubscriptionByService(ByVal PersonId As Integer, ByVal isLazy As Boolean, ByVal ServiceCode As String) As String
	'	Return String.Format(_SubscriptionByService, "_" & PersonId, "_" & isLazy.ToString, "_" & ServiceCode)
	'End Function

	Public Shared Function Subscripted_Organization() As String
		Return String.Format(_Organization, "", "", "")
	End Function
	Public Shared Function Subscripted_Organization(ByVal PersonId As Integer) As String
		Return String.Format(_Organization, "_" & PersonId, "", "")
	End Function
	Public Shared Function Subscripted_Organization(ByVal PersonId As Integer, ByVal isLazy As Boolean) As String
		Return String.Format(_Organization, "_" & PersonId, "_" & isLazy.ToString, "")
	End Function
	Public Shared Function Subscripted_Organization(ByVal PersonId As Integer, ByVal isLazy As Boolean, ByVal ServiceCode As String) As String
		Return String.Format(_Organization, "_" & PersonId, "_" & isLazy.ToString, "_" & ServiceCode)
	End Function
	Public Shared Function Subscripted_CommunityType() As String
		Return String.Format(_CommunityType, "", "", "", "")
	End Function
	Public Shared Function Subscripted_CommunityType(ByVal PersonId As Integer) As String
		Return String.Format(_CommunityType, "_" & PersonId, "", "", "", "")
	End Function
	Public Shared Function Subscripted_CommunityType(ByVal PersonId As Integer, ByVal LanguageID As Integer) As String
		Return String.Format(_CommunityType, "_" & PersonId, "_" & LanguageID, "", "", "")
	End Function
	Public Shared Function Subscripted_CommunityType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer) As String
		Return String.Format(_CommunityType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "", "")
	End Function
	Public Shared Function Subscripted_CommunityType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus) As String
		Return String.Format(_CommunityType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "_" & oStatus, "")
	End Function
	Public Shared Function Subscripted_CommunityType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal ServiceCode As String) As String
		Return String.Format(_CommunityType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "_" & oStatus, "_" & ServiceCode)
	End Function


	Public Shared Function Subscripted_DegreeType() As String
		Return String.Format(_DegreeType, "", "", "", "", "")
	End Function
	Public Shared Function Subscripted_DegreeType(ByVal PersonId As Integer) As String
		Return String.Format(_DegreeType, "_" & PersonId, "", "", "", "")
	End Function
	Public Shared Function Subscripted_DegreeType(ByVal PersonId As Integer, ByVal LanguageID As Integer) As String
		Return String.Format(_DegreeType, "_" & PersonId, "_" & LanguageID, "", "", "")
	End Function
	Public Shared Function Subscripted_DegreeType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer) As String
		Return String.Format(_DegreeType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "", "")
	End Function
	Public Shared Function Subscripted_DegreeType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus) As String
		Return String.Format(_DegreeType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "_" & oStatus, "")
	End Function
	Public Shared Function Subscripted_DegreeType(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal ServiceCode As String) As String
		Return String.Format(_DegreeType, "_" & PersonId, "_" & LanguageID, "_" & OrganizationID, "_" & oStatus, "_" & ServiceCode)
	End Function
	Public Shared Function Subscripted_AccademicYear() As String
		Return String.Format(_AccademicYear, "", "", "", "")
	End Function
	Public Shared Function Subscripted_AccademicYear(ByVal PersonId As Integer) As String
		Return String.Format(_AccademicYear, "_" & PersonId, "", "", "")
	End Function
	Public Shared Function Subscripted_AccademicYear(ByVal PersonId As Integer, ByVal OrganizationID As Integer) As String
		Return String.Format(_AccademicYear, "_" & PersonId, "_" & OrganizationID, "", "")
	End Function
	Public Shared Function Subscripted_AccademicYear(ByVal PersonId As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus) As String
		Return String.Format(_AccademicYear, "_" & PersonId, "_" & OrganizationID, "_" & oStatus, "")
	End Function
	Public Shared Function Subscripted_AccademicYear(ByVal PersonId As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal ServiceCode As String) As String
		Return String.Format(_AccademicYear, "_" & PersonId, "_" & OrganizationID, "_" & oStatus, "_" & ServiceCode)
	End Function



	Public Shared Function Subscripted_Periodo() As String
		Return String.Format(_Periodo, "", "", "", "", "", "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer) As String
		Return String.Format(_Periodo, "_" & PersonId, "", "", "", "", "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer, ByVal LanguageID As Integer) As String
		Return String.Format(_Periodo, "_" & PersonId, "_" & LanguageID.ToString, "", "", "", "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer) As String
		Return String.Format(_Periodo, "_" & PersonId, "_" & LanguageID.ToString, "_" & OrganizationID.ToString, "", "", "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus) As String
		Return String.Format(_Periodo, "_" & PersonId, "_" & LanguageID.ToString, "_" & OrganizationID.ToString, "_" & oStatus, "", "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal Year As Integer) As String
		Return String.Format(_Periodo, "_" & PersonId, "_" & LanguageID.ToString, "_" & OrganizationID.ToString, "_" & oStatus, "_" & Year, "")
	End Function
	Public Shared Function Subscripted_Periodo(ByVal PersonId As Integer, ByVal LanguageID As Integer, ByVal OrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal Year As Integer, ByVal ServiceCode As String) As String
		Return String.Format(_Periodo, "_" & PersonId, "_" & LanguageID.ToString, "_" & OrganizationID.ToString, "_" & oStatus, "_" & Year, "_" & ServiceCode)
	End Function
	Public Shared Function Subscripted_Status() As String
		Return String.Format(_Status, "", "", "")
	End Function
	Public Shared Function Subscripted_Status(ByVal PersonId As Integer) As String
		Return String.Format(_Status, "_" & PersonId, "", "")
	End Function
	Public Shared Function Subscripted_Status(ByVal PersonId As Integer, ByVal OrganizationID As Integer) As String
		Return String.Format(_Status, "_" & PersonId, "_" & OrganizationID.ToString, "")
	End Function
	Public Shared Function Subscripted_Status(ByVal PersonId As Integer, ByVal OrganizationID As Integer, ByVal ServiceCode As String) As String
		Return String.Format(_Status, "_" & PersonId, "_" & OrganizationID.ToString, "_" & ServiceCode)
	End Function
#End Region
End Class