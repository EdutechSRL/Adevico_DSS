Imports lm.Comol.Core.BaseModules.Tickets.Domain.Enums
Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports lm.Comol.Core.DomainModel

Public Class EditExternal
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewTicketEditExt

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketEditExtPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketEditExtPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketEditExtPresenter(Me.PageUtility.CurrentContext, Me)
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

    Private _IsFirst As Boolean = True

    Private _LastAccess As DateTime?
    Private _FirstNew As Boolean = False

    Private _IsLocked As Boolean = True

    Private _IsReadOnly As Boolean = False

#End Region

#Region "Implements"
    'Property della VIEW
    Private _currentTicketId As Int64 = 0
    Public Property TicketId As Long Implements TK.Presentation.View.iViewTicketEditExt.TicketId
        Get
            'If (Me._currentTicketId = 0) Then
            '    Try
            '        Me._currentTicketId = System.Convert.ToInt64(Me.ViewState("TicketId"))
            '    Catch ex As Exception

            '    End Try
            'End If

            'If (Me._currentTicketId = 0) Then
            '    Try
            '        Me._currentTicketId = System.Convert.ToInt64(Request.QueryString("TkId"))
            '    Catch ex As Exception
            '    End Try
            'End If

            'Return Me._currentTicketId


            Dim TkId As Int64 = -1
            Try
                TkId = System.Convert.ToInt64(Request.QueryString("TkId"))
            Catch ex As Exception
            End Try

            If (TkId <= 0) Then
                TkId = TK.Domain.RootObject.GetTicketId(Request.QueryString("TkId"))
            End If

            Me._currentTicketId = ViewStateOrDefault("TicketId", TkId)
            Return _currentTicketId

        End Get
        Set(value As Long)
            Me.ViewState("TicketId") = value
            Me._currentTicketId = value
        End Set
    End Property

    Public ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User Implements TK.Presentation.View.iViewTicketEditExt.CurrentUser
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property

    Public Property DraftMsgId As Long Implements TK.Presentation.View.iViewTicketEditExt.DraftMsgId
        Get
            'Dim DrfId As Int64 = 0
            Return ViewStateOrDefault("DraftId", 0)
        End Get
        Set(value As Long)
            ViewState("DraftId") = value
        End Set
    End Property


#End Region

#Region "Inherits"
    'Property del PageBase

#End Region


    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.Page_Title = Resource.getValue("Page.Title.External")

        'Solo per Ticket
        'Solitamente passato da Business all'inizializzazione
        Dim actions As New List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)

        actions.Add(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)

        If Not IsNothing(Me.CurrentUser) AndAlso Me.CurrentUser.PersonId > 0 Then
            Dim dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions = lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none

            Me.CTRLcommands.InitializeControlForJQuery(actions, dAction)
        Else
            Me.CTRLcommands.Visible = False
        End If


    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.Master.BindSkin()
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
            HYPbackTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalList()
            .setHyperLink(HYPbackBot, True, True)
            HYPbackBot.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalList()

            .setLinkButton(LNBsubmit, True, True)
            .setLinkButton(LNBsubmitCloseRes, True, True)
            .setLinkButton(LNBsubmitCloseUnres, True, True)

            .setLabel(LByourMessage_t)

            .setLabel(LBnotificationTogglerOn)
            .setLabel(LBnotificationTogglerOff)

            .setLinkButton(LNBsaveMailSet, True, True)
            .setLiteral(LTnotificationSettings_t)

            LTonBehalf.Text = String.Format(LTonBehalf.Text, .getValue("onBehalf.text"))

            If String.IsNullOrEmpty(LBnotificationTogglerOn.Text) Then
                LBnotificationTogglerOn.Text = "Show notification settings"
            End If
            If String.IsNullOrEmpty(LBnotificationTogglerOff.Text) Then
                LBnotificationTogglerOn.Text = "Hide notification settings"
            End If
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        'Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalLogin)
        Me.CTRLtopBar.LogOut()
    End Sub

    Public Overrides Sub ShowNoAccess()

        Me.CTRLtopBar.Visible = False

        Me.PNLcontent.Visible = False
        Me.PNLserviceDisabled.Visible = True
    End Sub

#End Region

