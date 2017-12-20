Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports COL_Questionario
Imports COL_Questionario.Business


Public Class ImportUsersFromCSV
    Inherits PageBaseQuestionario
    Implements IViewImportUsersFromCSV


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
    Private _Presenter As ImportUsersFromCSVpresenter
    Private ReadOnly Property CurrentPresenter() As ImportUsersFromCSVpresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ImportUsersFromCSVpresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Public ReadOnly Property SessionIdQuestionnnaire As Integer Implements IViewImportUsersFromCSV.SessionIdQuestionnnaire
        Get
            Return Me.QuestionarioCorrente.id
        End Get
    End Property
    Public ReadOnly Property PreloadedIdQuestionnnaire As Integer Implements IViewImportUsersFromCSV.PreloadedIdQuestionnnaire
        Get
            Return qs_questId
        End Get
    End Property
    Public Property CurrentIdQuestionnnaire As Integer Implements IViewImportUsersFromCSV.CurrentIdQuestionnnaire
        Get
            Return ViewStateOrDefault("CurrentIdQuestionnnaire", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdQuestionnnaire") = value
        End Set
    End Property
    Public Property CurrentStep As InvitedUsersImportStep Implements IViewImportUsersFromCSV.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", InvitedUsersImportStep.None)
        End Get
        Set(value As InvitedUsersImportStep)
            ViewState("CurrentStep") = value
        End Set
    End Property
    Public Property SkipSteps As List(Of InvitedUsersImportStep) Implements IViewImportUsersFromCSV.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of InvitedUsersImportStep))
        End Get
        Set(value As List(Of InvitedUsersImportStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Public WriteOnly Property AllowManagement As Boolean Implements IViewImportUsersFromCSV.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagement.Visible = value
            Me.HYPbackToManagement.NavigateUrl = BaseUrl & "Questionari/UtentiInvitati.aspx"
        End Set
    End Property
    Public Property AvailableSteps As List(Of InvitedUsersImportStep) Implements IViewImportUsersFromCSV.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of InvitedUsersImportStep))
        End Get
        Set(value As List(Of InvitedUsersImportStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Public Property ImportIdentifier As System.Guid Implements IViewImportUsersFromCSV.ImportIdentifier
        Get
            Return ViewStateOrDefault("ImportIdentifier", System.Guid.NewGuid)
        End Get
        Set(value As System.Guid)
            ViewState("ImportIdentifier") = value
        End Set
    End Property
#End Region


#Region "Inherits"
    Public Overrides ReadOnly Property isCompileForm As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            RPAimportUsers.ProgressIndicators = RPAimportUsers.ProgressIndicators And Not Telerik.Web.UI.Upload.ProgressIndicators.SelectedFilesCount
        End If
        RPAimportUsers.Localization.Total = Resource.getValue("Localization.Total")
        RPAimportUsers.Localization.TotalFiles = Resource.getValue("Localization.TotalFiles")
        RPAimportUsers.Localization.ElapsedTime = Resource.getValue("Localization.ElapsedTime")
        RPAimportUsers.Localization.EstimatedTime = Resource.getValue("Localization.EstimatedTime")
        RPAimportUsers.Localization.TransferSpeed = Resource.getValue("Localization.TransferSpeed")


        RPAimportUsers.Localization.Uploaded = Resource.getValue("Localization.Uploaded")
        RPAimportUsers.Localization.UploadedFiles = Resource.getValue("Localization.UploadedFiles")
        RPAimportUsers.Localization.CurrentFileName = Resource.getValue("Localization.CurrentFileName")
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.Master.AddToolTip()
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean Implements IViewImportUsersFromCSV.HasPermission
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.QuestionariSuInvito)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ImportUsersFromCSV", "Modules", "Questionnaire")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleImport")
            Me.Master.ServiceNopermission = .getValue("serviceImportNopermission")
            .setHyperLink(HYPbackToManagement, True, True)

            .setButton(BTNbackBottom, True, , , True)
            .setButton(BTNbackTop, True, , , True)
            .setButton(BTNnextBottom, True, , , True)
            .setButton(BTNnextTop, True, , , True)
            .setButton(BTNcompleteTop, True, , , True)
            .setButton(BTNcompleteBottom, True, , , True)
            .setLabel(LBinvitedUsersErrors_t)
        End With
    End Sub


#End Region


#Region "Implements"
    Public Function LoadQuestionnaire(id As Integer) As Questionario Implements IViewImportUsersFromCSV.LoadQuestionnaire
        Return LoadQuestionnaireById(id)
    End Function

