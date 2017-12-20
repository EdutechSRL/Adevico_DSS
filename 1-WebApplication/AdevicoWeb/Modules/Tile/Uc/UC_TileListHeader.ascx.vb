Public Class UC_TileListHeader
    Inherits System.Web.UI.UserControl
    Private _PageUtility As OLDpageUtility
    Protected ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Public Function GetBaseUrl() As String
        Return PageUtility.BaseUrl
    End Function
    Public Property FilterIdLanguage As Integer
        Get
            Return ViewStateOrDefault("FilterIdLanguage", -1)
        End Get
        Set(value As Integer)
            ViewState("FilterIdLanguage") = value
            CTRLfiltersHeader.FilterIdLanguage = value
        End Set
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'If Not Page.IsPostBack Then
        '    CTRLfiltersHeader.FilterIdTransaction = Guid.NewGuid.ToString() & "_" & PageUtility.CurrentContext.UserContext.CurrentUserID
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#Region "Internal"
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter))
        CTRLfiltersHeader.SetDefaultFilters(filters)
    End Sub
    Public Sub SetTransacionIdContainer(value As String)
        CTRLfiltersHeader.SetTransacionIdContainer(value)
    End Sub
#End Region
End Class