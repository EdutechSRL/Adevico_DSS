Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Imports lm.Comol.UI.Presentation

Public Class MenubarList
    Inherits PageBase
    Implements IViewMenubarList


#Region "Generics"
    Private _Presenter As MenubarListPresenter
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
    Public ReadOnly Property CurrentPresenter() As MenubarListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MenubarListPresenter(Me.CurrentContext, Me)
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
    Protected ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            Return ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
        End Get
    End Property
#End Region

#Region "IViewMenuBarList"
    Public Property AllowCreate As Boolean Implements IViewMenubarList.AllowCreate
        Get
            Return ViewStateOrDefault("AllowCreate", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowCreate") = value
            Me.HYPaddMenuBar.Visible = value
        End Set
    End Property

    Public Property CurrentView As MenuBarType Implements IViewMenubarList.CurrentView
        Get
            Return ViewStateOrDefault("CurrentView", MenuBarType.GenericCommunity)
        End Get
        Set(ByVal value As MenuBarType)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBSmenubarTypes.FindTabByValue(value.ToString)
            If Not IsNothing(oTab) Then
                oTab.Selected = True
                Me.ViewState("CurrentView") = value
                Me.HYPaddMenuBar.NavigateUrl = BaseUrl & rootObject.CreateMenuBar(value)
            End If
        End Set
    End Property

    Public Property Pager As PagerBase Implements IViewMenubarList.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.LTpage.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            DIVpageSize.Visible = (value.Count > Me.DDLpage.Items(0).Value)
        End Set
    End Property

    Public ReadOnly Property PreloadView As MenuBarType Implements IViewMenubarList.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Request.QueryString("View"), MenuBarType.GenericCommunity)
        End Get
    End Property
    Public Property CurrentPageSize As Integer Implements IViewMenubarList.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLpage.Items.FindByValue(value)) Then
                Me.DDLpage.SelectedValue = value
            End If
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

    Protected Function BackGroundItem(ByVal oItem As BaseStatusDeleted, ByVal ItemType As ListItemType) As String
        If oItem <> BaseStatusDeleted.None Then
            Return "ROW_Disabilitate_Small"
        ElseIf ItemType = ListItemType.AlternatingItem Then
            Return "ROW_Alternate_Small"
        Else
            Return "ROW_Normal_Small"
        End If
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If TypeOf Me.Master Is IviewMaster Then
            DirectCast(Me.Master, IviewMaster).ShowNoPermission = False
        End If
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
            ' Me.TMsession.Interval = Me.SystemSettings.Presenter.AjaxTimer
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
        MyBase.SetCulture("pg_MenubarList", "Modules", "Menu")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            If TypeOf Me.Master Is IviewMaster Then
                Dim imaster As IviewMaster = DirectCast(Me.Master, IviewMaster)
                imaster.ServiceTitle = .getValue("serviceTitle")
                imaster.ServiceNopermission = .getValue("nopermission")
            End If

            Dim options As New List(Of String)
            Me.DLGvirtualDeleteMenuBar.DialogTitle = .getValue("DLGvirtualDeleteMenuBarTitle")
            Me.DLGvirtualDeleteMenuBar.DialogText = .getValue("DLGvirtualDeleteMenuBarText")
            Me.DLGvirtualDeleteMenuBar.InitializeControl(options, -1)

            Me.DLGdeleteMenuBar.DialogTitle = .getValue("DLGdeleteMenuBarTitle")
            Me.DLGdeleteMenuBar.DialogText = .getValue("DLGdeleteMenuBarText")
            Me.DLGdeleteMenuBar.InitializeControl(options, -1)


            Me.DLGeditActiveMenuBar.DialogTitle = .getValue("DLGeditActiveMenuBarTitle")
            Me.DLGeditActiveMenuBar.DialogText = .getValue("DLGeditActiveMenuBarText")
            Me.DLGeditActiveMenuBar.InitializeControl(options, -1)


            .setHyperLink(HYPaddMenuBar, True, True)
            .setLabel(LBpagesize)
            .setLabel(LBnoItems)
            .setLiteral(LTpage)
        End With


    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    Public Sub NoPermission() Implements lm.Comol.Modules.Standard.Menu.Presentation.IViewMenubarList.NoPermission

    End Sub
    Public Sub LoadItems(items As List(Of dtoMenubarItemPermission)) Implements IViewMenubarList.LoadItems
        If Not IsNothing(items) Then
            Me.LBnoItems.Visible = (items.Count = 0)
            Me.RPTmenubar.Visible = (items.Count > 0)
            Me.DVpaging.Visible = (items.Count > 0)
            Me.RPTmenubar.DataSource = items
            Me.RPTmenubar.DataBind()
        Else
            Me.RPTmenubar.Visible = False
        End If
    End Sub
    Public Sub LoadAvailableViews(views As List(Of MenuBarType)) Implements IViewMenubarList.LoadAvailableViews
        Me.TBSmenubarTypes.Enabled = (views.Count > 0)
        Dim SelectedIndex As Integer = 0
        If views.Count > 0 Then
            For Each view As MenuBarType In views
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBSmenubarTypes.Tabs.FindTabByValue(view.ToString)
                If Not IsNothing(oTabView) Then
                    oTabView.NavigateUrl = Me.PageUtility.BaseUrl & rootObject.MenuBarList(view)
                    oTabView.Text = Resource.getValue("MenuBarType." & view.ToString)
                    oTabView.Visible = True
                End If
            Next
        End If
        Me.TBSmenubarTypes.SelectedIndex = SelectedIndex
    End Sub
    Public Sub EditMenubar(idMenubar As Long, type As MenuBarType) Implements IViewMenubarList.EditMenubar
        PageUtility.RedirectToUrl(rootObject.EditMenuBar(idMenubar, type))
    End Sub

    Public Sub ReloadPage(idMenubar As Long, type As MenuBarType, pageIndex As Integer, pageSize As Integer) Implements IViewMenubarList.ReloadPage
        PageUtility.RedirectToUrl(rootObject.MenuBarList(type))
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewMenubarList.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        webPost.Redirect(dto)
    End Sub