#Region "Move Wizard"

    Public Sub GotoStep(pStep As InvitedUsersImportStep) Implements IViewImportUsersFromCSV.GotoStep
        GotoStep(pStep, False)
    End Sub

    Public Sub GotoStep(pStep As InvitedUsersImportStep, initialize As Boolean) Implements IViewImportUsersFromCSV.GotoStep
        Me.MLVcsvImport.SetActiveView(VIWwizard)
        Select Case pStep
            Case InvitedUsersImportStep.SourceCSV
                If initialize Then : InitializeStep(pStep)
                End If
                Me.CTRLfieldsMatcher.isInitialized = False
                '   Me.CTRLitemsSelector.isInitialized = False
                MLVwizard.SetActiveView(VIWsourceCSV)
            Case InvitedUsersImportStep.FieldsMatcher
                If initialize AndAlso Not IsInitialized(pStep) Then
                Else
                    MLVwizard.SetActiveView(VIWfieldsMatcher)
                End If
            Case InvitedUsersImportStep.ItemsSelctor
                If initialize AndAlso Not IsInitialized(pStep) Then
                    InitializeStep(InvitedUsersImportStep.ItemsSelctor)
                End If
                MLVwizard.SetActiveView(VIWitemsSelector)
            Case InvitedUsersImportStep.Summary
                Dim total As Integer = Me.CTRLitemsSelector.SelectedItemsCount
                If total = 1 Then
                    Me.LBsummary.Text = String.Format(Resource.getValue("LBsummary.text.0"), QuestionarioCorrente.nome)
                Else
                    Me.LBsummary.Text = String.Format(Resource.getValue("LBsummary.text"), QuestionarioCorrente.nome, total.ToString)
                End If
                MLVwizard.SetActiveView(VIWsummary)
                Me.LBstepTitle.Text = Resource.getValue("InvitedUsersImportStep." & Me.CurrentStep.ToString & ".Title")
                Me.LBstepDescription.Text = Resource.getValue("InvitedUsersImportStep." & pStep.ToString & ".Description")
                If initialize Then : InitializeStep(pStep)
                End If
        End Select

        Me.CurrentStep = pStep
        Me.BTNbackBottom.Visible = (pStep <> InvitedUsersImportStep.SourceCSV)
        Me.BTNbackTop.Visible = (pStep <> InvitedUsersImportStep.SourceCSV)
        Me.BTNcompleteBottom.Visible = (pStep = InvitedUsersImportStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = InvitedUsersImportStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> InvitedUsersImportStep.Summary AndAlso pStep <> InvitedUsersImportStep.Errors AndAlso pStep <> InvitedUsersImportStep.Completed AndAlso pStep <> InvitedUsersImportStep.ImportWithErrors)
        Me.BTNnextTop.Visible = (pStep <> InvitedUsersImportStep.Summary AndAlso pStep <> InvitedUsersImportStep.Errors AndAlso pStep <> InvitedUsersImportStep.Completed AndAlso pStep <> InvitedUsersImportStep.ImportWithErrors)

        If pStep <> InvitedUsersImportStep.Errors AndAlso pStep <> InvitedUsersImportStep.Completed AndAlso pStep <> InvitedUsersImportStep.ImportWithErrors Then
            Me.LBstepTitle.Text = String.Format(Resource.getValue("InvitedUsersImportStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
            Me.LBstepDescription.Text = Resource.getValue("InvitedUsersImportStep." & pStep.ToString & ".Description")
        End If
    End Sub

    Public Sub InitializeStep(pStep As InvitedUsersImportStep) Implements IViewImportUsersFromCSV.InitializeStep
        Select Case pStep
            Case InvitedUsersImportStep.SourceCSV
                CTRLcsvSelector.CSVfolder = "/quest/" & CurrentIdQuestionnnaire & "/" & PageUtility.CurrentUser.ID & "/"
                Me.CTRLcsvSelector.InitializeControl()
            Case InvitedUsersImportStep.FieldsMatcher
                Me.CTRLfieldsMatcher.InitializeControl(AvailableColumns, GetAvailableColumns)
            Case InvitedUsersImportStep.ItemsSelctor
                Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = Me.CTRLfieldsMatcher.SourceFields
                Dim noDB As New List(Of DestinationItem(Of Integer))
                noDB.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .InputType = InputType.mail})
                Me.CTRLitemsSelector.InitializeControl(Me.CTRLcsvSelector.GetFileContent(columns), columns.Where(Function(c) c.InputType = InputType.mail).ToList(), columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.mail OrElse c.DestinationColumn.Id = ImportedUserColumn.name OrElse c.DestinationColumn.Id = ImportedUserColumn.surname).Select(Function(c) c.Number).ToList(), columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.mail).Select(Function(c) c.Number).ToList())
            Case InvitedUsersImportStep.Summary
                
                'If Not IsInitialized(pStep) Then
                '    Me.CTRLsummary.InitializeControl(Me.CTRLfieldsMatcher.SelectedAuthenticationTypeName, Me.CTRLfieldsMatcher.SelectedProfileTypeName, Me.CTRLmailTemplate.SendMailToUsers, PrimaryOrganizationName, OtherOrganizationsToSubscribe, Me.CTRLsubscriptions.SelectedSubscriptions, Me.CTRLitemsSelector.SelectedItemsCount)
                'End If
        End Select
    End Sub
    Public Function IsInitialized(pStep As InvitedUsersImportStep) As Boolean Implements IViewImportUsersFromCSV.IsInitialized
        Select Case pStep
            Case InvitedUsersImportStep.SourceCSV
                Return Me.CTRLcsvSelector.isInitialized
            Case InvitedUsersImportStep.FieldsMatcher
                Return Me.CTRLfieldsMatcher.isInitialized
            Case InvitedUsersImportStep.ItemsSelctor
                Return Me.CTRLitemsSelector.isInitialized
                'Case InvitedUsersImportStep.Summary
                '    Return Me.CTRLsummary.isInitialized
            Case Else
                Return True
        End Select
    End Function
    Private Function GetStepNumber(pStep As InvitedUsersImportStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of InvitedUsersImportStep) = Me.AvailableSteps
        Dim toSkip As List(Of InvitedUsersImportStep) = Me.SkipSteps
        For Each item As InvitedUsersImportStep In (From i In list Where toSkip.Contains(i) = False Select i).ToList
            If item = pStep Then
                Return Number
            Else
                Number += 1
            End If
        Next

    End Function
    Private Sub BTNbackBottom_Click(sender As Object, e As System.EventArgs) Handles BTNbackBottom.Click, BTNbackTop.Click
        ' If Me.MLVwizard.GetActiveView Is VIWerror Then
        '    MLVwizard.SetActiveView(VIWcomplete)

        '    Me.BTNcompleteBottom.Visible = True
        '    Me.BTNcompleteTop.Visible = True
        '    Me.CurrentStep = InvitedUsersImportStep.Summary
        '    Me.LBstepTitle.Text = String.Format(Resource.getValue("InvitedUsersImportStep." & InvitedUsersImportStep.Summary.ToString & ".Title"), GetStepNumber(InvitedUsersImportStep.Summary))
        '    Me.LBstepDescription.Text = Resource.getValue("InvitedUsersImportStep." & InvitedUsersImportStep.Summary.ToString & ".Description")
        'Else
        Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        ' End If
    End Sub

    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If Me.isCompleted Then
            If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
                Me.PageUtility.RedirectToUrl("Questionari/UtentiInvitati.aspx")
            End If
        Else
            Me.CurrentPresenter.ImportItems(CTRLitemsSelector.SelectedItems)
        End If
    End Sub


    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    End Sub