#Region "Implements"
    'Sub e function della View
    Public Function GetChangeStatusMessage(NewStatus As TK.Domain.Enums.TicketStatus) As String Implements TK.Presentation.View.iViewTicketEditExt.GetChangeStatusMessage
        Return Resource.getValue("ChangeStatus.message").Replace("{newstatus}", Resource.getValue("Status." & NewStatus.ToString()))
    End Function

    Public Sub InitView(TicketData As TK.Domain.DTO.DTO_UserModify) Implements TK.Presentation.View.iViewTicketEditExt.InitView

        PHsendError.Visible = _IsReadOnly

        If TicketData.Errors = TK.Domain.Enums.TicketEditUserErrors.none Then

            DIVhystWrapper.Attributes.Add("class", "historywrapper collapsablerows " & TicketData.Status.ToString())

            Me.CTRLeditorText.HTML = ""

            LTticketId.Text = TicketData.TicketId.ToString()
            LTstatus.Text = Resource.getValue("Status." & TicketData.Status.ToString())
            LTcategory.Text = TicketData.CategoryName
            LTtitle.Text = TicketData.Title

            If TicketData.IsClosed Or _IsReadOnly Then
                PHedit.Visible = False
            Else
                PHedit.Visible = Not TicketData.IsClosed
                LTcurUser.Text = TicketData.CurrentUserDisplayName
                LTcurRole.Text = Resource.getValue("UserType." & TicketData.CurrentUserType.ToString())
            End If

            _IsClosed = TicketData.IsClosed

            _TotalRow = TicketData.Messages.Count()

            _LastAccess = TicketData.LastUserAccess

            Me.RPTmessages.DataSource = TicketData.Messages
            Me.RPTmessages.DataBind()

            SetCollapsingLit(Me.LTmessagesExpCol, (_TotalRow > 0))

            If Not IsNothing(TicketData.DraftMessage) Then

                Me.CTRLeditorText.HTML = TicketData.DraftMessage.Text

                'SOLO per UTENTI DENTRO la piattaforma!

                Dim attachments As IList(Of TK.Domain.DTO.DTO_AttachmentItem) = New List(Of TK.Domain.DTO.DTO_AttachmentItem)()

                If Not IsNothing(TicketData.DraftMessage.Attachments) AndAlso TicketData.DraftMessage.Attachments.Count() > 0 AndAlso Me.CurrentUser.PersonId > 0 Then

                    attachments = (From atc As TK.Domain.TicketFile _
                                   In TicketData.DraftMessage.Attachments _
                                   Select New TK.Domain.DTO.DTO_AttachmentItem(atc)).ToList()

                    DIVatchCom.Attributes.Add("class", "fieldrow attachments")

                    Me.CTRLattView.InitUc(attachments, False, -1, True)
                Else
                    DIVatchCom.Attributes.Add("class", "fieldrow attachments empty")
                End If

                Dim actions As New List(Of Repository.RepositoryAttachmentUploadActions)
                actions.Add(Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem)

                'Upload File

                Me.CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

                'Inizializzatore Dialog
                CTRLinternalUpload.Visible = True
                CTRLinternalUpload.InitializeControl(TicketData.DraftMessage.Id, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, 0, True, CurrentUser.PersonId) 'NO COM ID => portale!

            Else
                CTRLinternalUpload.Visible = False
            End If

            If (TicketData.Condition = TK.Domain.Enums.TicketCondition.blocked OrElse _
    TicketData.Condition = TK.Domain.Enums.TicketCondition.cancelled) Then
                _IsLocked = True
                Me.ShowLockedTicket(TicketData.Condition)
            End If


            LTonBehalf.Visible = TicketData.IsBehalf

            'Notifiche
            Me.CTRLmailSetOwner.BindSettings(True, _
                                             TicketData.IsDefaultNotOwner, _
                                             TicketData.OwnerMailSettings, _
                                             TK.Domain.Enums.ViewSettingsUser.Owner, _
                                             TicketData.IsCreatorNotificationEnable, _
                                             False, False)


        ElseIf TicketData.Errors = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketEditUserErrors.IsDraft Then
            Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketAdd(ViewCommunityId, TicketData.TicketId))
        Else
            'Gestione altri errori
        End If

    End Sub

    Public Sub ShowChangeStatusError(IsReopen As Boolean) Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketEditExt.ShowChangeStatusError
        PHsendError.Visible = True
        If IsReopen Then
            LTSendErrorInfo.Text = Resource.getValue("SendError.reopen")
        Else
            LTSendErrorInfo.Text = Resource.getValue("SendError.close")
        End If
    End Sub

    Public Sub ShowSendError([Error] As TK.Domain.Enums.TicketMessageSendError) Implements TK.Presentation.View.iViewTicketEditExt.ShowSendError

        If [Error] = TicketMessageSendError.none Then
            PHsendError.Visible = False
            'Me.LTSendErrorInfo.Text = Resource.getValue("SendError." & [Error].ToString())
        Else
            PHsendError.Visible = True
            Me.LTSendErrorInfo.Text = Resource.getValue("SendError." & [Error].ToString())
        End If

        'PHsendError.Visible = True
        'Me.LTSendErrorInfo.Text = Resource.getValue("SendError." & [Error].ToString())

    End Sub

    Public Sub ShowDraft(TicketId As Long) Implements TK.Presentation.View.iViewTicketEditExt.ShowDraft
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalAdd(TicketId))
    End Sub

    Private Function GetUploadedItems(draftMessage As TK.Domain.Message) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements TK.Presentation.View.iViewTicketEditExt.GetUploadedItems
        Return CTRLinternalUpload.UploadFiles(False, draftMessage)
    End Function

    Public Sub SetReadOnly(IsTicketLocked As Boolean) Implements TK.Presentation.View.iViewTicketEditExt.SetReadOnly
        PHedit.Visible = False
        LNBsubmit.Enabled = False
        LNBsubmit.CssClass &= " disabled"
        LNBsubmitCloseRes.Enabled = False
        LNBsubmitCloseRes.CssClass &= " disabled"
        LNBsubmitCloseUnres.Enabled = False
        LNBsubmitCloseUnres.CssClass &= " disabled"

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
    Private Sub setRowContainer(ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                            ByVal IsFirst As Boolean, _
                            ByVal Type As TK.Domain.Enums.MessageType, _
                            ByVal IsCloseNotice As Boolean, _
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
        Text = Text.Replace("{date}", "<span class=""date"">" & Message.SendedOn.ToString(TicketHelper.DateTimeFormat) & "</span>")
        Text = Text.Replace("{ToCategory}", "<span class=""category"">" & Message.ToCategory & "</span>")
        Text = Text.Replace("{ToUser}", "<span class=""user"">" & Message.ToUser & "</span>")
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
        If IsNothing(Message.Attachments) OrElse Not Message.Attachments.Any() Then
            Return " empty"
        End If
    End Function

    Private Sub ShowLockedTicket(ByVal condition As TK.Domain.Enums.TicketCondition)

        Me.CTRLheadMessages.Visible = True
        Me.CTRLheadMessages.InitializeControl( _
            Resource.getValue("Message.Locked." & condition.ToString()), _
            lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

        Me.PHedit.Visible = False
        Me.LNBsubmit.Enabled = False
        Me.LNBsubmitCloseRes.Enabled = False
        Me.LNBsubmitCloseUnres.Enabled = False

        Me.CTRLcommands.Visible = False

    End Sub

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)
    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim Message As TK.Domain.DTO.DTO_UserModifyItem = e.Item.DataItem
            If Not IsNothing(Message) Then

                Me.setRowContainer(e.Item, Me._IsFirst, Message.MessageType, Message.IsCloseMessage, Message.MessageId)
                _IsFirst = False

                Dim PLHnormalMessage As PlaceHolder = e.Item.FindControl("PLHnormalMessage")
                Dim PLHsystemMessage As PlaceHolder = e.Item.FindControl("PLHsystemMessage")

                If Message.MessageType = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MessageType.System AndAlso Not IsNothing(PLHsystemMessage) Then
                    PLHnormalMessage.Visible = False
                    PLHsystemMessage.Visible = True
                    TicketHelper.SetRPTLiteral(e.Item, "LTusrName", Resource.getValue("UserName.System"))
                    TicketHelper.SetRPTLiteral(e.Item, "LTusrRole", "")

                    TicketHelper.SetRPTLiteral(e.Item, "LTsysText", GetActionText(Message))

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
                Else
                    If Message.IsFirst Then
                        Me.setAnchor(e.Item.FindControl("LTanchors"), TicketHelper.AnchorType.firstNews.ToString)
                    End If
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
            SetMessage(True)
        Else
            SetMessage(False)
            Me.CurrentPresenter.SendMessage(Me.CTRLeditorText.HTML, CTRLeditorText.Text, CloseMessage, IsSolved)
        End If

    End Sub

    Private Sub SetMessage(ByVal Show As Boolean)

        If Show Then
            DVmsgheader.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" empty", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl( _
                Resource.getValue("Message.EmptyMessage"), _
                lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
        Else
            DVmsgheader.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        End If

    End Sub

    Private Sub CTRLtopBar_UpdateInternationalization(oLingua As Comol.Entity.Lingua) Handles CTRLtopBar.UpdateInternationalization
        MyBase.UpdateLanguage(oLingua)
        Me.BindDati()
    End Sub

    Private Sub TMsession_Tick(sender As Object, e As System.EventArgs) Handles TMsession.Tick

        Me.CurrentPresenter.SendTimerAction()

        If IsNothing(Me.CurrentUser) OrElse Me.CurrentUser.UserId <= 0 Then
            Me.TMsession.Enabled = False
        End If

    End Sub

    Private Sub CTRLinternalUpload_ItemsNotAdded(action As Repository.RepositoryAttachmentUploadActions) Handles CTRLinternalUpload.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub CTRLinternalUpload_MessageNotFound() Handles CTRLinternalUpload.MessageNotFound
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub CTRLinternalUpload_UploadFile() Handles CTRLinternalUpload.UploadToModuleObject
        Me.CurrentPresenter.UploadFile(Me.CTRLeditorText.HTML, Me.CTRLeditorText.Text)
        Master.ClearOpenedDialogOnPostback()
    End Sub

    Private Sub CTRLattView_FileAction(sender As Object, e As RepeaterCommandEventArgs) Handles CTRLattView.FileAction
        Dim idAttachment As Int64 = 0
        Int64.TryParse(e.CommandArgument, idAttachment)
      
        If e.CommandName = "File_Delete" AndAlso idAttachment > 0 Then
            Me.CurrentPresenter.DeleteFile(idAttachment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End If
    End Sub
#End Region


    Public Function getExpandText() As String
        Return Me.Resource.getValue("ExpandCollapse.title")
    End Function

    Private Sub LNBsaveMailSet_Click(sender As Object, e As EventArgs) Handles LNBsaveMailSet.Click

        Me.CurrentPresenter.SetnotificationSettings(Me.CTRLmailSetOwner.GetSettings())

    End Sub

End Class