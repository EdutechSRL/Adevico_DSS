Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Partial Public Class WorkbookStatusEdit
    Inherits PageBase
    Implements IWKstatusEdit


#Region "Iview"
    Private _ModulePermission As ModuleWorkBookManagement
    Public Property AllowSaveStatus() As Boolean Implements IWKstatusEdit.AllowSaveStatus
        Get
            Return Me.LNBsave.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.LNBsave.Visible = value
        End Set
    End Property
    Public ReadOnly Property ModulePermission() As ModuleWorkBookManagement Implements IWKstatusEdit.ModulePermission
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
    Public ReadOnly Property PreLoadedStatusID() As Integer Implements IWKstatusEdit.PreLoadedStatusID
        Get
            If IsNumeric(Me.Request.QueryString("StatusID")) Then
                Return CInt(Me.Request.QueryString("StatusID"))
            Else
                Return 0
            End If
        End Get
    End Property

#End Region

#Region "Base"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As WKstatusEditPresenter
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As WKstatusEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New WKstatusEditPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Base"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.HYPreturnToList.NavigateUrl = Me.BaseUrl & "Generici/WorkBookStatusManagement.aspx"
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
            Me.Master.ServiceTitle = .getValue("serviceTitleEdit")
            Me.Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(Me.HYPreturnToList, True, True)
            .setLabel(Me.LBavailableFor_t)
            .setLabel(Me.LBdefaultInfo)
            .setLabel(LBdefault_t)
            .setLabel(Me.LBnoUpdate)
            .setLabel(Me.LBstatusNameFor_t)
            .setLinkButton(Me.LNBsave, True, True)
            .setCheckBox(Me.CBXdefault)
            .setCheckBoxList(Me.CBLpermission, EditingPermission.Owner)
            .setCheckBoxList(Me.CBLpermission, EditingPermission.Authors)
            .setCheckBoxList(Me.CBLpermission, EditingPermission.ModuleManager)
            .setCheckBoxList(Me.CBLpermission, EditingPermission.Responsible)
        End With
    End Sub
    Public Sub NoPermissionToAccessPage() Implements IWKstatusEdit.NoPermissionToAccessPage

    End Sub

#End Region

    Public Sub LoadStatus(ByVal oStatus As dtoWorkBookStatus, ByVal oList As List(Of dtoWorkBookStatusTranslation)) Implements IWKstatusEdit.LoadStatus
        Me.CBXdefault.Checked = oStatus.isDefault
        Me.CBLpermission.SelectedIndex = -1
        If oStatus.AvailableForPermission And EditingPermission.Owner Then
            Me.CBLpermission.Items.FindByValue(EditingPermission.Owner).Selected = True
        End If
        If oStatus.AvailableForPermission And EditingPermission.Responsible Then
            Me.CBLpermission.Items.FindByValue(EditingPermission.Responsible).Selected = True
        End If
        If oStatus.AvailableForPermission And EditingPermission.ModuleManager Then
            Me.CBLpermission.Items.FindByValue(EditingPermission.ModuleManager).Selected = True
        End If
        If oStatus.AvailableForPermission And EditingPermission.Authors Then
            Me.CBLpermission.Items.FindByValue(EditingPermission.Authors).Selected = True
        End If
        Me.RPTstatus.DataSource = oList
        Me.RPTstatus.DataBind()
    End Sub

    Public Sub ShowEditView() Implements IWKstatusEdit.ShowEditView
        Me.MLVworkbooks.SetActiveView(Me.VIWlist)
    End Sub

    Public Sub ShowNoStatusView() Implements IWKstatusEdit.ShowNoStatusView
        Me.MLVworkbooks.SetActiveView(Me.VIWerrors)
    End Sub


    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)
    
    End Sub

    Private Sub RPTstatus_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTstatus.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal
            oLiteral = e.Item.FindControl("LTheaderLanguage")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTheaderTranslation")
            Me.Resource.setLiteral(oLiteral)
        End If
    End Sub

    Private Sub LNBsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsave.Click
        Dim oStatus As New dtoWorkBookStatus
        oStatus.ID = Me.PreLoadedStatusID
        oStatus.isDefault = Me.CBXdefault.Checked

        If Me.CBLpermission.Items.FindByValue(EditingPermission.Owner).Selected Then
            oStatus.AvailableForPermission = oStatus.AvailableForPermission Or EditingPermission.Owner
        End If
        If Me.CBLpermission.Items.FindByValue(EditingPermission.Responsible).Selected Then
            oStatus.AvailableForPermission = oStatus.AvailableForPermission Or EditingPermission.Responsible
        End If
        If Me.CBLpermission.Items.FindByValue(EditingPermission.ModuleManager).Selected Then
            oStatus.AvailableForPermission = oStatus.AvailableForPermission Or EditingPermission.ModuleManager
        End If
        If Me.CBLpermission.Items.FindByValue(EditingPermission.Authors).Selected Then
            oStatus.AvailableForPermission = oStatus.AvailableForPermission Or EditingPermission.Authors
        End If

        Dim oList As New List(Of dtoWorkBookStatusTranslation)
        For Each oRow As RepeaterItem In (From r As RepeaterItem In Me.RPTstatus.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim oDto As New dtoWorkBookStatusTranslation
            Dim oLiteral As Literal
            oLiteral = oRow.FindControl("LTlanguageID")
            oDto.LanguageID = CInt(oLiteral.Text)
            oDto.StatusId = Me.PreLoadedStatusID
            oLiteral = oRow.FindControl("LTid")
            oDto.UniqueID = CInt(oLiteral.Text)
            Dim oTextBox As TextBox = oRow.FindControl("TXBstatusName")
            oDto.Translation = oTextBox.Text
            oList.Add(oDto)
        Next
        Me.CurrentPresenter.SaveStatus(oStatus, oList)
    End Sub

    Public Sub LoadStatusList() Implements lm.Comol.Modules.Base.Presentation.IWKstatusEdit.LoadStatusList
        Me.PageUtility.RedirectToUrl("Generici/WorkbookStatusManagement.aspx")
    End Sub
End Class