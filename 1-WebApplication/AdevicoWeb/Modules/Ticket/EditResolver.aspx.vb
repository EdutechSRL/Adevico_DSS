Imports TK = lm.Comol.Core.BaseModules.Tickets
Imports lm.Comol.Core.DomainModel

Public Class EditResolver
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewTicketEditMan
    
#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.TicketEditManPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.TicketEditManPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.TicketEditManPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property

    Private Property CurrentAssignerUserID As Int64
        Get
            Dim id As Int64 = 0
            Try
                id = System.Convert.ToInt64(Me.ViewStateOrDefault("AssignerUserId", 0))
            Catch ex As Exception

            End Try
            Return id
        End Get
        Set(value As Int64)
            Me.ViewState("AssignerUserId") = value
        End Set
    End Property

    Private Property CurrentAssignerPersonID As Integer
        Get
            Dim id As Integer = 0
            Try
                id = System.Convert.ToInt32(Me.ViewStateOrDefault("AssignerPersonId", 0))
            Catch ex As Exception

            End Try
            Return id
        End Get
        Set(value As Integer)
            Me.ViewState("AssignerPersonId") = value
        End Set
    End Property

#End Region

#Region "Internal"
    'Property interne

    Private _LastAccess As DateTime?
    Private _IsFirstCss As Boolean = True

    Private _FirstNew As Boolean = False

    Private _RowIndex As Integer = 0
    Private _TotalRow As Integer = 0
    Private _IsClosed As Boolean = False
    Private _IsReadOnly As Boolean = False

    Dim lastLkbReopen As LinkButton = Nothing

    Private Property CanCollapseMessage As Boolean
        Get
            Return System.Convert.ToBoolean(ViewStateOrDefault("CanCollapseMessage", True))
        End Get
        Set(value As Boolean)
            ViewState("CanCollapseMessage") = value
        End Set
    End Property


    Private ReadOnly Property AssignerIsManager As Boolean
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Title")
        End Get
    End Property

    Private ReadOnly Property IsManager As Boolean
        Get
            If Me.UserType = TK.Domain.Enums.MessageUserType.CategoryManager OrElse Me.UserType = TK.Domain.Enums.MessageUserType.Manager Then
                Return True
            End If
            Return False
        End Get
    End Property

#End Region

