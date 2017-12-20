Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.Repository.Domain
Imports lm.Comol.Core.DomainModel

Partial Public Class CommunityRepositoryEdit
    Inherits PageBase
    Implements IViewCommunityFileEdit

#Region "IVIEW"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As CRitemEditPresenter
    Public ReadOnly Property CurrentPresenter() As CRitemEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRitemEditPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
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
    Public WriteOnly Property AllowBackToDownloads(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileEdit.AllowBackToDownloads
        Set(ByVal value As Boolean)
            Me.HYPbackToDownloads.Visible = value
            If value Then
                Me.HYPbackToDownloads.NavigateUrl = Me.BaseUrl & RootObject.RepositoryCurrentList(FolderID, CommunityID, oView.ToString, 0, PreLoadedContentView)
                'Me.HYPbackToDownloads.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property
    Public WriteOnly Property AllowBackToManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IViewCommunityFileEdit.AllowBackToManagement
        Set(ByVal value As Boolean)
            Me.HYPbackToManagement.Visible = value
            If value Then
                Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView)
                '  Me.HYPbackToManagement.NavigateUrl = Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property

    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IViewCommunityFileEdit.CommunityRepositoryPermission
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


    Public ReadOnly Property PreloadedItemID() As Long Implements IViewCommunityFileEdit.PreloadedItemID
        Get
            If Not IsNumeric(Request.QueryString("ItemID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("ItemID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedCommunityID() As Integer Implements IViewCommunityFileEdit.PreloadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedFolderID() As Long Implements IViewCommunityFileEdit.PreloadedFolderID
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedPreviousView() As IViewExploreCommunityRepository.ViewRepository Implements IViewCommunityFileEdit.PreloadedPreviousView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return IViewExploreCommunityRepository.ViewRepository.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("View"), IViewExploreCommunityRepository.ViewRepository.None)
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPage() As RepositoryPage Implements IViewCommunityFileEdit.PreLoadedPage
        Get
            If IsNothing(Request.QueryString("PreviousPage")) Then
                Return RepositoryPage.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RepositoryPage).GetByString(Request.QueryString("PreviousPage"), RepositoryPage.DownLoadPage)
            End If
        End Get
    End Property

    Private ReadOnly Property Portalname() As String Implements IViewCommunityFileEdit.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IViewCommunityFileEdit.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
    Private Property RepositoryCommunityID() As Integer Implements IViewCommunityFileEdit.RepositoryCommunityID
        Get
            Return CInt(Me.ViewState("RepositoryCommunityID"))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Public Property RepositoryItemID() As Long Implements IViewCommunityFileEdit.RepositoryItemID
        Get
            Return CLng(Me.ViewState("RepositoryItemID"))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("RepositoryItemID") = value
        End Set
    End Property
    Private Property RepositoryFolderID() As Long Implements IViewCommunityFileEdit.RepositoryFolderID
        Get
            Return Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
            Me.UDPselectFolder.Update()
        End Set
    End Property
    Private WriteOnly Property CommunityName() As String Implements IViewCommunityFileEdit.CommunityName
        Set(ByVal value As String)
            Me.LBcommunity.Text = value
        End Set
    End Property
    Private Property Description() As String Implements IViewCommunityFileEdit.Description
        Get
            Return Me.TXBdescription.Text
        End Get
        Set(ByVal value As String)
            Me.TXBdescription.Text = value
        End Set
    End Property
    Private Property DownlodableByCommunity() As Boolean Implements IViewCommunityFileEdit.DownlodableByCommunity
        Get
            If Me.RBLallowTo.SelectedIndex = -1 Then
                Return True
            Else
                Return (Me.RBLallowTo.SelectedValue = True)
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.RBLallowTo.SelectedValue = value
            Me.BTNeditSave.Visible = value
            Me.BTNeditSaveAndPermission.Visible = Not value
            Me.BTNeditSaveBottom.Visible = value
            Me.BTNeditSaveAndPermissionBottom.Visible = Not value
        End Set
    End Property
    Private WriteOnly Property ItemPath() As String Implements IViewCommunityFileEdit.ItemPath
        Set(ByVal value As String)
            Me.LBpath.Text = value
        End Set
    End Property
    Private Property ItemExtension() As String Implements IViewCommunityFileEdit.ItemExtension
        Get
            Return Me.LBfileExtension.Text
        End Get
        Set(ByVal value As String)
            Me.LBfileExtension.Text = value
        End Set
    End Property
    Private Property ItemName() As String Implements IViewCommunityFileEdit.ItemName
        Get
            Return Me.TXBitemName.Text
        End Get
        Set(ByVal value As String)
            Me.TXBitemName.Text = value
        End Set
    End Property
    Public Property VisibleToDownloaders() As Boolean Implements IViewCommunityFileEdit.VisibleToDownloaders
        Get
            If Me.RBLvisibleTo.SelectedIndex = -1 Then
                Return True
            Else
                Return (Me.RBLvisibleTo.SelectedValue = True)
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.RBLvisibleTo.SelectedValue = value
        End Set
    End Property
    Public Property AllowDownload() As Boolean Implements IViewCommunityFileEdit.AllowDownload
        Get
            Return Me.RBLplay.SelectedValue
        End Get
        Set(ByVal value As Boolean)
            Me.RBLplay.SelectedValue = value
        End Set
    End Property
    Public Property AllowUpload() As Boolean Implements IViewCommunityFileEdit.AllowUpload
        Get
            Return Me.CBXallowUpload.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXallowUpload.Checked = value
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
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_CommunityFile", "Generici", "UC_File")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceEditTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")

            .setLabel(Me.LBcommunity_t)
            .setLabel(Me.LBdescription_t)
            .setLabel(Me.LBname_t)
            .setLabel(Me.LBpath_t)
            .setLabel(Me.LBtype_t)
            .setLabel(Me.LBvisibleTo_t)
            .setLabel(LBallowTo)
            .setButton(BTNeditPath, True, , , True)
            .setRadioButtonList(Me.RBLallowTo, True)
            .setRadioButtonList(Me.RBLallowTo, False)
            .setRadioButtonList(Me.RBLvisibleTo, True)
            .setRadioButtonList(Me.RBLvisibleTo, False)
            .setLabel(Me.LBadvancedInfo)

            .setButton(Me.BTNcloseModal, True, , , True)
            .setButton(Me.BTNbackToFolder, True, , , True)
            .setButton(Me.BTNclose, True, , , True)
            .setButton(Me.BTNeditPath, True, , , True)
            .setButton(Me.BTNeditSave, True, , , True)
            .setButton(Me.BTNeditSaveAndPermission, True, , , True)
            .setButton(Me.BTNeditSaveBottom, True, , , True)
            .setButton(Me.BTNeditSaveAndPermissionBottom, True, , , True)
            .setLabel(Me.LBplay)
            .setRadioButtonList(Me.RBLplay, True)
            .setRadioButtonList(Me.RBLplay, False)

            .setLabel(LBapplyTo)
            .setLinkButton(Me.LNBundo, True, True)
            .setLinkButton(Me.LNBapplyToThis, True, True)
            .setLinkButton(Me.LNBapplyToSubItems, True, True)

            .setHyperLink(HYPbackToDownloads, True, True)
            .setHyperLink(HYPbackToManagement, True, True)
            .setCheckBox(CBXallowUpload)
            CBXallowUpload.ToolTip = Resource.getValue("CBXallowUpload.ToolTip")
            .setLabel(Me.LBpermissionInfo_t)

        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Action"
    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '    PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    'End Sub
    Public Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean) Implements IViewCommunityFileEdit.SendActionInit
        Dim oActionID As Services_File.ActionType = IIf(ForFile, Services_File.ActionType.FileEditing, Services_File.ActionType.FolderEditing)
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, oActionID, Me.CreateNotifyObject(ModuleID, ItemID, ForFile, isScorm), lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub
    Public Sub SendActionCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean) Implements IViewCommunityFileEdit.SendActionCompleted
        Dim oActionID As Services_File.ActionType = IIf(ForFile, Services_File.ActionType.FileEdited, Services_File.ActionType.FolderEdited)
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, oActionID, Me.CreateNotifyObject(ModuleID, ItemID, ForFile, isScorm), lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub
#End Region


