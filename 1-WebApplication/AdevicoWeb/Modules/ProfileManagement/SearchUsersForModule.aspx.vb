Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation

Partial Public Class SearchUsersForModule
    Inherits PageBase
    Implements IViewSearchUsersForModule


#Region "Context"
    Private _Presenter As SearchUsersForModulePresenter
    Private ReadOnly Property CurrentPresenter() As SearchUsersForModulePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SearchUsersForModulePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property AllowAutoupdate As Boolean Implements IViewSearchUsersForModule.AllowAutoupdate
        Get
            Return Me.CBXautoUpdate.Checked
        End Get
    End Property
    Private ReadOnly Property AllowSearchByTaxCode As Boolean Implements IViewSearchUsersForModule.AllowSearchByTaxCode
        Get
            Return Me.SystemSettings.Presenter.DefaultTaxCodeRequired
        End Get
    End Property
    Private ReadOnly Property GetCurrentFilters As dtoFilters Implements IViewSearchUsersForModule.GetCurrentFilters
        Get
            Dim dto As New dtoFilters

            With dto
                .Ascending = OrderAscending
                .OrderBy = OrderBy
                .IdAvailableOrganization = AvailableOrganizations
                .IdOrganization = SelectedIdOrganization
                .PageIndex = Me.Pager.PageIndex
                .PageSize = Me.DDLpage.SelectedValue
                .IdProfileType = SelectedIdProfileType
                If SelectedIdProfileType = UserTypeStandard.Employee Then
                    .IdAgency = SelectedIdAgency
                Else
                    .IdAgency = -1
                End If
                .SearchBy = SelectedSearchBy
                .StartWith = CurrentStartWith
                .Status = SelectedStatusProfile
                .Value = CurrentValue
                .DisplayLoginInfo = False
            End With
            Return dto
        End Get
    End Property
    Private ReadOnly Property PreLoadedPageSize As Integer Implements IViewSearchUsersForModule.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property
    Private ReadOnly Property PreLoadedModuleCode As String Implements IViewSearchUsersForModule.PreLoadedModuleCode
        Get
            Return Request.QueryString("module")
        End Get
    End Property
    Private ReadOnly Property PreLoadedView As String Implements IViewSearchUsersForModule.PreLoadedView
        Get
            Return Request.QueryString("from")
        End Get
    End Property
    Private ReadOnly Property PreLoadedIdCommunity As Integer Implements IViewSearchUsersForModule.PreLoadedIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdCommunity")) AndAlso IsNumeric(Request.QueryString("IdCommunity")) Then
                Return CInt(Request.QueryString("IdCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private Property SelectedIdOrganization As Integer Implements IViewSearchUsersForModule.SelectedIdOrganization
        Get
            If (Me.DDLorganizations.SelectedIndex > -1) Then
                Return Me.DDLorganizations.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLorganizations.Items.FindByValue(value)) Then
                Me.DDLorganizations.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdProfileType As Integer Implements IViewSearchUsersForModule.SelectedIdProfileType
        Get
            If (Me.DDLprofileType.SelectedIndex > -1) Then
                Return Me.DDLprofileType.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            If Not IsNothing(Me.DDLprofileType.Items.FindByValue(value)) Then
                Me.DDLprofileType.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedIdAgency As Long Implements IViewSearchUsersForModule.SelectedIdAgency
        Get
            If (Me.DDLagencies.SelectedIndex > -1) Then
                Return Me.DDLagencies.SelectedValue
            Else
                Return 0
            End If
        End Get
        Set(value As Long)
            If Not IsNothing(Me.DDLagencies.Items.FindByValue(value)) Then
                Me.DDLagencies.SelectedValue = value
            End If
        End Set
    End Property
    Private Property SelectedSearchBy As SearchProfilesBy Implements IViewSearchUsersForModule.SelectedSearchBy
        Get
            If (Me.DDLsearchBy.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.DDLsearchBy.SelectedValue, SearchProfilesBy.All)
            Else
                Return SearchProfilesBy.All
            End If
        End Get
        Set(value As SearchProfilesBy)
            If Not IsNothing(Me.DDLsearchBy.Items.FindByValue(value.ToString)) Then
                Me.DDLsearchBy.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property SelectedStatusProfile As StatusProfile Implements IViewSearchUsersForModule.SelectedStatusProfile
        Get
            If (Me.DDLstatus.SelectedIndex > -1) Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.DDLstatus.SelectedValue, StatusProfile.AllStatus)
            Else
                Return StatusProfile.AllStatus
            End If
        End Get
        Set(value As StatusProfile)
            If Not IsNothing(Me.DDLstatus.Items.FindByValue(value.ToString)) Then
                Me.DDLstatus.SelectedValue = value.ToString
            End If
        End Set
    End Property
    Private Property CurrentStartWith As String Implements IViewSearchUsersForModule.CurrentStartWith
        Get
            Return CTRLalphabetSelector.SelectedItem
        End Get
        Set(value As String)
            CTRLalphabetSelector.SelectedItem = value
        End Set
    End Property
    Public Property CurrentValue As String Implements IViewSearchUsersForModule.CurrentValue
        Get
            If String.IsNullOrEmpty(TXBvalue.Text) Then
                Return ""
            Else
                Return TXBvalue.Text.Trim
            End If
        End Get
        Set(value As String)
            TXBvalue.Text = value
        End Set
    End Property
    Public ReadOnly Property GetSavedFilters As dtoFilters Implements IViewSearchUsersForModule.GetSavedFilters
        Get
            Dim dto As New dtoFilters
            Try
                With dto
                    Dim value As String = Me.Request.Cookies("SearchUsersForModule")("Ascending")
                    If String.IsNullOrEmpty(value) Then
                        .Ascending = False
                    Else
                        .Ascending = CBool(value)
                    End If
                    .idProvider = -1
                    .OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderProfilesBy).GetByString(Me.Request.Cookies("SearchUsersForModule")("OrderBy"), OrderProfilesBy.SurnameAndName)
                    .IdOrganization = Me.Request.Cookies("SearchUsersForModule")("IdOrganization")
                    .IdAgency = Me.Request.Cookies("SearchUsersForModule")("IdAgency")
                    .PageIndex = Me.Request.Cookies("SearchUsersForModule")("PageIndex")
                    .PageSize = Me.Request.Cookies("SearchUsersForModule")("PageSize")
                    .IdProfileType = Me.Request.Cookies("SearchUsersForModule")("IdProfileType")
                    .SearchBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SearchProfilesBy).GetByString(Me.Request.Cookies("SearchUsersForModule")("SearchBy"), SearchProfilesBy.All)
                    .StartWith = Me.Request.Cookies("SearchUsersForModule")("StartWith")
                    .Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of StatusProfile).GetByString(Me.Request.Cookies("SearchUsersForModule")("Status"), StatusProfile.AllStatus)
                    .Value = Me.Request.Cookies("SearchUsersForModule")("Value")
                    .DisplayLoginInfo = False

                End With
            Catch ex As Exception
                With dto
                    .Ascending = True
                    .idProvider = -1
                    .OrderBy = OrderProfilesBy.SurnameAndName
                    .IdOrganization = -1
                    .IdAgency = -1
                    .PageIndex = 0
                    .PageSize = CurrentPageSize
                    .IdProfileType = -1
                    .SearchBy = SearchProfilesBy.Contains
                    .StartWith = ""
                    .Status = StatusProfile.AllStatus
                    .Value = ""
                    .DisplayLoginInfo = False
                End With
            End Try

            Return dto

        End Get
    End Property
    Public ReadOnly Property PreloadedReloadFilters As Boolean Implements IViewSearchUsersForModule.PreloadedReloadFilters
        Get
            Return (Request.QueryString("ReloadFilters") = "true")
        End Get
    End Property
    Public Property SearchFilters As dtoFilters Implements IViewSearchUsersForModule.SearchFilters
        Get
            Return ViewStateOrDefault("SearchFilters", GetCurrentFilters)
        End Get
        Set(value As dtoFilters)
            ViewState("SearchFilters") = value
            SaveCurrentFilters(value)
        End Set
    End Property
    Public Property OrderAscending As Boolean Implements IViewSearchUsersForModule.OrderAscending
        Get
            Return ViewStateOrDefault("OrderAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("OrderAscending") = value
        End Set
    End Property
    Public Property OrderBy As OrderProfilesBy Implements IViewSearchUsersForModule.OrderBy
        Get
            Return ViewStateOrDefault("OrderBy", OrderProfilesBy.SurnameAndName)
        End Get
        Set(value As OrderProfilesBy)
            ViewState("OrderBy") = value
        End Set
    End Property
    Public Property CurrentPageSize As Integer Implements IViewSearchUsersForModule.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(value As Integer)
            Me.DDLpage.SelectedValue = value
        End Set
    End Property

    Public Property Pager As PagerBase Implements IViewSearchUsersForModule.Pager
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
    Public ReadOnly Property GetTranslatedProfileTypes As List(Of TranslatedItem(Of Integer)) Implements IViewSearchUsersForModule.GetTranslatedProfileTypes
        Get
            Return (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID)
                    Select New TranslatedItem(Of Integer) With {.Id = o.ID, .Translation = o.Descrizione}).ToList
        End Get
    End Property
    Private Property AvailableColumns As List(Of ProfileColumn) Implements IViewSearchUsersForModule.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ProfileColumn))
        End Get
        Set(value As List(Of ProfileColumn))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Private Property AvailableOrganizations As List(Of Integer) Implements IViewBaseProfilesManagement.AvailableOrganizations
        Get
            Return ViewStateOrDefault("AvailableOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("AvailableOrganizations") = value
        End Set
    End Property
    Private Property CurrentModuleCode As String Implements IViewSearchUsersForModule.CurrentModuleCode
        Get
            Return ViewStateOrDefault("CurrentModuleCode", "")
        End Get
        Set(value As String)
            ViewState("CurrentModuleCode") = value
            Select Case value
                Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                    Select Case CurrentModuleView
                        Case lm.Comol.Modules.EduPath.Domain.SummaryType.CommunityIndex.ToString
                            HYPback.NavigateUrl = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryIndex(lm.Comol.Modules.EduPath.Domain.SummaryType.CommunityIndex, CurrentModuleIdCommunity)
                        Case lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex.ToString
                            HYPback.NavigateUrl = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryIndex(lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex, 0)
                        Case Else

                    End Select
                    HYPback.Visible = Not String.IsNullOrEmpty(CurrentModuleView)
                    Resource.setHyperLink(HYPback, "ModuleCode." & CurrentModuleView, False, True)
            End Select
        End Set
    End Property
    Private Property CurrentModuleIdCommunity As Integer Implements IViewSearchUsersForModule.CurrentModuleIdCommunity
        Get
            Return ViewStateOrDefault("CurrentModuleIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentModuleIdCommunity") = value
        End Set
    End Property
    Private Property CurrentModuleView As String Implements IViewSearchUsersForModule.CurrentModuleView
        Get
            Return ViewStateOrDefault("CurrentModuleView", "")
        End Get
        Set(value As String)
            ViewState("CurrentModuleView") = value
        End Set
    End Property
#End Region

#Region "Property"
    Public ReadOnly Property TranslateModalView(viewName As String) As String
        Get
            Return ""
        End Get
    End Property
    Public ReadOnly Property isColumnVisible(ByVal column As ProfileColumn) As Boolean
        Get
            Return AvailableColumns.Contains(column)
        End Get
    End Property
    Public ReadOnly Property BackGroundItem(ByVal status As StatusProfile) As String
        Get
            If status = StatusProfile.Active Then
                Return "ROW_Normal_Small"
            ElseIf status = StatusProfile.Disabled Then
                Return "ROW_Disabilitate_Small"
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
            Me.CBXautoUpdate.Checked = True

            'Me.UDPprofiles.Update()
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
        MyBase.SetCulture("pg_ProfilesManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            If String.IsNullOrEmpty(PreLoadedModuleCode) Then
                Me.Master.ServiceTitle = .getValue("serviceTitle")
            Else
                Me.Master.ServiceTitle = .getValue("serviceTitle." & PreLoadedModuleCode)
            End If

            Me.Master.ServiceNopermission = .getValue("serviceNopermission")

            .setLabel(LBorganizzazione_t)
            .setLabel(LBprofileType_t)
            .setLabel(LBstatus_t)

            .setLabel(LBtipoRicerca_t)

            .setLabel(LBvalore_t)
            .setButton(BTNcerca, True)

            .setCheckBox(Me.CBXautoUpdate)
            .setLabel(LBpagesize)

            .setLabel(LBagencyFilter_t)


            DDLagencies.Attributes.Add("onchange", "onUpdating();")
            DDLorganizations.Attributes.Add("onchange", "onUpdating();")
            DDLpage.Attributes.Add("onchange", "onUpdating();")
            DDLprofileType.Attributes.Add("onchange", "onUpdating();")
            DDLstatus.Attributes.Add("onchange", "onUpdating();")
            CBXautoUpdate.Attributes.Add("onchange", "onUpdating();")
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region

    Public Sub NoPermission() Implements IViewSearchUsersForModule.NoPermission
        Master.ShowNoPermission = True
    End Sub
    Private Sub NoPermissionToAdmin() Implements IViewBaseProfilesManagement.NoPermissionToAdmin
        Master.ShowNoPermission = True
        Master.ServiceNopermission = Resource.getValue("NoPermissionToAdmin")
    End Sub
#Region "Filters"

#Region "Loaders"
    Private Sub LoadAvailableOrganizations(items As List(Of Organization), idDefaultOrganization As Integer) Implements IViewSearchUsersForModule.LoadAvailableOrganizations
        If items.Any Then
            AvailableOrganizations = items.Select(Function(i) i.Id).ToList()
        End If
        Me.DDLorganizations.DataSource = items
        Me.DDLorganizations.DataValueField = "Id"
        Me.DDLorganizations.DataTextField = "Name"
        Me.DDLorganizations.DataBind()
        If items.Count > 1 Then
            Me.DDLorganizations.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLorganizations." & -1), -1))
        End If
    End Sub

    Private Sub LoadProfileTypes(idProfileTypes As List(Of Integer), IdDefaultProfileType As Integer) Implements IViewSearchUsersForModule.LoadProfileTypes
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest AndAlso (idProfileTypes.Contains(o.ID)) Select o).ToList
        Me.DDLprofileType.DataSource = oUserTypes
        Me.DDLprofileType.DataValueField = "ID"
        Me.DDLprofileType.DataTextField = "Descrizione"
        Me.DDLprofileType.DataBind()
        If oUserTypes.Count > 1 Then
            Me.DDLprofileType.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLprofileType." & -1), -1))
        End If
        Me.SelectedIdProfileType = IdDefaultProfileType
    End Sub
    Private Sub LoadAvailableStatus(list As List(Of StatusProfile), defaultStatus As StatusProfile) Implements IViewSearchUsersForModule.LoadAvailableStatus
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("StatusProfile." & s.ToString)}).ToList

        Me.DDLstatus.DataSource = translations
        Me.DDLstatus.DataValueField = "Id"
        Me.DDLstatus.DataTextField = "Translation"
        Me.DDLstatus.DataBind()
        Me.SelectedStatusProfile = defaultStatus

    End Sub
    Private Sub LoadAgencies(items As Dictionary(Of Long, String), idDefaultAgency As Long) Implements IViewSearchUsersForModule.LoadAgencies
        Me.DDLagencies.Items.Clear()
        Me.DDLagencies.DataSource = items
        Me.DDLagencies.DataValueField = "Key"
        Me.DDLagencies.DataTextField = "Value"
        Me.DDLagencies.DataBind()

        If items.Count > 1 OrElse items.Count = 0 Then
            Me.DDLagencies.Items.Insert(0, New ListItem(Me.Resource.getValue("DDLagencies." & -1), -1))
        End If
        Me.DVagencyFilter.Visible = True
        Me.SelectedIdAgency = idDefaultAgency
    End Sub
    Private Sub UnLoadAgencies() Implements IViewSearchUsersForModule.UnLoadAgencies
        If Me.DDLagencies.SelectedIndex > -1 Then
            Me.DDLagencies.SelectedIndex = 0
        End If
        Me.DVagencyFilter.Visible = False
    End Sub

    Private Sub LoadSearchProfilesBy(list As List(Of SearchProfilesBy), defaultSearch As SearchProfilesBy) Implements IViewSearchUsersForModule.LoadSearchProfilesBy
        Dim translations As List(Of TranslatedItem(Of String)) = (From s In list Select New TranslatedItem(Of String) With {.Id = s.ToString, .Translation = Me.Resource.getValue("SearchProfilesBy." & s.ToString)}).ToList

        Me.DDLsearchBy.DataSource = translations
        Me.DDLsearchBy.DataValueField = "Id"
        Me.DDLsearchBy.DataTextField = "Translation"
        Me.DDLsearchBy.DataBind()
        Me.SelectedSearchBy = defaultSearch
    End Sub

    Public Sub SaveCurrentFilters(filters As dtoFilters) Implements IViewSearchUsersForModule.SaveCurrentFilters
        Try
            Me.Response.Cookies("SearchUsersForModule")("Value") = filters.Value
            Me.Response.Cookies("SearchUsersForModule")("idProvider") = -1
            Me.Response.Cookies("SearchUsersForModule")("IdOrganization") = filters.IdOrganization
            Me.Response.Cookies("SearchUsersForModule")("IdProfileType") = filters.IdProfileType
            Me.Response.Cookies("SearchUsersForModule")("IdAgency") = filters.IdAgency.ToString
            Me.Response.Cookies("SearchUsersForModule")("SearchBy") = filters.SearchBy.ToString
            Me.Response.Cookies("SearchUsersForModule")("StartWith") = filters.StartWith

            Me.Response.Cookies("SearchUsersForModule")("Status") = filters.Status.ToString
            Me.Response.Cookies("SearchUsersForModule")("PageIndex") = filters.PageIndex
            Me.Response.Cookies("SearchUsersForModule")("PageSize") = filters.PageSize
            Me.Response.Cookies("SearchUsersForModule")("OrderBy") = filters.OrderBy
            Me.Response.Cookies("SearchUsersForModule")("Ascending") = filters.Ascending
            Me.Response.Cookies("SearchUsersForModule")("DisplayLoginInfo") = filters.DisplayLoginInfo
            Me.Response.Cookies("SearchUsersForModule")("CurrentModuleCode") = CurrentModuleCode
            Me.Response.Cookies("SearchUsersForModule")("CurrentModuleView") = CurrentModuleView
            Me.Response.Cookies("SearchUsersForModule")("CurrentModuleIdCommunity") = CurrentModuleIdCommunity

        Catch ex As Exception

        End Try
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String)) Implements IViewSearchUsersForModule.InitializeWordSelector
        Me.CTRLalphabetSelector.InitializeControl(words)
    End Sub
    Private Sub InitializeWordSelector(words As List(Of String), activeWord As String) Implements IViewSearchUsersForModule.InitializeWordSelector
        Me.CTRLalphabetSelector.InitializeControl(words, activeWord)
    End Sub
