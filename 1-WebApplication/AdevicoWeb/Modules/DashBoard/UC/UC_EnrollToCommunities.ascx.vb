Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.CommunityManagement
Public Class UC_EnrollToCommunities
    Inherits DBbaseControl
    Implements IViewEnrollToCommunities


#Region "Context"
    Private _Presenter As EnrollToCommunitiesPresenter
    Private ReadOnly Property CurrentPresenter() As EnrollToCommunitiesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EnrollToCommunitiesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IsInitialized As Boolean Implements IViewEnrollToCommunities.IsInitialized
        Get
            Return ViewStateOrDefault("IsInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitialized") = value
        End Set
    End Property
    Public Property TitleCssClass As String Implements IViewEnrollToCommunities.TitleCssClass
        Get
            Return ViewStateOrDefault("TitleCssClass", "")
        End Get
        Set(value As String)
            ViewState("TitleCssClass") = value
        End Set
    End Property
    Public Property TitleImage As String Implements IViewEnrollToCommunities.TitleImage
        Get
            Return ViewStateOrDefault("TitleImage", "")
        End Get
        Set(value As String)
            ViewState("TitleImage") = value
            If Not String.IsNullOrEmpty(value) Then
                IMGtileIcon.Visible = True
                IMGtileIcon.ImageUrl = PageUtility.ApplicationUrlBase & TilesVirtualPath & value
            Else
                IMGtileIcon.Visible = False
            End If
        End Set
    End Property
    Public Property AutoDisplayTitle As String Implements IViewEnrollToCommunities.AutoDisplayTitle
        Get
            Return ViewStateOrDefault("AutoDisplayTitle", "")
        End Get
        Set(value As String)
            ViewState("AutoDisplayTitle") = value
            LTtitle.Text = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewEnrollToCommunities.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", False)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property FirstLoad As Boolean Implements IViewEnrollToCommunities.FirstLoad
        Get
            Return ViewStateOrDefault("FirstLoad", True)
        End Get
        Set(value As Boolean)
            ViewState("FirstLoad") = value
        End Set
    End Property
    Public Property CurrentOrderBy As lm.Comol.Core.Dashboard.Domain.OrderItemsToSubscribeBy Implements IViewEnrollToCommunities.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderItemsToSubscribeBy.Name)
        End Get
        Set(value As lm.Comol.Core.Dashboard.Domain.OrderItemsToSubscribeBy)
            ViewState("CurrentOrderBy") = value

        End Set
    End Property
    Private Property AvailableColumns As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn) Implements IViewEnrollToCommunities.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewEnrollToCommunities.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpager.Visible = (Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize))
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewEnrollToCommunities.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
    Private Property DefaultPageSize As Integer Implements IViewEnrollToCommunities.DefaultPageSize
        Get
            Return ViewStateOrDefault("DefaultPageSize", 25)
        End Get
        Set(value As Integer)
            ViewState("DefaultPageSize") = value
        End Set
    End Property
    Private Property DefaultRange As RangeSettings Implements IViewEnrollToCommunities.DefaultRange
        Get
            If ViewState("DefaultRange") Is Nothing Then
                Return Nothing
            ElseIf TypeOf (ViewState("DefaultRange")) Is RangeSettings Then
                Return ViewState("DefaultRange")
            Else
                Return Nothing
            End If
        End Get
        Set(value As RangeSettings)
            ViewState("DefaultRange") = value
        End Set
    End Property
    Private ReadOnly Property CurrentPageIndex As Integer Implements IViewEnrollToCommunities.CurrentPageIndex
        Get
            Return Pager.PageIndex
        End Get
    End Property
    Private Property IdCurrentCommunityType As Integer Implements IViewEnrollToCommunities.IdCurrentCommunityType
        Get
            Return ViewStateOrDefault("IdCurrentCommunityType", -1)
        End Get
        Set(value As Integer)
            ViewState("IdCurrentCommunityType") = value
        End Set
    End Property
    Private Property CurrentFilters As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters Implements IViewEnrollToCommunities.CurrentFilters
        Get
            Return ViewStateOrDefault("CurrentFilters", New lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters())
        End Get
        Set(value As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
    Private Property CurrentSelectedItems As List(Of dtoCommunityToEnroll) Implements IViewEnrollToCommunities.CurrentSelectedItems
        Get
            Return ViewStateOrDefault("CurrentSelectedItems", New List(Of dtoCommunityToEnroll))
        End Get
        Set(value As List(Of dtoCommunityToEnroll))
            ViewState("CurrentSelectedItems") = value
        End Set
    End Property
    Private Property KeepOpenBulkActions As Boolean Implements IViewEnrollToCommunities.KeepOpenBulkActions
        Get
            Return ViewStateOrDefault("KeepOpenBulkActions", False)
        End Get
        Set(value As Boolean)
            ViewState("KeepOpenBulkActions") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Private _noItemsToEnroll As Boolean
    Public Event SessionTimeout()
    Public Event HideDisplayMessage()
    Public Event DisplayMessage(message As String, type As lm.Comol.Core.DomainModel.Helpers.MessageType)
    Public Event OpenConfirmDialog(ByVal openCssClass As String)
    Public Event SetDefaultFilters(ByVal filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))
    Public Property AllowAutoUpdateCookie As Boolean
        Get
            Return ViewStateOrDefault("AllowAutoUpdateCookie", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowAutoUpdateCookie") = value
        End Set
    End Property
    Public ReadOnly Property ItemsCount As Integer
        Get
            Return 0 '(From row As RepeaterItem In RPTcommunities.Items Where row.ItemType = ListItemType.Item OrElse row.ItemType = ListItemType.AlternatingItem Select row).Count
        End Get
    End Property
    Private ReadOnly Property TilesVirtualPath As String
        Get
            Return SystemSettings.File.Tiles.VirtualPath
        End Get
    End Property
    Public Property HasDisplayMessage As Boolean
        Get
            Return ViewStateOrDefault("HasDisplayMessage", False)
        End Get
        Set(value As Boolean)
            ViewState("HasDisplayMessage") = value
        End Set
    End Property
    Public Property IsPreview As Boolean
        Get
            Return ViewStateOrDefault("IsPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPreview") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBspanExpandList)
            .setLabel(LBspanCollapseList)
            .setLiteral(LTsearchFiltersTitle)
            .setLinkButton(LNBapplySearchFilters, False, True)
            .setLabel(LBorderBySelectorDescription)

            .setLabel(LBenrollingTableLegend)
            .setLabel(LBlengendItemEnrollNotAvailable)
            .setLabel(LBlengendReason)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(itemsForPage As Integer, range As RangeSettings, preloadIdCommunityType As Integer, searchText As String, preloadList As Boolean) Implements IViewEnrollToCommunities.InitializeControl
        CurrentPresenter.InitView(itemsForPage, range, preloadIdCommunityType, searchText, preloadList)
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewEnrollToCommunities.DisplaySessionTimeout
        HideItems()
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub SetListTitle(name As String, tile As liteTile) Implements IViewEnrollToCommunities.SetListTitle
        Dim displayName As String = name
        If Not IsNothing(tile) Then 'AndAlso (Not String.IsNullOrEmpty(tile.ImageCssClass) OrElse Not String.IsNullOrEmpty(tile.ImageUrl)) Then
            If Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                TitleCssClass = tile.ImageCssClass
            ElseIf Not String.IsNullOrEmpty(tile.ImageCssClass) Then
                TitleImage = tile.ImageCssClass
            Else
                TitleCssClass = LTcssClassTitle.Text
            End If
            If String.IsNullOrEmpty(displayName) Then
                displayName = Resource.getValue("SearchAutoDisplayTitle.Communities")
            End If
        Else
            displayName = Resource.getValue("SearchAutoDisplayTitle.Communities")
        End If
        AutoDisplayTitle = displayName
    End Sub

