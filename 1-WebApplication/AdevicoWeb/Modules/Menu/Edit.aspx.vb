Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Imports lm.Comol.UI.Presentation
Public Class EditMenubar
    Inherits PageBase
    Implements IViewEditMenubar


#Region "Generics"
    Private _Presenter As EditMenubarPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _BaseUrl As String
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As EditMenubarPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditMenubarPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdMenubar As Long Implements IViewEditMenubar.IdMenubar
        Get
            Return ViewStateOrDefault("IdMenubar", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdMenubar") = value
        End Set
    End Property
    Public ReadOnly Property PreloadIdMenubar As Long Implements IViewEditMenubar.PreloadIdMenubar
        Get
            If IsNumeric(Request.QueryString("Id")) Then
                Return CLng(Request.QueryString("Id"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreviousView As lm.Comol.Modules.Standard.Menu.Domain.MenuBarType Implements IViewEditMenubar.PreviousView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Request.QueryString("View"), MenuBarType.GenericCommunity)
        End Get
    End Property
    Public Property AllowEdit As Boolean Implements lm.Comol.Modules.Standard.Menu.Presentation.IViewEditMenubar.AllowEdit
        Get
            Return ViewStateOrDefault("AllowEdit", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowEdit") = value
            Me.BTNsaveBottom.Enabled = value
            Me.DLBvirtualDelete.Enabled = value
            DLBdelete.enabled = value
        End Set
    End Property

    Public Property AllowManage As Boolean Implements lm.Comol.Modules.Standard.Menu.Presentation.IViewEditMenubar.AllowManage
        Get
            Return ViewStateOrDefault("AllowManage", False)
        End Get
        Set(value As Boolean)
            HYPbackToManagement.Visible = value
            HYPbackToManagement.NavigateUrl = BaseUrl & rootObject.MenuBarList(PreviousView)
            Me.ViewState("AllowManage") = value
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
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If TypeOf Me.Master Is IviewMaster Then
            DirectCast(Me.Master, IviewMaster).ShowNoPermission = False
        End If
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
            Me.CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        If TypeOf Me.Master Is IviewMaster Then
            DirectCast(Me.Master, IviewMaster).ShowNoPermission = True
        End If
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_MenubarEdit", "Modules", "Menu")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            If TypeOf Me.Master Is IviewMaster Then
                Dim imaster As IviewMaster = DirectCast(Me.Master, IviewMaster)
                imaster.ServiceTitle = .getValue("serviceTitleEdit")
                imaster.ServiceNopermission = .getValue("nopermission")
            End If
            .setHyperLink(HYPbackToManagement, True, True)
            .setButton(BTNsaveBottom, True, , , True)
            .setLinkButton(DLBvirtualDelete, True, True)
            .setLinkButton(DLBdelete, True, True)
            Dim options As New List(Of String)
            Me.DLGvirtualDeleteItem.DialogTitle = .getValue("DLGvirtualDeleteItemTitle")
            Me.DLGvirtualDeleteItem.DialogText = .getValue("DLGvirtualDeleteItemText")
            Me.DLGvirtualDeleteItem.InitializeControl(options, -1)


            Me.DLGphisicalDeleteItem.DialogTitle = .getValue("DLGphisicalDeleteItemTitle")
            Me.DLGphisicalDeleteItem.DialogText = .getValue("DLGphisicalDeleteItemText")
            Me.DLGphisicalDeleteItem.InitializeControl(options, -1)

            .setLiteral(UPBloadItems.FindControl("LTloadingMessage"))
            '  Me.DLBvirtualDelete.InitializeSingleSelectControlByClass(Me.DLGvirtualDeleteItem.DialogClass, 0)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "TreeView"
    Public Sub LoadMenubar(tree As dtoTree, selectedItem As dtoItem) Implements IViewEditMenubar.LoadMenubar
        Me.CTRLmenubarTree.InitalizeControl(tree, selectedItem)
        Me.UDPdata.Update()
    End Sub

    Private Sub CTRLmenubarTree_ItemMovedTo(startItem As dtoItem, endItem As dtoItem) Handles CTRLmenubarTree.ItemMovedTo
        Me.CurrentPresenter.MoveItemTo(startItem, endItem)
    End Sub

    Private Sub CTRLmenubarTree_ItemReorderedTo(startItem As dtoItem, endItem As dtoItem) Handles CTRLmenubarTree.ItemReorderedTo
        Me.CurrentPresenter.ItemReorderedTo(startItem, endItem)
    End Sub

    Private Sub CTRLmenubarTree_ItemToFirstDisplay(startItem As dtoItem) Handles CTRLmenubarTree.ItemToFirstDisplay
        Me.CurrentPresenter.ItemToFirstDisplay(startItem)
    End Sub
    Private Sub CTRLmenubarTree_SelectedItemChanged(item As dtoItem) Handles CTRLmenubarTree.SelectedItemChanged
        Me.CurrentPresenter.SelectItem(item)
    End Sub

    Public Sub ChangeTreeItemName(item As dtoItem, name As String, position As Long) Implements IViewEditMenubar.ChangeTreeItemName
        Me.CTRLmenubarTree.ChangeTreeItemName(item, name)
        Me.UDPdata.Update()
    End Sub
#End Region

#Region "MenuBarInfo"
    Public Sub LoadMenuBarInfo(menubar As dtoMenubar) Implements IViewEditMenubar.LoadMenuBarInfo
        MLVitemInfo.SetActiveView(VIWmenubarInfo)
        MLVitemData.SetActiveView(VIWstandard)
        Me.CTRLmenubarTree.UpdateDragAndDrop(menubar)
        Me.CTRLmenubarInfo.InitalizeControl(menubar, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub
    Private Sub CTRLmenubarInfo_AddNewTopItem(IdMenubar As Long) Handles CTRLmenubarInfo.AddNewTopItem
        Me.CurrentPresenter.AddItem(IdMenubar, MenuItemType.Menubar, MenuItemType.TopItemMenu)
    End Sub

    Private Sub CTRLmenubarInfo_SaveItem() Handles CTRLmenubarInfo.SaveItem
        Me.CurrentPresenter.SaveItem(Me.CTRLmenubarInfo.GetMenubar)
    End Sub
#End Region

#Region "Top Items"
    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation), selectedTypes As List(Of Integer)) Implements IViewEditMenubar.LoadTopMenuItem
        LoadTopMenuItem(item, translations, False)
        Me.CTRLprofileTypes.InitalizeControl(selectedTypes, Nothing, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub

    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation)) Implements IViewEditMenubar.LoadTopMenuItem
        LoadTopMenuItem(item, translations, True)
        Me.CTRLmodules.InitalizeControl(item.IdModule, item.Permission, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation), showModule As Boolean)
        MLVitemInfo.SetActiveView(VIWtopItemInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)

        Me.CTRLtranslations.InitalizeControl(translations, item.Id, Me.AllowEdit)
        Me.CTRLtranslations.Visible = True
        Me.CTRLmodules.Visible = showModule
        Me.CTRLprofileTypes.Visible = Not showModule
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLtopItem.InitalizeControl(item, AllowEdit)
        Me.DLBvirtualDelete.CommandArgument = item.Id
        Me.DLBvirtualDelete.CommandName = item.Type.ToString
        Me.DLBvirtualDelete.InitializeSingleSelectControlByClass("dialog_virtualDelete", 0)
        Me.DLBdelete.Enabled = AllowEdit
        Me.DLBdelete.CommandArgument = item.Id
        Me.DLBdelete.CommandName = item.Type.ToString
        Me.DLBdelete.InitializeSingleSelectControlByClass("dialog_delete", 0)
        Me.UDPdata.Update()
    End Sub
    Private Sub CTRLtopItem_AddNewtem(IdTopItem As Long, type As MenuItemType) Handles CTRLtopItem.AddNewtem
        Me.CurrentPresenter.AddItem(IdTopItem, MenuItemType.TopItemMenu, type)
    End Sub
