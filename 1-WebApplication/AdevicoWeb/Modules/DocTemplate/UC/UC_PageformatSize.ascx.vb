Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class UC_PageformatSize
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            BindDDL(lm.Comol.Core.DomainModel.DocTemplateVers.PageSize.A4)
            BindValue()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
    End Sub
#End Region

#Region "Internal"
    Private Sub BindDDL(ByVal SelSize As TemplateVers.PageSize)

        Dim Sizes() As Integer = CType([Enum].GetValues(GetType(TemplateVers.PageSize)), Integer())

        Me.DDLpageFormat.Items.Clear()

        Dim _L As String = ""
        Try
            _L = Me.Resource.getValue("PageFormat.Landscape")
        Catch ex As Exception

        End Try

        If String.IsNullOrEmpty(_L) Then
            _L = "Landscape"
        End If

        For Each SizeId As Integer In Sizes
            Dim liSize As New ListItem()
            liSize.Text = CType(SizeId, TemplateVers.PageSize).ToString().Replace("_L", " " & _L).Replace("_", "")
            liSize.Value = SizeId
            Me.DDLpageFormat.Items.Add(liSize)
        Next

        Me.DDLpageFormat.SelectedValue = SelSize

    End Sub
    Public Function GetPageSize() As TemplateVers.PageSize
        Dim SelSizeId As Integer = 0
        Try
            SelSizeId = System.Convert.ToInt16(Me.DDLpageFormat.SelectedValue)
        Catch ex As Exception

        End Try
        Return CType(SelSizeId, TemplateVers.PageSize)
    End Function
    Public Function GetPageSizeValue() As TemplateVers.Helpers.PageSizeValue
        Return TemplateVers.Helpers.Measure.GetSize(Me.DDLpageFormat.SelectedValue, Me.DDLMeasure.SelectedValue)
    End Function
    Private Sub BindValue()
        Dim Cursize As TemplateVers.Helpers.PageSizeValue = GetPageSizeValue()

        Me.LBLwidth.Text = Cursize.Width.ToString() & " " & Me.DDLMeasure.SelectedValue
        Me.LBLheight.Text = Cursize.Height.ToString() & " " & Me.DDLMeasure.SelectedValue

    End Sub
#End Region

#Region "Handler"
    Private Sub DDLMeasure_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLMeasure.SelectedIndexChanged
        BindValue()
    End Sub
    Private Sub DDLpageFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpageFormat.SelectedIndexChanged
        BindValue()
    End Sub
#End Region

End Class