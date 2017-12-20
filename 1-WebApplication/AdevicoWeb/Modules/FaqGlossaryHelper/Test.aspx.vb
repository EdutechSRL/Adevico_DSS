Imports Gls = lm.Comol.Modules.Standard.Glossary

Public Class Test1
    Inherits PageBase

#Region "Context"
    Private _FaqService As lm.Comol.Modules.Standard.Faq.FAQService
    Private _GloService As Gls.Business.ServiceGlossary

    Private ReadOnly Property FAQService As lm.Comol.Modules.Standard.Faq.FAQService
        Get
            If IsNothing(_FaqService) Then
                _FaqService = New lm.Comol.Modules.Standard.Faq.FAQService(Me.PageUtility.CurrentContext, Me.PageUtility.CurrentContext.DataContext.GetCurrentSession())
            End If
            Return _FaqService
        End Get
    End Property

    Private ReadOnly Property GloService As Gls.Business.ServiceGlossary
        Get
            If IsNothing(_GloService) Then
                _GloService = New Gls.Business.ServiceGlossary(Me.PageUtility.CurrentContext)
                ' CurrentContext.DataContext.GetCurrentSession())
            End If
            Return _GloService
        End Get
    End Property
#End Region

#Region "Inherits"

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
        End Get
    End Property
    Public Overrides Sub BindDati()
        Me.RPTfaq.Visible = True
        Me.RPTfaq.DataSource = FAQService.GetFaqList(-1)
        Me.RPTfaq.DataBind()

        Me.Rpt_Group.Visible = True
        Me.Rpt_Group.DataSource = GloService.GetAvailableDtoGoup()
        Me.Rpt_Group.DataBind()

    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.RPTfaq.Visible = False
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If Me.Request.Url.AbsoluteUri.StartsWith("http://localhost/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("https://localhost/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.235/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.203/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.207/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.242/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://193.205.200.34/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.212/") _
                OrElse Me.Request.Url.AbsoluteUri.StartsWith("http://192.168.222.200/") _
                Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Sub RegistraAccessoPagina()
    End Sub
    Public Overrides Sub SetCultureSettings()
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)
    End Sub
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"

    Public Function GetString(ByVal InputString As String)
        If (InputString.Length() > 6) Then
            Try
                Return InputString.Remove(5) & "..."
            Catch ex As Exception
                Return InputString
            End Try
        Else
            Return InputString
        End If
    End Function

#End Region


    Private Sub Rpt_Group_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_Group.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim group As Gls.DTO_GlsGroup = e.Item.DataItem

            If Not IsNothing(group) Then
                Dim lit_group As Literal = e.Item.FindControl("lit_group")
                If Not IsNothing(lit_group) Then
                    lit_group.Text = group.Name
                End If

                Dim Rpt_Item As Repeater = e.Item.FindControl("Rpt_Item")
                If Not IsNothing(Rpt_Item) Then
                    Rpt_Item.DataSource = Me.GloService.GetAvailableDtoItem(group.ID)
                    Rpt_Item.DataBind()
                End If
            End If
        End If
    End Sub
End Class