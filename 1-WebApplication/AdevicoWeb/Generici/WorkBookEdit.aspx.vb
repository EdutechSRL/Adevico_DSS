Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract

Partial Public Class WorkBookEdit
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit

#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As lm.Comol.Modules.Base.Presentation.WorkBookEditPresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Servizio As Services_WorkBook
    Private _BaseUrl As String
    Private _CommunitiesPermission As IList(Of WorkBookCommunityPermission)
#End Region
#Region "Public Property"
    Private ReadOnly Property CurrentService() As Services_WorkBook
        Get
            If IsNothing(_Servizio) Then
                If Me.WorkBookCommunityID = 0 Then
                    _Servizio = Services_WorkBook.Create
                    _Servizio.AddItemsToOther = False
                    _Servizio.Admin = False
                    _Servizio.ChangeApprovationStatus = False
                    _Servizio.ChangeOtherWorkbook = False
                    _Servizio.CreateGroupWorkbook = True
                    _Servizio.CreateWorkBook = True
                    _Servizio.DeleteItemsFromOther = False
                    _Servizio.DownLoadItemFiles = True
                    _Servizio.ListOtherWorkBook = False
                    _Servizio.PrintOtherWorkBook = False
                    _Servizio.ReadOtherWorkBook = False
                ElseIf Not Me.isPortalCommunity Then
                    If Me.WorkBookCommunityID = Me.ComunitaCorrenteID Then
                        _Servizio = Me.PageUtility.GetCurrentServices.Find(Services_WorkBook.Codex)
                        If IsNothing(_Servizio) Then
                            _Servizio = Services_WorkBook.Create
                        End If
                    ElseIf Me.AmministrazioneComunitaID = Me.WorkBookCommunityID Then
                        _Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_WorkBook.Codex))
                    Else
                        _Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByPersona(Me.CurrentContext.UserContext.CurrentUser.Id, Me.WorkBookCommunityID, Services_WorkBook.Codex))
                    End If
                Else
                    _Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByPersona(Me.CurrentContext.UserContext.CurrentUser.Id, Me.WorkBookCommunityID, Services_WorkBook.Codex))
                End If
            End If

            'If IsNothing(_Servizio) Then
            '	If isPortalCommunity And DiaryCommunityID > 0 Then
            '		_Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByPersona(Me.CurrentContext.UserContext.CurrentUser.Id, Me.DiaryCommunityID, Services_WorkBook.Codex))
            '	ElseIf Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
            '		_Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_WorkBook.Codex))
            '	ElseIf DiaryCommunityID > 0 And DiaryCommunityID <> Me.ComunitaCorrenteID Then
            '		_Servizio = New Services_WorkBook(COL_Comunita.GetPermessiForServizioByPersona(Me.CurrentContext.UserContext.CurrentUser.Id, Me.DiaryCommunityID, Services_WorkBook.Codex))
            '	Else
            '		_Servizio = Me.PageUtility.GetCurrentServices.Find(Services_WorkBook.Codex)
            '	End If
            'End If
            Return _Servizio
        End Get
    End Property

    Public ReadOnly Property Permission() As lm.Comol.Modules.Base.DomainModel.ModuleWorkBook Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.ModulePermission
        Get
            Dim oModulePermission As New ModuleWorkBook
            With Me.CurrentService
                oModulePermission.AddItemsToOther = .AddItemsToOther
                oModulePermission.Administration = .Admin
                oModulePermission.ChangeApprovation = .ChangeApprovationStatus
                oModulePermission.ChangeOtherWorkBook = .ChangeOtherWorkbook
                oModulePermission.CreateGroupWorkBook = .CreateGroupWorkbook
                oModulePermission.CreateWorkBook = .CreateWorkBook
                oModulePermission.DeleteItemsFromOther = .DeleteItemsFromOther
                oModulePermission.DownLoadItemFiles = .DownLoadItemFiles
                oModulePermission.ListOtherWorkBooks = .ListOtherWorkBook
                oModulePermission.ManagementPermission = .GrantPermission
                oModulePermission.PrintOtherWorkBook = .PrintOtherWorkBook
                oModulePermission.ReadOtherWorkBook = .ReadOtherWorkBook
            End With
            Return oModulePermission
        End Get
    End Property
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Modules.Base.Presentation.WorkBookCommunityPermission) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.CommunitiesPermission
        Get
            If IsNothing(_CommunitiesPermission) Then
                Dim oList As New List(Of lm.Comol.Modules.Base.Presentation.WorkBookCommunityPermission)
                Dim PermissionsList As IList(Of ServiceBase) = ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_WorkBook.Codex)

                For Each oPermission As ServiceBase In PermissionsList
                    oList.Add(New WorkBookCommunityPermission() With {.ID = oPermission.CommunityID, .Permissions = TranslateComolPermissionToModulePermission(New Services_WorkBook(oPermission.PermissionString))})
                Next
                _CommunitiesPermission = oList
            End If
            Return _CommunitiesPermission
        End Get
    End Property
    Private Function TranslateComolPermissionToModulePermission(ByVal oService As Services_WorkBook) As lm.Comol.Modules.Base.DomainModel.ModuleWorkBook
        Dim oModulePermission As New ModuleWorkBook
        With oService
            oModulePermission.AddItemsToOther = .AddItemsToOther
            oModulePermission.Administration = .Admin
            oModulePermission.ChangeApprovation = .ChangeApprovationStatus
            oModulePermission.ChangeOtherWorkBook = .ChangeOtherWorkbook
            oModulePermission.CreateGroupWorkBook = .CreateGroupWorkbook
            oModulePermission.CreateWorkBook = .CreateWorkBook
            oModulePermission.DeleteItemsFromOther = .DeleteItemsFromOther
            oModulePermission.DownLoadItemFiles = .DownLoadItemFiles
            oModulePermission.ListOtherWorkBooks = .ListOtherWorkBook
            oModulePermission.ManagementPermission = .GrantPermission
            oModulePermission.PrintOtherWorkBook = .PrintOtherWorkBook
            oModulePermission.ReadOtherWorkBook = .ReadOtherWorkBook
        End With
        Return oModulePermission
    End Function
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public ReadOnly Property CurrentWorkBookID() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.CurrentWorkBookID
        Get
            Dim UrlID As String = Request.QueryString("WorkBookID") 'PageUtility.DecryptQueryString("WorkBookID", SecretKeyUtil.EncType.Common)
            If Not UrlID = "" Then
                Try
                    Return New System.Guid(UrlID)
                Catch ex As Exception

                End Try
            End If
            Return System.Guid.Empty
        End Get
    End Property
    Public Property WorkBookCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.WorkBookCommunityID
        Get
            Return Me.ViewState("WorkBookCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("WorkBookCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.WorkBookEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.WorkBookEditPresenter(Me.CurrentContext, Me)
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

    Public Property CurrentWorkBookType() As lm.Comol.Modules.Base.DomainModel.WorkBookType Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.CurrentWorkBookType
        Get
            If Not TypeOf Me.ViewState("CurrentWorkBookType") Is WorkBookType Then
                Me.ViewState("CurrentWorkBookType") = WorkBookType.None
            End If
            Return Me.ViewState("CurrentWorkBookType")
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.DomainModel.WorkBookType)
            Me.ViewState("CurrentWorkBookType") = value
        End Set
    End Property
    Public Property CurrentStep() As lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.ViewStep Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.CurrentStep
        Get
            Dim oActive As System.Web.UI.WebControls.View = Me.MLVwizard.GetActiveView
            If oActive Is VIWaddAuthor Then
                Return IviewWorkBookEdit.ViewStep.AddAuthors
            ElseIf oActive Is VIWedit Then
                Return IviewWorkBookEdit.ViewStep.ChangeData
            ElseIf oActive Is VIWmanagementAuthors Then
                Return IviewWorkBookEdit.ViewStep.ManagementAuthors
            ElseIf oActive Is VIWselectOwner Then
                Return IviewWorkBookEdit.ViewStep.SelectOwner
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.ViewStep)
            Me.MLVworkBook.SetActiveView(Me.VIWwizardWorkBook)
            Select Case value
                Case IviewWorkBookEdit.ViewStep.ChangeData
                    Me.MLVwizard.SetActiveView(VIWedit)
                Case IviewWorkBookEdit.ViewStep.SelectOwner
                    Me.MLVwizard.SetActiveView(VIWselectOwner)
                Case IviewWorkBookEdit.ViewStep.AddAuthors
                    Me.MLVwizard.SetActiveView(VIWaddAuthor)
                Case IviewWorkBookEdit.ViewStep.ManagementAuthors
                    Me.MLVwizard.SetActiveView(VIWmanagementAuthors)
            End Select
        End Set
    End Property
    Public Property AllowShowItems() As Boolean Implements IviewWorkBookEdit.AllowShowItems
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property ListCurrentView() As lm.Comol.Modules.Base.DomainModel.WorkBookTypeFilter Implements IviewWorkBookEdit.ListCurrentView
        Get
            If IsNothing(Request.QueryString("View")) Then
                Return WorkBookTypeFilter.None
            Else
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of WorkBookTypeFilter).GetByString(Request.QueryString("View"), WorkBookTypeFilter.None)
            End If
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub ShowError(ByVal ErrorString As String) Implements IviewWorkBookEdit.ShowError

    End Sub

