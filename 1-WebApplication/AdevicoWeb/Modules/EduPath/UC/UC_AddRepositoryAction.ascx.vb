Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Modules.EduPath.Domain

Public Class UC_EduPathAddRepositoryAction
    Inherits BaseControl
    Implements IViewAddRepositoryAction



#Region "Context"
    Private _Presenter As AddRepositoryActionPresenter
    Private ReadOnly Property CurrentPresenter() As AddRepositoryActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddRepositoryActionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdActivity As Long Implements IViewAddRepositoryAction.IdActivity
        Get
            Return ViewStateOrDefault("IdActivity", 0)
        End Get
        Set(value As Long)
            ViewState("IdActivity") = value
        End Set
    End Property
    Private Property UnloadItems As List(Of Long) Implements IViewAddRepositoryAction.UnloadItems
        Get
            Return ViewStateOrDefault("UnloadItems", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            ViewState("UnloadItems") = value
        End Set
    End Property
    Public Property IsCreateActionAvailable As Boolean Implements IViewAddRepositoryAction.IsCreateActionAvailable
        Get
            Return ViewStateOrDefault("IsCreateActionAvailable", False)
        End Get
        Set(value As Boolean)
            ViewState("IsCreateActionAvailable") = value
        End Set
    End Property
    Private Property Identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IViewAddRepositoryAction.Identifier
        Get
            Return ViewStateOrDefault("Identifier", lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, 0))
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
            ViewState("Identifier") = value
        End Set
    End Property
    Private Property CurrentAction As DisplayRepositoryAction Implements IViewAddRepositoryAction.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", DisplayRepositoryAction.select)
        End Get
        Set(value As DisplayRepositoryAction)
            ViewState("CurrentAction") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event CloseClientWindow()
    Public Event ContainerCommands(ByVal show As Boolean)
    Public Event LinksAdded()
    'Public Event UpdateAjaxPanel()
    'Public Event LinkedModuleObjects(ByVal links As List(Of ModuleLink))
    'Public Event AddedModuleObjects(ByVal items As List(Of ModuleActionLink))
    Public Event EmptyUpload()
    'Public Event ClientUploadHandler(ByVal sender As Object, ByVal e As EventArgs)

    Public Property MaxFileInput As Integer
        Get
            Return CTRLinternalUploader.MaxFileInput
        End Get
        Set(ByVal value As Integer)
            CTRLinternalUploader.MaxFileInput = value
            CTRLrepositoryItemsUploader.MaxFileInput = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ViewActivity", "EduPath")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNaddInternalToItemTop, True)
            .setButton(BTNaddInternalToItemBottom, True)

            .setButton(BTNaddCommunityFileTop, True)
            .setButton(BTNaddCommunityFileBottom, True)

            .setButton(BTNselectActionTop, True)
            .setButton(BTNselectActionBottom, True)
            .setButton(BTNcloseAddActionWindowBottom, True)
            .setButton(BTNcloseAddActionWindowTop, True)

            .setButton(BTNLinkToModuleBottom, True)
            .setButton(BTNLinkToModuleTop, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, idActivity As Long, Optional unloadItems As List(Of Long) = Nothing) Implements IViewAddRepositoryAction.InitializeControl
        CurrentPresenter.InitView(identifier, idActivity, unloadItems)
    End Sub

#Region "Manage view"
    Private Sub LoadAvailableActions(actions As List(Of DisplayRepositoryAction), dAction As DisplayRepositoryAction) Implements IViewAddRepositoryAction.LoadAvailableActions
        RPTactions.DataSource = actions
        RPTactions.DataBind()
        DisplayAction(dAction)
    End Sub
    Private Sub DisplayAction(action As DisplayRepositoryAction) Implements IViewAddRepositoryAction.DisplayAction
        BTNLinkToModuleBottom.Visible = False
        BTNLinkToModuleTop.Visible = False
        BTNaddCommunityFileTop.Visible = False
        BTNaddCommunityFileBottom.Visible = False
        BTNaddInternalToItemBottom.Visible = False
        BTNaddInternalToItemTop.Visible = False
        BTNselectActionTop.Visible = False
        BTNselectActionBottom.Visible = False
        Select Case action
            Case DisplayRepositoryAction.repositoryDownloadOrPlay
                MLVcontrol.SetActiveView(VIWdownloadActionCommunityFile)
                BTNaddCommunityFileTop.Visible = True
                BTNaddCommunityFileBottom.Visible = True
                BTNselectActionTop.Visible = True
                BTNselectActionBottom.Visible = True
            Case DisplayRepositoryAction.internalDownloadOrPlay
                MLVcontrol.SetActiveView(VIWdownloadActionInternalFile)
                BTNaddInternalToItemBottom.Visible = True
                BTNaddInternalToItemTop.Visible = True
                BTNselectActionTop.Visible = True
                BTNselectActionBottom.Visible = True
            Case DisplayRepositoryAction.playMultimedia, DisplayRepositoryAction.downloadItem, DisplayRepositoryAction.playScormPackage
                MLVcontrol.SetActiveView(VIWdownloadActionSelectFile)
                BTNLinkToModuleBottom.Visible = True
                BTNLinkToModuleTop.Visible = True
                BTNselectActionTop.Visible = True
                BTNselectActionBottom.Visible = True
                CTRLlinkItems.UnselectAll()
            Case DisplayRepositoryAction.select
                MLVcontrol.SetActiveView(VIWactionRepositorySelector)
        End Select
        CurrentAction = action
        LTcurrentAction.Text = GetActionTitle(action)

        DVcommandsBottom.Visible = (action <> DisplayRepositoryAction.select)
        DVcommandsTop.Visible = (action <> DisplayRepositoryAction.select)
        RaiseEvent ContainerCommands(action = DisplayRepositoryAction.select)
    End Sub
    Private Sub ChangeDisplayAction(action As DisplayRepositoryAction) Implements IViewAddRepositoryAction.ChangeDisplayAction
        DisplayAction(action)
    End Sub
    Private Function GetActionTitle(action As DisplayRepositoryAction) As String Implements IViewAddRepositoryAction.GetActionTitle
        Return Resource.getValue("DisplayRepositoryAction.ActionTitle." & action.ToString)
    End Function
