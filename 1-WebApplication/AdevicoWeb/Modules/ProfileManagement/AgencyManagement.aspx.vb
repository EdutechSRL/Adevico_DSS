Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Partial Public Class AgencyManagement
    Inherits PageBase
    Implements IViewAgenciesManagement


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
    Private _Presenter As AgenciesManagementPresenter
    Private ReadOnly Property CurrentPresenter() As AgenciesManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AgenciesManagementPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property GetCurrentFilters As dtoAgencyFilters Implements IViewAgenciesManagement.GetCurrentFilters
        Get
            Dim dto As New dtoAgencyFilters

            With dto
                .Ascending = OrderAscending
                .OrderBy = OrderBy
                .PageIndex = Me.Pager.PageIndex
                .PageSize = CurrentPageSize
                .SearchBy = SelectedSearchBy
                .StartWith = CurrentStartWith
                .Availability = SelectedAvailability
                .Value = CurrentValue
            End With
            Return dto
        End Get
    End Property
    Private ReadOnly Property PreLoadedPageSize As Integer Implements IViewAgenciesManagement.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedReloadFilters As Boolean Implements IViewAgenciesManagement.PreloadedReloadFilters
        Get
            Return (Request.QueryString("ReloadFilters") = "true")
        End Get
    End Property

    Private Property SelectedSearchBy As SearchAgencyBy Implements IViewAgenciesManagement.SelectedSearchBy
        Get
            If (Me.DDLsearchBy.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchAgencyBy).GetByString(Me.DDLsearchBy.SelectedValue, SearchAgencyBy.All)
            Else
                Return SearchProfilesBy.All
            End If
        End Get
        Set(value As SearchAgencyBy)
            If Not IsNothing(Me.DDLsearchBy.Items.FindByValue(value.ToString)) Then
                Me.DDLsearchBy.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentStartWith As String Implements IViewAgenciesManagement.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property
    Private Property CurrentValue As String Implements IViewAgenciesManagement.CurrentValue
        Get
            Return TXBvalue.Text
        End Get
        Set(value As String)
            TXBvalue.Text = value
        End Set
    End Property
    Private Property SelectedAvailability As AgencyAvailability Implements IViewAgenciesManagement.SelectedAvailability
        Get
            If (Me.DDLagencyAvailability.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of AgencyAvailability).GetByString(Me.DDLagencyAvailability.SelectedValue, AgencyAvailability.All)
            Else
                Return AgencyAvailability.All
            End If
        End Get
        Set(value As AgencyAvailability)
            If Not IsNothing(Me.DDLagencyAvailability.Items.FindByValue(value.ToString)) Then
                Me.DDLagencyAvailability.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private ReadOnly Property GetSavedFilters As dtoAgencyFilters Implements IViewAgenciesManagement.GetSavedFilters
        Get
            Dim dto As New dtoAgencyFilters
            Try
                With dto
                    Dim value As String = Me.Request.Cookies("AgenciesManagement")("Ascending")
                    .Ascending = IIf(String.IsNullOrEmpty(value), True, CBool(value))
                    .OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderAgencyBy).GetByString(Me.Request.Cookies("AgenciesManagement")("OrderBy"), OrderAgencyBy.Name)
                    .PageIndex = Me.Request.Cookies("AgenciesManagement")("PageIndex")
                    .PageSize = Me.Request.Cookies("AgenciesManagement")("PageSize")
                    .SearchBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchAgencyBy).GetByString(Me.Request.Cookies("AgenciesManagement")("SearchBy"), SearchAgencyBy.All)
                    .Availability = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of AgencyAvailability).GetByString(Me.Request.Cookies("AgenciesManagement")("Availability"), AgencyAvailability.All)
                    .StartWith = Me.Request.Cookies("AgenciesManagement")("StartWith")
                    .Value = Me.Request.Cookies("AgenciesManagement")("Value")
                End With
            Catch ex As Exception
                With dto
                    .Ascending = True
                    .OrderBy = OrderAgencyBy.Name
                    .PageIndex = 0
                    .PageSize = CurrentPageSize
                    .SearchBy = SearchAgencyBy.Contains
                    .Availability = AgencyAvailability.All
                    .StartWith = ""
                    .Value = ""
                End With
            End Try

            Return dto

        End Get
    End Property

    Private Property SearchFilters As dtoAgencyFilters Implements IViewAgenciesManagement.SearchFilters
        Get
            Return ViewStateOrDefault("SearchFilters", GetCurrentFilters)
        End Get
        Set(value As dtoAgencyFilters)
            ViewState("SearchFilters") = value
            SaveCurrentFilters(value)
        End Set
    End Property
    Private Property OrderAscending As Boolean Implements IViewAgenciesManagement.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property
    Private Property OrderBy As OrderAgencyBy Implements IViewAgenciesManagement.OrderBy
        Get
            Return ViewStateOrDefault("OrderBy", OrderAgencyBy.Name)
        End Get
        Set(value As OrderAgencyBy)
            ViewState("OrderBy") = value
        End Set
    End Property
    Private Property CurrentPageSize As Integer Implements IViewAgenciesManagement.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Private Property Pager As PagerBase Implements IViewAgenciesManagement.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase With {.PageSize = CurrentPageSize})
        End Get
        Set(value As PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid.Pager = value
            Me.PGgrid.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DIVpageSize.Visible = (Not value.Count < Me.DefaultPageSize)
        End Set
    End Property
    Private Property AllowImport As Boolean Implements IViewAgenciesManagement.AllowImport
        Get
            Return ViewStateOrDefault("AllowImport", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowImport") = value
            Me.HYPaddAgency.Visible = value
            Me.HYPimportAgency.Visible = value
        End Set
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property TranslateModalView(viewName As String) As String
        Get
            Return ""
        End Get
    End Property
    Public ReadOnly Property BackGroundItem(ByVal type As ListItemType, deleted As BaseStatusDeleted) As String
        Get
            If deleted <> BaseStatusDeleted.None Then
                Return "ROW_Disabilitate_Small"
            ElseIf type = ListItemType.Item Then
                Return "ROW_Normal_Small"
            Else
                Return "ROW_Alternate_Small"
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer
        Get
            Return 25
        End Get
    End Property
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Pager
        Me.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBvalue.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNcerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBvalue.UniqueID
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False

        Me.SetFocus(Me.TXBvalue)
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
        MyBase.SetCulture("pg_AgencyManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("serviceNopermission")
            .setLabel(LBtipoRicerca_t)
            .setButton(BTNcerca, True)
            .setHyperLink(Me.HYPaddAgency, True, True)
            .setHyperLink(Me.HYPimportAgency, True, True)

            Me.HYPaddAgency.NavigateUrl = Me.BaseUrl & RootObject.AddAgency()
            Me.HYPimportAgency.NavigateUrl = Me.BaseUrl & RootObject.ImportAgencies()
            .setLabel(LBpagesize)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermission() Implements IViewAgenciesManagement.NoPermission
        Master.ShowNoPermission = True
    End Sub

#Region "Filters"

#Region "Loaders"
    Private Sub LoadSearchAgenciesBy(list As List(Of SearchAgencyBy), defaultSearch As SearchAgencyBy) Implements IViewAgenciesManagement.LoadSearchAgenciesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchAgencyBy." & s.ToString)}).ToList

        Me.DDLsearchBy.DataSource = translations
        Me.DDLsearchBy.DataValueField = "Id"
        Me.DDLsearchBy.DataTextField = "Translation"
        Me.DDLsearchBy.DataBind()
        Me.SelectedSearchBy = defaultSearch
    End Sub
    Private Sub LoadAgencyAvailability(list As List(Of AgencyAvailability), dValue As AgencyAvailability) Implements IViewAgenciesManagement.LoadAgencyAvailability
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("AgencyAvailability." & s.ToString)}).ToList

        Me.DDLagencyAvailability.DataSource = translations
        Me.DDLagencyAvailability.DataValueField = "Id"
        Me.DDLagencyAvailability.DataTextField = "Translation"
        Me.DDLagencyAvailability.DataBind()
        Me.SelectedSearchBy = dValue
    End Sub

    Private Sub SaveCurrentFilters(filters As dtoAgencyFilters) Implements IViewAgenciesManagement.SaveCurrentFilters
        Try
            Me.Response.Cookies("AgenciesManagement")("Value") = filters.Value
            Me.Response.Cookies("AgenciesManagement")("SearchBy") = filters.SearchBy.ToString
            Me.Response.Cookies("AgenciesManagement")("StartWith") = filters.StartWith
            Me.Response.Cookies("AgenciesManagement")("PageIndex") = filters.PageIndex
            Me.Response.Cookies("AgenciesManagement")("PageSize") = filters.PageSize
            Me.Response.Cookies("AgenciesManagement")("OrderBy") = filters.OrderBy
            Me.Response.Cookies("AgenciesManagement")("Ascending") = filters.Ascending
            Me.Response.Cookies("AgenciesManagement")("Availability") = filters.Availability
        Catch ex As Exception

        End Try
    End Sub

    Private Sub InitializeWordSelector(words As List(Of String)) Implements IViewAgenciesManagement.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(words)
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String), activeWord As String) Implements IViewAgenciesManagement.InitializeWordSelector
        Me.DVletters.Visible = True
        Me.CTRLalphabetSelector.InitializeControl(words, activeWord)
    End Sub
