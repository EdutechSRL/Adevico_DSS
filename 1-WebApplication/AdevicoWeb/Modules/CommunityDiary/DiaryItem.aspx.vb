Imports lm.Comol.UI.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain

Partial Public Class DiaryItem
    Inherits PageBase
    Implements IviewDiaryItem

#Region "Context"
    Private _Presenter As EditDiaryItemPresenter
    Private ReadOnly Property CurrentPresenter() As EditDiaryItemPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditDiaryItemPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentIdEvent() As Long Implements IviewDiaryItem.CurrentIdEvent
        Get
            Return ViewStateOrDefault("CurrentIdEvent", 0)
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentIdEvent") = value
        End Set
    End Property
    Private Property IdModuleCommunityDiary() As Integer Implements IviewDiaryItem.IdModuleCommunityDiary
        Get
            Return ViewStateOrDefault("IdModuleCommunityDiary", -1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdModuleCommunityDiary") = value
        End Set
    End Property
    Private Property IdModuleRepository() As Integer Implements IviewDiaryItem.IdModuleRepository
        Get
            Return ViewStateOrDefault("IdModuleRepository", -1)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdModuleRepository") = value
        End Set
    End Property
    Private Property IdCommunityDiary() As Integer Implements IviewDiaryItem.IdCommunityDiary
        Get
            Return ViewStateOrDefault("IdCommunityDiary", -1)
        End Get
        Set(ByVal value As Integer)
            ViewState("IdCommunityDiary") = value
        End Set
    End Property
    Private ReadOnly Property UnknownUserTranslation As String Implements IviewDiaryItem.UnknownUserTranslation
        Get
            Return Resource.getValue("UnknownUserTranslation")
        End Get
    End Property
    Private Property RepositoryIdentifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IviewDiaryItem.RepositoryIdentifier
        Get
            Dim result As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier = Nothing
            If Not IsNothing(ViewState("RepositoryIdentifier")) Then
                Try
                    result = DirectCast(ViewState("RepositoryIdentifier"), lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
                Catch ex As Exception

                End Try
            End If
            Return result
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
            ViewState("RepositoryIdentifier") = value
        End Set
    End Property


    Private ReadOnly Property CurrentItem() As CommunityEventItem Implements IviewDiaryItem.CurrentItem
        Get
            Dim eventItem As New CommunityEventItem
            With eventItem
                .Id = CurrentIdItem
                .IdCommunityOwner = IdCommunityDiary
                .IsVisible = (Me.RBLvisibleTo.SelectedValue = True)
                .Title = Me.TXBtitle.Text
                .Link = Me.TXBlink.Text

                .Place = Me.TXBplace.Text
                .StartDate = Me.RDPday.SelectedDate
                .StartDate = .StartDate.AddHours(Me.DDLstartHour.SelectedValue)
                .StartDate = .StartDate.AddMinutes(Me.DDLstartMinute.SelectedValue)
                .EndDate = Me.RDPday.SelectedDate
                .EndDate = .EndDate.AddHours(Me.DDLendHour.SelectedValue)
                .EndDate = .EndDate.AddMinutes(Me.DDLendMinute.SelectedValue)
                If Me.RBLrepeat.SelectedValue = 0 Then
                    .ShowDateInfo = Me.CBXshowDataInfo.Checked
                Else
                    .ShowDateInfo = Me.CBXshowDataInfoWeek.Checked
                End If
                .Note = CleanText(CTRLvisualEditorNote.HTML)
                If String.IsNullOrWhiteSpace(.Note) Then
                    .Note = ""
                End If
                .NotePlain = CleanText(CTRLvisualEditorNote.Text)
                If String.IsNullOrWhiteSpace(.NotePlain) Then
                    .NotePlain = ""
                End If
            End With
            Return eventItem
        End Get
    End Property

    Private Property CurrentIdItem() As Long Implements IviewDiaryItem.CurrentIdItem
        Get
            Return ViewStateOrDefault("CurrentIdItem", 0)
        End Get
        Set(ByVal value As Long)
            ViewState("CurrentIdItem") = value
        End Set
    End Property
    Private ReadOnly Property PreloadIdCommunity() As Integer Implements IviewDiaryItem.PreloadIdCommunity
        Get
            Dim idCommunity As Long = 0
            Integer.TryParse(Request.QueryString("idCommunity"), idCommunity)
            Return idCommunity
        End Get
    End Property
    Private ReadOnly Property PreloadedIsInsertMode() As Boolean Implements IviewDiaryItem.PreloadIsInsertMode
        Get
            Dim result As Boolean = False
            Boolean.TryParse(Request.QueryString("InsertMode"), result)
            Return result AndAlso PreloadIdItem = 0
        End Get
    End Property
    Private ReadOnly Property PreloadIdItem() As Long Implements IviewDiaryItem.PreloadIdItem
        Get
            Dim idItem As Long = 0
            Long.TryParse(Request.QueryString("idItem"), idItem)
            Return idItem
        End Get
    End Property
    Private WriteOnly Property AllowEdit() As Boolean Implements IviewDiaryItem.AllowEdit
        Set(ByVal value As Boolean)
            Me.LNBsaveItem.Visible = value
        End Set
    End Property

    Private WriteOnly Property AllowFileManagement() As Boolean Implements IviewDiaryItem.AllowFileManagement
        Set(ByVal value As Boolean)
            DIVfiles.Visible = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView(PreloadIdCommunity, PreloadIdItem, UnknownUserTranslation, PreloadedIsInsertMode)
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_DiaryItem", "Modules", "CommunityDiary")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(Me.HYPbackToItems, True, True)
            .setLinkButton(Me.LNBsaveItem, True, True)
            .setLabel(LBtitle_t)
            .setLabel(LBvisibleTo_t)
            .setRadioButtonList(RBLvisibleTo, True)
            .setRadioButtonList(RBLvisibleTo, False)
            .setLabel(LBdate)
            .setLabel(LBfrom_t)
            .setLabel(LBto_t)
            .setButton(BTNsavedData, True, , , True)
            .setLabel(LBplace_t)
            .setLabel(LBlink_t)
            .setLabel(LBdescription_t)
            .setLabel(LBnote_t)
            .setLiteral(LTitemFiles_t)
            .setCheckBox(CBXshowDataInfo)
            .setLabel(LBrepeat_t)
            .setRadioButtonList(RBLrepeat, 0)
            .setRadioButtonList(RBLrepeat, 1)
            .setLabel(LBfromDate_t)
            .setLabel(LBtoDate_t)
            .setCheckBox(CBXshowDataInfoWeek)
            .setLabel(LBinfo)


            Me.DLGmoduleFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGmoduleFileItemVisibilityTitle")
            Me.DLGmoduleFileItemVisibility.DialogText = Me.Resource.getValue("DLGmoduleFileItemVisibilityText")


            Dim options As New List(Of String)
            options.Add(.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
            Me.DLGmoduleFileItemVisibility.InitializeControl(options, 0, True)
            options = New List(Of String)
            options.Add(.getValue("ModuleItemFileVisibilityStatus.VisibleForModule"))
            options.Add(.getValue("ModuleItemFileVisibilityStatus.Visible"))
            Me.DLGrepositoryFileItemVisibility.DialogTitle = Me.Resource.getValue("DLGrepositoryFileItemVisibilityTitle")
            Me.DLGrepositoryFileItemVisibility.DialogText = Me.Resource.getValue("DLGrepositoryFileItemVisibilityText")
            Me.DLGrepositoryFileItemVisibility.InitializeControl(options, 0, True)

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Messages"
    Private Sub ShowNoItemWithThisID(ByVal idCommunity As Integer, ByVal idModule As Integer, ByVal idItem As Long) Implements IviewDiaryItem.ShowNoItemWithThisID
        MLVitemData.SetActiveView(VIWempty)
        CTRLmessages.InitializeControl(Resource.getValue("UnkownDiaryItem"), Helpers.MessageType.error)
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleCommunityDiary.ActionType.UnkownDiaryItem, PageUtility.CreateObjectsList(idModule, ModuleCommunityDiary.ObjectType.DiaryItem, idItem), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Private Sub NoPermission(ByVal idCommunity As Integer, ByVal idModule As Integer) Implements IviewDiaryItem.NoPermission
        Me.BindNoPermessi()
        PageUtility.AddActionToModule(idCommunity, idModule, ModuleCommunityDiary.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplaySessionTimeout(idCommunity As Integer, Optional ByVal idItem As Long = 0) Implements IviewDiaryItem.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.EditDiaryItem(idCommunity, idItem)
        webPost.Redirect(dto)
    End Sub
#End Region


    Private Sub UpdateItemData(ByVal StartDate As DateTime, ByVal EndDate As DateTime) Implements IviewDiaryItem.UpdateItemData
        Me.RDPday.SelectedDate = StartDate
        Me.DDLstartHour.SelectedValue = StartDate.Hour
        Dim sMinute As Integer = StartDate.Minute
        If sMinute > 0 AndAlso sMinute <= 15 Then
            sMinute = 15
        ElseIf sMinute > 15 AndAlso sMinute <= 30 Then
            sMinute = 30
        ElseIf sMinute > 30 AndAlso sMinute <= 45 Then
            sMinute = 45
        Else
            sMinute = 0
        End If
        Me.DDLstartMinute.SelectedValue = sMinute
        Me.DDLendHour.SelectedValue = EndDate.Hour

        Dim eMinute As Integer = EndDate.Minute
        If eMinute > 0 AndAlso eMinute <= 15 Then
            eMinute = 15
        ElseIf eMinute > 15 AndAlso eMinute <= 30 Then
            eMinute = 30
        ElseIf eMinute > 30 AndAlso eMinute <= 45 Then
            eMinute = 45
        Else
            eMinute = 0
        End If
        Me.DDLendMinute.SelectedValue = eMinute
        Me.RDPstartDay.SelectedDate = StartDate
        Me.RDPendDay.SelectedDate = EndDate
    End Sub

    Private Sub LoadItem(ByVal item As CommunityEventItem, ByVal description As String, ByVal communityName As String, attachments As List(Of lm.Comol.Core.Events.Domain.dtoAttachmentItem)) Implements IviewDiaryItem.LoadItem
        With item
            Me.TXBplace.Text = .Place
            Me.TXBtitle.Text = .Title
            Me.TXBlink.Text = .Link
            Me.CBXshowDataInfo.Checked = .ShowDateInfo
            Me.CBXshowDataInfoWeek.Checked = .ShowDateInfo
            Me.RBLvisibleTo.SelectedValue = .IsVisible

            UpdateItemData(item.StartDate, item.EndDate)

            If item.Id = 0 Then
                If item.IdCommunityOwner <> PageUtility.CurrentContext.UserContext.CurrentCommunityID Then
                    Master.ServiceTitle = String.Format(Resource.getValue("serviceAddTitle"), communityName)
                Else
                    Master.ServiceTitle = Resource.getValue("serviceAddTitle.Current")
                End If

                Me.Master.ServiceNopermission = Resource.getValue("nopermissionAdd")
                Me.DIVeditData.Visible = True
                Me.DIVinsertData.Visible = True
                Me.DIVfiles.Visible = False
                Me.DIVweekly.Visible = False
                Me.RBLrepeat.SelectedValue = 0
                Me.BTNsavedData.Visible = False
                Me.InitializeWeekPanel()
            Else
                Me.BTNsavedData.Visible = True
                Me.DIVeditData.Visible = True
                Me.DIVinsertData.Visible = False
                Me.DIVfiles.Visible = True
                If String.IsNullOrEmpty(item.Title) AndAlso item.IdCommunityOwner <> PageUtility.CurrentContext.UserContext.CurrentCommunityID Then
                    Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceEditTitle"), communityName)
                ElseIf Not String.IsNullOrEmpty(item.Title) Then
                    Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceItemTitle"), item.Title)
                Else
                    Me.Master.ServiceTitle = String.Format(Resource.getValue("serviceItemTitle"), "")
                End If
            End If
            Me.CTRLvisualEditorNote.HTML = .Note
        End With
        Me.CTRLvisualEditorDescription.HTML = description
        Me.UDPdata.Update()
        LoadAttachments(attachments)
    End Sub
    Private Sub LoadAttachments(attachments As List(Of lm.Comol.Core.Events.Domain.dtoAttachmentItem)) Implements IviewDiaryItem.LoadAttachments
        If Not IsNothing(attachments) AndAlso attachments.Any Then
            _DisplayManagerTable = attachments.Any(Function(a) a.Permissions.Delete OrElse a.Permissions.Edit OrElse a.Permissions.EditRepositoryVisibility OrElse a.Permissions.EditVisibility)
            _DisplayVisibilityColumn = attachments.Any(Function(i) i.Permissions.Edit OrElse i.Permissions.EditVisibility OrElse i.Permissions.EditRepositoryVisibility)
            _DisplayActionsColumn = attachments.Any(Function(i) i.Permissions.Edit OrElse i.Permissions.Download OrElse i.Permissions.Play)
            RPTitemFiles.DataSource = attachments
            RPTitemFiles.DataBind()
        Else
            RPTitemFiles.Visible = False
        End If
    End Sub

    Private Sub InitializeAttachmentsControl(idEvent As Long, idEventItem As Long, actions As List(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions), dAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Implements IviewDiaryItem.InitializeAttachmentsControl
        If Not IsNothing(actions) AndAlso actions.Any Then
            CTRLattachmentsHeader.Visible = True
            CTRLattachmentsHeader.InitializeControlForJQuery(actions, actions.ToDictionary(Of lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, String)(Function(a) a, Function(a) Resource.getValue("dialogTitle.RepositoryAttachmentUploadActions." & a.ToString)))
            CTRLcommandsTop.Visible = True
            CTRLattachmentsCommandsBottom.Visible = True
            CTRLcommandsTop.InitializeControlForJQuery(actions, dAction)
            CTRLattachmentsCommandsBottom.InitializeControlForJQuery(actions, dAction)
            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem) Then
                CTRLinternalUpload.Visible = True
                CTRLinternalUpload.InitializeControl(idEvent, idEventItem, 0, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem.ToString))
            End If
            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity) Then
                CTRLlinkFromCommunity.Visible = True
                CTRLlinkFromCommunity.InitializeControl(idEvent, idEventItem, 0, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity.ToString))
            End If
            If actions.Contains(lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity) Then
                CTRLcommunityUpload.Visible = True
                CTRLcommunityUpload.InitializeControl(idEvent, idEventItem, 0, lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity, RepositoryIdentifier, Resource.getValue("dialogDescription.RepositoryAttachmentUploadActions." & lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity.ToString))
            End If

        Else
            CTRLattachmentsHeader.Visible = False
            CTRLcommandsTop.Visible = False
            CTRLattachmentsCommandsBottom.Visible = False
        End If
    End Sub
    Private Sub SendToItemsList(ByVal idCommunity As Integer, ByVal goToIdItem As Long) Implements IviewDiaryItem.SendToItemsList
        PageUtility.RedirectToUrl(RootObject.CommunityDiary(idCommunity, goToIdItem))
    End Sub
    Private Sub SetBackToDiary(ByVal idCommunity As Integer, ByVal idItem As Long) Implements IviewDiaryItem.SetBackToDiary
        HYPbackToItems.Visible = (idCommunity <> -1)
        If idItem <= 0 Then
            HYPbackToItems.NavigateUrl = Me.BaseUrl & RootObject.CommunityDiary(idCommunity)
        Else
            HYPbackToItems.NavigateUrl = Me.BaseUrl & RootObject.CommunityDiary(idCommunity, idItem)
        End If
    End Sub
    Private Function GetPortalNameTranslation() As String Implements IviewDiaryItem.GetPortalNameTranslation
        Return Resource.getValue("PortalHome")
    End Function
#End Region

#Region "Internal"
#Region "Date selection"
    Private Sub InitializeWeekPanel()
        Dim oList As New List(Of dtoWeekDay)
        For i As Integer = 1 To 6
            oList.Add(New dtoWeekDay() With {.Selected = False, .DayName = WeekdayName(i, True, Microsoft.VisualBasic.FirstDayOfWeek.Monday), .DayNumber = i, .StartHour = 8, .EndHour = 11})
        Next
        oList.Add(New dtoWeekDay() With {.Selected = False, .DayName = WeekdayName(7, True, Microsoft.VisualBasic.FirstDayOfWeek.Monday), .DayNumber = 0, .StartHour = 8, .EndHour = 11})

        Me.RPTweek.DataSource = oList
        Me.RPTweek.DataBind()
    End Sub
    Private Sub RBLrepeat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLrepeat.SelectedIndexChanged
        If Me.RBLrepeat.SelectedValue = 0 Then
            Me.DIVeditData.Visible = True
            Me.DIVweekly.Visible = False
        Else
            Me.DIVweekly.Visible = True
            Me.DIVeditData.Visible = False
        End If
        UDPdata.Update()
    End Sub
    Private Sub RPTweek_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTweek.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoWeekDay = DirectCast(e.Item.DataItem, dtoWeekDay)

            Dim oCheckbox As CheckBox = e.Item.FindControl("CBXday")
            oCheckbox.Checked = oDto.Selected
            Dim oLabel As Label = e.Item.FindControl("LBday")
            oLabel.Text = oDto.DayName
            oLabel = e.Item.FindControl("LBfromHour_t")
            Me.Resource.setLabel(oLabel)
            Dim oDropDownList As DropDownList = e.Item.FindControl("DDLstartHour")
            oDropDownList.SelectedValue = oDto.StartHour
            oDropDownList = e.Item.FindControl("DDLstartMinute")
            oDropDownList.SelectedValue = oDto.StartMinutes

            oLabel = e.Item.FindControl("LBtoHour_t")
            Me.Resource.setLabel(oLabel)
            oDropDownList = e.Item.FindControl("DDLendHour")
            oDropDownList.SelectedValue = oDto.EndHour
            oDropDownList = e.Item.FindControl("DDLendMinute")
            oDropDownList.SelectedValue = oDto.EndMinutes

            Dim oLiteral As Literal = e.Item.FindControl("LTdaynumber")
            oLiteral.Text = oDto.DayNumber
        End If
    End Sub
    Private Function GetSelectedWeekDay() As List(Of dtoWeekDay)
        Dim oList As New List(Of dtoWeekDay)

        For Each oRow As RepeaterItem In (From i As RepeaterItem In Me.RPTweek.Items Where i.ItemType = ListItemType.Item OrElse i.ItemType = ListItemType.AlternatingItem Select i).ToList
            Dim oCheckbox As CheckBox = oRow.FindControl("CBXday")
            If oCheckbox.Checked Then
                Dim oWeek As New dtoWeekDay
                oWeek.Selected = True

                Dim oLiteral As Literal = oRow.FindControl("LTdaynumber")
                oWeek.DayNumber = oLiteral.Text

                Dim oDropDownList As DropDownList = oRow.FindControl("DDLstartHour")
                oWeek.StartHour = oDropDownList.SelectedValue
                oDropDownList = oRow.FindControl("DDLstartMinute")
                oWeek.StartMinutes = oDropDownList.SelectedValue
                oDropDownList = oRow.FindControl("DDLendHour")
                oWeek.EndHour = oDropDownList.SelectedValue
                oDropDownList = oRow.FindControl("DDLendMinute")
                oWeek.EndMinutes = oDropDownList.SelectedValue

                oList.Add(oWeek)
            End If
        Next
        Return oList
    End Function
#End Region

#Region "Attachments"
    Protected Sub RPTitemFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitemFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim attachmentItem As lm.Comol.Core.Events.Domain.dtoAttachmentItem = TryCast(e.Item.DataItem, lm.Comol.Core.Events.Domain.dtoAttachmentItem)

            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = attachmentItem.Attachment.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            Dim actions As List(Of dtoModuleActionControl)
            'initializer.OnModalPage
            '  initializer.OpenLinkCssClass

            Dim requiredActions As lm.Comol.Core.ModuleLinks.DisplayActionMode = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            If attachmentItem.Permissions.Edit Then
                requiredActions = requiredActions Or lm.Comol.Core.ModuleLinks.DisplayActionMode.adminMode
            Else
                requiredActions = requiredActions Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            End If
            actions = renderItem.InitializeRemoteControl(initializer, StandardActionType.Play, requiredActions)

            Dim isReadyToPlay As Boolean = (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.available)
            Dim isReadyToManage As Boolean = isReadyToPlay OrElse (renderItem.Availability = lm.Comol.Core.FileRepository.Domain.ItemAvailability.waitingsettings)
            Dim oHyperlink As HyperLink

            '<asp:HyperLink ID="HYPdownload" runat="server" Text="D" CssClass="icon download" Visible="false" />
            '                                                <asp:HyperLink ID="HYPstats" runat="server" Text="S" CssClass="icon stats" Visible="false" />
            '                                                <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="M" CssClass="icon editmetadata" Visible="false" />
            If attachmentItem.Permissions.Download AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.DownloadItem).Any Then
                oHyperlink = e.Item.FindControl("HYPdownload")
                oHyperlink.Visible = True
                oHyperlink.ToolTip = Resource.getValue("Download.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.DownloadItem).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If

            If isReadyToPlay AndAlso attachmentItem.Permissions.ViewOtherStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Any Then
                oHyperlink = e.Item.FindControl("HYPstats")
                oHyperlink.Visible = True
                oHyperlink.ToolTip = Resource.getValue("statistic.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewAdvancedStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
            ElseIf isReadyToPlay AndAlso attachmentItem.Permissions.ViewMyStatistics AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Any Then
                oHyperlink = e.Item.FindControl("HYPstats")
                oHyperlink.ToolTip = Resource.getValue("statistic.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.ViewUserStatistics).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If
            If isReadyToManage AndAlso attachmentItem.Permissions.SetMetadata AndAlso actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Any Then
                oHyperlink = e.Item.FindControl("HYPeditMetadata")
                oHyperlink.ToolTip = Resource.getValue("settings.ItemType." & renderItem.ItemType.ToString)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = actions.Where(Function(a) a.ControlType = StandardActionType.EditMetadata).Select(Function(a) a.LinkUrl).FirstOrDefault
            End If
            Dim oLiteral As Literal = Nothing
            Dim oCell As HtmlControls.HtmlTableCell = e.Item.FindControl("TDvisibilityLesson")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("TDvisibilityRepository")
            oCell.Visible = _DisplayVisibilityColumn
            If _DisplayVisibilityColumn Then
                oLiteral = e.Item.FindControl("LTvisibleForItem")
                If attachmentItem.Attachment.IsVisible Then
                    oLiteral.Text = Replace(LTtemplateVisibile.Text, "#visibility#", Resource.getValue("Attachment.Visible"))
                Else
                    oLiteral.Text = Replace(LTtemplateNotVisibile.Text, "#visibility#", Resource.getValue("Attachment.NotVisible"))
                End If
                oLiteral = e.Item.FindControl("LTvisibleForRepository")
                If IsNothing(attachmentItem.Attachment.File) OrElse attachmentItem.Attachment.File.IsInternal Then
                    oLiteral.Text = Replace(LTtemplateIgnore.Text, "#visibility#", "//")
                ElseIf attachmentItem.Attachment.File.IsVisible Then
                    oLiteral.Text = Replace(LTtemplateVisibile.Text, "#visibility#", Resource.getValue("Attachment.Visible"))
                Else
                    oLiteral.Text = Replace(LTtemplateNotVisibile.Text, "#visibility#", Resource.getValue("Attachment.NotVisible"))
                End If
            End If

            oCell = e.Item.FindControl("TDactions")
            oCell.Visible = _DisplayActionsColumn


            If Not IsNothing(attachmentItem.Attachment.File) AndAlso (attachmentItem.Permissions.EditRepositoryVisibility OrElse attachmentItem.Permissions.EditVisibility) Then
                Dim oDialogHide As MyUC.DialogLinkButton = e.Item.FindControl("LNBhide")
                Me.Resource.setLinkButton(oDialogHide, True, True)
                Dim fileStatus As ModuleItemFileVisibilityStatus = GetVisibilityStatus(attachmentItem.Attachment.File, attachmentItem.Attachment.IsVisible)

                oDialogHide.CommandArgument = attachmentItem.Attachment.Id.ToString & "," & attachmentItem.Attachment.IdEventItem.ToString
                Dim selectedVisibility As New List(Of Integer)
                If attachmentItem.Attachment.File.IsInternal OrElse Not attachmentItem.Permissions.EditRepositoryVisibility Then
                    If attachmentItem.Attachment.IsVisible Then
                        selectedVisibility.Add(0)
                    End If
                    oDialogHide.Visible = attachmentItem.Permissions.EditVisibility
                    oDialogHide.CommandName = "editModuleVisibility"
                    oDialogHide.InitializeMultiSelectControlByClass("moduleItemVisibility", selectedVisibility)
                Else
                    If attachmentItem.Attachment.IsVisible Then
                        selectedVisibility.Add(0)
                    End If
                    If attachmentItem.Attachment.File.IsVisible Then
                        selectedVisibility.Add(1)
                    End If
                    oDialogHide.Visible = attachmentItem.Permissions.EditRepositoryVisibility
                    oDialogHide.CommandName = "editCommunityVisibility"
                    oDialogHide.InitializeMultiSelectControlByClass("repositoryItemVisibility", selectedVisibility)
                End If
            End If

            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBvirtualDeleteAttachment")
            oLinkButton.Visible = attachmentItem.Permissions.VirtualDelete

            oLinkButton = e.Item.FindControl("LNBrecoverAttachment")
            oLinkButton.Visible = attachmentItem.Permissions.UnDelete
            oLinkButton = e.Item.FindControl("LNBdeleteAttachment")
            oLinkButton.Visible = attachmentItem.Permissions.Delete OrElse IsNothing(attachmentItem.Attachment.File) OrElse Not attachmentItem.Attachment.File.IsValid
            oLinkButton = e.Item.FindControl("LNBunlinkAttachment")
            oLinkButton.Visible = attachmentItem.Permissions.Unlink
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTvisibilityLessonHeader")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTvisibilityRepositoryHeader")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTattachmentactionsHeader")
            Resource.setLiteral(oLiteral)

            Dim oCell As HtmlControls.HtmlTableCell = e.Item.FindControl("THvisibilityLesson")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("THvisibilityRepository")
            oCell.Visible = _DisplayVisibilityColumn
            oCell = e.Item.FindControl("THactions")
            oCell.Visible = _DisplayActionsColumn
        End If
    End Sub
    Private Sub RPTitemFiles_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTitemFiles.ItemCommand
        Dim idAttachment As Long = 0
        Long.TryParse(e.CommandArgument, idAttachment)
        If idAttachment > 0 Then
            Select Case e.CommandName
                Case "delete"
                    CurrentPresenter.PhisicalDelete(CurrentIdItem, idAttachment, IdCommunityDiary, PageUtility.GetRepositoryDiskPath, PageUtility.GetRepositoryThumbnailDiskPath, UnknownUserTranslation)
                Case "virtualdelete"
                    CurrentPresenter.VirtualDeleteUndelete(CurrentIdItem, idAttachment, IdCommunityDiary, UnknownUserTranslation, True)
                Case "recover"
                    CurrentPresenter.VirtualDeleteUndelete(CurrentIdItem, idAttachment, IdCommunityDiary, UnknownUserTranslation, False)
                Case "unlink"
                    CurrentPresenter.UnlinkRepositoryItem(CurrentIdItem, idAttachment, IdCommunityDiary, UnknownUserTranslation)
            End Select
        End If
    End Sub
    Private Function GetVisibilityStatus(ByVal item As lm.Comol.Core.FileRepository.Domain.RepositoryItemObject, ByVal isVisible As Boolean) As ModuleItemFileVisibilityStatus
        Dim iResponse As ModuleItemFileVisibilityStatus
        If item.IsInternal Then
            iResponse = IIf(isVisible, ModuleItemFileVisibilityStatus.VisibleForModule, ModuleItemFileVisibilityStatus.HiddenForModule)
        Else
            If isVisible AndAlso item.IsVisible Then
                iResponse = (ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity)
            ElseIf Not isVisible AndAlso Not item.IsVisible Then
                iResponse = (ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity)
            ElseIf isVisible AndAlso Not item.IsVisible Then
                iResponse = ModuleItemFileVisibilityStatus.VisibleForModule Or ModuleItemFileVisibilityStatus.HiddenForCommunity
            ElseIf Not isVisible Then
                iResponse = ModuleItemFileVisibilityStatus.HiddenForModule Or ModuleItemFileVisibilityStatus.VisibleForCommunity
            End If
        End If
        Return iResponse
    End Function
    Private Sub DLGrepositoryFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As System.Collections.Generic.IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGrepositoryFileItemVisibility.ButtonPressedMulti
        Dim idEventItem As Long = 0
        Dim idItemFile As Long = 0
        Long.TryParse(CommandArgument.Split(",")(1), idEventItem)
        Long.TryParse(CommandArgument.Split(",")(0), idItemFile)

        Dim iResponse As ModuleItemFileVisibilityStatus
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            Exit Sub
        Else
            CurrentPresenter.EditAttachmentVisibility(idEventItem, idItemFile, IdCommunityDiary, dialogResults.Contains(0), dialogResults.Contains(1), UnknownUserTranslation)
        End If
    End Sub
    Private Sub DLGmoduleFileItemVisibility_ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String) Handles DLGmoduleFileItemVisibility.ButtonPressedMulti
        Dim idEventItem As Long = 0
        Dim idItemFile As Long = 0
        Long.TryParse(CommandArgument.Split(",")(1), idEventItem)
        Long.TryParse(CommandArgument.Split(",")(0), idItemFile)
        If dialogResult < 0 AndAlso dialogResult <> -3 AndAlso dialogResult <> -1 Then
            '    CurrentPresenter.LoadDiaryItems(IdCommunityDiary, UnknownUserTranslation)
            Exit Sub
        Else
            CurrentPresenter.EditAttachmentVisibility(idEventItem, idItemFile, IdCommunityDiary, dialogResults.Contains(0), False, UnknownUserTranslation)
        End If
    End Sub
    Public Function GetAttachmentsCssClass() As String
        If _DisplayManagerTable Then
            Return " asmanager"
        Else
            Return " asuser"
        End If
    End Function

    Private _DisplayManagerTable As Boolean
    Private _DisplayVisibilityColumn As Boolean
    Private _DisplayActionsColumn As Boolean
#End Region

#Region "Add Controls"
    Private Sub CTRLaddUrls_ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.ItemsAdded, CTRLinternalUpload.ItemsAdded, CTRLlinkFromCommunity.ItemsAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsAdded.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.success)
        CurrentPresenter.ReloadAttachments(CurrentIdItem, IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLaddUrls_ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.ItemsNotAdded, CTRLinternalUpload.ItemsNotAdded, CTRLlinkFromCommunity.ItemsNotAdded
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("ItemsNotAdded.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.error)
        CurrentPresenter.ReloadAttachments(CurrentIdItem, IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLadd_NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions) Handles CTRLcommunityUpload.NoFilesToAdd, CTRLinternalUpload.NoFilesToAdd, CTRLlinkFromCommunity.NoFilesToAdd
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("NoFilesToAdd.RepositoryAttachmentUploadActions." & action.ToString), Helpers.MessageType.alert)
        CurrentPresenter.ReloadAttachments(CurrentIdItem, IdCommunityDiary, UnknownUserTranslation)
    End Sub
    Private Sub CTRLadd_WorkingSessionExpired() Handles CTRLcommunityUpload.WorkingSessionExpired, CTRLinternalUpload.WorkingSessionExpired, CTRLlinkFromCommunity.WorkingSessionExpired
        Master.ClearOpenedDialogOnPostback()
        Me.DisplaySessionTimeout(IdCommunityDiary)
    End Sub
    Private Sub CTRLadd_EventItemNotFound() Handles CTRLcommunityUpload.EventItemNotFound, CTRLinternalUpload.EventItemNotFound, CTRLlinkFromCommunity.EventItemNotFound
        Master.ClearOpenedDialogOnPostback()
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayEventItemNotFound.Attachments"), Helpers.MessageType.error)
        CurrentPresenter.ReloadAttachments(CurrentIdItem, IdCommunityDiary, UnknownUserTranslation)
    End Sub
#End Region
    Private Function CleanText(text As String) As String
        If String.IsNullOrWhiteSpace(text) Then
            Return ""
        End If
        text = Replace(text, "<script>", "&lt;script&gt;")
        text = Replace(text, "</script>", "&lt;/script&gt;")
        text = Replace(text, "<script", "&lt; script")
        text = Replace(text, "</script", "&lt;script")
        text = Replace(text, "</pre>", "")

        If InStr(text, "<pre") > 0 Then
            Dim startPos, endPos As Integer

            While InStr(text, "<pre") > 0
                startPos = InStr(text, "<pre")
                endPos = InStr(startPos, text, ">")
                If startPos > 0 And endPos > 0 Then
                    text = text.Remove(startPos - 1, endPos - startPos + 1)
                End If
            End While
        End If
        Return text

    End Function

    Private Sub LNBsaveItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveItem.Click
        If CurrentIdItem = 0 AndAlso Me.RBLrepeat.SelectedValue = 1 Then
            Dim Selected As List(Of dtoWeekDay) = Me.GetSelectedWeekDay()
            If Selected.Count = 0 OrElse Me.RDPendDay.SelectedDate < Me.RDPstartDay.SelectedDate Then
                Exit Sub
            Else
                CurrentPresenter.SaveItem(IdCommunityDiary, CurrentIdItem, CurrentItem, RDPstartDay.SelectedDate, RDPendDay.SelectedDate, Selected, CleanText(CTRLvisualEditorDescription.HTML), CleanText(CTRLvisualEditorDescription.Text))
            End If
        Else
            CurrentPresenter.SaveItem(IdCommunityDiary, CurrentIdItem, CurrentItem, CleanText(CTRLvisualEditorDescription.HTML), CleanText(CTRLvisualEditorDescription.Text))
        End If
    End Sub

    Private Sub BTNsavedData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsavedData.Click
        CurrentPresenter.GetSavedItemData(IdCommunityDiary, CurrentIdItem)
        UDPdata.Update()
    End Sub
#End Region



    'Public Sub LoadFilesToManage(ByVal ItemID As Long, ByVal oPermission As CoreItemPermission, ByVal files As IList(Of iCoreItemFileLink(Of Long)), ByVal urlToPublish As String) Implements IviewDiaryItem.LoadFilesToManage
    '    Me.CTRLItemManagementFile.Visible = True
    '    Me.CTRLItemManagementFile.ShowManagementButtons = oPermission.AllowEdit
    '    Me.CTRLItemManagementFile.InitalizeControl(ItemID, oPermission, files, urlToPublish)
    'End Sub









#Region "Management File"

    'Private Sub CTRLItemManagementFile_EditFileItemVisibility(ByVal ItemID As Long, ByVal ItemLinkId As Long, ByVal visibleForModule As Boolean, ByVal visibleForRepository As Boolean) Handles CTRLItemManagementFile.EditFileItemVisibility
    '    CurrentPresenter.EditFileItemVisibility(ItemID, ItemLinkId, visibleForModule, visibleForRepository)
    'End Sub

    'Private Sub CTRLItemManagementFile_PhysicalDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.PhysicalDelete
    '    Dim cacheKey As String = "CommunityRepositorySize_" & Me.ItemCommunityID
    '    Dim CommunityPath As String = ""
    '    If Me.SystemSettings.File.Materiale.DrivePath = "" Then
    '        CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
    '    Else
    '        CommunityPath = Me.SystemSettings.File.Materiale.DrivePath
    '    End If

    '    GenericCacheManager.PurgeCacheItems(cacheKey)
    '    Me.CurrentPresenter.PhisicalDelete(ItemID, ItemLinkId, CommunityPath)
    'End Sub

    'Private Sub CTRLItemManagementFile_UnDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.UnDelete
    '    CurrentPresenter.VirtualUndelete(ItemID, ItemLinkId)
    'End Sub

    'Private Sub CTRLItemManagementFile_UnlinkRepositoryItem(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.UnlinkRepositoryItem
    '    CurrentPresenter.UnlinkRepositoryItem(ItemID, ItemLinkId)
    'End Sub

    'Private Sub CTRLItemManagementFile_VirtualDelete(ByVal ItemID As Long, ByVal ItemLinkId As Long) Handles CTRLItemManagementFile.VirtualDelete
    '    CurrentPresenter.VirtualDelete(ItemID, ItemLinkId)
    'End Sub
#End Region

    Private Sub Page_PreInit1(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub

   
End Class