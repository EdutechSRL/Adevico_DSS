Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel.Helpers

Namespace Business
    Public Class ServiceQuestionnaire
        Inherits lm.Comol.Core.Business.BaseCoreServices
        Implements iLinkedService

        Public Const UniqueID As String = "SRVQUST"
        Private _Context As iApplicationContext


#Region "initClass"
        Public Sub New()

        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.Manager = New BaseModuleManager(oContext.DataContext)
            _Context = oContext
            MyBase.UC = oContext.UserContext
        End Sub
        Public Sub New(oDC As iDataContext)
            MyBase.Manager = New BaseModuleManager(oDC)
            _Context = New ApplicationContext()
            _Context.DataContext = oDC
            'this.UC = null
        End Sub

#End Region

        Public Function ServiceModuleID() As Integer
            Return Manager.GetModuleID(UniqueID)
        End Function

#Region "DA IMPLEMENTARE "
        Public Function GetAllowedStandardAction(source As ModuleObject, destination As ModuleObject, idUser As Integer, idRole As Integer, idCommunity As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As List(Of StandardActionType) Implements iLinkedService.GetAllowedStandardAction
            Return New List(Of StandardActionType)
        End Function
        Public Function AllowActionExecution(link As ModuleLink, idUser As Integer, idCommunity As Integer, idRole As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As Boolean Implements iLinkedService.AllowActionExecution
            Return False
        End Function

        Public Function AllowStandardAction(actionType As StandardActionType, source As ModuleObject, destination As ModuleObject, idUser As Integer, idRole As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As Boolean Implements iLinkedService.AllowStandardAction
            Dim mQuest As ModuleQuestionnaire = Nothing
            If IsNothing(destination) Then
                mQuest = GetUserPermission(source.ObjectLongID, idUser)
                'Else
                '    mQuest = GetCommunityPermission(source.ObjectLongID)
            End If
            Select Case actionType
                Case StandardActionType.Edit, StandardActionType.Admin, StandardActionType.Create, StandardActionType.Delete
                    Return mQuest.Administration
                Case StandardActionType.ViewUserStatistics
                    Return mQuest.Administration OrElse mQuest.ViewStatistics
                Case StandardActionType.ViewPersonalStatistics
                    Return mQuest.Administration OrElse mQuest.ViewStatistics OrElse mQuest.ViewPersonalStatistics OrElse mQuest.Compile
                Case StandardActionType.ViewAdvancedStatistics
                    Return mQuest.Administration OrElse mQuest.ViewStatistics OrElse mQuest.ViewPersonalStatistics
                Case StandardActionType.Play
                    Return mQuest.Compile
            End Select
            Return False
        End Function

        Public Function EvaluateModuleLink(link As ModuleLink, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As dtoEvaluation Implements iLinkedService.EvaluateModuleLink
            Return EvaluateItem(link, idUser)
        End Function
        Public Function EvaluateModuleLinks(links As List(Of ModuleLink), idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As List(Of dtoItemEvaluation(Of Long)) Implements iLinkedService.EvaluateModuleLinks
            Dim respose As New List(Of dtoItemEvaluation(Of Long))
            respose = (From t In (From l In links Select EvaluateItem(l, idUser)).ToList() Where Not (t Is Nothing) Select t).ToList()
            Return respose
        End Function

        Private Function EvaluateItem(link As ModuleLink, idUser As Integer) As dtoItemEvaluation(Of Long)
            Dim response As dtoItemEvaluation(Of Long) = Nothing
            Dim idQuestionnnaire As Integer = CInt(link.DestinationItem.ObjectLongID)
            Dim quest As LazyQuestionnaire = GetItem(Of LazyQuestionnaire)(idQuestionnnaire)
            If Not IsNothing(quest) Then
                response = CalculateComplation(quest, idUser)
            End If
            If Not IsNothing(response) Then
                response.isStarted = True
                response.Item = link.Id
                Return response
            Else
                Return New dtoItemEvaluation(Of Long) With {.isStarted = False, .Item = link.Id}
            End If




            'If Not IsNothing(userResponse) Then
            '    Dim questionNumber As Integer = 0
            '    Dim answersNumber As Integer = 0
            '    Dim answersSkipped As Integer = 0

            '    If userResponse.QuestionsCount.HasValue Then
            '        questionNumber = userResponse.QuestionsCount.Value
            '    Else
            '        questionNumber = (From q In Manager.Linq(Of LazyAssociatedQuestion)() _
            '                                     Where q.IdQuestionnnaire.Equals(IdQuestionnnaire) Select q.IdQuestion).Count()
            '    End If
            '    If userResponse.WrongAnswers.HasValue Then
            '        answersNumber += userResponse.WrongAnswers.Value
            '    End If
            '    If userResponse.UngradedAnswers.HasValue Then
            '        answersNumber += userResponse.UngradedAnswers.Value
            '    End If
            '    If userResponse.CorrectAnswers.HasValue Then
            '        answersNumber += userResponse.CorrectAnswers.Value
            '    End If
            '    If userResponse.QuestionsSkipped.HasValue Then
            '        answersSkipped += userResponse.QuestionsSkipped.Value
            '    Else
            '        answersSkipped = 0
            '    End If
            '    '  respose.Mark = userResponse.Mark
            '    If questionNumber > 0 Then
            '        If answersSkipped > 0 Then
            '            respose.Completion = (100 - (answersSkipped * 100 / questionNumber))
            '        ElseIf answersNumber <= questionNumber Then
            '            respose.Completion = (100 - ((questionNumber - answersNumber) * 100 / questionNumber))
            '        End If
            '    End If
            '    respose.isStarted = True
            '    If (userResponse.CompletedOn.HasValue) Then
            '        respose.isCompleted = True
            '    Else
            '        respose.isCompleted = (respose.Completion = 100)
            '    End If
            '    respose.isPassed = True
            'End If
        End Function
        Public Function CalculateComplation(idQuestionnaire As Integer, idUser As Integer) As dtoItemEvaluation(Of Long)
            Dim quest As LazyQuestionnaire = GetItem(Of LazyQuestionnaire)(idQuestionnaire)
            If Not IsNothing(quest) Then
                Return CalculateComplation(quest, idUser)
            Else
                Return Nothing
            End If
        End Function
        Public Function CalculateComplation(idQuestionnaire As Integer, idUser As Integer, idResponse As Integer) As dtoItemEvaluation(Of Long)
            Dim result As dtoItemEvaluation(Of Long) = Nothing
            Dim quest As LazyQuestionnaire = GetItem(Of LazyQuestionnaire)(idQuestionnaire)


            Dim qResponse As LazyUserResponse
            'If Not isAttempt Then
            qResponse = GetRefreshItem(Of LazyUserResponse)(idResponse)
            'Else
            '    qResponse = Manager.GetAll(Of LazyUserResponse)(Function(qr) qr.IdRandomQuestionnaire = idResponse).FirstOrDefault()
            '    Manager.Refresh(Of LazyUserResponse)(qResponse)
            'End If

            If Not IsNothing(quest) AndAlso Not IsNothing(qResponse) Then
                Select Case quest.IdType
                    Case QuestionnaireType.RandomMultipleAttempts
                        Dim attempts As List(Of LazyUserResponse) = GetQuestionnaireAttempts(quest.Id, idUser, 0)
                        If Not IsNothing(attempts) AndAlso attempts.Count > 0 Then
                            result = CalculateComplation(qResponse)
                            If Not IsNothing(result) Then
                                result.isPassed = True
                                If result.isCompleted Then
                                    If qResponse.RelativeScore < quest.MinScore Then
                                        result.isPassed = False
                                    End If
                                End If
                            End If
                        End If
                    Case Else
                        result = CalculateComplation(qResponse)

                        If Not IsNothing(result) Then
                            result.isPassed = True
                        End If
                End Select
            End If

            Return result
        End Function
        Private Function CalculateComplation(quest As LazyQuestionnaire, idUser As Integer) As dtoItemEvaluation(Of Long)
            Dim response As dtoItemEvaluation(Of Long) = Nothing
            Select Case quest.IdType
                Case QuestionnaireType.RandomMultipleAttempts
                    Dim attempts As List(Of LazyUserResponse) = GetQuestionnaireAttempts(quest.Id, idUser, 0)
                    If Not IsNothing(attempts) AndAlso attempts.Count > 0 Then
                        Dim l As LazyUserResponse = attempts.OrderByDescending(Function(r) r.Id).FirstOrDefault()
                        response = CalculateComplation(l)
                        If Not IsNothing(response) Then
                            response.isPassed = True
                            If response.isCompleted Then
                                If l.RelativeScore < quest.MinScore Then
                                    response.isCompleted = False
                                End If
                            End If
                        End If
                    End If
                Case Else
                    Dim userResponse As LazyUserResponse = (From r In Manager.Linq(Of LazyUserResponse)()
                                                            Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(quest.Id) Select r).Skip(0).Take(1).ToList().FirstOrDefault()
                    response = CalculateComplation(userResponse)

                    If Not IsNothing(response) Then
                        response.isPassed = True
                    End If
            End Select
            Return response
        End Function
        Public Function CalculateComplation(userResponse As LazyUserResponse) As dtoItemEvaluation(Of Long)
            Dim respose As New dtoItemEvaluation(Of Long)
            respose.isStarted = False
            If Not IsNothing(userResponse) Then
                Dim questionNumber As Integer = 0
                Dim answersNumber As Integer = 0
                Dim answersSkipped As Integer = 0

                If userResponse.QuestionsCount.HasValue Then
                    questionNumber = userResponse.QuestionsCount.Value
                Else
                    questionNumber = (From q In Manager.Linq(Of LazyAssociatedQuestion)()
                                      Where q.IdQuestionnnaire.Equals(userResponse.IdQuestionnnaire) AndAlso (userResponse.IdRandomQuestionnaire.Equals(0) OrElse q.IdRandomQuestionnnaire.Equals(userResponse.IdRandomQuestionnaire)) Select q.IdQuestion).Count()
                End If
                If userResponse.WrongAnswers.HasValue Then
                    answersNumber += userResponse.WrongAnswers.Value
                End If
                If userResponse.UngradedAnswers.HasValue Then
                    answersNumber += userResponse.UngradedAnswers.Value
                End If
                If userResponse.CorrectAnswers.HasValue Then
                    answersNumber += userResponse.CorrectAnswers.Value
                End If
                If userResponse.QuestionsSkipped.HasValue Then
                    answersSkipped += userResponse.QuestionsSkipped.Value
                Else
                    answersSkipped = 0
                End If
                '  respose.Mark = userResponse.Mark
                If questionNumber > 0 Then
                    If answersSkipped > 0 Then
                        respose.Completion = (100 - (answersSkipped * 100 / questionNumber))
                    ElseIf answersNumber <= questionNumber Then
                        respose.Completion = (100 - ((questionNumber - answersNumber) * 100 / questionNumber))
                    End If
                End If
                respose.isStarted = True
                If (userResponse.CompletedOn.HasValue) Then
                    respose.isCompleted = True
                Else
                    respose.isCompleted = False ' (respose.Completion = 100)
                End If
            End If

            Return respose
        End Function
#End Region

#Region "NON SERVE IMPLEMENTARE"
        Public Function GetObjectItemFilesForStatistics(objectId As Long, objectTypeId As Integer, translations As Dictionary(Of Integer, String), idCommunity As Integer, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) As StatTreeNode(Of StatFileTreeLeaf) Implements iLinkedService.GetObjectItemFilesForStatistics
            Return New StatTreeNode(Of StatFileTreeLeaf)
        End Function
        Public Sub PhisicalDeleteCommunity(idCommunity As Integer, idUser As Integer, baseFilePath As String, baseThumbnailPath As String) Implements iLinkedService.PhisicalDeleteCommunity

        End Sub
        Public Sub PhisicalDeleteRepositoryItem(idFileItem As Long, idCommunity As Integer, idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.PhisicalDeleteRepositoryItem

        End Sub
        Public Sub SaveActionExecution(link As ModuleLink, isStarted As Boolean, isPassed As Boolean, Completion As Short, isCompleted As Boolean, mark As Short, idUser As Integer, AlreadyCompleted As Boolean, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.SaveActionExecution

        End Sub
        Public Sub SaveActionsExecution(evaluatedLinks As List(Of dtoItemEvaluation(Of ModuleLink)), idUser As Integer, Optional moduleUserLong As Dictionary(Of String, Long) = Nothing, Optional moduleUserString As Dictionary(Of String, String) = Nothing) Implements iLinkedService.SaveActionsExecution

        End Sub
#End Region

#Region "Invited Users"
        Public Function GenerateRequiredPasswordForInvitedUsers(idQuestionnnaire As Integer, lenght As Integer) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                If lenght < 4 Then
                    lenght = 6
                End If
                Dim items As List(Of LazyInvitedUser) = (From u In Manager.GetIQ(Of LazyInvitedUser)() Where (u.IdQuestionnnaire.Equals(idQuestionnnaire)) AndAlso (u.Password.Equals("") OrElse u.Password.Equals(Nothing)) Select u).ToList
                For Each item As LazyInvitedUser In items
                    item.Password = RandomPasswordGenerator(lenght)
                Next
                Manager.SaveOrUpdateList(items)
                Manager.Commit()
            Catch ex As Exception
                result = False
                Manager.RollBack()
            End Try
            Return result
        End Function
        Public Function AddExternalInvitedUser(idQuestionnnaire As Integer, dto As LazyExternalInvitedUser) As LazyExternalInvitedUser
            Dim invitedUser As LazyExternalInvitedUser
            Try
                Manager.BeginTransaction()
                invitedUser = (From u In Manager.GetIQ(Of LazyExternalInvitedUser)() Where (u.IdQuestionnnaire.Equals(idQuestionnnaire)) AndAlso u.Mail.Equals(dto.Mail) Select u).Skip(0).Take(1).ToList.FirstOrDefault()
                If (invitedUser Is Nothing) Then
                    invitedUser = New LazyExternalInvitedUser With {.IdQuestionnnaire = idQuestionnnaire, .MailSent = 0, .Mail = dto.Mail}
                End If
                With invitedUser
                    .Description = dto.Description
                    .Name = dto.Name
                    .Surname = dto.Surname
                    .Password = dto.Password
                End With
                Manager.SaveOrUpdate(invitedUser)
                Manager.Commit()
            Catch ex As Exception
                invitedUser = Nothing
                Manager.RollBack()
            End Try
            Return invitedUser
        End Function
        Public Function ExistExternalInvitedUser(idQuestionnnaire As Integer, mail As String) As String
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                result = (From u In Manager.GetIQ(Of LazyExternalInvitedUser)()
                          Where u.IdQuestionnnaire.Equals(idQuestionnnaire) AndAlso u.Mail.Equals(mail) Select u.Id).Any()
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = False
            End Try
            Return result
        End Function
        Public Function ExistInvitedUser(idQuestionnnaire As Integer, ByVal idUser As Integer) As String
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                result = (From u In Manager.GetIQ(Of LazyInternalInvitedUser)()
                          Where u.IdQuestionnnaire.Equals(idQuestionnnaire) AndAlso u.IdPerson.Equals(idUser) Select u.Id).Any()
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = False
            End Try
            Return result
        End Function
        Public Function UpdateMailSent(idUser As Integer) As Boolean
            Dim updated As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim invitedUser As LazyInvitedUser = Manager.Get(Of LazyInvitedUser)(idUser)
                If Not IsNothing(invitedUser) Then
                    invitedUser.MailSent = invitedUser.MailSent + 1
                    Manager.SaveOrUpdate(invitedUser)
                    updated = True
                End If
                Manager.Commit()
            Catch ex As Exception
                updated = False
                Manager.RollBack()
            End Try
            Return updated
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
#End Region

#Region "Templates"
        Public Function LoadTemplateNames(ByVal idUser As Integer) As Dictionary(Of Integer, String)
            Dim result As New Dictionary(Of Integer, String)
            Try
                result = (From t In Manager.GetIQ(Of LazyTemplate)() Where (t.IdPerson.Equals(idUser)) AndAlso (t.TemplateType.Equals(5)) Select t).ToList().ToDictionary(Of Integer, String)(Function(t) t.Id, Function(t) t.Name)


            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Function LoadTemplate(ByVal idUser As Integer) As List(Of LazyTemplate)
            Dim result As List(Of LazyTemplate)
            Try
                result = (From t In Manager.GetIQ(Of LazyTemplate)() Where (t.IdPerson.Equals(idUser)) AndAlso (t.TemplateType.Equals(5)) Select t).ToList()


            Catch ex As Exception
                result = New List(Of LazyTemplate)
            End Try
            Return result
        End Function
        Public Function GetTemplate(ByVal idTemplate As Integer) As LazyTemplate
            Dim result As LazyTemplate = Nothing
            Try
                result = Manager.Get(Of LazyTemplate)(idTemplate)
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Function SaveTemplate(template As LazyTemplate) As LazyTemplate
            Dim result As LazyTemplate
            Try
                Manager.BeginTransaction()
                result = Manager.Get(Of LazyTemplate)(template.Id)
                If IsNothing(result) Then
                    result = New LazyTemplate() With {.IdLanguage = template.IdLanguage, .IdPerson = template.IdPerson, .TemplateType = 5}
                End If
                With result
                    .Body = template.Body
                    .Link = template.Link
                    .MailSettings = template.MailSettings
                    If String.IsNullOrEmpty(template.Name) AndAlso String.IsNullOrEmpty(.Name) Then
                        .Name = (From t In Manager.GetIQ(Of LazyTemplate)() Select t.Id).Max() + 1
                    ElseIf Not String.IsNullOrEmpty(template.Name) Then
                        .Name = template.Name
                    End If
                    .Subject = template.Subject
                End With
                Manager.SaveOrUpdate(result)
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = Nothing
            End Try
            Return result
        End Function
        Public Function DeleteTemplate(ByVal idTemplate As Integer) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                Manager.DeletePhysical(Manager.Get(Of LazyTemplate)(idTemplate))
                Manager.Commit()
                result = True
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function
#End Region
#Region "Edit Questionnaire"
        Public Function MoveQuestion(idQuestion As Integer, idQuestionnaire As Integer, idLanguage As Integer, moveUp As Boolean) As Boolean
            Dim iResponse As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim quest As LazyQuestionnaireTranslation = (From q In Manager.GetIQ(Of LazyQuestionnaireTranslation)()
                                                         Where q.IdLanguage.Equals(idLanguage) AndAlso q.IdQuestionnnaire.Equals(idQuestionnaire)).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(quest) Then
                    Dim pages As List(Of dtoPage) = (From p In Manager.GetIQ(Of LazyQuestionnairePage)()
                                                               Where p.IdQuestionnaireTranslation.Equals(quest.Id)).ToList().OrderBy(Function(p) p.Number).ThenBy(Function(p) p.Id).Select(Function(p) New dtoPage() With {.Id = p.Id, .Number = p.Number, .FromQuestion = p.FromQuestion, .ToQuestion = p.ToQuestion}).ToList


                    Dim questions As List(Of dtoPageQuestion) = (From p In Manager.GetIQ(Of LazyQuestionAssociation)()
                                                                     Where p.IdQuestionnaire.Equals(idQuestionnaire)
                                                                     Order By p.Number, p.IdQuestion).Select(Function(p) New dtoPageQuestion() With {.Id = p.Id, .IdQuestion = p.IdQuestion, .Number = p.Number}).ToList

                    Dim fromQuestion, toQuestion As Integer
                    For Each page As dtoPage In pages
                        fromQuestion = page.FromQuestion
                        toQuestion = page.ToQuestion
                        page.Questions = questions.Where(Function(q) q.Number >= fromQuestion AndAlso q.Number <= toQuestion).OrderBy(Function(q) q.Number).ThenBy(Function(q) q.Id).ToList
                    Next
                End If
                Manager.Commit()
                iResponse = True
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return iResponse
        End Function

#End Region

#Region "Generic"
        Public Function GetPersonName(ByVal idPerson As Integer) As String
            Dim result As String = ""

            Try
                Manager.BeginTransaction()
                Dim p As Person = Manager.Get(Of Person)(idPerson)
                If Not IsNothing(p) Then
                    result = p.SurnameAndName
                End If

                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try

            Return result
        End Function
        Public Function GetUserDisplayNameByInvite(ByVal idInvite As Integer, ByVal anonymous As String) As String
            Return GetUserDisplayName(0, idInvite, anonymous)
        End Function

        Public Function GetUserDisplayName(ByVal idPerson As Integer, ByVal idInvite As Integer, ByVal anonymous As String) As String
            Dim result As String = anonymous

            Try
                Manager.BeginTransaction()
                Dim p As Person
                If idInvite > 0 Then
                    Dim invited As LazyInvitedUser = Manager.Get(Of LazyInvitedUser)(idInvite)
                    If Not IsNothing(invited) Then
                        If TypeOf (invited) Is LazyInternalInvitedUser Then
                            Dim internal As LazyInternalInvitedUser = DirectCast(invited, LazyInternalInvitedUser)
                            p = Manager.Get(Of Person)(internal.IdPerson)
                            If Not IsNothing(p) AndAlso p.TypeID <> UserTypeStandard.Guest Then
                                result = p.SurnameAndName
                            End If
                        ElseIf TypeOf (invited) Is LazyExternalInvitedUser Then
                            Dim external As LazyExternalInvitedUser = DirectCast(invited, LazyExternalInvitedUser)
                            result = external.Surname & " " & external.Name
                        End If
                    End If
                ElseIf idPerson > 0 Then
                    p = Manager.Get(Of Person)(idPerson)
                    If Not IsNothing(p) AndAlso p.TypeID <> UserTypeStandard.Guest Then
                        result = p.SurnameAndName
                    End If
                End If


                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try

            Return result
        End Function

        Public Function GetUserDtoDisplayName(ByVal idPerson As Integer, ByVal idInvite As Integer, ByVal anonymous As String) As dtoDisplayName
            Dim result As New dtoDisplayName(anonymous)

            Try
                'Manager.BeginTransaction()
                Dim p As litePerson
                If idInvite > 0 Then
                    Dim invited As LazyInvitedUser = Manager.Get(Of LazyInvitedUser)(idInvite)
                    If Not IsNothing(invited) Then
                        If TypeOf (invited) Is LazyInternalInvitedUser Then
                            Dim internal As LazyInternalInvitedUser = DirectCast(invited, LazyInternalInvitedUser)
                            p = Manager.GetLitePerson(internal.IdPerson)
                            If Not IsNothing(p) AndAlso p.TypeID <> UserTypeStandard.Guest Then
                                result = New dtoDisplayName(p)
                            End If
                        ElseIf TypeOf (invited) Is LazyExternalInvitedUser Then
                            Dim external As LazyExternalInvitedUser = DirectCast(invited, LazyExternalInvitedUser)
                            result = New dtoDisplayName(external)
                        End If
                    End If
                ElseIf idPerson > 0 Then
                    p = Manager.GetLitePerson(idPerson)
                    If Not IsNothing(p) AndAlso p.TypeID <> UserTypeStandard.Guest Then
                        result = New dtoDisplayName(p)
                    End If
                End If
                'Manager.Commit()
            Catch ex As Exception
                'Manager.RollBack()
            End Try

            Return result
        End Function

        Public Function GetItemName(ByVal idQuestionnnaire As Integer, ByVal idLanguage As Integer) As String
            Dim result As String = ""

            Try
                Manager.BeginTransaction()
                result = (From q In Manager.GetIQ(Of QuestionnaireTranslation)() Where q.IdQuestionnnaire.Equals(idQuestionnnaire) AndAlso q.IdLanguage.Equals(idLanguage) Select q.Name).Skip(0).Take(1).ToList().FirstOrDefault()

                'ToDo: ADDED - Check/Test
                'Provo con lingua di DEFAULT
                If String.IsNullOrEmpty(result) Then
                    Dim DefLangId As Integer = Manager.GetDefaultIdLanguage()

                    result = (From q In Manager.GetIQ(Of QuestionnaireTranslation)() Where q.IdQuestionnnaire.Equals(idQuestionnnaire) AndAlso q.IdLanguage.Equals(DefLangId) Select q.Name).Skip(0).Take(1).ToList().FirstOrDefault()

                End If

                'Altrimenti prendo la prima lingua disponibile...
                If String.IsNullOrEmpty(result) Then
                    
                    result = (From q In Manager.GetIQ(Of QuestionnaireTranslation)() Where q.IdQuestionnnaire.Equals(idQuestionnnaire) _
                              AndAlso q.IsDefault = True Select q.Name).Skip(0).Take(1).ToList().FirstOrDefault()

                End If


                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try

            Return result
        End Function
        Public Function GetItem(Of T)(id As Object) As T
            Dim result As T
            Try
                result = Manager.Get(Of T)(id)
            Catch ex As Exception


            End Try
            Return result
        End Function
        Public Function GetRefreshItem(Of T)(id As Object) As T
            Dim result As T
            Try
                result = Manager.Get(Of T)(id)
                If Not IsNothing(result) Then
                    Manager.Refresh(result)
                End If

            Catch ex As Exception


            End Try
            Return result
        End Function
        'Public Function GetGroup(idQuestionnnaire) As LazyGroup
        '    Dim result As LazyGroup
        '    Try
        '        result= (From g In Manager.GetIQ(Of LazyGroup) Where 
        '    Catch ex As Exception


        '    End Try
        '    Return result
        'End Function
        Public Function GetAvailableParticipantTypes(ByVal idQuestionnnaire As Integer, ByVal idCommunity As Integer) As List(Of participantType)
            Return GetAvailableParticipantTypes(Manager.GetIQ(Of LazyQuestionnaire)(idQuestionnnaire), idCommunity)
        End Function
        Public Function GetAvailableParticipantTypes(item As LazyQuestionnaire, ByVal idCommunity As Integer) As List(Of participantType)
            Dim items As New List(Of participantType)
            If Not IsNothing(item) Then
                Dim idUsers As New List(Of Integer)
                If (item.ForExternalUsers) Then
                    idUsers.AddRange((From p In Manager.GetIQ(Of Person)() Where p.TypeID.Equals(UserTypeStandard.Guest) OrElse p.TypeID.Equals(UserTypeStandard.PublicUser) Select p.Id).ToList())
                End If
                If idCommunity > 0 AndAlso item.ForCommunityUsers AndAlso (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(item.Id) AndAlso Not idUsers.Contains(r.IdPerson)).Any() Then
                    items.Add(participantType.communityUsers)
                End If
                If item.ForPortalUsers AndAlso (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(item.Id) AndAlso Not idUsers.Contains(r.IdPerson)).Any() Then
                    items.Add(participantType.allUsers)
                End If
                If (item.Participants And participantType.invitedUsers) AndAlso (From iu In Manager.GetIQ(Of LazyInvitedUser)() Where iu.IdQuestionnnaire.Equals(item.Id)).Any() Then
                    items.Add(participantType.invitedUsers)
                End If
                If (item.Participants And participantType.externalUsers) AndAlso (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(item.Id) AndAlso idUsers.Contains(r.IdPerson)).Any() Then
                    items.Add(participantType.externalUsers)
                End If

            End If


            Return items
        End Function

        Public Function GetUserPermission(ByVal idQuestionnnaire As Integer, idUser As Integer) As ModuleQuestionnaire
            Dim person As Person = Manager.GetPerson(idUser)
            If IsNothing(person) Then
                Return GetCommunityPermission(idQuestionnnaire, idUser, UserTypeStandard.Guest)
            Else
                Return GetCommunityPermission(idQuestionnnaire, idUser, person.TypeID)
            End If
        End Function
        Public Function GetQuestionnaireIdCommunity(ByVal idQuestionnnaire As Integer) As Integer
            Dim result As Integer = 0

            Try
                Manager.BeginTransaction()
                Dim idGroupOwner = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where q.Id.Equals(idQuestionnnaire) Select q.IdGroup).Skip(0).Take(1).ToList().FirstOrDefault
                Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(idGroupOwner)

                If Not IsNothing(groupOwner) Then
                    result = groupOwner.IdCommunity
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function

        Public Function GetQuestionnaireIdByLanguage(ByVal idQuestionnnaire As Integer, idLanguage As Integer) As Integer
            Dim result As Integer = 0

            Try
                result = (From q In Manager.GetIQ(Of QuestionnaireTranslation)() Where q.Id.Equals(idQuestionnnaire) AndAlso q.IdLanguage.Equals(idLanguage) Select q.Id).Skip(0).Take(1).ToList().FirstOrDefault
            Catch ex As Exception
            End Try
            Return result
        End Function

        Public Function GetCommunityPermission(ByVal idQuestionnnaire As Integer) As ModuleQuestionnaire
            Return GetCommunityPermission(idQuestionnnaire, Me.UC.CurrentUserID, Me.UC.UserTypeID)
        End Function
        Private Function GetCommunityPermission(ByVal idQuestionnnaire As Integer, ByVal idUser As Integer, ByVal userType As Integer) As ModuleQuestionnaire
            Dim moduleQ As New ModuleQuestionnaire

            Try
                Manager.BeginTransaction()
                Dim idGroupOwner = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where q.Id.Equals(idQuestionnnaire) Select q.IdGroup).Skip(0).Take(1).ToList().FirstOrDefault
                Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(idGroupOwner)

                If Not IsNothing(groupOwner) Then
                    If groupOwner.IdCommunity = 0 Then
                        moduleQ = ModuleQuestionnaire.CreatePortalmodule(userType)
                    Else
                        moduleQ = ModuleQuestionnaire.CreateByPermission(Manager.GetModulePermission(idUser, groupOwner.IdCommunity, ServiceModuleID))
                    End If
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return moduleQ
        End Function
        Public Function GetCommunityPermission(ByVal idQuestionnnaire As Integer, ByRef idCommunity As Integer) As ModuleQuestionnaire
            Dim moduleQ As New ModuleQuestionnaire

            Try
                Manager.BeginTransaction()
                Dim idGroupOwner = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where q.Id.Equals(idQuestionnnaire) Select q.IdGroup).Skip(0).Take(1).ToList().FirstOrDefault
                Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(idGroupOwner)

                If Not IsNothing(groupOwner) Then
                    idCommunity = groupOwner.IdCommunity

                    moduleQ = GetModulePermission(idCommunity, Me.UC.CurrentUserID, Me.UC.UserTypeID)
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return moduleQ
        End Function

        Public Function GetModulePermission(ByVal idCommunity As Integer, ByVal idUser As Integer, ByVal idUserType As Integer) As ModuleQuestionnaire
            If idCommunity = 0 Then
                Return ModuleQuestionnaire.CreatePortalmodule(idUserType)
            Else
                Return ModuleQuestionnaire.CreateByPermission(Manager.GetModulePermission(Me.UC.CurrentUserID, idCommunity, ServiceModuleID))
            End If
        End Function
        Public Function CanChangeAnonymousStatus(ByVal idQuestionnnaire As Integer) As Boolean
            Dim result As Boolean = False

            Try
                Manager.BeginTransaction()
                Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)

                If IsNothing(quest) Then
                    result = True
                Else
                    result = Not quest.isAnonymous OrElse (quest.isAnonymous AndAlso Not (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnnaire) Select r.Id).Any())
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function


        Public Function GetExternalContext(ByVal idQuestionnnaire As Integer, ByVal idInvited As Integer) As ExternalPageContext
            Dim result As ExternalPageContext = New ExternalPageContext

            Try
                Manager.BeginTransaction()
                result.Skin = GetItemSkinSettings(idQuestionnnaire, idInvited)
                result.Source = GetItemModuleObject(idQuestionnnaire)
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function
        Public Function GetModuleObject(ByVal idQuestionnnaire As Integer) As ModuleObject
            Dim result As ModuleObject = New ModuleObject With {.ObjectLongID = idQuestionnnaire}

            Try
                Manager.BeginTransaction()
                result = GetItemModuleObject(idQuestionnnaire)
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function
        Private Function GetItemModuleObject(ByVal idQuestionnnaire As Integer) As ModuleObject
            Dim result As ModuleObject = New ModuleObject With {.ObjectLongID = idQuestionnnaire}
            Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)
            If Not IsNothing(quest) Then
                Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(quest.IdGroup)

                If Not IsNothing(groupOwner) Then
                    result.CommunityID = groupOwner.IdCommunity
                End If
                'Select Case quest.IdType
                '    Case Questionario.TipoQuestionario.LibreriaDiDomande
                '        result.ObjectTypeID = ModuleQuestionnaire.ObjectType.Libreria
                '    Case Questionario.TipoQuestionario.Sondaggio
                '        result.ObjectTypeID = ModuleQuestionnaire.ObjectType.Sondaggio
                '    Case Else
                result.ObjectTypeID = ModuleQuestionnaire.ObjectType.Questionario
                ' End Select
                result.FQN = GetType(Questionario).FullName
                result.ServiceID = ServiceModuleID()
            End If

            Return result
        End Function
        Public Function GetDefaultSkinSettings(ByVal idQuestionnnaire As Integer) As dtoItemSkin
            Return GetDefaultSkinSettings(idQuestionnnaire, 0)
        End Function
        Public Function GetDefaultSkinSettings(ByVal idQuestionnnaire As Integer, ByVal idInvited As Integer) As dtoItemSkin
            Dim result As dtoItemSkin = Nothing

            Try
                Manager.BeginTransaction()
                result = GetItemSkinSettings(idQuestionnnaire, idInvited)
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = Nothing
            End Try
            Return result
        End Function
        Private Function GetItemSkinSettings(ByVal idQuestionnnaire As Integer, ByVal idInvited As Integer) As dtoItemSkin
            Dim result As dtoItemSkin = Nothing

            Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)
            If Not IsNothing(quest) Then
                Dim groupOwner As LazyGroup = Manager.Get(Of LazyGroup)(quest.IdGroup)

                If Not IsNothing(groupOwner) Then
                    result = New dtoItemSkin()
                    result.IdCommunity = groupOwner.IdCommunity
                    result.IsForPortal = (groupOwner.IdCommunity = 0)
                    If groupOwner.IdCommunity > 0 Then
                        result.IdOrganization = (From c In Manager.GetIQ(Of Community)() Where c.Id.Equals(groupOwner.Id) Select c.IdOrganization).Skip(0).Take(1).ToList().FirstOrDefault()
                    End If
                    If result.IdOrganization = 0 AndAlso idInvited > 0 Then
                        Dim invited As LazyInvitedUser = Manager.Get(Of LazyInvitedUser)(idInvited)
                        If TypeOf (invited) Is LazyInternalInvitedUser Then
                            result.IdOrganization = (From n In Manager.GetIQ(Of OrganizationProfiles)() Where Not IsNothing(n.Profile) AndAlso n.Profile.Id.Equals(DirectCast(invited, LazyInternalInvitedUser).IdPerson) AndAlso n.isDefault = True Select n.OrganizationID).Skip(0).Take(1).ToList().FirstOrDefault()
                        End If
                    End If
                End If
            End If
            Return result
        End Function

        Public Function CanEditSkin(ByVal idQuestionnnaire As Integer) As Boolean
            Dim result As Boolean = False

            Try
                Manager.BeginTransaction()
                Dim quest As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)

                If IsNothing(quest) Then
                    result = True
                Else
                    result = Not quest.isAnonymous OrElse (quest.isAnonymous AndAlso Not (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnnaire) Select r.Id).Any())
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function

        Public Function HasSelectableQuestionnaire(ByVal idCommunity As Integer) As Boolean
            Dim result As Boolean = False

            Try
                Dim groups As List(Of Integer) = (From g In Manager.GetIQ(Of LazyGroup)() Where g.IdCommunity = idCommunity AndAlso g.IsDeleted = False Select g.Id).ToList()
                If groups.Count > 0 Then
                    result = (From q In Manager.GetIQ(Of LazyQuestionnaire)() _
                              Where (q.IdType = QuestionnaireType.Standard OrElse q.IdType = QuestionnaireType.Random) AndAlso groups.Contains(q.IdGroup) AndAlso q.OwnerId = 0).Any
                End If
            Catch ex As Exception

            End Try

            Return result
        End Function
        Public Function SaveSettings(idQuestionnaire As Integer, displayScoreToUser As Boolean, displayAttemptScoreToUser As Boolean, displayAvailableAttempts As Boolean, displayResultsStatus As Boolean, displayCurrentAttempts As Boolean) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim q As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(q) Then
                    q.DisplayScoreToUser = displayScoreToUser
                    q.DisplayAttemptScoreToUser = displayAttemptScoreToUser
                    q.DisplayAvailableAttempts = displayAvailableAttempts
                    q.DisplayResultsStatus = displayResultsStatus
                    q.DisplayCurrentAttempts = displayCurrentAttempts
                    Manager.SaveOrUpdate(q)
                End If
                Manager.Commit()
                result = Not IsNothing(q)
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function
#End Region

