Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Partial Public Class WorkBookStatusManagement
    Inherits PageBase
    Implements IWKstatusManagement

#Region "View Property"
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
    Private _Presenter As WKstatusManagementPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _ModulePermission As ModuleWorkBookManagement
    Private _BaseUrl As String

    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property ModulePermission() As ModuleWorkBookManagement Implements IWKstatusManagement.ModulePermission
        Get
            If IsNothing(_ModulePermission) Then
                _ModulePermission = New ModuleWorkBookManagement
                With _ModulePermission
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    .AddStatus = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                    .ListStatus = .AddStatus
                    .ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                    .EditStatus = .AddStatus
                    .DeleteStatus = .AddStatus
                    .Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                End With
            End If
            Return _ModulePermission
        End Get
    End Property

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As WKstatusManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WKstatusManagementPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Protected Function BackGroundItem(ByVal oItem As lm.Comol.Modules.Base.DomainModel.WorkBookItem) As String
        If oItem.isDeleted Then
            Return "ROW_Disabilitate_Small"
        Else
            Return ""
        End If
    End Function
    Public Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase Implements IWKstatusManagement.CurrentPager
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
            Me.DIVpageSize.Visible = (Not value.Count < Me.DDLpage.Items(0).Value)
        End Set
    End Property
    Public Property CurrentPageSize() As Integer Implements IWKstatusManagement.CurrentPageSize
        Get
            Return Me.DDLpage.SelectedValue
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(Me.DDLpage.Items.FindByValue(value)) Then
                Me.DDLpage.SelectedValue = value
            End If
        End Set
    End Property

    Public ReadOnly Property PreLoadedPageIndex() As Integer Implements IWKstatusManagement.PreLoadedPageIndex
        Get
            If IsNumeric(Request.QueryString("Page")) Then
                Return CInt(Request.QueryString("Page"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreLoadedPageSize() As Integer Implements IWKstatusManagement.PreLoadedPageSize
        Get
            If IsNumeric(Request.QueryString("PageSize")) Then
                Return CInt(Request.QueryString("PageSize"))
            Else
                Return Me.DDLpage.Items(0).Value
            End If
        End Get
    End Property
    Public ReadOnly Property DefaultPageSize() As Integer Implements IWKstatusManagement.DefaultPageSize
        Get
            Return 25
        End Get
    End Property
    Public Property AllowCreateStatus() As Boolean Implements IWKstatusManagement.AllowCreateStatus
        Get
            Return Me.HYPaddStatus.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.HYPaddStatus.Visible = value
            Me.HYPaddStatus.NavigateUrl = IIf(value, Me.BaseUrl & "Generici/WorkbookStatusEdit.aspx", "#")
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Function BackGroundItem(ByVal oItem As NEWdtoWorkbooks) As String
        If oItem.isDeleted Then
            Return "ROW_Disabilitate_Small"
        Else
            Return ""
        End If
    End Function
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgrid.Pager = Me.CurrentPager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
            Me.CurrentPresenter.InitView()
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
        Me.PageUtility.AddAction(Services_WorkBook.ActionType.NoPermission, Nothing, InteractionType.Generic)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookStatusManagement", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitle")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(Me.HYPaddStatus, True, True)
            .setLabel(Me.LBerrors)
            .setLabel(Me.LBpagesize)
            .setLabel(LBdefault_t)
            .setLabel(Me.LBdefaultInfo)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
    Public Sub NoPermissionToAccessPage() Implements IWKstatusManagement.NoPermissionToAccessPage
        Me.BindNoPermessi()
    End Sub

    Public Sub ShowListView() Implements IWKstatusManagement.ShowListView
        Me.MLVworkbooks.SetActiveView(VIWlist)
    End Sub

    Public Sub ShowNoStatusView() Implements IWKstatusManagement.ShowNoStatusView
        Me.MLVworkbooks.SetActiveView(VIWerrors)
    End Sub
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub
#End Region


    Public Sub LoadStatus(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Modules.Base.DomainModel.dtoWorkBookStatus)) Implements IWKstatusManagement.LoadStatus
        Me.RPTworkbookStatus.DataSource = oList
        Me.RPTworkbookStatus.DataBind()
    End Sub

    Public Sub NavigationUrl() Implements IWKstatusManagement.NavigationUrl
        Me.PGgrid.BaseNavigateUrl = Me.GetBaseListUrl() & "&Page={0}"
    End Sub

    Private Function GetBaseListUrl(Optional ByVal WithBaseUrl As Boolean = True) As String
        Dim url As String = "?"
        url &= "&PageSize=" & Me.CurrentPageSize.ToString

        If url.StartsWith("?&") Then
            url = url.Replace("?&", "?")
        End If
        url = "Generici/WorkBookStatusManagement.aspx" & url
        If WithBaseUrl Then
            url = Me.BaseUrl & url
        End If
        Return url
    End Function

    Private Sub RPTworkbookStatus_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTworkbookStatus.ItemCommand
        Dim StatusID As Integer
        Try
            StatusID = CInt(e.CommandArgument)
            If e.CommandName = "confirmDelete" Then
                Me.CurrentPresenter.DeleteStatus(StatusID)
            Else
                Me.CurrentPresenter.LoadStatus()
            End If
        Catch ex As Exception
            Me.CurrentPresenter.LoadStatus()
        End Try
    End Sub

    Private Sub RPTworkbookStatus_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTworkbookStatus.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label
            oLabel = e.Item.FindControl("LBaction_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBedit_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBname_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcount_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDtoStatus As dtoWorkBookStatus = TryCast(e.Item.DataItem, dtoWorkBookStatus)
            If Not IsNothing(oDtoStatus) Then
                Dim oPermission As ModuleWorkBookManagement = Me.ModulePermission

                Dim isEditable As Boolean = oPermission.EditStatus OrElse oPermission.Administration
                Dim isDeletable As Boolean = (oPermission.Administration OrElse oPermission.DeleteStatus) AndAlso oDtoStatus.ItemsCount = 0 AndAlso oDtoStatus.WorkbookCount = 0 AndAlso oDtoStatus.isDefault = False


                Dim oLNBdelete As LinkButton
                oLNBdelete = e.Item.FindControl("LNBdelete")

                If Not IsNothing(oLNBdelete) Then
                    oLNBdelete.Visible = isDeletable
                    Me.Resource.setLinkButton(oLNBdelete, True, True, , True)
                    oLNBdelete.Text = String.Format(oLNBdelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBdelete.ToolTip)
                End If
                oLNBdelete.CommandArgument = oDtoStatus.ID

                Dim oHYPedit As HyperLink = e.Item.FindControl("HYPedit")
                If Not IsNothing(oHYPedit) Then
                    Me.Resource.setHyperLink(oHYPedit, True, True)
                    oHYPedit.Text = String.Format(oHYPedit.Text, Me.BaseUrl & "Images/grid/modifica.gif", oHYPedit.ToolTip)
                    oHYPedit.Visible = isEditable
                    oHYPedit.NavigateUrl = Me.BaseUrl & "generici/WorkbookStatusEdit.aspx?StatusId=" & oDtoStatus.ID.ToString
                End If

                Dim oLabel As Label = e.Item.FindControl("LBcount")
                Me.Resource.setLabel(oLabel)
                oLabel.Text = String.Format(oLabel.Text, oDtoStatus.ItemsCount, oDtoStatus.WorkbookCount)

                'oLabel = e.Item.FindControl("LBstatusname_t")
                'Me.Resource.setLabel(oLabel)
                oLabel = e.Item.FindControl("LBvisible_t")
                Me.Resource.setLabel(oLabel)
                oLabel = e.Item.FindControl("LBvisibleTo")
                If oDtoStatus.AvailableForPermission And EditingPermission.ModuleManager > 0 Then
                    oLabel.Text = Resource.getValue("EditingPermission." & EditingPermission.ModuleManager)
                End If
                If oDtoStatus.AvailableForPermission And EditingPermission.Responsible > 0 Then
                    If Not String.IsNullOrEmpty(oLabel.Text) Then
                        oLabel.Text &= ", "
                    End If
                    oLabel.Text &= Resource.getValue("EditingPermission." & EditingPermission.Responsible)
                End If
                If oDtoStatus.AvailableForPermission And EditingPermission.Authors > 0 Then
                    If Not String.IsNullOrEmpty(oLabel.Text) Then
                        oLabel.Text &= ", "
                    End If
                    oLabel.Text &= Resource.getValue("EditingPermission." & EditingPermission.Authors)
                End If
                If oDtoStatus.AvailableForPermission And EditingPermission.Owner > 0 Then
                    If Not String.IsNullOrEmpty(oLabel.Text) Then
                        oLabel.Text &= ", "
                    End If
                    oLabel.Text &= Resource.getValue("EditingPermission." & EditingPermission.Owner)
                End If
            End If
        End If
    End Sub

    Public WriteOnly Property DefaultStatus() As String Implements lm.Comol.Modules.Base.Presentation.IWKstatusManagement.DefaultStatus
        Set(ByVal value As String)
            Me.LBdefault_t.Visible = Not String.IsNullOrEmpty(value)
            Me.LBdefault.Visible = Me.LBdefault_t.Visible
            If value <> "" Then
                Me.Resource.setLabel(Me.LBdefault)
                Me.LBdefault.Text = String.Format(Me.LBdefault.Text, value)
            End If
        End Set
    End Property

    Private Sub DDLpage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLpage.SelectedIndexChanged
        Me.CurrentPresenter.LoadStatus()
    End Sub
End Class