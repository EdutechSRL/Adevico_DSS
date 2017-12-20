Public Class UC_EditUrlItems
    Inherits BaseUserControl

#Region "Internal"
    Private _itemsCount As Integer
    Public ReadOnly Property ItemsCount As Int32
        Get
            Return RPTitems.Items.Count
        End Get
    End Property
    Public Property ShowDisplayName As Boolean
        Get
            Return ViewStateOrDefault("ShowDisplayName", True)
        End Get
        Set(value As Boolean)
            ViewState("ShowDisplayName") = value
        End Set
    End Property
    Public Property EditingCssClass As String
        Get
            Return ViewStateOrDefault("EditingCssClass", "dlgediturl")
        End Get
        Set(value As String)
            ViewState("EditingCssClass") = value
        End Set
    End Property
    Public Event SavingSettings(ByVal items As List(Of lm.Comol.Core.DomainModel.dtoUrl))
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Repository", "Modules", "Repository")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setliteral(LTdescriptionEditingUrls)
            .setButton(BTNsaveUrlSettings, True)
            .setLinkButton(LNBcloseUrlSettingsWindow, False, True)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(item As lm.Comol.Core.DomainModel.dtoUrl)
        Dim items As New List(Of lm.Comol.Core.DomainModel.dtoUrl)
        items.Add(item)
        InitializeControl(items)
    End Sub
    Public Sub InitializeControl(items As List(Of lm.Comol.Core.DomainModel.dtoUrl))
        _itemsCount = items.Count
        If _itemsCount = 1 Then
            LTdescriptionEditingUrls.Text = Resource.getValue("LTdescriptionEditingUrls.1")
        End If
        RPTitems.DataSource = items
        RPTitems.DataBind()
        BTNsaveUrlSettings.Visible = items.Count > 0
    End Sub
    Public Function GetUrls() As List(Of lm.Comol.Core.DomainModel.dtoUrl)
        Dim urls As New List(Of lm.Comol.Core.DomainModel.dtoUrl)
        Dim alsoDisplayName As Boolean = ShowDisplayName

        For Each row As RepeaterItem In RPTitems.Items
            Dim oLiteral As Literal = row.FindControl("LTidItem")
            Dim oTextBox As TextBox = row.FindControl("TXBurl")
            Dim dto As New lm.Comol.Core.DomainModel.dtoUrl
            dto.Id = CLng(oLiteral.Text)
            dto.Address = oTextBox.Text
            If alsoDisplayName Then
                oTextBox = row.FindControl("TXBurlName")
                If Not String.IsNullOrEmpty(oTextBox.Text) Then
                    oTextBox.Text = oTextBox.Text.Trim
                End If
                dto.Name = IIf(String.IsNullOrEmpty(oTextBox.Text), dto.Address, oTextBox.Text)
            End If
            urls.Add(dto)
        Next
        Return urls.Where(Function(u) Not String.IsNullOrEmpty(u.Address)).ToList()
    End Function

    Protected Function GetCssClass(index As Integer)
        Dim cssClass As String = ""
        If index = 0 Then
            cssClass = lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString & " "
        End If
        If index - 1 = _itemsCount Then
            cssClass = lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
        End If
        Return cssClass
    End Function
    Private Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        Dim oLabel As Label = e.Item.FindControl("LBurl_t")
        Resource.setLabel(oLabel)

        oLabel = e.Item.FindControl("LBurlName_t")
        oLabel.Visible = ShowDisplayName
        Resource.setLabel(oLabel)


        Dim oTextBox As TextBox = e.Item.FindControl("TXBurlName")
        oTextBox.Visible = ShowDisplayName
    End Sub
    Private Sub BTNsaveUrlSettings_Click(sender As Object, e As System.EventArgs) Handles BTNsaveUrlSettings.Click
        RaiseEvent SavingSettings(GetUrls)
    End Sub
#End Region

   
    
End Class