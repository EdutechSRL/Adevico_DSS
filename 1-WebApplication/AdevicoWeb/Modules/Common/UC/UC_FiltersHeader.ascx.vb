Public Class UC_FiltersHeader
    Inherits BaseControl

#Region "inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "internal"
    Public Property FilterModuleCode As String
        Get
            Return ViewStateOrDefault("FilterModuleCode", "")
        End Get
        Set(value As String)
            ViewState("FilterModuleCode") = value
        End Set
    End Property
    Public Property FilterModuleScope As String
        Get
            Return ViewStateOrDefault("FilterModuleScope", "")
        End Get
        Set(value As String)
            ViewState("FilterModuleScope") = value
        End Set
    End Property

    Public Property FilterIdLanguage As Integer
        Get
            Return ViewStateOrDefault("FilterIdLanguage", -1)
        End Get
        Set(value As Integer)
            ViewState("FilterIdLanguage") = value
        End Set
    End Property
    Public Property FilterIdTransaction As String
        Get
            Return ViewStateOrDefault("FilterIdTransaction", "")
        End Get
        Set(value As String)
            ViewState("FilterIdTransaction") = value
        End Set
    End Property
    Public Property FilterInLinePermissions As String
        Get
            Return ViewStateOrDefault("FilterInLinePermissions", "")
        End Get
        Set(value As String)
            ViewState("FilterInLinePermissions") = value
        End Set
    End Property
    Public Property FilterInLineUnloadcommunitites As String
        Get
            Return ViewStateOrDefault("FilterInLineUnloadcommunitites", "")
        End Get
        Set(value As String)
            ViewState("FilterInLineUnloadcommunitites") = value
        End Set
    End Property
    Public Property FilterInLineOnlyfromOrganizations As String
        Get
            Return ViewStateOrDefault("FilterInLineOnlyfromOrganizations", "")
        End Get
        Set(value As String)
            ViewState("FilterInLineOnlyfromOrganizations") = value
        End Set
    End Property
    Public Property FilterInLineAvailability As String
        Get
            Return ViewStateOrDefault("FilterInLineAvailability", "")
        End Get
        Set(value As String)
            ViewState("FilterInLineAvailability") = value
        End Set
    End Property



    Public Function GetBaseUrl() As String
        Return PageUtility.ApplicationUrlBase
    End Function
    'Public Function GenerateTransactionId() As String
    '    'Return ViewStateOrDefault("LTtransactionId", Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID)
    '    If String.IsNullOrEmpty(LTtransactionId.Text) Then
    '        LTtransactionId.Text = Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID
    '    End If
    '    Return LTtransactionId.Text
    'End Function
    Private Property TransactionFilters As Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter))
        Get
            Return SessionOrDefault("TransactionFilters", New Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter)))
        End Get
        Set(value As Dictionary(Of String, List(Of lm.Comol.Core.DomainModel.Filters.Filter)))
            Session("TransactionFilters") = value
        End Set
    End Property

    Private Function SessionOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (Session(Key) Is Nothing) Then
            Session(Key) = DefaultValue
            Return DefaultValue
        Else
            Return Session(Key)
        End If
    End Function
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region


#Region "internal"
    Protected Friend Sub SetTransacionIdContainer(value As String)
        FilterIdTransaction = value
    End Sub
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), Optional requiredPermissions As Dictionary(Of Integer, Long) = Nothing, Optional unloadIdCommunities As List(Of Integer) = Nothing, Optional availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, Optional onlyFromOrganizations As List(Of Integer) = Nothing) ' preloadedAvailability As CommunityAvailability,
        Dim helper As New FilterHelpers(PageUtility.LinguaID, PageUtility.LinguaCode)
        helper.AnalyzeFiltersForTranslation(filters, FilterModuleCode, FilterModuleScope)

        TransactionFilters(FilterIdTransaction & "_" & FilterModuleCode & "_" & FilterModuleScope) = filters
        SetInitializeVariables(requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Public Sub SetInitializeVariables(Optional requiredPermissions As Dictionary(Of Integer, Long) = Nothing, Optional unloadIdCommunities As List(Of Integer) = Nothing, Optional availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, Optional onlyFromOrganizations As List(Of Integer) = Nothing) ' preloadedAvailability As CommunityAvailability,
        FilterInLinePermissions = ""
        If Not IsNothing(requiredPermissions) AndAlso requiredPermissions.Keys.Any Then
            FilterInLinePermissions = String.Join(",", requiredPermissions.Select(Function(p) p.Key & "-" & p.Value).ToList())
        End If
        If availability <> lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed Then
            FilterInLineAvailability = availability.ToString()
        End If
        FilterInLineUnloadcommunitites = ""
        If Not IsNothing(unloadIdCommunities) AndAlso unloadIdCommunities.Any Then
            FilterInLineUnloadcommunitites = String.Join(",", unloadIdCommunities)
        End If
        FilterInLineOnlyfromOrganizations = ""
        If Not IsNothing(onlyFromOrganizations) AndAlso onlyFromOrganizations.Any Then
            FilterInLineOnlyfromOrganizations = String.Join(",", onlyFromOrganizations)
        End If
    End Sub
#End Region

End Class