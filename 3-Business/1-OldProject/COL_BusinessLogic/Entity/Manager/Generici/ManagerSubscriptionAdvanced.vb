Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerSubscriptionAdvanced
		Inherits ObjectBase
		Implements iManagerAdvanced

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentDB As ConnectionDB
		Private _CurrentUser As Person
		Private _CurrentCommunity As Community
		Private _Language As Lingua
#End Region

#Region "Public property"
		Public ReadOnly Property CurrentCommunity() As Community Implements iManagerAdvanced.CurrentCommunity
			Get
				Return _CurrentCommunity
			End Get
		End Property
		Public ReadOnly Property CurrentUser() As Person Implements iManagerAdvanced.CurrentUser
			Get
				Return _CurrentUser
			End Get
		End Property
		Private ReadOnly Property UseCache() As Boolean Implements iManagerAdvanced.UseCache
			Get
				Return _UseCache
			End Get
		End Property
		Private ReadOnly Property CurrentDB() As ConnectionDB Implements iManagerAdvanced.CurrentDB
			Get
				If IsNothing(_CurrentDB) Then
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.Esse3, ConnectionType.SQL)
				End If
				Return _CurrentDB
			End Get
		End Property
		Private ReadOnly Property Language() As Lingua Implements iManagerAdvanced.Language
			Get
				Return _Language
			End Get
		End Property
