Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.Menu.Domain
Imports lm.Comol.Modules.Standard.Menu.Presentation
Imports lm.Comol.UI.Presentation
Public Class AddMenuBar
    Inherits PageBase
    Implements IViewAddMenubar


#Region "Generics"
    Private _Presenter As AddMenubarPresenter
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
    Public ReadOnly Property CurrentPresenter() As AddMenubarPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddMenubarPresenter(Me.CurrentContext, Me)
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
    Public Property AllowCreate As Boolean Implements IViewAddMenubar.AllowCreate
        Get
            Return Me.LNBcreate.Visible
        End Get
        Set(value As Boolean)
            Me.LNBcreate.Visible = value
            Me.HYPbackToManagement.NavigateUrl = BaseUrl & rootObject.MenuBarList(Me.PreloadType)
        End Set
    End Property
    Public Property MenuBar As dtoMenubar Implements IViewAddMenubar.MenuBar
        Get
            Dim dto As New dtoMenubar
            dto.CssClass = Me.TXBcssClass.Text
            dto.Name = Me.TXBname.Text
            dto.MenuBarType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Me.DDLtype.SelectedValue, MenuBarType.None)
            Return dto
        End Get
        Set(value As dtoMenubar)
            Me.TXBcssClass.Text = value.CssClass
            Me.TXBname.Text = value.Name
            Me.DDLtype.SelectedValue = value.MenuBarType.ToString
        End Set
    End Property
    Public ReadOnly Property PreloadType As MenuBarType Implements IViewAddMenubar.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of MenuBarType).GetByString(Request.QueryString("type"), MenuBarType.GenericCommunity)
        End Get
    End Property

    Public Property SubItemsnumber As Integer Implements IViewAddMenubar.SubItemsnumber
        Get
            Return Me.TXBsubItemsnumber.Text
        End Get
        Set(value As Integer)
            Me.TXBsubItemsnumber.Text = value
        End Set
    End Property

    Public Property TopItemsnumber As Integer Implements IViewAddMenubar.TopItemsnumber
        Get
            Return Me.TXBtopItemsnumber.Text
        End Get
        Set(value As Integer)
            Me.TXBtopItemsnumber.Text = value
        End Set
    End Property

    Public ReadOnly Property DefaultMenuName As String Implements IViewAddMenubar.DefaultMenuName
        Get
            Return String.Format(Resource.getValue("DefaultMenuName"), DateTime.Now.ToShortDateString)
        End Get
    End Property
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
                imaster.ServiceTitle = .getValue("serviceTitleAdd")
                imaster.ServiceNopermission = .getValue("nopermission")
            End If

            .setLinkButton(LNBcreate, True, True)

            .setLabel(LBmenubarName)
            .setLabel(LBmenubarType)
            .setLabel(LBdescription)
            .setLabel(LBtopItemsCount_t)
            .setLabel(LBsubItemsCount_t)
            .setLabel(LBcssClass)
            .setHyperLink(HYPbackToManagement, True, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region


    Public Sub LoadAvailableTypes(views As List(Of MenuBarType)) Implements IViewAddMenubar.LoadAvailableTypes
        Me.DDLtype.Items.Clear()
        For Each type As MenuBarType In views
            Me.DDLtype.Items.Add(New ListItem(Me.Resource.getValue("MenuBarType." & type.ToString), type.ToString))
        Next
    End Sub

    Public Sub NoPermission() Implements IViewAddMenubar.NoPermission
        Me.BindNoPermessi()
    End Sub
   

    Private Sub LNBcreate_Click(sender As Object, e As System.EventArgs) Handles LNBcreate.Click
        Me.CurrentPresenter.AddMenuBar()
    End Sub

    Public Sub EditMenubar(IdMenubar As Long, view As MenuBarType) Implements IViewAddMenubar.EditMenubar
        Me.PageUtility.RedirectToUrl(rootObject.EditMenuBar(IdMenubar, view))
    End Sub
    Public Sub CreationError() Implements IViewAddMenubar.CreationError
        ' VISUALIZZARE MESSAGGIO DI ERRORE
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewAddMenubar.DisplaySessionTimeout
        Dim idCommunity As Integer = 0
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = url
        webPost.Redirect(dto)
    End Sub
End Class