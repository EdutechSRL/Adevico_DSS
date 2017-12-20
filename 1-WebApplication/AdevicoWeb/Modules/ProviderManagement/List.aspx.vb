Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation

Public Class ListAuthenticationProvider
    Inherits PageBase
    Implements IViewProvidersManagement

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As ProvidersManagementPresenter
    Private ReadOnly Property CurrentPresenter() As ProvidersManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProvidersManagementPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public ReadOnly Property PreLoadedPageSize As Integer Implements IViewProvidersManagement.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property
    Public Property CurrentPageSize As Integer Implements IViewProvidersManagement.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property
    Public Property Pager As PagerBase Implements IViewProvidersManagement.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Resource.getValue("LTprogress.text")
        End Get
    End Property


    Public Function TranslateModalView(viewName As String) As String
        Return Resource.getValue(viewName)
    End Function
    Public ReadOnly Property BackGroundItem(ByVal deleted As lm.Comol.Core.DomainModel.BaseStatusDeleted, itemType As ListItemType) As String
        Get
            If deleted = lm.Comol.Core.DomainModel.BaseStatusDeleted.None Then
                Return IIf(itemType = ListItemType.AlternatingItem, "ROW_Alternate_Small", "ROW_Normal_Small")
            Else
                Return "ROW_Disabilitate_Small"
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer
        Get
            Return 25
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
        Me.Page.Form.DefaultFocus = Me.DDLpage.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.DDLpage.UniqueID
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProviderManagement", "Modules", "ProviderManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceManagementTitle")
            Me.Master.ServiceNopermission = .getValue("serviceManagementNopermission")

            .setHyperLink(Me.HYPaddProvider, True, True)
            Me.HYPaddProvider.NavigateUrl = Me.BaseUrl & RootObject.AddProvider
            .setLabel(LBpagesize)

            .setLabel(LBproviderDisplayInfoDescription)
            .setButton(BTNcloseProviderDisplayInfo, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

#Region "Grid Management"
    Private Sub RPTproviders_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTproviders.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBactions_t")
            Me.Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBproviderIsEnabled_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBproviderName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBproviderType_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBproviderUsedBy_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim provider As dtoProvider = DirectCast(e.Item.DataItem, dtoProviderPermission).Provider
            Dim permission As dtoPermission = DirectCast(e.Item.DataItem, dtoProviderPermission).Permission
            Dim link As LinkButton = e.Item.FindControl("LNBproviderInfo")
            link.Visible = permission.Info
            If permission.Info Then
                Resource.setLinkButton(link, True, True)
            End If

            Dim hyperlink As HyperLink = e.Item.FindControl("HYPedit")
            hyperlink.Visible = permission.Edit
            If permission.Edit Then
                Dim div As HtmlControl = e.Item.FindControl("DVsettings")
                Resource.setHyperLink(hyperlink, True, True)
                hyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditProvider(provider.IdProvider)
                hyperlink = e.Item.FindControl("HYPadvancedSettings")
                div.Visible = False
                If (provider.Type = lm.Comol.Core.Authentication.AuthenticationProviderType.Internal OrElse provider.Type = lm.Comol.Core.Authentication.AuthenticationProviderType.None) Then
                    hyperlink.Visible = False
                Else
                    div.Visible = True
                    hyperlink.Visible = True
                    Resource.setHyperLink(hyperlink, True, True)
                    hyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditProviderSettings(provider.IdProvider, provider.Type)
                End If
            End If

            If permission.Delete AndAlso provider.Type <> lm.Comol.Core.Authentication.AuthenticationProviderType.Internal Then
                link = e.Item.FindControl("LNBdelete")
                link.Visible = True
                Resource.setLinkButton(link, True, True, , True)
            End If
            If permission.VirtualUndelete Then
                link = e.Item.FindControl("LNBvirtualUnDelete")
                link.Visible = True
                Resource.setLinkButton(link, True, True)
            ElseIf permission.VirtualDelete Then
                link = e.Item.FindControl("LNBvirtualDelete")
                link.Visible = True
                Resource.setLinkButton(link, True, True)
            End If
            Dim label As Label = e.Item.FindControl("LBproviderName")
            If Not IsNothing(provider.Translation) AndAlso String.IsNullOrEmpty(provider.Translation.Name) = False Then
                label.Text = provider.Translation.Name
            Else
                label.Text = provider.Name
            End If
            label = e.Item.FindControl("LBproviderType")
            label.Text = Resource.getValue("AuthenticationProviderType." & provider.Type.ToString)

            label = e.Item.FindControl("LBproviderUsedBy")
            label.Text = provider.UsedBy

            label = e.Item.FindControl("LBproviderIsEnabled")
            label.Text = Resource.getValue("isEnabled." & provider.isEnabled.ToString)

        End If
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadProviders(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub RPTproviders_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTproviders.ItemCommand
        Dim idProvider As Long = 0
        If IsNumeric(e.CommandArgument) Then
            idProvider = CLng(e.CommandArgument)
        End If
        If idProvider > 0 Then
            Select Case e.CommandName.ToLower
                Case "infoprovider"
                    Me.LoadProviderInfo(idProvider)
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDelete(idProvider)
                Case "undelete"
                    Me.CurrentPresenter.VirtualUndelete(idProvider)
                Case "delete"
                    Me.CurrentPresenter.PhisicalDelete(idProvider)
            End Select
        Else
            Me.CurrentPresenter.LoadProviders(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
        End If
    End Sub
#End Region

#Region "Implements"
    Public Sub LoadProviderInfo(idProvider As Long) Implements IViewProvidersManagement.LoadProviderInfo
        Me.CTRLinfoProvider.InitializeControl(idProvider)
        Me.DVproviderInfo.Visible = True
    End Sub
    Public Sub LoadProviders(items As List(Of dtoProviderPermission)) Implements IViewProvidersManagement.LoadProviders
        Me.RPTproviders.Visible = (items.Count > 0)
        If items.Count > 0 Then
            Me.RPTproviders.DataSource = items
            Me.RPTproviders.DataBind()
        End If
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewProvidersManagement.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.Management
        webPost.Redirect(dto)
    End Sub
    Public Sub NoPermission() Implements IViewProvidersManagement.NoPermission
        Me.BindNoPermessi()
    End Sub
#End Region

    Private Sub BTNcloseProviderDisplayInfo_Click(sender As Object, e As System.EventArgs) Handles BTNcloseProviderDisplayInfo.Click
        Me.DVproviderInfo.Visible = False
    End Sub

End Class