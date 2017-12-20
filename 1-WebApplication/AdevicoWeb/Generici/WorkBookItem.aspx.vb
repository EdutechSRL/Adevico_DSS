Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Partial Public Class WorkBookItem
    Inherits PageBase
    Implements IviewWorkBookItem



#Region "View"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _CommunityRepositoryPermission As List(Of ModuleCommunityRepository)
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As WorkBookItemPresenter

    Protected ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CommunitiesPermission() As IList(Of WorkBookCommunityPermission) Implements IviewWorkBookItem.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                _CommunitiesPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex) _
                                          Select New WorkBookCommunityPermission() With {.ID = sb.CommunityID, .Permissions = New ModuleWorkBook(New Services_WorkBook(sb.PermissionString))}).ToList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.WorkBookItemPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WorkBookItemPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property AllowStatusChange() As Boolean Implements IviewWorkBookItem.AllowStatusChange
        Get
            Return Me.DDLstatus.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.DDLstatus.Visible = value
            Me.LBverificato.Visible = Not value
        End Set
    End Property
    Public WriteOnly Property AllowEdit() As Boolean Implements IviewWorkBookItem.AllowEdit
        Set(ByVal value As Boolean)
            Me.LNBsaveItem.Visible = value
            Me.TXBtitle.ReadOnly = Not value
            Me.CTRLeditorNote.IsEnabled = value
            Me.CTRLeditor.IsEnabled = value
            Me.RDPstartDate.Enabled = value
        End Set
    End Property
    Public WriteOnly Property AllowFileManagement() As Boolean Implements IviewWorkBookItem.AllowFileManagement
        Set(ByVal value As Boolean)
            Me.LNBgoToFileManagement.Visible = value
            Me.LNBgoToFileManagementBottom.Visible = value
        End Set
    End Property
    Public WriteOnly Property AllowEditingChanging() As Boolean Implements IviewWorkBookItem.AllowEditingChanging
        Set(ByVal value As Boolean)
            Me.DDLediting.Visible = value
            Me.LBediting.Visible = Not value
        End Set
    End Property
    Public ReadOnly Property PreloadedIsInsertMode() As Boolean Implements IviewWorkBookItem.PreloadedIsInsertMode
        Get
            Return (Request.QueryString("Action") = "Add")
        End Get
    End Property
    Public ReadOnly Property PreloadedItemID() As System.Guid Implements IviewWorkBookItem.PreloadedItemID
        Get
            Dim UrlID As String = Request.QueryString("ItemID")
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public ReadOnly Property PreloadedWorkBookID() As System.Guid Implements IviewWorkBookItem.PreloadedWorkBookID
        Get
            Dim WorkBookID As String = Request.QueryString("WorkBookID")
            If Not WorkBookID = "" Then
                Try
                    Return New System.Guid(WorkBookID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public ReadOnly Property PreviousWorkBookView() As WorkBookTypeFilter Implements IviewWorkBookItem.PreviousWorkBookView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
    Public WriteOnly Property ShowDraftMode() As Boolean Implements IviewWorkBookItem.ShowDraftMode
        Set(ByVal value As Boolean)
            Me.DIVdraft.Visible = value
            Me.DIVdraft.Style.Add("display", IIf(value, "block", "none"))
        End Set
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository Implements IviewWorkBookItem.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                       Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
        End If
        If IsNothing(oModule) Then
            oModule = New ModuleCommunityRepository
        End If
        Return oModule
    End Function
    Public ReadOnly Property AllowPublish() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookItem.AllowPublish
        Get
            If IsNothing(_CommunityRepositoryPermission) OrElse _CommunityRepositoryPermission.Count = 0 Then
                _CommunityRepositoryPermission = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).ToList
            End If
            Return (From sb In _CommunityRepositoryPermission Where sb.Administration OrElse sb.UploadFile).Any
        End Get
    End Property
    Public ReadOnly Property CurrentItem() As lm.Comol.Modules.Base.DomainModel.WorkBookItem Implements IviewWorkBookItem.CurrentItem
        Get
            Dim oWorkBookItem As New lm.Comol.Modules.Base.DomainModel.WorkBookItem

            oWorkBookItem.Title = Me.TXBtitle.Text
            oWorkBookItem.Body = Me.CTRLeditor.HTML
            oWorkBookItem.Note = Me.CTRLeditorNote.HTML
            oWorkBookItem.isDraft = Me.CBXdraft.Checked
            If Me.DDLstatus.SelectedIndex >= 0 Then
                oWorkBookItem.Status = New WorkBookStatus(Me.DDLstatus.SelectedValue)
            Else
                oWorkBookItem.Status = Nothing
            End If
            If Me.DDLediting.SelectedIndex >= 0 Then
                oWorkBookItem.Editing = Me.DDLediting.SelectedValue
            Else
                oWorkBookItem.Editing = EditingPermission.Authors
            End If
            oWorkBookItem.StartDate = Me.RDPstartDate.SelectedDate
            oWorkBookItem.EndDate = Me.RDPstartDate.SelectedDate
            Return oWorkBookItem
        End Get
    End Property
