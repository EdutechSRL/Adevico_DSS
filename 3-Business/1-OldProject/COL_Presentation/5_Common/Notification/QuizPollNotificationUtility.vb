Imports COL_BusinessLogic_v2.UCServices
Imports lm.Notification.DataContract.Domain

Public Class QuizPollNotificationUtility
    Inherits BaseNotificationUtility

    Sub New(ByVal oUtility As OLDpageUtility)
        MyBase.New(oUtility)
    End Sub

    Public Sub NotifyAddQuestionario(ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAdd(COL_Questionario.ModuleQuestionnaire.ActionType.CreateQuestionario, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddPoll(ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAdd(COL_Questionario.ModuleQuestionnaire.ActionType.CreateSondaggio, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddSelfTest(ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAdd(COL_Questionario.ModuleQuestionnaire.ActionType.CreateTestAutovalutazione, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddMeeting(ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAdd(COL_Questionario.ModuleQuestionnaire.ActionType.CreateMeetingPoll, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub

    Public Sub NotifyAddQuestionarioToPerson(ByVal oPersons As List(Of Integer), ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAddToPerson(oPersons, COL_Questionario.ModuleQuestionnaire.ActionType.CreateInviteToQuestionario, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddPollToPerson(ByVal oPersons As List(Of Integer), ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAddToPerson(oPersons, COL_Questionario.ModuleQuestionnaire.ActionType.CreateInviteToPoll, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddSelfTestToPerson(ByVal oPersons As List(Of Integer), ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAddToPerson(oPersons, COL_Questionario.ModuleQuestionnaire.ActionType.CreateInviteToTestAutovalutazione, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub
    Public Sub NotifyAddMeetingToPerson(ByVal oPersons As List(Of Integer), ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String)
        Dim oDto As dtoNotificatedObject = Me.CreateQuestionario(QuestionarioID)
        Me.NotifyBaseAddToPerson(oPersons, COL_Questionario.ModuleQuestionnaire.ActionType.CreateInviteToMeetingPoll, CommunityID, QuestionarioID, oStartCompile, oEndCompile, QuestionarioUrl, oDto)
    End Sub

    Private Sub NotifyBaseAddToPerson(ByVal oPersons As List(Of Integer), ByVal oType As COL_Questionario.ModuleQuestionnaire.ActionType, ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, QuestionarioUrl))
        oValues.Add(FormatDateTime(oStartCompile, DateFormat.ShortDate) & " " & FormatDateTime(oStartCompile, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(oEndCompile, DateFormat.ShortDate) & " " & FormatDateTime(oEndCompile, DateFormat.ShortTime))

        _Utility.SendNotificationToPerson(oPersons, oType, CommunityID, Services_Questionario.Codex, oValues, oDto)
    End Sub

    Private Sub NotifyBaseAdd(ByVal oType As COL_Questionario.ModuleQuestionnaire.ActionType, ByVal CommunityID As Integer, ByVal QuestionarioID As Integer, ByVal oStartCompile As DateTime, ByVal oEndCompile As DateTime, ByVal QuestionarioUrl As String, ByVal oDto As dtoNotificatedObject)
        Dim oValues = New List(Of String)
        oValues.Add(MyBase.ServiceLoaderPage(CommunityID, QuestionarioUrl))
        oValues.Add(FormatDateTime(oStartCompile, DateFormat.ShortDate) & " " & FormatDateTime(oStartCompile, DateFormat.ShortTime))
        oValues.Add(FormatDateTime(oEndCompile, DateFormat.ShortDate) & " " & FormatDateTime(oEndCompile, DateFormat.ShortTime))

        _Utility.SendNotificationToPermission(Me.PermissionToSee, oType, CommunityID, Services_Questionario.Codex, oValues, oDto)
    End Sub

#Region "Object To Notify"
    Private Function CreateQuestion(ByVal QuestionID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(QuestionID, Services_Questionario.ObjectType.Domanda)
    End Function
    Private Function CreateQuestionario(ByVal QuestionarioID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(QuestionarioID, Services_Questionario.ObjectType.Questionario)
    End Function
    Private Function CreateLibrary(ByVal LibraryID As Integer) As dtoNotificatedObject
        Return CreateObjectToNotify(LibraryID, Services_Questionario.ObjectType.Libreria)
    End Function
    Private Function CreateObjectToNotify(ByVal ObjectID As Integer, ByVal oType As Services_Questionario.ObjectType) As dtoNotificatedObject
        Dim obj As New dtoNotificatedObject
        obj.ObjectID = ObjectID.ToString
        obj.ObjectTypeID = oType
        obj.ModuleCode = Services_Questionario.Codex
        obj.ModuleID = MyBase._Utility.GetModuleID(Services_Questionario.Codex)
        Select Case oType
            Case Services_Questionario.ObjectType.Questionario
                obj.FullyQualiFiedName = GetType(COL_Questionario.Questionario).FullName
            Case Services_Questionario.ObjectType.Sondaggio
                obj.FullyQualiFiedName = GetType(COL_Questionario.Questionario).FullName
            Case Services_Questionario.ObjectType.Domanda
                obj.FullyQualiFiedName = GetType(COL_Questionario.Domanda).FullName
            Case Services_Questionario.ObjectType.Libreria
                obj.FullyQualiFiedName = GetType(COL_Questionario.LibreriaQuestionario).FullName
        End Select
        Return obj
    End Function
#End Region

#Region "Permission Utility"
    Public Function PermissionToSee() As Integer
        Return Services_Questionario.Base2Permission.Compila
    End Function
    Public Function PermissionToAdmin() As Integer
        Return Services_Questionario.Base2Permission.AdminService
    End Function
#End Region
End Class