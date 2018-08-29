Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerSubscription
		Inherits ObjectBase
		Implements iManagerAdvanced

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentDB As ConnectionDB
		Private _CurrentUser As Person
		Private _CurrentCommunity As Community
		Private _Language As Lingua
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

#Region "Public property"
        Public ReadOnly Property CurrentCommunity() As Community Implements iManagerAdvanced.CurrentCommunity
            Get
                Return _CurrentCommunity
            End Get
        End Property
        Public ReadOnly Property CurrentUser() As Person Implements iManagerAdvanced.CurrentUser
            Get
                If Not _CurrentUser Is Nothing Then
                    Return _CurrentUser
                Else
					''da eliminare quando si inserisce la pagina nella masterpage e ComOl
					'Dim oPersona As New Person(123456, "NomeMio", "CognomeMio")
					Return New Person(0, "", "")
                End If
			End Get
        End Property
        Public Property ElementsToHide(ByVal CurrentUserId As Integer)
            Get
                Dim cacheKey As String
                cacheKey = "ElementsToHideOf" & CurrentUserId.ToString
                If ObjectBase.Cache(cacheKey) Is Nothing Then
                    Return Nothing
                Else
                    Return CType(ObjectBase.Cache(cacheKey), List(Of Integer))
                End If
            End Get
            Set(ByVal value)
                If Not value Is Nothing Then
                    Dim cacheKey As String
                    cacheKey = "ElementsToHideOf" & CurrentUserId.ToString
                    ObjectBase.Cache.Insert(cacheKey, value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                End If
            End Set
        End Property

#End Region

        Public Shared Function GetCurrentDB() As ConnectionDB
            Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
        End Function
        Public Sub New(ByVal oPerson As Person, ByVal oCommunity As Community, ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True)
            Me._UseCache = UseCache
            Me._CurrentUser = oPerson
            Me._CurrentCommunity = oCommunity
            Me._Language = oLanguage
        End Sub

        Public Sub New(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer, Optional ByVal UseCache As Boolean = True)
            Me._UseCache = UseCache
            Me._CurrentUser = New Person(PersonID)
            Me._CurrentCommunity = New Community(CommunityID)
            Me._Language = ManagerLingua.GetByID(LanguageID)
        End Sub

        Public Function GetRolesAvailableByCommunity(ByVal oCommunitiesIdList As List(Of Integer), Optional ByVal ForceRetrieve As Boolean = False) As List(Of Role)
            Dim oList As New List(Of Role)
            If Me._UseCache Then
                Dim cacheKey As String = CachePolicySearch.SearchRoleAvailable(Me._Language.ID, oCommunitiesIdList)
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = GetRolesAvailableByCommunityFromDal(Me._Language.ID, oCommunitiesIdList)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of Role))
                End If
            Else
                oList = GetRolesAvailableByCommunityFromDal(Me._Language.ID, oCommunitiesIdList)
            End If
            Return oList
        End Function
        Private Function GetRolesAvailableByCommunityFromDal(ByVal LinguaID As Integer, ByVal ListCommunityID As List(Of Integer)) As List(Of Role)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.ListRolesAvailableByCommunity(LinguaID, ListCommunityID)
        End Function
