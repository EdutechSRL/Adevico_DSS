Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Public Class UC_ItemTranslation
    Inherits BaseControl
    Implements IViewItemTranslations

#Region "Implements"
    Public ReadOnly Property GetTranslations As List(Of dtoTranslation) Implements IViewItemTranslations.GetTranslations
        Get
            Dim idItem As Long = Me.IdMenuItem

            Return (From r As RepeaterItem In RPTtranslations.Items _
                    Where (r.ItemType = ListItemType.AlternatingItem) OrElse (r.ItemType = ListItemType.Item) Select CreateTranslation(r, idItem)).ToList
        End Get
    End Property
    Public Property IdMenuItem As Long Implements IViewItemTranslations.IdMenuItem
        Get
            Return ViewStateOrDefault("IdMenuItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdMenuItem") = value
        End Set
    End Property
    Private Property AllowEdit As Boolean
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowEdit") = value
        End Set
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MenubarEdit", "Modules", "Menu")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub InitalizeControl(translations As List(Of dtoTranslation), IdItem As Long, allowEdit As Boolean) Implements IViewItemTranslations.InitalizeControl
        Me.AllowEdit = allowEdit
        Me.IdMenuItem = IdItem
        Me.RPTtranslations.DataSource = translations
        Me.RPTtranslations.DataBind()
    End Sub
    Private Function CreateTranslation(row As RepeaterItem, ByVal IdMenuItem As Long) As dtoTranslation
        Dim result As New dtoTranslation

        result.Id = DirectCast(row.FindControl("LTid"), Literal).Text
        result.IdLanguage = DirectCast(row.FindControl("LTidLanguage"), Literal).Text
        result.IdMenuItem = IdMenuItem
        result.Name = DirectCast(row.FindControl("TXBname"), TextBox).Text
        result.ToolTip = DirectCast(row.FindControl("TXBtooltip"), TextBox).Text
        Return result
    End Function
#End Region

    Private Sub RPTtranslations_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtranslations.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim oLabel As Label = e.Item.FindControl("LBitemName")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBitemTooltip")
            Me.Resource.setLabel(oLabel)

            Dim oTextbox As TextBox = e.Item.FindControl("TXBname")
            oTextbox.ReadOnly = Not Me.AllowEdit
            oTextbox = e.Item.FindControl("TXBtooltip")
            oTextbox.ReadOnly = Not Me.AllowEdit
        End If
    End Sub
End Class