#Region "Implements"
    'Property della VIEW
    Public Property TicketId As Long Implements TK.Presentation.View.iViewTicketEditMan.TicketId
        Get
            Dim TkId As Int64 = -1
            Try
                TkId = System.Convert.ToInt64(Request.QueryString("Id"))
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
            Return ViewStateOrDefault("TicketCode", Request.QueryString("Id"))
        End Get
        Set(value As String)
            ViewState("TicketCode") = value
            HYPtoUserViewTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditUser(ViewCommunityId, value)
        End Set
    End Property

    Public Property DraftMsgId As Long Implements TK.Presentation.View.iViewTicketEditMan.DraftMsgId
        Get
            'Dim DrfId As Int64 = 0
            Return ViewStateOrDefault("DraftId", 0)
        End Get
        Set(value As Long)
            ViewState("DraftId") = value
        End Set
    End Property

    Public Property MessageFilter As TK.Domain.Enums.EditManResMessagesShow Implements TK.Presentation.View.iViewTicketEditMan.MassageFilter
        Get
            Dim vsString As String = ViewStateOrDefault("MsgFilter", TK.Domain.Enums.EditManResMessagesShow.All.ToString())
            Dim val As TK.Domain.Enums.EditManResMessagesShow = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.EditManResMessagesShow), vsString), TK.Domain.Enums.EditManResMessagesShow)
            Return val
        End Get
        Set(value As TK.Domain.Enums.EditManResMessagesShow)
            ViewState("MsgFilter") = value.ToString()
        End Set
    End Property

    Public Property MessagesOrder As TK.Domain.Enums.EditManResMessagesOrder Implements TK.Presentation.View.iViewTicketEditMan.MessagesOrder
        Get

            Dim vsString As String = ViewStateOrDefault("MsgOrder", TK.Domain.Enums.EditManResMessagesOrder.oldertorecent.ToString())

            Dim val As TK.Domain.Enums.EditManResMessagesOrder = DirectCast([Enum].Parse(GetType(TK.Domain.Enums.EditManResMessagesOrder), vsString), TK.Domain.Enums.EditManResMessagesOrder)

            Return val
        End Get
        Set(value As TK.Domain.Enums.EditManResMessagesOrder)
            ViewState("MsgOrder") = value.ToString()
        End Set
    End Property

    Public Property UserType As TK.Domain.Enums.MessageUserType Implements TK.Presentation.View.iViewTicketEditMan.UserType
        Get
            Dim UT As String = ViewStateOrDefault("CurrentUserType", TK.Domain.Enums.MessageUserType.none.ToString())

            Return DirectCast([Enum].Parse(GetType(TK.Domain.Enums.MessageUserType), UT), TK.Domain.Enums.MessageUserType)
        End Get
        Set(value As TK.Domain.Enums.MessageUserType)
            ViewState("CurrentUserType") = value.ToString()
        End Set
    End Property
    Public Property ViewCommunityId As Integer Implements TK.Presentation.View.iViewTicketEditMan.ViewCommunityId
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
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PNLerrors.Visible = _IsReadOnly
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
        MyBase.SetCulture("pg_EditResolver", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTcontentTitle_t)

            .setLiteral(LTrequest_t)
            .setLiteral(LTstatus_t)
            .setLiteral(LTcategory_t)
            '.setLiteral(LTreqSet_Show)
            '.setLiteral(LTreqSet_Hide)

            '.setLabel(LBstatus_t)
            .setLabel(LBcommunity_t)
            .setLabel(LBcategoryInit_t)
            .setLabel(LBcategoryCur_t)
            .setLabel(LBassignTo_t)

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)

            '.setLinkButton(LNBchangeStatus, True, True)
            .setLinkButton(LNBassCate, True, True)
            .setLinkButton(LNBreassignCom, True, True)
            .setLinkButton(LNBreassign, True, True)
            .setLinkButton(LNBassignMe, True, True)
            .setLinkButton(LNBshowAll, True, True)
            .setLinkButton(LNBshowMsg, True, True)
            .setLinkButton(LNBshowSys, True, True)

            .setLinkButton(LNBorderOlder, True, True)
            .setLinkButton(LNBorderRecent, True, True)

            '.setLiteral(LTsort_t)
            '.setLiteral(LTswhow_t)
            .setLabel(LBsort_t)
            .setLabel(LBshow_t)

            .setHyperLink(HYPtoUserViewTop, True, True, False, False)
            'HYPtoUserViewTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketEditUser(ViewCommunityId, TicketCode)

            '.setLabel(LBnotification_t)
            .setLiteral(LTnotification_t)

            Me.CTRLmailSettings.LBalternate.Text = .getValue("LBalternate.text") '"** testo alternativo alle checkbox"
            Me.CTRLmailSettings.LBalternate.ToolTip = .getValue("LBalternate.ToolTip")

            .setLinkButton(LNBsaveMailSet, True, True, False, False)


            'hide/Show
            Me.LTsettingsHideShow.Text = Me.LTsettingHideShow_template.Text
            MyBase.SetCollapsingLit(LTsettingsHideShow, True)

            'Aggiunte per pop-up categoria
            LBLcatModInit_t.Text = LBcategoryInit_t.Text
            LBcatModCurrent_t.Text = LBcategoryCur_t.Text

            LTcatModify_t.Text = .getValue("CatModify.Description")

            .setLinkButton(LNBassCateUndo, True, True, False, False)
            .setHyperLink(HYPcateOpen, True, True, False, False)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        Me.PNLerrors.Visible = True
        Me.LBerrors.Text = errorMessage
    End Sub

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.TicketEditResolver(CommunityId, Me.TicketCode), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region

