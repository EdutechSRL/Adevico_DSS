Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Presentation
Imports lm.Comol.Core.BaseModules.Repository.Domain

Public Class UC_ModuleToRepository
    Inherits BaseControlWithLoad
    Implements IViewModuleToRepository

#Region "Context"
    Private _Presenter As ModuleToRepositoryPresenter
    Private ReadOnly Property CurrentPresenter() As ModuleToRepositoryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleToRepositoryPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    '#Region "Iview"
    '    Private _BaseUrl As String
    '    Public Overloads ReadOnly Property BaseUrl() As String
    '        Get
    '            If _BaseUrl = "" Then
    '                _BaseUrl = Me.PageUtility.BaseUrl
    '            End If
    '            Return _BaseUrl
    '        End Get
    '    End Property

    '#End Region

#Region "Implements"
    Public Property AjaxViewUpdate As Boolean Implements IViewModuleToRepository.AjaxViewUpdate
        Get
            Return ViewStateOrDefault("AjaxViewUpdate", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxViewUpdate") = value
        End Set
    End Property
    Private Property SourceModuleCode As String Implements IViewModuleToRepository.SourceModuleCode
        Get
            Return ViewStateOrDefault("SourceModuleCode", "")
        End Get
        Set(value As String)
            Me.ViewState("SourceModuleCode") = value
        End Set
    End Property
    Private Property SourceModuleIdAction As Integer Implements IViewModuleToRepository.SourceModuleIdAction
        Get
            Return ViewStateOrDefault("SourceModuleIdAction", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("SourceModuleIdAction") = value
        End Set
    End Property
    Private Property IdCommunityRepository As Integer Implements IViewModuleToRepository.IdCommunityRepository
        Get
            Return ViewStateOrDefault("IdCommunityRepository", CInt(0))
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunityRepository") = value
        End Set
    End Property
    Private Property UnloadItems As List(Of Long) Implements IViewModuleToRepository.UnloadItems
        Get
            Return ViewStateOrDefault("UnloadItems", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("UnloadItems") = value
        End Set
    End Property
    Public Property InternalFileType As FileRepositoryType Implements IViewModuleToRepository.InternalFileType
        Get
            Return ViewStateOrDefault("InternalFileType", FileRepositoryType.None)
        End Get
        Set(ByVal value As FileRepositoryType)
            Me.ViewState("InternalFileType") = value
        End Set
    End Property
    Public Property IsCreateActionAvailable As Boolean Implements IViewModuleToRepository.IsCreateActionAvailable
        Get
            Return ViewStateOrDefault("IsCreateActionAvailable", False)
        End Get
        Set(value As Boolean)
            ViewState("IsCreateActionAvailable") = value
        End Set
    End Property
    Public Property CurrentAction As UserRepositoryAction Implements IViewModuleToRepository.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", UserRepositoryAction.SelectAction)
        End Get
        Set(value As UserRepositoryAction)
            ViewState("CurrentAction") = value
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

#Region "UC property / events"
    Public Event CloseClientWindow()
    Public Event ContainerCommands(ByVal show As Boolean)
    Public Event UpdateAjaxPanel()
    Public Event LinkedModuleObjects(ByVal links As List(Of ModuleLink))
    Public Event AddedModuleObjects(ByVal items As List(Of ModuleActionLink))
    Public Event EmptyUpload()
    Public Event ClientUploadHandler(ByVal sender As Object, ByVal e As EventArgs)

    Public Property MaxFileInput As Integer
        Get
            Return Me.CTRLinternalUpload.MaxFileInputsCount
        End Get
        Set(ByVal value As Integer)
            Me.CTRLinternalUpload.MaxFileInputsCount = value
            Me.CTRLRepositoryUpload.MaxFileInputsCount = value
        End Set
    End Property

    Private _PageScriptManager As ScriptManager
    Public Property PageScriptManager As ScriptManager
        Get
            Return _PageScriptManager
        End Get
        Set(ByVal value As ScriptManager)
            _PageScriptManager = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Me.PageScriptManager) Then
            Select Case Me.MLVcontrol.GetActiveView.UniqueID
                Case VIWdownloadActionCommunityFile.UniqueID
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileBottom)
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileTop)
                Case VIWdownloadActionInternalFile.UniqueID
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemBottom)
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemTop)
            End Select
        End If
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(Me.BTNaddInternalToItemTop, True)
            .setButton(Me.BTNaddInternalToItemBottom, True)

            .setButton(Me.BTNaddCommunityFileTop, True)
            .setButton(Me.BTNaddCommunityFileBottom, True)

            .setButton(BTNselectActionTop, True)
            .setButton(BTNselectActionBottom, True)
            .setButton(BTNcloseAddActionWindowBottom, True)
            .setButton(BTNcloseAddActionWindowTop, True)

            .setButton(BTNLinkToModuleBottom, True)
            .setButton(BTNLinkToModuleTop, True)
            .setButton(BTNselectFolderTop, True)
            .setButton(BTNselectFolderBottom, True)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub AjaxInitializeControl(ByVal idCommunity As Integer, ByVal sourceModuleCode As String, ByVal sourceModuleIdAction As Integer, ByVal internalType As FileRepositoryType, ByVal ScriptManager As ScriptManager)
        Me.PageScriptManager = ScriptManager
        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        Me.CurrentPresenter.InitView(idCommunity, sourceModuleCode, sourceModuleIdAction, internalType, New List(Of Long))
    End Sub
    Public Sub AjaxInitializeControlRemovingFiles(ByVal idCommunity As Integer, ByVal sourceModuleCode As String, ByVal sourceModuleIdAction As Integer, ByVal internalType As FileRepositoryType, ByVal ScriptManager As ScriptManager, ByVal items As List(Of Long))
        Me.PageScriptManager = ScriptManager
        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        Me.CurrentPresenter.InitView(idCommunity, sourceModuleCode, sourceModuleIdAction, internalType, items)
    End Sub
    Public Sub InitializeControl(ByVal idCommunity As Integer, ByVal sourceModuleCode As String, ByVal sourceModuleIdAction As Integer, ByVal internalType As FileRepositoryType) Implements IViewModuleToRepository.InitializeControl
        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        Me.CurrentPresenter.InitView(idCommunity, sourceModuleCode, sourceModuleIdAction, internalType, New List(Of Long))
    End Sub
    Public Sub InitializeControlRemovingFiles(ByVal idCommunity As Integer, ByVal sourceModuleCode As String, ByVal sourceModuleIdAction As Integer, ByVal internalType As FileRepositoryType, ByVal items As List(Of Long)) Implements IViewModuleToRepository.InitializeControlRemovingFiles
        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        Me.CurrentPresenter.InitView(idCommunity, sourceModuleCode, sourceModuleIdAction, internalType, items)
    End Sub

    Private Sub InitializeAjaxPostBack()
        If Not IsNothing(Me.PageScriptManager) Then
            Select Case Me.MLVcontrol.GetActiveView.UniqueID
                Case VIWdownloadActionCommunityFile.UniqueID
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileBottom)
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileTop)
                Case VIWdownloadActionInternalFile.UniqueID
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemBottom)
                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemTop)
            End Select
        End If
    End Sub
    Private Sub InitializeCommunityUploader(idFolder As Long, idCommunity As Integer, permission As lm.Comol.Core.DomainModel.CoreModuleRepository) Implements IViewModuleToRepository.InitializeCommunityUploader
        Me.CTRLRepositoryUpload.InitializeControl(0, idCommunity, permission)
    End Sub
    Private Sub InitializeFileSelector(idCommunity As Integer, selectedFiles As List(Of Long), showHiddenItems As Boolean, forAdminPurpose As Boolean, type As Repository.RepositoryItemType) Implements IViewModuleToRepository.InitializeFileSelector
        Me.CTRLselectCommunityFile.InitializeControl(idCommunity, selectedFiles, showHiddenItems, forAdminPurpose, True, type)
    End Sub
