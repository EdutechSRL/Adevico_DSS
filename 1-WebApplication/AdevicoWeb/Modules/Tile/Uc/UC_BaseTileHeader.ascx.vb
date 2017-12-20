Public Class UC_BaseTileHeader
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region "internal"
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
#End Region
  
End Class