Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.Header
Imports lm.Comol.Modules.Standard.Header.Presentation
Imports lm.Comol.Modules.Standard.Menu.Domain
Public Class UC_PortalHeader
    Inherits BaseControl
    Implements IViewPortalHeader

#Region "Context"
    Private _presenter As Presentation.PortalHeaderPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.PortalHeaderPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New Presentation.PortalHeaderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "New Skin"

    Private _NewSkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin

    Private ReadOnly Property NewSkinService As lm.Comol.Modules.Standard.Skin.Business.ServiceSkin
        Get
            If IsNothing(_NewSkinService) Then
                _NewSkinService = New lm.Comol.Modules.Standard.Skin.Business.ServiceSkin(PageUtility.CurrentContext)
            End If

            Return _NewSkinService
        End Get
    End Property

#End Region



#Region "Implements"
    Public ReadOnly Property GetAdministrationName As String Implements IViewPortalHeader.GetAdministrationName
        Get
            ' VEDERE se inserire un  Me.Resource.getValue("AdministrationName")
            'ComName.AdministrationName
            'ComName.PortalName
            Return Me.Resource.getValue("ComName.AdministrationName")
        End Get
    End Property

    Public ReadOnly Property GetPortalName As String Implements IViewPortalHeader.GetPortalName
        Get
            Return SystemSettings.Presenter.PortalDisplay.LocalizeName(PageUtility.LinguaID)
        End Get
    End Property
    Public Property MenubarType As MenuBarType Implements IViewPortalHeader.MenubarType
        Get
            Return ViewStateOrDefault("MenubarType", MenubarType.None)
        End Get
        Set(value As MenuBarType)
            ViewState("MenubarType") = value
        End Set
    End Property
    Public Property IdHeaderCommunity As Integer Implements IViewPortalHeader.IdHeaderCommunity
        Get
            Return ViewStateOrDefault("IdHeaderCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdHeaderCommunity") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region


    Private _Visibility As VisibilityState = VisibilityState.Full
    Public Property Visibility As VisibilityState
        Get
            Return _Visibility
        End Get
        Set(ByVal value As VisibilityState)
            _Visibility = value

            Me.Visible = True
            Me.LTmenu.Visible = True
            Me.UC_TopBar.Visible = True

            Select Case value
                Case VisibilityState.none
                    Me.Visible = False
                Case VisibilityState.OnlyLogo
                    Me.LTmenu.Visible = False
                    Me.UC_TopBar.Visible = False
                Case (VisibilityState.noMenu)
                    Me.LTmenu.Visible = False
            End Select
        End Set
    End Property

    Public Enum VisibilityState
        none = 0
        OnlyLogo = 1
        noMenu = 2
        Full = 3
    End Enum


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitalizeControl()
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("Uc_PortalHeader", "UC")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setLiteral(Me.Lit_Memo_t)
            '.setLiteral(Me.Lit_MemoNew_t)
            '.setLiteral(Me.Lit_MemoToggle_t)
        End With
    End Sub

#End Region

    Public Sub InitalizeControl() Implements IViewPortalHeader.InitalizeControl
        If Not Page.IsPostBack Then
            Dim defaultModuleTooltip As String = Resource.getValue("defaultModuleTooltip")
            If defaultModuleTooltip = "" Then
                defaultModuleTooltip = "Torna alla pagina principale della comunità"
            End If
            CurrentPresenter.InitView(IdHeaderCommunity, MenubarType, PageUtility.ApplicationUrlBase, defaultModuleTooltip, SystemSettings.Presenter.DefaultCommunityModule, Resource.getValue("defaultModuleText"))
        End If
    End Sub

    Public Sub DisplayName(ByVal name As String) Implements IViewPortalHeader.DisplayName
        Me.LBcommunityName.Text = name
        Me.LBcommunityName.ToolTip = name
    End Sub

    ''' <summary>
    ''' Appena ci sarà il nuovo menu: sostituire...
    ''' </summary>
    ''' <param name="HtmlMenu"></param>
    ''' <remarks></remarks>
    Public Sub LoadMenuBar(ByVal HtmlMenu As String) Implements Presentation.IViewPortalHeader.LoadMenuBar
        Me.LTmenu.Text = HtmlMenu
    End Sub

    Public Sub BindLogo(idCommunity As Integer, idOrganization As Integer, languageCode As String) Implements Presentation.IViewPortalHeader.BindLogo
        IMGlogo.Visible = False
        LTlogo.Visible = True
        'NEW SKIN <- spostare in PageUtility?!
        Dim virtualPath As String = Me.BaseUrl & Me.SystemSettings.SkinSettings.SkinVirtualPath
        ' Render Logo
        Dim html As lm.Comol.Modules.Standard.Skin.Domain.HTML.HTMLSkin = NewSkinService.GetSkinHTML( _
            idCommunity, _
            idOrganization, _
            virtualPath, _
            languageCode, _
            Me.SystemSettings.DefaultLanguage.Codice,
            Me.SystemSettings.SkinSettings, Me.BaseUrl
            )

        Me.LTlogo.Text = html.HtmlHeadLogo
    End Sub
End Class