#Region "Load Items"
    Private Sub DisplayNoCommunitiesToEnroll(name As String) Implements IViewEnrollToCommunities.DisplayNoCommunitiesToEnroll
        _noItemsToEnroll = True
        DisplayFilters(False)
        InitializeRepeater(New List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem))

        Dim oLabel As Label = RPTcommunities.Controls(RPTcommunities.Controls.Count - 1).Controls(0).FindControl("LBemptyItems")
        If oLabel.Text.Contains("{0}") Then
            oLabel.Text = String.Format(oLabel.Text, name)
        End If
    End Sub
    Private Sub LoadDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter)) Implements IViewEnrollToCommunities.LoadDefaultFilters
        RaiseEvent SetDefaultFilters(filters)
        DVfilters.Visible = filters.Any()
    End Sub
    Private Sub LoadItems(items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem), orderby As OrderItemsToSubscribeBy, ascending As Boolean) Implements IViewEnrollToCommunities.LoadItems
        CurrentAscending = ascending
        CurrentOrderBy = orderby
        RPTorderBy.Visible = (items.Count > 1)
       
        InitializeRepeater(items)
    End Sub
    Private Sub InitializeRepeater(items As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem))
        RPTcommunities.DataSource = items
        RPTcommunities.DataBind()
        SPNlegend.Visible = items.Any()
        Dim columns As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn) = AvailableColumns

        For Each c As lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
            Dim oCell As HtmlControl = RPTcommunities.Controls(0).Controls(0).FindControl("TH" & c.ToString.ToLower)
            If Not IsNothing(oCell) Then
                oCell.Visible = columns.Contains(c)
                Dim oLabel As Label = oCell.FindControl("LBth" & c.ToString.ToLower)
                If Not IsNothing(oLabel) Then
                    If oLabel.Text.StartsWith("*") Then
                        Resource.setLabel(oLabel)
                    End If
                End If
            End If
        Next
        If columns.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.select) Then
            Dim oCheck As HtmlInputCheckBox = RPTcommunities.Controls(0).Controls(0).FindControl("CBXselectAll")
            If Not IsNothing(oCheck) Then
                oCheck.Visible = (items.Where(Function(i) i.AllowSubscribe).Count > 1)
            End If

            If items.Where(Function(i) i.AllowSubscribe).Count > 1 Then
                CTRLbulkActionTop.Visible = True
                If items.Where(Function(i) i.AllowSubscribe).Count > 15 Then
                    CTRLbulkActionBottom.Visible = True
                End If
            End If
        End If
    End Sub
    Private Sub DisplayFilters(show As Boolean) Implements IViewEnrollToCommunities.DisplayFilters
        DVfilters.Visible = show
    End Sub
    Private Sub DisplayErrorFromDB() Implements IViewEnrollToCommunities.DisplayErrorFromDB
        HideItems()
    End Sub
    Private Sub InitializeOrderBySelector(items As List(Of dtoItemFilter(Of OrderItemsToSubscribeBy))) Implements IViewEnrollToCommunities.InitializeOrderBySelector
        If Not IsNothing(items) AndAlso items.Count > 1 Then
            DVorderBySelector.Visible = True
            RPTorderBy.DataSource = items
            RPTorderBy.DataBind()
            LBorderBySelected.Text = Resource.getValue("OrderItemsToSubscribeBy." & items.Where(Function(i) i.Selected).FirstOrDefault.Value.ToString)
        Else
            DVorderBySelector.Visible = False
        End If
    End Sub
