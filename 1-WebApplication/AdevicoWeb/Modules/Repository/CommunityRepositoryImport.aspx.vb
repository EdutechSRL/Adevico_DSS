Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.Repository.Domain
Partial Public Class CommunityRepositoryImport
    Inherits PageBase
    Implements IviewImportItemsIntoRepository

#Region "Inherits"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As CRimportPresenter
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
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CRimportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRimportPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Interface"

    Public Property VisibleToAll() As Boolean Implements IviewImportItemsIntoRepository.VisibleToAll
        Get
            Return CBool(RBLvisibleTo.SelectedValue)
        End Get
        Set(ByVal value As Boolean)
            Me.RBLvisibleTo.SelectedValue = value
        End Set
    End Property
    Public Property SelectedFolder() As Long Implements IviewImportItemsIntoRepository.SelectedFolder
        Get
            SelectedFolder = Me.CTRLCommunityFolder.SelectedFolder
        End Get
        Set(ByVal value As Long)
            Me.CTRLCommunityFolder.SelectedFolder = value
            Me.UDPselectFolder.Update()
        End Set
    End Property
    Private WriteOnly Property FilePath() As String Implements IviewImportItemsIntoRepository.FilePath
        Set(ByVal value As String)
            Me.LBpath.Text = value
        End Set
    End Property
    Public ReadOnly Property SelectedFolderName() As String Implements IviewImportItemsIntoRepository.SelectedFolderName
        Get
            Return Me.CTRLCommunityFolder.SelectedFolderName
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleCommunityRepository Implements IviewImportItemsIntoRepository.CommunityRepositoryPermission
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
    Public WriteOnly Property AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean Implements IviewImportItemsIntoRepository.AllowManagement
        Set(ByVal value As Boolean)
            Dim url As String = Me.BaseUrl & RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView)
            Me.HYPbackToFileManagement.Visible = value
            If value Then
                Me.HYPbackToFileManagement.NavigateUrl = url 'Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If

            Dim oHyperLink As HyperLink
            oHyperLink = Me.WZRimportCommunityItem.FindControl("StartNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperLink) Then
                oHyperLink.Visible = value
                oHyperLink.NavigateUrl = url ' Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
            oHyperLink = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperLink) Then
                oHyperLink.Visible = value
                oHyperLink.NavigateUrl = url 'Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
            oHyperLink = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperLink) Then
                oHyperLink.Visible = value
                oHyperLink.NavigateUrl = url 'Me.BaseUrl & "Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID
            End If
        End Set
    End Property

    Public ReadOnly Property PreLoadedPageIndex() As Integer Implements IviewImportItemsIntoRepository.PreLoadedPageIndex
        Get
            If IsNumeric(Request.QueryString("Page")) Then
                Return CInt(Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedFolderID() As Long Implements IviewImportItemsIntoRepository.PreLoadedFolderID
        Get
            If Not IsNumeric(Request.QueryString("FolderID")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("FolderID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedCommunityID() As Integer Implements IviewImportItemsIntoRepository.PreLoadedCommunityID
        Get
            If Not IsNumeric(Request.QueryString("CommunityID")) Then
                Return 0
            Else
                Return CInt(Request.QueryString("CommunityID"))
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedView() As IViewExploreCommunityRepository.ViewRepository Implements IviewImportItemsIntoRepository.PreLoadedView
        Get
            If IsNothing(Request.QueryString("PreviousView")) Then
                Return IViewExploreCommunityRepository.ViewRepository.FileList
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of IViewExploreCommunityRepository.ViewRepository).GetByString(Request.QueryString("PreviousView"), IViewExploreCommunityRepository.ViewRepository.FileList)
            End If
        End Get
    End Property
    Public Property SourceCommunityID() As Integer Implements IviewImportItemsIntoRepository.SourceCommunityID
        Get
            Dim oID As List(Of Integer) = Me.CTRLcommunity.SelectedCommunitiesID
            If oID.Count > 0 Then
                Return oID(0)
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Dim oID As New List(Of Integer)
            oID.Add(value)
            Me.CTRLcommunity.SelectedCommunitiesID = oID
        End Set
    End Property
    Public Property DestinationCommunityID() As Integer Implements IviewImportItemsIntoRepository.DestinationCommunityID
        Get
            If IsNumeric(Me.ViewState("DestinationCommunityID")) Then
                Return CInt(Me.ViewState("DestinationCommunityID"))
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("DestinationCommunityID") = value
        End Set
    End Property


    Private ReadOnly Property Portalname() As String Implements IviewImportItemsIntoRepository.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property
    Public WriteOnly Property TitleCommunity() As String Implements IviewImportItemsIntoRepository.TitleCommunity
        Set(ByVal value As String)
            Dim CommunityName As String = value
            If Len(CommunityName) > 62 Then
                CommunityName = Left(value, 32) & " ... " & Right(value, 20)
            End If
            Me.Master.ServiceTitle = String.Format(Me.Resource.getValue("serviceTitleCommunityName"), CommunityName)
            Me.Master.ServiceTitleToolTip = String.Format(Me.Resource.getValue("serviceTitleCommunityName"), value)
        End Set
    End Property
    Public WriteOnly Property DestinationCommunityName() As String Implements IviewImportItemsIntoRepository.DestinationCommunityName
        Set(ByVal value As String)
            Me.Resource.setLabel(Me.LBfileToExport)
            Me.LBfileToExport.Text = String.Format(Me.LBfileToExport.Text, value)
        End Set
    End Property

    Public WriteOnly Property SourceCommunityName() As String Implements IviewImportItemsIntoRepository.SourceCommunityName
        Set(ByVal value As String)
            Me.LBcommunity.Text = value
        End Set
    End Property
    Private ReadOnly Property BaseFolder() As String Implements IviewImportItemsIntoRepository.BaseFolder
        Get
            Return Me.Resource.getValue("BaseFolder")
        End Get
    End Property
#End Region

    Private ReadOnly Property RepositoryPath() As String
        Get
            Dim Path As String = ""
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                Path = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\"
            Else
                Path = Me.SystemSettings.File.Materiale.DrivePath & "\"
            End If
            RepositoryPath = Replace(Path, "\\", "\")
        End Get
    End Property
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
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityRepositoryImport", "Generici")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPbackToFileManagement, True, True)

            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(Me.LBfileToExport)
            .setLabel(Me.LBmultipleFileError)
            .setLabel(Me.LBselectFolder)
            .setLabel(Me.LBcommunity_t)
            .setLabel(Me.LBpath_t)
            .setLabel(Me.LBvisibleTo_t)
            .setButton(BTNeditPath, True)
            Me.Resource.setRadioButtonList(RBLvisibleTo, True)
            Me.Resource.setRadioButtonList(RBLvisibleTo, False)
            Dim oButton As Button
            Dim oHyperlink As HyperLink
            oButton = Me.WZRimportCommunityItem.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("StartNavigationTemplateContainerID").FindControl("BTNgoToManagement")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oHyperlink = Me.WZRimportCommunityItem.FindControl("StartNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperlink) Then
                .setHyperLink(oHyperlink, True, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("BTNgoToManagement")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oHyperlink = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperlink) Then
                .setHyperLink(oHyperlink, True, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNgoToManagement")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oHyperlink = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("HYPbackToFileManagement")
            If Not IsNothing(oHyperlink) Then
                .setHyperLink(oHyperlink, True, True)
            End If

        End With
    End Sub
#End Region

#Region "Notification / Action"
    'Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
    '    PageUtility.CurrentModule = PageUtility.GetModule(Services_File.Codex)
    'End Sub
    Public Sub SendActionImportCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IviewImportItemsIntoRepository.SendActionImportCompleted
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.ImportItemsCompleted, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Public Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer) Implements IviewImportItemsIntoRepository.SendActionInit
        Me.PageUtility.AddActionToModule(CommunityID, ModuleID, Services_File.ActionType.ViewImportFoldersPage, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Public Sub NotifyImportedItems(ByVal CommunityID As Integer, ByVal oContext As dtoImportedItem) Implements IviewImportItemsIntoRepository.NotifyImportedItems
        Dim oSender As New RepositoryNotificationUtility(Me.PageUtility)
        Dim BaseFolder As String = Me.BaseFolder

        If oContext.FileCount > 0 Then
            oSender.NotifyFileImport(CommunityID, oContext.FolderID, oContext.FolderName, oContext.FileCount)
        End If
        If oContext.FolderCount > 0 Then
            oSender.NotifyFolderImport(CommunityID, oContext.FolderID, oContext.FolderName, oContext.FolderCount)
        End If
    End Sub
#End Region


#Region "Wizard"
#Region "Manage step "
    Public Sub ShowFoldersList() Implements IviewImportItemsIntoRepository.ShowFoldersList
        Me.WZRimportCommunityItem.MoveTo(Me.WSTfolder)
    End Sub
    Public Sub ShowFileList() Implements IviewImportItemsIntoRepository.ShowFileList
        Me.WZRimportCommunityItem.MoveTo(Me.WSTfile)
    End Sub
    Public Sub ShowRenamedFileList() Implements lm.Comol.Modules.Base.Presentation.IviewImportItemsIntoRepository.ShowRenamedFileList
        Me.WZRimportCommunityItem.MoveTo(Me.WSTmultipleFileError)
    End Sub
    Public Sub ShowSelectCommunity() Implements IviewImportItemsIntoRepository.ShowSelectCommunity
        Me.WZRimportCommunityItem.MoveTo(Me.WSTcommunity)
    End Sub

    Private Sub WZRimportCommunityItem_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRimportCommunityItem.NextButtonClick
        If Me.WZRimportCommunityItem.ActiveStep Is Me.WSTcommunity Then
            If Me.CTRLcommunity.SelectedCommunitiesID.Count = 1 Then
                Me.CurrentPresenter.LoadSourceItems(Me.CTRLcommunity.SelectedCommunitiesID(0))
            Else
                Me.WZRimportCommunityItem.MoveTo(Me.WSTcommunity)
            End If
        ElseIf Me.WZRimportCommunityItem.ActiveStep Is WSTfile Then
            If Me.CTRLsourceItems.GetSelectedItems().Count > 0 Then
                Me.CurrentPresenter.SelectDestinationFolder()
            Else
                Me.WZRimportCommunityItem.MoveTo(Me.WSTfile)
            End If

        ElseIf Me.WZRimportCommunityItem.ActiveStep Is Me.WSTfolder Then
            If Me.CTRLCommunityFolder.SelectedFolder > -1 Then

                Me.CurrentPresenter.TryToImport(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLsourceItems.GetSelectedItemsStructureOld(), RepositoryPath)
            Else
                Me.WZRimportCommunityItem.MoveTo(Me.WSTfolder)
            End If
        ElseIf Me.WZRimportCommunityItem.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.EvaluateNameChanged(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLsourceItems.GetSelectedItemsStructureOld(), RepositoryPath)
        End If
    End Sub
    Private Sub WZRimportCommunityItem_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRimportCommunityItem.PreviousButtonClick
        If Me.WZRimportCommunityItem.ActiveStep Is WSTfolder Then
            Me.WZRimportCommunityItem.MoveTo(Me.WSTfile)
        ElseIf Me.WZRimportCommunityItem.ActiveStep Is WSTfile Then
            Me.WZRimportCommunityItem.MoveTo(Me.WSTcommunity)
        ElseIf Me.WZRimportCommunityItem.ActiveStep Is Me.WSTmultipleFileError Then
            Me.CurrentPresenter.SelectDestinationFolder()
            'ElseIf Me.WZRimportCommunityItem.ActiveStep Is Me.WSTcomplete Then
            '    Me.CurrentPresenter.GotoPreviousFromSummary()
        End If
    End Sub
    Private Sub WZRimportCommunityItem_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRimportCommunityItem.CancelButtonClick
        Me.ReturnToFileManagement(Me.PreLoadedFolderID, Me.PreLoadedCommunityID, Me.PreLoadedView)
    End Sub
    Private Sub WZRimportCommunityItem_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRimportCommunityItem.FinishButtonClick
        If Me.CTRLCommunityFolder.SelectedFolder > -1 Then
            If Me.WZRimportCommunityItem.ActiveStep Is WSTfolder Then
                Me.CurrentPresenter.TryToImport(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLsourceItems.GetSelectedItemsStructureOld(), RepositoryPath)
            Else
                Me.CurrentPresenter.EvaluateNameChanged(Me.CTRLCommunityFolder.SelectedFolder, Me.CTRLsourceItems.GetSelectedItemsStructureOld(), RepositoryPath)
            End If
        Else
            Exit Sub
        End If
        '        Me.CurrentPresenter.PublishIntoCommunityRepository(Server.MapPath(Me.PageUtility.BaseUrl & "file/"), Me.PageUtility.SystemSettings.BaseFileRepositoryPath.DrivePath)
    End Sub
#End Region

    Public Sub InitCommunitySelection(ByVal CommunityID As Integer) Implements IviewImportItemsIntoRepository.InitCommunitySelection
        Me.WZRimportCommunityItem.MoveTo(Me.WSTcommunity)

        Dim oService As Services_File = Services_File.Create
        oService.Upload = True
        oService.Moderate = True
        oService.Admin = True
        oService.Read = True
        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.InitializeControl(CommunityID)

        Dim oNextButton As Button = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Visible = True
        oNextButton.Enabled = Not (Me.CTRLcommunity.SelectedCommunitiesID.Count = 0)
    End Sub
    Private Sub CTRLcommunity_SelectedCommunityChanged(ByVal CommunityID As Integer) Handles CTRLcommunity.SelectedCommunityChanged
        Dim oNextButton As Button = Me.WZRimportCommunityItem.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
        oNextButton.Enabled = (CommunityID >= 0)
    End Sub
    Public Sub InitializeFolderSelector(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowHiddenFiles As Boolean) Implements IviewImportItemsIntoRepository.InitializeFolderSelector
        Me.CTRLCommunityFolder.InitializeControl(False, CommunityID, SelectedFolderID, ShowHiddenFiles, True)
        Me.BTNeditPath.Enabled = Me.CTRLCommunityFolder.HasMoreFolder
        Me.UDPselectFolder.Update()
    End Sub

    Public Sub LoadMultipleFileName(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoFileExist(Of Long))) Implements IviewImportItemsIntoRepository.LoadMultipleFileName
        Me.WZRimportCommunityItem.MoveTo(Me.WSTmultipleFileError)
        Me.RPTfileName.DataSource = oList
        Me.RPTfileName.DataBind()
    End Sub
    Private Sub RPTfileName_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfileName.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDto As dtoFileExist(Of Long) = DirectCast(e.Item.DataItem, dtoFileExist(Of Long))

            Dim oLTfileID, oLTimageFile, oLTfileType As Literal
            Dim oTXBfileName As TextBox
            oLTfileID = e.Item.FindControl("LTfileID")
            oLTfileID.Text = oDto.Id.ToString
            oLTimageFile = e.Item.FindControl("LTimageFile")
            If oDto.Extension = "-1" Then
                oLTimageFile.Text = "<img src='" & BaseUrl & "RadControls/TreeView/Skins/Materiale/folder.gif" & "' alt='" & oDto.ExistFileName & "'>&nbsp;"
            Else
                oLTimageFile.Text = "<img src='" & BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(oDto.Extension) & "' alt='" & oDto.ExistFileName & "'>&nbsp;"
            End If
            oLTfileType = e.Item.FindControl("LTfileType")
            oLTfileType.Text = oDto.Extension

            oTXBfileName = e.Item.FindControl("TXBfileName")
            oTXBfileName.Text = oDto.ProposedFileName

            Dim oLBfileNameToReplace As Label
            oLBfileNameToReplace = e.Item.FindControl("LBfileNameToReplace")
            Me.Resource.setLabel(oLBfileNameToReplace)
            oLBfileNameToReplace.Text = String.Format(oLBfileNameToReplace.Text, oDto.ExistFileName)
        End If
    End Sub
#End Region

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub


    Public Sub ReturnToFileManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) Implements IviewImportItemsIntoRepository.ReturnToFileManagement
        Me.RedirectToUrl(RootObject.RepositoryManagement(FolderID, CommunityID, oView.ToString, PreLoadedContentView))
        ' Me.RedirectToUrl("Modules/Repository/CommunityRepositoryManagement.aspx?FolderID=" & FolderID.ToString & "&View=" & oView.ToString & "&Page=0" & "&CommunityID=" & CommunityID)
    End Sub


    Public WriteOnly Property AllowCommunityImport() As Boolean Implements IviewImportItemsIntoRepository.AllowCommunityImport
        Set(ByVal value As Boolean)
            Dim oButton As Button
            oButton = Me.WZRimportCommunityItem.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                oButton.Enabled = value
            End If
        End Set
    End Property

    Public Sub NoPermissionToAccessPage(ByVal CommunityID As Integer) Implements IviewImportItemsIntoRepository.NoPermissionToAccessPage
        Me.BindNoPermessi()
        Me.PageUtility.AddAction(CommunityID, Services_WorkBook.ActionType.NoPermission, Nothing, lm.ActionDataContract.InteractionType.SystemToSystem)
    End Sub

    Public Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer) Implements IviewImportItemsIntoRepository.NoPermissionToManagementFiles
        Me.MLVimport.ActiveViewIndex = 0
        Me.LBerror.Text = Me.Resource.getValue("NoPermissionToManagementFiles")
    End Sub

    Public Sub NoPermissionToPublishFiles(ByVal CommunityID As Integer) Implements IviewImportItemsIntoRepository.NoPermissionToImportItems
        Me.MLVimport.ActiveViewIndex = 0
        Me.LBerror.Text = Me.Resource.getValue("NoPermissionToPublishFiles")
    End Sub


    Public Function GetChangedFileName() As List(Of dtoFileExist(Of Long)) Implements IviewImportItemsIntoRepository.GetChangedFileName
        Dim oList As New List(Of dtoFileExist(Of Long))
        For Each oRow As RepeaterItem In Me.RPTfileName.Items
            Dim oLTfileID, oLTfileType As Literal
            Dim oTXBfileName As TextBox
            oLTfileID = oRow.FindControl("LTfileID")
            oTXBfileName = oRow.FindControl("TXBfileName")
            oLTfileType = oRow.FindControl("LTfileType")
            oList.Add(New dtoFileExist(Of Long) With {.Id = CLng(oLTfileID.Text), .ProposedFileName = oTXBfileName.Text, .Extension = oLTfileType.Text})
        Next
        Return oList
    End Function

    Public ReadOnly Property CommunitySelectionLoaded() As Boolean Implements IviewImportItemsIntoRepository.CommunitySelectionLoaded
        Get
            Return Me.CTRLcommunity.HasCommunity
        End Get
    End Property

    Public Sub InitializeSourceItemsSelector(ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IviewImportItemsIntoRepository.InitializeSourceItemsSelector
        Me.CTRLsourceItems.InitializeControl(CommunityID, New List(Of Long), ShowHiddenItems, AdminPurpose, True, Repository.RepositoryItemType.None)
        Me.WZRimportCommunityItem.MoveTo(Me.WSTfile)
    End Sub

    Private Sub CTRLCommunityFolder_AjaxFolderSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLCommunityFolder.AjaxFolderSelected
        Me.LBpath.Text = Me.CTRLCommunityFolder.SelectedFolderPathName
        Me.CloseDialog("selectFolder")
        Me.UDPdata.Update()
    End Sub

    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
End Class