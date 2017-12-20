Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.DomainModel.Helpers

Public Class ImportAgencies
    Inherits PageBase
    Implements IViewImportAgencies


#Region "Context"
    Private _Presenter As AgenciesImportPresenter
    Private ReadOnly Property CurrentPresenter() As AgenciesImportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AgenciesImportPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowManagement As Boolean Implements IViewImportAgencies.AllowManagement
        Set(value As Boolean)
            Me.HYPbackToManagement.Visible = value
            Me.HYPbackToManagement.NavigateUrl = BaseUrl & RootObject.ManagementAgenciesWithFilters()
        End Set
    End Property
    Private Property CurrentStep As AgencyImportStep Implements IViewImportAgencies.CurrentStep
        Get
            Return ViewStateOrDefault("CurrentStep", AgencyImportStep.None)
        End Get
        Set(value As AgencyImportStep)
            ViewState("CurrentStep") = value
        End Set
    End Property
    Private Property SkipSteps As List(Of AgencyImportStep) Implements IViewImportAgencies.SkipSteps
        Get
            Return ViewStateOrDefault("SkipSteps", New List(Of AgencyImportStep))
        End Get
        Set(value As List(Of AgencyImportStep))
            ViewState("SkipSteps") = value
        End Set
    End Property
    Private Property AvailableSteps As List(Of AgencyImportStep) Implements IViewImportAgencies.AvailableSteps
        Get
            Return ViewStateOrDefault("AvailableSteps", New List(Of AgencyImportStep))
        End Get
        Set(value As List(Of AgencyImportStep))
            ViewState("AvailableSteps") = value
        End Set
    End Property
    Private Property ImportIdentifier As System.Guid Implements IViewImportAgencies.ImportIdentifier
        Get
            Return ViewStateOrDefault("ImportIdentifier", System.Guid.NewGuid)
        End Get
        Set(value As System.Guid)
            ViewState("ImportIdentifier") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            RPAimportAgencies.ProgressIndicators = RPAimportAgencies.ProgressIndicators And Not Telerik.Web.UI.Upload.ProgressIndicators.SelectedFilesCount
        End If
        RPAimportAgencies.Localization.Total = Resource.getValue("Localization.Total")
        RPAimportAgencies.Localization.TotalFiles = Resource.getValue("Localization.TotalAgencies")
        RPAimportAgencies.Localization.ElapsedTime = Resource.getValue("Localization.ElapsedTime")
        RPAimportAgencies.Localization.EstimatedTime = Resource.getValue("Localization.EstimatedTime")
        RPAimportAgencies.Localization.TransferSpeed = Resource.getValue("Localization.TransferSpeed")


        RPAimportAgencies.Localization.Uploaded = Resource.getValue("Localization.Uploaded")
        RPAimportAgencies.Localization.UploadedFiles = Resource.getValue("Localization.UploadedAgencies")
        RPAimportAgencies.Localization.CurrentFileName = Resource.getValue("Localization.CurrentAgencyName")
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
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AgencyManagement", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleImportAgency")
            Me.Master.ServiceNopermission = .getValue("serviceTitleImportAgencyNopermission")
            .setHyperLink(HYPbackToManagement, True, True)

            .setButton(BTNbackBottom, True, , , True)
            .setButton(BTNbackTop, True, , , True)
            .setButton(BTNnextBottom, True, , , True)
            .setButton(BTNnextTop, True, , , True)
            .setButton(BTNcompleteTop, True, , , True)
            .setButton(BTNcompleteBottom, True, , , True)
            .setLiteral(LTprogress)
            .setLabel(LBimportedAgenciesErrors_t)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"

