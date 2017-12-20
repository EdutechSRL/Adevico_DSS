Imports lm.Comol.Core.BaseModules.Tickets.Domain.Enums
Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports lm.Comol.Core.DomainModel

Public Class EditUser
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewTicketEditUsr
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketEditUsrPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketEditUsrPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketEditUsrPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne
    Private _RowIndex As Integer = 0
    Private _TotalRow As Integer = 0
    Private _IsClosed As Boolean = False
    Dim lastLkbReopen As LinkButton = Nothing
    Private _IsFirst As Boolean = True
    Private _LastAccess As DateTime?
    Private _FirstNew As Boolean = False
    Private _IsLocked As Boolean = True
    Private Property _IsReadOnly As Boolean = False

    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Description")
        End Get
    End Property
#End Region

#Region "Implements"
    'Property della VIEW
    Public Property TicketId As Long Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketEditUsr.TicketId
        Get
            Dim TkId As Int64 = -1
            Try
                TkId = System.Convert.ToInt64(Request.QueryString("Id"))
                'TK.Domain.RootObject.GetTicketId(System.Convert.ToInt64(Request.QueryString("Id")))
            Catch ex As Exception
            End Try

            If (TkId <= 0) Then
                TkId = TK.Domain.RootObject.GetTicketId(Request.QueryString("Id"))
            End If

            Return ViewStateOrDefault("TicketId", TkId)

        End Get
        Set(value As Long)
            ViewState("TicketId") = value
        End Set
    End Property

    Private Property TicketCode As String
        Get
            Dim UrlCode As String = Request.QueryString("Id")
            Return ViewStateOrDefault("TicketCode", UrlCode)
        End Get
        Set(value As String)
            ViewState("TicketCode") = value
            HYPbackManagementTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditResolver(ViewCommunityId, value)
        End Set
    End Property

    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewTicketEditUsr.ViewCommunityId
        Get
            Dim ComId As Integer = 0
            Try
                ComId = ViewStateOrDefault("CurrentComId", -1)
            Catch ex As Exception
            End Try

            If ComId < 0 Then
                Try
                    ComId = System.Convert.ToInt32(Request.QueryString("CommunityId"))
                Catch ex As Exception
                End Try
            End If

            Return ComId
        End Get
        Set(value As Integer)
            Me.ViewState("CurrentComId") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    'Property del PageBase

    'Public Overrides ReadOnly Property AlwaysBind As Boolean
    '    Get
    '        Return False
    '    End Get
    'End Property

    'Public Overrides ReadOnly Property VerifyAuthentication As Boolean
    '    Get
    '        Return False
    '    End Get
    'End Property

#End Region


    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()

        Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditUser", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)
            .setLiteral(LTticketId_t)
            .setLiteral(LTstatus_t)
            .setLiteral(LTcategory_t)

            .setHyperLink(HYPbackTop, True, True)
            HYPbackTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId)
            .setHyperLink(HYPbackBot, True, True)
            HYPbackBot.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId)

            .setHyperLink(HYPbackManagementTop, True, True)
            '            HYPbackManagementTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditResolver(ViewCommunityId, TicketId)

            'ToDo!!!
            .setHyperLink(HYPbackBehalfTop, True, True)
            HYPbackBehalfTop.NavigateUrl = "" 'ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(ViewCommunityId)

            .setLinkButton(LNBsubmit, True, True)
            .setLinkButton(LNBsubmitCloseRes, True, True)
            .setLinkButton(LNBsubmitCloseUnres, True, True)

            .setLabel(LByourMessage_t)

            .setLinkButton(LNBaddUsers, True, True, False, False)
            .setLinkButton(LNBremoveBehalf, True, True, False, False)

            .setCheckBox(CBXhideToOwner)

            '.setLabel(LBbehalfAction_t)

            .setLinkButton(Me.LNBhideToOwner, True, True, False, False)
            .setLinkButton(Me.LNBshowToOwner, True, True, False, False)

            LTonBehalf.Text = String.Format(LTonBehalf.Text, .getValue("onBehalf.text"))

            'Notification
            .setLiteral(LTnotificationSettings_t)

            .setLabel(LBMailSetOwner_t)
            .setLabel(LBMailSetCreator_t)

            .setLinkButton(LNBsaveMailSet, True, True, False, False)

            .setLabel(LBnotificationTogglerOn)
            .setLabel(LBnotificationTogglerOff)

            If String.IsNullOrEmpty(LBnotificationTogglerOn.Text) Then
                LBnotificationTogglerOn.Text = "Show notification settings"
            End If
            If String.IsNullOrEmpty(LBnotificationTogglerOff.Text) Then
                LBnotificationTogglerOn.Text = "Hide notification settings"
            End If
        End With
        '
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Sub InitPersonSelector(hidePersonId As List(Of Integer)) Implements TK.Presentation.View.iViewTicketEditUsr.InitPersonSelector
        CTRLselectUsers.InitializeControl( _
            lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, _
            False, 0, _
            hidePersonId, Nothing, _
            Resource.getValue("Modal.InternalUsers.Description"))

        Me.DIVusers.Visible = True
    End Sub

