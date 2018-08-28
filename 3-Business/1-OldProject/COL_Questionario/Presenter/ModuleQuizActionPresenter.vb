Imports COL_Questionario.Business
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.ModuleLinks
Imports System.Linq

Public Class ModuleQuizActionPresenter
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
    Protected Overridable ReadOnly Property View() As IViewModuleQuizAction
        Get
            Return DirectCast(MyBase.View, IViewModuleQuizAction)
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
    Public Sub New(oContext As iApplicationContext, view As IViewModuleQuizAction)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView(dto As dtoModuleDisplayActionInitializer)
        View.InsideOtherModule = True
        InitializeControlByLink(dto, IIf(isValidDisplay(dto.Display, DisplayActionMode.defaultAction), StandardActionType.Play, StandardActionType.None))
    End Sub
    Public Sub InitView(dto As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType)
        View.InsideOtherModule = True
        InitializeControlByLink(dto, actionsToDisplay)
    End Sub
    Public Function InitRemoteControlView(dto As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl)
        View.InsideOtherModule = True
        Return InitializeControlByLink(dto, actionsToDisplay)
    End Function
    Private Function InitializeControlByLink(dto As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl)
        Dim actions As New List(Of dtoModuleActionControl)
        If (IsNothing(dto.Link) OrElse UserContext.isAnonymous OrElse IsNothing(dto.Link.DestinationItem)) Then
            View.DisplayEmptyAction()
        ElseIf dto.Link.DestinationItem.ServiceCode = ServiceQuestionnaire.UniqueID Then
            View.ItemIdentifier = "link_" + dto.Link.Id.ToString()
            actions = AnalyzeActions(dto, actionsToDisplay)
        Else
            View.DisplayEmptyAction()
        End If
        Return actions
    End Function
    Private Function AnalyzeActions(dto As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl)
        Dim actions As New List(Of dtoModuleActionControl)
        View.ContainerCSS = dto.ContainerCSS
        View.IconSize = dto.IconSize

        If (IsNothing(dto.Link) OrElse IsNothing(dto.Link.DestinationItem)) Then
            View.DisplayEmptyAction()
        Else
            View.Display = dto.Display
            Dim idQuestionnaire As Integer = 0
            If dto.Link.DestinationItem.ObjectLongID < Int32.MaxValue Then
                idQuestionnaire = Convert.ToInt32(dto.Link.DestinationItem.ObjectLongID)
            End If
            'QuestId = idQuestionnaire
            Dim item As LazyQuestionnaire = CurrentManager.Get(Of LazyQuestionnaire)(idQuestionnaire)

            If (IsNothing(item)) Then
                View.DisplayRemovedObject()
            Else
                If (dto.PlaceHolders.Where(Function(p) Not String.IsNullOrEmpty(p.Text)).Any() AndAlso (isValidDisplay(dto.Display, DisplayActionMode.defaultAction) OrElse isValidDisplay(dto.Display, DisplayActionMode.text))) Then
                    View.DisplayPlaceHolders(dto.PlaceHolders.Where(Function(p) Not String.IsNullOrEmpty(p.Text)).ToList())
                End If
                actions = AnalyzeModuleLinkItem(dto.Link, item, dto.Display, actionsToDisplay)
            End If
        End If

        Return actions
    End Function
    Private Function AnalyzeModuleLinkItem(link As ModuleLink, item As LazyQuestionnaire, display As DisplayActionMode, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl)
        Dim actions As New List(Of dtoModuleActionControl)
        If Not IsNothing(item) Then
            Dim dto As dtoUserQuest = Nothing

            If (isValidDisplay(display, DisplayActionMode.textDefault)) Then
                dto = New dtoUserQuest(item, Service.GetQuestionaireName(item.Id, UserContext.Language.Id))
            Else
                dto = GetQuestionnaireInfo(link.SourceItem.ObjectLongID, link.SourceItem.ObjectTypeID, item)
            End If
            If (isValidDisplay(display, DisplayActionMode.text)) OrElse (isValidDisplay(display, DisplayActionMode.textDefault)) Then
                DisplayTextInfo(link, item, dto)
            ElseIf (isValidDisplay(display, DisplayActionMode.defaultAction)) Then
                DisplayDefaultAction(link, item, dto)
            End If
            actions = GenerateActions(link, item, dto)
            If (isValidDisplay(display, DisplayActionMode.actions) AndAlso actionsToDisplay <> StandardActionType.None) Then
                View.DisplayActions(actions.Where(Function(a) (a.ControlType And actionsToDisplay) > 0).ToList())
            End If
        End If
            Return actions
    End Function

    Private Sub DisplayTextInfo(link As ModuleLink, item As LazyQuestionnaire, dto As dtoUserQuest)
        View.DisplayTextInfo(dto)
    End Sub
    Private Sub DisplayDefaultAction(link As ModuleLink, item As LazyQuestionnaire, dto As dtoUserQuest)
        View.Score = dto.Score
        View.ItemStatus = dto.Status

        Select Case dto.Status
            Case QuizStatus.ToCompile
                View.DisplayItemToCompile(dto, View.GetBaseUrl & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl)
            Case QuizStatus.ViewCompiled
                View.DisplayItemViewCompiled(dto, View.GetBaseUrl & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl & "&View=true")
            Case QuizStatus.Compiled
                View.DisplayItemCompiled(dto)
            Case QuizStatus.CompiledWithAttempts, QuizStatus.RetrieveErrors, QuizStatus.MaxAttempts
                View.MaxScore = item.EvaluationScale
                View.MinScore = item.MinScore
                Select Case dto.Status
                    Case QuizStatus.MaxAttempts
                        If dto.IdLastAttempt > 0 AndAlso item.ViewAnswers Then
                            View.DisplayAttemptItem(dto, View.GetBaseUrl & RootObject.CompileRandomQuiz(link.SourceItem.ObjectLongID, item.Id, dto.IdLastAttempt, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl & "&View=true")
                        Else
                            View.DisplayAttemptItem(dto)
                        End If
                    Case Else
                        View.DisplayAttemptItem(dto)
                End Select



            Case QuizStatus.ViewAttemptCompiled, QuizStatus.ToCompileWithAttempts
                Dim url As String = View.GetBaseUrl & RootObject.CompileRandomQuiz(link.SourceItem.ObjectLongID, item.Id, dto.IdLastAttempt, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl
                View.MaxScore = item.EvaluationScale
                View.MinScore = item.MinScore
                Select Case dto.Status
                    Case QuizStatus.ViewAttemptCompiled
                        url &= "&View=true"
                End Select
                View.DisplayAttemptItem(dto, url)

            Case QuizStatus.NewAttempt
                View.DisplayAttemptItem(dto, View.GetBaseUrl & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl)
        End Select
    End Sub

    Private Function GenerateActions(link As ModuleLink, item As LazyQuestionnaire, dto As dtoUserQuest) As List(Of dtoModuleActionControl)
        Dim actions As New List(Of dtoModuleActionControl)
        Dim base As String = View.GetBaseUrl

        Select Case link.SourceItem.ObjectTypeID

        End Select

        Select Case dto.Status
            Case QuizStatus.ToCompile, QuizStatus.ViewCompiled
                actions.Add(New dtoModuleActionControl(base & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl, StandardActionType.Play, False))
            Case QuizStatus.ViewAttemptCompiled, QuizStatus.ToCompileWithAttempts
                actions.Add(New dtoModuleActionControl(base & RootObject.CompileRandomQuiz(link.SourceItem.ObjectLongID, item.Id, dto.IdLastAttempt, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl, StandardActionType.Play, False))
            Case QuizStatus.NewAttempt
                actions.Add(New dtoModuleActionControl(base & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl, StandardActionType.Play, False))
        End Select

        actions.Add(New dtoModuleActionControl(base & RootObject.UpdateQuiz(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, item.IdType) & "&BackUrl=" & View.DestinationUrl, StandardActionType.Edit, False))

        If item.IdType = QuestionnaireType.RandomMultipleAttempts Then
            If dto.Attempts = 1 AndAlso (item.ViewAnswers OrElse item.ViewCorrections OrElse item.ViewSuggestions) Then
                actions.Add(New dtoModuleActionControl(base & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewPersonalStatistics, False))
            ElseIf dto.Attempts > 1 AndAlso (item.ViewAnswers OrElse item.ViewCorrections OrElse item.ViewSuggestions) Then
                actions.Add(New dtoModuleActionControl(base & RootObject.UserRandomAttemptsStat(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, View.ForUserId, StatisticsType.MyAttempts) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewPersonalStatistics, False))
            End If
            If Not item.isAnonymous Then
                If item.IdType = QuestionnaireType.RandomMultipleAttempts AndAlso dto.Attempts > 1 Then
                    actions.Add(New dtoModuleActionControl(base & RootObject.UserRandomAttemptsStat(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, View.ForUserId, StatisticsType.UserAttempts) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewUserStatistics, False))
                Else
                    actions.Add(New dtoModuleActionControl(base & RootObject.UserStat(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, View.ForUserId) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewUserStatistics, False))
                End If
            End If
        Else
            If item.ViewAnswers OrElse item.ViewCorrections OrElse item.ViewSuggestions Then
                actions.Add(New dtoModuleActionControl(base & RootObject.CompileQuiz(link.SourceItem.ObjectLongID, item.Id, item.IdType, link.SourceItem.ObjectTypeID, link.Id) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewPersonalStatistics, False))
            End If

            If Not item.isAnonymous Then
                actions.Add(New dtoModuleActionControl(base & RootObject.UserStat(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, View.ForUserId) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewUserStatistics, False))
            End If
        End If




        'actions.Add(New dtoModuleActionControl(base & RootObject.UserStat(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, UserContext.CurrentUserID) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewPersonalStatistics, False))



        actions.Add(New dtoModuleActionControl(base & RootObject.UsersStat(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, UserContext.CurrentUserID) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewAdvancedStatistics, False))
        actions.Add(New dtoModuleActionControl(base & RootObject.GlobalStat(link.SourceItem.ObjectLongID, item.Id, link.SourceItem.ObjectTypeID, UserContext.CurrentUserID) & "&BackUrl=" & View.DestinationUrl, StandardActionType.ViewAdministrationCharts, False))

        Return actions
    End Function


    Private Function GetQuestionnaireInfo(ByVal sourceId As Integer, ByVal sourceType As Integer, item As LazyQuestionnaire) As dtoUserQuest
        Dim result As New dtoUserQuest() With {.Status = QuizStatus.Unknown, .Score = 0}

        Dim quest As Questionario = DALQuestionario.readQuestionarioByPersona(Me.AppContext, False, item.Id, UserContext.Language.Id, UserContext.CurrentUser.Id, 0)
        result.Id = quest.id  'Add: MB
        result.Name = quest.nome
        result.DisplayScoreToUser = quest.DisplayScoreToUser
        result.DisplayAttemptScoreToUser = quest.DisplayAttemptScoreToUser
        result.DisplayResultsStatus = quest.DisplayResultsStatus
        result.DisplayAvailableAttempts = quest.DisplayAvailableAttempts
        result.DisplayCurrentAttempts = quest.DisplayCurrentAttempts
        result.Type = quest.tipo

        result.MaxAttempts = quest.MaxAttempts
        result.MaxScore = quest.scalaValutazione
        result.MinScore = quest.MinScore

        If quest.tipo = QuestionnaireType.RandomMultipleAttempts Then
            'result.MaxAttempts = quest.MaxAttempts
            'result.MaxScore = quest.scalaValutazione
            'result.MinScore = quest.MinScore

            ' VERIFICARE SE METTERE IL FORUSERID
            Dim attempts As List(Of LazyUserResponse) = Service.GetQuestionnaireAttempts(item.Id, UserContext.CurrentUser.Id, 0)
            If IsNothing(attempts) Then
                result.Status = QuizStatus.RetrieveErrors
            Else
                result.Attempts = attempts.Count
                If result.MaxAttempts > 0 AndAlso attempts.Count > result.MaxAttempts Then
                    result.Attempts = result.MaxAttempts
                    attempts = attempts.Skip(0).Take(result.MaxAttempts).ToList()
                End If

                Dim l As LazyUserResponse = attempts.OrderByDescending(Function(r) r.Id).FirstOrDefault()
                If IsNothing(l) Then
                    result.Status = QuizStatus.ToCompile
                    ' COMPLETATO
                Else
                    Dim calc As dtoItemEvaluation(Of Long) = Service.CalculateComplation(l)

                    If calc.isCompleted OrElse Not quest.editaRisposta Then
                        If Not quest.editaRisposta AndAlso Not calc.isCompleted Then
                            If l.ModifiedOn.HasValue Then
                                l.CompletedOn = l.ModifiedOn
                            Else
                                l.CompletedOn = l.StartedOn
                            End If
                            CurrentManager.SaveOrUpdate(l)
                            calc.isCompleted = True
                        End If
                        If l.RelativeScore < result.MinScore Then
                            If result.MaxAttempts < 1 Then
                                result.Status = QuizStatus.NewAttempt
                            ElseIf result.Attempts < result.MaxAttempts Then
                                result.Status = QuizStatus.NewAttempt
                            Else
                                result.Status = QuizStatus.MaxAttempts
                            End If
                            ' result.Status = IIf((result.Attempts = result.MaxAttempts), QuizStatus.MaxAttempts, QuizStatus.NewAttempt)
                            result.Score = l.RelativeScore
                            If quest.visualizzaRisposta Then
                                result.IdLastAttempt = l.IdRandomQuestionnaire
                            End If
                        ElseIf quest.visualizzaRisposta Then
                            result.Status = QuizStatus.ViewAttemptCompiled
                            result.Score = l.RelativeScore
                            result.IdLastAttempt = l.IdRandomQuestionnaire
                        Else
                            result.Status = QuizStatus.CompiledWithAttempts
                            result.Score = l.RelativeScore
                            result.IdLastAttempt = l.IdRandomQuestionnaire
                        End If
                    Else
                        result.Status = QuizStatus.ToCompileWithAttempts
                        result.IdLastAttempt = l.IdRandomQuestionnaire
                    End If
                End If
            End If
        Else
            If quest.rispostaQuest Is Nothing OrElse quest.rispostaQuest.id < 1 Then
                result.Status = QuizStatus.ToCompile
            ElseIf quest.visualizzaRisposta Then
                result.Status = QuizStatus.ViewCompiled
                result.Score = quest.rispostaQuest.oStatistica.punteggio
            Else
                result.Score = quest.rispostaQuest.oStatistica.punteggio
                result.Status = QuizStatus.Compiled
            End If
        End If
        'Select Case sourceType
        '    Case COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity

        '    Case COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity

        '    Case Else
        '        result.Status = QuizStatus.Unknown
        'End Select
        View.CurrentInfo = result
        Return result
    End Function


    '    private void DisplayTextInfo(BaseCommunityFile item, Int32 idAction) {
    'If (item.isFile) Then
    '            View.DisplayItem(item.DisplayName, item.Extension,item.Size, item.RepositoryItemType);
    '        else {
    '    Switch(idAction)
    '            {
    '                case (int)CoreModuleRepository.ActionType.CreateFolder:
    '                    View.DisplayCreateFolder(item.Name);
    '                    break;
    '                case (int)CoreModuleRepository.ActionType.UploadFile:
    '                    View.DisplayUploadFile(item.Name);
    '                    break;
    '                case 0:
    '                    View.DisplayFolder(item.Name);
    '                    break;
    '            }
    '        }
    '    }
    '    private void DisplayDefaultAction(ModuleLink link, BaseCommunityFile item)
    '    {
    '        if (typeof(CommunityFile) == item.GetType())
    '        {
    '            View.ServiceCode = link.DestinationItem.ServiceCode;
    '            View.ServiceID = link.DestinationItem.ServiceID;
    '        }
    '    Else
    '        {
    '            View.ServiceCode = link.SourceItem.ServiceCode;
    '            View.ServiceID = link.SourceItem.ServiceID;
    '        }
    '        int idCommunity = 0;
    '        if (item.CommunityOwner != null)
    '            idCommunity = item.CommunityOwner.Id;


    '            Switch(link.Action)
    '        {
    '            case (int)CoreModuleRepository.ActionType.CreateFolder:
    '                View.DisplayCreateFolder(item.Name);
    '                break;
    '            case (int)CoreModuleRepository.ActionType.UploadFile:
    '                View.DisplayUploadFile(item.Name);
    '                break;
    '            case (int)CoreModuleRepository.ActionType.DownloadFile:
    '                View.DisplayItem(item.DisplayName, RootObject.DownloadFileFromModule(item.Id,(long) UserContext.CurrentUserID, UserContext.Language.Code,View.ServiceID,idCommunity,link.Id) , item.Extension, item.Size, item.RepositoryItemType);
    '                break;
    '            case (int)CoreModuleRepository.ActionType.PlayFile:
    '                View.ServiceCode = link.SourceItem.ServiceCode;
    '                View.ServiceID = link.SourceItem.ServiceID;
    '                String url = "";
    '                if (typeof(ModuleInternalFile) == item.GetType()) {
    '                    ModuleInternalFile internalFile = (ModuleInternalFile)item;
    '                    url= (internalFile.RepositoryItemType== RepositoryItemType.ScormPackage) ?
    '                View.GetUrlForPlayScorm(link.Id, internalFile.Id, internalFile.UniqueID, idCommunity, View.ServiceID, internalFile.ServiceActionAjax)

    '                        RootObject.PlayMultimediaFileFromModule(internalFile.Id, internalFile.UniqueID,UserContext.CurrentUserID, UserContext.Language.Code, View.ServiceID,  idCommunity, link.Id, internalFile.ServiceActionAjax, View.PreLoadedContentView);
    '                }
    '                else {
    '                    url = (item.RepositoryItemType == RepositoryItemType.ScormPackage) ?
    '                View.GetUrlForPlayScorm(link.Id, item.Id, item.UniqueID, idCommunity, link.SourceItem.ServiceID)

    '                        RootObject.PlayMultimediaFile(item.Id, item.UniqueID,UserContext.CurrentUserID, UserContext.Language.Code, View.PreLoadedContentView);
    '                }
    '                View.DisplayPlayItem(item.Name,url,item.RepositoryItemType );
    '                break;
    '            case 0:
    '                View.DisplayFolder(item.Name);
    '                break;
    '        }
    '    }
    '    private List<dtoModuleActionControl> GenerateActions(ModuleLink link, BaseCommunityFile item)
    '    {
    '        List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
    '        String serviceCode = link.DestinationItem.ServiceCode;
    '        Int32 IdModule = link.DestinationItem.ServiceID;
    '        Boolean allowDownload = View.AllowDownload(item.RepositoryItemType) && item.IsDownloadable && item.isFile;
    '        allowDownload = true;
    '        if (typeof(CommunityFile) != item.GetType())
    '        {
    '            serviceCode = link.SourceItem.ServiceCode;
    '            IdModule = link.SourceItem.ServiceID;
    '        }

    '        int idCommunity = 0;
    '        if (item.CommunityOwner != null)
    '            idCommunity = item.CommunityOwner.Id;

    '        switch (item.RepositoryItemType){
    '            case RepositoryItemType.FileStandard:
    '                actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id,(long) UserContext.CurrentUserID, UserContext.Language.Code,View.ServiceID,idCommunity,link.Id), StandardActionType.Play,true));
    '                actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, View.ServiceID, idCommunity, link.Id), StandardActionType.DownloadItem, true));
    '                break;
    '            case RepositoryItemType.ScormPackage:
    '                if (typeof(ModuleInternalFile) == item.GetType()){
    '                    ModuleInternalFile internalFile = (ModuleInternalFile)item;
    '                    actions.Add(new dtoModuleActionControl(View.GetUrlForPlayScorm(link.Id, internalFile.Id, internalFile.UniqueID, idCommunity, link.SourceItem.ServiceID, internalFile.ServiceActionAjax), StandardActionType.Play,true));
    '                }
    '                        Else
    '                    actions.Add(new dtoModuleActionControl(View.GetUrlForPlayScorm(link.Id, item.Id, item.UniqueID, idCommunity, IdModule), StandardActionType.Play, true));
    '                actions.Add(new dtoModuleActionControl(RootObject.EditScormPackageSettings(item.Id, link.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.EditMetadata, false));
    '                actions.Add(new dtoModuleActionControl(RootObject.ManagementScormStatistics(item.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewAdvancedStatistics, false));
    '                actions.Add(new dtoModuleActionControl(RootObject.ManagementScormStatistics(item.Id, View.ForUserId, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewUserStatistics, false));

    '                actions.Add(new dtoModuleActionControl(RootObject.UserScormStatistics(item.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.ViewPersonalStatistics, false));
    '                            If (allowDownload) Then
    '                    actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, View.ServiceID, idCommunity, link.Id), StandardActionType.DownloadItem, true));
    '                break;
    '            case RepositoryItemType.Multimedia:
    '            case RepositoryItemType.VideoStreaming:
    '                 if (typeof(ModuleInternalFile) == item.GetType()){
    '                    ModuleInternalFile internalFile = (ModuleInternalFile)item;
    '                    actions.Add(new dtoModuleActionControl(RootObject.PlayMultimediaFileFromModule(internalFile.Id, internalFile.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code, link.SourceItem.ServiceID, idCommunity, link.Id, internalFile.ServiceActionAjax, View.PreLoadedContentView), StandardActionType.Play,true));
    '                }
    '                                Else
    '                    actions.Add(new dtoModuleActionControl(RootObject.PlayMultimediaFile(item.Id, item.UniqueID, UserContext.CurrentUserID, UserContext.Language.Code, View.PreLoadedContentView), StandardActionType.Play,true));

    '                 actions.Add(new dtoModuleActionControl(RootObject.EditMultimediaFileSettings(item.Id, link.Id, View.DestinationUrl, View.PreLoadedContentView), StandardActionType.EditMetadata, false));

    '                                    If (allowDownload) Then
    '                     actions.Add(new dtoModuleActionControl(RootObject.DownloadFileFromModule(item.Id, (long)UserContext.CurrentUserID, UserContext.Language.Code, View.ServiceID, idCommunity, link.Id), StandardActionType.DownloadItem, true));
    '                break;
    '            case RepositoryItemType.Folder:
    '                break;
    '        }
    '        return actions;
    '    }

    Public Function GetItemDescription(ByVal link As ModuleLink, ByVal isGeneric As Boolean) As String
        If (IsNothing(link) OrElse IsNothing(link.DestinationItem)) OrElse UserContext.isAnonymous Then
            Return ""
        Else
            Dim idQuestionnaire As Integer = 0
            If link.DestinationItem.ObjectLongID < Int32.MaxValue Then
                idQuestionnaire = Convert.ToInt32(link.DestinationItem.ObjectLongID)
            End If
            Dim item As LazyQuestionnaire = CurrentManager.Get(Of LazyQuestionnaire)(idQuestionnaire)

            If (IsNothing(item)) Then
                Return "--"
            Else
                Dim dto As dtoUserQuest = Nothing
                If isGeneric Then
                    dto = New dtoUserQuest(item, Service.GetQuestionaireName(item.Id, UserContext.Language.Id))
                Else
                    dto = GetQuestionnaireInfo(link.SourceItem.ObjectLongID, link.SourceItem.ObjectTypeID, item)
                End If

                If Not IsNothing(dto) Then
                    Return View.GetQuestionnaireDescription(dto)
                Else
                    Return "--"
                End If

            End If
        End If
    End Function
    Function isValidDisplay(current As DisplayActionMode, required As DisplayActionMode) As Boolean
        Return (current And required) > 0
    End Function
End Class