#Region "Move Wizard"
    Private Sub GotoStep(pStep As AgencyImportStep) Implements IViewImportAgencies.GotoStep
        GotoStep(pStep, False)
    End Sub
    Private Sub GotoStep(pStep As AgencyImportStep, initialize As Boolean) Implements IViewImportAgencies.GotoStep
        Me.MLVimport.SetActiveView(VIWwizard)
        Select Case pStep
            Case AgencyImportStep.SourceCSV
                If initialize Then : InitializeStep(pStep)
                End If
                Me.CTRLfieldsMatcher.isInitialized = False
                '   Me.CTRLitemsSelector.isInitialized = False
                MLVwizard.SetActiveView(VIWsourceCSV)
            Case AgencyImportStep.FieldsMatcher
                If initialize AndAlso Not IsInitialized(pStep) Then
                Else
                    MLVwizard.SetActiveView(VIWfieldsMatcher)
                End If
            Case AgencyImportStep.ItemsSelctor
                If initialize AndAlso Not IsInitialized(pStep) Then
                    InitializeStep(AgencyImportStep.ItemsSelctor)
                End If
                MLVwizard.SetActiveView(VIWitemsSelector)
            Case AgencyImportStep.OrganizationAvailability
                If initialize AndAlso Not IsInitialized(pStep) Then
                    InitializeStep(AgencyImportStep.OrganizationAvailability)
                End If
                MLVwizard.SetActiveView(VIWorganizationAvailability)
            Case AgencyImportStep.Summary
                Dim total As Integer = Me.CTRLitemsSelector.SelectedItemsCount
                If total <= 1 Then
                    Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = Me.CTRLfieldsMatcher.SourceFields
                    Dim index As Integer = columns.Where(Function(cc) cc.DestinationColumn.Id = ImportedAgencyColumn.name).Select(Function(c) c.Number).ToList().FirstOrDefault()

                    LBsummaryAgency_t.Text = Resource.getValue("Summary.0")
                    LBsummaryAgency.Text = Me.CTRLitemsSelector.SelectedItems.Rows.FirstOrDefault().Cells.Where(Function(c) index = c.Column.Number).FirstOrDefault.Value
                    LBsummaryOrganization_t.Text = Resource.getValue("Summary.Organizations.0")
                Else
                    LBsummaryAgency_t.Text = Resource.getValue("Summary")
                    LBsummaryAgency.Text = total.ToString
                    LBsummaryOrganization_t.Text = Resource.getValue("Summary.Organizations")
                End If


                If AvailableForAll Then
                    LBsummaryOrganization.Text = Resource.getValue("Summary.Platform")
                ElseIf SelectedOrganizations.Count = 0 Then
                    LBsummaryOrganization.Text = SelectedOrganizations.FirstOrDefault.Value
                Else
                    LBsummaryOrganization.Text = String.Join(", ", SelectedOrganizations.Select(Function(o) o.Value).OrderBy(Function(o) o).ToArray())
                End If
                'If total = 1 Then
                '    Me.LBsummary.Text = String.Format(Resource.getValue("LBsummary.text.0"), QuestionarioCorrente.nome)
                'Else
                '    Me.LBsummary.Text = String.Format(Resource.getValue("LBsummary.text"), QuestionarioCorrente.nome, total.ToString)
                'End If


                MLVwizard.SetActiveView(VIWsummary)
                Me.LBstepTitle.Text = Resource.getValue("AgencyImportStep." & Me.CurrentStep.ToString & ".Title")
                Me.LBstepDescription.Text = Resource.getValue("AgencyImportStep." & pStep.ToString & ".Description")
                If initialize Then : InitializeStep(pStep)
                End If
        End Select

        Me.CurrentStep = pStep
        Me.BTNbackBottom.Visible = (pStep <> AgencyImportStep.SourceCSV)
        Me.BTNbackTop.Visible = (pStep <> AgencyImportStep.SourceCSV)
        Me.BTNcompleteBottom.Visible = (pStep = AgencyImportStep.Summary)
        Me.BTNcompleteTop.Visible = (pStep = AgencyImportStep.Summary)
        Me.BTNnextBottom.Visible = (pStep <> AgencyImportStep.Summary AndAlso pStep <> AgencyImportStep.Errors AndAlso pStep <> AgencyImportStep.ImportCompleted AndAlso pStep <> AgencyImportStep.ImportWithErrors)
        Me.BTNnextTop.Visible = (pStep <> AgencyImportStep.Summary AndAlso pStep <> AgencyImportStep.Errors AndAlso pStep <> AgencyImportStep.ImportCompleted AndAlso pStep <> AgencyImportStep.ImportWithErrors)

        If pStep <> AgencyImportStep.Errors AndAlso pStep <> AgencyImportStep.ImportCompleted AndAlso pStep <> AgencyImportStep.ImportWithErrors Then
            Me.LBstepTitle.Text = String.Format(Resource.getValue("AgencyImportStep." & Me.CurrentStep.ToString & ".Title"), GetStepNumber(Me.CurrentStep))
            Me.LBstepDescription.Text = Resource.getValue("AgencyImportStep." & pStep.ToString & ".Description")
        End If
    End Sub
    Private Sub InitializeStep(pStep As AgencyImportStep) Implements IViewImportAgencies.InitializeStep
        Select Case pStep
            Case AgencyImportStep.SourceCSV
                CTRLcsvSelector.CSVfolder = "/AgencyImport/" & "/" & PageUtility.CurrentUser.ID & "/"
                Me.CTRLcsvSelector.InitializeControl()
            Case AgencyImportStep.FieldsMatcher
                Me.CTRLfieldsMatcher.InitializeControl(AvailableColumns, GetAvailableColumns)
            Case AgencyImportStep.ItemsSelctor
                Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = Me.CTRLfieldsMatcher.SourceFields
                Me.CTRLitemsSelector.InitializeControl(Me.CTRLcsvSelector.GetFileContent(columns), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList(), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList(), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList())
            Case AgencyImportStep.OrganizationAvailability
                Me.CTRLorganizations.InitializeControl()
            Case AgencyImportStep.Summary

                'If Not IsInitialized(pStep) Then
                '    Me.CTRLsummary.InitializeControl(Me.CTRLfieldsMatcher.SelectedAuthenticationTypeName, Me.CTRLfieldsMatcher.SelectedProfileTypeName, Me.CTRLmailTemplate.SendMailToUsers, PrimaryOrganizationName, OtherOrganizationsToSubscribe, Me.CTRLsubscriptions.SelectedSubscriptions, Me.CTRLitemsSelector.SelectedItemsCount)
                'End If
        End Select
    End Sub
    Private Function IsInitialized(pStep As AgencyImportStep) As Boolean Implements IViewImportAgencies.IsInitialized
        Select Case pStep
            Case AgencyImportStep.SourceCSV
                Return Me.CTRLcsvSelector.isInitialized
            Case AgencyImportStep.FieldsMatcher
                Return Me.CTRLfieldsMatcher.isInitialized
            Case AgencyImportStep.ItemsSelctor
                Return Me.CTRLitemsSelector.isInitialized
            Case AgencyImportStep.OrganizationAvailability
                Return Me.CTRLorganizations.isInitialized
                'Case AgencyImportStep.Summary
                '    Return Me.CTRLsummary.isInitialized
            Case Else
                Return True
        End Select
    End Function
    Private Function GetStepNumber(pStep As AgencyImportStep) As Integer
        Dim Number As Integer = 1
        Dim list As List(Of AgencyImportStep) = Me.AvailableSteps
        Dim toSkip As List(Of AgencyImportStep) = Me.SkipSteps
        For Each item As AgencyImportStep In (From i In list Where toSkip.Contains(i) = False Select i).ToList
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
        '    Me.CurrentStep = AgencyImportStep.Summary
        '    Me.LBstepTitle.Text = String.Format(Resource.getValue("AgencyImportStep." & AgencyImportStep.Summary.ToString & ".Title"), GetStepNumber(AgencyImportStep.Summary))
        '    Me.LBstepDescription.Text = Resource.getValue("AgencyImportStep." & AgencyImportStep.Summary.ToString & ".Description")
        'Else
        Me.CurrentPresenter.MoveToPreviousStep(CurrentStep)
        ' End If
    End Sub
    Private Sub BTNcompleteBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcompleteBottom.Click, BTNcompleteTop.Click
        If Me.isCompleted Then
            Me.PageUtility.RedirectToUrl(BaseUrl & RootObject.ManagementAgenciesWithFilters())
        Else
            Me.CurrentPresenter.ImportAgencies(CTRLitemsSelector.SelectedItems)
        End If
    End Sub
    Private Sub BTNnextBottom_Click(sender As Object, e As System.EventArgs) Handles BTNnextBottom.Click, BTNnextTop.Click
        Me.CurrentPresenter.MoveToNextStep(CurrentStep)
    End Sub
