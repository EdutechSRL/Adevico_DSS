Imports lm.Comol.Core.DomainModel

<Serializable(), CLSCompliant(True)> Public Class ModuleQuestionnaire
    Public Const UniqueID As String = "SRVQUST"

    Public Administration As Boolean
    Public Compile As Boolean
    Public CopyQuestionnaire As Boolean
    Public DeleteQuestionnaire As Boolean
    Public GrantPermission As Boolean
    Public ManageQuestions As Boolean
    Public ManageInvitedQuestionnaire As Boolean
    Public ViewStatistics As Boolean
    Public ViewPersonalStatistics As Boolean
    Sub New()

    End Sub
    Sub New(permission As Long)
        Administration = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
        Compile = PermissionHelper.CheckPermissionSoft(Base2Permission.Compile, permission)
        ViewPersonalStatistics = PermissionHelper.CheckPermissionSoft(Base2Permission.Compile, permission)
        CopyQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.CopyQuestionnaire, permission)
        DeleteQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.DeleteQuestionnaire, permission)
        GrantPermission = PermissionHelper.CheckPermissionSoft(Base2Permission.GrantPermission, permission)
        ManageQuestions = PermissionHelper.CheckPermissionSoft(Base2Permission.ManageQuestions, permission)
        ManageInvitedQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.ManageInvitedQuestionnaire, permission)
        ViewStatistics = PermissionHelper.CheckPermissionSoft(Base2Permission.ViewStatistics, permission)
    End Sub

    Public Shared Function CreatePortalmodule(userTypeId As Integer) As ModuleQuestionnaire
        Dim moduleQ As New ModuleQuestionnaire
        With moduleQ
            .Administration = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .Compile = (userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest AndAlso userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.PublicUser)
            .CopyQuestionnaire = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .DeleteQuestionnaire = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .GrantPermission = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .ManageQuestions = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .ManageInvitedQuestionnaire = (userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.AdminSecondario OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.Amministrativo OrElse userTypeId = COL_BusinessLogic_v2.Main.TipoPersonaStandard.SysAdmin)
            .ViewStatistics = (userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest AndAlso userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.PublicUser)
            .ViewPersonalStatistics = (userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest AndAlso userTypeId <> COL_BusinessLogic_v2.Main.TipoPersonaStandard.PublicUser)
        End With
        Return moduleQ
    End Function
    Public Shared Function CreateByPermission(permission As Long) As ModuleQuestionnaire
        Dim moduleQ As New ModuleQuestionnaire
        With moduleQ
            .Administration = PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
            .Compile = PermissionHelper.CheckPermissionSoft(Base2Permission.Compile, permission)
            .ViewPersonalStatistics = PermissionHelper.CheckPermissionSoft(Base2Permission.Compile, permission)
            .CopyQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.CopyQuestionnaire, permission)
            .DeleteQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.DeleteQuestionnaire, permission)
            .GrantPermission = PermissionHelper.CheckPermissionSoft(Base2Permission.GrantPermission, permission)
            .ManageQuestions = PermissionHelper.CheckPermissionSoft(Base2Permission.ManageQuestions, permission)
            .ManageInvitedQuestionnaire = PermissionHelper.CheckPermissionSoft(Base2Permission.ManageInvitedQuestionnaire, permission)
            .ViewStatistics = PermissionHelper.CheckPermissionSoft(Base2Permission.ViewStatistics, permission)
        End With
        Return moduleQ
    End Function

    <Flags()> Public Enum Base2Permission As Long
        AdminService = 64 '6
        Compile = 1 '0
        CopyQuestionnaire = 4 '2
        DeleteQuestionnaire = 8 '3
        GrantPermission = 32 '5
        ManageQuestions = 2 '1
        ManageInvitedQuestionnaire = 128 '7
        ViewStatistics = 16 '4
    End Enum

    Public Enum ActionType
        None = 0
        NoPermission = 1
        GenericError = 2
        QuestionariList = 50003
        SondaggiList = 50004
        LibrerieList = 50005
        TestAutovalutazioneList = 50006
        CreateQuestionario = 50007
        CreateSondaggio = 50008
        CreateLibreria = 50009
        CreateTestAutovalutazione = 50010
        DeleteQuestionario = 50011
        DeleteSondaggio = 50012
        DeleteLibreria = 50013
        DeleteTestAutovalutazione = 50014
        CompileStartQuestionario = 50015
        CompileStartSondaggio = 50016
        CompileEndQuestionario = 50017
        CompileEndSondaggio = 50018
        QuestionariAdminList = 50019
        SondaggiAdminList = 50020
        LibrerieAdminList = 50021
        TestAutovalutazioneAdminList = 50022
        CompileStartTestAutovalutazione = 50023
        CompileEndTestAutovalutazione = 50024
        CreateMeetingPoll = 50025
        CreateInviteToMeetingPoll = 50026
        CreateInviteToQuestionario = 50027
        CreateInviteToPoll = 50028
        CreateInviteToTestAutovalutazione = 50029
        CompileExternal = 50030
        DeleteOneAnswerQuestionario = 50031
        DeleteOneAnswerSondaggio = 50032
        DeleteOneAnswerTestAutovalutazione = 50033
        DeleteOneAnswerMeetingPoll = 50034
        DeleteOneAnswerElse = 50035
        DeleteAllAnswersQuestionario = 50036
        DeleteAllAnswersSondaggio = 50037
        DeleteAllAnswersTestAutovalutazione = 50038
        DeleteAllAnswersMeetingPoll = 50039
        DeleteAllAnswersElse = 50040
        CompileStartMeetingPoll = 50041
        CompileStartElse = 50042
        CompileEndMeetingPoll = 50043
        CompileEndElse = 50044
        CreateElse = 50045
        DeleteElse = 50046
        DeleteMeetingPoll = 50047
        ElseList = 50048
        MeetingPollList = 50049
        ElseAdminList = 50049
        MeetingPollAdminList = 50050
        QuestionDelete = 50051
        QuestionDeleteFromLibrary = 50051
        QuestionEdit = 50052
        QuestionEditFromLibrary = 50053
        QuestionAdd = 50054
        QuestionAddToLibrary = 50055
        QuestionAddFromLibrary = 50056
        QuestionVirtualRemove = 50057
        QuestionVirtualRemoveFromLibrary = 50058
        EditLibraryAssociation = 50059
        QuestionStartEditing = 50060
        QuestionMoved = 50061
        QuestionMovedUp = 50062
        QuestionMovedDown = 50063
        PageVirtualDelete = 50064
        PageVirtualUndelete = 50065
        PagePhisicalDelete = 50066
        PageStartEditing = 50067
        QuestionStartAdding = 50068
        QuestionsSelectFromLibrary = 50069
        QuestionsSelectFromQuestionnaire = 50070
        QuestionsCopyToLibrary = 50071
        QuestionsCopyToQuestionnaire = 50072
        QuestionsSelectForCopyToLibrary = 50073
        QuestionsSelectForCopyToQuestionnaire = 50074
        QuestionsSelectForDelete = 50075
        PageAdd = 50076
        QuestionErrorRemoving = 50077
        ExportResults = 50078
        ExportResult = 50079
        ViewUserResult = 50080
        ViewUsersResult = 50081
    End Enum


    Public Enum ObjectType
        None = 0
        Questionario = 1
        Sondaggio = 2
        Libreria = 3
        Domanda = 4
        Page = 5
        QuestionnaireTranslation = 6
        Language = 7
        QuestionnaireAnswer = 8
    End Enum

End Class