#End Region

#Region "STEP 1 - CSV"
    Public Function AvailableColumns() As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewImportUsersFromCSV.AvailableColumns
        If Me.CTRLcsvSelector.isValid Then
            Return Me.CTRLcsvSelector.AvailableColumns
        Else
            Return Me.CTRLcsvSelector.GetAvailableColumns
        End If

    End Function
    Public Function GetFileContent(columns As List(Of ExternalColumnComparer(Of String, Integer))) As ExternalResource Implements IViewImportUsersFromCSV.GetFileContent
        Return Me.CTRLcsvSelector.GetFileContent(columns)
    End Function
    Public Function RetrieveFile() As lm.Comol.Core.DomainModel.Helpers.CsvFile Implements IViewImportUsersFromCSV.RetrieveFile
        Return Me.CTRLcsvSelector.RetrieveFile
    End Function
#End Region

#Region "Step 2- Fields Matcher"
    Public ReadOnly Property Fields As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewImportUsersFromCSV.Fields
        Get
            Return Me.CTRLfieldsMatcher.SourceFields
        End Get
    End Property
    Public ReadOnly Property ValidDestinationFields As Boolean Implements IViewImportUsersFromCSV.ValidDestinationFields
        Get
            Return CTRLfieldsMatcher.isValid
        End Get
    End Property
    Public Sub InitializeFieldsMatcher(sourceColumns As List(Of ExternalColumnComparer(Of String, Integer))) Implements IViewImportUsersFromCSV.InitializeFieldsMatcher
        Me.CTRLfieldsMatcher.InitializeControl(sourceColumns, GetAvailableColumns)
    End Sub