#End Region
    Private Sub LoadAvailableActions(actions As List(Of UserRepositoryAction), dAction As UserRepositoryAction) Implements IViewModuleToRepository.LoadAvailableActions
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.DisplayAction(dAction)
    End Sub

    Private Sub DisplayAction(action As UserRepositoryAction) Implements IViewModuleToRepository.DisplayAction
        Me.BTNselectFolderTop.Visible = False
        Me.BTNselectFolderBottom.Visible = False
        Me.BTNLinkToModuleBottom.Visible = False
        Me.BTNLinkToModuleTop.Visible = False
        Me.BTNselectFolderBottom.Visible = False
        Me.BTNselectFolderTop.Visible = False
        Me.BTNaddCommunityFileTop.Visible = False
        Me.BTNaddCommunityFileBottom.Visible = False
        Me.BTNaddInternalToItemBottom.Visible = False
        Me.BTNaddInternalToItemTop.Visible = False
        Select Case action
            Case UserRepositoryAction.CommunityUploadPlay
                MLVcontrol.SetActiveView(VIWdownloadActionCommunityFile)
                Me.BTNaddCommunityFileTop.Visible = True
                Me.BTNaddCommunityFileBottom.Visible = True
            Case UserRepositoryAction.CreateFolder
                MLVcontrol.SetActiveView(VIWuploadFileAction)
                Me.BTNselectFolderBottom.Visible = True
                Me.BTNselectFolderTop.Visible = True
            Case UserRepositoryAction.UploadFile
                MLVcontrol.SetActiveView(VIWuploadFileAction)
            Case UserRepositoryAction.InternalUploadPlay
                MLVcontrol.SetActiveView(VIWdownloadActionInternalFile)
                Me.BTNaddInternalToItemBottom.Visible = True
                Me.BTNaddInternalToItemTop.Visible = True
            Case UserRepositoryAction.LinkForDownload, UserRepositoryAction.LinkForMultimedia, UserRepositoryAction.LinkForScorm
                MLVcontrol.SetActiveView(VIWdownloadActionSelectFile)
                Me.BTNLinkToModuleBottom.Visible = True
                Me.BTNLinkToModuleTop.Visible = True
                Me.CTRLselectCommunityFile.UnselectAll()
            Case UserRepositoryAction.SelectAction
                MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        End Select
        CurrentAction = action
        LTcurrentAction.Text = GetActionTitle(action)
        If Me.AjaxViewUpdate Then
            Me.DVcommandsBottom.Visible = (action <> UserRepositoryAction.SelectAction)
            Me.DVcommandsTop.Visible = (action <> UserRepositoryAction.SelectAction)
            Me.InitializeAjaxPostBack()
            RaiseEvent ContainerCommands(action = UserRepositoryAction.SelectAction)
        End If

    End Sub
    Private Sub DisplaySessionTimeout(idCommunity As Integer, idModule As Integer) Implements IViewModuleToRepository.DisplaySessionTimeout
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub InitializeInternalUploader(idCommunity As Integer) Implements IViewModuleToRepository.InitializeInternalUploader
        Me.CTRLinternalUpload.InitializeControl(idCommunity)
    End Sub
    Private Sub LinkCommunityFiles(items As List(Of ModuleLink)) Implements IViewModuleToRepository.LinkCommunityFiles
        RaiseEvent LinkedModuleObjects(items)
    End Sub

    Private Sub UploadedInternalFile(items As List(Of ModuleActionLink)) Implements IViewModuleToRepository.UploadedInternalFile
        RaiseEvent AddedModuleObjects(items)
    End Sub
    Public Sub RemoveUploadedInternalFiles(items As List(Of iModuleObject)) Implements IViewModuleToRepository.RemoveUploadedInternalFiles
        Dim CommunityPath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.IdCommunityRepository
        Else
            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.IdCommunityRepository
        End If
        CommunityPath &= "\"
        CommunityPath = Replace(CommunityPath, "\\", "\")

        Dim oManager As New lm.Comol.Modules.Base.BusinessLogic.ManagerCommunityFiles(PageUtility.CurrentContext)
        Dim oFiles As List(Of Long) = (From f In items Select CType(f.ObjectOwner, ModuleInternalFile).Id).ToList
        oManager.DeleteFiles(oFiles, CommunityPath)

    End Sub

    Public Sub UpdateModuleInternalFile(items As List(Of ModuleLink)) Implements IViewModuleToRepository.UpdateModuleInternalFile
        Me.CurrentPresenter.UpdateModuleInternalFile(items)
    End Sub
    Private Sub LoadFolderSelector(idExcludeFolder As Long, idFolder As Long, idCommunity As Integer, showHiddenItems As Boolean, forAdminPurpose As Boolean) Implements lm.Comol.Core.BaseModules.Repository.Presentation.IViewModuleToRepository.LoadFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(idCommunity, idFolder, showHiddenItems, forAdminPurpose, idExcludeFolder)
    End Sub
#Region "NOT implemented"
    Private Sub AddCommunityFileAction(idCommunity As Integer, idModule As Integer) Implements IViewModuleToRepository.AddCommunityFileAction

    End Sub
    Private Sub AddInternalFileAction(idCommunity As Integer, idModule As Integer) Implements IViewModuleToRepository.AddInternalFileAction

    End Sub
    Private Sub LoadEmptyUploaders() Implements IViewModuleToRepository.LoadEmptyUploaders

    End Sub
    Private Sub LoadErrorIDtype() Implements IViewModuleToRepository.LoadErrorIDtype

    End Sub
    Private Sub LoadErrorUploading(files As List(Of dtoUploadedFile)) Implements IViewModuleToRepository.LoadErrorUploading

    End Sub
    
#End Region

    Public Sub ChangeDisplayAction(action As UserRepositoryAction) Implements IViewModuleToRepository.ChangeDisplayAction
        Me.DisplayAction(action)
    End Sub

    Private Function GetActionTitle(action As UserRepositoryAction) As String Implements IViewModuleToRepository.GetActionTitle
        Return Resource.getValue("UserRepositoryAction.ActionTitle." & action.ToString)
    End Function

#End Region

#Region ""
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As UserRepositoryAction = DirectCast(e.Item.DataItem, UserRepositoryAction)

            Dim oButton As Button = e.Item.FindControl("BTNaddAction")
            oButton.Text = Resource.getValue("UserRepositoryAction.ButtonText." & action.ToString())
            oButton.ToolTip = Resource.getValue("UserRepositoryAction.ToolTip." & action.ToString())
            oButton.CommandName = action.ToString
            Dim oLiteral As Literal = e.Item.FindControl("LTaddAction")
            oLiteral.Text = Resource.getValue("UserRepositoryAction.Description." & action.ToString())
        End If
    End Sub
    Private Sub RPTactions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTactions.ItemCommand
        Dim action As UserRepositoryAction
        action = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of UserRepositoryAction).GetByString(e.CommandName, UserRepositoryAction.SelectAction)
        If action <> UserRepositoryAction.None Then
            Me.CurrentPresenter.ChangeAction(action)
        End If
    End Sub

