Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.CommunityManagement

Public Class UC_ViewCommunitiesTree
    Inherits DBbaseControl
    Implements IViewCommunitiesTree

#Region "Context"
    Private _Presenter As CommunitiesTreePresenter
    Private ReadOnly Property CurrentPresenter() As CommunitiesTreePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CommunitiesTreePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewCommunitiesTree.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property ReferenceIdCommunity As Integer Implements IViewCommunitiesTree.ReferenceIdCommunity
        Get
            Return ViewStateOrDefault("ReferenceIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("ReferenceIdCommunity") = value
        End Set
    End Property
    Public Property AdvancedMode As Boolean Implements IViewCommunitiesTree.AdvancedMode
        Get
            Return ViewStateOrDefault("AdvancedMode", False)
        End Get
        Set(value As Boolean)
            ViewState("AdvancedMode") = value
        End Set
    End Property
    Public Property IsFirstLoad As Boolean Implements IViewCommunitiesTree.IsFirstLoad
        Get
            Return ViewStateOrDefault("IsFirstLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("IsFirstLoad") = value
        End Set
    End Property
    Public Property LoadMode As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability Implements IViewCommunitiesTree.LoadMode
        Get
            Return ViewStateOrDefault("AdvancedMode", lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed)
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability)
            ViewState("AdvancedMode") = value
        End Set
    End Property
    Private Property CurrentFilters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewCommunitiesTree.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters())
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event SessionTimeout()
    Public Event HideDisplayMessage()
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event OpenConfirmDialog(ByVal openCssClass As String)
    Public Event SetDefaultFilters(ByVal filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))

    Public Property HasDisplayMessage As Boolean
        Get
            Return ViewStateOrDefault("HasDisplayMessage", False)
        End Get
        Set(value As Boolean)
            ViewState("HasDisplayMessage") = value
        End Set
    End Property
    Public Property DisplayMessageOnControl As Boolean
        Get
            Return ViewStateOrDefault("DisplayMessageOnControl", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayMessageOnControl") = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTtreeFiltersTitle)
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLinkButton(LNBapplyTreeFilters, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(advancedMode As Boolean, Optional availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, Optional idReferenceCommunity As Integer = 0, Optional referencePath As String = "") Implements IViewCommunitiesTree.InitializeControl
        CurrentPresenter.InitView(advancedMode, availability, idReferenceCommunity, referencePath)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewCommunitiesTree.DisplaySessionTimeout
        HideItems()
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewCommunitiesTree.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idActionCommunity As Integer, action As ModuleDashboard.ActionType) Implements IViewCommunitiesTree.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Community, idActionCommunity), InteractionType.UserWithLearningObject)
    End Sub

#Region "Load tree"
    Private Sub LoadDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Implements IViewCommunitiesTree.LoadDefaultFilters
        RaiseEvent SetDefaultFilters(filters)
        DVfilters.Visible = filters.Any()
    End Sub
    Private Function GetIdcommunitiesWithNews(idCommunities As List(Of Integer), iduser As Integer) As List(Of Integer) Implements IViewCommunitiesTree.GetIdcommunitiesWithNews
        If idCommunities.Any Then
            Dim service As New lm.Modules.NotificationSystem.Business.ManagerCommunitynews(PageUtility.CurrentContext)

            Return (From r As lm.Modules.NotificationSystem.Presentation.dtoCommunityNewsCount In service.GetCommunityNewsCount(iduser)
                         Where idCommunities.Contains(r.ID) Select r.ID).ToList()
        Else
            Return New List(Of Integer)
        End If
    End Function
    Private Sub LoadTree(items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem)) Implements IViewCommunitiesTree.LoadTree
        If items.Any Then
            MLVtree.SetActiveView(VIWtree)
            RPTchildren.DataSource = items
            RPTchildren.DataBind()
        Else
            MLVtree.SetActiveView(VIWmessage)
            LTmessage.Text = Resource.getValue("IViewCommunitiesTree.LoadTree." & IsFirstLoad.ToString)
        End If
        IsFirstLoad = False
    End Sub
    Public Sub DisplayNoTreeToLoad(cName As String) Implements IViewCommunitiesTree.DisplayNoTreeToLoad
        MLVtree.SetActiveView(VIWmessage)
        If String.IsNullOrEmpty(cName) Then
            LTmessage.Text = Resource.getValue("IViewCommunitiesTree.DisplayNoTreeToLoad")
        Else
            LTmessage.Text = String.Format(Resource.getValue("IViewCommunitiesTree.DisplayNoTreeToLoad.Name"), cName)
        End If
        DVfilters.Visible = False
    End Sub

    Public Sub DisplayUnableToLoadTree() Implements IViewCommunitiesTree.DisplayUnableToLoadTree
        MLVtree.SetActiveView(VIWmessage)
        LTmessage.Text = Resource.getValue("IViewCommunitiesTree.DisplayUnableToLoadTree")
        DVfilters.Visible = False
    End Sub