#End Region

#Region "Implements"
    Public Sub InitView(
                       TicketData As TK.Domain.DTO.DTO_UserModify, _
                        actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), _
                        dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, _
                        rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, _
                        idCommunity As Integer,
                        idMessage As Long) Implements TK.Presentation.View.iViewTicketEditUsr.InitView

        'CONTROLLARE!!! --> 'Me.CTRLheadMessages.Visible = False
        TicketCode = TicketData.Code

        PHsendError.Visible = _IsReadOnly AndAlso TicketData.IsReadOnly

        Me.DIVhystWrapper.Visible = True
        Me.PNLinfo.Visible = True
        Me.PNLmailSettings.Visible = True

        DVnotificationToggler.Visible = True

        With TicketData

            If .Errors = TK.Domain.Enums.TicketEditUserErrors.none Then



                HYPbackTop.Visible = .ShowToUserList
                HYPbackManagementTop.Visible = .ShowToManagementList
                HYPbackBehalfTop.Visible = False '.ShowToBehalfList


                Dim HystWrapperCss As String = "historywrapper collapsablerows " & .Status.ToString()

                If (.IsBehalf) Then
                    HystWrapperCss &= " behalf"
                End If

                If (.IsHideToOwner) Then
                    HystWrapperCss &= " hidetoowner"
                End If

                DIVhystWrapper.Attributes.Add("class", HystWrapperCss.Trim())


                Me.CTRLeditorText.HTML = ""

                LTticketId.Text = .Code '.TicketId.ToString()
                LTstatus.Text = Resource.getValue("Status." & .Status.ToString())

                If Not .Condition = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketCondition.active Then
                    LTstatus.Text &= String.Format(" ({0})", Resource.getValue("Condition." & .Condition.ToString()))
                End If

                LTcategory.Text = .CategoryName
                LTtitle.Text = .Title

                If .IsClosed OrElse _IsReadOnly OrElse .IsReadOnly Then
                    PHedit.Visible = False
                Else
                    PHedit.Visible = Not .IsClosed
                    LTcurUser.Text = .CurrentUserDisplayName
                    LTcurRole.Text = Resource.getValue("UserType." & .CurrentUserType.ToString())
                End If

                _IsClosed = .IsClosed

                _TotalRow = .Messages.Count()

                _LastAccess = .LastUserAccess

                Me.RPTmessages.DataSource = .Messages
                Me.RPTmessages.DataBind()

                If Not IsNothing(lastLkbReopen) Then
                    lastLkbReopen.Visible = True
                    Resource.setLinkButton(lastLkbReopen, True, True, False, True)
                    lastLkbReopen.CommandName = "ReOpen"
                End If

                If Not IsNothing(.DraftMessage) Then
                    Me.CTRLeditorText.HTML = .DraftMessage.Text
                    Me.DraftMsgId = .DraftMessage.Id


                    Dim Attachments As IList(Of TK.Domain.DTO.DTO_AttachmentItem) = New List(Of TK.Domain.DTO.DTO_AttachmentItem)()

                    If Not IsNothing(.DraftMessage.Attachments) Then
                        Attachments = (From atc As TK.Domain.TicketFile _
                                       In .DraftMessage.Attachments _
                                       Select New TK.Domain.DTO.DTO_AttachmentItem(atc)).ToList()

                        DIVatchCom.Attributes.Add("class", "fieldrow attachments")

                        Me.CTRLattView.InitUc(Attachments, False, -1, True)
                    Else
                        DIVatchCom.Attributes.Add("class", "fieldrow attachments empty")
                    End If

                End If

                'Upload File
                Me.CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

                'Inizializzatore Dialog
                CTRLinternalUpload.Visible = True
                CTRLinternalUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, 0, False) 'NO COM ID => portale!

                If (.Condition = TK.Domain.Enums.TicketCondition.blocked OrElse _
                    .Condition = TK.Domain.Enums.TicketCondition.flaggedNblocked OrElse _
                    .Condition = TK.Domain.Enums.TicketCondition.cancelled) Then
                    _IsLocked = True
                    Me.ShowLockedTicket(.Condition)
                End If

                'Tasti visibilità TICKET
                Me.LNBshowToOwner.Visible = .IsHideToOwner
                Me.LNBhideToOwner.Visible = Not .IsHideToOwner

                LTonBehalf.Visible = .IsBehalf


                'Notifiche
                Me.PNLmailSettings.Visible = False
                Me.PNLownerNotification.Visible = False

                If Not .IsReadOnly Then
                    'NON è Manager o Resolver

                    Me.PNLmailSettings.Visible = True

                    If .IsBehalf Then
                        'SE è in behalf
                        If .isOwner Then
                            'Sono l'assegnatario (vedo solo x Owner)
                            Me.PNLownerNotification.Visible = True
                            PNLcreatorNotification.Visible = False
                        Else
                            'Sono il CREATORE (vedo CRATORE e se non ha fatto accesso, l'Owner)
                            If (.IsHideToOwner) Then
                                Me.PNLownerNotification.Visible = True
                            Else
                                Me.PNLownerNotification.Visible = False
                            End If
                            Me.PNLcreatorNotification.Visible = True
                        End If

                    Else
                        'Non è in behalf (vedo solo CREATORE!)

                        Me.PNLownerNotification.Visible = False
                        Me.LBMailSetCreator_t.Visible = False
                        Me.PNLcreatorNotification.Visible = True
                    End If
                Else
                    'Me.PNLownerNotification.Visible = False
                    Me.LBMailSetCreator_t.Visible = False
                    Me.PNLcreatorNotification.Visible = False
                    HideNotification()

                End If


                If (Me.PNLownerNotification.Visible OrElse Me.PNLcreatorNotification.Visible) Then
                    Me.LBMailSetOwner_t.Visible = True
                    Me.LBMailSetCreator_t.Visible = True
                Else
                    Me.LBMailSetOwner_t.Visible = False
                    Me.LBMailSetCreator_t.Visible = False
                End If


                If (Me.PNLownerNotification.Visible) Then
                    Me.CTRLmailSetOwner.BindSettings( _
                        True, .IsDefaultNotOwner, .OwnerMailSettings, _
                        TK.Domain.Enums.ViewSettingsUser.Owner, _
                        .IsOwnerNotificationEnable, _
                        False, False)
                End If

                If (Me.PNLmailSettings.Visible) Then
                    Me.CTRLmailSetCreator.BindSettings( _
                        True, .IsDefaultNotCreator, .CreatorMailSettings, _
                        TK.Domain.Enums.ViewSettingsUser.Owner, _
                        .IsCreatorNotificationEnable, _
                        False, False)
                End If


                'Gestione Upload File
                'Passato da Business all'inizializzazione
                Me.CTRLcommands.InitializeControlForJQuery(actions, dAction)

                'Upload su message E community
                If idCommunity > 0 AndAlso actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                    CTRLcommunityUpload.Visible = True
                    CTRLcommunityUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity, idCommunity, rPermissions)

                Else
                    CTRLcommunityUpload.Visible = False
                End If

                'Link da comunità
                If idCommunity > 0 AndAlso actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                    CTRLlinkFromCommunity.Visible = True
                    CTRLlinkFromCommunity.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.linkfromcommunity, idCommunity, rPermissions)
                Else
                    CTRLlinkFromCommunity.Visible = False
                End If




                'GESTIONE ERRORI!
            ElseIf .Errors = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketEditUserErrors.IsDraft Then
                Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketAdd(ViewCommunityId, .TicketId))
            Else
                Me.HYPbackBot.Visible = False
                Me.HYPbackManagementTop.Visible = False
                Me.HYPbackBehalfTop.Visible = False
                Me.HYPbackTop.Visible = True

                Me.DIVhystWrapper.Visible = False
                Me.PNLinfo.Visible = False
                Me.PNLmailSettings.Visible = False
                LBnotificationTogglerOn.Visible = False

                DVbehalf.Visible = False

                PNLmessages.Visible = True
                CTRLheadMessages.Visible = True
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.InitError." & .Errors.ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.alert)


                'Gestione altri errori
            End If


            'Visibilità basata su Behalf

            Me.DVbehalf.Visible = .ShowBehalf OrElse .CanRemoveBehalf
            Me.LNBremoveBehalf.Visible = .IsBehalf AndAlso (.ShowBehalf OrElse .CanRemoveBehalf)
            Me.LNBaddUsers.Visible = .ShowBehalf
            'Me.LNBaddNewTicketUSer.Visible = .ShowBehalf

        End With

        SetCollapsingLit(Me.LTmessagesExpCol, (_TotalRow > 0))

    End Sub

    Public Sub ShowSendError([Error] As TK.Domain.Enums.TicketMessageSendError) Implements TK.Presentation.View.iViewTicketEditUsr.ShowSendError

        If ([Error] = TicketMessageSendError.none) Then
            Me.PNLmessages.Visible = False
            'PHsendError.Visible = False
        Else
            PNLmessages.Visible = True
            'PHsendError.Visible = True
            CTRLheadMessages.InitializeControl( _
                    Resource.getValue("SendError." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            'Me.LTSendErrorInfo.Text = Resource.getValue("SendError." & [Error].ToString())
        End If


    End Sub

    Public Sub ShowBehalfError(behalfError As TK.Domain.Enums.BehalfError) Implements TK.Presentation.View.iViewTicketEditUsr.ShowBehalfError

        Dim msgType As lm.Comol.Core.DomainModel.Helpers.MessageType = lm.Comol.Core.DomainModel.Helpers.MessageType.none

        DIVsend.Visible = True

        Select Case behalfError
            Case TK.Domain.Enums.BehalfError.success
                msgType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case TK.Domain.Enums.BehalfError.deleteSuccess
                msgType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
            Case TK.Domain.Enums.BehalfError.NoPermission
                msgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
            Case TK.Domain.Enums.BehalfError.dBerror
                msgType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
            Case TK.Domain.Enums.BehalfError.permissionRevoked
                msgType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
                ShowBehalfRevoked()
        End Select

        If msgType = Helpers.MessageType.none Then
            PNLmessages.Visible = True
            Return
        End If

        PNLmessages.Visible = True
        Me.CTRLheadMessages.InitializeControl( _
            Resource.getValue("BehalfOperation." & behalfError.ToString()), msgType)

    End Sub

    Public Function GetChangeStatusMessage(NewStatus As TK.Domain.Enums.TicketStatus) As String Implements TK.Presentation.View.iViewTicketEditUsr.GetChangeStatusMessage
        Dim message As String = Resource.getValue("ChangeStatus.message")
        Dim status As String = Resource.getValue("Status." & NewStatus.ToString())

        If Not String.IsNullOrEmpty(message) _
            AndAlso message.Contains("{0}") _
            AndAlso Not String.IsNullOrEmpty(status) Then

            Return String.Format(message, status)
        End If

        Return String.Format("{0} : {1}", "Status modified: ", NewStatus.ToString())

    End Function

    Public Sub ShowChangeStatusError(IsReopen As Boolean) Implements TK.Presentation.View.iViewTicketEditUsr.ShowChangeStatusError
        PHsendError.Visible = True
        If IsReopen Then
            LTSendErrorInfo.Text = Resource.getValue("SendError.reopen")
        Else
            LTSendErrorInfo.Text = Resource.getValue("SendError.close")
        End If

    End Sub

    Public Property DraftMsgId As Long Implements TK.Presentation.View.iViewTicketEditUsr.DraftMsgId
        Get
            'Dim DrfId As Int64 = 0
            Return ViewStateOrDefault("DraftId", 0)
        End Get
        Set(value As Long)
            ViewState("DraftId") = value
        End Set
    End Property

    Private Function GetUploadedItems(draftMessage As TK.Domain.Message, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements TK.Presentation.View.iViewTicketEditUsr.GetUploadedItems

        ''NOTIFICATION
        'Dim creatorSets As TK.Domain.Enums.MailSettings = Me.CTRLmailSetCreator.GetSettings()

        'Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        'If Me.PNLownerNotification.Visible Then
        '    ownerSets = Me.CTRLmailSetOwner.GetSettings()
        'End If
        Select Case action
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                Return CTRLinternalUpload.UploadFiles(False, draftMessage)
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                Return CTRLcommunityUpload.UploadFiles(True, draftMessage)
            Case Else
                Return New List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem)
        End Select
    End Function


    Public Sub SetReadOnly(IsTicketLocked As Boolean) Implements TK.Presentation.View.iViewTicketEditUsr.SetReadOnly

        PHedit.Visible = False
        DIVsend.Visible = False
        'LNBsubmit.Enabled = False
        'LNBsubmitCloseRes.Enabled = False
        'LNBsubmitCloseUnres.Enabled = False

        PHsendError.Visible = True
        If (IsTicketLocked) Then
            LTSendErrorInfo.Text = Resource.getValue("Error.TicketLocked")
        Else
            LTSendErrorInfo.Text = Resource.getValue("Error.ReadOnlySystem")
        End If

        _IsReadOnly = True
    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"
    Private Sub ShowLockedTicket(ByVal condition As TK.Domain.Enums.TicketCondition)


        Me.CTRLheadMessages.Visible = True
        Me.CTRLheadMessages.InitializeControl( _
            Resource.getValue("Message.Locked." & condition.ToString()), _
            lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

        Me.PHedit.Visible = False
        DIVsend.Visible = False
        'Me.LNBsubmit.Enabled = False
        'Me.LNBsubmitCloseRes.Enabled = False
        'Me.LNBsubmitCloseUnres.Enabled = False

        Me.CTRLcommands.Visible = False

    End Sub


    Private Sub ShowBehalfRevoked()
        DIVsend.Visible = False
        PHedit.Visible = False

        HideNotification()
    End Sub
    Private Sub setRowContainer(ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                                ByVal IsFirst As Boolean, _
                                ByVal Type As TK.Domain.Enums.MessageType, _
                                ByVal IsCloseNotice As Boolean, _
                                ByVal IsBehalf As Boolean, _
                                ByVal Id As Int64)

        Dim Lit As Literal = Item.FindControl("LTcontainter")
        If IsNothing(Lit) Then
            Return
        End If

        Dim IsLast As Boolean = False

        If (_RowIndex + 1) >= _TotalRow And Me._IsClosed Then
            IsLast = True
        End If

        Dim CssClass As String = "row message"

        If IsFirst Then
            CssClass = " first"
        ElseIf IsLast Then
            CssClass = " last"
        End If

        Select Case Type
            Case TK.Domain.Enums.MessageType.Request
                CssClass &= " request"
            Case TK.Domain.Enums.MessageType.FeedBack
                CssClass &= " feedback"
            Case TK.Domain.Enums.MessageType.PersonalFeedBack
                CssClass &= " feedback personal"
            Case TK.Domain.Enums.MessageType.System
                CssClass &= " notice"
        End Select

        If IsCloseNotice Then
            CssClass &= " closenotice"
        End If

        If IsBehalf Then
            CssClass &= " isbehalf"
        End If

        CssClass &= " clearfix"

        Lit.Text = Lit.Text.Replace("{CssClass}", CssClass)
        Lit.Text = Lit.Text.Replace("{RowId}", Id.ToString())
        _RowIndex += 1
    End Sub

    Private Function GetActionText(ByVal Message As TK.Domain.DTO.DTO_UserModifyItem)
        Dim Text As String = Resource.getValue("SysMessage." & Message.Action.ToString())

        If String.IsNullOrEmpty(Text) Then
            Return Message.MessageText
        End If

        Dim Status As String = Resource.getValue("ToStatus." & Message.ToStatus.ToString())
        If String.IsNullOrEmpty(Status) Then
            Status = Message.ToStatus.ToString()
        End If

        Dim Condition As String = Resource.getValue("ToCondition." & Message.ToCondition.ToString())
        If String.IsNullOrEmpty(Condition) Then
            Status = Message.ToCondition.ToString()
        End If


        Text = Text.Replace("{user}", "<span class=""creator"">" & Message.UserDisplayName & "</span>")
        Text = Text.Replace("{ToUser}", "<span class=""user"">" & Message.ToUser & "</span>")



        Text = Text.Replace("{date}", "<span class=""date"">" & Message.SendedOn.ToString(TicketHelper.DateTimeFormat) & "</span>")
        Text = Text.Replace("{ToCategory}", "<span class=""category"">" & Message.ToCategory & "</span>")

        Text = Text.Replace("{toStatus}", "<span class=""status"">" & Status & "</span>")

        Text = Text.Replace("{toCondition}", "<span class=""condition"">" & Condition & "</span>")

        Return Text
    End Function

    Public Function getAnchorText(ByVal AnchorId As String) As String
        Return Me.LTanchorTemplate.Text.Replace("{id}", AnchorId)
    End Function

    Public Sub setAnchor(ByRef Lit As Literal, ByVal AnchorId As String)
        If Not IsNothing(Lit) Then
            If Not String.IsNullOrEmpty(AnchorId) Then
                Lit.Text = Lit.Text & getAnchorText(AnchorId)
            End If

        End If
    End Sub

    Public Function GetCssFooter(ByVal Message As TK.Domain.DTO.DTO_UserModifyItem) As String
        If IsNothing(Message.Attachments) OrElse Not Message.Attachments.Any() AndAlso _
            Not Message.IsBehalf Then
            Return " empty"
        End If
        Return ""
    End Function

    Private Sub SetAlertMessageVisibility(ByVal Show As Boolean)

        'DVmsgheader.Attributes.Clear()
        'CTRLheadMessages
        If Show Then
            PNLmessageEmpy.Visible = True
            CTRLmessagesInfo.InitializeControl( _
            Resource.getValue("Message.EmptyMessage"), _
                lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            PNLmessageEmpy.CssClass = PNLmessageEmpy.CssClass.Replace(" empty", "")
            'DVmsgheader.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" empty", "").Trim())
            'CTRLmessagesInfo.Visible = True
            'CTRLmessagesInfo.InitializeControl( _
            '    Resource.getValue("Message.EmptyMessage"), _
            '    lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
            'Else
            'DVmsgheader.Attributes.Add("class", Me.LTmessageheaderCss.Text.Trim())
            'CTRLmessagesInfo.Visible = False
        Else
            PNLmessageEmpy.CssClass = String.Format("{0} empty", PNLmessageEmpy.CssClass)
        End If

        'Me.CTRLheadMessages.Visible = True

    End Sub

    ''' <summary>
    ''' Imposta la visibilità del Ticket.
    ''' Fatta funzione per permettere agevolmente l'utilizzo di altri sistemi rispetto al link...
    ''' </summary>
    ''' <param name="HideToUser"></param>
    ''' <remarks></remarks>
    Private Sub SetVisibility(ByVal HideToUser As Boolean)
        Me.CurrentPresenter.SetTicketVisibility(HideToUser)
    End Sub
    'Private Sub EditUser_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    '    'Me.PNLmessages.Visible = Me.CTRLheadMessages.Visible

    'End Sub

    Public Function getExpandText() As String
        Return Me.Resource.getValue("ExpandCollapse.title")
    End Function


    Private Sub HideNotification()
        Me.PNLmailSettings.Visible = False
        Me.LBnotificationTogglerOff.Visible = False
        Me.LBnotificationTogglerOn.Visible = False

        DVnotificationToggler.Visible = False

    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.TicketEditUser(CommunityId, Me.TicketCode), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region

#Region "Handler"

    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

    Private Sub RPTmessages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmessages.ItemCommand
        If e.CommandName = "ReOpen" Then
            Me.CurrentPresenter.ReopenTicket()
        End If
    End Sub

    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim Message As TK.Domain.DTO.DTO_UserModifyItem = e.Item.DataItem
            If Not IsNothing(Message) Then


                Me.setRowContainer(e.Item, Me._IsFirst, Message.MessageType, Message.IsCloseMessage, Message.IsBehalf, Message.MessageId)  'FALSE = Message.IsCloseMessage!!!
                _IsFirst = False

                Dim PLHnormalMessage As PlaceHolder = e.Item.FindControl("PLHnormalMessage")
                Dim PLHsystemMessage As PlaceHolder = e.Item.FindControl("PLHsystemMessage")

                If Message.MessageType = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MessageType.System AndAlso Not IsNothing(PLHsystemMessage) Then

                    PLHnormalMessage.Visible = False
                    PLHsystemMessage.Visible = True
                    TicketHelper.SetRPTLiteral(e.Item, "LTusrName", Resource.getValue("UserName.System"))
                    TicketHelper.SetRPTLiteral(e.Item, "LTusrRole", "")

                    TicketHelper.SetRPTLiteral(e.Item, "LTsysText", GetActionText(Message))

                    Dim LKBreopen As LinkButton = e.Item.FindControl("LNBreopen")
                    If Not IsNothing(LKBreopen) Then
                        If _IsClosed And _
                            (Message.ToStatus = TK.Domain.Enums.TicketStatus.closeSolved OrElse _
                             Message.ToStatus = TK.Domain.Enums.TicketStatus.closeUnsolved) Then
                            lastLkbReopen = LKBreopen
                        End If
                        LKBreopen.Visible = False
                    End If
                ElseIf Not IsNothing(PLHnormalMessage) Then
                    PLHnormalMessage.Visible = True
                    PLHsystemMessage.Visible = False

                    TicketHelper.SetRPTLiteral(e.Item, "LTusrName", Message.UserDisplayName)
                    TicketHelper.SetRPTLiteral(e.Item, "LTusrRole", Resource.getValue("UserType." & Message.UserType.ToString()))
                    TicketHelper.SetRPTLiteral(e.Item, "LTdate", Message.SendedOn, TicketHelper.DateTimeMode.OnlyDate)
                    TicketHelper.SetRPTLiteral(e.Item, "LTtime", Message.SendedOn, TicketHelper.DateTimeMode.OnlyTime)
                    TicketHelper.SetRPTLiteral(e.Item, "LTtextPrev", Message.MessagePreview)
                    TicketHelper.SetRPTLiteral(e.Item, "LTtext", Message.MessageText)

                    Dim UCattView As UC_AttachmentsView = e.Item.FindControl("CTRLattView")
                    If Not IsNothing(UCattView) Then
                        UCattView.InitUc(Message.Attachments, False, -1, False)
                    End If

                    Dim PNLbehalfInfo As Panel = e.Item.FindControl("PNLbehalfInfo")
                    If Not IsNothing(PNLbehalfInfo) Then
                        If Message.IsBehalf Then
                            PNLbehalfInfo.Visible = True

                            Dim LTbehalfInfo As Literal = e.Item.FindControl("LTbehalfInfo")
                            Dim LTbhUser As Literal = e.Item.FindControl("LTbhUser")

                            If Not IsNothing(LTbehalfInfo) AndAlso Not IsNothing(LTbhUser) Then
                                Resource.setLiteral(LTbehalfInfo)
                                LTbhUser.Text = Message.CreatorName
                            End If
                        Else
                            PNLbehalfInfo.Visible = False
                        End If
                    End If
                End If

                'Anchor - Begin -
                'ID
                Me.setAnchor(e.Item.FindControl("LTanchors"), "id-row-" & Message.MessageId.ToString())

                'Type IsNews
                If Not IsNothing(_LastAccess) Then
                    If Message.SendedOn >= _LastAccess AndAlso Not _FirstNew Then
                        _FirstNew = True
                        Me.setAnchor(e.Item.FindControl("LTanchors"), TicketHelper.AnchorType.firstNews.ToString)
                    End If
                ElseIf Message.IsFirst Then
                    Me.setAnchor(e.Item.FindControl("LTanchors"), TicketHelper.AnchorType.firstNews.ToString)
                End If

                'First
                If Message.IsFirst Then
                    Me.setAnchor(e.Item.FindControl("LTanchors"), TicketHelper.AnchorType.first.ToString)
                End If

                'Last
                If Message.IsLast Then
                    Me.setAnchor(e.Item.FindControl("LTanchors"), TicketHelper.AnchorType.last.ToString)
                End If

                'Editor (by init editor)

                'Anchor - End -

            End If

        End If
    End Sub
    Private Sub LNBsubmit_Click(sender As Object, e As System.EventArgs) Handles LNBsubmit.Click
        Me.SendMessage(False, False)
    End Sub

    Private Sub LNBsubmitCloseRes_Click(sender As Object, e As System.EventArgs) Handles LNBsubmitCloseRes.Click
        Me.SendMessage(True, True)
    End Sub

    Private Sub LNBsubmitCloseUnres_Click(sender As Object, e As System.EventArgs) Handles LNBsubmitCloseUnres.Click
        Me.SendMessage(True, False)
    End Sub

    Private Sub SendMessage(ByVal CloseMessage As Boolean, ByVal IsSolved As Boolean)

        If String.IsNullOrEmpty(Me.CTRLeditorText.Text) Then
            SetAlertMessageVisibility(True)
        Else
            SetAlertMessageVisibility(False)
            Me.CurrentPresenter.SendMessage(Me.CTRLeditorText.HTML, Me.CTRLeditorText.Text, CloseMessage, IsSolved)
        End If

    End Sub

    Private Sub TMsession_Tick(sender As Object, e As System.EventArgs) Handles TMsession.Tick

        Me.CurrentPresenter.SendTimerAction()

        If IsNothing(Me.CurrentPresenter.UserContext) OrElse Me.CurrentPresenter.UserContext.CurrentUserID <= 0 Then
            Me.TMsession.Enabled = False
        End If

    End Sub

#Region "Upload Items"
    Private Sub CTRLinternalUpload_ItemsNotAdded(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsNotAdded, CTRLcommunityUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_NoFilesToAdd(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.NoFilesToAdd, CTRLcommunityUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLinternalUpload_UploadToModuleObject() Handles CTRLinternalUpload.UploadToModuleObject
        CurrentPresenter.AttachmentsAddInternal(CTRLeditorText.HTML, CTRLeditorText.Text)
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLlinkFromCommunity_LinkFromRepository(items As List(Of ModuleActionLink)) Handles CTRLlinkFromCommunity.LinkFromRepository
        Me.CurrentPresenter.AttachmentsLinkFromCommunity(CTRLeditorText.HTML, CTRLeditorText.Text, items)
        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLcommunityUpload_UploadFileToCommunity() Handles CTRLcommunityUpload.UploadToRepository
        CurrentPresenter.AttachmentsAddAlsoToCommunity(CTRLeditorText.HTML, CTRLeditorText.Text)
        Master.ClearOpenedDialogOnPostback()
    End Sub
#End Region

    Private Sub CTRLattView_FileAction(sender As Object, e As RepeaterCommandEventArgs) Handles CTRLattView.FileAction
        Dim idAttachment As Int64 = 0
        Int64.TryParse(e.CommandArgument, idAttachment)
        If e.CommandName = "File_Delete" AndAlso idAttachment > 0 Then
            Me.CurrentPresenter.DeleteFile(idAttachment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End If

    End Sub

    Private Sub LNBremoveBehalf_Click(sender As Object, e As EventArgs) Handles LNBremoveBehalf.Click
        Me.CurrentPresenter.RemoveBehalf()
    End Sub

    Private Sub LNBaddUsers_Click(sender As Object, e As EventArgs) Handles LNBaddUsers.Click
        Me.CurrentPresenter.InitPersonSelector()
    End Sub

    Private Sub CTRLselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DIVusers.Visible = False
    End Sub

    Private Sub CTRLselectUsers_UserSelected(idUser As Integer) Handles CTRLselectUsers.UserSelected
        Me.CurrentPresenter.SetBehalfPerson(idUser, Me.CBXhideToOwner.Checked)
        Me.DIVusers.Visible = False
    End Sub

    Private Sub LNBhideToOwner_Click(sender As Object, e As EventArgs) Handles LNBhideToOwner.Click
        SetVisibility(True)
    End Sub


    Private Sub LNBshowToOwner_Click(sender As Object, e As EventArgs) Handles LNBshowToOwner.Click
        SetVisibility(False)
    End Sub

    Private Sub LKBsaveMailSet_Click(sender As Object, e As EventArgs) Handles LNBsaveMailSet.Click

        Dim ownerSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED
        Dim creatorSets As TK.Domain.Enums.MailSettings = TK.Domain.Enums.MailSettings.DISABLED

        If Me.PNLcreatorNotification.Visible Then
            creatorSets = Me.CTRLmailSetCreator.GetSettings()
        End If

        If Me.PNLownerNotification.Visible Then
            ownerSets = Me.CTRLmailSetOwner.GetSettings()
        End If

        Me.CurrentPresenter.SetnotificationSettings(creatorSets, ownerSets)

    End Sub



    Private Sub EditUser_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Dim itemCount As Integer = (If(LNBremoveBehalf.Visible, 1, 0)) + _
                (If(LNBaddUsers.Visible, 1, 0)) + _
                (If(LNBhideToOwner.Visible, 1, 0)) + _
                (If(LNBshowToOwner.Visible, 1, 0)) + _
                (If(LNBaddTicketUser.Visible, 1, 0)) + _
                (If(LNBaddNewTicketUSer.Visible, 1, 0))

        TicketHelper.CheckDropDownButton(DVbehalf, itemCount)

    End Sub


#End Region

End Class