#Region "UCsearchuser"

        Public Function GetRolesByCommunity(ByVal oCommunitiesIdList As List(Of Integer), Optional ByVal ForceRetrieve As Boolean = False) As List(Of Role)
            Dim oList As New List(Of Role)
            If Me._UseCache Then
                Dim cacheKey As String = CachePolicySearch.SearchRole(Me._Language.ID, oCommunitiesIdList)
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = GetRolesByCommunityFromDal(Me._Language.ID, oCommunitiesIdList)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of Role))
                End If
            Else
                oList = GetRolesByCommunityFromDal(Me._Language.ID, oCommunitiesIdList)
            End If
            Return oList
        End Function
        Private Function GetRolesByCommunityFromDal(ByVal LinguaID As Integer, ByVal ListCommunityID As List(Of Integer)) As List(Of Role)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.ListRolesByCommunity(LinguaID, ListCommunityID)
        End Function
        Private Shared Function CompositeCommunityID(ByVal ListCommunityID As List(Of Integer)) As String
            Dim composite As String = String.Empty

            For Each id As Integer In ListCommunityID
                composite &= id.ToString & "_"
            Next

            Return composite
        End Function
        Public Function GetUserList(ByVal oCommunitiesIdList As List(Of Integer), ByRef CurrentUserId As Integer, Optional ByRef ForceRetrieve As Boolean = False) As List(Of MemberContact)
            Dim oList As New List(Of MemberContact)
            Dim cacheKey As String = CachePolicySearch.SearchUserByCommunitiesId(oCommunitiesIdList)
            If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                oList = GetUserListFromDal(oCommunitiesIdList, ConfirmedUserIdList(CurrentUserId))
                ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                oList = CType(ObjectBase.Cache(cacheKey), List(Of MemberContact))
            End If
            Return oList
        End Function

        Public Function GetUserIDsList(ByVal oCommunitiesIdList As List(Of Integer), ByRef CurrentUserId As Integer, Optional ByRef ForceRetrieve As Boolean = False) As List(Of Integer)
            'GetUserListFromDal()
            Dim oResponse As New List(Of Integer)
            Dim oList As List(Of MemberContact) = GetUserListFromDal(oCommunitiesIdList, ConfirmedUserIdList(CurrentUserId))

            For Each MC As MemberContact In oList
                Dim id As Integer = MC.Id

                oResponse.Add(id)
            Next


            Return oResponse
        End Function




        Public Function GetUserList(ByVal oCommunitiesIdList As List(Of Integer), ByVal Name As String, _
                                    ByVal Surname As String, ByVal RegistrationCode As String, ByVal MailAddress As String, _
                                    ByVal Login As String, ByVal SelectedRoleId As Integer, ByRef CurrentUserId As Integer, _
                                    Optional ByRef ForceRetrieve As Boolean = False) As List(Of MemberContact)
            Dim oList As New List(Of MemberContact)
            Dim SearchGUID As String = String.Empty

            'SearchGUID = CompositeCommunityID(oCommunitiesIdList) & Name & "_" & Surname & "_" & RegistrationCode & "_" & MailAddress & "_" & Login & "_" & SelectedRoleId
            'Dim cacheKey As String = CachePolicySearch.TemporaryUserIdList(SearchGUID)

            oList = GetUserList(oCommunitiesIdList, CurrentUserId, ForceRetrieve)

            Dim oFilteredList = From oElement As MemberContact In oList Where (isNothingOrEmpty(Name) OrElse oElement.Name.ToLower.Contains(Name.ToLower)) And (isNothingOrEmpty(Surname) OrElse oElement.Surname.ToLower.Contains(Surname.ToLower)) _
                            And (isNothingOrEmpty(RegistrationCode) OrElse oElement.RegistrationCode.ToLower.Contains(RegistrationCode.ToLower)) And (isNothingOrEmpty(MailAddress) OrElse oElement.Mail.ToLower.Contains(MailAddress.ToLower)) _
                            And (isNothingOrEmpty(Login) OrElse oElement.Login.ToLower.Contains(Login.ToLower)) And (SelectedRoleId = -1 OrElse oElement.MembershipInfo.Any(Function(o) o.MemberRole.ID = SelectedRoleId)) Order By oElement.Surname, oElement.Name Ascending
            '(YearID = -1 OrElse DirectCast(oIscrizione.CommunitySubscripted, UniversityCourse).AccademicY.Year = YearID) AndAlso (PeriodoID = -1 OrElse DirectCast(oIscrizione.CommunitySubscripted, UniversityCourse).CoursePeriodo.ID = PeriodoID)

            Return oFilteredList.ToList
        End Function
        Private Function isNothingOrEmpty(ByRef value As String) As Boolean
            If value Is Nothing Then
                Return True
            ElseIf value = String.Empty Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Property ConfirmedUserIdList(ByVal CurrentUserId As Integer)
            Get
                Dim SearchGUID As String
                SearchGUID = "UserIdList_" & CurrentUserId.ToString
                Dim cacheKey As String = CachePolicySearch.SearchRole(SearchGUID)
                If ObjectBase.Cache(cacheKey) Is Nothing Then
                    Return New List(Of Integer)
                Else
                    Return CType(ObjectBase.Cache(cacheKey), List(Of Integer))
                End If
            End Get
            Set(ByVal value)
                Dim SearchGUID As String
                SearchGUID = "UserIdList_" & CurrentUserId.ToString
                ObjectBase.Cache.Insert(SearchGUID, value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            End Set
        End Property
        Public Property TemporaryUserList(ByVal CurrentUserId As Integer)
            Get
                Dim cacheKey As String = CachePolicySearch.TemporaryUserIdList(CurrentUserId.ToString)
                Dim oList As New List(Of MemberContact)
                If Not CType(ObjectBase.Cache(cacheKey), List(Of MemberContact)) Is Nothing Then
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of MemberContact))
                End If
                Return oList
            End Get
            Set(ByVal value)
                Dim cacheKey As String = CachePolicySearch.TemporaryUserIdList(CurrentUserId.ToString)
                ObjectBase.Cache.Insert(cacheKey, value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            End Set
        End Property
        Private Function GetUserListFromDal(ByVal oCommunitiesIdList As List(Of Integer), ByRef oConfirmedUserIdList As List(Of Integer)) As List(Of MemberContact)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)
            Return oDALsubscriptions.ListForFilterUser(oCommunitiesIdList) ', oConfirmedUserIdList)
        End Function
