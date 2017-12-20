Imports lm.Modules.NotificationSystem.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports lm.ActionDataContract

Partial Public Class ManageNotificationTemplates
    Inherits PageBase
    Implements ITemplateManagementView

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As TemplateManagementPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_NotificationManagement
    Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleNotificationManagement))
    Private _BaseUrl As String
    Private _PagingUrl As String
#End Region

#Region "Base Context"
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As TemplateManagementPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TemplateManagementPresenter(Me.CurrentContext, Me)
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
    Private ReadOnly Property CurrentService() As Services_NotificationManagement
        Get
            If IsNothing(_Servizio) Then
                If isPortalCommunity Then
                    Dim PersonTypeID As Integer = Me.TipoPersonaID
                    _Servizio = Services_NotificationManagement.Create
                    _Servizio.AddTemplate = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                    _Servizio.Administration = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                    _Servizio.ManagementPermission = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
                    _Servizio.EditTemplate = (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin OrElse PersonTypeID = Main.TipoPersonaStandard.Amministrativo)
                ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_NotificationManagement(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_NotificationManagement.Codex))
                Else
                    _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_NotificationManagement.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = Services_NotificationManagement.Create
                    End If
                End If
            End If
            Return _Servizio
        End Get
    End Property
    Public ReadOnly Property ModulePermission() As ModuleNotificationManagement Implements ITemplateManagementView.ModulePermission
        Get
            Return New ModuleNotificationManagement(Me.CurrentService)
        End Get
    End Property
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Modules.NotificationSystem.Domain.ModuleNotificationManagement) Implements lm.Modules.NotificationSystem.Presentation.ITemplateManagementView.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                Dim oList As New List(Of ModuleCommunityPermission(Of ModuleNotificationManagement))
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_OnLineUsers.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New ModuleCommunityPermission(Of ModuleNotificationManagement)() With {.ID = oPermission.CommunityID, .Permissions = New ModuleNotificationManagement(New Services_NotificationManagement(oPermission.PermissionString))})
                Next
                _CommunitiesPermission = oList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
#End Region
#Region "View"
    Public ReadOnly Property ActionID() As Integer Implements ITemplateManagementView.ActionID
        Get
            If Me.DDLmoduleAction.Items.Count = 0 Then
                Return 0
            Else
                Return CInt(Me.DDLmoduleAction.SelectedValue)
            End If
        End Get
    End Property
    Public WriteOnly Property AllowUpdate() As Boolean Implements ITemplateManagementView.AllowUpdate
        Set(ByVal value As Boolean)
            Me.BTNsave.Enabled = value
        End Set
    End Property
    Public ReadOnly Property TemplateTypeID() As Integer Implements ITemplateManagementView.TemplateTypeID
        Get
            Return CInt(Me.DDLtemplateType.SelectedValue)
        End Get
    End Property
    Public ReadOnly Property ModuleID() As Integer Implements ITemplateManagementView.ModuleID
        Get
            If Me.DDLmodule.Items.Count = 0 Then
                Return 0
            Else
                Return CInt(Me.DDLmodule.SelectedValue)
            End If
        End Get
    End Property
    Public ReadOnly Property ActionName() As String Implements lm.Modules.NotificationSystem.Presentation.ITemplateManagementView.ActionName
        Get
            If Me.DDLmoduleAction.Items.Count > 0 Then
                Return Me.DDLmoduleAction.SelectedItem.Text
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region "Defaults"
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