#End Region
#Region "Default"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Default"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Me.Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        Else
            Me.CurrentPresenter.ReloadManagementFileView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookItem", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            If Me.PreloadedIsInsertMode Then
                Me.Master.ServiceTitle = .getValue("serviceTitleAdd")
            Else
                Me.Master.ServiceTitle = .getValue("serviceTitleEdit")
            End If

            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBtext)
            .setLabel(Me.LBtitle)
            .setLabel(Me.LBnote)
            .setLabel(Me.LBsceltaGiorno)
            .setLabel(Me.LBverificato_t)
            .setLabel(Me.LBowner_t)
            .setLabel(Me.LBdraft_t)
            .setLinkButton(Me.LNBgoToFileManagement, True, True)
            .setLinkButton(Me.LNBsaveItem, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
            .setHyperLink(Me.HYPgoToFileManagement, True, True)
            .setCheckBox(CBXdraft)
            .setLiteral(LTitemFiles_t)
            .setLabel(LBnoItem)
            .setLinkButton(Me.LNBgoToFileManagementBottom, True, True)
            .setLabel(LBediting_t)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub LoadFilesToManage(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal AllowManagement As Boolean, ByVal oPermission As lm.Comol.Modules.Base.DomainModel.WorkBookItemPermission, ByVal oModule As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository, ByVal AllowPublish As Boolean) Implements IviewWorkBookItem.LoadFilesToManage
        Me.CTRLmanagementFile.Visible = True
        Me.CTRLmanagementFile.InitializeControl(ItemID, CommunityID, True, AllowManagement, True, oPermission, oModule, AllowPublish)
    End Sub

    Public Sub LoadItem(ByVal oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer))) Implements IviewWorkBookItem.LoadItem
        Me.LBmetadata.Text = ""
        Try
            Me.DDLstatus.Items.Clear()
            Me.DDLstatus.DataSource = oAvailableStatus
            Me.DDLstatus.DataTextField = "Translation"
            Me.DDLstatus.DataValueField = "Id"
            Me.DDLstatus.DataBind()
            Me.DDLstatus.SelectedValue = oItem.Status.Id
        Catch ex As Exception

        End Try
        If Me.DDLstatus.SelectedIndex >= 0 Then
            Me.LBverificato.Text = Me.DDLstatus.SelectedItem.Text
        End If

        If oItem.Title = "" AndAlso Me.PreloadedIsInsertMode Then
            Me.TXBtitle.Text = String.Format(Me.Resource.getValue("defaultTitle"), oItem.StartDate.ToShortDateString)
        Else
            Me.TXBtitle.Text = oItem.Title
        End If
        Me.CTRLeditorNote.HTML = oItem.Note
        Me.CTRLeditor.HTML = oItem.Body
        Me.RDPstartDate.SelectedDate = oItem.StartDate

        Me.LBmetadata.Text = "<br>"
        If oItem.Id = System.Guid.Empty Then
            Me.LBowner.Text = String.Format(Me.Resource.getValue("Creatingowner"), oItem.CreatedBy.SurnameAndName)
        Else
            Me.Resource.setLabel(Me.LBowner)
            Me.LBowner.Text = String.Format(Me.Resource.getValue("createdHeader"), oItem.CreatedBy.SurnameAndName, oItem.CreatedOn.Value.ToString("dd/MM/yy"), oItem.CreatedOn.Value.ToString("hh:mm"))
            If oItem.ModifiedOn.HasValue Then
                If oItem.isDeleted Then
                    Me.LBowner.Text &= " " & String.Format(MyBase.Resource.getValue("deletedHeader"), oItem.ModifiedOn.Value.ToString("dd/MM/yy"), oItem.ModifiedOn.Value.ToString("hh:mm"), oItem.ModifiedBy.SurnameAndName)
                ElseIf oItem.ModifiedBy Is oItem.CreatedBy Then
                    Me.LBowner.Text &= " " & String.Format(Me.Resource.getValue("selfchangedHeader"), oItem.ModifiedOn.Value.ToString("dd/MM/yy"), oItem.ModifiedOn.Value.ToString("hh:mm"), oItem.ModifiedBy.SurnameAndName)
                Else
                    Me.LBowner.Text &= " " & String.Format(Me.Resource.getValue("changedHeader"), oItem.ModifiedOn.Value.ToString("dd/MM/yy"), oItem.ModifiedOn.Value.ToString("hh:mm"), oItem.ModifiedBy.SurnameAndName)
                End If
            End If
        End If
        Me.CBXdraft.Checked = oItem.isDraft
    End Sub
    Public WriteOnly Property SetStatus() As lm.Comol.Core.DomainModel.MetaApprovationStatus Implements IviewWorkBookItem.SetStatus
        Set(ByVal value As lm.Comol.Core.DomainModel.MetaApprovationStatus)
            Me.LBverificato_t.Text = MyBase.Resource.getValue("status." & value)
        End Set
    End Property
    Public Sub SendToItemsList(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter, ByVal GoToItemId As System.Guid) Implements IviewWorkBookItem.SendToItemsList
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString & "#" & GoToItemId.ToString)
    End Sub

    Public Sub SetBackToWorkbookURL(ByVal WorkBookID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IviewWorkBookItem.SetBackToWorkbookURL
        Me.HYPbackToItems.Visible = (WorkBookID <> System.Guid.Empty)
        If Me.PreloadedItemID = System.Guid.Empty Then
            Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString
        Else
            Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & "Generici/WorkBookItemsList.aspx?WorkBookID=" & WorkBookID.ToString & "&View=" & oView.ToString & "#" & Me.PreloadedItemID.ToString
        End If
    End Sub
    Public Sub SendToFileManagement(ByVal ItemID As System.Guid, ByVal oView As WorkBookTypeFilter) Implements IviewWorkBookItem.SendToFileManagement
        Me.PageUtility.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & ItemID.ToString & "&View=" & oView.ToString)
    End Sub