#End Region

    Private Sub RPTmenubar_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmenubar.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dtoMenubarItemPermission As dtoMenubarItemPermission = DirectCast(e.Item.DataItem, dtoMenubarItemPermission)
            Dim dto As dtoMenubar = dtoMenubarItemPermission.MenuBar
            Dim permission As dtoPermission = dtoMenubarItemPermission.Permission
            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBsetDefault")
            oLinkButton.Visible = Not dto.IsCurrent
            oLinkButton.CommandArgument = dto.Id
            Me.Resource.setLinkButton(oLinkButton, True, True)

            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPeditMenuBar")
            oHyperlink.Visible = Not dto.IsCurrent
            Me.Resource.setHyperLink(oHyperlink, True, True)
            If Not dto.IsCurrent Then
                oHyperlink.NavigateUrl = BaseUrl & rootObject.EditMenuBar(dto.Id, dto.MenuBarType)
            Else
                'cosa fare se è quello in linea ? 
                ' un alert e poi si genera il nuovo! 
                ' quindi va messo un dialogbutton e non un hyperlink perchè serve codice lato server !
                Dim editButton As MyUC.DialogLinkButton = e.Item.FindControl("DLBedit")
                editButton.CommandName = "edit"
                editButton.CommandArgument = dto.Id
                editButton.InitializeSingleSelectControlByClass("editActiveDial", -1)
                editButton.Visible = True
                Me.Resource.setLinkButton(editButton, True, True)

            End If

            oHyperlink = e.Item.FindControl("HYPviewMenuBar")
            oHyperlink.Visible = True
            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.NavigateUrl = BaseUrl & rootObject.ViewMenuBar(dto.Id, dto.MenuBarType)


            Dim oVirtualDelete As MyUC.DialogLinkButton = e.Item.FindControl("LNBvirtualDelete")
            Me.Resource.setLinkButton(oVirtualDelete, True, True)

            oVirtualDelete.CommandArgument = dto.Id
            oVirtualDelete.Visible = permission.VirtualDelete
            oVirtualDelete.InitializeSingleSelectControlByClass("mandatoryDial", -1)

            Dim oLabel As Label = e.Item.FindControl("LBmenubarType")
            oLabel.Text = Me.Resource.getValue("MenuBarType." & dto.MenuBarType.ToString())
            oLabel = e.Item.FindControl("LBisDefault")
            oLabel.Visible = dto.IsCurrent

            Dim oVirtualUnDelete As LinkButton = e.Item.FindControl("LNBvirtualUndelete")
            Me.Resource.setLinkButton(oVirtualUnDelete, True, True)
            oVirtualUnDelete.CommandArgument = dto.Id
            oVirtualUnDelete.Visible = permission.VirtualUndelete


            Dim oPhisicalDelete As MyUC.DialogLinkButton = e.Item.FindControl("LNBphisicalDelete")
            Me.Resource.setLinkButton(oPhisicalDelete, True, True)
            oPhisicalDelete.CommandArgument = dto.Id
            oPhisicalDelete.InitializeSingleSelectControlByClass("deleteMenubar", -1)
            oPhisicalDelete.Visible = permission.Delete

            oLabel = e.Item.FindControl("LBstatus")
            oLabel.Text = Me.Resource.getValue("ItemStatus." & dto.Status.ToString())

            oLabel = e.Item.FindControl("LBinfo")
            oLabel.Text = GetDateTimeString(dto.ModifiedOn, "//")
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBname")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBmenubarType")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBstatus")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBinfo")
            Resource.setLabel(oLabel)
        End If
    End Sub
    Private Sub RPTmenubar_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmenubar.ItemCommand
        Select Case e.CommandName
            Case "setDefault"
                Dim idMenubar As Long = CLng(e.CommandArgument)
                Me.CurrentPresenter.SetActiveMenubar(idMenubar)
            Case "virtualUnDelete"
                Dim idMenubar As Long = CLng(e.CommandArgument)
                Me.CurrentPresenter.VirtualUnDeleteMenubar(idMenubar)
        End Select
    End Sub
    Private Sub DLGvirtualDeleteMenuBar_ButtonPressed(dialogResult As Integer, commandArgument As String, commandName As String) Handles DLGvirtualDeleteMenuBar.ButtonPressed
        Select Case commandName
            Case "virtualDelete"
                Dim idMenubar As Long = CLng(commandArgument)
                Me.CurrentPresenter.VirtualDeleteMenubar(idMenubar)
        End Select
    End Sub

    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadMenubarItems(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub DLGeditActiveMenuBar_ButtonPressed(dialogResult As Integer, CommandArgument As String, CommandName As String) Handles DLGeditActiveMenuBar.ButtonPressed
        If dialogResult = -1 AndAlso CommandName = "edit" AndAlso IsNumeric(CommandArgument) Then
            Dim idMenubar As Long = CLng(CommandArgument)
            Me.CurrentPresenter.EditActiveMenubar(idMenubar, Resource.getValue("EditActiveMenubar"))
        End If
    End Sub
    Private Sub DLGdeleteMenuBar_ButtonPressed(dialogResult As Integer, CommandArgument As String, CommandName As String) Handles DLGdeleteMenuBar.ButtonPressed
        If dialogResult = -1 AndAlso CommandName = "phisicalDelete" AndAlso IsNumeric(CommandArgument) Then
            Dim idMenubar As Long = CLng(CommandArgument)
            Me.CurrentPresenter.PhisicalDeleteMenubar(idMenubar)
        End If
    End Sub
  
End Class