Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Domain
Partial Public Class CommunityRepositoryItemPermission
    Inherits PageBase
    Implements IviewItemPermission



#Region "View"
    Private _Presenter As CRitemPermissionPresenter
    Private ReadOnly Property CurrentPresenter() As CRitemPermissionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRitemPermissionPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IviewItemPermission.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = New ModuleCommunityRepository
            End If
        End If

        Return oModule
    End Function
    Public ReadOnly Property PreloadedItemID() As Long Implements IviewItemPermission.PreloadedItemID
        Get
            If IsNumeric(Me.Request.QueryString("ItemID")) Then
                Return CLng(Me.Request.QueryString("ItemID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPage() As RepositoryPage Implements IviewItemPermission.PreLoadedPage
        Get
            If IsNothing(Request.QueryString("PreviousPage")) Then
                Return RepositoryPage.EditItem
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PreviousPage"), RepositoryPage.EditItem)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository Implements IviewItemPermission.PreloadedPreviousView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IViewExploreCommunityRepository.ViewRepository.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("View"), IViewExploreCommunityRepository.ViewRepository.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedCommunityID() As Long Implements IviewItemPermission.PreloadedCommunityID
        Get
            If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                Return CLng(Me.Request.QueryString("CommunityID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedFolderID() As Long Implements IviewItemPermission.PreloadedFolderID
        Get
            If IsNumeric(Me.Request.QueryString("FolderID")) Then
                Return CLng(Me.Request.QueryString("FolderID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedRemoteItemID() As String Implements IviewItemPermission.PreloadedRemoteItemID
        Get
            Return Me.Request.QueryString("RemoteItemID")
        End Get
    End Property
    Public Property RemoteItemID() As String Implements IviewItemPermission.RemoteItemID
        Get
            Return Me.ViewState("RemoteItemID")
        End Get
        Set(ByVal value As String)
            Me.ViewState("RemoteItemID") = value
        End Set
    End Property
    Public WriteOnly Property FileName() As String Implements IviewItemPermission.FileName
        Set(ByVal value As String)
            Me.LBinfoItem_t.Text = String.Format(Me.Resource.getValue("infoFileTitle"), value)
            Me.LBinfoItem.Text = Me.Resource.getValue("infoFile")
        End Set
    End Property
    Public WriteOnly Property FilesName() As List(Of String) Implements IviewItemPermission.FilesName
        Set(ByVal value As System.Collections.Generic.List(Of String))
            Dim FileNames As String = ""
            For i As Integer = 0 To value.Count - 1
                If i = 0 Then
                    FileNames = value(i)
                Else
                    FileNames &= ", " & value(i)
                End If
            Next
            FileNames &= "."
            Me.LBinfoItem_t.Text = String.Format(Me.Resource.getValue("infoFileMultiple"), FileNames)
            Me.LBinfoItem.Text = Me.Resource.getValue("infoFileMultiple")
        End Set
    End Property
    Public WriteOnly Property FolderName() As String Implements IviewItemPermission.FolderName
        Set(ByVal value As String)
            Me.LBinfoItem_t.Text = String.Format(Me.Resource.getValue("infoFolderTitle"), value)
            Me.LBinfoItem.Text = Me.Resource.getValue("infoFolder")
        End Set
    End Property
    Public Property AllMembers() As Boolean Implements IviewItemPermission.AllMembers
        Get
            Return Me.RBLallowTo.SelectedValue
        End Get
        Set(ByVal value As Boolean)
            Me.RBLallowTo.SelectedValue = value
            Me.MLVpermission.SetActiveView(IIf(value, VIWnone, VIWdefinePermission))
            'Me.UDPpermission.Update()
            Me.UPdpermissionContainer.Update()
        End Set
    End Property
    Public Property SelectedMembers() As List(Of dtoMember(Of Integer)) Implements IviewItemPermission.SelectedMembers
        Get
            If TypeOf Session("SelectedMembers") Is List(Of dtoMember(Of Integer)) Then
                Return Session("SelectedMembers")
            Else
                Session("SelectedMembers") = New List(Of COL_BusinessLogic_v2.FilterElement)
                Return Session("SelectedMembers")
            End If
        End Get
        Set(ByVal value As List(Of dtoMember(Of Integer)))
            Session("SelectedMembers") = value
        End Set
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IviewItemPermission.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
    Public ReadOnly Property SelectedMembersID() As List(Of Integer) Implements IviewItemPermission.SelectedMembersID
        Get
            Return (From m In SelectedMembers Select m.Id).ToList
        End Get
    End Property
    Public Property SelectedRoles() As List(Of COL_BusinessLogic_v2.FilterElement) Implements IviewItemPermission.SelectedRoles
        Get
            If TypeOf Session("SelectedRoles") Is List(Of COL_BusinessLogic_v2.FilterElement) Then
                Return Session("SelectedRoles")
            Else
                Session("SelectedRoles") = New List(Of COL_BusinessLogic_v2.FilterElement)
                Return Session("SelectedRoles")
            End If
        End Get
        Set(ByVal value As List(Of COL_BusinessLogic_v2.FilterElement))
            Session("SelectedRoles") = value
        End Set
    End Property
    Public ReadOnly Property SelectedRolesID() As List(Of Integer) Implements IviewItemPermission.SelectedRolesID
        Get
            Return (From m As ListItem In Me.CBLselectedRole.Items Where m.Selected Select CInt(m.Value)).ToList
        End Get
    End Property
    Public WriteOnly Property SessionUniqueID() As System.Guid Implements IviewItemPermission.SessionUniqueID
        Set(ByVal value As System.Guid)
            Me.ViewState("SessionUniqueID") = value
        End Set
    End Property


    Public ReadOnly Property PreloadedMultipleItemsID() As List(Of Long) Implements IviewItemPermission.PreloadedMultipleItemsID
        Get
            If Me.Request.Form.Keys.Count > 0 Then
                Return (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_ItemID_") Select CLng(Me.Request.Form(v))).ToList
            Else
                Return New List(Of Long)
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedMultipleItemsName() As List(Of String) Implements IviewItemPermission.PreloadedMultipleItemsName
        Get
            If Me.Request.Form.Keys.Count > 0 Then
                Return (From v As String In Me.Request.Form.Keys Where v.StartsWith("FILE_C_Name_") Select Me.Request.Form(v)).ToList
            Else
                Return New List(Of String)
            End If

        End Get
    End Property



    Public Property ItemCommunityID() As Integer Implements IviewItemPermission.ItemCommunityID
        Get
            If IsNumeric(Me.ViewState("ItemCommunityID")) Then
                Return CInt(Me.ViewState("ItemCommunityID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ItemCommunityID") = value
        End Set
    End Property

    Public Property PermissionForMultipleFile() As Boolean Implements IviewItemPermission.isSetPermissionForMultipleFile
        Get
            If TypeOf Me.ViewState("PermissionForMultipleFile") Is Boolean Then
                Return CBool(Me.ViewState("PermissionForMultipleFile"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("PermissionForMultipleFile") = value
        End Set
    End Property

    Private ReadOnly Property PreloadedPreserveUrl() As Boolean Implements IviewItemPermission.PreloadedPreserveUrl
        Get
            Try
                Return CBool(Request.QueryString("PreserveUrl"))
            Catch ex As Exception

            End Try
            Return False
        End Get
    End Property
    Public Property PreservedUrl() As String Implements IviewItemPermission.PreservedUrl
        Get

            Return Me.ViewState("value")
        End Get
        Set(ByVal value As String)
            Me.ViewState("value") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedAction() As PermissionAction Implements IviewItemPermission.PreloadedAction
        Get
            If IsNothing(Request.QueryString("Action")) Then
                Return PermissionAction.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of PermissionAction).GetByString(Request.QueryString("Action"), PermissionAction.None)
            End If
        End Get
    End Property
    Public WriteOnly Property AllowSave() As Boolean Implements IviewItemPermission.AllowSave
        Set(ByVal value As Boolean)
            Me.LNBsave.Visible = value
            Me.LNBaddPerson.Enabled = value
            Me.LNBroles.Enabled = value
        End Set
    End Property
    Public Property RepositoryItemID() As Long Implements IviewItemPermission.RepositoryItemID
        Get
            If IsNumeric(Me.ViewState("RepositoryItemID")) Then
                Return CLng(Me.ViewState("RepositoryItemID"))
            Else
                Me.ViewState("RepositoryItemID") = 0
                Return 0
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("RepositoryItemID") = value
        End Set
    End Property
    Public ReadOnly Property ForService() As String Implements IviewItemPermission.ForService
        Get
            Try
                Return CBool(Request.QueryString("ForService"))
            Catch ex As Exception

            End Try
            Return False
        End Get
    End Property
#End Region

#Region "Inherits"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
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

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityRepositoryItemPermission", "Generici")
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBnoPermissionToEdit)
            .setHyperLink(HYPbackToDownloads, True, True)
            .setHyperLink(HYPbackToManagement, True, True)
            .setHyperLink(HYPbackToItem, True, True)

            .setLinkButton(Me.LNBsave, True, True)
            '.setLabel(LBnoPermissionToUpload)

            .setLinkButton(Me.LNBroles, True, True)
            .setLinkButton(Me.LNBaddPerson, True, True)
            .setLinkButton(Me.LNBsaveRole, True, True)
            .setLinkButton(Me.LNBunSaveRole, True, True)
            .setLinkButton(Me.LNBunSavePerson, True, True)
            .setLinkButton(Me.LNBsavePerson, True, True)

            .setLabel(LBrolesInfo)
            .setLabel(LBpersonInfo)
            .setLabel(LBselectRole)
            .setLabel(LBinfoItem_t)
            .setLabel(LBinfoItem)
            .setRadioButtonList(RBLallowTo, True)
            .setRadioButtonList(RBLallowTo, False)

            .setLabel(LBapplyTo)
            .setLinkButton(Me.LNBundo, True, True)
            .setLinkButton(Me.LNBapplyToThis, True, True)
            .setLinkButton(Me.LNBapplyToSubItems, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IviewItemPermission.NoPermission
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
        BindNoPermessi()
    End Sub

#Region "Action"

    Public Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean) Implements IviewItemPermission.SendActionInit
        Dim oActionID As Services_File.ActionType = IIf(ForFile, Services_File.ActionType.FileEditingPermission, Services_File.ActionType.FolderEditingPermission)
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, oActionID, CreateNotifyObject(ModuleID, ItemID, ForFile, isScorm), lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub
    Public Sub SendActionCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ForFile As Boolean) Implements IviewItemPermission.SendActionCompleted
        Dim oActionID As Services_File.ActionType = IIf(ForFile, Services_File.ActionType.FilePermissionModifyed, Services_File.ActionType.FolderPermissionModifyed)
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, oActionID, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Public Sub NoPermissionToEdit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean) Implements IviewItemPermission.NoPermissionToEdit
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, CreateNotifyObject(ModuleID, ItemID, ForFile, isScorm), lm.ActionDataContract.InteractionType.SystemToSystem)
        Me.MLVdata.SetActiveView(Me.VIWnoPermissionToEdit)
    End Sub

    Public Sub NotifyPermissionChanged(ByVal ToAllCommunity As Boolean, ByVal OwnerID As Integer, ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal ItemName As String, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueId As System.Guid, ByVal isVisible As Boolean, type As Repository.RepositoryItemType) Implements IviewItemPermission.NotifyPermissionChanged
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        Dim oContext As New RepositoryNotificationUtility.NotifyContext
        oContext.FatherID = FatherID
        oContext.FatherName = FatherName

        oContext.RepositoryItemType = type

        oContext.ItemID = ItemID
        oContext.ItemName = ItemName
        oContext.OwnerID = OwnerID

        If isVisible Then
            If ToAllCommunity Then : oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAllSee
            Else
                oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAdmin
                If OwnerID > 0 Then : oContext.ToUsers = oContext.ToUsers Or RepositoryNotificationUtility.NotifyTo.ToOwner
                End If
            End If
        Else
            oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAdmin
            If OwnerID > 0 Then : oContext.ToUsers = oContext.ToUsers Or RepositoryNotificationUtility.NotifyTo.ToOwner
            End If
        End If
        If ToAllCommunity Then : oSender.NotifyItemPermissionModifyedToCommunity(CommunityID, oContext)
        Else : oSender.NotifyItemPermissionModifyedToSome(CommunityID, oContext)
        End If

    End Sub
#End Region



#Region "Management members"
    Public Sub Initialize(ByVal oRoles As List(Of COL_BusinessLogic_v2.FilterElement), ByVal oPersons As List(Of dtoMember(Of Integer)), ByVal ForAll As Boolean) Implements IviewItemPermission.Initialize
        Me.SelectedMembers = oPersons
        Me.SelectedRoles = oRoles
        Me.AllMembers = ForAll
        Me.CBLselectedRole.Visible = (oRoles.Count > 0)
        If oRoles.Count > 0 Then
            Me.CBLselectedRole.DataSource = oRoles
            Me.CBLselectedRole.DataTextField = "Text"
            Me.CBLselectedRole.DataValueField = "Value"
            Me.CBLselectedRole.DataBind()

            For Each oItem As ListItem In Me.CBLselectedRole.Items
                oItem.Selected = True
            Next

        End If
        Me.RPTperson.DataSource = oPersons
        Me.RPTperson.DataBind()
        Me.MLVdata.SetActiveView(Me.VIWdata)
        'Me.UDPpermission.Update()
        Me.UPdpermissionContainer.Update()
    End Sub
    Public Sub InitializeAvailableRoles(ByVal oRoles As List(Of COL_BusinessLogic_v2.FilterElement), ByVal SelectedID As System.Collections.Generic.List(Of Integer)) Implements IviewItemPermission.InitializeAvailableRoles
        Me.CBLselectRole.DataSource = oRoles
        Me.CBLselectRole.DataTextField = "Text"
        Me.CBLselectRole.DataValueField = "Value"
        Me.CBLselectRole.DataBind()

        For Each oItem As ListItem In (From o As ListItem In Me.CBLselectRole.Items Where SelectedID.Contains(o.Value) Select o).ToList
            oItem.Selected = True
        Next
        Me.UDPselectRoles.Update()
        'Me.UPdpermissionContainer.Update()
    End Sub
    Public Sub InitializeMembersSelection(ByVal CommunityID As Integer) Implements IviewItemPermission.InitializeMembersSelection
        Dim oCommunities As New List(Of Integer)

        oCommunities.Add(CommunityID)
        Me.CTRLuserList.CurrentPresenter.Init(oCommunities, ListSelectionMode.Multiple, (From m In Me.SelectedMembers Select m.Id).ToList)
        Me.SetFocus(Me.CTRLuserList.GetSearchButtonControl)
    End Sub
#End Region

#Region "Management members"
    Private Sub RPTperson_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTperson.ItemCommand
        If e.CommandName = "remove" Then
            Dim oSelected As List(Of dtoMember(Of Integer)) = Me.SelectedMembers
            Me.SelectedMembers = (From p In oSelected Where p.Id <> CInt(e.CommandArgument) Select p).ToList
            Me.RPTperson.DataSource = Me.SelectedMembers
            Me.RPTperson.DataBind()
            'Me.UDPpermission.Update()
            Me.UPdpermissionContainer.Update()
        End If
    End Sub
    Private Sub RPTperson_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTperson.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oMember As dtoMember(Of Integer) = TryCast(e.Item.DataItem, dtoMember(Of Integer))
            Dim oLNBdelete As LinkButton

            oLNBdelete = e.Item.FindControl("LNBdelete")
            If oMember.Id = Me.CurrentContext.UserContext.CurrentUserID Then
                oLNBdelete.Visible = False
            Else
                oLNBdelete.Visible = True
                Me.Resource.setLinkButton(oLNBdelete, True, True)
                oLNBdelete.Text = String.Format(oLNBdelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBdelete.ToolTip)
            End If
        End If
    End Sub
#End Region


    Private Sub LNBsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsave.Click
        Me.CurrentPresenter.SavePermission()
    End Sub

#Region "Manage Roles"
    Private Sub LNBroles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBroles.Click
        Me.CurrentPresenter.LoadRoles()
        Me.UDPselectRoles.Update()
        'Me.UPdpermissionContainer.Update()
    End Sub
    Private Sub LNBsaveRole_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveRole.Click
        CloseDialog("selectRole")
        Dim SelectedID As List(Of Integer) = (From o As ListItem In Me.CBLselectRole.Items Where o.Selected Select CInt(o.Value)).ToList
        Me.CurrentPresenter.UpdateSelectedRoles(SelectedID)

        ' Me.UDPpermission.Update()
        Me.UPdpermissionContainer.Update()

    End Sub
    Private Sub LNBunSaveRole_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBunSaveRole.Click

    End Sub
    Public Sub UpdateSelectRoles(ByVal oRoles As List(Of COL_BusinessLogic_v2.FilterElement)) Implements IviewItemPermission.UpdateSelectRoles
        Me.CBLselectedRole.DataSource = oRoles
        Me.CBLselectedRole.DataTextField = "Text"
        Me.CBLselectedRole.DataValueField = "Value"
        Me.CBLselectedRole.DataBind()

        For Each oItem As ListItem In Me.CBLselectedRole.Items
            oItem.Selected = True
        Next
        '   Me.UDPpermission.Update()
        Me.CBLselectedRole.Visible = (oRoles.Count > 0)
        Me.UPdpermissionContainer.Update()
    End Sub
#End Region

    Private Sub LNBaddPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBaddPerson.Click
        CloseDialog("selectRole")
        '  Me.UDPpermission.Update()
        Me.DVmenu.Visible = False
        Me.CurrentPresenter.LoadMembers()
        Me.MLVdata.SetActiveView(VIWusers)
        Me.UPdpermissionContainer.Update()
    End Sub

    Private Sub LNBsavePerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsavePerson.Click
        Dim oSelected As List(Of MemberContact) = Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers()
        Me.CurrentPresenter.UpdateSelectedMembers((From c In oSelected Select New dtoMember(Of Integer) With {.Id = c.Id, .Name = c.FullName}).ToList)
        Me.MLVdata.SetActiveView(VIWdata)
        Me.DVmenu.Visible = True
        CloseDialog("selectRole")
        'Me.UDPpermission.Update()
        Me.UPdpermissionContainer.Update()
    End Sub

    Public Sub UpdateSelecteMembers(ByVal oMembers As List(Of dtoMember(Of Integer))) Implements IviewItemPermission.UpdateSelecteMembers
        Me.RPTperson.DataSource = oMembers
        Me.RPTperson.DataBind()

        Me.MLVdata.SetActiveView(Me.VIWdata)
        'Me.UDPpermission.Update()
        Me.UPdpermissionContainer.Update()
    End Sub


    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub

    Private Sub RBLallowTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLallowTo.SelectedIndexChanged
        If Me.RBLallowTo.SelectedValue = "False" Then
            Me.MLVpermission.SetActiveView(Me.VIWdefinePermission)
        Else
            Me.MLVpermission.SetActiveView(Me.VIWnone)
        End If
        Me.CurrentPresenter.ChangePermissionSelector()
        Me.UPdpermissionContainer.Update()
    End Sub

    Public Sub SavePreservedUrl() Implements IviewItemPermission.SavePreservedUrl
        If Not IsNothing(Request.UrlReferrer) Then
            Me.PreservedUrl = Request.UrlReferrer.ToString()
        Else
            Me.PreservedUrl = ""
        End If
    End Sub

    Private Sub CTRLuserList_AjaxEventUpdate() Handles CTRLuserList.AjaxEventUpdate
        Me.UPdpermissionContainer.Update()
    End Sub

    Private Sub LNBunSavePerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBunSavePerson.Click
        Me.DVmenu.Visible = True
        Me.MLVdata.SetActiveView(VIWdata)
        Me.UPdpermissionContainer.Update()
    End Sub


    Public Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IviewItemPermission.LoadRepositoryPage
        If Me.PreloadedPreserveUrl AndAlso Not String.IsNullOrEmpty(Me.PreservedUrl) Then
            Me.RedirectToUrl(Me.PreservedUrl)
        Else
            Select Case GotoPage
                Case RepositoryPage.WorkBookManagementFile
                    Me.RedirectToUrl("Generici/WorkBookItemManagementFile.aspx?ItemID=" & Me.RemoteItemID)
                Case RepositoryPage.CommunityDiaryManagementFilePage
                    Me.RedirectToUrl("Modules/CommunityDiary/CDitemManagementFile.aspx?ItemID=" & Me.RemoteItemID)
                Case RepositoryPage.DownLoadPage
                    Me.RedirectToUrl(RootObject.RepositoryCurrentList(ItemID, FolderID, View.ToString, 0, PreLoadedContentView))
                Case RepositoryPage.ManagementPage
                    Me.RedirectToUrl(RootObject.RepositoryManagement(ItemID, FolderID, View.ToString, PreLoadedContentView))
                Case RepositoryPage.None
                    Me.RedirectToUrl(RootObject.RepositoryCurrentList(ItemID, FolderID, View.ToString, 0, PreLoadedContentView))
                Case RepositoryPage.EditItem
                    Me.RedirectToUrl(RootObject.RepositoryEdit(ItemID, FolderID, CommunityID, View.ToString, "", PreLoadedContentView))
                    '"Modules/Repository/CommunityRepositoryEdit.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
                Case Else
                    Me.RedirectToUrl(RootObject.RepositoryCurrentList(ItemID, FolderID, View.ToString, 0, PreLoadedContentView))
                    'Me.RedirectToUrl("Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            End Select
        End If
    End Sub

    Public Sub SetBackUrl(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal FolderID As Long, ByVal View As lm.Comol.Modules.Base.Presentation.IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As lm.Comol.Modules.Base.DomainModel.RepositoryPage) Implements lm.Comol.Modules.Base.Presentation.IviewItemPermission.SetBackUrl
        Dim NavigateUrl As String = ""
        Select Case GotoPage
            Case RepositoryPage.WorkBookManagementFile
                NavigateUrl = "Generici/WorkBookItemManagementFile.aspx?ItemID=" & Me.RemoteItemID
            Case RepositoryPage.CommunityDiaryManagementFilePage
                NavigateUrl = "Modules/CommunityDiary/CDitemManagementFile.aspx?ItemID=" & Me.RemoteItemID
            Case RepositoryPage.DownLoadPage
                NavigateUrl = RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView)
                '"Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString
            Case RepositoryPage.ManagementPage
                NavigateUrl = RootObject.RepositoryManagement(FolderID, CommunityID, View.ToString, PreLoadedContentView)
                '"Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString
            Case RepositoryPage.None
                NavigateUrl = RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView)
                '"Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString
            Case RepositoryPage.EditItem
                NavigateUrl = RootObject.RepositoryEdit(ItemID, FolderID, CommunityID, View.ToString, "", PreLoadedContentView)
                ' "Modules/Repository/CommunityRepositoryEdit.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString
        End Select
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & NavigateUrl
        Me.HYPbackToItem.Visible = True
    End Sub

    Public Sub SetBackUrlToPrevious() Implements IviewItemPermission.SetBackUrlToPrevious
        Me.HYPbackToItem.NavigateUrl = Me.PreservedUrl
        Me.HYPbackToItem.Visible = Not String.IsNullOrEmpty(Me.PreservedUrl)
    End Sub

    Public WriteOnly Property AskToApplyToAllSubItems() As Boolean Implements IviewItemPermission.AskToApplyToAllSubItems
        Set(ByVal value As Boolean)
            Me.LNBsave.OnClientClick = IIf(value, "return showDialog('applyToSubItems');", "")
        End Set
    End Property

    Private Sub LNBapplyToThis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapplyToThis.Click
        Me.CurrentPresenter.SavePermission()
        Me.CloseDialog("applyToSubItems")
    End Sub

    Private Sub LNBapplyToSubItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapplyToSubItems.Click
        Me.CurrentPresenter.SavePermissionToSubFolders()
        Me.CloseDialog("applyToSubItems")
    End Sub

    Private Function CreateNotifyObject(ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal isFile As Boolean, ByVal IsScorm As Boolean) As List(Of PresentationLayer.WS_Actions.ObjectAction)
        Dim ObjectTypeID As Integer = Services_File.ObjectType.File
        If IsScorm Then
            ObjectTypeID = Services_File.ObjectType.FileScorm
        ElseIf Not isFile Then
            ObjectTypeID = Services_File.ObjectType.Folder
        End If
        Return Me.PageUtility.CreateObjectsList(ModuleID, ObjectTypeID, ItemID.ToString)
    End Function

End Class