#Region "Implements"
    'Sub e function della View

    Public Sub InitView(TicketData As TK.Domain.DTO.DTO_ManagerModify, _
                        Categories As System.Collections.Generic.IList(Of TK.Domain.DTO.DTO_CategoryTree), _
                        CurrentCate As TK.Domain.DTO.DTO_CategoryTree, _
                        uploadActions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), _
                         rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, _
                         idCommunity As Integer, idMessage As Long,
                         hasManOrRes As Boolean,
                         hasCommunity As Boolean) _
                    Implements TK.Presentation.View.iViewTicketEditMan.InitView

        If (Me.ViewCommunityId > 0) Then
            Me.LNBreassignCom.Visible = False
        Else
            Me.LNBreassignCom.Visible = True
        End If

        TicketCode = TicketData.Code
        Me.UserType = TicketData.CurrentUserType

        Me.HYPbackTop.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListResolver(Me.ViewCommunityId)
        Me.HYPbackBottom.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.TicketListResolver(Me.ViewCommunityId)

        Me.HideReassign()
        PNLerrorMsg.Visible = False
        CTRLheadMessages.Visible = False

        If Not (TicketData.Errors = TK.Domain.Enums.TicketEditManErrors.none) Then
            'GESTIONE ERRORI
            Me.PNLglobal.Visible = False
            Me.PNLerrors.Visible = True

            HYPtoUserViewTop.Visible = False
            HYPbackBottom.Visible = False

            Dim errorString As String = Resource.getValue("Errors." & TicketData.Errors.ToString())

            If String.IsNullOrEmpty(errorString) Then
                errorString = TicketData.Errors.ToString()

                'Else
                '    Me.LBerrors.Text = ErrorString
            End If
            PNLerrorMsg.Visible = True
            CTRLheadMessages.Visible = True
            Me.CTRLheadMessages.InitializeControl( _
                       errorString, _
                       lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

            Return
        End If

        Dim WrapperString As String = "historywrapper collapsablerows " & Me.MessagesOrder.ToString() & " " & TicketData.Status.ToString()

        Me.DIVhystoryWrapper.Attributes.Add("class", WrapperString)

        PNLglobal.Visible = True
        Me.PNLerrors.Visible = Not _IsReadOnly 'false
        With TicketData

            Me.CurrentAssignerPersonID = .PersonAssignedId
            Me.CurrentAssignerUserID = .UserAssignedId


            If .Status = TK.Domain.Enums.TicketStatus.closeSolved OrElse .Status = TK.Domain.Enums.TicketStatus.closeUnsolved Then
                Me._IsClosed = True
            Else
                Me._IsClosed = False
            End If

            SetUcEditor(Me.MessagesOrder = TK.Domain.Enums.EditManResMessagesOrder.recentolder, _
                .CurrentUserDisplayName, _
                .CurrentUserType, _
                .Status, _
                uploadActions, _
                True)

            SetCollapsingLit(Me.LTmessagesExpCol, Me.CanCollapseMessage)

            'BindDDLstatus(.Status)


            _TotalRow = .Messages.Count()

            Me._IsFirstCss = True

            'Dati Anchor: Editor, first, last, LastRead, FirstNews
            _LastAccess = .LastUserAccess

            Me.RPTmessages.DataSource = .Messages
            Me.RPTmessages.DataBind()

            If Not IsNothing(lastLkbReopen) Then
                lastLkbReopen.Visible = True
                Resource.setLinkButton(lastLkbReopen, True, True, False, True)
                lastLkbReopen.CommandName = "reopen"
                lastLkbReopen.CommandArgument = "0"
            End If

            Me.LTcategory.Text = .CategoryCurrentName
            Me.LTstatus.Text = Resource.getValue("Status." & .Status.ToString())

            If Not (.Condition = TK.Domain.Enums.TicketCondition.active) Then
                Me.LTstatus.Text &= " <span class=""condition"">("
                Me.LTstatus.Text &= Resource.getValue("Condition." & .Condition.ToString())
                Me.LTstatus.Text &= ")</span>"

            End If

            Me.LTrequest.Text = .Code
            Me.LTtitle.Text = .Title

            If .CommunityName = TK.TicketService.ComPortalName Then
                Me.LBcommunity.Text = Resource.getValue("Community.Portal")
            Else
                Me.LBcommunity.Text = .CommunityName
            End If

            Me.LBcategoryInit.Text = .CategoryCreationName
            LBLcatModInit.Text = .CategoryCreationName

            Me.LBcategoryCurrent.Text = .CategoryCurrentName


            If String.IsNullOrEmpty(.UserAssigned) Then
                Me.LBassignTo.Text = Resource.getValue("Assignment.none")
            Else
                Me.LBassignTo.Text = .UserAssigned
            End If

            'DDL categorie
            Me.CTRLddlCat.InitDDL(Categories, .CategoryCurrentId, Resource.getValue("Category.select")) ', CurrentCate

            If Not IsNothing(TicketData.DraftMessage) Then



                Dim Attachments As IList(Of TK.Domain.DTO.DTO_AttachmentItem) = New List(Of TK.Domain.DTO.DTO_AttachmentItem)()

                If Not IsNothing(TicketData.DraftMessage.Attachments) Then
                    Attachments = (From atc As TK.Domain.TicketFile _
                                   In TicketData.DraftMessage.Attachments _
                                   Select New TK.Domain.DTO.DTO_AttachmentItem(atc)).ToList()
                End If

                Me.CTRLeditBottom.SetDraft( _
                    TicketData.DraftMessage.Text, _
                    TicketData.DraftMessage.Id, _
                    Attachments, TicketData.Condition)

                'Me.CTRLeditTop.SetDraft( _
                '    TicketData.DraftMessage.Text, _
                '    TicketData.DraftMessage.Id, _
                '    Attachments, TicketData.Condition)
            Else
                Me.CTRLeditBottom.ClearMessage()
                Me.CTRLeditTop.ClearMessage()
            End If

            'Notification settings
            Me.CTRLmailSettings.BindSettings(True, .IsDefault, .MailSettings, _
                                             TK.Domain.Enums.ViewSettingsUser.Manager, _
                                             .IsNotificationActive, _
                                             False, False)
            Me.CTRLmailSettings.ShowAlternateText = Not .IsActiveUser
            'Rivedere, tramite CTRLmailsettings e nascondendo i checkbox non "default"...

            'LTcreatorInfo.Visible = Not .IsActiveUser

            'Visibilità secondo la chiusura

            Me.DVddbuttonListAssign.Visible = Not .IsClosed
            'Me.LBcategoryCurrent.Visible = .IsClosed
            HYPcateOpen.Visible = Not .IsClosed
            Me.CTRLddlCat.Visible = Not .IsClosed
            Me.LNBassCate.Visible = Not .IsClosed

        End With

        Select Case Me.MessageFilter
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.All
                LNBshowAll.CssClass = "btnswitch first active"
                LNBshowMsg.CssClass = "btnswitch"
                LNBshowSys.CssClass = "btnswitch last"
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.MessageOnly
                LNBshowAll.CssClass = "btnswitch first"
                LNBshowMsg.CssClass = "btnswitch active"
                LNBshowSys.CssClass = "btnswitch last"
            Case lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.NotifiesOnly
                LNBshowAll.CssClass = "btnswitch first"
                LNBshowMsg.CssClass = "btnswitch"
                LNBshowSys.CssClass = "btnswitch last active"
        End Select


        If Me.MessagesOrder = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesOrder.oldertorecent Then
            LNBorderOlder.CssClass = "btnswitch first active"
            LNBorderRecent.CssClass = "btnswitch last"
        Else
            LNBorderOlder.CssClass = "btnswitch first"
            LNBorderRecent.CssClass = "btnswitch last active"
        End If

        'Upload File

        CTRLattachmentsHeader.InitializeControlForJQuery(uploadActions, uploadActions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))

        'Upload su message (portale)
        CTRLinternalUpload.Visible = True
        CTRLinternalUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, 0, False) ', idCommunity)


        'Upload su message E community
        If idCommunity > 0 AndAlso uploadActions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
            CTRLcommunityUpload.Visible = True
            CTRLcommunityUpload.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity, idCommunity, rPermissions)
        Else
            CTRLcommunityUpload.Visible = False
        End If

        'Link da comunità
        If idCommunity > 0 AndAlso uploadActions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
            CTRLlinkFromCommunity.Visible = True
            CTRLlinkFromCommunity.InitializeControl(idMessage, Repository.RepositoryAttachmentUploadActions.linkfromcommunity, idCommunity, rPermissions)
        Else
            CTRLlinkFromCommunity.Visible = False
        End If

        ShowByRole()

        Me.ShowAssignError(TK.Domain.Enums.CategoryReassignError.none)


        Me.LNBreassign.Visible = hasManOrRes

        Me.LNBreassignCom.Visible = hasCommunity

    End Sub

    Public Function GetChangeCategoryMessage() As String Implements TK.Presentation.View.iViewTicketEditMan.GetChangeCategoryMessage
        Return Resource.getValue("Message.ChangeCategory")
    End Function

    Public Function GetChangeUserMessage() As String Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketEditMan.GetChangeUserMessage
        Return Resource.getValue("Message.ChangeUser")
    End Function

    Public Function GetChangeStatusMessage(NewStatus As TK.Domain.Enums.TicketStatus) As String Implements TK.Presentation.View.iViewTicketEditMan.GetChangeStatusMessage
        Dim Message As String = Resource.getValue("Message.ChangeStatus")
        If String.IsNullOrEmpty(Message) Then
            Message = "New status: {newstatus}"
        End If

        Dim Status As String = Resource.getValue("Status." & NewStatus.ToString())
        If String.IsNullOrEmpty(Status) Then
            Status = NewStatus.ToString()
        End If

        Return Message.Replace("{newstatus}", Status)
    End Function

    Public Function GetChangeConditionMessage(Condition As TK.Domain.Enums.TicketCondition) As String Implements TK.Presentation.View.iViewTicketEditMan.GetChangeConditionMessage

        Dim Message As String = Resource.getValue("Message.ChangeCondition")
        If String.IsNullOrEmpty(Message) Then
            Message = "New condition: {newcondition}"
        End If

        Dim Status As String = Resource.getValue("Condition." & Condition.ToString())
        If String.IsNullOrEmpty(Status) Then
            Status = Condition.ToString()
        End If

        Return Message.Replace("{newcondition}", Status)
    End Function

    Public Sub ShowAssignError([Error] As TK.Domain.Enums.CategoryReassignError) Implements TK.Presentation.View.iViewTicketEditMan.ShowAssignError
        PNLerrorMsg.Visible = True
        Me.CTRLheadMessages.Visible = True

        Select Case [Error]
            Case TK.Domain.Enums.CategoryReassignError.CategoryNotFound
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.Reassign." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.error)

            Case TK.Domain.Enums.CategoryReassignError.error
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.Reassign." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.error)

            Case TK.Domain.Enums.CategoryReassignError.invalidTicket
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.Reassign." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

            Case TK.Domain.Enums.CategoryReassignError.noChange
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.Reassign." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.info)

            Case TK.Domain.Enums.CategoryReassignError.none
                PNLerrorMsg.Visible = False
                Me.CTRLheadMessages.Visible = False

            Case TK.Domain.Enums.CategoryReassignError.noPermission
                Me.CTRLheadMessages.InitializeControl( _
                    Resource.getValue("Message.Reassign." & [Error].ToString()), _
                    lm.Comol.Core.DomainModel.Helpers.MessageType.alert)

        End Select

    End Sub

    Public Sub ShowReopenError() Implements TK.Presentation.View.iViewTicketEditMan.ShowReopenError
        Me.ShowMessageToPage(Resource.getValue("Error.Reopen"))
    End Sub

    Public Sub ShowCategoryChanged() Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewTicketEditMan.ShowCategoryChanged
        PNLerrorMsg.Visible = True
        CTRLheadMessages.Visible = True
        Me.CTRLheadMessages.InitializeControl( _
                   Resource.getValue("Message.Category.Changed"), _
                   lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub

    Public Sub ShowSendError([Error] As TK.Domain.Enums.TicketMessageSendError) Implements TK.Presentation.View.iViewTicketEditMan.ShowSendError
        Me.ShowMessageToPage(Resource.getValue("Error.Send." & [Error].ToString()))
    End Sub

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewTicketEditMan.ShowNoPermission
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.TicketListUser(Me.ViewCommunityId))
    End Sub

    Public Sub SetReadOnly(IsTicketLocked As Boolean) Implements TK.Presentation.View.iViewTicketEditMan.SetReadOnly
        If Not IsTicketLocked Then

            'Assegnazione categoria
            Me.CTRLddlCat.IsReadOnly = True
            Me.LNBassCate.Enabled = False
            Me.LNBassCate.CssClass &= " disabled"

            'Assegnaizone utente
            Me.LNBreassign.Enabled = False
            Me.LNBreassign.CssClass &= " disabled"
            Me.LNBreassignCom.Enabled = False
            Me.LNBreassignCom.CssClass &= " disabled"

            Me.PHmessageEditorBottom.Visible = False
            Me.PHmessageEditorTop.Visible = False

            If Not IsNothing(lastLkbReopen) Then
                Me.lastLkbReopen.Enabled = False
                Me.lastLkbReopen.CssClass &= " disabled"
            End If

            Me.ShowMessageToPage(Resource.getValue("Errors.EditDisabledSystem"))

            _IsReadOnly = True
        End If
    End Sub

    Private Function GetUploadedItems(draftMessage As TK.Domain.Message, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements TK.Presentation.View.iViewTicketEditMan.GetUploadedItems
        Select Case action
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                Return CTRLinternalUpload.UploadFiles(False, draftMessage)
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                Return CTRLcommunityUpload.UploadFiles(True, draftMessage)
            Case Else
                Return New List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem)
        End Select
    End Function
