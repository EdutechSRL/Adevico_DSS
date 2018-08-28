Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports COL_Questionario.Business

Public Class ImportUsersFromCSVpresenter
    Inherits lm.Comol.Core.DomainModel.Common.DomainPresenter
#Region "Initialize"
    Private _ModuleID As Integer
    Private _Service As ServiceQuestionnaire
    'private int ModuleID
    '{
    '    get
    '    {
    '        if (_ModuleID <= 0)
    '        {
    '            _ModuleID = this.Service.ServiceModuleID();
    '        }
    '        return _ModuleID;
    '    }
    '}
    Public Overridable Property CurrentManager() As BaseModuleManager
        Get
            Return m_CurrentManager
        End Get
        Set(value As BaseModuleManager)
            m_CurrentManager = value
        End Set
    End Property
    Private m_CurrentManager As BaseModuleManager
    Protected Overridable ReadOnly Property View() As IViewImportUsersFromCSV
        Get
            Return DirectCast(MyBase.View, IViewImportUsersFromCSV)
        End Get
    End Property
    Private ReadOnly Property Service() As ServiceQuestionnaire
        Get
            If _Service Is Nothing Then
                _Service = New ServiceQuestionnaire(AppContext)
            End If
            Return _Service
        End Get
    End Property
    Public Sub New(oContext As iApplicationContext)
        MyBase.New(oContext)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
    Public Sub New(oContext As iApplicationContext, view As IViewImportUsersFromCSV)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        Else
            Dim idQuest As Integer = View.PreloadedIdQuestionnnaire
            Dim idSessionQuest As Integer = View.SessionIdQuestionnnaire
            Dim quest As Questionario = Nothing
            Dim hasPermission As Boolean = View.HasPermission
            If idQuest = 0 Then
                idQuest = idSessionQuest
            ElseIf idSessionQuest <> idQuest Then
                quest = View.LoadQuestionnaire(idQuest)
                Dim group As QuestionarioGruppo = DALQuestionarioGruppo.readGruppoBYID(quest.idGruppo)
                If quest.ownerType = COL_BusinessLogic_v2.OwnerType_enum.None Then

                End If
            End If
            View.AllowManagement = hasPermission
            If hasPermission AndAlso idQuest > 0 Then
                View.CurrentIdQuestionnnaire = idQuest
                View.PreviousStep = InvitedUsersImportStep.None
                View.ImportIdentifier = System.Guid.NewGuid()
                InitializeSteps()
                View.GotoStep(InvitedUsersImportStep.SourceCSV, True)
            Else
                View.DisplayNoPermission()
            End If
        End If
    End Sub

    Private Sub InitializeSteps()
        Dim steps As New List(Of InvitedUsersImportStep)()
        steps.Add(InvitedUsersImportStep.SourceCSV)
        steps.Add(InvitedUsersImportStep.FieldsMatcher)
        steps.Add(InvitedUsersImportStep.ItemsSelctor)
        steps.Add(InvitedUsersImportStep.Summary)

        View.AvailableSteps = steps
        View.SkipSteps = New List(Of InvitedUsersImportStep)
    End Sub

    Public Sub MoveToNextStep(pStep As InvitedUsersImportStep)
        Select Case pStep
            Case InvitedUsersImportStep.SourceCSV
                MoveFromSelectedSource()
                Exit Select
                Exit Select
            Case InvitedUsersImportStep.FieldsMatcher
                MoveFromFieldsMatcher()
                Exit Select
            Case InvitedUsersImportStep.ItemsSelctor
                MoveFromItemsSelector()
                Exit Select
            Case InvitedUsersImportStep.Summary
                Exit Select
        End Select
    End Sub
    Public Sub MoveToPreviousStep(pStep As InvitedUsersImportStep)
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        Else
            Select Case pStep
                Case InvitedUsersImportStep.FieldsMatcher
                    View.GotoStep(InvitedUsersImportStep.SourceCSV)
                Case InvitedUsersImportStep.ItemsSelctor
                    View.GotoStep(InvitedUsersImportStep.FieldsMatcher)
                    Exit Select
                Case InvitedUsersImportStep.Summary
                    View.GotoStep(InvitedUsersImportStep.ItemsSelctor)
                Case InvitedUsersImportStep.Completed
                    If View.ItemsToSelect > 0 AndAlso View.PreviousStep <> InvitedUsersImportStep.None Then
                        View.GotoStep(View.PreviousStep)
                    Else
                        View.GotoStep(InvitedUsersImportStep.SourceCSV, True)
                    End If
                    View.PreviousStep = InvitedUsersImportStep.None
                    Exit Select
                Case InvitedUsersImportStep.ImportWithErrors
                    If View.ItemsToSelect > 0 AndAlso View.PreviousStep <> InvitedUsersImportStep.None Then
                        View.GotoStep(View.PreviousStep)
                    Else
                        View.GotoStep(InvitedUsersImportStep.SourceCSV, True)
                    End If
                    View.PreviousStep = InvitedUsersImportStep.None
                    Exit Select
                Case InvitedUsersImportStep.Errors
                    View.GotoStep(View.PreviousStep)
            End Select
        End If
    End Sub

    Private Sub MoveFromSelectedSource()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        Else
            Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = View.AvailableColumns()
            If columns.Count > 0 Then
                View.InitializeFieldsMatcher(columns)
                View.GotoStep(InvitedUsersImportStep.FieldsMatcher, False)
            End If
        End If
    End Sub
    Private Sub MoveFromFieldsMatcher()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        ElseIf View.ValidDestinationFields Then
            View.GotoStep(InvitedUsersImportStep.ItemsSelctor, True)
        End If
    End Sub
    Private Sub MoveFromItemsSelector()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        ElseIf View.HasSelectedItems Then
            View.GotoStep(InvitedUsersImportStep.Summary, True)
        End If
    End Sub
    Private Sub UpdateStepsToSkip(pStep As InvitedUsersImportStep, add As Boolean)
        Dim toSkip As List(Of InvitedUsersImportStep) = View.SkipSteps
        If add AndAlso Not toSkip.Contains(pStep) Then
            toSkip.Add(pStep)
        ElseIf Not add AndAlso toSkip.Contains(pStep) Then
            toSkip.Remove(pStep)
        End If
        View.SkipSteps = toSkip
    End Sub

    Public Sub ImportItems(ByVal selectedItems As ExternalResource)
        View.PreviousStep = InvitedUsersImportStep.None
        View.SetupProgressInfo(selectedItems.Rows.Count)

        Dim notAddedItems As New Dictionary(Of Int32, String)
        View.SetupProgressInfo(selectedItems.Rows.Count)
        Dim addedUsers As List(Of LazyExternalInvitedUser) = AddInvitedUsers(selectedItems, notAddedItems)

        View.UpdateSourceItems()
        Dim itemsToSelect As Integer = View.ItemsToSelect
        View.PreviousStep = IIf(itemsToSelect > 0, InvitedUsersImportStep.ItemsSelctor, InvitedUsersImportStep.None)
        If (notAddedItems.Count = 0) Then
            View.isCompleted = (itemsToSelect = 0)
            View.DisplayImportedItems(addedUsers.Count, itemsToSelect)
        Else
            View.DisplayImportError(addedUsers.Count, notAddedItems)
        End If
    End Sub

    Private Function AddInvitedUsers(ByVal selectedItems As ExternalResource, ByRef notAddedItems As Dictionary(Of Int32, String)) As List(Of LazyExternalInvitedUser)
        Dim list As New List(Of LazyExternalInvitedUser)
        Dim idQuestionnnaire As Integer = View.CurrentIdQuestionnnaire
        Dim columns As List(Of ExternalColumnComparer(Of String, Integer)) = View.Fields()
        Dim created As Boolean = False
        Dim index As Integer = 1
        For Each row As ExternalRow In selectedItems.Rows
            Dim user As New LazyExternalInvitedUser With {.IdQuestionnnaire = idQuestionnnaire}
            With user
                .Description = row.GetCellValue(columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.description).Select(Function(c) c.Number).FirstOrDefault)
                .Mail = row.GetCellValue(columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.mail).Select(Function(c) c.Number).FirstOrDefault)
                .Name = row.GetCellValue(columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.name).Select(Function(c) c.Number).FirstOrDefault)
                .Surname = row.GetCellValue(columns.Where(Function(c) c.DestinationColumn.Id = ImportedUserColumn.surname).Select(Function(c) c.Number).FirstOrDefault)
                .Password = RandomPasswordGenerator(6)
            End With
            created = Not IsNothing(Service.AddExternalInvitedUser(idQuestionnnaire, user))
            If created Then : list.Add(user)
            Else : notAddedItems.Add(row.Number, user.Surname & " " & user.Name)
            End If

            View.UpdateInternalUserCreation(selectedItems.Rows.Count, index, created, user.Surname & " " & user.Name)
            index += 1
        Next
        Return list
    End Function
    Public Shared Function RandomPasswordGenerator(ByRef lenght As Integer) As String
        Dim n As System.Security.Cryptography.RandomNumberGenerator = _
        System.Security.Cryptography.RandomNumberGenerator.Create
        Dim Symbol(0) As Byte
        Dim SConverter As New System.Text.ASCIIEncoding
        Dim result As String = ""

        Do While (result.Length < lenght)
            n.GetBytes(Symbol)
            Dim st As String = SConverter.GetString(Symbol)
            If Char.IsLetterOrDigit(st(0)) Then result &= st(0)
        Loop

        Return result.ToLower
    End Function
    Public Function DBValidation(item As DestinationItem(Of Integer), cells As List(Of ExternalCell)) As List(Of ExternalCell)
        Dim errors As New List(Of ExternalCell)
        Dim idQuestionnnaire As Integer = View.CurrentIdQuestionnnaire
        If item.Id = ImportedUserColumn.mail Then
            For Each cell As ExternalCell In cells
                If (Service.ExistExternalInvitedUser(idQuestionnnaire, cell.Value)) Then
                    errors.Add(cell)
                End If
            Next
        End If

        Return errors
    End Function
End Class