#Region "Add existing files"
    Private Sub BTNLinkToModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNLinkToModuleBottom.Click, BTNLinkToModuleTop.Click
        Me.CurrentPresenter.AddLinkToCommunityFile(Me.CTRLselectCommunityFile.GetSelectedFiles)
    End Sub
#End Region
#Region "Add Internal file"
    Private Sub BTNaddToItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddInternalToItemTop.Click, BTNaddInternalToItemBottom.Click
        Me.CurrentPresenter.AddInternalFile(Me.CTRLinternalUpload.AddModuleInternalFiles(Me.InternalFileType, Nothing, SourceModuleCode, SourceModuleIdAction, 0))
    End Sub
#End Region
#Region "Add Community file"
    Private Sub BTNaddCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddCommunityFileTop.Click, BTNaddCommunityFileBottom.Click
        Me.CurrentPresenter.AddCommunityFile(Me.CTRLRepositoryUpload.AddFilesToCommunityRepository, True)
    End Sub
#End Region
#Region "Upload File"

#End Region

    Private Sub BTNselectActionBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectActionBottom.Click, BTNselectActionTop.Click
        Me.CurrentPresenter.ChangeAction(UserRepositoryAction.SelectAction)
    End Sub
#End Region

   
    Private Sub BTNcloseAddActionWindowBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcloseAddActionWindowBottom.Click, BTNcloseAddActionWindowTop.Click
        RaiseEvent CloseClientWindow()
    End Sub