#End Region

#Region "Columns"
    Public Sub LoadColumnItem(column As dtoColumn) Implements IViewEditMenubar.LoadColumnItem
        MLVitemInfo.SetActiveView(VIWcolumnInfo)
        MLVitemData.SetActiveView(VIWstandard)
        Me.CTRLmenubarTree.UpdateDragAndDrop(column)
        Me.CTRLcolumn.InitalizeControl(column, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub
    Private Sub CTRLcolumn_AddNewtem(IdColumn As Long, type As MenuItemType) Handles CTRLcolumn.AddNewtem
        Me.CurrentPresenter.AddItem(IdColumn, MenuItemType.ItemColumn, type)
    End Sub

    Private Sub CTRLcolumn_DeleteItem(IdColumn As Long, type As lm.Comol.Modules.Standard.Menu.Domain.MenuItemType) Handles CTRLcolumn.DeleteItem
        Me.CurrentPresenter.DeleteItem(IdColumn, type)
    End Sub
    Private Sub CTRLcolumn_VirtualDeleteItem(IdColumn As Long, type As lm.Comol.Modules.Standard.Menu.Domain.MenuItemType) Handles CTRLcolumn.VirtualDeleteItem
        Me.CurrentPresenter.VirtualDeleteItem(IdColumn, type)
    End Sub
    Private Sub CTRLcolumn_SaveItem() Handles CTRLcolumn.SaveItem
        Me.CurrentPresenter.SaveItem(Me.CTRLcolumn.GetColumn)
    End Sub
#End Region

#Region "Items"
    Private Sub LoadSeparatorItem(item As dtoMenuItem) Implements IViewEditMenubar.LoadSeparatorItem
        MLVitemInfo.SetActiveView(VIWmenuInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)
        Me.CTRLmodules.Visible = False
        Me.CTRLprofileTypes.Visible = False
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLmenuItem.InitalizeControl(item, AllowEdit, New List(Of MenuItemType), New List(Of MenuItemType))
        Me.DLBvirtualDelete.CommandArgument = item.Id
        Me.DLBvirtualDelete.CommandName = item.Type.ToString
        Me.DLBvirtualDelete.InitializeSingleSelectControlByClass("dialog_virtualDelete", 0)
        Me.DLBdelete.Enabled = AllowEdit
        Me.DLBdelete.CommandArgument = item.Id
        Me.DLBdelete.CommandName = item.Type.ToString
        Me.DLBdelete.InitializeSingleSelectControlByClass("dialog_delete", 0)
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType)) Implements IViewEditMenubar.LoadMenuItem
        LoadMenuItem(item, translations, True, availabeTypes, availabeSubTypes)
        Me.CTRLmodules.InitalizeControl(item.IdModule, item.Permission, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub

    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), selectedTypes As List(Of Integer), availableProfileTypes As List(Of Integer), availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType)) Implements IViewEditMenubar.LoadMenuItem
        LoadMenuItem(item, translations, False, availabeTypes, availabeSubTypes)
        Me.CTRLprofileTypes.InitalizeControl(selectedTypes, availableProfileTypes, Me.AllowEdit)
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), showModule As Boolean, availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType))
        MLVitemInfo.SetActiveView(VIWmenuInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)

        Me.CTRLtranslations.InitalizeControl(translations, item.Id, Me.AllowEdit)
        Me.CTRLtranslations.Visible = True
        Me.CTRLmodules.Visible = showModule
        Me.CTRLprofileTypes.Visible = Not showModule
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLmenuItem.InitalizeControl(item, AllowEdit, availabeTypes, availabeSubTypes)
        Me.DLBvirtualDelete.CommandArgument = item.Id
        Me.DLBvirtualDelete.CommandName = item.Type.ToString
        Me.DLBvirtualDelete.InitializeSingleSelectControlByClass("dialog_virtualDelete", 0)

        Me.DLBdelete.Enabled = AllowEdit
        Me.DLBdelete.CommandArgument = item.Id
        Me.DLBdelete.CommandName = item.Type.ToString
        Me.DLBdelete.InitializeSingleSelectControlByClass("dialog_delete", 0)
    End Sub

    Private Sub CTRLmenuItem_AddNewtem(ByVal IdItem As Long, ByVal itemType As MenuItemType, ByVal newType As MenuItemType) Handles CTRLmenuItem.AddNewtem
        Me.CurrentPresenter.AddItem(IdItem, itemType, newType)
    End Sub
