Imports lm.Comol.Modules.Base.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
Imports lm.Comol.Modules.TaskList
Imports lm.Comol.Modules.TaskList.Domain

Public Class AddRepositoryFileToTask
    Inherits PageBase
    Implements IViewLinkRepositoryItemsToTask


#Region "PageBase"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#End Region

#Region "Default Inherited"
    Private _BaseUrl As String
    Private _PageUtility As OLDpageUtility
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As LinkRepositoryItemstoTaskPresenter

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
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As LinkRepositoryItemstoTaskPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New LinkRepositoryItemstoTaskPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "IView"
    Public WriteOnly Property AllowCommunityLink() As Boolean Implements IViewLinkRepositoryItemsToTask.AllowCommunityLink
        Set(ByVal value As Boolean)
            Me.LNBlinkToItem.Enabled = value
        End Set
    End Property
    Private ReadOnly Property PreloadedCommunityID() As Integer Implements IViewLinkRepositoryItemsToTask.PreloadedCommunityID
        Get
            If IsNumeric(Request.QueryString("CommunityID")) Then
                Return CLng(Request.QueryString("CommunityID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedItemID() As Long Implements IViewLinkRepositoryItemsToTask.PreloadedItemID
        Get
            If IsNumeric(Request.QueryString("TaskID")) Then
                Return CLng(Request.QueryString("TaskID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property CurrentItemCommunityID() As Integer Implements IViewLinkRepositoryItemsToTask.CurrentItemCommunityID
        Get
            If IsNumeric(Me.ViewState("CurrentItemCommunityID")) Then
                Return CLng(Me.ViewState("CurrentItemCommunityID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentItemCommunityID") = value
        End Set
    End Property
    Private Property CurrentItemID() As Long Implements IViewLinkRepositoryItemsToTask.CurrentItemID
        Get
            If IsNumeric(Me.ViewState("CurrentItemID")) Then
                Return CLng(Me.ViewState("CurrentItemID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Long)
            Me.ViewState("CurrentItemID") = value
        End Set
    End Property
    Private Property ModuleID() As Integer Implements IViewLinkRepositoryItemsToTask.ModuleID
        Get
            If IsNumeric(Me.ViewState("ModuleID")) Then
                Return CLng(Me.ViewState("ModuleID"))
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ModuleID") = value
        End Set
    End Property
#End Region

#Region "Default Method"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Me.Page.IsPostBack = False Then
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
        MyBase.SetCulture("pg_CommunityDiaryManagementFile", "Modules", "CommunityDiary")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleLinkCommunityFile")
            Me.Master.ServiceNopermission = .getValue("nopermissionLinkCommunityFile")
            .setHyperLink(Me.HYPbackToFileManagement, True, True)
            .setHyperLink(Me.HYPbackToItems, True, True)
            .setHyperLink(HYPbackToItem, True, True)
            .setLinkButton(LNBlinkToItem, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Management file"
    Public Sub InitializeFileSelector(ByVal IdCommunity As Integer, ByVal itemLinks As List(Of iCoreItemFileLink(Of Long)), ByVal ShowAlsoHiddenItems As Boolean, ByVal AdminPurpose As Boolean) Implements IViewLinkRepositoryItemsToTask.InitializeFileSelector
        Me.CTRLlinkRepositoryItems.InitializeControl(IdCommunity, itemLinks, True, False, ShowAlsoHiddenItems, AdminPurpose)
    End Sub

    Private Sub LNBlinkToItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBlinkToItem.Click
        Me.CurrentPresenter.UpdateFileLink(Me.CTRLlinkRepositoryItems.GetNewRepositoryItemLinks, Me.CTRLlinkRepositoryItems.GetRepositoryItemLinksId)
    End Sub
#End Region

#Region "Rediect"
    Public Sub ReturnToTaskDetailRead(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.ReturnToProject
        PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailRead(IdItem))
    End Sub
    Public Sub ReturnToItem(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.ReturnToTask
        PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailEditable(IdItem))
    End Sub
    Public Sub ReturnToFileManagement(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.ReturnToFileManagement
        PageUtility.RedirectToUrl(lm.Comol.Modules.TaskList.Domain.RootObject.ItemManagementFiles(IdItem))
    End Sub
    Public Sub SetBackToCommunityDiary(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.SetBackToProject
        Me.HYPbackToItems.Visible = True ' Not (IdCommunity = 0)
        Me.HYPbackToItems.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailRead(IdItem)
    End Sub
    Public Sub SetBackToItemUrl(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.SetBackToTaskUrl
        Me.HYPbackToItem.Visible = True ' Not (IdCommunity = 0)
        Me.HYPbackToItem.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.TaskDetailEditable(IdItem)
    End Sub
    Public Sub SetBackToManagementUrl(ByVal IdCommunity As Integer, ByVal IdItem As Long) Implements IViewLinkRepositoryItemsToTask.SetBackToManagementUrl
        Me.HYPbackToFileManagement.Visible = True ' Not (IdCommunity = 0)
        Me.HYPbackToFileManagement.NavigateUrl = Me.BaseUrl & lm.Comol.Modules.TaskList.Domain.RootObject.ItemManagementFiles(IdItem)
    End Sub
#End Region






    Public Sub NoPermissionToManagementFiles(ByVal IdCommunity As Integer, ByVal ModuleID As Integer) Implements IViewLinkRepositoryItemsToTask.NoPermissionToManagementFiles
        Me.Master.ShowNoPermission = True
    End Sub

End Class