#End Region

#Region "Step 3 - Items selection "
    Private ReadOnly Property HasSelectedItems As Boolean Implements IViewImportUsersFromCSV.HasSelectedItems
        Get
            Return Me.CTRLitemsSelector.HasSelectedItems
        End Get
    End Property
    Private ReadOnly Property ItemsToSelect As Integer Implements IViewImportUsersFromCSV.ItemsToSelect
        Get
            Return Me.CTRLitemsSelector.ItemsToSelect
        End Get
    End Property
    Private ReadOnly Property SelectedItems As lm.Comol.Core.DomainModel.Helpers.ExternalResource Implements IViewImportUsersFromCSV.SelectedItems
        Get
            Return Me.CTRLitemsSelector.SelectedItems
        End Get
    End Property
    Private Sub CTRLitemsSelector_RequireDBValidation(item As DestinationItem(Of Integer), cells As List(Of ExternalCell), ByRef errors As List(Of ExternalCell)) Handles CTRLitemsSelector.RequireDBValidation
        errors = Me.CurrentPresenter.DBValidation(item, cells)
    End Sub
    Public Sub UpdateSourceItems() Implements IViewImportUsersFromCSV.UpdateSourceItems
        Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = Me.CTRLfieldsMatcher.SourceFields
        Dim noDB As New List(Of DestinationItem(Of Integer))
        noDB.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .InputType = InputType.mail})
        Me.CTRLitemsSelector.InitializeControlAfterImport(Me.CTRLcsvSelector.GetFileContent(columns), columns.Where(Function(c) c.InputType = InputType.mail).ToList(), columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.mail OrElse c.DestinationColumn.Id = ImportedUserColumn.name OrElse c.DestinationColumn.Id = ImportedUserColumn.surname).Select(Function(c) c.Number).ToList(), columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.mail).Select(Function(c) c.Number).ToList())
    End Sub
#End Region

#Region "STEP 4 - Conferma"
    Public Property isCompleted As Boolean Implements IViewImportUsersFromCSV.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
    Private Sub SetupProgressInfo(userCount As Integer) Implements IViewImportUsersFromCSV.SetupProgressInfo
        RPAimportUsers.Visible = True

        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        progress.CurrentOperationText = ""
        progress.Speed = "" ' "N/A"
        progress.PrimaryTotal = 1
        progress.PrimaryValue = 0
        progress.PrimaryPercent = 0

        progress.SecondaryTotal = userCount
        progress.SecondaryValue = 0
        progress.SecondaryPercent = 0
    End Sub

    Private Sub UpdateInternalUserCreation(userCount As Integer, userIndex As Integer, created As Boolean, displayName As String) Implements IViewImportUsersFromCSV.UpdateInternalUserCreation
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        progress.PrimaryValue = 1
        progress.CurrentOperationText = String.Format(Resource.getValue("CurrentOperationText" & "." & created.ToString), displayName)
        progress.SecondaryValue = userIndex
        progress.SecondaryPercent = Int((userIndex / userCount) * 100)
        progress.TimeEstimated = (userCount - userIndex) * 100
    End Sub
    Private Sub DisplayError(currentStep As InvitedUsersImportStep) Implements IViewImportUsersFromCSV.DisplayError

    End Sub

    Private Sub DisplayImportedItems(importedItems As Integer, itemsToImport As Integer) Implements IViewImportUsersFromCSV.DisplayImportedItems
        DisplayImportCommon(importedItems, InvitedUsersImportStep.Completed)

        Me.LBcompleteInfo.Text = IIf(importedItems = 1, Resource.getValue("Import.Completed.1"), String.Format(Resource.getValue("Import.Completed"), importedItems))
        Select Case itemsToImport
            Case 0
                Exit Select
            Case 1
                Me.LBcompleteInfo.Text &= vbCrLf & Resource.getValue("Import.ImportOther.1")
            Case Else
                Me.LBcompleteInfo.Text &= vbCrLf & String.Format(Resource.getValue("Import.ImportOther"), itemsToImport)
        End Select
    End Sub

    Public Sub DisplayImportError(importedItems As Integer, notCreatedItems As Dictionary(Of Integer, String)) Implements IViewImportUsersFromCSV.DisplayImportError
        DisplayImportCommon(importedItems, InvitedUsersImportStep.ImportWithErrors)

        Me.LBcompleteInfo.Text = IIf(importedItems = 1, Resource.getValue("ImportError.Completed.1"), String.Format(Resource.getValue("ImportError.Completed"), importedItems))

        SPNusers.Visible = (notCreatedItems.Count > 0)
        GetDisplayErrors(notCreatedItems.Select(Function(p) p.Value & "</li>").ToList, LBusers)
    End Sub

    Private Sub DisplayImportCommon(importedItems As Integer, ByVal eStep As InvitedUsersImportStep)
        Me.BTNbackBottom.Visible = (PreviousStep <> InvitedUsersImportStep.None)
        Me.BTNbackTop.Visible = (PreviousStep <> InvitedUsersImportStep.None)
        Me.BTNcompleteBottom.Visible = False
        Me.BTNcompleteTop.Visible = False
        Me.BTNnextBottom.Visible = False
        Me.BTNnextTop.Visible = False

        Me.LBstepTitle.Text = Resource.getValue("InvitedUsersImportStep." & eStep.ToString & ".Title")
        Me.LBstepDescription.Text = Resource.getValue("InvitedUsersImportStep." & eStep.ToString & ".Description")
        Me.CurrentStep = eStep
        Me.MLVwizard.SetActiveView(VIWcomplete)
    End Sub
    Private Function GetDisplayErrors(ByVal items As List(Of String), ByVal olabel As Label) As String
        Dim errors As String = ""
        errors = String.Join("<li>", items.ToArray)
        If Not String.IsNullOrEmpty(errors) Then
            olabel.Text = "<ul>" & errors & "</ul>"
        Else
            olabel.Text = ""
        End If
    End Function

    Public Property PreviousStep As InvitedUsersImportStep Implements IViewImportUsersFromCSV.PreviousStep
        Get
            Return ViewStateOrDefault("PreviousStep", InvitedUsersImportStep.None)
        End Get
        Set(value As InvitedUsersImportStep)
            ViewState("PreviousStep") = value
        End Set
    End Property
