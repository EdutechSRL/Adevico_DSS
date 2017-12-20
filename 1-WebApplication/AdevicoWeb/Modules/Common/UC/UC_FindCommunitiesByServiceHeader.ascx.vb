Public Class UC_FindCommunitiesByServiceHeader
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
#Region "Internal"
    Public WriteOnly Property LoadCss As Boolean
        Set(value As Boolean)
            LTcss.Visible = value
            If value AndAlso String.IsNullOrEmpty(LTcss.Text) Then
                LTcss.Text = String.Format(LTcssTemplate.Text, PageUtility.BaseUrl)
            End If
        End Set
    End Property
    Public WriteOnly Property LoadScripts As Boolean
        Set(value As Boolean)
            LTscript.Visible = value
            If value AndAlso String.IsNullOrEmpty(LTscript.Text) Then
                LTscript.Text = String.Format(LTscriptTemplate.Text, PageUtility.BaseUrl)
            End If
        End Set
    End Property
    Public Property SelectionMode As ListSelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", ListSelectionMode.Multiple)
        End Get
        Set(value As ListSelectionMode)
            ViewState("SelectionMode") = value
            Select Case value
                Case ListSelectionMode.Multiple
                    LTscriptSingle.Visible = False
                Case ListSelectionMode.Single
                    LTscriptSingle.Visible = True
            End Select

        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CTRLfiltersHeader.SetTransacionIdContainer(Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID & "_" & PageUtility.CurrentContext.UserContext.WorkSessionID.ToString)
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region
#Region "Internal"
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
        CTRLfiltersHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Public Sub SetInitializeVariables(Optional requiredPermissions As Dictionary(Of Integer, Long) = Nothing, Optional unloadIdCommunities As List(Of Integer) = Nothing, Optional availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, Optional onlyFromOrganizations As List(Of Integer) = Nothing)
        CTRLfiltersHeader.SetInitializeVariables(requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
#End Region

End Class
