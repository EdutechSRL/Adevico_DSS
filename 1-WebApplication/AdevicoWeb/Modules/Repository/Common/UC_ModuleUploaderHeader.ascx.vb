Public Class UC_ModuleInternalUploaderHeader
    Inherits FRbaseControl

#Region "Internal"
    Public WriteOnly Property DisplayCommonScripts As Boolean
        Set(value As Boolean)
            LTcommonRepositoryScript.Visible = value
        End Set
    End Property
    Public WriteOnly Property DisplayTagCssScripts As Boolean
        Set(value As Boolean)
            LTtagCssScript.Visible = value
        End Set
    End Property
    Public WriteOnly Property DisplayCommonCss As Boolean
        Set(value As Boolean)
            LTcommonRepositoryCss.Visible = value
        End Set
    End Property
    Public WriteOnly Property DisplayFancybox As Boolean
        Set(value As Boolean)
            LTcommonFancybox.Visible = value
        End Set
    End Property
#End Region
    'Public Property ProgressAreaClientId As String
    '    Get
    '        Return LTprogressAreaClientId.Text
    '    End Get
    '    Set(value As String)
    '        If Not String.IsNullOrWhiteSpace(value) Then
    '            LTprogressAreaClientId.Text = value
    '        End If
    '    End Set
    'End Property
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

#Region "Internal"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Sub InitializeHeader(Optional tags As List(Of String) = Nothing)
        Dim url As String = PageUtility.ApplicationUrlBase()
        If Not url.EndsWith("/") Then
            url &= "/"
        End If
        LTcommonRepositoryCss.Text = Replace(LTcommonRepositoryCss.Text, "#baseurl#", url)
        LTcommonFancybox.Text = Replace(LTcommonFancybox.Text, "#baseurl#", url)

        LTtagCssScript.Text = Replace(LTtagCssScript.Text, "#baseurl#", url)
        LTcommonRepositoryScript.Text = Replace(LTcommonRepositoryScript.Text, "#baseurl#", url)

        LTscriptRender.Visible = True
        LTscriptRender.Text = LTscript.Text
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Extension#", Resource.getValue("itemError_Extension"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_Size#", Resource.getValue("itemError_Size"))
        LTscriptRender.Text = Replace(LTscriptRender.Text, "#itemError_NotSupported#", Resource.getValue("itemError_NotSupported"))
        If (Not IsNothing(tags) AndAlso tags.Any) Then
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", """" & Join(tags.ToArray(), """,""") & """")
        Else
            LTscriptRender.Text = Replace(LTscriptRender.Text, "#tags#", "")
        End If
    End Sub
#End Region
End Class