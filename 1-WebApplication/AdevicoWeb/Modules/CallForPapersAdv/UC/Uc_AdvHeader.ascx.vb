Public Class Uc_AdvHeader
    Inherits System.Web.UI.UserControl

#Region "Internal"
    Public Property EnableChoosenScript As Boolean
        Get
            Return LTchoosenScripts.Visible
        End Get
        Set(value As Boolean)
            LTchoosenScripts.Visible = value
        End Set
    End Property
    Public Property EnableCommonScript As Boolean
        Get
            Return LTcommonScripts.Visible
        End Get
        Set(value As Boolean)
            LTcommonScripts.Visible = value
        End Set
    End Property
    Public Property EnableModuleScript As Boolean
        Get
            Return LTmoduleScripts.Visible
        End Get
        Set(value As Boolean)
            LTmoduleScripts.Visible = value
        End Set
    End Property
    Public Property EnableSemiFixedScript As Boolean
        Get
            Return LTsemiFixedScripts.Visible
        End Get
        Set(value As Boolean)
            LTsemiFixedScripts.Visible = value
        End Set
    End Property

    Public WriteOnly Property EnableDropDownButtonsScript As Boolean
        Set(value As Boolean)
            LTdropDownButtonsScript.Visible = value
        End Set
    End Property
    Public WriteOnly Property EnableTreeTableScript As Boolean
        Set(value As Boolean)
            LTtreeTableScripts.Visible = value
        End Set
    End Property
    Public WriteOnly Property EnableScripts As Boolean
        Set(value As Boolean)
            LTmoduleScripts.Visible = value
            LTcommonScripts.Visible = value
            LTchoosenScripts.Visible = value
            LTsemiFixedScripts.Visible = value
        End Set
    End Property
    Private _PageUtility As OLDpageUtility
    Protected ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'If String.IsNullOrEmpty(LTbaseUrl.Text) Then
        Dim baseUrl As String = GetBaseUrl()
        'LTbaseUrl.Text = baseUrl



        LTchoosenScripts.Text = Replace(LTchoosenScripts.Text, "#baseurl#", baseUrl)
            LTcommonScripts.Text = Replace(LTcommonScripts.Text, "#baseurl#", baseUrl)
            LTsemiFixedScripts.Text = Replace(LTsemiFixedScripts.Text, "#baseurl#", baseUrl)
            LTdropDownButtonsScript.Text = Replace(LTdropDownButtonsScript.Text, "#baseurl#", baseUrl)
            LTtreeTableScripts.Text = Replace(LTtreeTableScripts.Text, "#baseurl#", baseUrl)
            LTmoduleScripts.Text = Replace(LTmoduleScripts.Text, "#baseurl#", baseUrl)
        'End If
    End Sub

    Public Function GetBaseUrl() As String
        'If String.IsNullOrEmpty(LTbaseUrl.Text) Then
        '    LTbaseUrl.Text = PageUtility.BaseUrl
        'End If
        Return LTbaseUrl.Text
    End Function

    Public Sub SetBaseUrl(baseurl As String)
        LTbaseUrl.Text = baseurl
    End Sub

End Class