#End Region

#Region "Internal"
    'Sub e Function "della pagina"

    'Funzione che gestirà l'evento dell'UC per l'edit/invio del messaggio.
    Private Sub SendMessage( _
                           ByVal Message As String, _
                        ByVal Preview As String, _
                           ByVal Status As TK.Domain.Enums.TicketStatus, _
                           ByVal HideToUser As Boolean)

        Me.CurrentPresenter.SendMessage(Message, Preview, HideToUser, Status, lm.Comol.Core.BaseModules.Tickets.Domain.Enums.MessageType.FeedBack, Me.DraftMsgId)

    End Sub

    Private Sub setRowContainer(ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                            ByVal Type As TK.Domain.Enums.MessageType, _
                            ByVal IsCloseNotice As Boolean)
    End Sub

    Public Function getRowIndex() As String
        Dim CurrenteRowIndex = _RowIndex
        _RowIndex += 1
        Return CurrenteRowIndex.ToString()
    End Function

    Public Function getItemEmptycss(ByRef Message As TK.Domain.DTO.DTO_UserModifyItem) As String

        If Not IsNothing(Message.Attachments) AndAlso Message.Attachments.Any() Then
            Return ""
        End If

        Return " empty"

    End Function

    Public Function getRowCss(ByRef Message As TK.Domain.DTO.DTO_UserModifyItem)

        If IsNothing(Message) Then
            Return ""
        End If

        Dim CssClass As String = "row message"
        If Me._IsFirstCss AndAlso Me.MessagesOrder = TK.Domain.Enums.EditManResMessagesOrder.oldertorecent Then
            CssClass &= " first"
            Me._IsFirstCss = False
        End If

        If Me.MessagesOrder = TK.Domain.Enums.EditManResMessagesOrder.recentolder AndAlso (_RowIndex + 1) >= _TotalRow Then
            CssClass &= " last"
        End If

        Select Case Message.MessageType
            Case TK.Domain.Enums.MessageType.Request
                CssClass &= " request"
            Case TK.Domain.Enums.MessageType.FeedBack
                CssClass &= " feedback"
            Case TK.Domain.Enums.MessageType.PersonalFeedBack
                CssClass &= " feedback personal"
            Case TK.Domain.Enums.MessageType.System
                CssClass &= " notice"
        End Select

        If Message.IsCloseMessage Then
            CssClass &= " closenotice"
        End If

        CssClass &= " clearfix"
        Return CssClass

    End Function

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

    Private Sub SetUcEditor(ByVal IsTop As Boolean, _
                             ByVal UserName As String, _
                             ByVal Role As TK.Domain.Enums.MessageUserType, _
                             ByVal Status As TK.Domain.Enums.TicketStatus, _
                            ByVal actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), _
                   Optional ByVal CanShowToUser As Boolean = True)

        EditIsTop = IsTop

        Me.CTRLeditTop.ClearMessage()
        Me.CTRLeditBottom.ClearMessage()

        If Me._IsClosed Then
            Me.PHmessageEditorBottom.Visible = False
            Me.PHmessageEditorTop.Visible = False
        Else
            If IsTop Then
                Me.PHmessageEditorBottom.Visible = False
                Me.PHmessageEditorTop.Visible = Not _IsReadOnly

                Me.CTRLeditTop.InitUC( _
                    UserName, _
                    Resource.getValue("UserType." & Role.ToString()), _
                    Status, _
                    Me.Resource, _
                    actions, _
                    CanShowToUser)
                Me.CTRLeditTop.AnchorVisibility = True
                Me.CTRLeditBottom.AnchorVisibility = False
            Else
                Me.PHmessageEditorBottom.Visible = Not _IsReadOnly
                Me.PHmessageEditorTop.Visible = False

                Me.CTRLeditBottom.InitUC( _
                    UserName, _
                    Resource.getValue("UserType." & Role.ToString()), _
                    Status, _
                    Me.Resource, _
                    actions, _
                    CanShowToUser)

                Me.CTRLeditTop.AnchorVisibility = False
                Me.CTRLeditBottom.AnchorVisibility = True
            End If
        End If

    End Sub

    Private Property EditIsTop As Boolean
        Get
            Return ViewStateOrDefault("EditIsTop", False)
        End Get
        Set(value As Boolean)
            ViewState("EditIsTop") = value
        End Set
    End Property

    Private Sub ShowReassign(ByVal IsInternal As Boolean)

        Me.PNLSelectorCommunity.Visible = Not IsInternal
        Me.PNLSelectorInternal.Visible = IsInternal

        Me.DVselectUsers.Visible = True

        If (IsInternal) Then
            Me.CTRLuserSelector.BindUser(Me.CurrentPresenter.GetManRes(Me.CurrentAssignerUserID))
        End If

    End Sub

    Private Sub HideReassign()
        Me.DVselectUsers.Visible = False
    End Sub

    Private Sub ShowByRole()
        Me.LNBreassign.Visible = Me.IsManager
        Me.LNBreassignCom.Visible = Me.IsManager
    End Sub

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

