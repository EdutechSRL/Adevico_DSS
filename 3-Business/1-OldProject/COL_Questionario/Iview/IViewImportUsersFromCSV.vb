Imports lm.Comol.Core.DomainModel.Helpers

Public Interface IViewImportUsersFromCSV
    Inherits lm.Comol.Core.DomainModel.Common.iDomainView
    WriteOnly Property AllowManagement() As Boolean

    ReadOnly Property SessionIdQuestionnnaire As Integer
    ReadOnly Property PreloadedIdQuestionnnaire As Integer
    Property CurrentIdQuestionnnaire As Integer

    Property ImportIdentifier() As System.Guid
    Property CurrentStep() As InvitedUsersImportStep
    Property AvailableSteps() As List(Of InvitedUsersImportStep)
    Property SkipSteps() As List(Of InvitedUsersImportStep)

    Function HasPermission() As Boolean

    Function IsInitialized(pStep As InvitedUsersImportStep) As Boolean
    Sub GotoStep(pStep As InvitedUsersImportStep)
    Sub GotoStep(pStep As InvitedUsersImportStep, initialize As Boolean)
    Sub InitializeStep(pStep As InvitedUsersImportStep)
    Function LoadQuestionnaire(ByVal id As Integer) As Questionario

    ' STEP CSV
    Function RetrieveFile() As CsvFile
    Function GetFileContent(columns As List(Of ExternalColumnComparer(Of String, Integer))) As ExternalResource
    ' STEP Request For Membership

    ' STEP CSV && Request For Membership
    Function AvailableColumns() As List(Of ExternalColumnComparer(Of String, Integer))

    'SETP
    ReadOnly Property Fields() As List(Of ExternalColumnComparer(Of String, Integer))
    ReadOnly Property ValidDestinationFields() As Boolean
    Sub InitializeFieldsMatcher(sourceColumns As List(Of ExternalColumnComparer(Of String, Integer)))

    'Step select Items
    ReadOnly Property SelectedItems() As ExternalResource
    ReadOnly Property HasSelectedItems() As Boolean
    ReadOnly Property ItemsToSelect() As Int32
    Sub UpdateSourceItems()

    'STEP SUMMARY
    Property isCompleted() As [Boolean]
    Sub SetupProgressInfo(userCount As Int32)
    Sub UpdateInternalUserCreation(userCount As Int32, userIndex As Int32, created As Boolean, displayName As String)

    'Sub UpdateSourceItems(type As SourceType)
    'Function AddUserProfile(profile As dtoBaseProfile, info As PersonInfo, idDefaultOrganization As Int32, type As AuthenticationProviderType) As Int32

    '''/STEP SUMMARY
    Sub DisplayImportedItems(importedItems As Int32, itemsToImport As Int32)
    Sub DisplayError(currentStep As InvitedUsersImportStep)
    Sub DisplayImportError(importedItems As Int32, notCreatedItems As Dictionary(Of Int32, String))
    Property PreviousStep() As InvitedUsersImportStep

    Sub DisplaySessionTimeout()
    Sub DisplayNoPermission()
End Interface
