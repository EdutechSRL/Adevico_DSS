Public Class UC_AddUrlItems
    Inherits BaseUserControl

#Region "Internal"
    Public Property MaxItems As Int32
        Get
            Return ViewStateOrDefault("MaxItems", 1)
        End Get
        Set(value As Int32)
            ViewState("MaxItems") = value
        End Set
    End Property
    Public Property ShowDisplayName As Boolean
        Get
            Return ViewStateOrDefault("ShowDisplayName", True)
        End Get
        Set(value As Boolean)
            ViewState("ShowDisplayName") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Repository", "Modules", "Repository")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource

        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl()
        InitializeControl(MaxItems)
    End Sub
    Public Sub InitializeControl(itemcount As Integer)
        If itemcount < 1 Then
            itemcount = 1
        End If
        MaxItems = itemcount
        RPTitems.DataSource = (From i As Integer In Enumerable.Range(0, itemcount) Select i).ToList()
        RPTitems.DataBind()
    End Sub
    Public Function GetUrls() As List(Of lm.Comol.Core.DomainModel.dtoUrl)
        Dim urls As New List(Of lm.Comol.Core.DomainModel.dtoUrl)
        Dim alsoDisplayName As Boolean = ShowDisplayName

        For Each row As RepeaterItem In RPTitems.Items
            Dim oTextBox As TextBox = row.FindControl("TXBurl")
            Dim dto As New lm.Comol.Core.DomainModel.dtoUrl
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
#End Region

    Protected Function GetCssClass(index As Integer)
        Dim cssClass As String = ""
        If index = 1 Then
            cssClass = lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString & " "
        End If
        If index = MaxItems Then
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
End Class