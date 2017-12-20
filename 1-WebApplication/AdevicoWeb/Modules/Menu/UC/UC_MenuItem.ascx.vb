Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Public Class UC_MenuItem
    Inherits BaseControl
    Implements IViewMenuItem

    Public Event AddNewtem(ByVal IdItem As Long, ByVal itemType As MenuItemType, ByVal newType As MenuItemType)

#Region "Implements"
    Public ReadOnly Property GetItem As dtoMenuItem Implements IViewMenuItem.GetItem
        Get

            Dim item As New dtoMenuItem
            item.Id = IdItem
            item.IdColumnOwner = IdColumnOwner
            item.IdItemOwner = IdItemOwner
            item.IsEnabled = Me.CBXisEnabled.Checked
            item.CssClass = Me.TXBcssClass.Text
            item.ShowDisabledItems = CBool(Me.RBLshowDisabledItems.SelectedValue)
            item.DisplayOrder = Me.DDLposition.SelectedValue
            item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(Me.DDLtype.SelectedValue, MenuItemType.None)
            item.TextPosition = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TextPosition).GetByString(Me.DDLtextPosition.SelectedValue, TextPosition.None)

            If item.Type = MenuItemType.Separator OrElse item.Type = MenuItemType.Text OrElse item.Type = MenuItemType.TextContainer Then
                item.Link = ""
                If item.Type = MenuItemType.Separator Then
                    Me.TXBlink.Text = ""
                Else
                    item.Name = Me.TXBname.Text
                End If
            Else
                item.Link = Me.TXBlink.Text
                item.Name = Me.TXBname.Text
            End If

            Return item
        End Get
    End Property

    Public Property IdColumnOwner As Long Implements IViewMenuItem.IdColumnOwner
        Get
            Return ViewStateOrDefault("IdColumnOwner", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdColumnOwner") = value
        End Set
    End Property

    Public Property IdItem As Long Implements IViewMenuItem.IdItem
        Get
            Return ViewStateOrDefault("IdItem", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdItem") = value
        End Set
    End Property

    Public Property IdItemOwner As Long Implements IViewMenuItem.IdItemOwner
        Get
            Return ViewStateOrDefault("IdItemOwner", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdItemOwner") = value
        End Set
    End Property

    Public Property IdTopItem As Long Implements IViewMenuItem.IdTopItem
        Get
            Return ViewStateOrDefault("IdTopItem", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdTopItem") = value
        End Set
    End Property

    'Public ReadOnly Property GetColumn As dtoColumn Implements IViewItemColumn.GetColumn
    '    Get
   
    '    End Get
    'End Property

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
            .setLabel(LBitemType_t)


            .setButton(BTNaddIconManage, True, , , True)
            .setButton(BTNaddIconNew, True, , , True)
            .setButton(BTNaddIconStatistic, True, , , True)
            .setButton(BTNaddText, True, , , True)
            .setButton(BTNaddLink, True, , , True)
            .setButton(BTNaddSeparator, True, , , True)

            Me.DDLtextPosition.Items.Clear()
            For Each name As String In [Enum].GetNames(GetType(TextPosition))
                Me.DDLtextPosition.Items.Add(New ListItem(Me.Resource.getValue("TextPosition." & name), name))
            Next
        End With
    End Sub
#End Region


    Public Sub InitalizeControl(item As dtoMenuItem, allowEdit As Boolean, availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType)) Implements IViewMenuItem.InitalizeControl
        Me.CBXisEnabled.Checked = item.IsEnabled
        Me.TXBname.Text = item.Name
        Me.TXBcssClass.Text = item.CssClass
        Me.TXBlink.Text = item.Link
        Me.RBLshowDisabledItems.SelectedValue = item.ShowDisabledItems.ToString
        Me.DDLposition.Items.Clear()
        If Not (item.Type = MenuItemType.IconManage OrElse item.Type = MenuItemType.IconNewItem OrElse item.Type = MenuItemType.IconStatistic) Then
            Me.DDLposition.Items.AddRange((From v In (From n In Enumerable.Range(1, item.ParentsNumber).ToList) Select New ListItem(v, v)).ToArray)
            If Not Me.DDLposition.Items.FindByValue(item.DisplayOrder) Is Nothing Then
                Me.DDLposition.SelectedValue = item.DisplayOrder
            End If
            Me.DVdisplayOrder.Visible = True
        Else
            Me.DVdisplayOrder.Visible = False
            Me.DDLposition.Items.Add(item.DisplayOrder)
        End If


        Me.DDLtextPosition.SelectedValue = item.TextPosition.ToString

        Me.IdItem = item.Id
        Me.IdColumnOwner = item.IdColumnOwner
        Me.IdItemOwner = item.IdItemOwner


        Me.DDLtype.Items.Clear()
        For Each type As MenuItemType In availabeTypes
            Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType."), type.ToString))
        Next
        Me.BTNaddIconManage.Visible = (availabeSubTypes.Contains(MenuItemType.IconManage))
        Me.BTNaddIconNew.Visible = (availabeSubTypes.Contains(MenuItemType.IconNewItem))
        Me.BTNaddIconStatistic.Visible = (availabeSubTypes.Contains(MenuItemType.IconStatistic))

        SetupButtons(item.Type)
        EnableEditing(allowEdit)
    End Sub

    Private Sub SetupButtons(type As MenuItemType)
        Me.DVcssClass.Visible = Not (type = MenuItemType.IconManage OrElse type = MenuItemType.IconNewItem OrElse type = MenuItemType.IconStatistic)
        Me.DVdisabledItems.Visible = Not (type = MenuItemType.Separator)
        '     Me.DVisEnabled.Visible=
        Me.DVlink.Visible = Not (type = MenuItemType.Separator OrElse type = MenuItemType.Text OrElse type = MenuItemType.TextContainer)
        Me.DVname.Visible = Not (type = MenuItemType.Separator OrElse type = MenuItemType.IconManage OrElse type = MenuItemType.IconNewItem OrElse type = MenuItemType.IconStatistic)
        Me.DVposition.Visible = Not (type = MenuItemType.Separator)
        Me.DDLposition.Enabled = Not (type = MenuItemType.IconManage OrElse type = MenuItemType.IconNewItem OrElse type = MenuItemType.IconStatistic)

        Select Case type
            Case MenuItemType.TextContainer
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.TextContainer.ToString), MenuItemType.TextContainer.ToString))
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.LinkContainer.ToString), MenuItemType.LinkContainer.ToString))
                Me.MLVaddButton.SetActiveView(VIWcontainer)

            Case MenuItemType.LinkContainer
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.TextContainer.ToString), MenuItemType.TextContainer.ToString))
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.LinkContainer.ToString), MenuItemType.LinkContainer.ToString))
                Me.MLVaddButton.SetActiveView(VIWcontainer)

            Case MenuItemType.Text
            Case MenuItemType.Link
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.Text.ToString), MenuItemType.Text.ToString))
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & MenuItemType.Link.ToString), MenuItemType.Link.ToString))
                Me.MLVaddButton.SetActiveView(VIWicons)
            Case Else
                'Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType." & type.ToString), type.ToString))
                Me.MLVaddButton.SetActiveView(VIWnoButtons)
        End Select
        If Me.DDLtype.Items.FindByValue(type.ToString) Is Nothing Then
            Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuItemType."), type.ToString))
        End If
        Me.DDLtype.SelectedValue = type.ToString
    End Sub

    Private Sub EnableEditing(ByVal allowEdit As Boolean)
        Me.BTNaddLink.Visible = allowEdit
        Me.BTNaddIconNew.Visible = allowEdit
        Me.BTNaddSeparator.Visible = allowEdit
        Me.BTNaddText.Visible = allowEdit
        Me.BTNaddIconNew.Visible = allowEdit
        Me.BTNaddIconStatistic.Visible = allowEdit
        BTNaddIconManage.Visible = allowEdit
        Me.TXBname.ReadOnly = Not allowEdit
        Me.TXBlink.ReadOnly = Not allowEdit
        Me.TXBcssClass.ReadOnly = Not allowEdit

        Me.CBXisEnabled.Enabled = allowEdit
        Me.RBLshowDisabledItems.Enabled = allowEdit
        Me.DDLtextPosition.Enabled = allowEdit
        Me.DDLposition.Enabled = allowEdit
        DDLtype.Enabled = allowEdit
    End Sub

    Private Sub DDLtype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLtype.SelectedIndexChanged
        Me.SetupButtons(lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(Me.DDLtype.SelectedValue, MenuItemType.None))
    End Sub

    Private Sub BTNaddLink_Click(sender As Object, e As System.EventArgs) Handles BTNaddLink.Click, BTNaddIconManage.Click, BTNaddSeparator.Click, BTNaddText.Click, BTNaddIconNew.Click, BTNaddIconStatistic.Click, BTNaddText.Click
        RaiseEvent AddNewtem(Me.IdItem, lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(Me.DDLtype.SelectedValue, MenuItemType.None), lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(sender.CommandArgument, MenuItemType.None))
    End Sub
End Class