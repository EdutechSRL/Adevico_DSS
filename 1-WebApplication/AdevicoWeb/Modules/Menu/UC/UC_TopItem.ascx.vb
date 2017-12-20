Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation

Public Class UC_TopItem
    Inherits BaseControl
    Implements IViewTopItem

    Public Event AddNewtem(ByVal IdTopItem As Long, ByVal type As MenuItemType)
    '    Public Event SaveItem()

#Region "Implements"
    Public ReadOnly Property GetTopItem As dtoTopMenuItem Implements IViewTopItem.GetTopItem
        Get
            Dim oDto As New dtoTopMenuItem With {.Id = IdTopItem}
            oDto.CssClass = Me.TXBcssClass.Text
            oDto.DisplayOrder = Me.DDLposition.SelectedValue
            oDto.IsEnabled = Me.CBXisEnabled.Checked
            oDto.Link = Me.TXBlink.Text
            oDto.ShowDisabledItems = CBool(RBLshowDisabledItems.SelectedValue)
            oDto.TextPosition = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TextPosition).GetByString(Me.DDLtextPosition.SelectedValue, TextPosition.None)
            oDto.Type = MenuItemType.TopItemMenu
            oDto.Name = Me.TXBname.Text
            oDto.Id = IdTopItem
            Return odto
        End Get
    End Property
    Private ReadOnly Property IdTopItem As Long Implements IViewTopItem.IdItem
        Get
            Return ViewStateOrDefault("IdTopItem", 0)
        End Get
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
        With Me.Resource
            .setLabel(LBisEnabled_t)
            .setCheckBox(CBXisEnabled)
            .setLabel(LBcssClass)
            .setLabel(LBdisplayOrder_t)
            .setLabel(LBitemName_t)
            .setLabel(LBtextDisposition_t)
            .setLabel(LBshowDisabledItems)
            .setRadioButtonList(RBLshowDisabledItems, "True")
            .setRadioButtonList(RBLshowDisabledItems, "False")
            .setLabel(LBlink_t)

            .setButton(BTNaddcolumn, True, , , True)
            ' .setButton(BTNsaveBottom, True, , , True)
        End With
    End Sub
#End Region

    Public Sub InitalizeControl(item As dtoTopMenuItem, allowEdit As Boolean) Implements IViewTopItem.InitalizeControl

        Me.CBXisEnabled.Checked = item.IsEnabled
        Me.TXBname.Text = item.Name
        Me.TXBcssClass.Text = item.CssClass
        Me.TXBlink.Text = item.Link
        Me.RBLshowDisabledItems.SelectedValue = item.ShowDisabledItems.ToString
        Me.DDLposition.Items.Clear()
        Me.DDLposition.Items.AddRange((From v In (From n In Enumerable.Range(1, item.ParentsNumber).ToList) Select New ListItem(v, v)).ToArray)
        If Not Me.DDLposition.Items.FindByValue(item.DisplayOrder) Is Nothing Then
            Me.DDLposition.SelectedValue = item.DisplayOrder
        End If
        For Each name As String In [Enum].GetNames(GetType(TextPosition))
            Me.DDLtextPosition.Items.Add(New ListItem(Me.Resource.getValue("TextPosition." & name), name))
        Next
        Me.DDLtextPosition.SelectedValue = item.TextPosition.ToString

        Me.ViewState("IdTopItem") = item.Id

        Me.TXBname.ReadOnly = Not allowEdit
        Me.TXBlink.ReadOnly = Not allowEdit
        Me.TXBcssClass.ReadOnly = Not allowEdit

        Me.CBXisEnabled.Enabled = allowEdit
        Me.RBLshowDisabledItems.Enabled = allowEdit
        Me.DDLtextPosition.Enabled = allowEdit
        Me.DDLposition.Enabled = allowEdit
        Me.BTNaddcolumn.Enabled = allowEdit

    End Sub

    Private Sub BTNaddcolumn_Click(sender As Object, e As System.EventArgs) Handles BTNaddcolumn.Click
        RaiseEvent AddNewtem(Me.IdTopItem, MenuItemType.ItemColumn)
    End Sub

    'Private Sub BTNsaveBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveBottom.Click
    '    RaiseEvent SaveItem()
    'End Sub
End Class