End Class

#Region "Old"


'Imports lm.Comol.Modules.Base.Presentation
'Imports COL_BusinessLogic_v2.UCServices
'Imports lm.ActionDataContract
'Imports lm.Comol.Modules.Base.DomainModel
'Imports lm.Comol.UI.Presentation
'Imports lm.Comol.Core.DomainModel


'Public Class UC_ModuleToRepository
'    Inherits BaseControlWithLoad
'    Implements ImoduleToRepository

'    Public Event UpdateAjaxPanel()
'    Public Event LinkedModuleObjects(ByVal links As List(Of ModuleLink))
'    Public Event AddedModuleObjects(ByVal items As List(Of ModuleActionLink))
'    Public Event EmptyUpload()
'    Public Event ClientUploadHandler(ByVal sender As Object, ByVal e As EventArgs)

'#Region "Iview"
'    Private _BaseUrl As String
'    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
'    Private _Presenter As ModuleToRepositoryPresenter
'    Private _CommunityRepositoryPermission As List(Of ModuleCommunityRepository)

'    Public Overrides ReadOnly Property AlwaysBind As Boolean
'        Get
'            Return False
'        End Get
'    End Property
'    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
'        Get
'            If IsNothing(_CurrentContext) Then
'                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
'            End If
'            Return _CurrentContext
'        End Get
'    End Property