#End Region

#Region "Remove enrollments"
    Private Sub DisplayConfirmMessage(idCommunity As Integer, path As String, community As lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode, actions As List(Of RemoveAction), selected As RemoveAction, Optional alsoFromCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode) = Nothing) Implements IViewCommunitiesTree.DisplayConfirmMessage
        InternalHideDisplayMessage()

        RaiseEvent OpenConfirmDialog(CTRLconfirmUnsubscription.DialogIdentifier)
        CTRLconfirmUnsubscription.Visible = True
        CTRLconfirmUnsubscription.InitializeControl(idCommunity, path, community, actions, selected, alsoFromCommunities, Resource.getValue("ConfirmUnsubscription.Description"))
    End Sub
    Private Sub DisplayUnableToUnsubscribe(community As String) Implements IViewCommunitiesTree.DisplayUnableToUnsubscribe
        InternalDisplayMessage(String.Format(Resource.getValue("DisplayUnableToUnsubscribe"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Private Sub DisplayUnsubscribeNotAllowed(community As String) Implements IViewCommunitiesTree.DisplayUnsubscribeNotAllowed
        InternalDisplayMessage(String.Format(Resource.getValue("DisplayUnsubscribeNotAllowed"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayUnsubscriptionMessage(unsubscribedFrom As List(Of String), unableToUnsubscribeFrom As List(Of String)) Implements IViewCommunitiesTree.DisplayUnsubscriptionMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        Dim dMessage As String = ""
        Dim unableItems As String = LTtemplateMessageDetails.Text
        Dim removedItems As String = LTtemplateMessageDetails.Text
        HasDisplayMessage = True
        If unsubscribedFrom.Any() AndAlso unableToUnsubscribeFrom.Any() Then
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            unableItems = String.Format(unableItems, String.Join("", unableToUnsubscribeFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            removedItems = String.Format(removedItems, String.Join("", unsubscribedFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.alert"), removedItems, unableItems)
        ElseIf unableToUnsubscribeFrom.Any() Then
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            unableItems = String.Format(unableItems, String.Join("", unableToUnsubscribeFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.error"), unableItems)
        ElseIf unsubscribedFrom.Any() Then
            removedItems = String.Format(removedItems, String.Join("", unsubscribedFrom.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s)).ToList()))

            dMessage = String.Format(Resource.getValue("DisplayUnsubscriptionMessage.success"), removedItems)
        Else
            dMessage = Resource.getValue("DisplayUnsubscriptionMessage.None")
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End If

        InternalDisplayMessage(dMessage, t)
    End Sub
    Private Sub DisplayUnsubscribedFrom(community As String) Implements IViewCommunitiesTree.DisplayUnsubscribedFrom
        InternalDisplayMessage(String.Format(Resource.getValue("DisplayUnsubscriptionMessage.success"), community), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
#End Region

#Region "Enroll To"
    Private Sub DisplayUnknownCommunity(name As String) Implements IViewCommunitiesTree.DisplayUnknownCommunity
        InternalDisplayMessage(String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayUnknownCommunity"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayEnrollMessage(item As dtoCommunityInfoForEnroll, action As ModuleDashboard.ActionType) Implements IViewCommunitiesTree.DisplayEnrollMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Select Case action
            Case ModuleDashboard.ActionType.EnrollNotAllowed
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case ModuleDashboard.ActionType.UnableToEnroll
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType.EnrollToCommunity
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        Dim message As String = ""
        Select Case action
            Case ModuleDashboard.ActionType.EnrollNotAllowed

                Dim status As List(Of EnrollingStatus) = item.NotAvailableFor
                If status.Contains(EnrollingStatus.StartDate) AndAlso status.Contains(EnrollingStatus.EndDate) Then
                    status.Remove(EnrollingStatus.StartDate)
                End If
                If status.Contains(EnrollingStatus.Unavailable) Then
                    status = status.Where(Function(s) s = EnrollingStatus.Unavailable).ToList()
                End If
                If status.Count = 1 Then
                    message = Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.EnrollingStatus." & status.FirstOrDefault().ToString)
                    Select Case status.FirstOrDefault()
                        Case EnrollingStatus.StartDate
                            message = String.Format(message, item.Name, GetDateTimeString(item.SubscriptionStartOn, "", True))
                        Case EnrollingStatus.EndDate
                            message = String.Format(message, item.Name, GetDateTimeString(item.SubscriptionEndOn, "", True))
                        Case Else
                            message = String.Format(message, item.Name)
                    End Select
                Else
                    message = String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.EnrollingStatus.n"), String.Join(", ", status.Select(Function(i) Resource.getValue("DisplayEnrollMessage.EnrollingStatus.n." & i.ToString)).ToArray))
                End If

            Case Else
                message = String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.ActionType." & action.ToString), item.Name)
        End Select
        InternalDisplayMessage(message, t)
    End Sub
    Private Sub DisplayConfirmMessage(item As dtoCommunityInfoForEnroll) Implements IViewCommunitiesTree.DisplayConfirmMessage
        InternalHideDisplayMessage()
        RaiseEvent OpenConfirmDialog(CTRLconfirmEnrollToCommunity.DialogIdentifier)
        '   DVconfirmUnsubscription.Visible = True
        CTRLconfirmEnrollToCommunity.Visible = True
        CTRLconfirmEnrollToCommunity.InitializeControl(item, String.Format(Resource.getValue("ConfirmEnrollTo.Description"), item.Name))
    End Sub
    Private Sub DisplayEnrollMessage(item As dtoEnrollment, idCommunity As Integer, person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String) Implements IViewCommunitiesTree.DisplayEnrollMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
        Select Case item.Status
            Case EnrolledStatus.NeedConfirm
                NotifyEnrollment(item, person, profileType, organizationName)
            Case EnrolledStatus.Available
                NotifyEnrollment(item, person, profileType, organizationName)
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case EnrolledStatus.PreviousEnrolled
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case Else
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        End Select

        InternalDisplayMessage(String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.EnrolledStatus." & item.Status.ToString), item.CommunityName), t)
    End Sub
    Private Function GetMessage(status As EnrolledStatus, enrolledItems As List(Of dtoEnrollment)) As String
        Dim message As String = "DisplayEnrollMessage.EnrolledStatus." & status.ToString & "."
        Select Case enrolledItems.Count
            Case 1
                message &= "1"
                Return String.Format(Resource.getValue(message), enrolledItems.FirstOrDefault().CommunityName)
            Case Else
                message &= "n"
                Return String.Format(Resource.getValue(message), enrolledItems.Count)
        End Select
    End Function
    Private Function GetMessage(status As EnrolledStatus, names As List(Of String)) As String
        Dim message As String = "DisplayEnrollMessage.EnrolledStatus." & status.ToString & "."
        Select Case names.Count
            Case 1
                message &= "1"
                Return String.Format(Resource.getValue(message), names.FirstOrDefault())
            Case Else
                message &= "n"
                Return String.Format(Resource.getValue(message), names.Count)
        End Select
    End Function
    Private Sub NotifyEnrollment(item As dtoEnrollment, person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String) Implements IViewCommunitiesTree.NotifyEnrollment
        Dim oServiceUtility As New SubscriptionNotificationUtility(Me.PageUtility)
        Select Case item.Status
            Case EnrolledStatus.Available
                oServiceUtility.NotifyAddWaitingSubscription(item.IdCommunity, person.Id, person.SurnameAndName)
            Case EnrolledStatus.NeedConfirm
                oServiceUtility.NotifyAddWaitingSubscription(item.IdCommunity, person.Id, person.SurnameAndName)
                NotifyEnrollmentByMail(item, PageUtility.ApplicationUrlBase, Session.SessionID, person, profileType, organizationName)
        End Select
    End Sub
#Region "TO REMOVE WITH NOTIFICATION SYSTEM ACTIVE"
    Public Sub NotifyEnrollmentByMail(item As dtoEnrollment, ByVal link As String, ByVal idSession As String, person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String)
        Dim oLocalizedMail As MailLocalized
        If item.ExtendedInfo.IsValid() Then
            Dim oLingua As Lingua
            If Not IsNothing(item.ExtendedInfo.Language) Then
                oLingua = New Lingua(item.ExtendedInfo.Language.Id, item.ExtendedInfo.Language.Name, item.ExtendedInfo.Language.Code)
                oLingua.isDefault = item.ExtendedInfo.Language.isDefault
            End If
            oLocalizedMail = ManagerConfiguration.GetMailLocalized(oLingua)

            Dim oMail As New COL_E_Mail(oLocalizedMail)

            oMail.IndirizziTO.Add(New MailAddress(item.ExtendedInfo.Responsible.Mail, item.ExtendedInfo.Responsible.SurnameAndName))

            While InStr(idSession, "&") > 0
                idSession = Replace(idSession, "&", "_")
            End While

            COL_BusinessLogic_v2.Comunita.COL_Comunita.Aggiungi_InAttesaConferma(idSession, person.Id, item.IdCommunity)
            If oMail.IndirizziTO.Count > 0 Then
                Dim stringaCriptata As String
                '  Dim oEncrypter As New COL_Encrypter

                'stringaCriptata = oEncrypter.Encrypt("activate&PRSN_ID=" & oPersona.Id & "&CMNT_ID=" & Me.Id)
                stringaCriptata = "activate&" & CriptaParametriAttivazione(person.Id, idSession, item.IdCommunity)

                'PRSN_ID = " & oPersona.Id & " & LimboID = " & SessionId & " & CMNT_ID = " & Me.n_CMNT_id"

                oMail.Oggetto = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ComunitySubscription).Subject
                If Hour(Now) <= 12 Then
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour0).Message
                ElseIf Hour(Now) > 12 And Hour(Now) < 18 Then
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour12).Message
                Else
                    oMail.Body = oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.Hour24).Message
                End If
                oMail.Body = oMail.Body & vbCrLf & oLocalizedMail.NotificationMessages(NotificationMessage.NotificationType.ComunitySubscription).Message
                oMail.Body = Replace(oMail.Body, "#data#", FormatDateTime(Now, DateFormat.LongDate))
                oMail.Body = Replace(oMail.Body, "#username#", person.SurnameAndName)
                oMail.Body = Replace(oMail.Body, "#facolta#", organizationName) 'oOrganizzazione.RagioneSociale)
                oMail.Body = Replace(oMail.Body, "#usermail#", person.Mail)
                oMail.Body = Replace(oMail.Body, "#usertipo#", profileType)
                'If oPersona.TipoPersona.ID = 4 Then 'se è esterno scrivo la mansione
                '    Dim oEsterno As New COL_Esterno
                '    oEsterno.Id = oPersona.ID
                '    oEsterno.GetMansione()
                '    oMail.Body = Replace(oMail.Body, "#mansione#", ", " & oEsterno.Mansione)
                'Else
                oMail.Body = Replace(oMail.Body, "#mansione#", "")
                'End If
                oMail.Body = Replace(oMail.Body, "#comunita#", item.CommunityName)
                oMail.Body = Replace(oMail.Body, "#ruolo#", item.ExtendedInfo.RoleName)
                oMail.Body = Replace(oMail.Body, "#link#", link & "activateFromMail.aspx?action=" & stringaCriptata)

                oMail.Body = oMail.Body & vbCrLf & vbCrLf & vbCrLf & oLocalizedMail.SystemFirmaNotifica

                oMail.Body = oMail.Body.Replace("<br>", vbCrLf)
                oMail.Mittente = oLocalizedMail.SystemSender


                oMail.InviaMail()
            End If
        End If


    End Sub

    Private Function CriptaParametriAttivazione(ByVal PRSN_Id As Integer, ByVal LimboID As String, ByVal CMNT_ID As Integer) As String
        Dim RandomCode As String
        Dim Link As String
        RandomCode = COL_Persona.generaPasswordNumerica(8)

        Link = "AddCode=" & RandomCode & CMNT_ID

        RandomCode = COL_Persona.generaPasswordNumerica(4)
        Link = Link & "&ExpUrl=t" & RandomCode & PRSN_Id

        RandomCode = COL_Persona.generaPasswordNumerica(5)
        Link = Link & "&ExpUrl2=j" & RandomCode & LimboID

        Return Link
    End Function
#End Region
#End Region
    Private Sub DisplayErrorFromDB() Implements IViewCommunitiesTree.DisplayErrorFromDB
        HideItems()
    End Sub
#End Region

#Region "Internal"
    Private Sub LNBapplyTreeFilters_Click(sender As Object, e As EventArgs) Handles LNBapplyTreeFilters.Click
        CurrentPresenter.ApplyFilters(GetSubmittedFilters, AdvancedMode, ReferenceIdCommunity)
    End Sub
    Protected Friend Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        Dim filter As New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed
        If Not Page.IsPostBack Then
            filter.IdOrganization = -1
            filter.IdcommunityType = -1
        End If

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdcommunityType = CInt(Request.Form(item.ToString))
                        Else
                            .IdcommunityType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.coursetime
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdCourseTime = CInt(Request.Form(item.ToString))
                        Else
                            .IdCourseTime = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.degreetype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdDegreeType = CInt(Request.Form(item.ToString))
                        Else
                            .IdDegreeType = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdOrganization = CInt(Request.Form(item.ToString))
                        Else
                            .IdOrganization = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdResponsible = CInt(Request.Form(item.ToString))
                        Else
                            .IdResponsible = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.status
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Status = CInt(Request.Form(item.ToString))
                        Else
                            .Status = lm.Comol.Core.Communities.CommunityStatus.None
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .Year = CInt(Request.Form(item.ToString))
                        Else
                            .Year = -1
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag
                        .IdTags = New List(Of Long)
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            For Each idTag As String In Request.Form(item.ToString).Split(",")
                                .IdTags.Add(CLng(idTag))
                            Next
                        End If
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.name
                        .SearchBy = lm.Comol.Core.BaseModules.CommunityManagement.SearchCommunitiesBy.Contains
                        .Value = Request.Form(item.ToString)
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters
                        .StartWith = ""
                        If Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            Dim charInt As Integer = CInt(Request.Form(item.ToString))
                            Select Case charInt
                                Case -1
                                    .StartWith = ""
                                Case -9
                                    .StartWith = "#"
                                Case Else
                                    .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
                            End Select
                        End If
                End Select
            Next
        End With

        Return filter
    End Function
    Private Sub RPTchildren_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTchildren.ItemCommand
        Dim info As String = e.CommandArgument

        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            DisplaySessionTimeout()
        ElseIf Not String.IsNullOrEmpty(info) AndAlso info.Contains(",") Then
            Dim idCommunity As Integer = CInt(info.Split(",")(0))
            Dim path As String = info.Split(",")(1)
            Dim oResourceConfig As New ResourceManager
            Dim message As String = ""
            Dim mType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Dim oControl As UC_CommunityNode = e.Item.FindControl("CTRLnode")
            Select Case e.CommandName
                Case "enroll"
                    CurrentPresenter.EnrollTo(idCommunity, oControl.Displayname, path, CurrentFilters)
                Case "unsubscribe"
                    CurrentPresenter.UnsubscribeFromCommunity(idCommunity, path, CurrentFilters)
                Case "access"
                    oResourceConfig = GetResourceConfig(Session("LinguaCode"))

                    Dim result As lm.Comol.Core.DomainModel.SubscriptionStatus = PageUtility.AccessToCommunity(PageUtility.CurrentContext.UserContext.CurrentUserID, idCommunity, path, oResourceConfig, True)

                    message = Resource.getValue("Tree.AccessTo.SubscriptionStatus." & result.ToString)
                    If message.Contains("{0}") Then
                        message = String.Format(message, oControl.Displayname)
                    End If

                    Select Case result
                        Case lm.Comol.Core.DomainModel.SubscriptionStatus.blocked, lm.Comol.Core.DomainModel.SubscriptionStatus.communityblocked
                            mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
                    End Select
                    HasDisplayMessage = True
                    If DisplayMessageOnControl Then
                        CTRLmessages.Visible = True
                        CTRLmessages.InitializeControl(message, mType)
                    Else
                        RaiseEvent DisplayMessage(message, mType)
                    End If
            End Select
        End If
    End Sub

    'Private Sub RPTchildren_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles RPTchildren.ItemCreated
    '    Dim oMultiView As MultiView = e.Item.FindControl("MLVnode")
    '    Dim oView As View = e.Item.FindControl("VIWtype" & lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.OpenCommunityNode.ToString)
    '    Dim oLiteral As LiteralControl = oView.Controls(0)
    'End Sub
    Private Sub RPTchildren_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTchildren.ItemDataBound
        Dim item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityNodeItem = e.Item.DataItem
        Dim oMultiView As MultiView = e.Item.FindControl("MLVnode")
        Dim oView As View = e.Item.FindControl("VIWtype" & item.Type.ToString)
        Select Case item.Type
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.OpenCommunityNode, lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.OpenVirtualNode
                Dim oLiteral As Literal = Nothing
                oLiteral = e.Item.FindControl("LTnode" & item.Type.ToString)

                If oLiteral.Text.Contains("{0}") Then
                    If item.HasCurrent Then
                        oLiteral.Text = String.Format(oLiteral.Text, LTtreeKeepAutoOpenCssClass.Text, item.UniqueId.ToString)
                    Else
                        oLiteral.Text = String.Format(oLiteral.Text, "", item.UniqueId.ToString)
                    End If
                End If
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.Community, lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.Virtual
                Dim oControl As UC_CommunityNode = e.Item.FindControl("CTRLnode")
                oControl.InitializeControl(item)
                oView = e.Item.FindControl("VIWtype" & lm.Comol.Core.BaseModules.Dashboard.Domain.NodeType.Community.ToString)
        End Select
        oMultiView.SetActiveView(oView)
    End Sub
    Private Sub CTRLconfirmUnsubscription_UnsubscribeFromCommunity(idCommunity As Integer, path As String, rAction As RemoveAction) Handles CTRLconfirmUnsubscription.UnsubscribeFromCommunity
        RaiseEvent OpenConfirmDialog("")
        CTRLconfirmUnsubscription.Visible = False
        CurrentPresenter.UnsubscribeFromCommunity(idCommunity, path, rAction, CurrentFilters)
    End Sub
    Private Sub CTRLconfirmEnrollToCommunity_EnrollToCommunities(items As List(Of dtoCommunityInfoForEnroll)) Handles CTRLconfirmEnrollToCommunity.EnrollToCommunities
        RaiseEvent OpenConfirmDialog("")
        CTRLconfirmEnrollToCommunity.Visible = False
        If Not IsNothing(items) Then
            CurrentPresenter.EnrollTo(items.FirstOrDefault(), CurrentFilters, AdvancedMode, ReferenceIdCommunity)
        End If
    End Sub
    Private Sub HideItems()
        'LNBviewAll.Visible = False
        'LNBviewLess.Visible = False
        'LNBorderByDown.Visible = False
        'LNBorderByNameDown.Visible = False
        'LNBorderByNameUp.Visible = False
        'LNBorderByUp.Visible = False
        'RPTorderBy.Visible = False
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTchildren.Items)
            Dim oControl As UC_CommunityNode = row.FindControl("CTRLnode")
            oControl.HideItems()
        Next
    End Sub
    Public Function GetCollapsedCssClass() As String
        If Not Page.IsPostBack Then
            Response.SetCookie(New HttpCookie("collapsed-cl-tree-filters-" & ReferenceIdCommunity & "-" & AdvancedMode, "true"))
            Return LTcssClassCollapsed.Text
            'ElseIf Not Page.IsPostBack Then
            '    Response.SetCookie(New HttpCookie("collapsed-cl-tree-filters-" & ReferenceIdCommunity & "-" & AdvancedMode, "true"))
            '    Return LTcssClassCollapsed.Text
        End If
        Return ""
    End Function

    Private Sub InternalDisplayMessage(ByVal message As String, mType As lm.Comol.Core.DomainModel.Helpers.MessageType)
        HasDisplayMessage = True
        If DisplayMessageOnControl Then
            CTRLmessages.Visible = True
            CTRLmessages.InitializeControl(message, mType)
        Else
            RaiseEvent DisplayMessage(message, mType)
        End If
    End Sub
    Private Sub InternalHideDisplayMessage()
        HasDisplayMessage = False
        If DisplayMessageOnControl Then
            CTRLmessages.Visible = False
        Else
            RaiseEvent HideDisplayMessage()
        End If
    End Sub
#End Region

End Class