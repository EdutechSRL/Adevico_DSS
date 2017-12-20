Imports lm.Comol.Core.BaseModules.MailSender
Imports lm.Comol.Core.BaseModules.MailSender.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract

Public Class UserMessages
    Inherits PageBase
    Implements lm.Comol.Core.BaseModules.MailSender.Presentation.IViewUserMessages

#Region "Context"
    Private _Presenter As UserMessagesPresenter
    Private ReadOnly Property CurrentPresenter() As UserMessagesPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UserMessagesPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadModuleObject As ModuleObject Implements IViewUserMessages.PreloadModuleObject
        Get
            Dim item As New ModuleObject()

            If Not String.IsNullOrEmpty(Request.QueryString("oId")) AndAlso IsNumeric(Request.QueryString("oId")) Then
                item.ObjectLongID = CLng(Request.QueryString("oId"))
            End If
            If item.ObjectLongID > 0 Then
                If Not String.IsNullOrEmpty(Request.QueryString("oType")) AndAlso IsNumeric(Request.QueryString("oType")) Then
                    item.ObjectTypeID = CInt(Request.QueryString("oType"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oIdModule")) AndAlso IsNumeric(Request.QueryString("oIdModule")) Then
                    item.ServiceID = CInt(Request.QueryString("oIdModule"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oCommunity")) AndAlso IsNumeric(Request.QueryString("oCommunity")) Then
                    item.CommunityID = CInt(Request.QueryString("oCommunity"))
                End If
                item.ServiceCode = Request.QueryString("oMcode")
                Return item
            Else
                Return Nothing
            End If

        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewUserMessages.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                Return CLng(Request.QueryString("idCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadModuleCode As String Implements IViewUserMessages.PreloadModuleCode
        Get
            Return Request.QueryString("code")
        End Get
    End Property
    Private ReadOnly Property PreloadIdModule As Integer Implements IViewUserMessages.PreloadIdModule
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdPerson As Integer Implements IViewUserMessages.PreloadIdPerson
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idP")) AndAlso IsNumeric(Request.QueryString("idP")) Then
                Return CInt(Request.QueryString("idP"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdUserModule As Long Implements IViewUserMessages.PreloadIdUserModule
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idU")) AndAlso IsNumeric(Request.QueryString("idU")) Then
                Return CLng(Request.QueryString("idU"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadMail As String Implements IViewUserMessages.PreloadMail
        Get
            Return Request.QueryString("mail")
        End Get
    End Property
#End Region

#Region "Current"
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewUserMessages.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Private Property ContainerContext As lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext Implements IViewUserMessages.ContainerContext
        Get
            Return ViewStateOrDefault("ContainerContext", New lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext())
        End Get
        Set(value As lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext)
            ViewState("ContainerContext") = value
        End Set
    End Property
    Private Property CurrentModuleCode As Integer Implements IViewUserMessages.CurrentPageSize
        Get
            Return ViewStateOrDefault("CurrentPageSize", 50)
        End Get
        Set(value As Integer)
            ViewState("CurrentPageSize") = value
        End Set
    End Property
#End Region

    Private ReadOnly Property RemovedUserName As String Implements IViewUserMessages.RemovedUserName
        Get
            Return Resource.getValue("RemovedUserName")
        End Get
    End Property
    Private ReadOnly Property UnknownUserName As String Implements IViewUserMessages.UnknownUserName
        Get
            Return Resource.getValue("UnknownUserName")
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    'Private _SysParameter As lm.Comol.Modules.Standard.WebConferencing.Domain.WbSystemParameter
    'Private ReadOnly Property SysParameter As lm.Comol.Modules.Standard.WebConferencing.Domain.WbSystemParameter
    '    Get
    '        If IsNothing(_SysParameter) Then
    '            _SysParameter = EWtmpHelper.GetCurrentParameters(MyBase.SystemSettings.WebConferencingSettings)
    '        End If

    '        Return _SysParameter
    '    End Get
    'End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Messages", "Modules", "Mail")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTmessageSentSubject_t)
            .setLiteral(LTmessageSentAction_t)
            .setLiteral(LTmessageSentOn_t)
            .setLiteral(LTmessageSentBy_t)
            Master.ServiceTitle = .getValue("serviceTitle.ModuleUserMessages")
            Master.ServiceTitleToolTip = .getValue("serviceTitle.ModuleUserMessages")
            Master.ServiceNopermission = .getValue("nopermission")
        End With
    End Sub
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewModuleMessages.DisplaySessionTimeout
        Dim cContext As lm.Comol.Core.Mail.Messages.dtoModuleMessagesContext = ContainerContext
        Dim idCommunity As Integer = cContext.IdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Core.Mail.Messages.RootObject.ViewMessage(cContext.IdPerson, cContext.ModuleCode, cContext.IdCommunity, cContext.IdModule, cContext.ModuleObject)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer, moduleCode As String) Implements IViewModuleMessages.DisplayNoPermission
        Dim idAction As Integer = CInt(lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission)
        Select Case moduleCode
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                idAction = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                idAction = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                idAction = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType.NoPermission
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                idAction = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.ActionType.NoPermission
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                idAction = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.ActionType.NoPermission
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                idAction = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType.NoPermission
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                idAction = COL_Questionario.ModuleQuestionnaire.ActionType.NoPermission
        End Select
        Me.PageUtility.AddActionToModule(idCommunity, idModule, idAction, , InteractionType.UserWithUser)
        Me.BindNoPermessi()
    End Sub

    Private Function HasModulePermissions(moduleCode As String, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewModuleMessages.HasModulePermissions
        Dim result As Boolean = False
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
                End If
                result = wModule.ManageRoom
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement(permissions)
                End If
                result = wModule.Administration
                If Not IsNothing(obj) Then

                End If
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Dim wModule As lm.Comol.Core.BaseModules.Tickets.ModuleTicket
                If idCommunity = 0 Then
                    wModule = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Core.BaseModules.Tickets.ModuleTicket(permissions)
                End If
                result = wModule.Administration
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim cModule As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper
                If idCommunity = 0 Then
                    cModule = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.CreatePortalmodule(profileType)
                Else
                    cModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper(permissions)
                End If
                result = cModule.Administration OrElse cModule.ManageCallForPapers
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim rModule As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership
                If idCommunity = 0 Then
                    rModule = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.CreatePortalmodule(profileType)
                Else
                    rModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership(permissions)
                End If
                result = rModule.Administration OrElse rModule.ManageBaseForPapers
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                Dim epModule As lm.Comol.Modules.EduPath.Domain.ModuleEduPath
                If idCommunity = 0 Then
                    epModule = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.CreatePortalmodule(profileType)
                Else
                    epModule = New lm.Comol.Modules.EduPath.Domain.ModuleEduPath(permissions)
                End If
                result = epModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                Dim qModule As COL_Questionario.ModuleQuestionnaire
                If idCommunity = 0 Then
                    qModule = COL_Questionario.ModuleQuestionnaire.CreatePortalmodule(profileType)
                Else
                    qModule = New COL_Questionario.ModuleQuestionnaire(permissions)
                End If
                result = qModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
        End Select
        Return result
    End Function
    Private Sub DisplayNoMessagesFound() Implements IViewUserMessages.DisplayNoMessagesFound
        Me.DVpreview.Visible = False
        RPTmessages.DataSource = New List(Of lm.Comol.Core.Mail.Messages.dtoDisplayUserMessage)
        RPTmessages.DataBind()
    End Sub
    Private Sub LoadItems(items As List(Of lm.Comol.Core.Mail.Messages.dtoDisplayUserMessage)) Implements IViewUserMessages.LoadItems
        Me.DVpreview.Visible = False
        RPTmessages.DataSource = items
        RPTmessages.DataBind()
    End Sub
    Private Function GetRecipient(moduleCode As String, idUserModule As Long) As lm.Comol.Core.Mail.dtoRecipient Implements IViewUserMessages.GetRecipient
        Select Case moduleCode
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Return Nothing
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Return Nothing
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Dim s As New lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(PageUtility.CurrentContext)
                Return s.GetUserRecipient(idUserModule)
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Dim s As New lm.Comol.Core.BaseModules.Tickets.TicketService(PageUtility.CurrentContext)
                Return s.UserGetRecipient(idUserModule)
                'Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                '    Dim s As New lm.Comol.Modules.Standard.WebConferencing.Domain.WbServiceGeneric(SysParameter, PageUtility.CurrentContext)
                Return s.UserGetRecipient(idUserModule)
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                Return Nothing
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                Dim qService As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
                Return qService.GetUserRecipient(idUserModule)
            Case Else
                Return Nothing
        End Select
    End Function
    Private Sub LoadRecipientName(recipient As lm.Comol.Core.Mail.dtoRecipient) Implements IViewUserMessages.LoadRecipientName
        LTuserInfo.Text = recipient.DisplayName & IIf(recipient.DisplayName <> recipient.MailAddress AndAlso Not String.IsNullOrEmpty(recipient.MailAddress), " (" & recipient.MailAddress & ")", "")
    End Sub
    Private Sub DisplayMessagePreview(languageCode As String, translation As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, mSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, idCommunity As Integer, Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing) Implements IViewUserMessages.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.InitializeControlForPreview(languageCode, translation, New List(Of String), mSettings, idCommunity, obj)
    End Sub
#End Region

#Region "Control"
    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As lm.Comol.Core.Mail.Messages.dtoDisplayUserMessage = DirectCast(e.Item.DataItem, lm.Comol.Core.Mail.Messages.dtoDisplayUserMessage)
                Dim oLabel As Label = e.Item.FindControl("LBmessageSentOn")
                oLabel.Text = FormatDateTime(dto.CreatedOn, DateFormat.ShortDate) & " " & FormatDateTime(dto.CreatedOn, DateFormat.ShortTime)

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBdisplayMessage")
                Resource.setLinkButton(oLinkbutton, False, True)
            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                Dim oLabel As Label = e.Item.FindControl("LBmessageSentEmptyItems")
                oTableItem.Visible = (Me.RPTmessages.Items.Count = 0)

                oLabel.Text = Resource.getValue("NoMessageFound")
        End Select
    End Sub
    Private Sub RPTmessages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmessages.ItemCommand
        Dim idMessage As Long = CLng(e.CommandArgument)
        Me.CurrentPresenter.LoadMessage(idMessage)
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.DVpreview.Visible = False
        Me.CurrentPresenter.LoadUserMessages(ContainerContext, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub UserMessages_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
    End Sub
#End Region

End Class