'    Public Overloads ReadOnly Property BaseUrl() As String
'        Get
'            If _BaseUrl = "" Then
'                _BaseUrl = Me.PageUtility.BaseUrl
'            End If
'            Return _BaseUrl
'        End Get
'    End Property
'    Private ReadOnly Property CurrentPresenter() As ModuleToRepositoryPresenter
'        Get
'            If IsNothing(_Presenter) Then
'                _Presenter = New ModuleToRepositoryPresenter(Me.CurrentContext, Me)
'            End If
'            Return _Presenter
'        End Get
'    End Property
'#End Region

'#Region "Iview"
'    Private Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements ImoduleToRepository.CommunityRepositoryPermission
'        Dim oModule As ModuleCommunityRepository = Nothing

'        oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
'                   Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
'        If IsNothing(oModule) Then
'            oModule = New ModuleCommunityRepository
'        End If
'        Return oModule
'    End Function

'    Private WriteOnly Property AllowCommunityUpload() As Boolean Implements ImoduleToRepository.AllowCommunityUpload
'        Set(ByVal value As Boolean)
'            Me.DVcommunityUpload.Visible = value
'        End Set
'    End Property

'    Private WriteOnly Property AllowCommunityLink() As Boolean Implements ImoduleToRepository.AllowCommunityLink
'        Set(ByVal value As Boolean)
'            Me.DVcommunityLink.Visible = value
'        End Set
'    End Property
'    Public Property AjaxViewUpdate As Boolean Implements ImoduleToRepository.AjaxViewUpdate
'        Get
'            If TypeOf Me.ViewState("AjaxViewUpdate") Is Boolean Then
'                Return CBool(Me.ViewState("AjaxViewUpdate"))
'            Else
'                Return False
'            End If
'        End Get
'        Set(ByVal value As Boolean)
'            Me.ViewState("AjaxViewUpdate") = value
'        End Set
'    End Property
'    Private ReadOnly Property SourceModuleCode As String Implements ImoduleToRepository.SourceModuleCode
'        Get
'            Return Me.ViewState("SourceModuleCode")
'        End Get
'    End Property
'    Private ReadOnly Property ServiceOwnerActionID As Integer Implements ImoduleToRepository.ServiceOwnerActionID
'        Get
'            If IsNumeric(Me.ViewState("ServiceOwnerActionID")) Then
'                Return Me.ViewState("ServiceOwnerActionID")
'            Else
'                Return 0
'            End If
'        End Get
'    End Property
'    Private Property RepositoryCommunityID As Integer Implements ImoduleToRepository.RepositoryCommunityID
'        Get
'            If IsNumeric(Me.ViewState("RepositoryCommunityID")) Then
'                Return Me.ViewState("RepositoryCommunityID")
'            Else
'                Return 0
'            End If
'        End Get
'        Set(ByVal value As Integer)
'            Me.ViewState("RepositoryCommunityID") = value
'        End Set
'    End Property
'    Public Property InternalFileType As FileRepositoryType Implements ImoduleToRepository.InternalFileType
'        Get
'            If TypeOf Me.ViewState("InternalFileType") Is FileRepositoryType Then
'                Return CType(Me.ViewState("InternalFileType"), FileRepositoryType)
'            Else
'                Return FileRepositoryType.None
'            End If
'        End Get
'        Set(ByVal value As FileRepositoryType)
'            Me.ViewState("InternalFileType") = value
'        End Set
'    End Property

'    Public Property MaxFileInput As Integer
'        Get
'            Return Me.CTRLinternalUpload.MaxFileInputsCount
'        End Get
'        Set(ByVal value As Integer)
'            Me.CTRLinternalUpload.MaxFileInputsCount = value
'            Me.CTRLRepositoryUpload.MaxFileInputsCount = value
'        End Set
'    End Property
'    Private _PageScriptManager As ScriptManager
'    Public Property PageScriptManager As ScriptManager
'        Get
'            Return _PageScriptManager
'        End Get
'        Set(ByVal value As ScriptManager)
'            _PageScriptManager = value
'        End Set
'    End Property