#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)

    Private Sub LNBshowAll_Click(sender As Object, e As System.EventArgs) Handles LNBshowAll.Click
        Me.MessageFilter = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.All
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub LNBshowMsg_Click(sender As Object, e As System.EventArgs) Handles LNBshowMsg.Click
        Me.MessageFilter = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.MessageOnly
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub LNBshowSys_Click(sender As Object, e As System.EventArgs) Handles LNBshowSys.Click
        Me.MessageFilter = lm.Comol.Core.BaseModules.Tickets.Domain.Enums.EditManResMessagesShow.NotifiesOnly
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub LNBorderOlder_Click(sender As Object, e As System.EventArgs) Handles LNBorderOlder.Click
        Me.MessagesOrder = TK.Domain.Enums.EditManResMessagesOrder.oldertorecent
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub LNBorderRecent_Click(sender As Object, e As System.EventArgs) Handles LNBorderRecent.Click
        Me.MessagesOrder = TK.Domain.Enums.EditManResMessagesOrder.recentolder
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub RPTmessages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmessages.ItemCommand
        Dim Id As Int64 = 0

        Try
            Id = System.Convert.ToInt64(e.CommandArgument)
        Catch ex As Exception

        End Try

        Select Case e.CommandName
            Case "hide"
                Me.CurrentPresenter.ChangeMsgVisibility(Id, False)
            Case "show"
                Me.CurrentPresenter.ChangeMsgVisibility(Id, True)
            Case "reopen"
                Me.CurrentPresenter.ChangeStatus(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.TicketStatus.open)
            Case "File_Hide"
                Me.CurrentPresenter.HideShowFile(Id, True)
            Case "File_Show"
                Me.CurrentPresenter.HideShowFile(Id, False)
            Case "File_Delete"
                Me.CurrentPresenter.DeleteFile(Id, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
        End Select

    End Sub

    Private Sub RPTmessages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmessages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim Message As TK.Domain.DTO.DTO_UserModifyItem = e.Item.DataItem
            If Not IsNothing(Message) Then

                Dim PLHnormalMessage As PlaceHolder = e.Item.FindControl("PLHnormalMessage")
                Dim PLHsystemMessage As PlaceHolder = e.Item.FindControl("PLHsystemMessage")

                If Not IsNothing(PLHnormalMessage) AndAlso Not IsNothing(PLHsystemMessage) Then
                    If Message.MessageType = TK.Domain.Enums.MessageType.System Then
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

                                If (_IsReadOnly) Then
                                    lastLkbReopen.Enabled = False
                                    lastLkbReopen.CssClass &= " disabled"
                                End If
                            End If
                            LKBreopen.Visible = False
                        End If
                    Else
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

                            UCattView.InitUc(Message.Attachments, True, Message.MessageId, False)
                            AddHandler UCattView.FileAction, AddressOf Me.RPTmessages_ItemCommand

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

                    Dim EnableHideShowmessage As Boolean = True

                    If (Message.MessageType = TK.Domain.Enums.MessageUserType.Partecipant) Then
                        EnableHideShowmessage = False
                    End If

                    Dim LKBhide As LinkButton = e.Item.FindControl("LNBhide")
                    If Not IsNothing(LKBhide) Then
                        If Message.IsVisible Then
                            Resource.setLinkButton(LKBhide, True, True)
                            LKBhide.CommandName = "hide"
                            LKBhide.CommandArgument = Message.MessageId.ToString()
                            LKBhide.Enabled = EnableHideShowmessage
                            LKBhide.Visible = True
                        Else
                            LKBhide.Visible = False
                        End If
                    End If

                    Dim LKBshow As LinkButton = e.Item.FindControl("LNBshow")
                    If Not IsNothing(LKBshow) Then
                        If Not Message.IsVisible Then
                            Resource.setLinkButton(LKBshow, True, True)
                            LKBshow.CommandName = "show"
                            LKBshow.CommandArgument = Message.MessageId.ToString()
                            LKBshow.Enabled = EnableHideShowmessage
                            LKBshow.Visible = True
                        Else
                            LKBshow.Visible = False
                        End If
                    End If


                End If

            End If
        End If
    End Sub

    Private Sub LNBassCate_Click(sender As Object, e As System.EventArgs) Handles LNBassCate.Click
        Me.CurrentPresenter.CategoryReassign(Me.CTRLddlCat.SelectedId)
    End Sub

    Private Sub LNBreassign_Click(sender As Object, e As System.EventArgs) Handles LNBreassign.Click
        ShowReassign(True)
    End Sub

    Private Sub CTRLuserSelector_CloseWindows() Handles CTRLuserSelector.CloseWindows
        Me.DVselectUsers.Visible = False
    End Sub

    Private Sub CTRLuserSelector_UserSelected(UserId As Long) Handles CTRLuserSelector.UserSelected

        Me.CurrentPresenter.AssignUser(UserId, AssignerIsManager)
    End Sub

    Private Sub LNBreassignCom_Click(sender As Object, e As System.EventArgs) Handles LNBreassignCom.Click
        If (Me.ViewCommunityId > 0) Then

            Dim UsersId As New List(Of Integer)

            If (Me.CurrentAssignerPersonID > 0) Then
                UsersId.Add(Me.CurrentAssignerPersonID)
            End If

            Me.CLTRselectPerson.InitializeControl( _
            lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers,
            False, Me.ViewCommunityId, UsersId, Nothing, _
            Resource.getValue("Modal.InternalUsers.Description"))

            ShowReassign(False)

        End If
    End Sub

    Private Sub CLTRselectPerson_CloseWindow() Handles CLTRselectPerson.CloseWindow
        Me.DVselectUsers.Visible = False
    End Sub

    Private Sub CLTRselectPerson_UserSelected(idUser As Integer) Handles CLTRselectPerson.UserSelected
        Me.CurrentPresenter.AssingPerson(idUser, False)

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
        If EditIsTop Then
            CurrentPresenter.AttachmentsAddInternal(CTRLeditTop.HtmlMessage, CTRLeditTop.PreviewMessage)
        Else
            CurrentPresenter.AttachmentsAddInternal(CTRLeditBottom.HtmlMessage, CTRLeditBottom.PreviewMessage)
        End If

        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLlinkFromCommunity_LinkFromRepository(items As List(Of ModuleActionLink)) Handles CTRLlinkFromCommunity.LinkFromRepository
        If EditIsTop Then
            CurrentPresenter.AttachmentsLinkFromCommunity(CTRLeditTop.HtmlMessage, CTRLeditTop.PreviewMessage, items)
        Else
            CurrentPresenter.AttachmentsLinkFromCommunity(CTRLeditBottom.HtmlMessage, CTRLeditBottom.PreviewMessage, items)
        End If

        Master.ClearOpenedDialogOnPostback()
    End Sub
    Private Sub CTRLcommunityUpload_UploadFileToCommunity() Handles CTRLcommunityUpload.UploadToRepository
        If EditIsTop Then
            CurrentPresenter.AttachmentsAddAlsoToCommunity(CTRLeditTop.HtmlMessage, CTRLeditTop.PreviewMessage)
        Else
            CurrentPresenter.AttachmentsAddAlsoToCommunity(CTRLeditBottom.HtmlMessage, CTRLeditBottom.PreviewMessage)
        End If
        Master.ClearOpenedDialogOnPostback()
    End Sub
#End Region


    Private Sub CTRLeditTop_FileDelete(idAttachment As Long) Handles CTRLeditTop.FileDelete
        Me.CurrentPresenter.DeleteFile(idAttachment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
    End Sub

    Private Sub CTRLeditTop_SendMessage(Status As TK.Domain.Enums.TicketStatus, HideToUser As Boolean) Handles CTRLeditTop.SendMessage

        Dim Preview As String = CTRLeditTop.PreviewMessage
        Dim Message As String = CTRLeditTop.HtmlMessage

        If Not String.IsNullOrEmpty(Preview) Then
            CTRLeditTop.ShowMessage("", Helpers.MessageType.none)
            Me.SendMessage(Message, Preview, Status, HideToUser)
        Else
            CTRLeditTop.ShowMessage(Resource.getValue("Message.EmptyMessage"), Helpers.MessageType.alert)
        End If
    End Sub

    Private Sub CTRLeditBottom_FileDelete(idAttachment As Long) Handles CTRLeditBottom.FileDelete
        Me.CurrentPresenter.DeleteFile(idAttachment, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath)
    End Sub

    Private Sub CTRLeditBottom_SendMessage(Status As TK.Domain.Enums.TicketStatus, HideToUser As Boolean) Handles CTRLeditBottom.SendMessage

        Dim Preview As String = CTRLeditBottom.PreviewMessage
        Dim Message As String = CTRLeditBottom.HtmlMessage

        If Not String.IsNullOrEmpty(Preview) Then
            CTRLeditBottom.ShowMessage("", Helpers.MessageType.none)
            Me.SendMessage(Message, Preview, Status, HideToUser)
        Else
            CTRLeditBottom.ShowMessage(Resource.getValue("Message.EmptyMessage"), Helpers.MessageType.alert)
        End If
    End Sub

    Private Sub CTRLeditTop_TicketChangeCondition(Condition As TK.Domain.Enums.TicketCondition, status As Boolean) Handles CTRLeditTop.TicketChangeCondition
        Me.CurrentPresenter.TicketChangeCondition(Condition, status)
    End Sub

    Private Sub CTRLeditBottom_TicketChangeCondition(Condition As TK.Domain.Enums.TicketCondition, status As Boolean) Handles CTRLeditBottom.TicketChangeCondition
        Me.CurrentPresenter.TicketChangeCondition(Condition, status)
    End Sub
    Private Sub LNBassignMe_Click(sender As Object, e As EventArgs) Handles LNBassignMe.Click
        Me.CurrentPresenter.AssignMe()
    End Sub

#End Region


    Private Sub LNBsaveMailSet_Click(sender As Object, e As EventArgs) Handles LNBsaveMailSet.Click
        Me.CurrentPresenter.SetnotificationSettings(Me.CTRLmailSettings.GetSettings())
    End Sub

    Public Function getExpandText() As String
        Return Me.Resource.getValue("ExpandCollapse.title")
    End Function

    Public Function getCatModifyTitle() As String
        Return Me.Resource.getValue("CatModify.Title")
    End Function



    'Private Sub EditResolver_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    '    PNLerrorMsg.Visible = Me.CTRLheadMessages.Visible
    'End Sub

    Private Sub LNBassCateUndo_Click(sender As Object, e As EventArgs) Handles LNBassCateUndo.Click
        Me.CurrentPresenter.InitView()
    End Sub

    Public Sub ShowAssignUsrError([Error] As TK.Domain.Enums.UserReassignError) Implements TK.Presentation.View.iViewTicketEditMan.ShowAssignUsrError
        PNLerrorMsg.Visible = True
        CTRLheadMessages.Visible = True

        If ([Error] = TK.Domain.Enums.UserReassignError.none) Then
            Me.CTRLheadMessages.InitializeControl( _
                   Resource.getValue("Message.AssUser.Changed"), _
                   lm.Comol.Core.DomainModel.Helpers.MessageType.success)
        Else
            Me.CTRLheadMessages.InitializeControl( _
                   Resource.getValue("Message.AssUser.notChange"), _
                   lm.Comol.Core.DomainModel.Helpers.MessageType.info)
        End If

    End Sub

    Private Sub EditResolver_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim itemCount As Integer = (If(LNBassignMe.Visible, 1, 0)) + _
        (If(LNBreassign.Visible, 1, 0)) + _
        (If(LNBreassignCom.Visible, 1, 0))

        TicketHelper.CheckDropDownButton(DVddbuttonListAssign, itemCount)

    End Sub
End Class