#Region "Default sub"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
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
        MyBase.SetCulture("pg_UsageGlobal", "Statistiche")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


    Public Sub LoadAction(ByVal actions As List(Of Element(Of Integer))) Implements ITemplateManagementView.LoadAction
        Me.DDLmoduleAction.DataSource = actions
        Me.DDLmoduleAction.DataTextField = "Name"
        Me.DDLmoduleAction.DataValueField = "ID"
        Me.DDLmoduleAction.DataBind()
        Me.DDLmoduleAction.Enabled = (actions.Count > 0)
    End Sub

    Public Sub LoadMessages(ByVal messages As List(Of TranslatedMessage)) Implements ITemplateManagementView.LoadMessages
        Me.RPTtemplates.DataSource = messages
        Me.RPTtemplates.DataBind()
    End Sub

    Public Sub LoadModules(ByVal modules As List(Of Element(Of Integer))) Implements ITemplateManagementView.LoadModules
        Me.DDLmodule.DataSource = modules
        Me.DDLmodule.DataTextField = "Name"
        Me.DDLmodule.DataValueField = "ID"
        Me.DDLmodule.DataBind()
        Me.DDLmodule.Enabled = (modules.Count > 0)
    End Sub



    Public Sub NoPermissionToAccess() Implements ITemplateManagementView.NoPermissionToAccess
        Me.BindNoPermessi()
    End Sub

    Public Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer) Implements ITemplateManagementView.AddActionNoPermission
        Me.PageUtility.AddAction(Services_NotificationManagement.ActionType.NoPermission, CreateObjectList(CommunityID, PersonID), InteractionType.Generic)
    End Sub

    Private Function CreateObjectList(ByVal CommunityID As Integer, ByVal PersonID As Integer) As List(Of WS_Actions.ObjectAction)
        Dim oList As New List(Of WS_Actions.ObjectAction)
        oList = Me.PageUtility.CreateObjectsList(Services_UserAccessReports.ObjectType.Community, CommunityID.ToString)
        If PersonID > 0 Then
            oList.Add(Me.PageUtility.CreateObjectAction(Services_UserAccessReports.ObjectType.Person, PersonID.ToString))
        End If
        Return oList
    End Function


    Private Sub DDLmodule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmodule.SelectedIndexChanged
        Me.CurrentPresenter.LoadActions()
    End Sub

    Private Sub DDLmoduleAction_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmoduleAction.SelectedIndexChanged
        Me.CurrentPresenter.LoadMessages()
    End Sub

    Private Sub DDLtemplateType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLtemplateType.SelectedIndexChanged
        Me.CurrentPresenter.LoadMessages()
    End Sub

    Public Function GetTranslatedMessages() As List(Of TranslatedMessage) Implements lm.Modules.NotificationSystem.Presentation.ITemplateManagementView.GetTranslatedMessages
        Dim Messages As New List(Of TranslatedMessage)
        For Each row As RepeaterItem In RPTtemplates.Items
            Dim oLiteralID As Literal = row.FindControl("LTid")
            Dim oLiteralLanguageID As Literal = row.FindControl("LTlanguageID")
            Dim oTexbox As TextBox = row.FindControl("TXBmessage")
            Dim oTemplateName As TextBox = row.FindControl("TXBtemplate")
            If Not (IsNothing(oLiteralID) AndAlso IsNothing(oLiteralLanguageID) AndAlso IsNothing(oTexbox) AndAlso IsNothing(oTemplateName)) Then
                Messages.Add(New TranslatedMessage() With {.ID = CInt(oLiteralID.Text), .LanguageID = CInt(oLiteralLanguageID.Text), .Message = oTexbox.Text, .TemplateName = oTemplateName.Text})
            End If
        Next
        Return Messages
    End Function

    Private Sub BTNsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsave.Click
        Me.CurrentPresenter.SaveMessage()
    End Sub


    Public Sub UpdateMessageID(ByVal TemplateID As Long, ByVal RowNumber As Integer) Implements lm.Modules.NotificationSystem.Presentation.ITemplateManagementView.UpdateMessageID
        Dim row As RepeaterItem = RPTtemplates.Items(RowNumber)
        If Not IsNothing(row) Then
            Dim oLiteralID As Literal = row.FindControl("LTid")
            If Not IsNothing(oLiteralID) Then
                oLiteralID.Text = TemplateID
            End If
        End If
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_CommunityNews.Codex)
    End Sub
End Class