'#End Region

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        If Not IsNothing(Me.PageScriptManager) Then
'            Select Case Me.MLVcontrol.GetActiveView.UniqueID
'                Case VIWdownloadActionCommunityFile.UniqueID
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileBottom)
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileTop)
'                Case VIWdownloadActionInternalFile.UniqueID
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemBottom)
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemTop)
'            End Select

'        End If
'    End Sub

'#Region "inherited"

'    Public Overrides Sub BindDati()

'    End Sub

'    Public Overrides Sub SetCultureSettings()
'        MyBase.SetCulture("pg_ModuleToRepository", "Modules", "Repository")
'    End Sub

'    Public Overrides Sub SetInternazionalizzazione()
'        With MyBase.Resource
'            .setLiteral(LTdownloadExistCommunityFileActivity)
'            .setButton(BTNaddDownloadExistCommunityFileActivity, True, , , True)
'            .setLiteral(LTdownloadInternalFileActivity)
'            .setButton(BTNaddDownloadInternalFileActivity, True, , , True)
'            .setLiteral(LTdownloadCommunityFileActivity)
'            .setButton(BTNaddDownloadCommunityFileActivity, True, , , True)
'            .setLiteral(LTaddUploadActivity)
'            .setButton(BTNaddUploadActivity, True, , , True)
'            .setLiteral(LTaddFolderActivity)
'            .setButton(BTNaddFolderActivity, True, , , True)
'            .setLiteral(Me.LTselectFromCommunity_t)
'            .setButton(Me.BTNbackToActionSelector, True, , , True)
'            .setButton(Me.BTNLinkToModule, True, , , True)

'            .setLiteral(LTuploadToItem_t)
'            .setButton(Me.BTNbackToActionSelectorTopFromInternal, True, , , True)
'            .setButton(Me.BTNaddInternalToItemTop, True, , , True)
'            .setButton(Me.BTNbackToActionSelectorBottomFromInternal, True, , , True)
'            .setButton(Me.BTNaddInternalToItemBottom, True, , , True)

'            .setLiteral(LTuploadToCommunity_t)

'            .setButton(Me.BTNbackToActionSelectorTopFromCommunity, True, , , True)
'            .setButton(Me.BTNaddCommunityFileTop, True, , , True)
'            .setButton(Me.BTNbackToActionSelectorBottomFromCommunity, True, , , True)
'            .setButton(Me.BTNaddCommunityFileBottom, True, , , True)



'            .setLiteral(LTuploadFileAction)
'            .setButton(Me.BTNbackToASfromUploadFile, True, , , True)
'            .setButton(Me.BTNselectFolder, True, , , True)
'        End With
'    End Sub
'#End Region

'#Region "Initializers"
'    Public Sub AjaxInitializeControl(ByVal CommunityID As Integer, ByVal SourceModuleCode As String, ByVal ServiceOwnerActionID As Integer, ByVal InternalFileType As FileRepositoryType, ByVal ScriptManager As ScriptManager)
'        Me.PageScriptManager = ScriptManager
'        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
'        Me.ViewState("SourceModuleCode") = SourceModuleCode
'        Me.ViewState("ServiceOwnerActionID") = ServiceOwnerActionID
'        Me.CurrentPresenter.InitView(CommunityID, SourceModuleCode, New List(Of Long), InternalFileType)
'    End Sub
'    Public Sub AjaxInitializeControlRemovingFiles(ByVal CommunityID As Integer, ByVal SourceModuleCode As String, ByVal ServiceOwnerActionID As Integer, ByVal FilesToRemove As List(Of Long), ByVal InternalFileType As FileRepositoryType, ByVal ScriptManager As ScriptManager)
'        Me.PageScriptManager = ScriptManager
'        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
'        Me.ViewState("SourceModuleCode") = SourceModuleCode
'        Me.ViewState("ServiceOwnerActionID") = ServiceOwnerActionID
'        Me.CurrentPresenter.InitView(CommunityID, SourceModuleCode, FilesToRemove, InternalFileType)
'    End Sub
'    Public Sub InitializeControl(ByVal CommunityID As Integer, ByVal SourceModuleCode As String, ByVal ServiceOwnerActionID As Integer, ByVal InternalFileType As FileRepositoryType) Implements ImoduleToRepository.InitializeControl
'        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
'        Me.ViewState("SourceModuleCode") = SourceModuleCode
'        Me.ViewState("ServiceOwnerActionID") = ServiceOwnerActionID
'        Me.CurrentPresenter.InitView(CommunityID, SourceModuleCode, New List(Of Long), InternalFileType)
'    End Sub
'    Public Sub InitializeControlRemovingFiles(ByVal CommunityID As Integer, ByVal SourceModuleCode As String, ByVal ServiceOwnerActionID As Integer, ByVal FilesToRemove As List(Of Long), ByVal InternalFileType As FileRepositoryType) Implements ImoduleToRepository.InitializeControlRemovingFiles
'        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
'        Me.ViewState("SourceModuleCode") = SourceModuleCode
'        Me.ViewState("ServiceOwnerActionID") = ServiceOwnerActionID
'        Me.CurrentPresenter.InitView(CommunityID, SourceModuleCode, FilesToRemove, InternalFileType)
'    End Sub