#End Region

#Region "Actions"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewEnrollToCommunities.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idActionCommunity As Integer, action As ModuleDashboard.ActionType) Implements IViewEnrollToCommunities.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Community, idActionCommunity), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCommunities As List(Of Integer), action As ModuleDashboard.ActionType) Implements IViewEnrollToCommunities.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Community, idCommunities), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Messages"
    Private Sub DisplayUnknownCommunity(name As String) Implements IViewEnrollToCommunities.DisplayUnknownCommunity
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayUnknownCommunity"), name), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayEnrollMessage(item As dtoCommunityInfoForEnroll, action As ModuleDashboard.ActionType) Implements IViewEnrollToCommunities.DisplayEnrollMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Select Case action
            Case ModuleDashboard.ActionType.EnrollNotAllowed
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case ModuleDashboard.ActionType.UnableToEnroll
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType.EnrollToCommunity
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        CTRLmessages.Visible = True
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
        CTRLmessages.InitializeControl(message, t)
    End Sub
    Private Sub DisplayConfirmMessage(item As dtoCommunityInfoForEnroll) Implements IViewEnrollToCommunities.DisplayConfirmMessage
        RaiseEvent OpenConfirmDialog(CTRLconfirmEnrollToCommunity.DialogIdentifier)
        '   DVconfirmUnsubscription.Visible = True
        CTRLconfirmEnrollToCommunity.Visible = True
        CTRLconfirmEnrollToCommunity.InitializeControl(item, String.Format(Resource.getValue("ConfirmEnrollTo.Description"), item.Name))
    End Sub
    Private Sub DisplayEnrollMessage(item As dtoEnrollment, idCommunity As Integer, person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String) Implements IViewEnrollToCommunities.DisplayEnrollMessage
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

        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.EnrolledStatus." & item.Status.ToString), item.CommunityName), t)
    End Sub
    Private Sub DisplayEnrollMessage(number As Integer, action As ModuleDashboard.ActionType) Implements IViewEnrollToCommunities.DisplayEnrollMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        Select Case action
            Case ModuleDashboard.ActionType.EnrollNotAllowed
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case ModuleDashboard.ActionType.UnableToEnroll
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType.EnrollToCommunity
                t = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        CTRLmessages.Visible = True

        CTRLmessages.InitializeControl(String.Format(Resource.getValue("IViewEnrollToCommunities.DisplayEnrollMessage.n.ActionType." & action.ToString), number), t)
    End Sub
    Private Sub DisplayEnrollMessage(enrolledItems As List(Of dtoEnrollment), Optional ByVal notEnrolledCommunities As List(Of String) = Nothing) Implements IViewEnrollToCommunities.DisplayEnrollMessage
        Dim t As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        If enrolledItems.Where(Function(e) e.Status <> EnrolledStatus.PreviousEnrolled AndAlso e.Status <> EnrolledStatus.Available).Any() Then
            t = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End If
        CTRLmessages.Visible = True
        Dim messages As List(Of String) = Nothing
        If IsNothing(notEnrolledCommunities) Then
            messages = enrolledItems.GroupBy(Function(e) e.Status).Select(Function(e) GetMessage(e.Key, e.ToList())).ToList()
        Else
            Dim dict As Dictionary(Of EnrolledStatus, List(Of String)) = enrolledItems.GroupBy(Function(e) e.Status).ToDictionary(Function(e) e.Key, Function(e) e.Select(Function(s) s.CommunityName).ToList())
            messages = dict.Select(Function(e) GetMessage(e.Key, e.Value)).ToList()
        End If

        If messages.Count = 1 Then
            CTRLmessages.InitializeControl(messages.FirstOrDefault(), t)
        Else
            CTRLmessages.InitializeControl(String.Format(LTtemplateMessageDetails.Text, String.Join(" ", messages.Select(Function(m) String.Format(LTtemplateMessageDetail.Text, m)).ToArray())), t)
        End If
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
    Private Sub DisplayConfirmMessage(enrolledItems As List(Of dtoEnrollment), notEnrolledItems As List(Of dtoEnrollment), enrollingCommunities As List(Of dtoCommunityInfoForEnroll), person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String) Implements IViewEnrollToCommunities.DisplayConfirmMessage
        RaiseEvent OpenConfirmDialog(CTRLconfirmEnrollToCommunity.DialogIdentifier)
        '   DVconfirmUnsubscription.Visible = True

        If (enrolledItems.Any()) Then
            For Each item As dtoEnrollment In enrolledItems.Where(Function(e) e.Status = EnrolledStatus.Available OrElse e.Status = EnrolledStatus.NeedConfirm)
                NotifyEnrollment(item, person, profileType, organizationName)
            Next
        End If

        CTRLconfirmEnrollToCommunity.Visible = True
        CTRLconfirmEnrollToCommunity.InitializeControl(enrolledItems, notEnrolledItems, enrollingCommunities)
    End Sub
    Private Sub NotifyEnrollment(item As dtoEnrollment, person As lm.Comol.Core.DomainModel.litePerson, profileType As String, organizationName As String) Implements IViewEnrollToCommunities.NotifyEnrollment
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

    Private Sub DeselectAll() Implements IViewEnrollToCommunities.DeselectAll
        If RPTcommunities.HasControls Then
            Dim oCheck As HtmlInputCheckBox = RPTcommunities.Controls(0).Controls(0).FindControl("CBXselectAll")
            If Not IsNothing(oCheck) Then
                oCheck.Checked = False
            End If
        End If
    End Sub
    Private Sub RemoveFromSelectedItems(idCommunities As List(Of Integer)) Implements IViewEnrollToCommunities.RemoveFromSelectedItems
        For Each row As RepeaterItem In RPTcommunities.Items
            Dim oLiteral As Literal = row.FindControl("LTidCommunity")
            If idCommunities.Contains(CInt(oLiteral.Text)) Then
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBselect")
                oCheck.Checked = False
            End If
        Next
    End Sub
    Private Sub InitializeBulkActions(hasMultiPage As Boolean, items As List(Of dtoCommunityToEnroll)) Implements IViewEnrollToCommunities.InitializeBulkActions
        CTRLbulkActionTop.InitializeControl(False, items)
        CTRLbulkActionBottom.InitializeControl(False, items)
    End Sub
    Private Function GetSelectedItems() As List(Of dtoCommunityToEnroll) Implements IViewEnrollToCommunities.GetSelectedItems
        Dim result As New List(Of dtoCommunityToEnroll)

        For Each row As RepeaterItem In RPTcommunities.Items
            Dim oLiteral As Literal = row.FindControl("LTidCommunity")
            Dim item As New dtoCommunityToEnroll
            item.Id = CInt(oLiteral.Text)
            item.Path = DirectCast(row.FindControl("LTpath"), Literal).Text
            item.PageIndex = Pager.PageIndex + 1
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBselect")
            item.Selected = oCheck.Checked AndAlso Not oCheck.Disabled
            result.Add(item)
        Next
        Return result
    End Function
    Private Sub HideItems()
        'LNBviewAll.Visible = False
        'LNBviewLess.Visible = False
        'LNBorderByDown.Visible = False
        'LNBorderByNameDown.Visible = False
        'LNBorderByNameUp.Visible = False
        'LNBorderByUp.Visible = False
        DVorderBySelector.Visible = False
        For Each row As RepeaterItem In (From r As RepeaterItem In RPTcommunities.Items)
            Dim oLinkbutton As LinkButton = row.FindControl("LNBsubscribe")
            oLinkbutton.Visible = False
        Next
        For Each c As OrderItemsToSubscribeBy In [Enum].GetValues(GetType(OrderItemsToSubscribeBy))
            Dim oLinkButton As LinkButton = RPTcommunities.Controls(0).Controls(0).FindControl("LNBorderBy" & c.ToString & "Up")
            If Not IsNothing(oLinkButton) Then
                oLinkButton.Visible = False
            End If
            oLinkButton = RPTcommunities.Controls(0).Controls(0).FindControl("LNBorderBy" & c.ToString & "Down")
            If Not IsNothing(oLinkButton) Then
                oLinkButton.Visible = False
            End If
        Next
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTorderBy_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTorderBy.ItemDataBound
        Dim oLinkButton As LinkButton = e.Item.FindControl("LNBorderItemsBy")
        Dim oItem As dtoItemFilter(Of OrderItemsToSubscribeBy) = e.Item.DataItem

        oLinkButton.CommandArgument = CInt(oItem.Value)
        oLinkButton.Text = String.Format(LTorderItemsByTemplate.Text, Resource.getValue("OrderItemsToSubscribeBy." & oItem.Value.ToString))

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        oControl.Attributes("class") = LTcssClassOrderBy.Text & " " & GetOrderByItemCssClass(oItem)
    End Sub
    Private Sub RPTorderBy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTorderBy.ItemCommand
        CurrentOrderBy = CInt(e.CommandArgument)
        Select Case CInt(e.CommandArgument)
            Case OrderItemsToSubscribeBy.MaxUsers, OrderItemsToSubscribeBy.Year
                CurrentAscending = False
            Case Else
                CurrentAscending = True

        End Select
        LBorderBySelected.Text = Resource.getValue("OrderItemsToSubscribeBy." & CurrentOrderBy.ToString)

        Dim oControl As HtmlControl = e.Item.FindControl("DVitemOrderBy")
        If Not oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
            oControl.Attributes("class") = oControl.Attributes("class") & " " & LTcssClassActive.Text

            For Each row As RepeaterItem In (From r As RepeaterItem In RPTorderBy.Items Where r.ItemIndex <> e.Item.ItemIndex)
                oControl = row.FindControl("DVitemOrderBy")
                If oControl.Attributes("class").Contains(LTcssClassActive.Text) Then
                    oControl.Attributes("class") = Replace(oControl.Attributes("class"), LTcssClassActive.Text, "")
                    Exit For
                End If
            Next
        End If
        CurrentPresenter.LoadCommunities(DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, 0)
    End Sub
    Private Sub RPTcommunities_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTcommunities.ItemCommand
        '    Dim oResourceConfig As New ResourceManager
        Dim arguments As String = e.CommandArgument
        If HasDisplayMessage Then
            RaiseEvent HideDisplayMessage()
            HasDisplayMessage = False
        End If

        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            RaiseEvent SessionTimeout()
        Else
            Select Case e.CommandName
                Case "enroll"
                    Dim cValues As String() = arguments.Split(",")
                    CurrentPresenter.EnrollTo(cValues(0), DirectCast(e.Item.FindControl("LBcommunityName"), Label).Text, cValues(1), DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, Pager.PageIndex)
                Case "orderby"
                    Dim values As String() = arguments.Split(".")
                    CurrentPresenter.LoadCommunities(DefaultPageSize, DefaultRange, CurrentFilters, lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderItemsToSubscribeBy).GetByString(values(0), OrderItemsToSubscribeBy.Name), values(1) = Boolean.TrueString, 0)
            End Select
        End If
    End Sub
    Private Sub RPTcommunities_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTcommunities.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem = e.Item.DataItem

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPcommunityInfo")
                oHyperlink.ToolTip = Resource.getValue("HYPcommunityInfo.ToolTip." & HasGenericConstraints(dto))
                'Resource.
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase() & lm.Comol.Core.Dashboard.Domain.RootObject.CommunityDetails(dto.Community.Id, True)
                Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBselect")
                If Not IsNothing(oCheck) Then
                    oCheck.Visible = dto.AllowSubscribe
                    If dto.AllowSubscribe Then
                        oCheck.Checked = CurrentSelectedItems.Where(Function(a) a.Id.Equals(dto.Community.Id) AndAlso a.Selected).Any
                    Else
                        oCheck.Checked = False
                    End If
                End If

                Dim oLabel As Label = Nothing

                Dim oControl As HtmlControl = e.Item.FindControl("DVtag")
                oControl.Visible = dto.Community.Tags.Any()
                If dto.Community.Tags.Any() Then
                    oLabel = e.Item.FindControl("LBcommunityTagsTitle")
                    Resource.setLabel(oLabel)
                End If
                If dto.Community.IdType = lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization Then
                    oLabel = e.Item.FindControl("LBtagCommunityType")
                    oLabel.Visible = True
                    oLabel.Text = Resource.getValue("LBtagCommunityType." & lm.Comol.Core.DomainModel.CommunityTypeStandard.Organization.ToString)
                End If

                Dim oCell As HtmlTableCell = Nothing
                Dim columns As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn) = AvailableColumns

                For Each c As lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn))
                    oCell = e.Item.FindControl("TD" & c.ToString.ToLower)
                    If Not IsNothing(oCell) Then
                        oCell.Visible = columns.Contains(c)
                        If columns.Contains(c) Then
                            Select Case c
                                Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.maxsubscribers
                                    oLabel = oCell.FindControl("LBmaxsubscribers")
                                    oLabel.Text = dto.PrintAvailableSeats("/")
                                Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.endsubscriptionon
                                    oLabel = oCell.FindControl("LBendDate")
                                    oLabel.Text = GetDateTimeString(dto.Community.SubscriptionEndOn, " ", True)
                                Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.startsubscriptionon
                                    oLabel = oCell.FindControl("LBstartDate")
                                    oLabel.Text = GetDateTimeString(dto.Community.SubscriptionStartOn, " ", True)
                            End Select
                        End If
                    End If
                Next

                oCell = e.Item.FindControl("TDactions")
                If AvailableColumns.Contains(lm.Comol.Core.BaseModules.Dashboard.Domain.searchColumn.actions) Then
                    oCell.Visible = True
                    Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBsubscribe")
                    If dto.AllowSubscribe Then
                        oLinkbutton.Visible = True
                        'oLinkbutton.Enabled = Not IsPreview
                        oLinkbutton.CommandArgument = dto.Community.Id & "," & dto.Community.Path
                        Resource.setLinkButton(oLinkbutton, False, True)
                        If oLinkbutton.ToolTip.Contains("{0}") Then
                            oLinkbutton.ToolTip = String.Format(oLinkbutton.Text, dto.Community.Name)
                        End If
                    Else
                        oLinkbutton.Visible = False
                    End If
                Else
                    oCell.Visible = False
                End If
            Case ListItemType.Header

                For Each c As OrderItemsToSubscribeBy In [Enum].GetValues(GetType(OrderItemsToSubscribeBy))
                    Dim oLinkButton As LinkButton = e.Item.FindControl("LNBorderBy" & c.ToString & "Up")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.ToolTip = Resource.getValue("OrderItemsToSubscribeBy." & c.ToString & ".True")
                        oLinkButton.Visible = (Not CurrentAscending AndAlso CurrentOrderBy = c)
                    End If
                    oLinkButton = e.Item.FindControl("LNBorderBy" & c.ToString & "Down")
                    If Not IsNothing(oLinkButton) Then
                        oLinkButton.ToolTip = Resource.getValue("OrderItemsToSubscribeBy." & c.ToString & ".False")
                        oLinkButton.Visible = (CurrentAscending AndAlso CurrentOrderBy = c)
                    End If
                Next
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTcommunities.Items.Count = 0)
                If (RPTcommunities.Items.Count = 0) Then
                    Dim oTableCell As HtmlTableCell = e.Item.FindControl("TDemptyItems")

                    oTableCell.ColSpan = AvailableColumns.Count
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                    If _noItemsToEnroll Then
                        oLabel.Text = Resource.getValue("NoCommunities.DashboardViewType." & DashboardViewType.Subscribe.ToString & ".DisplayNoCommunitiesToEnroll")
                        _noItemsToEnroll = False
                    Else
                        oLabel.Text = Resource.getValue("NoCommunities.DashboardViewType." & DashboardViewType.Subscribe.ToString & "." & FirstLoad.ToString)
                    End If
                End If
        End Select
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Dim pagesize As Integer = CurrentPageSize

        If HasDisplayMessage Then
            RaiseEvent HideDisplayMessage()
            HasDisplayMessage = False
        End If
        CurrentPresenter.LoadCommunities(DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, Me.Pager.PageIndex)
    End Sub
    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        CurrentAscending = CBool(DirectCast(sender, LinkButton).CommandArgument)
        If HasDisplayMessage Then
            RaiseEvent HideDisplayMessage()
            HasDisplayMessage = False
        End If
        CurrentPresenter.LoadCommunities(DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, 0)
    End Sub
    Private Sub LNBapplySearchFilters_Click(sender As Object, e As EventArgs) Handles LNBapplySearchFilters.Click
        CTRLmessages.Visible = False
        CurrentPresenter.ApplyFilters(DefaultPageSize, DefaultRange, GetSubmittedFilters, CurrentOrderBy)
    End Sub
    Protected Friend Function GetSubmittedFilters() As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters
        Dim filter As lm.Comol.Core.BaseModules.CommunityManagement.dtoCommunitiesFilters = CurrentFilters
        'filter.Availability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.NotSubscribed
        'If Not Page.IsPostBack Then
        '    filter.IdOrganization = -1
        '    filter.IdcommunityType = -1
        'End If

        With filter
            Dim keys As List(Of String) = Request.Form.AllKeys.ToList()
            For Each item As lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType In [Enum].GetValues(GetType(lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType))
                Select Case item
                    Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype
                        If keys.Contains(item.ToString) AndAlso Not String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .IdcommunityType = CInt(Request.Form(item.ToString))
                        Else
                            .IdcommunityType = IdCurrentCommunityType
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
                        If String.IsNullOrEmpty(Request.Form(item.ToString)) Then
                            .StartWith = ""
                        Else
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
    Private Sub CTRLbulkActionTop_EnrollToBulkActions(applyToAll As Boolean) Handles CTRLbulkActionTop.EnrollToBulkActions, CTRLbulkActionBottom.EnrollToBulkActions
        Dim idCommunities As List(Of Integer)
        If Not applyToAll Then
            idCommunities = GetSelectedCommunities()
        End If
        CurrentPresenter.EnrollTo(GetSelectedItems(), True, DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, Pager.PageIndex)
    End Sub
    Private Sub CTRLconfirmEnrollToCommunity_EnrollToCommunities(items As List(Of dtoCommunityInfoForEnroll)) Handles CTRLconfirmEnrollToCommunity.EnrollToCommunities
        RaiseEvent OpenConfirmDialog("")
        CTRLconfirmEnrollToCommunity.Visible = False
        CurrentPresenter.EnrollTo(items, DefaultPageSize, DefaultRange, CurrentFilters, CurrentOrderBy, CurrentAscending, Pager.PageIndex)
    End Sub
    Private Function GetSelectedCommunities() As List(Of Integer)
        Return GetSelectedItems.Where(Function(c) c.Selected).Select(Function(c) c.Id).ToList()
    End Function