#Region "Repeat Attempts questionnaire"
        'Public Function GetQuestionnaireChildrenCount(idQuestionnaire As Integer, idPerson As Integer, idInvitedUser As Integer) As Integer
        '    Dim result As Integer = 0
        '    Try
        '        Manager.BeginTransaction()
        '        result = (From qr In Manager.GetIQ(Of LazyQuestionnaireRandom)() Where qr.IdFather.Equals(idQuestionnaire) AndAlso qr.IdPerson.Equals(idPerson) AndAlso qr.IdUser.Equals(idInvitedUser) Select qr.Id).Count
        '        Manager.Commit()
        '    Catch ex As Exception
        '        Manager.RollBack()
        '    End Try
        '    Return result
        'End Function
        'Public Function GetQuestionnaireChildren(idQuestionnaire As Integer, idPerson As Integer, idInvitedUser As Integer) As List(Of LazyQuestionnaireRandom)
        '    Dim result As List(Of LazyQuestionnaireRandom) = Nothing
        '    Try
        '        Manager.BeginTransaction()
        '        result = (From qr In Manager.GetIQ(Of LazyQuestionnaireRandom)() Where qr.IdFather.Equals(idQuestionnaire) AndAlso qr.IdPerson.Equals(idPerson) AndAlso qr.IdUser.Equals(idInvitedUser) Select qr).ToList()
        '        Manager.Commit()
        '    Catch ex As Exception
        '        Manager.RollBack()
        '        result = New List(Of LazyQuestionnaireRandom)()
        '    End Try
        '    Return result
        'End Function
        Public Function SaveRepeatSettings(idQuestionnaire As Integer, minScore As Integer, maxAttempts As Integer, displayScoreToUser As Boolean, displayAttemptScoreToUser As Boolean, displayAvailableAttempts As Boolean, displayResultsStatus As Boolean, displayCurrentAttempts As Boolean) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim q As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(q) Then
                    q.MaxAttempts = maxAttempts
                    q.MinScore = minScore
                    q.DisplayScoreToUser = displayScoreToUser
                    q.DisplayAttemptScoreToUser = displayAttemptScoreToUser
                    q.DisplayAvailableAttempts = displayAvailableAttempts
                    q.DisplayResultsStatus = displayResultsStatus
                    q.DisplayCurrentAttempts = displayCurrentAttempts
                    Manager.SaveOrUpdate(q)
                End If
                Manager.Commit()
                result = Not IsNothing(q)
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return result
        End Function
        Public Function GetQuestionnaireAttempts(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer) As List(Of LazyUserResponse)
            Dim result As New List(Of LazyUserResponse)
            Try
                Manager.BeginTransaction()
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(qFather) Then
                    result = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                              AndAlso (idInvitedUser.Equals(0) OrElse r.IdInvitedUser.Equals(idInvitedUser)) Select r).ToList()
                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = Nothing
            End Try
            Return result
        End Function

        Public Function GetQuestionnaireAttempts(idQuestionnaire As Integer, status As AnswerStatus, anonymous As String) As List(Of dtoUserAnswerItem)
            Dim result As New List(Of dtoUserAnswerItem)
            Try
                Manager.BeginTransaction()
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(qFather) Then
                    result = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnaire) Select r).ToList().Select(Function(e) New dtoUserAnswerItem(e)).ToList()
                    result = (From r In result Where (status = AnswerStatus.All OrElse (status = AnswerStatus.Compiling AndAlso Not r.CompletedOn.HasValue) OrElse (status = AnswerStatus.Completed AndAlso r.CompletedOn.HasValue)) Select r).ToList()

                    SetDisplayName(result, anonymous)

                    Dim attempt As Integer
                    Dim idPerson As Integer = 0
                    Dim idInvited As Integer? = 0
                    For Each item As dtoUserAnswerItem In (From r In result Order By r.IdPerson, r.IdInvitedUser, r.Id Select r).ToList
                        If idPerson <> item.IdPerson AndAlso (Not item.IdInvitedUser.HasValue OrElse item.IdInvitedUser <> idInvited) Then
                            idPerson = item.IdPerson
                            idInvited = item.IdInvitedUser
                            attempt = 1
                            item.AttemptNumber = attempt
                        ElseIf idInvited <> item.IdInvitedUser Then
                            idPerson = item.IdPerson
                            idInvited = item.IdInvitedUser
                            attempt = 1
                            item.AttemptNumber = attempt
                        Else
                            attempt += 1
                            item.AttemptNumber = attempt
                        End If
                    Next


                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = Nothing
            End Try
            Return result.OrderBy(Function(r) r.DisplayName).ThenBy(Function(r) r.IdPerson).ToList()
        End Function
        Public Function GetQuestionnaireAnswers(idQuestionnaire As Integer, idUsers As List(Of Integer), status As AnswerStatus, onlyLastAttempts As Boolean) As List(Of dtoFullUserAnswerItem)
            Dim result As New List(Of dtoFullUserAnswerItem)
            Try
                Manager.BeginTransaction()
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(qFather) AndAlso idUsers.Any Then
                    Dim answers As List(Of dtoFullUserAnswerItem) = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnaire) Select r).ToList().Select(Function(e) New dtoFullUserAnswerItem(e)).ToList()
                    answers = (From r In answers Where idUsers.Contains(r.Answer.IdPerson) AndAlso (status = AnswerStatus.All OrElse (status = AnswerStatus.Compiling AndAlso Not r.Answer.CompletedOn.HasValue) OrElse (status = AnswerStatus.Completed AndAlso r.Answer.CompletedOn.HasValue)) Select r).ToList()

                    If answers.Any() Then
                        'Dim userAttempts As ILookup(Of Integer, List(Of dtoFullUserAnswerItem)) = (From a In answers Group a By IdPerson = a.Answer.IdPerson Into items = Group).ToLookup(Function(c) c.IdPerson, Function(c) c.items.OrderBy(Function(i) i.Id).ToList)
                        For Each idUser As Integer In idUsers.Distinct.ToList
                            Dim attempt As Integer = 1
                            Dim idPerson As Integer = idUser
                            Dim attempts As List(Of dtoFullUserAnswerItem) = answers.Where(Function(a) a.Answer.IdPerson.Equals(idPerson)).ToList()
                            If Not IsNothing(attempts) AndAlso attempts.Any Then
                                For Each item As dtoFullUserAnswerItem In attempts
                                    item.AttemptNumber = attempt
                                    attempt += 1
                                Next
                                If onlyLastAttempts Then
                                    result.Add(attempts.OrderByDescending(Function(a) a.AttemptNumber).FirstOrDefault())
                                Else
                                    result.AddRange(attempts)
                                End If
                            End If
                        Next
                    End If

                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = New List(Of dtoFullUserAnswerItem)
            End Try
            Return result
        End Function

        Public Function GetQuestionnaireAnswers(idQuestionnaire As Integer, status As AnswerStatus, onlyLastAttempts As Boolean) As List(Of dtoFullUserAnswerItem)
            Dim result As New List(Of dtoFullUserAnswerItem)
            Try
                Manager.BeginTransaction()
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(qFather) Then
                    Dim answers As List(Of dtoFullUserAnswerItem) = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdQuestionnnaire.Equals(idQuestionnaire) Select r).ToList().Select(Function(e) New dtoFullUserAnswerItem(e)).ToList()
                    answers = (From r In answers Where (status = AnswerStatus.All OrElse (status = AnswerStatus.Compiling AndAlso Not r.Answer.CompletedOn.HasValue) OrElse (status = AnswerStatus.Completed AndAlso r.Answer.CompletedOn.HasValue)) Select r).ToList()

                    If answers.Any() Then
                        Dim anonumoys As Person = Manager.GetUnknownUser()
                        Dim idAnonymous As Integer = 0
                        If Not IsNothing(anonumoys) Then
                            idAnonymous = anonumoys.Id
                        End If
                        Dim idUsers As List(Of Int32) = answers.Select(Function(a) a.Answer.IdPerson).ToList
                        'Dim userAttempts As ILookup(Of Integer, List(Of dtoFullUserAnswerItem)) = (From a In answers Group a By IdPerson = a.Answer.IdPerson Into items = Group).ToLookup(Function(c) c.IdPerson, Function(c) c.items.OrderBy(Function(i) i.Id).ToList)
                        For Each idUser As Integer In idUsers.Distinct.ToList
                            Dim attempt As Integer = 1
                            Dim idPerson As Integer = idUser
                            If idPerson <> idAnonymous Then
                                Dim attempts As List(Of dtoFullUserAnswerItem) = answers.Where(Function(a) a.Answer.IdPerson.Equals(idPerson)).ToList()
                                If Not IsNothing(attempts) AndAlso attempts.Any Then
                                    For Each item As dtoFullUserAnswerItem In attempts
                                        item.AttemptNumber = attempt
                                        attempt += 1
                                    Next
                                    If onlyLastAttempts Then
                                        result.Add(attempts.OrderByDescending(Function(a) a.AttemptNumber).FirstOrDefault())
                                    Else
                                        result.AddRange(attempts)
                                    End If
                                End If
                            Else
                                Dim idInvitedUsers As List(Of Integer) = answers.Where(Function(a) (a.Answer.IdPerson.Equals(0) OrElse a.Answer.IdPerson.Equals(idPerson)) AndAlso a.Answer.IdInvitedUser.HasValue()).Select(Function(a) a.Answer.IdInvitedUser.Value).ToList
                                If idInvitedUsers.Any() Then
                                    For Each idInvited As Integer In idInvitedUsers.Distinct.ToList
                                        Dim attempts As List(Of dtoFullUserAnswerItem) = answers.Where(Function(a) a.Answer.IdPerson.Equals(idPerson) AndAlso a.Answer.IdInvitedUser.HasValue AndAlso a.Answer.IdInvitedUser.Value.Equals(idInvited)).ToList()
                                        attempt = 1
                                        If Not IsNothing(attempts) AndAlso attempts.Any Then
                                            For Each item As dtoFullUserAnswerItem In attempts
                                                item.AttemptNumber = attempt
                                                attempt += 1
                                            Next
                                            If onlyLastAttempts Then
                                                result.Add(attempts.OrderByDescending(Function(a) a.AttemptNumber).FirstOrDefault())
                                            Else
                                                result.AddRange(attempts)
                                            End If
                                        End If
                                    Next

                                Else
                                    Dim attempts As List(Of dtoFullUserAnswerItem) = answers.Where(Function(a) a.Answer.IdPerson.Equals(idPerson)).ToList()
                                    If Not IsNothing(attempts) AndAlso attempts.Any Then
                                        For Each item As dtoFullUserAnswerItem In attempts
                                            item.AttemptNumber = attempt
                                            attempt += 1
                                        Next
                                        If onlyLastAttempts Then
                                            result.Add(attempts.OrderByDescending(Function(a) a.AttemptNumber).FirstOrDefault())
                                        Else
                                            result.AddRange(attempts)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If

                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = New List(Of dtoFullUserAnswerItem)
            End Try
            Return result
        End Function

        'Public Function GetQuestionnaireAttempts(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer, status As AnswerStatus, anonymous As String) As List(Of dtoUserAnswerItem)
        '    Dim result As New List(Of dtoUserAnswerItem)
        '    Try
        '        Manager.BeginTransaction()
        '        Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
        '        If Not IsNothing(qFather) Then
        '            result = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
        '                AndAlso (idInvitedUser.Equals(0) OrElse r.IdInvitedUser.Equals(idInvitedUser)) Select r).ToList().Select(Function(e) New dtoUserAnswerItem(e)).ToList()
        '            result = (From r In result Where (status = AnswerStatus.All OrElse (status = AnswerStatus.Compiling AndAlso Not r.CompletedOn.HasValue) OrElse (status = AnswerStatus.Completed AndAlso r.CompletedOn.HasValue)) Select r).ToList()
        '            SetDisplayName(result, anonymous)

        '            Dim attempt As Integer
        '            Dim idPerson As Integer = 0
        '            Dim idInvited As Integer? = 0
        '            For Each item As dtoUserAnswerItem In (From r In result Order By r.IdPerson, r.IdInvitedUser, r.Id Select r).ToList
        '                If idPerson <> item.IdPerson AndAlso (Not item.IdInvitedUser.HasValue OrElse item.IdInvitedUser <> idInvited) Then
        '                    idPerson = item.IdPerson
        '                    idInvited = item.IdInvitedUser
        '                    attempt = 1
        '                    item.AttemptNumber = attempt
        '                ElseIf idInvited <> item.IdInvitedUser Then
        '                    idPerson = item.IdPerson
        '                    idInvited = item.IdInvitedUser
        '                    attempt = 1
        '                    item.AttemptNumber = attempt
        '                Else
        '                    attempt += 1
        '                    item.AttemptNumber = attempt
        '                End If
        '            Next


        '        End If
        '        Manager.Commit()
        '    Catch ex As Exception
        '        Manager.RollBack()
        '        result = Nothing
        '    End Try
        '    Return result.OrderBy(Function(r) r.DisplayName).ThenBy(Function(r) r.IdPerson).ToList()
        'End Function
        Public Function GetAttemptsNumber(ByVal idRandom As Integer, attempts As List(Of dtoUserAnswerBaseItem)) As Integer
            If IsNothing(attempts) OrElse Not attempts.Any() Then
                Return 0
            Else
                Return attempts.Where(Function(a) a.IdRandomQuestionnaire.Equals(idRandom)).Select(Function(a) a.AttemptNumber).FirstOrDefault()
            End If
        End Function
        Public Function GetQuestionnaireBaseAttempts(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer) As List(Of dtoUserAnswerBaseItem)
            Dim result As New List(Of dtoUserAnswerBaseItem)
            Try
                result = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                    AndAlso (idInvitedUser.Equals(0) OrElse r.IdInvitedUser.Equals(idInvitedUser)) Order By r.Id Select r).ToList().Select(Function(e) New dtoUserAnswerBaseItem(e)).ToList()
                Dim attempt As Integer = 1
                Dim idPerson As Integer = 0
                Dim idInvited As Integer? = 0
                For Each item As dtoUserAnswerBaseItem In result
                    item.AttemptNumber = attempt
                    attempt += 1
                Next
            Catch ex As Exception
                result = New List(Of dtoUserAnswerBaseItem)
            End Try
            Return result
        End Function
        Public Function GetQuestionnaireAttempts(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer, status As AnswerStatus, anonymous As String) As List(Of dtoUserAnswerItem)
            Dim result As New List(Of dtoUserAnswerItem)
            Try
                Manager.BeginTransaction()
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not IsNothing(qFather) Then
                    result = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                        AndAlso (idInvitedUser.Equals(0) OrElse r.IdInvitedUser.Equals(idInvitedUser)) Select r).ToList().Select(Function(e) New dtoUserAnswerItem(e)).ToList()
                    result = (From r In result Where (status = AnswerStatus.All OrElse (status = AnswerStatus.Compiling AndAlso Not r.CompletedOn.HasValue) OrElse (status = AnswerStatus.Completed AndAlso r.CompletedOn.HasValue)) Select r).ToList()
                    SetDisplayName(result, anonymous)

                    Dim attempt As Integer
                    Dim idPerson As Integer = 0
                    Dim idInvited As Integer? = 0
                    For Each item As dtoUserAnswerItem In (From r In result Order By r.IdPerson, r.IdInvitedUser, r.Id Select r).ToList
                        If idPerson <> item.IdPerson AndAlso (Not item.IdInvitedUser.HasValue OrElse item.IdInvitedUser <> idInvited) Then
                            idPerson = item.IdPerson
                            idInvited = item.IdInvitedUser
                            attempt = 1
                            item.AttemptNumber = attempt
                        ElseIf idInvited <> item.IdInvitedUser Then
                            idPerson = item.IdPerson
                            idInvited = item.IdInvitedUser
                            attempt = 1
                            item.AttemptNumber = attempt
                        Else
                            attempt += 1
                            item.AttemptNumber = attempt
                        End If
                    Next


                End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
                result = Nothing
            End Try
            Return result.OrderBy(Function(r) r.DisplayName).ThenBy(Function(r) r.IdPerson).ToList()
        End Function

        Public Function IsValidAttempts(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer, idCurrentAnswer As Integer) As Boolean
            Dim isValid As Boolean = True
            Try
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If qFather.MaxAttempts > 0 Then
                    Dim answers As List(Of LazyUserResponse) = Nothing
                    answers = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(idUser) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                        AndAlso (idInvitedUser.Equals(0) OrElse (r.IdInvitedUser.HasValue AndAlso r.IdInvitedUser.Value.Equals(idInvitedUser))) Select r).ToList().OrderByDescending(Function(r) r.Id).ToList()

                    If idCurrentAnswer < 1 Then
                        Return answers.Count < qFather.MaxAttempts
                    Else
                        Dim idIndex As Integer = answers.Select(Function(r) r.Id).OrderByDescending(Function(r) r).ToList().IndexOf(idCurrentAnswer) + 1
                        If answers.Count >= qFather.MaxAttempts AndAlso idIndex > qFather.MaxAttempts Then
                            isValid = False
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            Return isValid
        End Function

        Public Function IsValidSave(idQuestionnaire As Integer, idUser As Integer, idInvitedUser As Integer, idCurrentAnswer As Integer) As Boolean
            Dim isValid As Boolean = True
            Try
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If qFather.MaxAttempts > 0 Then
                    isValid = IsValidAttempts(idQuestionnaire, idUser, idInvitedUser, idCurrentAnswer)
                End If
                If isValid AndAlso Not qFather.EditAnswer Then
                    'Dim answers As List(Of LazyUserResponse) = (
                    '    From r In Manager.GetIQ(Of LazyUserResponse)()
                    '    Where r.IdPerson.Equals(idUser) _
                    '        AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                    '        AndAlso (idInvitedUser.Equals(0) _
                    '        OrElse (r.IdInvitedUser.HasValue AndAlso r.IdInvitedUser.Value.Equals(idInvitedUser))
                    '        )
                    '    Select r).ToList().OrderByDescending(Function(r) r.Id).ToList()


                    'Dim answers As List(Of LazyUserResponse) = (
                    '    From r In Manager.GetIQ(Of LazyUserResponse)()
                    '    Where r.IdPerson.Equals(idUser) _
                    '        AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) _
                    '        AndAlso (r.IdInvitedUser.HasValue AndAlso r.IdInvitedUser = 0) _
                    '        OrElse (r.IdInvitedUser.HasValue AndAlso r.IdInvitedUser = idInvitedUser)
                    '    Select r).ToList().OrderByDescending(Function(r) r.Id).ToList()

                    Dim zero As Integer = 0

                    Dim answers As List(Of LazyUserResponse) = (
                        From r In Manager.GetIQ(Of LazyUserResponse)()
                        Where r.IdPerson.Equals(idUser) _
                            AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire)
                        Select r).ToList()

                    'Try
                    '    answers = answers.Where(Function(a) a.IdInvitedUser = 0 OrElse a.IdInvitedUser = idInvitedUser).ToList()
                    'Catch ex As Exception

                    'End Try

                    If (idInvitedUser > 0) Then
                        Try
                            answers = answers.Where(Function(a) a.IdInvitedUser.Equals(idInvitedUser)).ToList()
                        Catch ex As Exception

                        End Try
                    Else
                        Try
                            answers = answers.Where(Function(a) Not a.IdInvitedUser.HasValue OrElse a.IdInvitedUser.Equals(zero)).ToList()
                        Catch ex As Exception

                        End Try
                    End If


                    'AndAlso ()

                    If Not IsNothing(answers) Then
                        answers = answers.OrderByDescending(Function(r) r.Id).ToList()
                    End If


                    Dim answer As LazyUserResponse = Nothing
                    If idCurrentAnswer = 0 Then
                        idCurrentAnswer = answers.Select(Function(a) a.Id).FirstOrDefault()
                    End If
                    answer = answers.Where(Function(a) a.Id.Equals(idCurrentAnswer)).FirstOrDefault()
                    isValid = Not (Not IsNothing(answer) AndAlso answer.CompletedOn.HasValue)
                End If
            Catch ex As Exception

            End Try
            Return isValid
        End Function
        Public Function IsAnswerSavingAllowed(idQuestionnaire As Integer, idCurrentAnswer As Integer) As Boolean
            Dim isValid As Boolean = True
            Try
                Dim qFather As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnaire)
                If Not qFather.EditAnswer AndAlso idCurrentAnswer > 0 Then
                    Dim answer As LazyUserResponse = Manager.Get(Of LazyUserResponse)(idCurrentAnswer)
                    isValid = Not (Not IsNothing(answer) AndAlso answer.CompletedOn.HasValue)
                    If isValid AndAlso qFather.TimeSavingValidity.HasValue AndAlso qFather.TimeSavingValidity.Value > 0 AndAlso Not IsNothing(answer) AndAlso answer.StartedOn.HasValue Then
                        isValid = Not ((qFather.TimeSavingValidity.Value * 60) - DateDiff(DateInterval.Minute, answer.StartedOn.Value, DateTime.Now) < 1)
                    End If
                End If
            Catch ex As Exception

            End Try
            Return isValid
        End Function