'    Private Sub InitializeAjaxPostBack()
'        If Not IsNothing(Me.PageScriptManager) Then
'            Select Case Me.MLVcontrol.GetActiveView.UniqueID
'                Case VIWdownloadActionCommunityFile.UniqueID
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileBottom)
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddCommunityFileTop)
'                Case VIWdownloadActionInternalFile.UniqueID
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemBottom)
'                    Me.PageScriptManager.RegisterPostBackControl(Me.BTNaddInternalToItemTop)
'            End Select
'        End If
'    End Sub
'    Private Sub InitializeCommunityUploader(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oPermission As ModuleCommunityRepository) Implements ImoduleToRepository.InitializeCommunityUploader
'        Me.CTRLRepositoryUpload.InitializeControl(0, CommunityID, oPermission)
'    End Sub
'    Private Sub InitializeFileSelector(ByVal CommunityID As Integer, ByVal SelectedFiles As System.Collections.Generic.List(Of Long), ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements ImoduleToRepository.InitializeFileSelector
'        Me.CTRLselectCommunityFile.InitializeControl(CommunityID, SelectedFiles, ShowHiddenItems, AdminPurpose)
'    End Sub
'#End Region

'#Region "View selectors"
'    Private Sub LoadActionsSelector() Implements ImoduleToRepository.LoadActionsSelector
'        Me.MLVcontrol.SetActiveView(VIWactionRepositorySelector)
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'    End Sub
'    Private Sub LoadFilesSelector() Implements ImoduleToRepository.LoadFilesSelector
'        Me.CTRLselectCommunityFile.UnselectAllFiles()
'        Me.MLVcontrol.SetActiveView(VIWdownloadActionSelectFile)
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'        Me.InitializeAjaxPostBack()
'    End Sub
'    Private Sub LoadAddCommunityFilesSelector() Implements ImoduleToRepository.LoadAddCommunityFilesSelector
'        Me.MLVcontrol.SetActiveView(VIWdownloadActionCommunityFile)
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'        Me.InitializeAjaxPostBack()
'    End Sub
'    Private Sub LoadAddInternalFilesSelector() Implements ImoduleToRepository.LoadAddInternalFilesSelector
'        Me.MLVcontrol.SetActiveView(VIWdownloadActionInternalFile)
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'        Me.InitializeAjaxPostBack()
'    End Sub
'    Private Sub BTNbackToActionSelector_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNbackToActionSelector.Click, BTNbackToActionSelectorBottomFromCommunity.Click, BTNbackToActionSelectorBottomFromInternal.Click, BTNbackToActionSelectorTopFromCommunity.Click, BTNbackToActionSelectorTopFromInternal.Click
'        Me.LoadActionsSelector()
'        'Me.CTRLselectCommunityFile.UnselectAllFiles()
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'    End Sub

'    Private Sub BTNaddDownloadExistCommunityFileActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddDownloadExistCommunityFileActivity.Click
'        Me.LoadFilesSelector()
'    End Sub
'    Private Sub BTNaddDownloadCommunityFileActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddDownloadCommunityFileActivity.Click
'        Me.LoadAddCommunityFilesSelector()
'    End Sub

'    Private Sub BTNaddDownloadInternalFileActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddDownloadInternalFileActivity.Click
'        Me.LoadAddInternalFilesSelector()
'    End Sub

'    Private Sub BTNaddFolderActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddFolderActivity.Click

'    End Sub
'    Private Sub BTNaddUploadActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddUploadActivity.Click
'        LoadUploadFileSelector()
'    End Sub
'    Private Sub LoadUploadFileSelector() Implements ImoduleToRepository.LoadUploadFileSelector
'        Me.MLVcontrol.SetActiveView(VIWuploadFileAction)
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'    End Sub
'    Private Sub BTNbackToASfromUploadFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNbackToASfromUploadFile.Click
'        Me.LoadActionsSelector()
'    End Sub
'#End Region

