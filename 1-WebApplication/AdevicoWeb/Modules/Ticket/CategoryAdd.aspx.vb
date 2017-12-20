Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class CategoryAdd
    Inherits TicketBase 'PageBase
    Implements TK.Presentation.View.iViewCategoryAdd

#Region "Context"
    'Definizion Presenter...
    Private _presenter As TK.Presentation.CategoryAddPresenter
    Private ReadOnly Property CurrentPresenter() As TK.Presentation.CategoryAddPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TK.Presentation.CategoryAddPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Internal"
    'Property interne

    Public ReadOnly Property InternalUserAddTitle As String
        Get
            Return Resource.getValue("InternalUserAdd.title")
        End Get
    End Property

    Public ReadOnly Property UsersDialogTitle As String
        Get
            Return Resource.getValue("Modal.InternalUsers.Title")
        End Get
    End Property

    ''' <summary>
    ''' Manager selezionato
    ''' </summary>
    ''' <value>Id PERSON dell'utente selezioanto: -1 = utente corrente</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property SelectedManagerId As Integer
        Get
            Dim Id As Integer = -1
            Try
                Id = System.Convert.ToInt32(Me.ViewState("ManagerId"))
            Catch ex As Exception

            End Try
            Return Id
        End Get
        Set(value As Integer)
            Me.ViewState("ManagerId") = value
        End Set
    End Property

#End Region

#Region "Implements"
    'Property della VIEW
    Public Property Description As String Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategoryAdd.Description
        Get
            Return Me.TXBdescC.Text
        End Get
        Set(value As String)
            Me.TXBdescC.Text = value
        End Set
    End Property

    Public Property Name As String Implements TK.Presentation.View.iViewCategoryAdd.Name
        Get
            Return Me.TXBnameC.Text
        End Get
        Set(value As String)
            Me.TXBnameC.Text = value
        End Set
    End Property

    Public Property Type As TK.Domain.Enums.CategoryType Implements TK.Presentation.View.iViewCategoryAdd.Type
        Get
            Dim CurrentType As Int32 = System.Convert.ToInt32(TK.Domain.Enums.CategoryType.Current)

            Try
                CurrentType = System.Convert.ToInt32(Me.DDLtype.SelectedValue)
            Catch ex As Exception
            End Try

            Return CType(CurrentType, TK.Domain.Enums.CategoryType)

        End Get

        Set(value As TK.Domain.Enums.CategoryType)
            Try
                Me.DDLtype.SelectedValue = System.Convert.ToInt32(value)
            Catch ex As Exception
            End Try
        End Set
    End Property

    Public Property ViewCommunityId As Integer Implements lm.Comol.Core.BaseModules.Tickets.Presentation.View.iViewCategoryAdd.ViewCommunityId
        Get

        End Get
        Set(value As Integer)

        End Set
    End Property

    Public ReadOnly Property SelectedManagerID_imp As Integer Implements TK.Presentation.View.iViewCategoryAdd.SelectedManagerID
        Get
            Return Me.SelectedManagerId
        End Get
    End Property

#End Region

#Region "Inherits"
    'Property del PageBase

#Region "TicketBase"

    Public Overrides Sub DisplaySessionTimeout(CommunityId As Integer)
        RedirectOnSessionTimeOut(TK.Domain.RootObject.CategoryAdd(CommunityId), CommunityId)
    End Sub

    Public Overrides Sub ShowNoAccess()
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

#End Region

#End Region


    Private Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Me.Master.ShowDocType = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

        End If
    End Sub

#Region "Inherits"
    'Sub e Function PageBase

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CategoryAdd", "Modules", "Ticket")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTpageTitle_t)
            .setLiteral(LTtitle_t)

            .setLabel(LBnameC_t)
            .setLabel(LBdescC_t)
            .setLabel(LBtype_t)
            .setLabel(LBcurUser_t)

            .setLinkButton(LNBadd, True, True)
            .setLinkButton(LNBtoList, True, True)

            .setLinkButton(LNBchangeUser, True, True)
            .setLinkButton(LNBuserSetCurrent, True, True)

        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

#End Region