#Region "Pagina"
    Public Overrides Sub BindNoPermessi()
        Me.MLVworkBook.SetActiveView(Me.VIWnoPermission)
        Me.Resource.setLabel(Me.LBNopermessi)
    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.SetFocus(Me.BTNreturnToList)
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
            Me.PageUtility.AddAction(Services_WorkBook.ActionType.CreateWorkBook, Nothing, InteractionType.UserWithLearningObject)
        End If
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_WorkBookEdit", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBNopermessi)
            '.setLabel(Me.LBtitoloServizio)
            Me.Master.ServiceTitle = .getValue("LBtitoloServizio.text")
            .setButton(Me.BTNreturnToList, True)
            .setLabel(Me.LBworkBookData)
            .setButton(Me.BTNgoToWorkBookList, True)
            .setButton(Me.BTNsaveGoToWorkBookList, True)
            .setButton(Me.BTNmanagementAuthors, True)
            .setLabel(Me.LBworkBooManagement)
            .setHeaderGridView(Me.GDVauthors, 0, "E", True)
            .setHeaderGridView(Me.GDVauthors, 1, "R", True)
            .setHeaderGridView(Me.GDVauthors, 2, "Autore", True)
            .setHeaderGridView(Me.GDVauthors, 3, "Data", True)
            .setButton(Me.BTNgoToWorkBookList_1, True)
            .setButton(Me.BTNgoToWorkBookList_2, True)
            .setButton(Me.BTNgoToData, True)
            .setButton(Me.BTNsearchAuthors, True)
            .setButton(Me.BTNselectOwner, True)
            .setLabel(Me.LBaddAuthor)
            .setButton(Me.BTNgoToManagement, True)
            .setButton(Me.BTNaddAuthors, True)
            .setLabel(Me.LBselectOwner)
            .setButton(Me.BTNgoToManagement_2, True)
            .setButton(Me.BTNconfirmOwner, True)
            .setButton(Me.BTNfinish, True)
            .setButton(Me.BTNreturnToWorkBookList, True)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Public Sub LoadWorkBook(ByVal oWorkBookDTO As dtoWorkBook, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer))) Implements IviewWorkBookEdit.LoadWorkBook
        Me.MLVworkBook.SetActiveView(Me.VIWwizardWorkBook)
        Me.MLVwizard.SetActiveView(Me.VIWedit)
        Me.CTRLworkBook.LoadWorkBook(oWorkBookDTO, oAvailableStatus)
    End Sub

    Public Sub LoadAuthors(ByVal oList As IList(Of dtoWorkBookAuthor)) Implements IviewWorkBookEdit.LoadAuthors
        Me.GDVauthors.DataSource = oList
        Me.GDVauthors.DataBind()
    End Sub

    Public Property AllowAddAuthors() As Boolean Implements IviewWorkBookEdit.AllowAddAuthors
        Get
            Return Me.ViewState("AllowAddAuthors")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowAddAuthors") = value
            Me.BTNsearchAuthors.Visible = value
        End Set
    End Property

    Public Property AllowSelectOwner() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.AllowSelectOwner
        Get
            Return Me.ViewState("AllowSelectOwner")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSelectOwner") = value
            Me.BTNconfirmOwner.Visible = value
            Me.BTNselectOwner.Visible = value
        End Set
    End Property

    Public Property AllowManagementAuthors() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.AllowManagementAuthors
        Get
            Return Me.BTNgoToManagement.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.BTNmanagementAuthors.Visible = value
            Me.BTNgoToManagement.Visible = value
            Me.BTNgoToManagement_2.Visible = value
            Me.BTNsaveGoToWorkBookList.Visible = Not value
        End Set
    End Property

    Private Sub BTNaddAuthors_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BTNaddAuthors.Command, BTNconfirmOwner.Command, BTNfinish.Command, BTNgoToData.Command, BTNgoToManagement.Command, BTNgoToManagement_2.Command, BTNgoToWorkBookList.Command, BTNgoToWorkBookList_1.Command, BTNgoToWorkBookList_2.Command, BTNmanagementAuthors.Command, BTNsearchAuthors.Command, BTNselectOwner.Command, BTNreturnToList.Command, BTNsaveGoToWorkBookList.Command
        Select Case e.CommandName
            Case "tolist"
                Me.LoadWorkBookList()
            Case "tomanagement"
                Me.CurrentPresenter.LoadWorkBookAuthors()
            Case "todata"
                Me.CurrentPresenter.LoadWorkBook()
            Case "searchauthors"
                Me.CurrentPresenter.LoadSearchAuthors()
            Case "selectOwner"
                Me.CurrentPresenter.LoadSelectOwner()
            Case "savetolist"
                Me.CurrentPresenter.SaveAndGoToList()
            Case "addauthors"
                If Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers.Count > 0 Then
                    Me.CurrentPresenter.AddAuthors()
                End If
            Case "confirmowner"
                If Me.RBLowner.SelectedIndex <> -1 Then
                    Me.CurrentPresenter.SaveOwner(Me.RBLowner.SelectedValue, False)
                End If
            Case "confirmownertolist"
                If Me.RBLowner.SelectedIndex <> -1 Then
                    Me.CurrentPresenter.SaveOwner(Me.RBLowner.SelectedValue, True)
                End If
            Case Else
                Me.LoadWorkBookList()
        End Select
    End Sub

    Public Sub LoadWorkBookList() Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.LoadWorkBookList
        Dim ReturnUrl As String = "Generici/WorkBookList.aspx?View=" & Me.ListCurrentView.ToString
        If Me.CurrentContext.UserContext.CurrentCommunityID > 0 Then
            ReturnUrl &= "&CommunityFilter=" & WorkBookCommunityFilter.CurrentCommunity.ToString & "&Order=" & WorkBookOrder.ChangedOn.ToString
        Else
            ReturnUrl &= "&CommunityFilter=" & WorkBookCommunityFilter.AllCommunities.ToString & "&Order=" & WorkBookOrder.Community.ToString
        End If
        Me.PageUtility.RedirectToUrl(ReturnUrl)
        'Me.PageUtility.RedirectToUrl("Generici/WorkBookList.aspx?View=" & Me.ListCurrentView.ToString)
    End Sub

    Public Sub LoadSearchUser(ByVal oCommunity As System.Collections.Generic.List(Of Integer), ByVal multiple As Boolean, ByVal oExceptUsers As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.LoadSearchUser
        Me.CTRLuserList.CurrentPresenter.Init(oCommunity, IIf(multiple, ListSelectionMode.Multiple, ListSelectionMode.Single), oExceptUsers)
    End Sub

    Public Sub LoadOwner(ByVal OwnerID As Integer, ByVal oList As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.iPerson), ByVal oPermission As lm.Comol.Modules.Base.DomainModel.iWorkBookPermission) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.LoadOwner
        Me.RBLowner.DataSource = oList
        Me.RBLowner.DataTextField = "SurnameAndName"
        Me.RBLowner.DataValueField = "Id"
        Me.RBLowner.Enabled = (oPermission.EditWorkBook OrElse oPermission.CreateWorkBook)
        Me.RBLowner.DataBind()

        If Me.RBLowner.Items.Count > 0 Then
            Me.RBLowner.SelectedValue = OwnerID
            If Me.RBLowner.SelectedIndex = -1 Then
                Me.RBLowner.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub GDVauthors_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GDVauthors.RowCommand
        Try
            If e.CommandName <> "Sort" Then
                Dim AuthorID As System.Guid
                AuthorID = New System.Guid(e.CommandArgument.ToString)
                If e.CommandName = "confirmDelete" Then
                    Me.CurrentPresenter.RemoveAuthor(AuthorID)
                ElseIf e.CommandName = "virtualdelete" Then
                    Me.CurrentPresenter.VirtualDeleteAuthor(AuthorID)
                ElseIf e.CommandName = "undelete" Then
                    Me.CurrentPresenter.UnDeleteAuthor(AuthorID)
                End If
            End If
        Catch ex As Exception
            Me.CurrentPresenter.LoadWorkBookAuthors()
        End Try
    End Sub
    Private Sub GDVauthors_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GDVauthors.RowDataBound
        If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
            Dim oItem As dtoWorkBookAuthor = TryCast(e.Row.DataItem, dtoWorkBookAuthor)
            If Not IsNothing(oItem) Then

                Dim oLNBcancellaDefinitivo, oLNBvirtualDelete, oLNBundelete, oLNBripristina As LinkButton
                oLNBcancellaDefinitivo = e.Row.FindControl("LNBdelete")
                oLNBvirtualDelete = e.Row.FindControl("LNBvirtualDelete")
                oLNBundelete = e.Row.FindControl("LNBundelete")
                oLNBripristina = e.Row.FindControl("LNBripristina")
                oLNBripristina = e.Row.FindControl("LNBripristina")

                If Not IsNothing(oLNBcancellaDefinitivo) Then
                    Me.Resource.setLinkButton(oLNBcancellaDefinitivo, True, True, , True)
                    oLNBcancellaDefinitivo.Text = String.Format(oLNBcancellaDefinitivo.Text, Me.BaseUrl & "images/grid/eliminato1.gif", oLNBcancellaDefinitivo.ToolTip)

                    Me.Resource.setLinkButton(oLNBvirtualDelete, True, True, , True)
                    oLNBvirtualDelete.Text = String.Format(oLNBvirtualDelete.Text, Me.BaseUrl & "images/grid/cancella.gif", oLNBvirtualDelete.ToolTip)

                    Me.Resource.setLinkButton(oLNBundelete, True, True)
                    oLNBundelete.Text = String.Format(oLNBundelete.Text, Me.BaseUrl & "images/grid/ripristina.gif", oLNBundelete.ToolTip)

                    oLNBcancellaDefinitivo.CommandArgument = oItem.Author.Id.ToString
                    oLNBvirtualDelete.CommandArgument = oItem.Author.Id.ToString
                    oLNBundelete.CommandArgument = oLNBvirtualDelete.CommandArgument
                    Me.Resource.setLinkButton(oLNBripristina, True, True)

                    oLNBvirtualDelete.Visible = Not oItem.Author.isDeleted AndAlso oItem.Permission.DeleteWorkBook AndAlso Not oItem.Author.IsOwner
                    oLNBundelete.Visible = oItem.Author.isDeleted AndAlso oItem.Permission.UndeleteWorkBook
                    oLNBcancellaDefinitivo.Visible = oItem.Author.isDeleted AndAlso oItem.Permission.DeleteWorkBook
                End If
                If oItem.Author.isDeleted Then
                    e.Row.CssClass = "GrigliaNuovaDeleted"
                End If

                Dim oLiteral As Literal = e.Row.FindControl("LTisOwner")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = Me.Resource.getValue("isOwner." & oItem.Author.IsOwner)
                End If

                oLiteral = e.Row.FindControl("LTsurnameName")
                If Not IsNothing(oLiteral) Then
                    oLiteral.Text = oItem.Author.Author.SurnameAndName
                End If

                oLiteral = e.Row.FindControl("LTcreatedOn")
                If Not IsNothing(oLiteral) Then
                    If oItem.Author.ModifiedOn.HasValue Then
                        oLiteral.Text = CDate(oItem.Author.ModifiedOn).ToString("dd/MM/yy HH:mm:ss")
                    ElseIf oItem.Author.CreatedOn.HasValue Then
                        oLiteral.Text = CDate(oItem.Author.CreatedOn).ToString("dd/MM/yy HH:mm:ss")
                    Else
                        oLiteral.Text = "&nbsp;"
                    End If
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property SelectedUsers() As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.Person) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.SelectedUsers
        Get
            Dim oList As List(Of lm.Comol.Core.DomainModel.Person)
            oList = (From o In Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers Select New Person() With {.Id = o.Id, .Name = o.Name, .Surname = o.Surname}).ToList

            Return oList
        End Get
    End Property

    Public Function GetWorkBook() As lm.Comol.Modules.Base.DomainModel.WorkBook Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.GetWorkBook
        Return Me.CTRLworkBook.GetWorkBook
    End Function

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub

    Public Sub NotifyCommunityEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.NotifyCommunityEdit
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyCommunityWorkBookEdit(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.ListCurrentView.ToString)
    End Sub

    Public Sub NotifyPersonalEdit(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookEdit.NotifyPersonalEdit
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyPersonalWorkBookEdit(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.ListCurrentView.ToString)
    End Sub

    Public WriteOnly Property AllowEditingChanging() As Boolean Implements IviewWorkBookEdit.AllowEditingChanging
        Set(ByVal value As Boolean)
            Me.CTRLworkBook.AllowEditingChanging = value
        End Set
    End Property

    Public ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String Implements IviewWorkBookEdit.GetEditingTranslation
        Get
            Return Me.CTRLworkBook.GetEditingTranslation(Translation)
        End Get
    End Property

    Public Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission) Implements IviewWorkBookEdit.SetEditing
        Me.CTRLworkBook.SetEditing(oAvailableEditing, ItemEditing)
    End Sub

    Public Property AllowStatusChange() As Boolean Implements IviewWorkBookEdit.AllowStatusChange
        Get
            Return Me.CTRLworkBook.AllowStatusChange
        End Get
        Set(ByVal value As Boolean)
            Me.CTRLworkBook.AllowStatusChange = value
        End Set
    End Property
End Class