'#Region "Add existing files"
'    Private Sub BTNLinkToModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNLinkToModule.Click
'        Me.CurrentPresenter.AddLinkToCommunityFile(Me.CTRLselectCommunityFile.GetSelectedFiles)
'    End Sub
'#End Region

'#Region "Add Internal file"
'    Private Sub BTNaddToItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddInternalToItemTop.Click, BTNaddInternalToItemBottom.Click
'        Me.CurrentPresenter.AddInternalFile(Me.CTRLinternalUpload.AddModuleInternalFiles(Me.InternalFileType, Nothing, SourceModuleCode, ServiceOwnerActionID, 0))
'    End Sub
'#End Region

'#Region "Add Community file"
'    Private Sub BTNaddCommunityFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNaddCommunityFileTop.Click, BTNaddCommunityFileBottom.Click
'        Me.CurrentPresenter.AddCommunityFile(Me.CTRLRepositoryUpload.AddFilesToCommunityRepository, True)
'    End Sub
'#End Region

'#Region "Upload File"

'    Public Sub LoadFolderSelector(ByVal ExludeFolderID As Long, ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements ImoduleToRepository.LoadFolderSelector
'        Me.CTRLCommunityFolder.InitializeControl(CommunityID, FolderID, ShowHiddenItems, AdminPurpose, ExludeFolderID)
'        Me.BTNaddUploadActivity.Visible = Me.CTRLCommunityFolder.HasMoreFolder
'    End Sub

'#End Region

'#Region "Action"
'    Public Sub AddCommunityFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements ImoduleToRepository.AddCommunityFileAction

'    End Sub
'    Public Sub AddInternalFileAction(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements ImoduleToRepository.AddInternalFileAction

'    End Sub
'#End Region

'    Public Sub InitializeInternalUploader(ByVal CommunityID As Integer) Implements ImoduleToRepository.InitializeInternalUploader
'        Me.CTRLinternalUpload.InitializeControl(CommunityID)
'    End Sub

'    Public Sub RemoveUploadedInternalFiles(ByVal items As List(Of iModuleObject)) Implements ImoduleToRepository.RemoveUploadedInternalFiles
'        Dim CommunityPath As String = ""
'        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
'            CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\" & Me.RepositoryCommunityID
'        Else
'            CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & "\" & Me.RepositoryCommunityID
'        End If
'        CommunityPath &= "\"
'        CommunityPath = Replace(CommunityPath, "\\", "\")

'        Me.CurrentPresenter.RemoveUploadedInternalFiles(items, CommunityPath)
'    End Sub

'    Public Sub UpdateModuleInternalFile(ByVal items As List(Of ModuleLink)) Implements ImoduleToRepository.UpdateModuleInternalFile
'        Me.CurrentPresenter.UpdateModuleInternalFile(items)
'    End Sub

'    Public Sub UploadedInternalFile(ByVal items As List(Of ModuleActionLink)) Implements ImoduleToRepository.UploadedInternalFile
'        RaiseEvent AddedModuleObjects(items)
'    End Sub
'    Public Sub LoadSessionTimeout(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements ImoduleToRepository.LoadSessionTimeout

'    End Sub
'    Public Sub LinkCommunityFiles(ByVal links As List(Of ModuleLink)) Implements ImoduleToRepository.LinkCommunityFiles
'        RaiseEvent LinkedModuleObjects(links)
'    End Sub

'#Region "Errors"
'    Public Sub LoadErrorUploading(ByVal Files As System.Collections.Generic.List(Of dtoUploadedFile)) Implements ImoduleToRepository.LoadErrorUploading

'    End Sub

'    Public Sub LoadEmptyUploaders() Implements lm.Comol.Modules.Base.Presentation.ImoduleToRepository.LoadEmptyUploaders

'    End Sub
'    Public Sub LoadErrorIDtype() Implements lm.Comol.Modules.Base.Presentation.ImoduleToRepository.LoadErrorIDtype
'        'Me.MLVcontrol.SetActiveView(VIWselecto)
'        'Me.CTRLselectCommunityFile.UnselectAllFiles()
'        'If Me.AjaxViewUpdate Then
'        '    RaiseEvent UpdateAjaxPanel()
'        'End If
'    End Sub
'#End Region



'End Class
#End Region