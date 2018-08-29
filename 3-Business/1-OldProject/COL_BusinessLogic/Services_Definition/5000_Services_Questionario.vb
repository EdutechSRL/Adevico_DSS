Namespace UCServices
    Public Class Services_Questionario
        Inherits Abstract.MyServices

        Public Const Codex As String = "SRVQUST"

#Region "Public Property"
        Public Property GrantPermission() As Boolean
            Get
                GrantPermission = MyBase.GetPermissionValue(PermissionType.Grant)
            End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Grant, IIf(Value, 1, 0))
			End Set
        End Property
        Public Property Admin() As Boolean
            Get
                Admin = MyBase.GetPermissionValue(PermissionType.Admin)
            End Get
			Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Admin, IIf(Value, 1, 0))
			End Set
        End Property

        Public Property CancellaQuestionario() As Boolean
            Get
                CancellaQuestionario = MyBase.GetPermissionValue(PermissionType.Delete)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Delete, IIf(Value, 1, 0))
			End Set
        End Property

        Public Property GestioneDomande() As Boolean
            Get
                GestioneDomande = MyBase.GetPermissionValue(PermissionType.Write)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Write, IIf(Value, 1, 0))
            End Set
        End Property
     
        Public Property CopiaQuestionario() As Boolean
            Get
                CopiaQuestionario = MyBase.GetPermissionValue(PermissionType.Change)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Change, IIf(Value, 1, 0))
			End Set
        End Property

        Public Property VisualizzaStatistiche() As Boolean
            Get
                VisualizzaStatistiche = MyBase.GetPermissionValue(PermissionType.Moderate)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Moderate, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property QuestionariSuInvito() As Boolean
            Get
                QuestionariSuInvito = MyBase.GetPermissionValue(PermissionType.Send)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Send, IIf(Value, 1, 0))
            End Set
        End Property

        Public Property Compila() As Boolean
            Get
                Compila = MyBase.GetPermissionValue(PermissionType.Read)
            End Get
            Set(ByVal Value As Boolean)
				MyBase.SetPermissionByPosition(PermissionType.Read, IIf(Value, 1, 0))
            End Set
        End Property

#End Region

        Sub New()
            MyBase.New()
            Me.Admin = False
            Me.GrantPermission = False
            Me.GestioneDomande = False
            Me.CopiaQuestionario = False
            Me.VisualizzaStatistiche = False
            Me.Compila = False
            Me.QuestionariSuInvito = False
            Me.CancellaQuestionario = False
        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
		End Sub

		Public Shared Function Create() As Services_Questionario
			Return New Services_Questionario("00000000000000000000000000000000")
		End Function

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
            ExportUsersStatistics = 50078
            ViewUsersStatistics = 50079
            ViewUserStatistics = 50080
        End Enum
		Public Enum ObjectType
			None = 0
			Questionario = 1
			Sondaggio = 2
			Libreria = 3
			Domanda = 4
        End Enum


        <Flags()> Public Enum Base2Permission
            GrantPermission = 32 '5
            AdminService = 64 '6
            DeleteQuestionario = 8 '3
            GestioneDomande = 2 '1
            CopiaQuestionario = 4 '2
            VisualizzaStatistiche = 16 '4
            QuestionariSuInvito = 128 '7
            Compila = 1 '0
        End Enum

    End Class
End Namespace