#Region "Implements"
    'Sub e function della View
    Public Sub NavigateToEdit(NewCategoryId As Long) Implements TK.Presentation.View.iViewCategoryAdd.NavigateToEdit
        Response.Redirect("Category.aspx?Id=" & NewCategoryId.ToString())
    End Sub

    Public Sub Initialize(ByVal CatTypePermission As TK.Domain.DTO.DTO_CategoryTypeComPermission) Implements TK.Presentation.View.iViewCategoryAdd.Initialize

        Me.InitDDLType(CatTypePermission)

        If Not CatTypePermission.CanCreate Then
            Exit Sub
        End If

        Dim selectedIds As New List(Of Integer)

        Dim ComId As Integer = CurrentPresenter.CurrentCommunityId

        If (ComId > 0) Then
            Me.CTRLselectUsers.InitializeControl( _
            lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, _
            False, ComId, selectedIds, Nothing, _
            Resource.getValue("Modal.InternalUsers.Description"))
        End If

    End Sub

    Public Sub ShowNoPermission() Implements TK.Presentation.View.iViewCategoryAdd.ShowNoPermission
        Me.Master.ShowNoPermission = True
        Me.Master.ServiceNopermission = Resource.getValue("Error.NoAccess")
    End Sub

    Public Sub SendAction( _
                        Action As TK.ModuleTicket.ActionType, _
                        idCommunity As Integer, _
                        Type As TK.ModuleTicket.InteractionType, _
                       Optional Objects As System.Collections.Generic.IList(Of System.Collections.Generic.KeyValuePair(Of Integer, String)) = Nothing) _
                    Implements TK.Presentation.View.iViewCategoryAdd.SendAction

        Dim oList As List(Of WS_Actions.ObjectAction) = Nothing

        If Not IsNothing(Objects) Then
            oList = (From kvp As KeyValuePair(Of Integer, String) In Objects
                    Select Me.PageUtility.CreateObjectAction(kvp.Key, kvp.Value)).ToList()
        End If

        Me.PageUtility.AddActionToModule(idCommunity, Me.CurrentModuleID, Action, oList, Type)
    End Sub

    Public Sub UpdateUserName(Name As String, IsCurrent As Boolean) Implements TK.Presentation.View.iViewCategoryAdd.UpdateUserName
        Me.LBmanager.Text = Name

        Me.LNBuserSetCurrent.Visible = Not IsCurrent

        Dim CssClass As String = "fieldlabel"

        If IsCurrent Then
            CssClass &= " current"
        End If

        LBcurUser_t.CssClass = CssClass
    End Sub

#End Region

#Region "Internal"
    'Sub e Function "della pagina"
    Private Sub InitDDLType(ByVal Permission As TK.Domain.DTO.DTO_CategoryTypeComPermission)

        If Not Permission.CanCreate Then
            Me.ShowNoPermission()
            Exit Sub
        End If

        Me.DDLtype.Items.Clear()

        Dim HasNoSelection As Boolean = True

        If Permission.CanPrivate Then
            Me.DDLtype.Items.Add(GetLiType(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType.Current, HasNoSelection))
            HasNoSelection = False
        End If

        If Permission.CanPublic Then
            Me.DDLtype.Items.Add(GetLiType(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType.Public, HasNoSelection))
            HasNoSelection = False
        End If

        If Permission.CanTicket Then
            Me.DDLtype.Items.Add(GetLiType(lm.Comol.Core.BaseModules.Tickets.Domain.Enums.CategoryType.Ticket, HasNoSelection))
            HasNoSelection = False
        End If

        If (DDLtype.Items.Count() <= 1) Then
            DDLtype.Enabled = False
        End If

    End Sub

    Private Function GetLiType(ByVal Type As TK.Domain.Enums.CategoryType, ByVal IsSelcted As Boolean) As ListItem
        Dim item As New ListItem(Resource.getValue("DDLtype." & Type.ToString()), System.Convert.ToInt32(Type))
        item.Selected = IsSelcted
        Return item
    End Function



#End Region

#Region "Handler"
    'Gestione eventi oggetti pagina (Repeater.ItemDataBound(), Button.Click, ...)
    Private Sub LNBadd_Click(sender As Object, e As System.EventArgs) Handles LNBadd.Click
        If String.IsNullOrEmpty(Me.TXBnameC.Text) Then
            Me.LTnameError.Visible = True
            Return
        End If
        Me.LTnameError.Visible = False
        Me.CurrentPresenter.Create()
    End Sub

    Private Sub LNBtoList_Click(sender As Object, e As System.EventArgs) Handles LNBtoList.Click
        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.CategoryList(Me.ViewCommunityId))
    End Sub

    Private Sub LNBchangeUser_Click(sender As Object, e As EventArgs) Handles LNBchangeUser.Click
        Me.DVselectUsr.Visible = True
    End Sub

    Private Sub LNBuserSetCurrent_Click(sender As Object, e As EventArgs) Handles LNBuserSetCurrent.Click
        Me.DVselectUsr.Visible = False
        Me.SelectedManagerId = -1
        Me.CurrentPresenter.UpdateManager()
    End Sub

    Private Sub CTRLselectUsers_CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DVselectUsr.Visible = False
    End Sub


    Private Sub CTRLselectUsers_UserSelected(idUser As Integer) Handles CTRLselectUsers.UserSelected
        Me.SelectedManagerId = idUser
        Me.DVselectUsr.Visible = False
        Me.CurrentPresenter.UpdateManager()
    End Sub

#End Region

End Class