#End Region

		Public Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		Public Sub New(ByVal oPerson As Person, ByVal oCommunity As Community, ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True)
			Me._UseCache = False ' UseCache
			Me._CurrentUser = oPerson
			Me._CurrentCommunity = oCommunity
			Me._Language = oLanguage
		End Sub

		'		Public Function GetFilterOrganization(ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal isLazy As Boolean = True, Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As New List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_Organization(Me._CurrentUser.ID, isLazy, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = GetOrganizationsFromDal(Me._CurrentUser, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = GetOrganizationsFromDal(Me._CurrentUser, oClause)
		'			End If
		'			Return oList
		'		End Function
		'		Public Function GetFilterStatus(ByVal oOrganization As Organization, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As New List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_Status(Me._CurrentUser.ID, oOrganization.OrganizationID, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = GetCommunityStatusFromDal(Me._CurrentUser, oOrganization.OrganizationID, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = GetCommunityStatusFromDal(Me._CurrentUser, oOrganization.OrganizationID, oClause)
		'			End If
		'			Return oList
		'		End Function
		'		Public Function GetFilterCommunityType(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As New List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_CommunityType(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = Me.GetCommunitiesTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = Me.GetCommunitiesTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
		'			End If
		'			Return oList
		'		End Function
		'		Public Function GetFilterDegreeType(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_DegreeType(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = Me.GetDegreeTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = Me.GetDegreeTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
		'			End If
		'			Return oList
		'		End Function
		'		Public Function GetFilterPeriod(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oAccademicYear As AcademicYear, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As New List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_Periodo(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = Me.GetPeriodoFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = Me.GetPeriodoFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, oClause)
		'			End If
		'			Return oList
		'		End Function
		'		Public Function GetFilterAccademicYear(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
		'			Dim oList As New List(Of FilterElement)

		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscripted_AccademicYear(Me._CurrentUser.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = Me.GetAccademicYearFromDal(Me._CurrentUser, oOrganization.OrganizationID, oStatus, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
		'				End If
		'			Else
		'				oList = Me.GetAccademicYearFromDal(Me._CurrentUser, oOrganization.OrganizationID, oStatus, oClause)
		'			End If
		'			Return oList
		'		End Function


		'		Private Function CompositeServiceCode(ByVal oClause As GenericClause(Of ServiceClause)) As String
		'			Dim iCode As String = ""

		'			If IsNothing(oClause) Then
		'				Return iCode
		'			ElseIf IsNothing(oClause.NextClause) Then
		'				Return oClause.Clause.Service.Code
		'			Else
		'				iCode = oClause.Clause.Service.Code
		'				Select Case oClause.OperatorForNextClause
		'					Case OperatorType.AndCondition
		'						iCode &= "_AND_" & CompositeServiceCode(oClause.NextClause)
		'					Case OperatorType.OrCondition
		'						iCode &= "_OR_" & CompositeServiceCode(oClause.NextClause)
		'					Case OperatorType.XorCondition
		'						iCode &= "_XOR_" & CompositeServiceCode(oClause.NextClause)
		'				End Select
		'			End If
		'			Return iCode
		'		End Function
		'		Private Function HasPermission(ByVal oServiceClause As ServiceClause, ByVal PermissionToValidate As String) As String
		'			Dim intValid, intValidate As Int64

		'			intValid = Convert.ToInt64(New String(oServiceClause.Service.PermissionString.Reverse.ToArray), 2)
		'			intValidate = Convert.ToInt64(New String(PermissionToValidate.Reverse.ToArray), 2)

		'			Select Case oServiceClause.PermissionOperator
		'				Case OperatorType.AndCondition
		'					Return intValid And intValidate
		'				Case OperatorType.OrCondition
		'					Return intValid Or intValidate
		'				Case OperatorType.XorCondition
		'					Return intValid Xor intValidate
		'				Case Else
		'					Return False
		'			End Select
		'			Return False
		'		End Function

		'		Public Function FilterSubscriptions(ByVal isLazy As Boolean, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of Subscription)
		'			Dim oList As New List(Of Subscription)
		'			If Me._UseCache Then
		'				Dim cacheKey As String = CachePolicyPerson.Subscription(Me._CurrentUser.ID, isLazy, "CompositeServiceCode-" & CompositeServiceCode(oClause))
		'				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
		'					oList = GetSubscriptionsFromDal(Me._CurrentUser, isLazy, oClause)
		'					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
		'				Else
		'					oList = CType(ObjectBase.Cache(cacheKey), List(Of Subscription))
		'				End If
		'			Else
		'				oList = GetSubscriptionsFromDal(Me._CurrentUser, isLazy, oClause)
		'			End If

		'			Return oList
		'		End Function


		'#Region "Common function to Retrieve from DAL"
		'		Private Function GetOrganizationsFromDal(ByVal oPerson As Person, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)

		'			Return oDALsubscriptions.FiltersOrganization(oPerson, oClause)
		'		End Function
		'		Private Function GetCommunityStatusFromDal(ByVal oPerson As Person, ByVal oOrganizationID As Integer, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)
		'			Dim oList As List(Of FilterElement) = oDALsubscriptions.FiltersCommunityStatus(oPerson, oOrganizationID, oClause)

		'			For Each oElement As FilterElement In oList
		'				Select Case oElement.Value
		'					Case CommunityStatus.Archiviata
		'						oElement.Text = My.Resources.Resource.CommunityStatus_Archiviata
		'					Case CommunityStatus.ClosedByAdministration
		'						oElement.Text = My.Resources.Resource.CommunityStatus_ClosedByAdministration
		'					Case CommunityStatus.OnlyActivated
		'						oElement.Text = My.Resources.Resource.CommunityStatus_OnlyActivated
		'				End Select
		'			Next
		'			Return oList
		'		End Function
		'		Private Function GetCommunitiesTypeFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)
		'			Return oDALsubscriptions.FiltersCommunityType(oPerson, oClause, LanguageID, oOrganizationID, oStatus)
		'		End Function
		'		Private Function GetDegreeTypeFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)

		'			Return oDALsubscriptions.FiltersDegreeType(oPerson, LanguageID, oOrganizationID, oStatus, oClause)
		'		End Function
		'		Private Function GetPeriodoFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal AccademicYearID As Integer, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)

		'			Return oDALsubscriptions.FiltersPeriodo(oPerson, LanguageID, oOrganizationID, oStatus, AccademicYearID, oClause)
		'		End Function
		'		Private Function GetAccademicYearFromDal(ByVal oPerson As Person, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)

		'			Return oDALsubscriptions.FiltersAccademicYear(oPerson, oOrganizationID, oStatus, oClause)
		'		End Function
		'		Private Function GetSubscriptionsFromDal(ByVal oPerson As Person, ByVal isLazy As Boolean, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of Subscription)
		'			Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptionAdvanced(GetCurrentDB)
		'			If isLazy Then
		'				Return oDALsubscriptions.ListForFilters(oPerson, Me._Language.ID, oClause)
		'			Else
		'				Return New List(Of Subscription)
		'			End If
		'		End Function
		'#End Region

	End Class
End Namespace