#End Region

#Region "STEP 1 - CSV"
    Private Function AvailableColumns() As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewImportAgencies.AvailableColumns
        If Me.CTRLcsvSelector.isValid Then
            Return Me.CTRLcsvSelector.AvailableColumns
        Else
            Return Me.CTRLcsvSelector.GetAvailableColumns
        End If
    End Function
    Private Function GetFileContent(columns As List(Of ExternalColumnComparer(Of String, Integer))) As ExternalResource Implements IViewImportAgencies.GetFileContent
        Return Me.CTRLcsvSelector.GetFileContent(columns)
    End Function
    Private Function RetrieveFile() As lm.Comol.Core.DomainModel.Helpers.CsvFile Implements IViewImportAgencies.RetrieveFile
        Return Me.CTRLcsvSelector.RetrieveFile
    End Function
#End Region

#Region "Step 2- Fields Matcher"
    Private ReadOnly Property Fields As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewImportAgencies.Fields
        Get
            Return Me.CTRLfieldsMatcher.SourceFields
        End Get
    End Property
    Private ReadOnly Property ValidDestinationFields As Boolean Implements IViewImportAgencies.ValidDestinationFields
        Get
            Return CTRLfieldsMatcher.isValid
        End Get
    End Property
    Private Sub InitializeFieldsMatcher(sourceColumns As List(Of ExternalColumnComparer(Of String, Integer))) Implements IViewImportAgencies.InitializeFieldsMatcher
        Me.CTRLfieldsMatcher.InitializeControl(sourceColumns, GetAvailableColumns)
    End Sub