#Region "Styles"
    Public Function KeepOpenCssClass() As String
        If KeepOpenBulkActions Then
            Return LTcssClassKeepOpen.Text & " " & LTcssClassBulkOn.Text
        Else
            Return LTcssClassBulkOff.Text
        End If
    End Function
    Public Function GetCollapsedCssClass() As String
        If Not Page.IsPostBack Then
            Response.SetCookie(New HttpCookie("cl-subscribefilters", "true"))
            Return LTcssClassCollapsed.Text
        End If
        Return ""
    End Function


    Public Function GetTitleCssClass() As String
        Dim cssClass As String = TitleCssClass
        If String.IsNullOrEmpty(cssClass) AndAlso String.IsNullOrEmpty(TitleImage) Then
            cssClass = " " & LTcssClassTileIcon.Text & " " & LTcssClassDefaultItemClass.Text
        ElseIf Not String.IsNullOrEmpty(cssClass) Then
            cssClass = " " & cssClass & " " & LTcssClassTileIcon.Text
        ElseIf Not String.IsNullOrEmpty(TitleImage) Then
            cssClass &= " " & LTcssClassCustomTile.Text
        End If
        Return cssClass
    End Function
    Public Function GetOrderByItemCssClass(ByVal item As dtoItemFilter(Of OrderItemsToSubscribeBy)) As String
        Dim cssClass As String = GetItemCssClass(item.DisplayAs)
        If item.Selected Then
            cssClass &= " " & LTcssClassActive.Text
        End If
        Return cssClass
    End Function
    Private Function GetItemCssClass(ByVal d As lm.Comol.Core.DomainModel.ItemDisplayOrder) As String
        Dim cssClass As String = ""
        Select Case d
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.first, lm.Comol.Core.DomainModel.ItemDisplayOrder.last
                cssClass = " " & d.ToString
            Case lm.Comol.Core.DomainModel.ItemDisplayOrder.item
                cssClass = ""
            Case Else
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString() & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString()
        End Select
        Return cssClass
    End Function
    Public Function GetCommunityCssClass(ByVal item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem) As String
        Dim cssClass As String = ""
        If HasGenericConstraints(item) Then
            cssClass &= LTcssClassConstraints.Text
        End If
        If Not item.AllowSubscribe Then
            cssClass &= " " & LTcssClassEnrollUnavailable.Text
        End If
        If item.NotAvailableFor.Any Then
            If item.NotAvailableFor.Contains(EnrollingStatus.EndDate) Then
                cssClass &= " " & LTcssClassEndDate.Text
            End If
            If item.NotAvailableFor.Contains(EnrollingStatus.StartDate) Then
                cssClass &= " " & LTcssClassStartDate.Text
            End If
            If item.NotAvailableFor.Contains(EnrollingStatus.Seats) Then
                cssClass &= " " & LTcssClassSeats.Text
            End If
        End If

        If Not String.IsNullOrEmpty(cssClass) Then
            cssClass = cssClass.Trim
        End If
        Return cssClass
    End Function