#End Region

    Private Sub BTNsaveBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveBottom.Click
        If MLVitemInfo.GetActiveView Is VIWtopItemInfo Then
            Dim dto As dtoTopMenuItem = Me.CTRLtopItem.GetTopItem()
            dto.IdMenubar = IdMenubar
            If Me.CTRLmodules.Visible Then
                dto.Permission = Me.CTRLmodules.GetPermission
                dto.IdModule = Me.CTRLmodules.GetModuleId
                Me.CurrentPresenter.SaveItem(dto, Me.CTRLtranslations.GetTranslations)
            Else
                dto.Permission = 0
                dto.IdModule = 0
                Me.CurrentPresenter.SaveItem(dto, Me.CTRLtranslations.GetTranslations, Me.CTRLprofileTypes.GetSelectedTypes)
            End If
        ElseIf MLVitemInfo.GetActiveView Is VIWmenuInfo Then
            Dim dto As dtoMenuItem = Me.CTRLmenuItem.GetItem()
            dto.IdMenubar = IdMenubar
            If Me.CTRLmodules.Visible Then
                dto.Permission = Me.CTRLmodules.GetPermission
                dto.IdModule = Me.CTRLmodules.GetModuleId
                Me.CurrentPresenter.SaveItem(dto, Me.CTRLtranslations.GetTranslations)
            Else
                dto.Permission = 0
                dto.IdModule = 0
                Me.CurrentPresenter.SaveItem(dto, Me.CTRLtranslations.GetTranslations, Me.CTRLprofileTypes.GetSelectedTypes)
            End If
        End If
    End Sub
    Private Sub DLGvirtualDeleteItem_ButtonPressed(dialogResult As Integer, CommandArgument As String, CommandName As String) Handles DLGvirtualDeleteItem.ButtonPressed
        Try
            Dim IdItem As Long = CLng(CommandArgument)
            Dim type As MenuItemType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(CommandName, MenuItemType.None)

            Me.CurrentPresenter.VirtualDeleteItem(IdItem, type)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DLGphisicalDeleteItem_ButtonPressed(dialogResult As Integer, CommandArgument As String, CommandName As String) Handles DLGphisicalDeleteItem.ButtonPressed
        Try
            Dim IdItem As Long = CLng(CommandArgument)
            Dim type As MenuItemType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuItemType).GetByString(CommandName, MenuItemType.None)

            Me.CurrentPresenter.DeleteItem(IdItem, type)
        Catch ex As Exception

        End Try
    End Sub