#Region "Control Methods"
    Private Sub CTRLCommunityFolder_AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLCommunityFolder.AjaxFolderSelected
        Me.UDPselectFolder.Update()
        Me.CurrentPresenter.Verify(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLCommunityFolder.SelectedFolderName)
    End Sub
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    'Private Sub OpenDialog(ByVal dialogId As String)

    '    Me.LTscript.Text = "<script language='Javascript' type='text/javascript'> "
    '    Me.LTscript.Text &= String.Format("showDialog('{0}');", dialogId)
    '    Me.LTscript.Text &= " </script>"
    '    'Dim script As String = String.Format("showDialog('{0}');", dialogId)
    '    'ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    'End Sub
#End Region

    Public Sub InitializeView(ByVal isFile As Boolean, type As Repository.RepositoryItemType) Implements IViewCommunityFileEdit.InitializeView
        Me.MLVeditItem.SetActiveView(VIWedit)
        Me.DVtype.Visible = isFile
        Me.DVtypeDownload.Visible = Not (type = Repository.RepositoryItemType.FileStandard OrElse type = Repository.RepositoryItemType.Folder)
        If isFile Then
            Me.LBeditInfo.Text = Me.Resource.getValue("fileInfoEdit")
            Me.LBtype.Text = Me.Resource.getValue("RepositoryItemType." & type.ToString)
            ' If oTypeID = Main.TipoMaterialeUpload.Scorm Then
            Me.RBLplay.Enabled = SystemSettings.Presenter.RepositoryConfiguration.DefaultDownload.Contains(CInt(type))
            'End If
        Else
            Me.LBeditInfo.Text = Me.Resource.getValue("folderInfoEdit")
        End If
        DVpermissionUpload.Visible = Not isFile
        Me.DVadvancedContainer.Attributes.Add("class", IIf(isFile, "AdvancedContainer h120", "AdvancedContainer h150"))
        Me.DVdetailsContainer.Attributes.Add("class", IIf(isFile, "DetailsContainerFile", "DetailsContainerFolder"))
        Resource.setLabel_To_Value(LBallowTo, "LBallowTo." & isFile)
        Resource.setRadioButtonList(Me.RBLvisibleTo, IIf(isFile, "File.True", "Folder.True"))
    End Sub

    Public Sub LoadFolderSelector(ByVal ExludeFolderID As Long, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IViewCommunityFileEdit.LoadFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, FolderID, ShowHiddenItems, AdminPurpose, ExludeFolderID)
        Me.BTNeditPath.Visible = Me.CTRLCommunityFolder.HasMoreFolder
        Me.UDPselectFolder.Update()
    End Sub

    Private Sub RBLallowTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLallowTo.SelectedIndexChanged
        Me.CurrentPresenter.ChangePermissionSelector()
        Me.BTNeditSave.Visible = (Me.RBLallowTo.SelectedValue = True)
        Me.BTNeditSaveAndPermission.Visible = Not (Me.RBLallowTo.SelectedValue = True)
        Me.BTNeditSaveBottom.Visible = (Me.RBLallowTo.SelectedValue = True)
        Me.BTNeditSaveAndPermissionBottom.Visible = Not (Me.RBLallowTo.SelectedValue = True)
        Me.UPDdetails.Update()
        Me.UPDsaveButtons.Update()
    End Sub


    Public Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IViewCommunityFileEdit.NoPermission
        BindNoPermessi()
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Public Sub NoPermissionToEdit(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal ForFile As Boolean, ByVal isScorm As Boolean) Implements IViewCommunityFileEdit.NoPermissionToEdit
        Me.MLVeditItem.SetActiveView(Me.VIWpermissionToEdit)
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.NoPermission, Me.CreateNotifyObject(ModuleID, ItemID, ForFile, isScorm), lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Private Sub BTNeditSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNeditSave.Click, BTNeditSaveAndPermission.Click, BTNeditSaveAndPermissionBottom.Click, BTNeditSaveBottom.Click
        Me.CurrentPresenter.SaveItem(False)
    End Sub

    Public Sub ShowFileNameExist(ByVal FolderParentName As String, ByVal FileName As String) Implements IViewCommunityFileEdit.ShowFileNameExist
        Me.LBerrorNotification.Text = String.Format(Resource.getValue("ShowFolderExist"), FolderParentName, FileName)
        Me.MLVselector.SetActiveView(Me.VIWerrorSelector)
        Me.UDPselectFolder.Update()
    End Sub

    Public Sub ShowFolderDoesntExist(ByVal FolderName As String) Implements IViewCommunityFileEdit.ShowFolderDoesntExist
        Me.LBerrorNotification.Text = String.Format(Resource.getValue("ShowFolderDoesntExist"), FolderName)
        Me.MLVselector.SetActiveView(Me.VIWerrorSelector)
        Me.UDPselectFolder.Update()
    End Sub

    Public Sub ShowFolderExist(ByVal FolderParentName As String, ByVal FolderName As String) Implements IViewCommunityFileEdit.ShowFolderExist
        Me.LBerrorNotification.Text = String.Format(Resource.getValue("ShowFolderExist"), FolderParentName, FolderName)
        Me.MLVselector.SetActiveView(Me.VIWerrorSelector)
        Me.UDPselectFolder.Update()
    End Sub

    Public Sub ItemFolderChanged() Implements IViewCommunityFileEdit.ItemFolderChanged
        Me.LBpath.Text = Me.CTRLCommunityFolder.SelectedFolderPathName
        Me.CloseDialog("selectFolder")
        Me.UPDdetails.Update()
        Me.MLVselector.SetActiveView(VIWfolderSelector)
        Me.UDPselectFolder.Update()
    End Sub

    Private Sub BTNclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNclose.Click
        Me.CloseDialog("selectFolder")
        Me.UPDdetails.Update()
        Me.MLVselector.SetActiveView(VIWfolderSelector)
        Me.UDPselectFolder.Update()
    End Sub

    Private Sub BTNbackToFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNbackToFolder.Click
        Me.MLVselector.SetActiveView(Me.VIWfolderSelector)
        Me.RepositoryFolderID = Me.RepositoryFolderID
    End Sub

    Public Sub LoadRepositoryPage(ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IViewCommunityFileEdit.LoadRepositoryPage
        Select Case GotoPage
            Case RepositoryPage.CommunityDiaryPage

            Case RepositoryPage.DownLoadPage
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
                ' Me.RedirectToUrl("Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            Case RepositoryPage.ManagementPage
                Me.RedirectToUrl(RootObject.RepositoryManagement(FolderID, CommunityID, View.ToString, PreLoadedContentView))
                'Me.RedirectToUrl("Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)
            Case RepositoryPage.None
                Me.RedirectToUrl(RootObject.RepositoryCurrentList(FolderID, CommunityID, View.ToString, 0, PreLoadedContentView))
                '"Modules/Repository/CommunityRepository.aspx?FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString)

        End Select
    End Sub

    Public Sub LoadEditingPermission(ByVal ItemID As Long, ByVal CommunityID As Integer, ByVal FolderID As Long, ByVal View As lm.Comol.Modules.Base.Presentation.IViewExploreCommunityRepository.ViewRepository, ByVal GotoPage As RepositoryPage) Implements IViewCommunityFileEdit.LoadEditingPermission
        Me.RedirectToUrl("Modules/Repository/CommunityRepositoryItemPermission.aspx?ItemID=" & ItemID.ToString & "&FolderID=" & FolderID.ToString & "&CommunityID=" & CommunityID.ToString & "&View=" & View.ToString & "&PreviousPage=" & GotoPage.ToString & "&Action=" & PermissionAction.EditItem & "&PreserveUrl=true")
    End Sub

    Public WriteOnly Property AskToApplyToAllSubItems() As Boolean Implements IViewCommunityFileEdit.AskToApplyToAllSubItems
        Set(ByVal value As Boolean)
            Me.BTNeditSave.OnClientClick = IIf(value, "return showDialog('applyToSubItems');", "")
            Me.BTNeditSaveAndPermission.OnClientClick = IIf(value, "return showDialog('applyToSubItems');", "")
            Me.BTNeditSaveBottom.OnClientClick = IIf(value, "return showDialog('applyToSubItems');", "")
            Me.BTNeditSaveAndPermissionBottom.OnClientClick = IIf(value, "return showDialog('applyToSubItems');", "")
        End Set
    End Property

    Private Sub LNBapplyToThis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapplyToThis.Click
        Me.CurrentPresenter.SaveItem(False)
        Me.CloseDialog("applyToSubItems")
    End Sub

    Private Sub LNBapplyToSubItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapplyToSubItems.Click
        Me.CurrentPresenter.SaveItem(True)
        Me.CloseDialog("applyToSubItems")
    End Sub


#Region "Notifications"
    'Public Sub NotifyItemChanges(ByVal CommunityID As Integer, ByVal OwnerID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal isVisible As Boolean) Implements IViewCommunityFileEdit.NotifyItemChanges
    '    Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
    '    Dim oContext As New RepositoryNotificationUtility.NotifyContext
    '    oContext.FatherID = FatherID
    '    oContext.FatherName = FatherName
    '    oContext.ForItem = IIf(IsFile, RepositoryNotificationUtility.NotifyFor.File, RepositoryNotificationUtility.NotifyFor.Folder)
    '    oContext.ItemID = ItemID
    '    oContext.ItemName = ItemName
    '    oContext.OwnerID = OwnerID

    '    If isVisible Then
    '        oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAllSee
    '    Else
    '        oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAdmin
    '        If OwnerID > 0 Then : oContext.ToUsers = oContext.ToUsers Or RepositoryNotificationUtility.NotifyTo.ToOwner
    '        End If
    '    End If
    '    oSender.NotifyItemModifyed(CommunityID, oContext)
    'End Sub

    Public Sub NotifyItemChanges(ByVal CommunityID As Integer, ByVal OwnerID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal uniqueID As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewCommunityFileEdit.NotifyItemChanges
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        Dim oContext As New RepositoryNotificationUtility.NotifyContext
        oContext.FatherID = FatherID
        oContext.FatherName = FatherName
        oContext.RepositoryItemType = type
        oContext.ItemID = ItemID
        oContext.ItemName = ItemName
        oContext.UniqueId = uniqueID
        oContext.OwnerID = OwnerID

        If isVisible Then
            oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAllSee
        Else
            oContext.ToUsers = RepositoryNotificationUtility.NotifyTo.ToAdmin
            If OwnerID > 0 Then : oContext.ToUsers = oContext.ToUsers Or RepositoryNotificationUtility.NotifyTo.ToOwner
            End If
        End If

        oSender.NotifyItemModifyed(CommunityID, oContext)
    End Sub

    Public Sub NotifyVisibilityChange(ByVal CommunityID As Integer, ByVal ItemName As String, ByVal ItemID As Long, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueID As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewCommunityFileEdit.NotifyVisibilityChange
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        If IsFile Then
            oSender.NotifyItemVisibility(CommunityID, FatherID, ItemID, uniqueID, ItemName, FatherName, isVisible, type)
        Else
            oSender.NotifyFolderVisibility(CommunityID, FatherID, ItemID, ItemName, FatherName, isVisible)
        End If
    End Sub

    Public Sub NotifyPermissionChanged(ByVal ToAllCommunity As Boolean, ByVal OwnerID As Integer, ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal ItemName As String, ByVal FatherID As Long, ByVal FatherName As String, ByVal IsFile As Boolean, ByVal uniqueId As System.Guid, ByVal isVisible As Boolean, type As lm.Comol.Core.DomainModel.Repository.RepositoryItemType) Implements IViewCommunityFileEdit.NotifyPermissionChanged
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


    Private Function CreateNotifyObject(ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal isFile As Boolean, ByVal IsScorm As Boolean) As List(Of PresentationLayer.WS_Actions.ObjectAction)
        Dim ObjectTypeID As Integer = Services_File.ObjectType.File
        If IsScorm Then
            ObjectTypeID = Services_File.ObjectType.FileScorm
        ElseIf Not isFile Then
            ObjectTypeID = Services_File.ObjectType.Folder
        End If
        Return Me.PageUtility.CreateObjectsList(ModuleID, ObjectTypeID, ItemID.ToString)
    End Function
#End Region
    Public Sub RenameItemError(ByVal folderParentName As String, ByVal itemName As String, ByVal isFile As Boolean) Implements IViewCommunityFileEdit.RenameItemError
        Me.MLVerrors.SetActiveView(VIWerrorModal)
        Me.LBerrorModal.Text = String.Format(IIf(isFile, Resource.getValue("ShowFileNameExist"), Resource.getValue("ShowFolderExist")), folderParentName, itemName)
        Me.MLVselector.SetActiveView(Me.VIWerrorSelector)
        Me.OpenDialog("editError")
        Me.UDPerrors.Update()
    End Sub

    Private Sub BTNcloseModal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcloseModal.Click
        Me.CloseDialog("editError")
        'Me.UDPdata.Update()
        Me.MLVerrors.SetActiveView(VIWerrorEmpty)
        Me.UDPerrors.Update()
    End Sub

    Private Sub BTNeditPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNeditPath.Click
        Me.MLVselector.SetActiveView(VIWfolderSelector)
        Me.OpenDialog("selectFolder")
        Me.UDPselectFolder.Update()
    End Sub

    Private Sub CBXallowUpload_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXallowUpload.CheckedChanged
        Me.CurrentPresenter.ChangeUploadDeleteFolderItems(CBXallowUpload.Checked)
    End Sub
End Class