#End Region

#Region "Library Settings"
        Public Function GetLibraryAssignments(ByVal idLibrary As Integer, ByVal forPortal As Boolean) As List(Of dtoQuestionnaireAssignment)
            Dim items As New List(Of dtoQuestionnaireAssignment)
            Try
                'If forPortal Then
                '    'items =   (from pa in Manager.GetAll<BaseForPaperPersonTypeAssignment>(pa => pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None)
                '    'select  new dtoCallPersonTypeAssignment() { AssignedTo= pa.AssignedTo, Deleted= BaseStatusDeleted.None, 
                '    'Id= pa.Id, Deny= pa.Deny}).ToList().ForEach(pa=> assignments.Add(pa));
                'Else
                '    '(from pa in Manager.GetAll<BaseForPaperRoleAssignment>(pa => pa.BaseForPaper.Id == idCall && !pa.Deny && pa.Deleted == BaseStatusDeleted.None && pa.AssignedTo!=null)
                '    '        Select New dtoCallRoleAssignment()
                '    '        {
                '    '            AssignedTo = pa.AssignedTo,
                '    '            Deleted = BaseStatusDeleted.None,
                '    '            Id = pa.Id,
                '    '            Deny = pa.Deny
                '    '        }).ToList().ForEach(pa => assignments.Add(pa));

                '    items.AddRange((From pa In Manager.GetAll(Of LazyQuestionnaireCommunityAssignment)(Function(pa) pa.Questionnaire.Id.Equals(idLibrary) AndAlso pa.Deleted.Equals(BaseStatusDeleted.None) AndAlso Not pa.AssignedTo.Equals(Nothing))
                '        Select New dtoQuestionnaireCommunityAssignment() With {
                '            .AssignedTo = pa.AssignedTo,
                '            .Deleted = BaseStatusDeleted.None,
                '            .Id = pa.Id}).ToList())
                'End If
                Dim pAssignments As List(Of dtoQuestionnairePersonAssignment)
                pAssignments = (From pa In Manager.GetAll(Of LazyQuestionnairePersonAssignment)(Function(pa) pa.Questionnaire.Id.Equals(idLibrary) AndAlso pa.Deleted.Equals(BaseStatusDeleted.None))
                         Select New dtoQuestionnairePersonAssignment() With {
                             .AssignedTo = pa.AssignedTo,
                             .Deleted = BaseStatusDeleted.None,
                             .Id = pa.Id}).ToList()
                For Each p As dtoQuestionnairePersonAssignment In pAssignments
                    items.Add(p)
                Next

            Catch ex As Exception

            End Try
            Return items
        End Function

        Public Function SaveLibraryAvailability(ByVal idLibrary As Integer, ByVal access As LibraryAccessibility, users As List(Of Integer), roles As List(Of Integer), profileTypes As List(Of Integer)) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim l As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idLibrary)
                Dim p As Person = Manager.GetPerson(UC.CurrentUserID)
                If Not IsNothing(l) AndAlso l.IdType = QuestionnaireType.QuestionLibrary Then
                    For Each item As LazyQuestionnaireAssignment In (From i In Manager.GetIQ(Of LazyQuestionnaireAssignment)() Where i.Questionnaire.Id.Equals(idLibrary) AndAlso i.Deleted.Equals(BaseStatusDeleted.None) Select i).ToList()
                        item.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress)
                        Manager.SaveOrUpdate(item)
                    Next
                    Select Case access
                        'Case LibraryAccessibility.someProfileType
                        '    For Each item As LazyQuestionnaireAssignment In (From i In Manager.GetIQ(Of LazyQuestionnaireAssignment)() Where i.Deleted = BaseStatusDeleted.None Select i).ToList()
                        '        item.SetDeleteMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress)
                        '        Manager.SaveOrUpdate(item)
                        '    Next
                        Case LibraryAccessibility.someUser
                            Dim availableUsers As New List(Of Integer)
                            If Not users.Contains(p.Id) Then
                                users.Add(p.Id)
                            End If
                            For Each item As LazyQuestionnairePersonAssignment In (From i In Manager.GetIQ(Of LazyQuestionnairePersonAssignment)() Where i.Questionnaire.Id.Equals(idLibrary) AndAlso users.Contains(i.AssignedTo.Id) Select i).ToList()
                                item.Deleted = BaseStatusDeleted.None
                                item.UpdateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress)
                                availableUsers.Add(item.AssignedTo.Id)
                                Manager.SaveOrUpdate(item)
                            Next
                            For Each idUser As Integer In users.Except(availableUsers).ToList
                                Dim pAssignment As New LazyQuestionnairePersonAssignment
                                pAssignment.AssignedTo = Manager.GetPerson(idUser)
                                pAssignment.Questionnaire = l
                                pAssignment.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress)
                                Manager.SaveOrUpdate(pAssignment)
                            Next
                    End Select
                    l.LibraryAccessibility = access
                    Manager.SaveOrUpdate(l)
                End If

                Manager.Commit()
                result = True

            Catch ex As Exception
                result = False
                Manager.RollBack()
            End Try
            Return result
        End Function
        Public Function CloneLibraryAvailability(ByVal idLibrary As Integer, ByVal idNewLibrary As Integer) As Boolean
            Dim result As Boolean = False
            Try
                Manager.BeginTransaction()
                Dim oldL As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idLibrary)
                Dim newL As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idNewLibrary)
                Dim p As Person = Manager.GetPerson(UC.CurrentUserID)
                If Not IsNothing(p) AndAlso Not IsNothing(oldL) AndAlso oldL.IdType = QuestionnaireType.QuestionLibrary AndAlso Not IsNothing(newL) AndAlso newL.IdType = QuestionnaireType.QuestionLibrary Then
                    For Each item As LazyQuestionnaireAssignment In (From i In Manager.GetIQ(Of LazyQuestionnaireAssignment)() Where i.Questionnaire.Id = idLibrary AndAlso i.Deleted = BaseStatusDeleted.None Select i).ToList()
                        If TypeOf item Is LazyQuestionnairePersonAssignment Then
                            Dim nItem As New LazyQuestionnairePersonAssignment()
                            nItem.CreateMetaInfo(p, UC.IpAddress, UC.ProxyIpAddress)
                            nItem.AssignedTo = DirectCast(item, LazyQuestionnairePersonAssignment).AssignedTo
                            nItem.Questionnaire = newL
                            Manager.SaveOrUpdate(nItem)
                        End If
                    Next
                    newL.LibraryAccessibility = oldL.LibraryAccessibility
                    Manager.SaveOrUpdate(newL)
                End If

                Manager.Commit()
                result = True

            Catch ex As Exception
                result = False
                Manager.RollBack()
            End Try
            Return result
        End Function

        Public Function GetAvailableLibraries(ByVal idPerson As Integer, ByVal idCommunity As Integer, ByVal idLanguage As Integer) As List(Of Questionario)
            Dim items As New List(Of Questionario)
            Try
                Dim c As Community = Manager.Get(Of Community)(idCommunity)
                Dim p As Person = Manager.Get(Of Person)(idPerson)
                If Not IsNothing(p) AndAlso Not IsNothing(c) Then
                    items = GetCommunityLibraries(idCommunity, idLanguage)
                    items.AddRange(GetLibrariesByUser(p, idLanguage, items.Select(Function(q) q.id).Distinct.ToList()))

                    If c.IdFather > 0 Then
                        Dim father As Community = (From cm In Manager.GetIQ(Of Community)() Where cm.IdFather.Equals(0) AndAlso cm.IdOrganization.Equals(c.IdOrganization) Select cm).ToList.FirstOrDefault()
                        If Not IsNothing(father) Then
                            items.AddRange(GetCommunityLibraries(father.Id, idLanguage, items.Select(Function(qA) qA.id).ToList()))
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            Return items.OrderBy(Function(i) i.nome).ToList
        End Function

        Public Function GetCommunityLibraries(ByVal idCommunity As Integer, ByVal idLanguage As Integer, Optional ByVal exceptId As List(Of Integer) = Nothing) As List(Of Questionario)
            Dim items As New List(Of Questionario)

            ' Find groups 
            Dim groups As List(Of LazyGroup) = (From g In Manager.GetIQ(Of LazyGroup)() Where g.IdCommunity = idCommunity).ToList()
            For Each lGroup As LazyGroup In groups
                Dim idGroup As Integer = lGroup.Id
                Dim qList As List(Of LazyQuestionnaire) = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where q.IdGroup.Equals(idGroup) AndAlso q.IdType.Equals(CInt(QuestionnaireType.QuestionLibrary)) Select q).ToList()

                For Each q As LazyQuestionnaire In qList.Where(Function(qi) exceptId Is Nothing OrElse Not exceptId.Contains(qi.Id)).ToList
                    Dim translation As LazyQuestionnaireTranslation = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(q.Id) AndAlso l.IdLanguage.Equals(idLanguage) AndAlso l.IsDeleted = False Select l).Skip(0).Take(1).ToList().FirstOrDefault()
                    If Not IsNothing(translation) Then
                        items.Add(New Questionario() With {.id = q.Id, .nDomandeDiffAlta = q.HighQuestionNumber, .nDomandeDiffBassa = q.LowQuestionNumber, .nDomandeDiffMedia = q.MediumQuestionNumber, .nome = translation.Name, .descrizione = translation.Description, .idLingua = translation.IdLanguage, .idQuestionarioMultilingua = translation.Id})
                    Else
                        Dim translations As List(Of LazyQuestionnaireTranslation) = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(q.Id) AndAlso l.IsDeleted = False Select l).ToList
                        For Each t As LazyQuestionnaireTranslation In translations
                            items.Add(New Questionario() With {.id = q.Id, .nDomandeDiffAlta = q.HighQuestionNumber, .nDomandeDiffBassa = q.LowQuestionNumber, .nDomandeDiffMedia = q.MediumQuestionNumber, .nome = t.Name, .descrizione = t.Description, .idLingua = t.IdLanguage, .idQuestionarioMultilingua = t.Id})
                        Next
                    End If
                Next
            Next
            Return items.OrderBy(Function(i) i.nome).ToList
        End Function
        Public Function GetLibrariesByUser(ByVal user As Person, ByVal idLanguage As Integer, Optional ByVal exceptId As List(Of Integer) = Nothing) As List(Of Questionario)
            Dim items As New List(Of Questionario)

            'AndAlso user.Id.Equals(pa.AssignedTo.Id)
            Dim userAssigned As List(Of Integer) = (From pa In Manager.GetIQ(Of LazyQuestionnairePersonAssignment)() _
                                                          Where pa.Deleted.Equals(BaseStatusDeleted.None) AndAlso Not exceptId.Contains(pa.Questionnaire.Id) Select pa.Questionnaire.Id).ToList
            Dim qAssigned As List(Of LazyQuestionnaire) = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where Not exceptId.Contains(q.Id) AndAlso userAssigned.Contains(q.Id) AndAlso q.IdType.Equals(CInt(QuestionnaireType.QuestionLibrary)) Select q).ToList()
            For Each q As LazyQuestionnaire In qAssigned
                Dim translation As LazyQuestionnaireTranslation = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(q.Id) AndAlso l.IdLanguage.Equals(idLanguage) AndAlso l.IsDeleted = False Select l).Skip(0).Take(1).ToList().FirstOrDefault()
                If Not IsNothing(translation) Then
                    items.Add(New Questionario() With {.id = q.Id, .nDomandeDiffAlta = q.HighQuestionNumber, .nDomandeDiffBassa = q.LowQuestionNumber, .nDomandeDiffMedia = q.MediumQuestionNumber, .nome = translation.Name, .descrizione = translation.Description, .idLingua = translation.IdLanguage, .idQuestionarioMultilingua = translation.Id})
                Else
                    Dim translations As List(Of LazyQuestionnaireTranslation) = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(q.Id) AndAlso l.IsDeleted = False Select l).ToList
                    For Each t As LazyQuestionnaireTranslation In translations
                        items.Add(New Questionario() With {.id = q.Id, .nDomandeDiffAlta = q.HighQuestionNumber, .nDomandeDiffBassa = q.LowQuestionNumber, .nDomandeDiffMedia = q.MediumQuestionNumber, .nome = t.Name, .descrizione = t.Description, .idLingua = t.IdLanguage, .idQuestionarioMultilingua = t.Id})
                    Next
                End If
            Next
            Return items.OrderBy(Function(i) i.nome).ToList
        End Function

        Public Function GetQuestionnaireLibrariesDifficulty(ByVal idQuestionnnaire As Integer) As List(Of DifficultyLevel)
            Dim items As Dictionary(Of Integer, List(Of DifficultyLevel)) = GetLibrariesDifficulty(idQuestionnnaire)
            Dim results As New List(Of DifficultyLevel)
            For Each r As KeyValuePair(Of Integer, List(Of DifficultyLevel)) In items
                results.AddRange(r.Value)
            Next
            Return results.Distinct.ToList()
        End Function
        Public Function GetLibrariesDifficulty(ByVal idQuestionnnaire As Integer) As Dictionary(Of Integer, List(Of DifficultyLevel))
            Dim results As New Dictionary(Of Integer, List(Of DifficultyLevel))
            Dim q As LazyQuestionnaire = Manager.Get(Of LazyQuestionnaire)(idQuestionnnaire)
            Dim levels As New List(Of DifficultyLevel)

            If IsNothing(q) Then
                results.Add(idQuestionnnaire, levels)
            ElseIf (q.IdType <> QuestionnaireType.AutoEvaluation AndAlso q.IdType <> QuestionnaireType.Random AndAlso q.IdType <> QuestionnaireType.RandomMultipleAttempts) Then
                If q.HighQuestionNumber > 0 Then
                    levels.Add(DifficultyLevel.high)
                End If
                If q.LowQuestionNumber > 0 Then
                    levels.Add(DifficultyLevel.easy)
                End If
                If q.MediumQuestionNumber > 0 Then
                    levels.Add(DifficultyLevel.medium)
                End If
                results.Add(idQuestionnnaire, levels)
            Else
                Dim libraries As List(Of LazyAssignedLibrary) = (From l In Manager.GetIQ(Of LazyAssignedLibrary)() Where l.IdQuestionnnaire.Equals(idQuestionnnaire) Select l).ToList()
                Dim idQuestionnaires As List(Of Integer) = libraries.Select(Function(l) l.IdLibrary).ToList()
                Dim qlibraries As List(Of LazyQuestionnaire) = (From lq In Manager.GetIQ(Of LazyQuestionnaire)() Where idQuestionnaires.Contains(lq.Id) Select lq).ToList()
                For Each library As LazyQuestionnaire In qlibraries
                    levels = New List(Of DifficultyLevel)
                    If library.HighQuestionNumber > 0 Then
                        levels.Add(DifficultyLevel.high)
                    End If
                    If library.LowQuestionNumber > 0 Then
                        levels.Add(DifficultyLevel.easy)
                    End If
                    If library.MediumQuestionNumber > 0 Then
                        levels.Add(DifficultyLevel.medium)
                    End If
                    results.Add(library.Id, levels)
                Next
                '                SELECT     dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_Id, dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_nDomandeDiffBassa, 
                '                      dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_nDomandeDiffMedia, dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_nDomandeDiffAlta, 
                '                      dbo.QS_QUESTIONARIO_MULTILINGUA.QSML_Nome, dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_LIBRERIA_Id, 
                '                      dbo.QS_QUESTIONARIO.QSTN_nDomandeDiffBassa, dbo.QS_QUESTIONARIO.QSTN_nDomandeDiffMedia, 
                '                      dbo.QS_QUESTIONARIO.QSTN_nDomandeDiffAlta, dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_QSTN_Id,dbo.QS_QUESTIONARIO_MULTILINGUA.QSML_IdLingua
                'FROM         dbo.QS_QUESTIONARIO_MULTILINGUA INNER JOIN
                '                      dbo.QS_QUESTIONARIO ON dbo.QS_QUESTIONARIO_MULTILINGUA.QSML_QSTN_Id = dbo.QS_QUESTIONARIO.QSTN_Id INNER JOIN
                '                      dbo.QS_LK_QUESTIONARIO_LIBRERIA ON dbo.QS_QUESTIONARIO.QSTN_Id = dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_LIBRERIA_Id
                'WHERE     
                '(dbo.QS_LK_QUESTIONARIO_LIBRERIA.LKQL_QSTN_Id = @idQuestionario) 

                '          End


            End If
            Return results
        End Function