#End Region

#Region "Filters use"
    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem

        Dim dto As dtoAgencyFilters = GetCurrentFilters
        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        dto.StartWith = letter
        Me.SearchFilters = dto
        Me.CurrentPresenter.LoadAgencies(dto, 0, Me.CurrentPageSize)
    End Sub
#End Region

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Dim dto As dtoAgencyFilters = GetCurrentFilters

        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        Me.SearchFilters = dto
        Me.CurrentPresenter.LoadAgencies(dto, 0, Me.CurrentPageSize)
    End Sub
#End Region

#Region "Grid use"

    Public Sub LoadProfiles(items As List(Of dtoAgencyItem)) Implements IViewAgenciesManagement.LoadAgencies
        If items.Count = 0 Then
            Me.RPTagencies.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTagencies.DataSource = items
            Me.RPTagencies.DataBind()
            Me.RPTagencies.Visible = True
        End If
    End Sub
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadAgencies(Me.SearchFilters, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub

    Private Sub RPTagencies_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTagencies.ItemCommand
        Dim idAgency As Long = 0

        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAgency = CLng(e.CommandArgument)

            Select Case e.CommandName
                Case "recover"
                    Me.CurrentPresenter.Recover(idAgency, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
                Case "virtualdelete"
                    Me.CurrentPresenter.VirtualDelete(idAgency, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
                Case Else
                    Me.CurrentPresenter.LoadAgencies(Me.SearchFilters, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
            End Select
        Else
            Me.CurrentPresenter.LoadAgencies(Me.SearchFilters, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
        End If
    End Sub

    Private Sub RPTprofiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTagencies.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBagencyActions_t")
            Me.Resource.setLabel(oLabel)
           
            oLabel = e.Item.FindControl("LBname_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBexternalCode_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtaxCode_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBnationalCode_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBemployeesNumber_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoAgencyItem = DirectCast(e.Item.DataItem, dtoAgencyItem)
            Dim agency As dtoAgency = item.Agency
            Dim permission As dtoAgencyPermission = item.Permission
            Dim hyperlink As HyperLink = e.Item.FindControl("HYPinfo")

            Resource.setHyperLink(hyperlink, True, True)
            Dim oLiteral As Literal
            If permission.Info Then
                hyperlink.NavigateUrl = Me.BaseUrl & RootObject.AgencyInfo(agency.Id)
            Else
                hyperlink.NavigateUrl = "#"
            End If
            hyperlink.Visible = permission.Info

            hyperlink = e.Item.FindControl("HYPedit")
            hyperlink.NavigateUrl = Me.BaseUrl & RootObject.EditAgency(agency.Id)
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.Visible = permission.Edit


            Dim oLinkButton As LinkButton
            If permission.VirtualDelete Then
                oLinkButton = e.Item.FindControl("LNBvirtualDelete")
                oLinkButton.Visible = True
                Resource.setLinkButton(oLinkButton, True, True)
            End If
            If permission.VirtualUndelete Then
                oLinkButton = e.Item.FindControl("LNBrecover")
                oLinkButton.Visible = True
                Resource.setLinkButton(oLinkButton, True, True)
            End If
            If permission.Delete Then
                hyperlink = e.Item.FindControl("HYPdelete")
                Resource.setHyperLink(hyperlink, True, True)
                hyperlink.NavigateUrl = Me.BaseUrl & RootObject.DeleteAgency(agency.Id)
                hyperlink.Visible = permission.Delete
            End If

            oLiteral = e.Item.FindControl("LTactions")
            oLiteral.Visible = Not permission.Delete AndAlso Not permission.VirtualDelete AndAlso Not permission.VirtualUndelete
        End If
    End Sub

#End Region

    Private Sub DDLpage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.LoadAgencies(Me.SearchFilters, Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewAgenciesManagement.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.ManagementAgenciesWithFilters
        webPost.Redirect(dto)
    End Sub


   
    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class