#Region "Implements"
    Public Sub MenubarUnknown() Implements IViewEditMenubar.MenubarUnknown
        MLVitemInfo.SetActiveView(VIWunknown)
        MLVitemData.SetActiveView(VIWstandard)
        Me.LTitemUnknown.Text = Resource.getValue("MenubarUnknown")
        Me.UDPdata.Update()
    End Sub
    Public Sub CreateError(type As MenuItemType) Implements IViewEditMenubar.CreateError
        Me.MLVitemInfo.SetActiveView(VIWunknown)
        Me.MLVitemData.SetActiveView(VIWstandard)

        Me.LTitemUnknown.Text = String.Format(Resource.getValue("CreateError"), Resource.getValue("MenuItemType." & type.ToString))
        Me.UDPdata.Update()
    End Sub
    Public Sub SaveError() Implements IViewEditMenubar.SaveError
        Me.MLVitemInfo.SetActiveView(VIWunknown)
        Me.MLVitemData.SetActiveView(VIWstandard)

        Me.LTitemUnknown.Text = Resource.getValue("SaveError")
        Me.UDPdata.Update()
    End Sub
    Public Sub ItemUnknown() Implements IViewEditMenubar.ItemUnknown
        Me.MLVitemInfo.SetActiveView(VIWunknown)
        Me.MLVitemData.SetActiveView(VIWstandard)

        Me.LTitemUnknown.Text = Resource.getValue("ItemUnknown")
        Me.UDPdata.Update()
    End Sub
    Public Sub DeleteError(type As MenuItemType) Implements IViewEditMenubar.DeleteError
        Me.MLVitemInfo.SetActiveView(VIWunknown)
        Me.MLVitemData.SetActiveView(VIWstandard)

        Me.LTitemUnknown.Text = String.Format(Resource.getValue("DeleteError"), Resource.getValue("MenuItemType." & type.ToString))
        Me.UDPdata.Update()
    End Sub
    Public Sub NoPermission() Implements IViewEditMenubar.NoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewEditMenubar.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        webPost.Redirect(dto)
    End Sub
#End Region

End Class