#End Region

#Region "Step 3 - Items selection "
    Private ReadOnly Property HasSelectedItems As Boolean Implements IViewImportAgencies.HasSelectedItems
        Get
            Return Me.CTRLitemsSelector.HasSelectedItems
        End Get
    End Property
    Private ReadOnly Property ItemsToSelect As Integer Implements IViewImportAgencies.ItemsToSelect
        Get
            Return Me.CTRLitemsSelector.ItemsToSelect
        End Get
    End Property
    Private ReadOnly Property SelectedItems As lm.Comol.Core.DomainModel.Helpers.ExternalResource Implements IViewImportAgencies.SelectedItems
        Get
            Return Me.CTRLitemsSelector.SelectedItems
        End Get
    End Property
    Private Sub CTRLitemsSelector_RequireDBValidation(item As DestinationItem(Of Integer), cells As List(Of ExternalCell), ByRef errors As List(Of ExternalCell)) Handles CTRLitemsSelector.RequireDBValidation
        errors = Me.CurrentPresenter.DBValidation(item, cells)
    End Sub

    Private Sub CTRLitemsSelector_RequireDBValidationAlternativeItems(items As List(Of DestinationItemCells(Of Integer)), ByRef errors As List(Of ExternalCell)) Handles CTRLitemsSelector.RequireDBValidationAlternativeItems
        errors = Me.CurrentPresenter.DBValidation(items)
    End Sub

    Public Sub UpdateSourceItems() Implements IViewImportAgencies.UpdateSourceItems
        Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = Me.CTRLfieldsMatcher.SourceFields
        'Dim noDB As New List(Of DestinationItem(Of Integer))
        'noDB.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .InputType = InputType.mail})
        Me.CTRLitemsSelector.InitializeControlAfterImport(Me.CTRLcsvSelector.GetFileContent(columns), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList(), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList(), columns.Where(Function(c) c.DestinationColumn.Id <> ImportedAgencyColumn.skip).ToList())
    End Sub
#End Region

#Region "Step 4 - Organizations "
    Private ReadOnly Property AvailableForAll As Boolean Implements IViewImportAgencies.AvailableForAll
        Get
            Return Me.CTRLorganizations.AvailableForAll
        End Get
    End Property
    Private ReadOnly Property SelectedOrganizations As Dictionary(Of Integer, String) Implements IViewImportAgencies.SelectedOrganizations
        Get
            Return Me.CTRLorganizations.SelectedOrganizations
        End Get
    End Property
    Private ReadOnly Property HasAvailableOrganizations As Boolean Implements IViewImportAgencies.HasAvailableOrganizations
        Get
            Return Me.CTRLorganizations.HasAvailableOrganizations
        End Get
    End Property
#End Region