#End Region


    Public Sub DisplayNoPermission() Implements IViewImportUsersFromCSV.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewImportUsersFromCSV.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        If PreloadedIdQuestionnnaire > 0 Then
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
            dto.DestinationUrl = "Questionari/ImportUsersFromCSV.aspx?IdQ=" & PreloadedIdQuestionnnaire
            Dim groupItem As QuestionarioGruppo = Nothing
            Dim quest As Questionario = GetQuestionnaireById(PreloadedIdQuestionnnaire, groupItem)
            If Not IsNothing(groupItem) Then
                dto.IdCommunity = groupItem.idComunita
            End If
        Else
            dto.Display = dtoExpiredAccessUrl.DisplayMode.None
        End If
        webPost.Redirect(dto)

    End Sub

    Private Function GetAvailableColumns() As List(Of DestinationItem(Of Integer))
        Dim items As New List(Of DestinationItem(Of Integer))

        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.skip, .Name = Resource.getValue("ImportedUserColumn." & ImportedUserColumn.skip.ToString), .InputType = InputType.skip})
        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.name, .ColumnName = ImportedUserColumn.name.ToString, .Name = Resource.getValue("ImportedUserColumn." & ImportedUserColumn.name.ToString), .InputType = InputType.strings, .Mandatory = True})
        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.surname, .ColumnName = ImportedUserColumn.surname.ToString, .Name = Resource.getValue("ImportedUserColumn." & ImportedUserColumn.surname.ToString), .InputType = InputType.strings, .Mandatory = True})
        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.description, .ColumnName = ImportedUserColumn.description.ToString, .Name = Resource.getValue("ImportedUserColumn." & ImportedUserColumn.description.ToString), .InputType = InputType.strings})
        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .ColumnName = ImportedUserColumn.mail.ToString, .Name = Resource.getValue("ImportedUserColumn." & ImportedUserColumn.mail.ToString), .InputType = InputType.mail, .Mandatory = True})

        Return items
    End Function
    'Private Function GetRequiredColumns() As List(Of DestinationItem(Of Integer))
    '    Dim items As New List(Of DestinationItem(Of Integer))

    '    items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.name, .InputType = InputType.strings})
    '    items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.surname, .InputType = InputType.strings})
    '    items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .InputType = InputType.mail})
    '    Return items
    'End Function
#End Region

    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class