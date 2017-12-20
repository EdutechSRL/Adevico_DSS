Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Imports lm.Comol.UI.Presentation
Public Class ViewMenubar
    Inherits PageBase
    Implements IViewViewMenubar


#Region "Generics"
    Private _Presenter As ViewMenubarPresenter
    Private _BaseUrl As String
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As ViewMenubarPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ViewMenubarPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdMenubar As Long Implements IViewViewMenubar.IdMenubar
        Get
            Return ViewStateOrDefault("IdMenubar", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdMenubar") = value
        End Set
    End Property
    Public ReadOnly Property PreloadIdMenubar As Long Implements IViewViewMenubar.PreloadIdMenubar
        Get
            If IsNumeric(Request.QueryString("Id")) Then
                Return CLng(Request.QueryString("Id"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreviousView As lm.Comol.Modules.Standard.Menu.Domain.MenuBarType Implements IViewViewMenubar.PreviousView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Request.QueryString("View"), MenuBarType.GenericCommunity)
        End Get
    End Property
    Public Property AllowManage As Boolean Implements lm.Comol.Modules.Standard.Menu.Presentation.IViewViewMenubar.AllowManage
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
            .setLiteral(UPBloadItems.FindControl("LTloadingMessage"))
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "TreeView"
    Public Sub LoadMenubar(tree As dtoTree, selectedItem As dtoItem) Implements IViewViewMenubar.LoadMenubar
        Me.CTRLmenubarTree.InitalizeControl(tree, selectedItem)
        Me.UDPdata.Update()
    End Sub
    Private Sub CTRLmenubarTree_SelectedItemChanged(item As dtoItem) Handles CTRLmenubarTree.SelectedItemChanged
        Me.CurrentPresenter.SelectItem(item)
    End Sub
#End Region

#Region "MenuBarInfo"
    Public Sub LoadMenuBarInfo(menubar As dtoMenubar) Implements IViewViewMenubar.LoadMenuBarInfo
        MLVitemInfo.SetActiveView(VIWmenubarInfo)
        MLVitemData.SetActiveView(VIWstandard)
        Me.CTRLmenubarTree.UpdateDragAndDrop(menubar)
        Me.CTRLmenubarInfo.InitalizeControl(menubar, False)
        Me.UDPdata.Update()
    End Sub
#End Region

#Region "Top Items"
    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation), selectedTypes As List(Of Integer)) Implements IViewViewMenubar.LoadTopMenuItem
        LoadTopMenuItem(item, translations, False)
        Me.CTRLprofileTypes.InitalizeControl(selectedTypes, Nothing, False)
        Me.UDPdata.Update()
    End Sub

    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation)) Implements IViewViewMenubar.LoadTopMenuItem
        LoadTopMenuItem(item, translations, True)
        Me.CTRLmodules.InitalizeControl(item.IdModule, item.Permission, False)
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadTopMenuItem(item As dtoTopMenuItem, translations As List(Of dtoTranslation), showModule As Boolean)
        MLVitemInfo.SetActiveView(VIWtopItemInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)

        Me.CTRLtranslations.InitalizeControl(translations, item.Id, False)
        Me.CTRLtranslations.Visible = True
        Me.CTRLmodules.Visible = showModule
        Me.CTRLprofileTypes.Visible = Not showModule
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLtopItem.InitalizeControl(item, False)
        Me.UDPdata.Update()
    End Sub
#End Region

#Region "Columns"
    Public Sub LoadColumnItem(column As dtoColumn) Implements IViewViewMenubar.LoadColumnItem
        MLVitemInfo.SetActiveView(VIWcolumnInfo)
        MLVitemData.SetActiveView(VIWstandard)
        Me.CTRLmenubarTree.UpdateDragAndDrop(column)
        Me.CTRLcolumn.InitalizeControl(column, False)
        Me.UDPdata.Update()
    End Sub
#End Region

#Region "Items"
    Private Sub LoadSeparatorItem(item As dtoMenuItem) Implements IViewViewMenubar.LoadSeparatorItem
        MLVitemInfo.SetActiveView(VIWmenuInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)
        Me.CTRLmodules.Visible = False
        Me.CTRLprofileTypes.Visible = False
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLmenuItem.InitalizeControl(item, False, New List(Of MenuItemType), New List(Of MenuItemType))
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType)) Implements IViewViewMenubar.LoadMenuItem
        LoadMenuItem(item, translations, True, availabeTypes, availabeSubTypes)
        Me.CTRLmodules.InitalizeControl(item.IdModule, item.Permission, False)
        Me.UDPdata.Update()
    End Sub

    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), selectedTypes As List(Of Integer), availableProfileTypes As List(Of Integer), availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType)) Implements IViewViewMenubar.LoadMenuItem
        LoadMenuItem(item, translations, False, availabeTypes, availabeSubTypes)
        Me.CTRLprofileTypes.InitalizeControl(selectedTypes, availableProfileTypes, False)
        Me.UDPdata.Update()
    End Sub
    Private Sub LoadMenuItem(item As dtoMenuItem, translations As List(Of dtoTranslation), showModule As Boolean, availabeTypes As List(Of MenuItemType), availabeSubTypes As List(Of MenuItemType))
        MLVitemInfo.SetActiveView(VIWmenuInfo)
        Me.MLVitemData.SetActiveView(VIWassignments)

        Me.CTRLtranslations.InitalizeControl(translations, item.Id, False)
        Me.CTRLtranslations.Visible = True
        Me.CTRLmodules.Visible = showModule
        Me.CTRLprofileTypes.Visible = Not showModule
        Me.CTRLmenubarTree.UpdateDragAndDrop(item)
        Me.CTRLmenuItem.InitalizeControl(item, False, availabeTypes, availabeSubTypes)
    End Sub
#End Region

#Region "Implements"
    Public Sub MenubarUnknown() Implements IViewViewMenubar.MenubarUnknown
        MLVitemInfo.SetActiveView(VIWunknown)
        MLVitemData.SetActiveView(VIWstandard)
        Me.LTitemUnknown.Text = Resource.getValue("MenubarUnknown")
        Me.UDPdata.Update()
    End Sub
    Public Sub ItemUnknown() Implements IViewViewMenubar.ItemUnknown
        Me.MLVitemInfo.SetActiveView(VIWunknown)
        Me.MLVitemData.SetActiveView(VIWstandard)

        Me.LTitemUnknown.Text = Resource.getValue("ItemUnknown")
        Me.UDPdata.Update()
    End Sub
    Public Sub NoPermission() Implements IViewViewMenubar.NoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewViewMenubar.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        webPost.Redirect(dto)
    End Sub
#End Region

End Class