#End Region

#Region "Subscription - Filters"
        Public Function GetFilterOrganization(ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal isLazy As Boolean = True, Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As New List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_Organization(Me._CurrentUser.ID, isLazy, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = GetOrganizationsFromDal(Me._CurrentUser, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = GetOrganizationsFromDal(Me._CurrentUser, oClause)
            End If
            Return oList
        End Function
        Public Function GetFilterStatus(ByVal oOrganization As Organization, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As New List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_Status(Me._CurrentUser.ID, oOrganization.OrganizationID, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = GetCommunityStatusFromDal(Me._CurrentUser, oOrganization.OrganizationID, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = GetCommunityStatusFromDal(Me._CurrentUser, oOrganization.OrganizationID, oClause)
            End If
            Return oList
        End Function
        Public Function GetFilterCommunityType(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As New List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_CommunityType(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = Me.GetCommunitiesTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = Me.GetCommunitiesTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
            End If
            Return oList
        End Function
        Public Function GetFilterDegreeType(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_DegreeType(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = Me.GetDegreeTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = Me.GetDegreeTypeFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oClause)
            End If
            Return oList
        End Function
        Public Function GetFilterPeriod(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oAccademicYear As AcademicYear, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As New List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_Periodo(Me._CurrentUser.ID, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = Me.GetPeriodoFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = Me.GetPeriodoFromDal(Me._CurrentUser, Me._Language.ID, oOrganization.OrganizationID, oStatus, oAccademicYear.Year, oClause)
            End If
            Return oList
        End Function
        Public Function GetFilterAccademicYear(ByVal oOrganization As Organization, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of FilterElement)
            Dim oList As New List(Of FilterElement)

            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscripted_AccademicYear(Me._CurrentUser.ID, oOrganization.OrganizationID, oStatus, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = Me.GetAccademicYearFromDal(Me._CurrentUser, oOrganization.OrganizationID, oStatus, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of FilterElement))
                End If
            Else
                oList = Me.GetAccademicYearFromDal(Me._CurrentUser, oOrganization.OrganizationID, oStatus, oClause)
            End If
            Return oList
        End Function


        Private Function CompositeServiceCode(ByVal oClause As GenericClause(Of ServiceClause)) As String
            Dim iCode As String = ""

            If IsNothing(oClause) Then
                Return iCode
            ElseIf IsNothing(oClause.NextClause) Then
                Return oClause.Clause.Service.Code
            Else
                iCode = oClause.Clause.Service.Code
                Select Case oClause.OperatorForNextClause
                    Case OperatorType.AndCondition
                        iCode &= "_AND_" & CompositeServiceCode(oClause.NextClause)
                    Case OperatorType.OrCondition
                        iCode &= "_OR_" & CompositeServiceCode(oClause.NextClause)
                    Case OperatorType.XorCondition
                        iCode &= "_XOR_" & CompositeServiceCode(oClause.NextClause)
                End Select
            End If
            Return iCode
        End Function
        Private Function HasPermission(ByVal oServiceClause As ServiceClause, ByVal PermissionToValidate As String) As String
            Dim intValid, intValidate As Int64

            intValid = Convert.ToInt64(New String(oServiceClause.Service.PermissionString.Reverse.ToArray), 2)
            intValidate = Convert.ToInt64(New String(PermissionToValidate.Reverse.ToArray), 2)

            Select Case oServiceClause.PermissionOperator
                Case OperatorType.AndCondition
                    Return intValid And intValidate
                Case OperatorType.OrCondition
                    Return intValid Or intValidate
                Case OperatorType.XorCondition
                    Return intValid Xor intValidate
                Case Else
                    Return False
            End Select
            Return False
        End Function

        Public Function FilterSubscriptions(ByVal isLazy As Boolean, ByVal oClause As GenericClause(Of ServiceClause), Optional ByVal ForceRetrieve As Boolean = False) As List(Of Subscription)
            Dim oList As New List(Of Subscription)
            If Me._UseCache Then
                Dim cacheKey As String = CachePolicyPerson.Subscription(Me._CurrentUser.ID, isLazy, "CompositeServiceCode-" & CompositeServiceCode(oClause))
                If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                    oList = GetSubscriptionsFromDal(Me._CurrentUser, isLazy, oClause)
                    ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                Else
                    oList = CType(ObjectBase.Cache(cacheKey), List(Of Subscription))
                End If
            Else
                oList = GetSubscriptionsFromDal(Me._CurrentUser, isLazy, oClause)
            End If

            Return oList
        End Function

#Region "Common function to Retrieve from DAL"
        Private Function GetOrganizationsFromDal(ByVal oPerson As Person, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.FiltersOrganization(oPerson, oClause)
        End Function
        Private Function GetCommunityStatusFromDal(ByVal oPerson As Person, ByVal oOrganizationID As Integer, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)
            Dim oList As List(Of FilterElement) = oDALsubscriptions.FiltersCommunityStatus(oPerson, oOrganizationID, oClause)

            For Each oElement As FilterElement In oList
                Select Case oElement.Value
                    Case CommunityStatus.Archiviata
                        oElement.Text = My.Resources.Resource.CommunityStatus_Archiviata
                    Case CommunityStatus.ClosedByAdministration
                        oElement.Text = My.Resources.Resource.CommunityStatus_ClosedByAdministration
                    Case CommunityStatus.OnlyActivated
                        oElement.Text = My.Resources.Resource.CommunityStatus_OnlyActivated
                End Select
            Next
            Return oList
        End Function
        Private Function GetCommunitiesTypeFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)
            Return oDALsubscriptions.FiltersCommunityType(oPerson, oClause, LanguageID, oOrganizationID, oStatus)
        End Function
        Private Function GetDegreeTypeFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.FiltersDegreeType(oPerson, LanguageID, oOrganizationID, oStatus, oClause)
        End Function
        Private Function GetPeriodoFromDal(ByVal oPerson As Person, ByVal LanguageID As Integer, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal AccademicYearID As Integer, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.FiltersPeriodo(oPerson, LanguageID, oOrganizationID, oStatus, AccademicYearID, oClause)
        End Function
        Private Function GetAccademicYearFromDal(ByVal oPerson As Person, ByVal oOrganizationID As Integer, ByVal oStatus As CommunityStatus, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of FilterElement)

            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)

            Return oDALsubscriptions.FiltersAccademicYear(oPerson, oOrganizationID, oStatus, oClause)
        End Function
        Private Function GetSubscriptionsFromDal(ByVal oPerson As Person, ByVal isLazy As Boolean, ByVal oClause As GenericClause(Of ServiceClause)) As List(Of Subscription)
            Dim oDALsubscriptions As New DAL.StandardDB.DALsubscriptions(GetCurrentDB)
            If isLazy Then
                Return oDALsubscriptions.ListForFilters(oPerson, Me._Language.ID, oClause)
            Else
                Return New List(Of Subscription)
            End If
        End Function
#End Region
#End Region
	End Class
End Namespace