#End Region

#Region "Manage uploader"
    Private Sub InitializeCommunityUploader(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddRepositoryAction.InitializeCommunityUploader
        CTRLrepositoryItemsUploader.InitializeControl(0, identifier)
    End Sub
    Private Sub InitializeInternalUploader(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddRepositoryAction.InitializeInternalUploader
        CTRLinternalUploader.InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, identifier)
    End Sub
    Private Sub InitializeLinkRepositoryItems(idUser As Integer, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, alreadyLinkedFiles As List(Of Long), type As lm.Comol.Core.FileRepository.Domain.ItemType) Implements IViewAddRepositoryAction.InitializeLinkRepositoryItems
        CTRLlinkItems.InitializeControl(idUser, identifier, alreadyLinkedFiles, New List(Of lm.Comol.Core.FileRepository.Domain.ItemType) From {type}, lm.Comol.Core.FileRepository.Domain.ItemAvailability.ignore, rPermissions.Administration, rPermissions.Administration)
    End Sub
#End Region

    Private Function UploadFiles(moduleCode As String, idObjectType As Integer, idAction As Integer, addToRepository As Boolean) As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) Implements IViewAddRepositoryAction.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(New SubActivity(), 0, idObjectType, moduleCode, idAction)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(New SubActivity(), 0, idObjectType, moduleCode, idAction)
        End If
    End Function

    Private Sub DisplayItemsAdded() Implements IViewAddRepositoryAction.DisplayItemsAdded
        RaiseEvent LinksAdded()
    End Sub
    Private Sub DisplayItemsNotAdded() Implements IViewAddRepositoryAction.DisplayItemsNotAdded
        RaiseEvent EmptyUpload()
    End Sub
    Private Sub DisplayNoFilesToAdd() Implements IViewAddRepositoryAction.DisplayNoFilesToAdd
        RaiseEvent EmptyUpload()
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As DisplayRepositoryAction = DirectCast(e.Item.DataItem, DisplayRepositoryAction)

            Dim oButton As Button = e.Item.FindControl("BTNaddAction")
            oButton.Text = Resource.getValue("DisplayRepositoryAction.ButtonText." & action.ToString())
            oButton.ToolTip = Resource.getValue("DisplayRepositoryAction.ToolTip." & action.ToString())
            oButton.CommandName = action.ToString
            Dim oLiteral As Literal = e.Item.FindControl("LTaddAction")
            oLiteral.Text = Resource.getValue("DisplayRepositoryAction.Description." & action.ToString())
        End If
    End Sub
    Private Sub RPTactions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTactions.ItemCommand
        Dim action As DisplayRepositoryAction
        action = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplayRepositoryAction).GetByString(e.CommandName, DisplayRepositoryAction.select)
        If action <> DisplayRepositoryAction.none Then
            CurrentPresenter.ChangeAction(IdActivity, Identifier, action, UnloadItems)
        End If
    End Sub
    Private Sub BTNcloseAddActionWindowBottom_Click(sender As Object, e As EventArgs) Handles BTNcloseAddActionWindowBottom.Click, BTNcloseAddActionWindowTop.Click
        RaiseEvent CloseClientWindow()
    End Sub
    Private Sub BTNselectActionTop_Click(sender As Object, e As EventArgs) Handles BTNselectActionTop.Click, BTNselectActionBottom.Click
        CurrentPresenter.ChangeAction(IdActivity, Identifier, DisplayRepositoryAction.select, UnloadItems)
    End Sub
    Private Sub BTNLinkToModuleTop_Click(sender As Object, e As EventArgs) Handles BTNLinkToModuleTop.Click, BTNLinkToModuleBottom.Click
        CurrentPresenter.AddCommunityFilesToItem(IdActivity, Identifier, CTRLlinkItems.GetNewRepositoryItemLinks())
    End Sub
    Private Sub BTNaddInternalToItemBottom_Click(sender As Object, e As EventArgs) Handles BTNaddInternalToItemBottom.Click, BTNaddInternalToItemTop.Click
        CurrentPresenter.AddFilesToItem(IdActivity, Identifier)
    End Sub
    Private Sub BTNaddCommunityFileBottom_Click(sender As Object, e As EventArgs) Handles BTNaddCommunityFileBottom.Click, BTNaddCommunityFileTop.Click
        CurrentPresenter.AddCommunityFilesToItem(IdActivity, Identifier)
    End Sub
#End Region

    Public Sub DisplayWorkingSessionExpired(idCommunity As Integer, idModule As Integer) Implements IViewAddRepositoryAction.DisplayWorkingSessionExpired

    End Sub


    Public Sub DisplayActivityNotFound() Implements IViewAddRepositoryAction.DisplayActivityNotFound

    End Sub


    
   
End Class