#End Region
    Public Function GetCommunityTitle(ByVal item As lm.Comol.Core.BaseModules.Dashboard.Domain.dtoEnrollingItem) As String
        Dim title As String = ""
        If Not item.AllowSubscribe Then
            Dim status As List(Of EnrollingStatus) = item.NotAvailableFor
            If status.Contains(EnrollingStatus.StartDate) AndAlso status.Contains(EnrollingStatus.EndDate) Then
                status.Remove(EnrollingStatus.StartDate)
            End If
            If status.Count = 1 Then
                title = Resource.getValue("NotAvailableFor.EnrollingStatus." & status.FirstOrDefault().ToString)
                Select Case status.FirstOrDefault()
                    Case EnrollingStatus.StartDate
                        title = String.Format(title, GetDateTimeString(item.Community.SubscriptionStartOn, "", True))
                    Case EnrollingStatus.EndDate
                        title = String.Format(title, GetDateTimeString(item.Community.SubscriptionEndOn, "", True))
                End Select
            Else
                title = String.Format(Resource.getValue("NotAvailableFor.EnrollingStatus.n"), String.Join(", ", status.Select(Function(i) Resource.getValue("NotAvailableFor.EnrollingStatus.n." & i.ToString)).ToArray))
            End If
        End If
        Return title
    End Function
#End Region

End Class