#End Region
        Public Function GetStatisticsFilter(ByVal idQuestionnaire As Integer) As QuestionnaireStatisticsFilter
            Dim filter As New QuestionnaireStatisticsFilter
            Try
                Manager.BeginTransaction()

                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return QuestionnaireStatisticsFilter.GetDefault()
        End Function

        Public Function GetQuestionaireName(ByVal idQuestionnaire As Long, ByVal idLanguage As Integer) As String
            Dim name As String = ""
            Try
                Dim translations As List(Of LazyQuestionnaireTranslation) = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(CInt(idQuestionnaire)) Select l).ToList
                Dim translation As LazyQuestionnaireTranslation = (From l In translations Where l.IsDeleted.Equals(False) AndAlso l.IdLanguage.Equals(idLanguage) Select l).FirstOrDefault()
                If Not IsNothing(translation) Then
                    name = translation.Name
                ElseIf translations.Where(Function(t) t.IsDefault).Any Then
                    name = translations.Where(Function(t) t.IsDefault).FirstOrDefault.Name
                ElseIf translations.Count > 0 Then
                    name = translations(0).Name
                End If
            Catch ex As Exception

            End Try
            Return name
        End Function
        Public Function QuestionnaireHasAnonymousValues(ByVal idQuestionnaire As Long) As Boolean
            Dim result As Boolean = False
            Try
                result = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where q.Id.Equals(idQuestionnaire) AndAlso q.isAnonymous Select q.Id).Any()
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Function GetQuestionairesName(ByVal identifiers As List(Of Long), ByVal idLanguage As Integer) As Dictionary(Of Long, String)
            Dim items As New Dictionary(Of Long, String)
            Try
                Manager.BeginTransaction()
                Dim query = (From q In Manager.GetIQ(Of LazyQuestionnaire)() Where identifiers.Contains(CLng(q.Id)) Select q).ToList
                For Each q As LazyQuestionnaire In query
                    Dim translations As List(Of LazyQuestionnaireTranslation) = (From l In Manager.GetIQ(Of LazyQuestionnaireTranslation)() Where l.IdQuestionnnaire.Equals(q.Id) AndAlso l.IsDeleted = False Select l).ToList
                    Dim translation As LazyQuestionnaireTranslation = (From l In translations Where l.IdLanguage.Equals(idLanguage) Select l).FirstOrDefault()
                    If Not IsNothing(translation) Then
                        items.Add(CLng(q.Id), translation.Name)
                    ElseIf translations.Where(Function(t) t.IsDefault).Any Then
                        items.Add(CLng(q.Id), translations.Where(Function(t) t.IsDefault).FirstOrDefault.Name)
                    ElseIf translations.Count > 0 Then
                        items.Add(CLng(q.Id), translations(0).Name)
                    End If
                Next
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return items
        End Function
        Public Function DeleteStatistics(ByVal idQuestionnaire As Integer, ByVal userId As Integer) As Boolean
            Dim q As LazyQuestionnaire = GetItem(Of LazyQuestionnaire)(idQuestionnaire)
            If Not IsNothing(q) Then
                Dim responses As List(Of LazyUserResponse) = (From r In Manager.GetIQ(Of LazyUserResponse)() Where r.IdPerson.Equals(userId) AndAlso r.IdQuestionnnaire.Equals(idQuestionnaire) Select r).ToList

                For Each response As LazyUserResponse In responses
                    If q.IdType = QuestionnaireType.Random OrElse q.IdType = QuestionnaireType.RandomMultipleAttempts OrElse q.IdType = QuestionnaireType.AutoEvaluation Then
                        DALRisposte.cancellaRispostaQuestionarioRandomBYID(response.Id, q.Id)
                    Else
                        DALRisposte.cancellaRispostaBYID(response.Id)
                    End If
                Next
            End If
        End Function
        Public Function DeleteStatistics(ByVal idQuestionnaire As Integer) As Boolean
            Dim q As LazyQuestionnaire = GetItem(Of LazyQuestionnaire)(idQuestionnaire)
            If Not IsNothing(q) Then
                If q.IdType = QuestionnaireType.Random OrElse q.IdType = QuestionnaireType.RandomMultipleAttempts OrElse q.IdType = QuestionnaireType.AutoEvaluation Then
                    DALRisposte.cancellaRisposteBYIDQuestionarioRandom(_Context, idQuestionnaire)
                Else
                    DALRisposte.cancellaRisposteBYIDQuestionario(idQuestionnaire, UC.CurrentUserID)
                End If
            End If

        End Function


        Public Function GetLazyAssociatedQuestion(idQuestionnnaire As Integer, Optional idRandomQuestionnaire As Integer = 0) As List(Of LazyAssociatedQuestion)
            Dim questions As List(Of LazyAssociatedQuestion)

            Try
                If idRandomQuestionnaire > 0 Then
                    questions = (From q In Manager.Linq(Of LazyAssociatedQuestion)() _
                                                Where q.IdQuestionnnaire.Equals(idQuestionnnaire) Select q).ToList().Where(Function(q) q.IdRandomQuestionnnaire.HasValue AndAlso q.IdRandomQuestionnnaire.Value.Equals(idRandomQuestionnaire)).ToList()
                Else
                    questions = (From q In Manager.Linq(Of LazyAssociatedQuestion)() _
                                                 Where q.IdQuestionnnaire.Equals(idQuestionnnaire) Select q).ToList()
                End If

            Catch ex As Exception
                questions = New List(Of LazyAssociatedQuestion)
            End Try

            Return questions
        End Function