#End Region

#Region "Filters use"
    Private Sub DDLorganizations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizations.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CurrentPresenter.ChangeOrganization(SelectedIdOrganization, SelectedIdProfileType, PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLprofileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLprofileType.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CurrentPresenter.ChangeProfileType(SelectedIdOrganization, SelectedIdProfileType, PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLstatus.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        'Me.UDPprofiles.Update()
        Me.CurrentPresenter.LoadProfiles(PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub DDLagencies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLagencies.SelectedIndexChanged
        Dim PageIndex As Integer = Me.Pager.PageIndex
        If AllowAutoupdate Then
            Dim dto As dtoFilters = GetCurrentFilters

            dto.PageIndex = 0
            dto.PageSize = Me.CurrentPageSize
            PageIndex = 0
            Me.SearchFilters = dto
        End If
        Me.CurrentPresenter.LoadProfiles(PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.SearchFilters = GetCurrentFilters
        Me.DDLstatus.AutoPostBack = Me.CBXautoUpdate.Checked

        Me.DDLagencies.AutoPostBack = Me.CBXautoUpdate.Checked
        If Me.CBXautoUpdate.Checked Then
            DDLagencies.Attributes.Add("onchange", "onUpdating();")
            DDLorganizations.Attributes.Add("onchange", "onUpdating();")
            DDLpage.Attributes.Add("onchange", "onUpdating();")
            DDLprofileType.Attributes.Add("onchange", "onUpdating();")
            DDLstatus.Attributes.Add("onchange", "onUpdating();")
            CBXautoUpdate.Attributes.Add("onchange", "onUpdating();")
        Else
            DDLstatus.Attributes.Add("onchange", "")
        End If
        'Me.UDPprofiles.Update()
        Me.CurrentPresenter.LoadProfiles(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub CTRLalphabetSelector_SelectItem(letter As String) Handles CTRLalphabetSelector.SelectItem
        Dim dto As dtoFilters = GetCurrentFilters
        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        dto.StartWith = letter
        Me.SearchFilters = dto
        Me.CurrentStartWith = letter
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadProfiles(0, Me.CurrentPageSize)
    End Sub
#End Region

    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcerca.Click
        Dim dto As dtoFilters = GetCurrentFilters

        dto.PageIndex = 0
        dto.PageSize = Me.CurrentPageSize
        Me.SearchFilters = dto
        Me.CurrentPresenter.LoadProfiles(0, Me.CurrentPageSize)
    End Sub
#End Region

#Region "Grid use"
    Private Sub PGgrid_OnPageSelected() Handles PGgrid.OnPageSelected
        Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Private Sub RPTprofiles_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTprofiles.ItemCommand
        Dim idProfile As Integer = 0
        If IsNumeric(e.CommandArgument) Then
            idProfile = CInt(e.CommandArgument)
        End If
    End Sub


    Private Sub RPTprofiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTprofiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBnameSurname_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtipo_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBstatusInfo_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBstatus_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcompanyName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBagencyName_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim profile As lm.Comol.Core.Authentication.dtoBaseProfile
            Dim permission As dtoProfilePermission = Nothing
            Dim hyperlink As HyperLink = e.Item.FindControl("HYPinfo")
            Dim status As StatusProfile = StatusProfile.None
            Dim providersCount As Long = 0
            Dim oLabel As Label = e.Item.FindControl("LBcompanyName")
            Dim oLiteral As Literal
            Dim authenticationType As lm.Comol.Core.Authentication.AuthenticationProviderType = lm.Comol.Core.Authentication.AuthenticationProviderType.None
            If TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                authenticationType = item.AuthenticationType
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                oLabel.Text = DirectCast(profile, lm.Comol.Core.Authentication.dtoCompany).Info.Name
                authenticationType = item.AuthenticationType
            ElseIf TypeOf e.Item.DataItem Is dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) Then
                Dim item As dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee) = DirectCast(e.Item.DataItem, dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))
                permission = item.Permission
                profile = item.Profile
                status = item.Status
                providersCount = item.ProvidersCount
                oLabel = e.Item.FindControl("LBagencyName")
                If IsNothing(item.Profile.CurrentAgency) Then
                    oLabel.Text = ""
                Else
                    oLabel.Text = item.Profile.CurrentAgency.Value
                End If
                authenticationType = item.AuthenticationType
            End If

            Resource.setHyperLink(hyperlink, True, True)
            ' 
            If permission.Info OrElse permission.AdvancedInfo Then
                hyperlink.NavigateUrl = Me.BaseUrl & RootObject.ProfileInfo(profile.Id, profile.IdProfileType)
                'hyperlink.Attributes.Add("onClick", "OpenWin('" & Me.BaseUrl & RootObject.ProfileInfo(profile.Id, profile.IdProfileType) & "','480','560','no','yes');return false;")
            End If
            hyperlink.Visible = permission.Info OrElse permission.AdvancedInfo

            hyperlink = e.Item.FindControl("HYPmoduleAction")
            Select Case CurrentModuleCode
                Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                    Select Case CurrentModuleView
                        Case lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex.ToString
                            hyperlink.NavigateUrl = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryUser(lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex, profile.Id, 0)
                        Case lm.Comol.Modules.EduPath.Domain.SummaryType.CommunityIndex.ToString
                            hyperlink.NavigateUrl = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryUser(lm.Comol.Modules.EduPath.Domain.SummaryType.CommunityIndex, profile.Id, CurrentModuleIdCommunity)
                    End Select
                    hyperlink.Visible = Not String.IsNullOrEmpty(CurrentModuleView) AndAlso Not String.IsNullOrEmpty(hyperlink.NavigateUrl)
                    Resource.setHyperLink(hyperlink, "ModuleCode." & CurrentModuleView, False, True)
            End Select


            Dim oImage As System.Web.UI.WebControls.Image = e.Item.FindControl("IMGstatus")
            Try
                Select Case status
                    Case StatusProfile.Active
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/attivo.jpg"
                    Case StatusProfile.Disabled
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/bloccato.jpg"
                    Case StatusProfile.Waiting
                        oImage.ImageUrl = Me.BaseUrl & "Images/Grid/inattesa.jpg"
                End Select
                oImage.ToolTip = Resource.getValue("StatusProfile." & status.ToString)
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoCompany))) Implements IViewSearchUsersForModule.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoBaseProfile))) Implements IViewSearchUsersForModule.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoExternal))) Implements IViewSearchUsersForModule.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub
 
    Private Sub LoadProfiles(items As List(Of dtoProfileItem(Of lm.Comol.Core.Authentication.dtoEmployee))) Implements IViewSearchUsersForModule.LoadProfiles
        If items.Count = 0 Then
            Me.RPTprofiles.Visible = False
            Me.DIVpageSize.Visible = False
            Me.PGgrid.Visible = False
        Else
            Me.RPTprofiles.DataSource = items
            Me.RPTprofiles.DataBind()
            Me.RPTprofiles.Visible = True
        End If
        'Me.UDPprofiles.Update()
    End Sub

    Private Sub DDLpage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.LoadProfiles(Me.PGgrid.Pager.PageIndex, Me.CurrentPageSize)
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewSearchUsersForModule.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.SearchUsersForModule(PreLoadedModuleCode, PreloadedReloadFilters)
        webPost.Redirect(dto)
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub

End Class