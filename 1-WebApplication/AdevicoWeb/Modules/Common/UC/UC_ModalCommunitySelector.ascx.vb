Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_ModalCommunitySelector
    Inherits DBbaseControl
    Implements IViewSearchCommunitiesModal

#Region "Implements"
    Public ReadOnly Property isInitialized As Boolean Implements IViewSearchCommunitiesModal.isInitialized
        Get
            Return CTRLcommunity.isInitialized
        End Get
    End Property
   
    Public Property OrderAscending As Boolean Implements IViewSearchCommunitiesModal.OrderAscending
        Get
            Return CTRLcommunity.OrderAscending
        End Get
        Set(value As Boolean)
            CTRLcommunity.OrderAscending = value
        End Set
    End Property
    Public Property OrderBy As lm.Comol.Core.Dashboard.Domain.OrderItemsBy Implements IViewSearchCommunitiesModal.OrderBy
        Get
            Return CTRLcommunity.OrderBy
        End Get
        Set(value As lm.Comol.Core.Dashboard.Domain.OrderItemsBy)
            CTRLcommunity.OrderBy = value
        End Set
    End Property
    Public ReadOnly Property HasAvailableCommunities As Boolean Implements IViewSearchCommunitiesModal.HasAvailableCommunities
        Get
            Return CTRLcommunity.HasAvailableCommunities
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event SessionTimeout()
    Public Event SelectedCommunity(ByVal idCommunity As Integer, ByVal identifier As String)
    Public Event SelectedCommunities(ByVal idCommunities As List(Of Integer), ByVal identifier As String)
    Public Event LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
    Public Property SelectionMode As ListSelectionMode
        Get
            Return CTRLcommunity.SelectionMode
        End Get
        Set(value As ListSelectionMode)
            CTRLcommunity.SelectionMode = value
        End Set
    End Property
    Public Property Description As String
        Get
            Return LBselectCommunityDescription.Text
        End Get
        Set(value As String)
            LBselectCommunityDescription.Text = value
            DVdescription.Visible = Not String.IsNullOrWhiteSpace(value)
        End Set
    End Property
    Public WriteOnly Property CloseButtonText As String
        Set(value As String)
            HYPcloseSelectCommunityDialog.Text = value
        End Set
    End Property
    Public WriteOnly Property CloseButtonToolTip As String
        Set(value As String)
            HYPcloseSelectCommunityDialog.ToolTip = value
        End Set
    End Property
    Public WriteOnly Property SelectButtonText As String
        Set(value As String)
            BTNselectCommunity.Text = value
        End Set
    End Property
    Public WriteOnly Property SelectButtonToolTip As String
        Set(value As String)
            BTNselectCommunity.ToolTip = value
        End Set
    End Property
    Public ReadOnly Property ModalIdentifier As String
        Get
            Return LTidentifier.Text
        End Get
    End Property
    Public Property SingleSelectEvent As Boolean
        Get
            Return ViewStateOrDefault("SingleSelectEvent", (SelectionMode = ListSelectionMode.Single))
        End Get
        Set(value As Boolean)
            ViewState("SingleSelectEvent") = value
        End Set
    End Property


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        If Not Page.IsPostBack Then
            With Resource
                .setButton(BTNselectCommunity, True)
                .setHyperLink(HYPcloseSelectCommunityDialog, False, True)
            End With
        End If
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeAdministrationControl(idProfile As Integer, unloadIdCommunities As List(Of Integer), preloadedAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Implements lm.Comol.Core.BaseModules.Dashboard.Presentation.IViewSearchCommunitiesModal.InitializeAdministrationControl
        CTRLcommunity.InitializeAdministrationControl(idProfile, unloadIdCommunities, preloadedAvailability, onlyFromOrganizations)
    End Sub
    Public Sub InitializeControlByModules(idProfile As Integer, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), preloadedAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability) Implements lm.Comol.Core.BaseModules.Dashboard.Presentation.IViewSearchCommunitiesModal.InitializeControlByModules
        CTRLcommunity.InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, preloadedAvailability)
    End Sub
    Public Sub InitializeControlByModules(idProfile As Integer, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), preloadedAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Implements lm.Comol.Core.BaseModules.Dashboard.Presentation.IViewSearchCommunitiesModal.InitializeControlByModules
        CTRLcommunity.InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, preloadedAvailability, onlyFromOrganizations)
    End Sub
    Public Sub ReloadAdministrationControl(unloadIdCommunities As List(Of Integer), preloadedAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability) Implements lm.Comol.Core.BaseModules.Dashboard.Presentation.IViewSearchCommunitiesModal.ReloadAdministrationControl
        CTRLcommunity.ReloadAdministrationControl(unloadIdCommunities, preloadedAvailability)
    End Sub
    Public Sub ReloadControlByModules(unloadIdCommunities As List(Of Integer), preloadedAvailability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability) Implements lm.Comol.Core.BaseModules.Dashboard.Presentation.IViewSearchCommunitiesModal.ReloadControlByModules
        CTRLcommunity.ReloadControlByModules(unloadIdCommunities, preloadedAvailability)
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewSearchCommunitiesModal.DisplaySessionTimeout
        RaiseEvent SessionTimeout()
    End Sub
    Public Function GetIdSelectedItems() As List(Of Integer) Implements IViewSearchCommunitiesModal.GetIdSelectedItems
        Return CTRLcommunity.GetIdSelectedItems
    End Function
    Public Function GetSelectedItems() As List(Of lm.Comol.Core.BaseModules.Dashboard.Domain.dtoCommunityPlainItem) Implements IViewSearchCommunitiesModal.GetSelectedItems
        Return CTRLcommunity.GetSelectedItems
    End Function
#End Region

    Private Sub BTNselectCommunity_Click(sender As Object, e As EventArgs) Handles BTNselectCommunity.Click
        Dim items As List(Of Integer) = CTRLcommunity.GetIdSelectedItems()
        If SingleSelectEvent AndAlso SelectionMode = ListSelectionMode.Single Then
            RaiseEvent SelectedCommunity(items.FirstOrDefault(), ModalIdentifier)
        Else
            RaiseEvent SelectedCommunities(items, ModalIdentifier)
        End If
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
        RaiseEvent LoadDefaultFiltersToHeader(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
End Class