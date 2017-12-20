Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Public Class UC_Menubar
    Inherits BaseControl
    Implements IViewMenubarInfo


    Public Event AddNewTopItem(ByVal IdMenubar As Long)
    Public Event SaveItem()

#Region "Implements"
    Public ReadOnly Property GetMenubar As dtoMenubar Implements IViewMenubarInfo.GetMenubar
        Get
            Dim dto As New dtoMenubar
            dto.Id = IdMenubar
            dto.CssClass = Me.TXBcssClass.Text
            dto.Name = Me.TXBname.Text
            dto.MenuBarType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Me.DDLtype.SelectedValue, MenuBarType.None)
            Return dto
        End Get
    End Property
    Private ReadOnly Property IdMenubar As Long Implements IViewMenubarInfo.IdMenubar
        Get
            Return ViewStateOrDefault("IdMenubar", 0)
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
        MyBase.SetCulture("pg_MenubarList", "Modules", "Menu")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBmenubarName)
            .setLabel(LBmenubarType)
            .setLabel(LBcssClass)
            .setButton(BTNaddTopItem, True, , , True)
            .setButton(BTNsaveBottom, True, , , True)
        End With
    End Sub
#End Region

    Public Sub InitalizeControl(menubar As dtoMenubar, allowEdit As Boolean) Implements IViewMenubarInfo.InitalizeControl
        Me.ViewState("IdMenubar") = menubar.Id
        Me.DDLtype.Items.Clear()
        Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuBarType." & menubar.MenuBarType.ToString), menubar.MenuBarType.ToString))
        Me.TXBname.Text = menubar.Name
        Me.TXBcssClass.Text = menubar.CssClass

        Me.TXBname.ReadOnly = Not allowEdit
        Me.TXBcssClass.ReadOnly = Not allowEdit
        Me.DDLtype.Enabled = allowEdit
        Me.BTNaddTopItem.Visible = allowEdit
        Me.BTNsaveBottom.Visible = allowEdit
    End Sub

    Private Sub BTNaddTopItem_Click(sender As Object, e As System.EventArgs) Handles BTNaddTopItem.Click
        RaiseEvent AddNewTopItem(Me.IdMenubar)
    End Sub

    Private Sub BTNsaveBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveBottom.Click
        RaiseEvent SaveItem()
    End Sub
End Class