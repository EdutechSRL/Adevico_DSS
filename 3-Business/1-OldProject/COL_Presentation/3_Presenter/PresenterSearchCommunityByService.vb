Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Dynamic
Imports System.Reflection

Imports COL_BusinessLogic_v2.UCServices

Public Class PresenterSearchCommunityByService
	Inherits GenericPresenter

	Private _Manager As ManagerSubscription
	Private _ManagerAdvanced As ManagerSubscriptionAdvanced

	Private ReadOnly Property CurrentManager() As ManagerSubscription
		Get
			If _Manager Is Nothing Then
				_Manager = New ManagerSubscription(Me.View.CurrentSubscriber, Me.View.CurrentCommunity, Me.View.CurrentLanguage)
			End If
			Return _Manager
		End Get
	End Property

	Public Sub New(ByVal view As IviewSearchCommunity)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewSearchCommunity
		Get
			View = MyBase.view
		End Get
	End Property

    Public Sub Init(ByVal ExludeCommunityID As Integer)
        Dim oList As New List(Of Integer)
        oList.Add(ExludeCommunityID)
        Me.Init(oList)
    End Sub
    Public Sub Init(ByVal CommunitiesID As List(Of Integer))
        View.ExludeCommunities = CommunitiesID
        View.LoadCommunityTypes(Nothing)
        SetNullByCommunityType()
        View.SelectedCommunitiesID = New List(Of Integer)
        'Me.View.LoadCommunities(Nothing)
        LoadOrganizations()

        If Not IsNothing(Me.View.CurrentOrganization) Then
            LoadStatus()
            If View.CurrentStatus <> CommunityStatus.None Then
                LoadCommunityType()
            End If
        Else
            View.LoadStatus(New List(Of CommunityStatus))
        End If
        GoToPage(1)
    End Sub
	Private Sub SetNullByCommunityType()
		Me.View.LoadDegreeTypes(New List(Of TypeDegree))
		Me.View.LoadPeriodi(New List(Of Periodo))
		Me.View.LoadAcademicYears(New List(Of AcademicYear))
	End Sub
	Private Sub LoadOrganizations()
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterOrganization(Me.View.ServiceClauses)
		Dim oReturnedList As New List(Of FilterElement)

		If oList.Count > 1 And Me.View.AllowMultipleOrganizationSelection Then
			oReturnedList.AddRange(oList)
			oReturnedList.Insert(0, (New FilterElement(-1, My.Resources.Resource.ALLorganization)))
			Me.View.HasCommunity = True
			Me.View.LoadOrganizations(oReturnedList)
		Else
			Me.View.HasCommunity = oList.Count > 0
			Me.View.LoadOrganizations(oList)
		End If
	End Sub
	Private Sub LoadStatus()
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterStatus(Me.View.CurrentOrganization, Me.View.ServiceClauses)
		Dim oReturnedList As New List(Of FilterElement)
		If oList.Count = 0 Then
			oReturnedList.Insert(0, (New FilterElement(CommunityStatus.None, My.Resources.Resource.CommunityStatus_None)))
		ElseIf oList.Count > 1 And Me.View.AllowMultipleOrganizationSelection Then
			oReturnedList.AddRange(oList)
			oReturnedList.Insert(0, (New FilterElement(CommunityStatus.All, My.Resources.Resource.CommunityStatus_All)))
			Me.View.LoadStatus(oReturnedList)
		Else
			Me.View.LoadStatus(oList)
		End If
	End Sub

	Private Sub LoadCommunityType(Optional ByVal oType As CommunityType = Nothing)
		Me.SetNullByCommunityType()


		Me.View.LoadCommunityTypes(Me.RetrieveCommunityTypes)
		If Not IsNothing(oType) Then
			Me.View.CurrentCommunityType = oType
		End If
		'Me.View.LoadCommunityTypes(Me.CurrentManager.GetFilterCommunityType(Me.View.CurrentOrganization, Me.View.CurrentStatus, Me.View.CurrentServiceCode, "Name"))



		If Not Me.View.CurrentCommunityType Is Nothing Then
			Select Case Me.View.CurrentCommunityType.ID
				Case StandardCommunityType.Degree
					Me.LoadDegreeTypes()
				Case StandardCommunityType.UniversityCourse
					Me.LoadAccademicYears()
					If Not IsNothing(Me.View.CurrentAcademicYear) Then
						Me.LoadPeriodi()
					End If
			End Select
		End If
	End Sub

	Private Function RetrieveCommunityTypes() As List(Of FilterElement)
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterCommunityType(Me.View.CurrentOrganization, Me.View.CurrentStatus, Me.View.ServiceClauses)
		Dim oReturnedList As New List(Of FilterElement)

		If oList.Count > 1 Then
			oReturnedList.AddRange(From oFilterElement As FilterElement In oList Order By oFilterElement.Text)
			oReturnedList.Insert(0, (New FilterElement(-1, "--		--")))
			Return oReturnedList
		Else
			Return oList
		End If
	End Function
	Private Sub LoadDegreeTypes()
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterDegreeType(Me.View.CurrentOrganization, Me.View.CurrentStatus, Me.View.ServiceClauses)
		Dim oReturnedList As New List(Of FilterElement)

		If oList.Count > 1 Then
			oReturnedList.AddRange(From oFilterElement As FilterElement In oList Order By oFilterElement.Text)
			oReturnedList.Insert(0, (New FilterElement(-1, "--		--")))
			Me.View.LoadDegreeTypes(oReturnedList)
		Else
			Me.View.LoadDegreeTypes(oList)
		End If
	End Sub
	Private Sub LoadAccademicYears()
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterAccademicYear(Me.View.CurrentOrganization, Me.View.CurrentStatus, Me.View.ServiceClauses)

		Dim oReturnedList As New List(Of FilterElement)

		If oList.Count > 1 Then
			oReturnedList.AddRange(From oFilterElement As FilterElement In oList Order By oFilterElement.Text Descending)
			oReturnedList.Insert(0, (New FilterElement(-1, "--		--")))
			Me.View.LoadAcademicYears(oReturnedList)
		Else
			Me.View.LoadAcademicYears(oList)
		End If
	End Sub
	Private Sub LoadPeriodi()
		Dim oList As List(Of FilterElement) = CurrentManager.GetFilterPeriod(Me.View.CurrentOrganization, Me.View.CurrentStatus, Me.View.CurrentAcademicYear, Me.View.ServiceClauses)
		Dim oReturnedList As New List(Of FilterElement)

		If oList.Count > 1 Then
			oReturnedList.AddRange(From oFilterElement As FilterElement In oList Order By oFilterElement.Text)
			oReturnedList.Insert(0, (New FilterElement(-1, "--		--")))
			Me.View.LoadPeriodi(oReturnedList)
		Else
			Me.View.LoadPeriodi(oList)
		End If
	End Sub


	Public Sub ChangeOrganization()
		Me.View.LoadCommunityTypes(New List(Of CommunityType))
		Me.SetNullByCommunityType()
		Me.LoadStatus()
		If Me.View.CurrentStatus = CommunityStatus.None Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.View.LoadCommunities(New List(Of Community))
		Else
			Me.LoadCommunityType()
			If Me.View.AutoUpdateList Then
				Me.View.SelectedCommunitiesID = New List(Of Integer)
				Me.GoToPage(1)
			End If
		End If
	End Sub
	Public Sub ChangeStatus()
		Dim oType As CommunityType = Me.View.CurrentCommunityType
		Me.SetNullByCommunityType()
		Me.LoadCommunityType(Me.View.CurrentCommunityType)
		If Me.View.AutoUpdateList Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.GoToPage(1)
		End If
	End Sub
	Public Sub ChangeCommunityType()
		Me.SetNullByCommunityType()
		Select Case Me.View.CurrentCommunityType.ID
			Case StandardCommunityType.Degree
				Me.LoadDegreeTypes()
			Case StandardCommunityType.UniversityCourse
				Me.LoadAccademicYears()
				If Not IsNothing(Me.View.CurrentAcademicYear) Then
					Me.LoadPeriodi()
				End If
		End Select
		If Me.View.AutoUpdateList Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.GoToPage(1)
		End If
	End Sub
	Public Sub ChangeDegree()
		If Me.View.AutoUpdateList Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.GoToPage(1)
		End If
	End Sub
	Public Sub ChangeAccademicYear()
		Me.LoadPeriodi()
		If Me.View.AutoUpdateList Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.GoToPage(1)
		End If
	End Sub
	Public Sub ChangePeriodo()
		If Me.View.AutoUpdateList Then
			Me.View.SelectedCommunitiesID = New List(Of Integer)
			Me.GoToPage(1)
		End If
	End Sub


	Public Sub LoadCommunityList()
		Me.GoToPage(1)
	End Sub

	Private Function FilterSubscriptions() As IList(Of Subscription)
		Dim oSubscriptionList As List(Of Subscription) = FilterLinqSubscriptions()


		'Return (From o As Subscription In oSubscriptionList Order By (Me.View.OrderBy & " desc")).ToList

		If Me.View.OrderBy = "" Or Me.View.OrderBy = "Name" Then
			Return IIf(Me.View.Direction = Comol.Entity.sortDirection.Descending, (oSubscriptionList.OrderByDescending(Function(o) o.CommunitySubscripted.Name)).ToList, (oSubscriptionList.OrderBy(Function(o) o.CommunitySubscripted.Name)).ToList)
		Else
			'Return (From o As Subscription In oSubscriptionList Order By (Me.View.OrderBy & " desc")).ToList


			Return IIf(Me.View.Direction = Comol.Entity.sortDirection.Descending, (oSubscriptionList.OrderByDescending(Function(o) o.CommunitySubscripted.Type.Name)).ToList, (oSubscriptionList.OrderBy(Function(o) o.CommunitySubscripted.Type.Name)).ToList)
		End If
	End Function

	Private Function FilterLinqSubscriptions() As IList(Of Subscription)
		Dim oSubscriptionList As List(Of Subscription)
		Dim oList As New List(Of Subscription)
		oSubscriptionList = Me.CurrentManager.FilterSubscriptions(True, Me.View.ServiceClauses, False)

        Dim ExludeID As List(Of Integer) = Me.View.ExludeCommunities

        oSubscriptionList = (From s In oSubscriptionList Where Not ExludeID.Contains(s.CommunitySubscriptedID) Select s).ToList
		If oSubscriptionList.Count > 0 Then
			Dim OrganizationID As Integer = Me.View.CurrentOrganization.OrganizationID
			Dim CommunityTypeID As Integer = -1
			Dim StatusID As CommunityStatus = Me.View.CurrentStatus
			Dim DegreeTypeID As Integer = -1, YearID As Integer = -1, PeriodoID As Integer = -1

			If Not Me.View.CurrentDegreeType Is Nothing Then
				DegreeTypeID = Me.View.CurrentDegreeType.ID
			End If
			If Not Me.View.CurrentCommunityType Is Nothing Then
				CommunityTypeID = Me.View.CurrentCommunityType.ID
			End If
			If Not Me.View.CurrentPeriodo Is Nothing Then
				PeriodoID = Me.View.CurrentPeriodo.ID
			End If
			If Not Me.View.CurrentAcademicYear Is Nothing Then
				YearID = Me.View.CurrentAcademicYear.Year
			End If


			Dim oQuery = From oIscrizione As Subscription In oSubscriptionList _
			Where (OrganizationID = -1 OrElse (oIscrizione.CommunitySubscripted.Type.ID = StandardCommunityType.Organization AndAlso DirectCast(oIscrizione.CommunitySubscripted, Organization).OrganizationID = OrganizationID) OrElse _
			(oIscrizione.CommunitySubscripted.Type.ID <> StandardCommunityType.Organization AndAlso oIscrizione.CommunitySubscripted.Organization.OrganizationID = OrganizationID)) AndAlso _
			(oIscrizione.CommunitySubscripted.Status = Me.View.CurrentStatus Or Me.View.CurrentStatus = CommunityStatus.All) AndAlso _
			(CommunityTypeID = -1 OrElse oIscrizione.CommunitySubscripted.Type.ID = CommunityTypeID)

			If oQuery.Count > 0 And CommunityTypeID = StandardCommunityType.UniversityCourse Then
				oQuery = From oIscrizione As Subscription In oQuery Where oIscrizione.CommunitySubscripted.Type.ID = CommunityTypeID AndAlso (YearID = -1 OrElse DirectCast(oIscrizione.CommunitySubscripted, UniversityCourse).AccademicY.Year = YearID) AndAlso (PeriodoID = -1 OrElse DirectCast(oIscrizione.CommunitySubscripted, UniversityCourse).CoursePeriodo.ID = PeriodoID)
			ElseIf oQuery.Count > 0 And CommunityTypeID = StandardCommunityType.Degree Then
				oQuery = From oIscrizione As Subscription In oQuery Where (oIscrizione.CommunitySubscripted.Type.ID = CommunityTypeID AndAlso (DegreeTypeID = -1 OrElse DirectCast(oIscrizione.CommunitySubscripted, Degree).DegreeType.ID = DegreeTypeID))


			End If


			Dim SearchBy As String = Me.View.SearchBy
			Dim Searchtype As StandardCommunitySearch = Me.View.CurrentSearch
			If oQuery.Count > 0 And SearchBy <> "" And Searchtype <> StandardCommunitySearch.All Then
				SearchBy = Trim(SearchBy)
				If SearchBy <> "" Then
					SearchBy = SearchBy.ToLower
					oQuery = From oIscrizione As Subscription In oQuery Where (Searchtype = StandardCommunitySearch.NameStartWith AndAlso oIscrizione.CommunitySubscripted.Name.ToLower.StartsWith(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.NameContains AndAlso oIscrizione.CommunitySubscripted.Name.ToLower.Contains(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.Responsible AndAlso oIscrizione.CommunitySubscripted.Responsible.ID = Me.View.CurrentResponsibleID) OrElse (Searchtype = StandardCommunitySearch.SurnameResponsible AndAlso oIscrizione.CommunitySubscripted.Responsible.Surname.Contains(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.CreateAfter AndAlso oIscrizione.CommunitySubscripted.CreatedAt >= CDate(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.CreateBefore AndAlso oIscrizione.CommunitySubscripted.CreatedAt <= CDate(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.SubscriptionAfter AndAlso oIscrizione.CommunitySubscripted.StartSubscription >= CDate(SearchBy)) OrElse (Searchtype = StandardCommunitySearch.SubscriptionBefore AndAlso oIscrizione.CommunitySubscripted.StartSubscription <= CDate(SearchBy))

				End If
			End If


			If oQuery.Count > 0 Then
				oList = oQuery.ToList
			End If

		End If
		Return oList
	End Function

	Public Sub GoToPage(ByVal PageIndex As Integer)
		Dim oLista As IList = Me.FilterSubscriptions
		If oLista.Count = 0 Then
			Me.View.GridCurrentPage = 1
		Else
			Me.View.GridMaxPage = Math.Ceiling(oLista.Count / Me.View.GridPageSize)
			If PageIndex > Me.View.GridMaxPage Then
				PageIndex = Me.View.GridMaxPage
			End If
			Me.View.GridCurrentPage = PageIndex
		End If

		Me.View.LoadCommunities(oLista)
	End Sub

	'Public Function ServiceAvailable(ByVal CommunityID As Integer) As Boolean
	'	Dim ServiceCode As String = Me.View.CurrentServiceCode
	'	Dim oService As CommunityService = Me.CurrentManager.GetcommunityService(CommunityID, ServiceCode)
	'	'	Dim oCurrentService As ServiceBase = Me.View.CurrentService
	'	'	Dim st As String = ""

	'	'	st = oCurrentService.PermissionString.Reverse
	'	'	st.ge()
	'	'And oService.Service.PermissionString.Reverse

	'	Select Case ServiceCode
	'		Case Services_File.Codex
	'			Dim oServizio As New Services_File(oService.Service.PermissionString)
	'			Return (oServizio.Admin Or oServizio.Moderate)

	'		Case Services_Mail.Codex
	'			Dim oServizio As New Services_Mail(oService.Service.PermissionString)
	'			Return (oServizio.Admin Or oServizio.SendMail)

	'		Case Services_PostIt.Codex
	'			Dim oServizio As New Services_PostIt(oService.Service.PermissionString)
	'			Return (oServizio.GestioneServizio Or oServizio.UsoServizio Or oServizio.Create)

	'		Case Services_RaccoltaLink.Codex
	'			Dim oServizio As New Services_RaccoltaLink(oService.Service.PermissionString)
	'			Return (oServizio.Admin Or oServizio.ExportLink Or oServizio.Moderate)

	'		Case Services_Forum.Codex
	'			Dim oServizio As New Services_Forum(oService.Service.PermissionString)
	'			Return oServizio.GestioneForum

	'		Case Services_Eventi.Codex
	'			Dim oServizio As New Services_Eventi(oService.Service.PermissionString)
	'			Return oServizio.AddEvents Or oServizio.AdminService Or oServizio.ChangeEvents

	'		Case Services_Bacheca.Codex
	'			Dim oServizio As New Services_Bacheca(oService.Service.PermissionString)
	'			Return oServizio.Admin Or oServizio.Write

	'		Case Services_Cover.Codex
	'			Dim oServizio As New Services_Cover(oService.Service.PermissionString)
	'			Return oServizio.Admin Or oServizio.Management
	'		Case Else
	'			Return False
	'	End Select
	'End Function

	Private Function HasPermission(ByVal oServiceClause As ServiceClause, ByVal PermissionToValidate As String) As String
		Dim intValid, intValidate As Int64
		intValid = Convert.ToInt64(oServiceClause.Service.PermissionString.Reverse, 2)
		intValidate = Convert.ToInt64(PermissionToValidate.Reverse, 2)

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
	'Public Function SearchByText(ByVal iSearch As StandardCommunitySearch, ByVal Value As String, ByVal oList As List(Of Subscription)) As List(Of Subscription)
	'	Dim oFound As New List(Of Subscription)

	'	Select Case iSearch
	'		Case StandardCommunitySearch.CreateAfter
	'			Try
	'				oFound = oList.FindAll(New GenericPredicate(Of Subscription, DateTime)(CDate(Value), AddressOf Subscription.FindByCreatedAfterData))
	'			Catch ex As Exception

	'			End Try
	'		Case StandardCommunitySearch.CreateBefore
	'			Try
	'				oFound = oList.FindAll(New GenericPredicate(Of Subscription, DateTime)(CDate(Value), AddressOf Subscription.FindByCreatedBeforeData))
	'			Catch ex As Exception

	'			End Try
	'		Case StandardCommunitySearch.NameStartWith
	'			oFound = oList.FindAll(New GenericPredicate(Of Subscription, String)(Value, AddressOf Subscription.FindByNameStartWith))
	'		Case StandardCommunitySearch.NameContains
	'			oFound = oList.FindAll(New GenericPredicate(Of Subscription, String)(Value, AddressOf Subscription.FindByNameContains))
	'		Case StandardCommunitySearch.Responsible
	'			oFound = oList.FindAll(New GenericPredicate(Of Subscription, Integer)(Me.View.CurrentResponsibleID, AddressOf Subscription.FindByResponsible))
	'		Case StandardCommunitySearch.SubscriptionAfter
	'			Try
	'				oFound = oList.FindAll(New GenericPredicate(Of Subscription, DateTime)(CDate(Value), AddressOf Subscription.FindBySubscriptionAfterData))
	'			Catch ex As Exception

	'			End Try
	'		Case StandardCommunitySearch.SubscriptionBefore
	'			Try
	'				oFound = oList.FindAll(New GenericPredicate(Of Subscription, DateTime)(CDate(Value), AddressOf Subscription.FindBySubscriptionBeforeData))
	'			Catch ex As Exception

	'			End Try
    '		Case StandardCommunitySearch.SurnameResponsible
    '			Try
    '				oFound = oList.FindAll(New GenericPredicate(Of Subscription, String)(Value, AddressOf Subscription.FindBySurnameResponsible))
	'			Catch ex As Exception

	'			End Try
	'	End Select
	'	Return oFound
	'End Function

	Public Sub SelectCommunity(ByVal CommunityID As Integer)
		Dim SelectedID As List(Of Integer) = Me.View.SelectedCommunitiesID
		If IsNothing(SelectedID) Then
			SelectedID = New List(Of Integer)
        End If
        If Not SelectedID.Contains(CommunityID) Then
            If View.SelectionMode = ListSelectionMode.Multiple Then
                SelectedID.Add(CommunityID)
            Else
                SelectedID.Clear()
                SelectedID.Add(CommunityID)
            End If
        End If

        Me.View.SelectedCommunitiesID = SelectedID
    End Sub
    Public Sub SelectCommunity(ByVal idCommunity As Integer, name As String)
        Dim SelectedID As List(Of Integer) = Me.View.SelectedCommunitiesID
        Dim names As Dictionary(Of Integer, String) = Me.View.SelectedCommunities
        If Not SelectedID.Contains(idCommunity) Then
            If View.SelectionMode = ListSelectionMode.Multiple Then
                SelectedID.Add(idCommunity)
            Else
                SelectedID.Clear()
                SelectedID.Add(idCommunity)
            End If
            If Not names.ContainsKey(idCommunity) Then
                names(idCommunity) = name
            End If
        End If
        Me.View.SelectedCommunities = names
        Me.View.SelectedCommunitiesID = SelectedID
    End Sub
End Class