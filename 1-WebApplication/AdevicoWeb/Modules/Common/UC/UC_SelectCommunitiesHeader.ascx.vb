Public Class UC_SelectCommunitiesHeader
    Inherits System.Web.UI.UserControl

#Region "Internal"
    Public WriteOnly Property LoadCss As Boolean
        Set(value As Boolean)
            CTRLcommunitySelectorHeader.LoadCss = value
        End Set
    End Property
    Public WriteOnly Property LoadScripts As Boolean
        Set(value As Boolean)
            CTRLcommunitySelectorHeader.LoadScripts = value
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region "Internal"
    Public Sub SetModalTitle(title As String)
        CTRLcommunitySelectorHeader.ModalTitle = title
    End Sub
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Public Sub SetInitializeVariables(requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
        CTRLcommunitySelectorHeader.SetInitializeVariables(requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
#End Region
End Class