#Region "Notification / action / messages "
    Public Sub NoItemWithThisID(ByVal CommunityID As Integer) Implements IviewWorkBookItem.NoItemWithThisID
        Me.MLVitemData.SetActiveView(Me.VIWnoItem)
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.GenericError, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
    Public Sub NoPermission(ByVal CommunityID As Integer) Implements IviewWorkBookItem.NoPermission
        Me.BindNoPermessi()
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
    Public Sub NotifyAdd(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IviewWorkBookItem.NotifyAdd
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemAdd(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub NotifyEdit(ByVal isPersonal As Boolean, ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal WorkBookName As String, ByVal WorkBookItemID As System.Guid, ByVal ItemName As String, ByVal ItemData As Date, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements IviewWorkBookItem.NotifyEdit
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyItemEdit(isPersonal, CommunityID, WorkBookID, WorkBookName, WorkBookItemID, ItemName, ItemData, CreatorName, Authors, Me.PreviousWorkBookView.ToString)
    End Sub
    Public Sub SendAddAction(ByVal ItemID As System.Guid) Implements IviewWorkBookItem.SendAddAction
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.CreateItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, ItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub SendEditAction(ByVal ItemID As System.Guid) Implements IviewWorkBookItem.SendEditAction
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.EditItem, Me.PageUtility.CreateObjectsList(Services_WorkBook.ObjectType.WorkBookItem, ItemID.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub
#End Region

    Private Sub LNBgoToFileManagement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBgoToFileManagement.Click, LNBgoToFileManagementBottom.Click
        Me.CurrentPresenter.GoToFileManagement()
    End Sub

    Private Sub LNBsaveItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveItem.Click
        Me.CurrentPresenter.SaveItem()
    End Sub

    Public Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission) Implements IviewWorkBookItem.SetEditing
        Me.DDLediting.Items.Clear()
        Me.DDLediting.DataSource = oAvailableEditing
        Me.DDLediting.DataValueField = "Id"
        Me.DDLediting.DataTextField = "Translation"
        Me.DDLediting.DataBind()
        Try
            Me.DDLediting.SelectedValue = ItemEditing
        Catch ex As Exception

        End Try
        If Me.DDLediting.SelectedIndex > -1 Then
            Me.LBediting.Text = Me.DDLediting.SelectedItem.Text
        End If
    End Sub

    Public ReadOnly Property GetEditingTranslation(ByVal Permissions As Integer) As String Implements IviewWorkBookItem.GetEditingTranslation
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString)
        End Get
    End Property

    Public ReadOnly Property GetEditingTranslationOwner(ByVal isOwner As Boolean, ByVal Permissions As Integer) As String Implements IviewWorkBookItem.GetEditingTranslationOwner
        Get
            Return Me.Resource.getValue("EditingSettings." & Permissions.ToString & "." & isOwner.ToString)
        End Get
    End Property

  
End Class