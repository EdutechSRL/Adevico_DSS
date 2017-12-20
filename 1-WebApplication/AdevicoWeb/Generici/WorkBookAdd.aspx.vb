Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract


Partial Public Class WorkBookAdd
    Inherits PageBase
    Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd


#Region "Private Property"
    Private _PageUtility As OLDpageUtility
    Private _Presenter As lm.Comol.Modules.Base.Presentation.WorkBookAddPresenter
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
            Return _Servizio
        End Get
    End Property

    Public ReadOnly Property Permission() As lm.Comol.Modules.Base.DomainModel.ModuleWorkBook Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ModulePermission
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
    Public ReadOnly Property CommunitiesPermission() As System.Collections.Generic.IList(Of lm.Comol.Modules.Base.Presentation.WorkBookCommunityPermission) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CommunitiesPermission
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
    Public Property CurrentWorkBookID() As System.Guid Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CurrentWorkBookID
        Get
            If TypeOf Me.ViewState("CurrentWorkBookID") Is System.Guid Then
                Return Me.ViewState("CurrentWorkBookID")
            Else
                Return System.Guid.Empty
            End If
        End Get
        Set(ByVal value As System.Guid)
            Me.ViewState("CurrentWorkBookID") = value
        End Set
    End Property
    Public Property WorkBookCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.WorkBookCommunityID
        Get
            Return Me.ViewState("WorkBookCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("WorkBookCommunityID") = value
        End Set
    End Property
    Public Property CurrentCommunityID() As Integer Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CurrentCommunityID
        Get
            Return Me.ViewState("CurrentCommunityID")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("CurrentCommunityID") = value
        End Set
    End Property
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.WorkBookAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.WorkBookAddPresenter(Me.CurrentContext, Me)
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

    Public Property CurrentWorkBookType() As lm.Comol.Modules.Base.DomainModel.WorkBookType Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CurrentWorkBookType
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
    'Public Property AvailableStep() As IList(Of IviewWorkBookAdd.ViewStep) Implements IviewWorkBookAdd.AvailableStep
    '    Get
    '        Return Me.ViewState("AvailableStep")
    '    End Get
    '    Set(ByVal value As IList(Of IviewWorkBookAdd.ViewStep))
    '        Me.ViewState("AvailableStep") = value
    '    End Set
    'End Property
    Public Property CurrentStep() As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CurrentStep
        Get
            If Me.MLVworkBook.Visible Then
                If Me.WZRworkBook.ActiveStep Is Me.WSTcommunity Then
                    Return IviewWorkBookAdd.ViewStep.SelectCommunity
                ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTdata Then
                    Return IviewWorkBookAdd.ViewStep.SelectData
                ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTtype Then
                    Return IviewWorkBookAdd.ViewStep.SelectType
                ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTauthors Then
                    Return IviewWorkBookAdd.ViewStep.SelectAuthors
                ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTowner Then
                    Return IviewWorkBookAdd.ViewStep.SelectOwner
                ElseIf Me.WZRworkBook.ActiveStep Is Me.WSTcomplete Then
                    Return IviewWorkBookAdd.ViewStep.FinalMessage
                End If
            Else
                Return IviewWorkBookAdd.ViewStep.None
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep)
            Me.MLVworkBook.SetActiveView(Me.VIWcreate)
            Select Case value
                Case IviewWorkBookAdd.ViewStep.SelectCommunity
                    Me.WZRworkBook.ActiveStepIndex = 0
                Case IviewWorkBookAdd.ViewStep.SelectData
                    Me.WZRworkBook.ActiveStepIndex = 2
                Case IviewWorkBookAdd.ViewStep.SelectType
                    Me.WZRworkBook.ActiveStepIndex = 1
                Case IviewWorkBookAdd.ViewStep.SelectOwner
                    Me.WZRworkBook.ActiveStepIndex = 4
                Case IviewWorkBookAdd.ViewStep.SelectAuthors
                    Me.WZRworkBook.ActiveStepIndex = 3
                Case IviewWorkBookAdd.ViewStep.FinalMessage
                    Me.WZRworkBook.ActiveStepIndex = 5
            End Select
        End Set
    End Property
    Public WriteOnly Property FirstStep() As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.FirstStep
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep)
            Select Case value
                Case IviewWorkBookAdd.ViewStep.SelectCommunity
                    Me.WSTcommunity.StepType = WizardStepType.Start
                Case IviewWorkBookAdd.ViewStep.SelectType
                    Me.WSTtype.StepType = WizardStepType.Start
                Case IviewWorkBookAdd.ViewStep.SelectData
                    Me.WSTdata.StepType = WizardStepType.Start
                Case IviewWorkBookAdd.ViewStep.SelectAuthors
                    Me.WSTauthors.StepType = WizardStepType.Start
                Case IviewWorkBookAdd.ViewStep.SelectOwner
                    Me.WSTowner.StepType = WizardStepType.Start
            End Select
        End Set
    End Property
    Public Property LastStep() As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.LastStep
        Get
            If Me.WSTdata.StepType = WizardStepType.Finish Then
                Return IviewWorkBookAdd.ViewStep.SelectData
            ElseIf Me.WSTauthors.StepType = WizardStepType.Finish Then
                Return IviewWorkBookAdd.ViewStep.SelectAuthors
            ElseIf Me.WSTowner.StepType = WizardStepType.Finish Then
                Return IviewWorkBookAdd.ViewStep.SelectOwner
            End If
        End Get
        Set(ByVal value As lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ViewStep)
            Me.WSTdata.StepType = WizardStepType.Step
            Me.WSTauthors.StepType = WizardStepType.Step
            Me.WSTowner.StepType = WizardStepType.Step
            Select Case value
                Case IviewWorkBookAdd.ViewStep.SelectData
                    Me.WSTdata.StepType = WizardStepType.Finish
                Case IviewWorkBookAdd.ViewStep.SelectAuthors
                    Me.WSTauthors.StepType = WizardStepType.Finish
                Case IviewWorkBookAdd.ViewStep.SelectOwner
                    Me.WSTowner.StepType = WizardStepType.Finish
            End Select
        End Set
    End Property
    Public ReadOnly Property CreateForOther() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CreateForOther
        Get
            Try
                If Not String.IsNullOrEmpty(Me.Request.QueryString("CreateForOther")) Then
                    Return CBool(Me.Request.QueryString("CreateForOther"))
                End If
            Catch ex As Exception

            End Try
            Return False
        End Get
    End Property

    Public Property CurrentOwner() As lm.Comol.Core.DomainModel.Person Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.CurrentOwner
        Get
            Dim oPerson As lm.Comol.Core.DomainModel.Person = Nothing
            If Me.RBLowner.Items.Count = 0 OrElse Me.RBLowner.SelectedIndex = -1 Then
                Return Nothing
            Else
                oPerson = New lm.Comol.Core.DomainModel.Person With {.Id = Me.RBLowner.SelectedValue}
                Return oPerson
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.Person)
            If Me.RBLowner.Items.Count = 1 Then
                Me.RBLowner.SelectedIndex = 0
            ElseIf Me.RBLowner.Items.Count > 1 Then
                Me.RBLowner.SelectedValue = value.Id
                If Me.RBLowner.SelectedIndex = -1 Then
                    Me.RBLowner.SelectedIndex = 0
                End If
            End If
        End Set
    End Property

    Public Property AddCurrentUserToAuthors() As Boolean Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.AddCurrentUserToAuthors
        Get
            Return Me.CBXselfAuthor.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXselfAuthor.Checked = value
        End Set
    End Property
    Public ReadOnly Property GetCurrentWorkBook() As lm.Comol.Modules.Base.DomainModel.WorkBook Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.GetCurrentWorkBook
        Get
            Return Me.CTRLworkBook.GetWorkBook
        End Get
    End Property
    Public Property SelectOnlyCurrentUser() As Boolean Implements IviewWorkBookAdd.SelectOnlyCurrentUser
        Get
            Return CBool(Me.ViewState("SelectOnlyCurrentUser"))
            ' Return Me.CBXselfAuthor.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("SelectOnlyCurrentUser") = value
            Me.CBXselfAuthor.Enabled = value
            Me.CTRLuserList.Visible = Not value
        End Set
    End Property
    Public Property AllowSelectCurrentUser() As Boolean Implements IviewWorkBookAdd.AllowSelectCurrentUser
        Get
            Return CBool(Me.ViewState("AllowSelectCurrentUser"))
            ' Return Me.CBXselfAuthor.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSelectCurrentUser") = value
            Me.CBXselfAuthor.Enabled = value
        End Set
    End Property


    Public ReadOnly Property ListCurrentView() As WorkBookTypeFilter Implements IviewWorkBookAdd.ListCurrentView
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

    ' FASE -1 - ATTIVO LA SELEZIONE DELLA COMUNITA DI DESTINAZIONE
    Public Sub InitCommunitySelection() Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.InitCommunitySelection
        Me.MLVworkBook.SetActiveView(Me.VIWcreate)
        Me.WZRworkBook.ActiveStepIndex = 0

        Dim oService As Services_WorkBook = Services_WorkBook.Create
        oService.CreateWorkBook = True
        oService.CreateGroupWorkbook = True
        oService.Admin = True

        Dim oServiceBase As New ServiceBase(0, oService.Codex, oService.PermessiAssociati)
        Dim oClause As New GenericClause(Of ServiceClause)
        oClause.OperatorForNextClause = OperatorType.OrCondition
        oClause.Clause = New ServiceClause(oServiceBase, OperatorType.OrCondition)

        Me.CTRLcommunity.ServiceClauses = oClause
        Me.CTRLcommunity.InitializeControl(-1)

        Dim loNextButton As Button = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
        loNextButton.Visible = True
        loNextButton.Enabled = Not (Me.CTRLcommunity.SelectedCommunitiesID.Count = 0)
    End Sub

    ' FASE 0 - HO SELEZIONATO LA DI DESTINAZIONE
    Private Sub CTRLcommunity_SelectedCommunityChanged(ByVal CommunityID As Integer) Handles CTRLcommunity.SelectedCommunityChanged
        Dim oNextButton As Button = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
        oNextButton.Enabled = (CommunityID >= 0)
    End Sub

    ' FASE 1 - DEVO SCEGLIERE IL TIPO
    Public Sub LoadAvailableTypes(ByVal oList As System.Collections.Generic.List(Of IviewWorkBookAdd.viewWorkBookType)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.LoadAvailableTypes
        Me.CTRLtype.DefineSelectableTypes(oList)
        Me.WZRworkBook.ActiveStepIndex = 1

        If Me.WSTtype.StepType = WizardStepType.Start Then
            Dim loNextButton As Button = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
            loNextButton.Visible = False
        Else
            Dim loNextButton As Button = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            loNextButton.Visible = False
        End If
    End Sub

    ' FASE 1 - SCELTO IL TIPO di WORKBOOK
    Private Sub CTRLtype_TypeSelected(ByVal oType As IviewWorkBookAdd.viewWorkBookType) Handles CTRLtype.TypeSelected
        Me.CurrentPresenter.LoadWorkBookDataEntry(oType)
    End Sub

    Private Sub WZRworkBook_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles WZRworkBook.CancelButtonClick
        Me.LoadWorkBookList(Me.CurrentWorkBookType)
    End Sub

    Private Sub WZworkBook_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.NextButtonClick
        Select Case Me.CurrentStep
            Case IviewWorkBookAdd.ViewStep.SelectCommunity
                If Not Me.CTRLcommunity.SelectedCommunitiesID.Count = 0 Then
                    Me.CurrentPresenter.LoadWorkBookType(Me.CTRLcommunity.SelectedCommunitiesID(0))
                End If
            Case IviewWorkBookAdd.ViewStep.SelectData
                Me.CurrentPresenter.LoadNextStep()
            Case IviewWorkBookAdd.ViewStep.SelectAuthors
                If Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers.Count > 0 Then
                    Me.CurrentPresenter.LoadNextStep()
                Else
                    e.Cancel = True
                End If
        End Select
    End Sub

    Private Sub WZworkBook_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.PreviousButtonClick
        Me.CurrentPresenter.LoadPreviousStep()
    End Sub
    Private Sub WZworkBook_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WZRworkBook.FinishButtonClick
        Me.CurrentPresenter.LoadNextStep()
    End Sub

    Public Sub ShowError(ByVal ErrorString As String) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ShowError
        Me.LBNopermessi.Text = ErrorString
        Me.MLVworkBook.SetActiveView(Me.VIWnoPermission)
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
        Dim oButton As Button
        If Me.WZRworkBook.ActiveStep Is WSTcommunity Then
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
        ElseIf Me.WZRworkBook.ActiveStep Is WSTcomplete Then
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StepNextButton")
        Else
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
        End If
        If Not IsNothing(oButton) Then
            Me.SetFocus(oButton)
        End If
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
        MyBase.SetCulture("pg_WorkBookAdd", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setLabel(Me.LBtitoloServizio)
            Me.Master.ServiceTitle = .getValue("LBtitoloServizio.text")
            .setLabel(Me.LBworkbookType)
            .setLabel(Me.LBworkbookCommunity)
            .setLabel(Me.LBworkBookData)
            .setLabel(Me.LBauthors)
            .setLabel(Me.LBownerTitle)
            .setCheckBox(Me.CBXselfAuthor)
            .setButton(BTNreturnToList)
            Dim oButton As Button
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StartNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("BTNgoToWorkBookList")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            oButton = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            If Not IsNothing(oButton) Then
                .setButton(oButton, True)
            End If
            'oButton = Me.WZRimport.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            'If Not IsNothing(oButton) Then
            '    .setButton(oButton, True)
            'End If
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

    Public Sub LoadWorkBook(ByVal oWorkBookDTO As dtoWorkBook, ByVal oAvailableStatus As List(Of TranslatedItem(Of Integer))) Implements IviewWorkBookAdd.LoadWorkBook
        'Me.MLVworkBook.SetActiveView(Me.VIWwizardWorkBook)
        '    Me.WZworkBook.ActiveStepIndex = 2
        Me.CTRLworkBook.LoadWorkBook(oWorkBookDTO, oAvailableStatus)
        If Me.WSTdata.StepType = WizardStepType.Finish Then
            Dim loNextButton As Button = Me.WZRworkBook.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")
            loNextButton.Visible = True
        Else
            Dim loNextButton As Button = Me.WZRworkBook.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton")
            loNextButton.Visible = True
        End If
    End Sub

    Public ReadOnly Property SelectedUsers() As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.Person) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.SelectedUsers
        Get
            Dim oList As List(Of lm.Comol.Core.DomainModel.Person)
            oList = (From o In Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers Select New Person() With {.Id = o.Id, .Name = o.Name, .Surname = o.Surname}).ToList

            Return oList
        End Get
    End Property

    Public Sub LoadSearchAuthors() Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.LoadSearchAuthors
        Me.WZRworkBook.ActiveStepIndex = 3
        Me.SetFocus(Me.CTRLuserList.GetSearchButtonControl)

        'Me.CTRLuserList.SelectionMode = IIf(multiple, ListSelectionMode.Multiple, ListSelectionMode.Single)
        'Me.CBXselfAuthor.Checked = AddSelf
    End Sub
    Public Sub LoadAuthorsToSelectOwner(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.Person)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.LoadAuthorsToSelectOwner
        Dim SelectedID As Integer = -1

        If Me.RBLowner.SelectedIndex <> -1 Then
            SelectedID = Me.RBLowner.SelectedValue
        End If

        Me.RBLowner.DataSource = oList
        Me.RBLowner.DataValueField = "Id"
        Me.RBLowner.DataTextField = "SurnameAndName"
        Me.RBLowner.DataBind()

        If SelectedID <> -1 Then
            Me.RBLowner.SelectedValue = SelectedID
        ElseIf Me.RBLowner.Items.Count > 0 AndAlso Me.RBLowner.SelectedIndex = -1 Then
            Me.RBLowner.SelectedIndex = 0
        End If
    End Sub

    Public Sub ShowCompleteMessage(ByVal iError As IviewWorkBookAdd.viewError) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.ShowCompleteMessage
        Me.LBcompleteMessage.Text = Me.Resource.getValue("viewError." & iError)
    End Sub

    Public Sub LoadWorkBookList(ByVal oType As lm.Comol.Modules.Base.DomainModel.WorkBookType) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.LoadWorkBookList
        Dim ReturnUrl As String = "Generici/WorkBookList.aspx?View=" & Me.ListCurrentView.ToString
        If Me.CurrentContext.UserContext.CurrentCommunityID > 0 Then
            ReturnUrl &= "&CommunityFilter=" & WorkBookCommunityFilter.CurrentCommunity.ToString & "&Order=" & WorkBookOrder.ChangedOn.ToString
        Else
            ReturnUrl &= "&CommunityFilter=" & WorkBookCommunityFilter.AllCommunities.ToString & "&Order=" & WorkBookOrder.Community.ToString
        End If
        Me.PageUtility.RedirectToUrl(ReturnUrl)
    End Sub

    Public Sub InitUsersList(ByVal oCommunityList As List(Of Integer), ByVal multiple As Boolean, ByVal ExceptUsers As List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.InitUsersList
        Me.CTRLuserList.CurrentPresenter.Init(oCommunityList, IIf(multiple, ListSelectionMode.Multiple, ListSelectionMode.Single), ExceptUsers)
    End Sub


    Private Sub BTNreturnToList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreturnToList.Click, BTNreturnToWorkBookList.Click
        Me.LoadWorkBookList(Me.CurrentWorkBookType)
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_WorkBook.Codex)
    End Sub

    Public Sub NotifyCommunity(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.NotifyCommunity
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyCommunityWorkBookCreated(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.ListCurrentView.ToString)
    End Sub

    Public Sub NotifyPersonal(ByVal CommunityID As Integer, ByVal WorkBookID As System.Guid, ByVal Name As String, ByVal CreatorName As String, ByVal Authors As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Modules.Base.Presentation.IviewWorkBookAdd.NotifyPersonal
        Dim oService As New WorkBookNotificationUtility(Me.PageUtility)
        oService.NotifyPersonalWorkBookCreated(CommunityID, WorkBookID, Name, CreatorName, Authors, Me.ListCurrentView.ToString)
    End Sub

    Public WriteOnly Property AllowEditingChanging() As Boolean Implements IviewWorkBookAdd.AllowEditingChanging
        Set(ByVal value As Boolean)
            Me.CTRLworkBook.AllowEditingChanging = value
        End Set
    End Property

    Public ReadOnly Property GetEditingTranslation(ByVal Translation As Integer) As String Implements IviewWorkBookAdd.GetEditingTranslation
        Get
            Return Me.CTRLworkBook.GetEditingTranslation(Translation)
        End Get
    End Property

    Public Sub SetEditing(ByVal oAvailableEditing As List(Of TranslatedItem(Of Integer)), ByVal ItemEditing As EditingPermission) Implements IviewWorkBookAdd.SetEditing
        Me.CTRLworkBook.SetEditing(oAvailableEditing, ItemEditing)
    End Sub

    Public Property AllowStatusChange() As Boolean Implements IviewWorkBookAdd.AllowStatusChange
        Get
            Return Me.CTRLworkBook.AllowStatusChange
        End Get
        Set(ByVal value As Boolean)
            Me.CTRLworkBook.AllowStatusChange = value
        End Set
    End Property
End Class