#Region "Questionnaire Answers"
        Public Sub SetDisplayName(ByVal answers As List(Of dtoUserAnswerItem), anonymous As String)
            Try
                Dim idAnonymous As Integer = 0
                Dim aUser As Person = (From p In Manager.GetIQ(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If



                Dim persons As Dictionary(Of Integer, String) = GetDisplayName(answers.Where(Function(r) r.IdPerson > 0 AndAlso r.IdPerson <> idAnonymous).Select(Function(r) r.IdPerson).Distinct.ToList())
                Dim invitedUsers As Dictionary(Of Integer, String) = GetInvitedUsersDisplayName(answers.Where(Function(r) r.IdInvitedUser.HasValue AndAlso r.IdInvitedUser > 0 AndAlso (r.IdPerson.Equals(0) OrElse r.IdPerson.Equals(idAnonymous))).Select(Function(r) r.IdInvitedUser.Value).Distinct.ToList())


                persons.ToList.ForEach(Sub(s) answers.Where(Function(a) a.IdPerson = s.Key).ToList.ForEach(Sub(aa) aa.DisplayName = s.Value))
                invitedUsers.ToList.ForEach(Sub(s) answers.Where(Function(a) a.IdInvitedUser.HasValue AndAlso a.IdInvitedUser.Value = s.Key).ToList.ForEach(Sub(aa) aa.DisplayName = s.Value))
                answers.Where(Function(a) String.IsNullOrEmpty(a.DisplayName)).ToList().ForEach(Sub(s) s.DisplayName = anonymous)
            Catch ex As Exception

            End Try

        End Sub
        Public Function GetDisplayNameByResponses(ByVal results As List(Of RispostaQuestionario), anonymous As String) As Dictionary(Of String, String)
            Dim items As New Dictionary(Of String, String)
            Try
                Manager.BeginTransaction()
                Dim idAnonymous As Integer = 0
                Dim aUser As Person = (From p In Manager.GetIQ(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If

                Dim persons As Dictionary(Of Integer, String) = GetDisplayName(results.Where(Function(r) r.idPersona > 0 AndAlso r.idPersona <> idAnonymous).Select(Function(r) r.idPersona).Distinct.ToList())
                Dim invitedUsers As Dictionary(Of Integer, String) = GetInvitedUsersDisplayName(results.Where(Function(r) r.idUtenteInvitato > 0 AndAlso (r.idPersona.Equals(0) OrElse r.idPersona.Equals(idAnonymous))).Select(Function(r) r.idUtenteInvitato).Distinct.ToList())

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, anonymous)
                End If
                items.Add("p_0", anonymous)
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return items
        End Function

        Public Function GetDtoDisplayNameByResponses(ByVal results As List(Of RispostaQuestionario), anonymous As String) As Dictionary(Of String, dtoDisplayName)
            Dim items As New Dictionary(Of String, dtoDisplayName)
            Try
                Manager.BeginTransaction()
                Dim idAnonymous As Integer = 0
                Dim aUser As Person = (From p In Manager.GetIQ(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If

                Dim persons As Dictionary(Of Integer, dtoDisplayName) = GetDtoDisplayName(results.Where(Function(r) r.idPersona > 0 AndAlso r.idPersona <> idAnonymous).Select(Function(r) r.idPersona).Distinct.ToList())
                Dim invitedUsers As Dictionary(Of Integer, dtoDisplayName) = GetInvitedUsersDtoDisplayName(results.Where(Function(r) r.idUtenteInvitato > 0 AndAlso (r.idPersona.Equals(0) OrElse r.idPersona.Equals(idAnonymous))).Select(Function(r) r.idUtenteInvitato).Distinct.ToList())

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, New dtoDisplayName(anonymous))
                End If
                items.Add("p_0", New dtoDisplayName(anonymous))
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return items
        End Function
        Public Function GetDtoDisplayNameByAnonymousResponses(ByVal results As List(Of RispostaQuestionario), anonymous As String) As Dictionary(Of String, dtoDisplayName)
            Dim items As New Dictionary(Of String, dtoDisplayName)
            Try
                Dim idAnonymous As Integer = 0
                Dim aUser As litePerson = (From p In Manager.GetIQ(Of litePerson)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If
                Dim extraInfo As Dictionary(Of Integer, String) = GetInternalInvitedUsersExtraInfo(results.Where(Function(r) r.idUtenteInvitato > 0 AndAlso r.idPersona > 0).Select(Function(r) r.idUtenteInvitato).Distinct().ToList())
                Dim persons As Dictionary(Of Integer, dtoDisplayName) = results.Where(Function(r) r.idPersona > 0 AndAlso r.idPersona <> idAnonymous).Select(Function(r) r.idPersona).Distinct.ToDictionary(Of Integer, dtoDisplayName)(Function(f) f, Function(x) New dtoDisplayName(anonymous))
                Dim invitedUsers As Dictionary(Of Integer, dtoDisplayName) = GetInvitedUsersDtoAnonymousDisplayName(results.Where(Function(r) r.idUtenteInvitato > 0 AndAlso (r.idPersona.Equals(0) OrElse r.idPersona.Equals(idAnonymous))).Select(Function(r) r.idUtenteInvitato).Distinct.ToList(), anonymous)

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, New dtoDisplayName(anonymous))
                End If
                items.Add("p_0", New dtoDisplayName(anonymous))
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))

            Catch ex As Exception

            End Try
            Return items
        End Function



        Public Function GetDisplayNameByResponses(ByVal results As List(Of dtoFullUserAnswerItem), anonymous As String) As Dictionary(Of String, String)
            Dim items As New Dictionary(Of String, String)
            Try
                Manager.BeginTransaction()
                Dim idAnonymous As Integer = 0
                Dim aUser As Person = (From p In Manager.GetIQ(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If

                Dim persons As Dictionary(Of Integer, String) = GetDisplayName(results.Where(Function(r) r.Answer.IdPerson > 0 AndAlso r.Answer.IdPerson <> idAnonymous).Select(Function(r) r.Answer.IdPerson).Distinct.ToList())
                Dim invitedUsers As Dictionary(Of Integer, String) = GetInvitedUsersDisplayName(results.Where(Function(r) r.Answer.IdInvitedUser.HasValue AndAlso r.Answer.IdInvitedUser.Value > 0 AndAlso (r.Answer.IdPerson.Equals(0) OrElse r.Answer.IdPerson.Equals(idAnonymous))).Select(Function(r) r.Answer.IdInvitedUser.Value).Distinct.ToList())

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, anonymous)
                End If
                items.Add("p_0", anonymous)
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return items
        End Function
        Public Function GetDtoDisplayNameByResponses(ByVal results As List(Of dtoFullUserAnswerItem), anonymous As String) As Dictionary(Of String, dtoDisplayName)
            Dim items As New Dictionary(Of String, dtoDisplayName)
            Try
                Manager.BeginTransaction()
                Dim idAnonymous As Integer = 0
                Dim aUser As litePerson = (From p In Manager.GetIQ(Of litePerson)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If

                Dim persons As Dictionary(Of Integer, dtoDisplayName) = GetDtoDisplayName(results.Where(Function(r) r.Answer.IdPerson > 0 AndAlso r.Answer.IdPerson <> idAnonymous).Select(Function(r) r.Answer.IdPerson).Distinct.ToList())
                Dim invitedUsers As Dictionary(Of Integer, dtoDisplayName) = GetInvitedUsersDtoDisplayName(results.Where(Function(r) r.Answer.IdInvitedUser.HasValue AndAlso r.Answer.IdInvitedUser.Value > 0 AndAlso (r.Answer.IdPerson.Equals(0) OrElse r.Answer.IdPerson.Equals(idAnonymous))).Select(Function(r) r.Answer.IdInvitedUser.Value).Distinct.ToList())

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, New dtoDisplayName(anonymous))
                End If
                items.Add("p_0", New dtoDisplayName(anonymous))
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return items
        End Function

        Public Function GetDtoDisplayNameByAnonymousResponses(ByVal results As List(Of dtoFullUserAnswerItem), anonymous As String) As Dictionary(Of String, dtoDisplayName)
            Dim items As New Dictionary(Of String, dtoDisplayName)
            Try
                Dim idAnonymous As Integer = 0
                Dim aUser As litePerson = (From p In Manager.GetIQ(Of litePerson)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList.FirstOrDefault()
                If Not IsNothing(aUser) Then
                    idAnonymous = aUser.Id
                End If

                'Dim extraInfo As Dictionary(Of Integer, String) = GetInternalInvitedUsersExtraInfo(results.Where(Function(r) r.Answer.IdInvitedUser > 0 AndAlso r.Answer.IdPerson > 0).Select(Function(r) r.Answer.IdInvitedUser).Distinct().ToList())
                Dim persons As Dictionary(Of Integer, dtoDisplayName) = results.Where(Function(r) r.Answer.IdPerson > 0 AndAlso r.Answer.IdPerson <> idAnonymous).Select(Function(r) r.Answer.IdPerson).Distinct.ToDictionary(Of Integer, dtoDisplayName)(Function(f) f, Function(x) New dtoDisplayName(anonymous))
                Dim invitedUsers As Dictionary(Of Integer, dtoDisplayName) = GetInvitedUsersDtoAnonymousDisplayName(results.Where(Function(r) r.Answer.IdInvitedUser.HasValue AndAlso r.Answer.IdInvitedUser.Value > 0 AndAlso (r.Answer.IdPerson.Equals(0) OrElse r.Answer.IdPerson.Equals(idAnonymous))).Select(Function(r) r.Answer.IdInvitedUser.Value).Distinct.ToList(), anonymous)

                If idAnonymous > 0 Then
                    items.Add("p_" & idAnonymous, New dtoDisplayName(anonymous))
                End If
                items.Add("p_0", New dtoDisplayName(anonymous))
                persons.ToList.ForEach(Sub(s) items.Add("p_" & s.Key, s.Value))
                invitedUsers.ToList.ForEach(Sub(s) items.Add("i_" & s.Key, s.Value))

            Catch ex As Exception

            End Try
            Return items
        End Function

        Private Function GetDisplayName(idPersons As List(Of Integer)) As Dictionary(Of Integer, String)
            Dim items As List(Of litePerson) = Manager.GetLitePersons(idPersons)
            Return items.ToDictionary(Function(p) p.Id, Function(p) p.SurnameAndName)
        End Function
        Private Function GetDtoDisplayName(idPersons As List(Of Integer)) As Dictionary(Of Integer, dtoDisplayName)
            Dim persons As List(Of litePerson) = Manager.GetLitePersons(idPersons)

            Return persons.ToDictionary(Of Integer, dtoDisplayName)(Function(p) p.Id, Function(p) New dtoDisplayName(p))
        End Function
        Private Function GetInvitedUsersDisplayName(idInvites As List(Of Integer)) As Dictionary(Of Integer, String)
            Dim result As New Dictionary(Of Integer, String)
            Dim pageSize As Integer = 100
            Dim pageIndex As Integer = 0
            Dim paged As List(Of Integer) = idInvites.Skip(pageIndex).Take(pageSize).ToList

            While paged.Any
                Dim query As List(Of LazyExternalInvitedUser) = (From p In Manager.GetIQ(Of LazyExternalInvitedUser)() Where paged.Contains(p.Id) Select p).ToList()
                query.ForEach(Sub(s) result.Add(s.Id, s.Surname & " " & s.Name))
                pageIndex += 1
                paged = idInvites.Skip(pageIndex * pageSize).Take(pageSize).ToList
            End While
            Return result
        End Function
        Private Function GetInvitedUsersDtoDisplayName(idInvites As List(Of Integer)) As Dictionary(Of Integer, dtoDisplayName)
            Dim result As New Dictionary(Of Integer, dtoDisplayName)
            Dim pageSize As Integer = 100
            Dim pageIndex As Integer = 0
            Dim paged As List(Of Integer) = idInvites.Skip(pageIndex).Take(pageSize).ToList

            While paged.Any
                Dim query As List(Of LazyExternalInvitedUser) = (From p In Manager.GetIQ(Of LazyExternalInvitedUser)() Where paged.Contains(p.Id) Select p).ToList()
                query.ForEach(Sub(s) result.Add(s.Id, New dtoDisplayName(s)))
                pageIndex += 1
                paged = idInvites.Skip(pageIndex * pageSize).Take(pageSize).ToList
            End While
            Return result
        End Function
        Private Function GetInvitedUsersDtoAnonymousDisplayName(idInvites As List(Of Integer), name As String) As Dictionary(Of Integer, dtoDisplayName)
            Dim result As New Dictionary(Of Integer, dtoDisplayName)
            Dim pageSize As Integer = 100
            Dim pageIndex As Integer = 0
            Dim paged As List(Of Integer) = idInvites.Skip(pageIndex).Take(pageSize).ToList

            While paged.Any
                Dim query As List(Of LazyExternalInvitedUser) = (From p In Manager.GetIQ(Of LazyExternalInvitedUser)() Where paged.Contains(p.Id) Select p).ToList()
                query.ForEach(Sub(s) result.Add(s.Id, New dtoDisplayName(name) With {.OtherInfos = s.Description}))
                pageIndex += 1
                paged = idInvites.Skip(pageIndex * pageSize).Take(pageSize).ToList
            End While
            Return result
        End Function

        Private Function GetInternalInvitedUsersExtraInfo(idInvites As List(Of Integer)) As Dictionary(Of Integer, String)
            Dim result As New Dictionary(Of Integer, String)

            Dim pageSize As Integer = 500
            Dim pageIndex As Integer = 0
            Dim paged As List(Of Integer) = idInvites.Skip(pageIndex).Take(pageSize).ToList

            While paged.Any
                Dim query As List(Of LazyInternalInvitedUser) = (From p In Manager.GetIQ(Of LazyInternalInvitedUser)() Where paged.Contains(p.Id) Select p).ToList()
                query.ForEach(Sub(s) result.Add(s.Id, s.Description))
                pageIndex += 1
                paged = idInvites.Skip(pageIndex * pageSize).Take(pageSize).ToList
            End While
            Return result
        End Function
        Private Function GetExternalInvitedUsersExtraInfo(idInvites As List(Of Integer)) As Dictionary(Of Integer, String)
            Dim result As New Dictionary(Of Integer, String)

            Dim pageSize As Integer = 500
            Dim pageIndex As Integer = 0
            Dim paged As List(Of Integer) = idInvites.Skip(pageIndex).Take(pageSize).ToList

            While paged.Any
                Dim query As List(Of LazyExternalInvitedUser) = (From p In Manager.GetIQ(Of LazyExternalInvitedUser)() Where paged.Contains(p.Id) Select p).ToList()
                query.ForEach(Sub(s) result.Add(s.Id, s.Description))
                pageIndex += 1
                paged = idInvites.Skip(pageIndex * pageSize).Take(pageSize).ToList

            End While
            Return result
        End Function
#End Region

#Region "Links"

#End Region

        Public Function GetUserRecipient(idUserModule As Long) As lm.Comol.Core.Mail.dtoRecipient
            Dim recipient As lm.Comol.Core.Mail.dtoRecipient = Nothing
            Try
                Dim idInvited As Integer = 0
                Integer.TryParse(idUserModule.ToString, idInvited)
                Dim user As LazyInvitedUser = Manager.Get(Of LazyInvitedUser)(idInvited)
                If Not IsNothing(user) Then
                    If TypeOf (user) Is LazyInternalInvitedUser Then
                        Dim p As Person = Manager.GetPerson(DirectCast(user, LazyInternalInvitedUser).IdPerson)
                        If Not IsNothing(p) Then
                            recipient = New lm.Comol.Core.Mail.dtoRecipient() With {.DisplayName = p.SurnameAndName, .MailAddress = p.Mail}
                        End If
                    ElseIf TypeOf (user) Is LazyExternalInvitedUser Then
                        recipient = New lm.Comol.Core.Mail.dtoRecipient() With {.DisplayName = DirectCast(user, LazyExternalInvitedUser).Surname & " " & DirectCast(user, LazyExternalInvitedUser).Name, .MailAddress = DirectCast(user, LazyExternalInvitedUser).Mail}
                    End If
                End If
            Catch ex As Exception

            End Try

            Return recipient
        End Function


    End Class

End Namespace