#Region "STEP 5 - Conferma"
    Public Property isCompleted As Boolean Implements IViewImportAgencies.isCompleted
        Get
            Return ViewStateOrDefault("isCompleted", False)
        End Get
        Set(value As Boolean)
            ViewState("isCompleted") = value
        End Set
    End Property
    Private Sub SetupProgressInfo(userCount As Integer) Implements IViewImportAgencies.SetupProgressInfo
        RPAimportAgencies.Visible = True

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

    Private Sub UpdateAgencyCreation(agencyCount As Integer, agencyIndex As Integer, created As Boolean, displayName As String) Implements IViewImportAgencies.UpdateAgencyCreation
        Dim progress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
        progress.PrimaryValue = 1
        progress.CurrentOperationText = String.Format(Resource.getValue("CurrentOperationText" & "." & created.ToString), displayName)
        progress.SecondaryValue = agencyIndex
        progress.SecondaryPercent = Int((agencyIndex / agencyCount) * 100)
        progress.TimeEstimated = (agencyCount - agencyIndex) * 100
    End Sub
    Private Sub DisplayError(currentStep As AgencyImportStep) Implements IViewImportAgencies.DisplayError

    End Sub

    Private Sub DisplayImportedAgencies(importedItems As Integer, itemsToImport As Integer) Implements IViewImportAgencies.DisplayImportedAgencies
        DisplayImportCommon(importedItems, AgencyImportStep.ImportCompleted)

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

    Public Sub DisplayImportError(importedItems As Integer, notCreatedItems As list(Of String)) Implements IViewImportAgencies.DisplayImportError
        DisplayImportCommon(importedItems, AgencyImportStep.ImportWithErrors)

        Me.LBcompleteInfo.Text = IIf(importedItems = 1, Resource.getValue("ImportError.Completed.1"), String.Format(Resource.getValue("ImportError.Completed"), importedItems))

        SPNagencies.Visible = (notCreatedItems.Count > 0)
        GetDisplayErrors(notCreatedItems.Select(Function(p) p & "</li>").ToList, LBimportedAgencies)
    End Sub

    Private Sub DisplayImportCommon(importedItems As Integer, ByVal eStep As AgencyImportStep)
        Me.BTNbackBottom.Visible = (PreviousStep <> AgencyImportStep.None)
        Me.BTNbackTop.Visible = (PreviousStep <> AgencyImportStep.None)
        Me.BTNcompleteBottom.Visible = False
        Me.BTNcompleteTop.Visible = False
        Me.BTNnextBottom.Visible = False
        Me.BTNnextTop.Visible = False

        Me.LBstepTitle.Text = Resource.getValue("AgencyImportStep." & eStep.ToString & ".Title")
        Me.LBstepDescription.Text = Resource.getValue("AgencyImportStep." & eStep.ToString & ".Description")
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

    Public Property PreviousStep As AgencyImportStep Implements IViewImportAgencies.PreviousStep
        Get
            Return ViewStateOrDefault("PreviousStep", AgencyImportStep.None)
        End Get
        Set(value As AgencyImportStep)
            ViewState("PreviousStep") = value
        End Set
    End Property
#End Region


    Public Sub DisplayNoPermission() Implements IViewImportAgencies.DisplayNoPermission
        Master.ShowNoPermission = True
    End Sub

    Public Sub DisplaySessionTimeout() Implements IViewImportAgencies.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()

        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.ImportAgencies()

        webPost.Redirect(dto)

    End Sub

    Private Function GetAvailableColumns() As List(Of DestinationItem(Of Integer))
        Dim items As New List(Of DestinationItem(Of Integer))

        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedAgencyColumn.skip, .ColumnName = ImportedAgencyColumn.skip.ToString, .Name = Resource.getValue("ImportedAgencyColumn." & ImportedAgencyColumn.skip.ToString), .InputType = InputType.skip})

        items.Add(New DestinationItem(Of Integer) With {.Id = ImportedAgencyColumn.name, .ColumnName = ImportedAgencyColumn.name.ToString, .Name = Resource.getValue("ImportedAgencyColumn." & ImportedAgencyColumn.name.ToString), .InputType = InputType.strings, .Mandatory = True})

        Dim alternatives As New List(Of ImportedAgencyColumn)
        alternatives.Add(ImportedAgencyColumn.externalCode)
        alternatives.Add(ImportedAgencyColumn.nationalCode)
        alternatives.Add(ImportedAgencyColumn.taxCode)

        For Each item As ImportedAgencyColumn In alternatives
            items.Add(New DestinationItem(Of Integer) With {.Id = item, .Mandatory = True, .ColumnName = item.ToString, .Name = Resource.getValue("ImportedAgencyColumn." & item.ToString), .InputType = InputType.strings, .AlternativeAttributes = alternatives.Where(Function(p) p <> item).Select(Function(p) CInt(p)).ToList()})
        Next
        Return items
    End Function
    'Private Function GetRequiredColumns() As List(Of DestinationItem(Of Integer))
    '    Dim items As New List(Of DestinationItem(Of Integer))

    '    items.Add(New DestinationItem(Of Integer) With {.Id = lm.Comol.Core.Authentication.ProfileAttributeType.agencyName.name, .InputType = InputType.strings})
    '    items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.surname, .InputType = InputType.strings})
    '    items.Add(New DestinationItem(Of Integer) With {.Id = ImportedUserColumn.mail, .InputType = InputType.mail})
    '    Return items
    'End Function
#End Region

   
    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class