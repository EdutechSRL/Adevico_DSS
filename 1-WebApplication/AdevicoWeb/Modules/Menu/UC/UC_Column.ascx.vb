Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Public Class UC_ColumnItem
    Inherits BaseControl
    Implements IViewItemColumn

    Public Event AddNewtem(ByVal IdColumn As Long, ByVal type As MenuItemType)
    Public Event VirtualDeleteItem(ByVal IdColumn As Long, ByVal type As MenuItemType)
    Public Event DeleteItem(ByVal IdColumn As Long, ByVal type As MenuItemType)
    Public Event SaveItem()

#Region "Implements"
    Public ReadOnly Property GetColumn As dtoColumn Implements IViewItemColumn.GetColumn
        Get
            Dim item As New dtoColumn
            item.Id = IdColumn
            item.IdTopItem = IdTopItem
            item.IsEnabled = Me.CBXisEnabled.Checked
            item.WidthPx = Me.TXBwidth.Text
            item.CssClass = Me.TXBcssClass.Text
            item.HeightPx = Me.TXBheight.Text
            item.HasSeparator = Me.CBXseparator.Checked
            item.DisplayOrder = Me.DDLposition.SelectedValue
            Return item
        End Get
    End Property


    Private Property IdTopItem As Long
        Get
            Return ViewStateOrDefault("IdTopItem", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdTopItem") = value
        End Set
    End Property
    Public ReadOnly Property IdColumn As Long Implements IViewItemColumn.IdColumn
        Get
            Return ViewStateOrDefault("IdColumn", 0)
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
            .setLabel(LBwidth_t)
            .setLabel(LBheight_t)
            .setLabel(LBcssClass)
            .setLabel(LBdisplayOrder_t)
            .setLabel(LBhasSeparator_t)
            .setCheckBox(CBXseparator)
            .setButton(BTNaddSeparator, True)
            .setButton(BTNaddTextContainer, True)
            .setButton(BTNaddLinkContainer, True)
            .setButton(BTNaddText, True)
            .setButton(BTNaddLink, True)
            .setButton(BTNsaveBottom, True)
            .setButton(BTNdelete, True)
            .setButton(BTNvirtualDelete, True)


        End With
    End Sub
#End Region

    Public Sub InitalizeControl(column As dtoColumn, allowEdit As Boolean) Implements IViewItemColumn.InitalizeControl
        Me.CBXisEnabled.Checked = column.IsEnabled
        Me.TXBwidth.Text = column.WidthPx
        Me.TXBcssClass.Text = column.CssClass
        Me.TXBheight.Text = column.HeightPx
        Me.CBXseparator.Checked = column.HasSeparator

        Me.DDLposition.Items.Clear()
        Me.DDLposition.Items.AddRange((From v In (From n In Enumerable.Range(1, column.ParentsNumber).ToList) Select New ListItem(v, v)).ToArray)
        If Not Me.DDLposition.Items.FindByValue(column.DisplayOrder) Is Nothing Then
            Me.DDLposition.SelectedValue = column.DisplayOrder
        End If
        Me.TXBcssClass.ReadOnly = Not allowEdit
        Me.TXBheight.ReadOnly = Not allowEdit
        Me.TXBwidth.ReadOnly = Not allowEdit
        Me.CBXisEnabled.Enabled = allowEdit
        Me.CBXseparator.Enabled = allowEdit
        Me.DDLposition.Enabled = allowEdit
        Me.ViewState("IdColumn") = column.Id
        IdTopItem = column.IdTopItem
        Me.BTNvirtualDelete.Visible = False 'allowEdit
        Me.BTNaddLink.Visible = allowEdit
        Me.BTNaddLinkContainer.Visible = allowEdit
        Me.BTNaddSeparator.Visible = allowEdit
        Me.BTNaddText.Visible = allowEdit
        Me.BTNaddTextContainer.Visible = allowEdit
        Me.BTNsaveBottom.Visible = allowEdit
        Me.BTNvirtualDelete.Visible = allowEdit
        Me.BTNvirtualDelete.CommandArgument = column.Id
        Me.BTNvirtualDelete.CommandName = column.Type.ToString
        Me.BTNdelete.Visible = allowEdit
        Me.BTNdelete.CommandArgument = column.Id
        Me.BTNdelete.CommandName = column.Type.ToString
    End Sub

    Private Sub BTNaddLink_Click(sender As Object, e As System.EventArgs) Handles BTNaddLink.Click, BTNaddLinkContainer.Click, BTNaddSeparator.Click, BTNaddText.Click, BTNaddTextContainer.Click
        RaiseEvent AddNewtem(Me.IdColumn, lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(sender.CommandArgument, MenuItemType.None))
    End Sub

    Private Sub BTNsaveBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveBottom.Click
        RaiseEvent SaveItem()
    End Sub

    Private Sub BTNdelete_Click(sender As Object, e As System.EventArgs) Handles BTNdelete.Click
        RaiseEvent DeleteItem(Me.IdColumn, MenuItemType.ItemColumn)
    End Sub

    Private Sub BTNvirtualDelete_Click(sender As Object, e As System.EventArgs) Handles BTNvirtualDelete.Click
        RaiseEvent DeleteItem(Me.IdColumn, MenuItemType.ItemColumn)
    End Sub
End Class