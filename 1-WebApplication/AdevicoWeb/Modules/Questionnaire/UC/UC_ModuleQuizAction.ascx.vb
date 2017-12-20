Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.ModuleLinks
Imports COL_Questionario

Public Class UC_ModuleQuizAction
    Inherits BaseControl
    Implements IViewModuleQuizAction

#Region "Context"
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As ModuleQuizActionPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleQuizActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleQuizActionPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property ContainerCSS As String Implements IViewModuleQuizAction.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IViewModuleQuizAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
            If value = DisplayActionMode.none Then
                MLVcontrol.SetActiveView(VIWempty)
                LBempty.Text = " "
            Else
                MLVcontrol.SetActiveView(VIWdata)
                RPTactions.Visible = ((value And DisplayActionMode.actions) > 0)
                LBname.Visible = ((value And DisplayActionMode.text) > 0) OrElse ((value And DisplayActionMode.defaultAction) > 0) OrElse ((value And DisplayActionMode.textDefault) > 0)
            End If
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IGenericModuleDisplayAction.IconSize
        Get
            Return ViewStateOrDefault("IconSize", lm.Comol.Core.DomainModel.Helpers.IconSize.Medium)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    Private ReadOnly Property IconSizeToString As String
        Get
            Dim result As String = ViewStateOrDefault("IconSizeToString", "")
            If String.IsNullOrEmpty(result) Then
                Select Case IconSize
                    Case Helpers.IconSize.Large
                        result = "_l"
                    Case Helpers.IconSize.Medium
                        result = "_m"
                    Case Helpers.IconSize.Small
                        result = "_s"
                    Case Helpers.IconSize.Smaller
                        result = "_xs"
                End Select
                ViewState("IconSizeToString") = result
            End If
            Return result
        End Get
    End Property
    Public Property ShortDescription As Boolean Implements IGenericModuleDisplayAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
    End Property
    Public Property EnableAnchor As Boolean Implements IGenericModuleDisplayAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
#End Region
    Public Property CurrentInfo As COL_Questionario.dtoUserQuest Implements COL_Questionario.IViewModuleQuizAction.CurrentInfo
        Get
            Return ViewStateOrDefault("CurrentInfo", New COL_Questionario.dtoUserQuest())
        End Get
        Set(value As COL_Questionario.dtoUserQuest)
            ViewState("CurrentInfo") = value
        End Set
    End Property
    Private Property ItemStatus As QuizStatus Implements IViewModuleQuizAction.ItemStatus
        Get
            Return ViewStateOrDefault("ItemStatus", QuizStatus.None)
        End Get
        Set(value As QuizStatus)
            ViewState("ItemStatus") = value
        End Set
    End Property
    Private Property QuizName As String Implements IViewModuleQuizAction.QuizName
        Get
            Return ViewStateOrDefault("QuizName", "")
        End Get
        Set(value As String)
            ViewState("QuizName") = value
        End Set
    End Property
    Private Property Score As Integer Implements IViewModuleQuizAction.Score
        Get
            Return ViewStateOrDefault("Score", 0)
        End Get
        Set(value As Integer)
            ViewState("Score") = value
        End Set
    End Property
    Private Property MaxScore As Integer Implements IViewModuleQuizAction.MaxScore
        Get
            Return ViewStateOrDefault("MaxScore", 0)
        End Get
        Set(value As Integer)
            ViewState("MaxScore") = value
        End Set
    End Property
    Private Property MinScore As Integer Implements IViewModuleQuizAction.MinScore
        Get
            Return ViewStateOrDefault("MinScore", 0)
        End Get
        Set(value As Integer)
            ViewState("MinScore") = value
        End Set
    End Property
    Public Property ForUserId As Integer Implements IViewModuleQuizAction.ForUserId
        Get
            Return ViewStateOrDefault("ForUserId", CurrentContext.UserContext.CurrentUserID)
        End Get
        Set(value As Integer)
            ViewState("ForUserId") = value
        End Set
    End Property
    Public Property InsideOtherModule As Boolean Implements IViewModuleQuizAction.InsideOtherModule
        Get
            Return ViewStateOrDefault("InsideOtherModule", False)
        End Get
        Set(value As Boolean)
            ViewState("InsideOtherModule") = value
        End Set
    End Property
    Private Property ItemIdentifier As String Implements IViewModuleQuizAction.ItemIdentifier
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTidentifier.Visible = False
            Else
                LTidentifier.Text = "<a name=""" & value & """> </a>"
                LTidentifier.Visible = True
            End If
        End Set
    End Property
    Private ReadOnly Property PreLoadedContentView As ContentView Implements IViewModuleQuizAction.PreLoadedContentView
        Get
            Return PageUtility.PreLoadedContentView
        End Get
    End Property
    Private ReadOnly Property DestinationUrl As String Implements IViewModuleQuizAction.DestinationUrl
        Get
            Dim url As String = Request.Url.LocalPath
            If BaseUrl <> "/" Then
                url = Replace(Request.Url.LocalPath, BaseUrl, "")
            End If
            url = Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
            Return url
        End Get
    End Property

    Public ReadOnly Property GetBaseUrl(useSSL As Boolean) As String Implements IViewModuleQuizAction.GetBaseUrl
        Get
            Return PageUtility.BaseUrl(useSSL)
        End Get
    End Property
    Public ReadOnly Property GetBaseUrl As String Implements COL_Questionario.IViewModuleQuizAction.GetBaseUrl
        Get
            Return PageUtility.BaseUrl()
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToQuiz", "Questionari")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"

#Region "InitializeControl"
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Function InitializeRemoteControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        ForUserId = idUser
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
#End Region

    Public Sub DisplayRemovedObject() Implements IGenericModuleDisplayAction.DisplayRemovedObject
        LBempty.Text = Resource.getValue("action.RemovedObject")
        MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyAction() Implements IViewModuleQuizAction.DisplayEmptyAction
        LBempty.Text = "&nbsp;"
        MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayUnknownAction() Implements COL_Questionario.IViewModuleQuizAction.DisplayUnknownAction
        LBempty.Text = Resource.getValue("action.unhandled")
        MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayItemViewCompiled(quest As dtoUserQuest, url As String) Implements IViewModuleQuizAction.DisplayItemViewCompiled
        'Score = quest.Score

        'InternalDisplayItem(name, score, displayScore, url)
        InternalDisplayItem(quest, url)
    End Sub
    Private Sub DisplayItemToCompile(quest As dtoUserQuest, url As String) Implements IViewModuleQuizAction.DisplayItemToCompile
        'Score = 0
        'LBscore.Visible = False
        'InternalDisplayItem(name, 0, False, url)
        InternalDisplayItem(quest, url)
    End Sub
    Private Sub DisplayTextInfo(quest As dtoUserQuest) Implements IViewModuleQuizAction.DisplayTextInfo
        InternalDisplayItem(quest, "", True)
    End Sub

    Private Sub DisplayItemCompiled(quest As dtoUserQuest) Implements IViewModuleQuizAction.DisplayItemCompiled
        'MLVcontrol.SetActiveView(VIWdata)
        'LBscore.Visible = (result.Status = QuizStatus.Compiled OrElse result.Status = QuizStatus.ViewCompiled)
        'Score = result.Score
        'If Not String.IsNullOrEmpty(LBscore.Text) Then
        '    LBscore.Text = String.Format(LBscore.Text, Score)
        'End If
        'ItemStatus = result.Status
        '  LBname.Text = result.Name
        InternalDisplayItem(quest, "")
    End Sub
    Private Sub InternalDisplayItem(quest As dtoUserQuest, url As String, Optional onlyText As Boolean = False)
        MLVcontrol.SetActiveView(VIWdata)
        Score = quest.Score
        ItemStatus = quest.Status

        Dim baseKey As String = GetQuestionnaireTranslationKey(quest)
        Dim tooltip As String = Resource.getValue(baseKey)
        Dim warningCode As String = "<span class=""invisibleitem"">Warning code: " & baseKey & "</span>"
        If Not String.IsNullOrEmpty(tooltip) Then
            tooltip = tooltip.Replace("#score#", quest.Score)
            tooltip = tooltip.Replace("#maxscore#", quest.MaxScore)
            tooltip = tooltip.Replace("#attemptsnumber#", quest.Attempts)
            tooltip = tooltip.Replace("#availableattempts#", quest.MaxAttempts)
            warningCode = ""
        Else
            tooltip = "#name#"
        End If
        

        Dim name As String = tooltip
        tooltip = tooltip.Replace("#name#", quest.Name)
        If String.IsNullOrEmpty(url) OrElse (ShortDescription AndAlso ItemStatus <> QuizStatus.Compiled AndAlso ItemStatus <> QuizStatus.ViewCompiled) OrElse onlyText Then
            name = tooltip & warningCode
        Else
            name = name.Replace("#name#", String.Format(LTtemplateUrl.Text, url, tooltip, quest.Name)) & warningCode
        End If
        LBname.Text = name
        'If ShortDescription Then
        '    If ItemStatus = QuizStatus.Compiled OrElse ItemStatus.ViewCompiled Then
        '        LBname.Text = String.Format("<a href=""{0}"" title=""{1}"">{2}</a>", url, toolTip, quest.Name)
        '    Else
        '        LBname.Text = quest.Name
        '    End If
        'Else
        '    LBname.Text = Resource.getValue("QuizStatus." & ItemStatus.ToString())
        '    If ItemStatus = QuizStatus.ViewCompiled OrElse ItemStatus = QuizStatus.ToCompile Then
        '        LBname.Text = String.Format(Me.LBname.Text, url, toolTip, quest.Name)
        '    ElseIf ItemStatus = QuizStatus.Compiled OrElse ItemStatus = QuizStatus.Deleted Then
        '        LBname.Text = String.Format(Me.LBname.Text, quest.Name)
        '    End If
        'End If
    End Sub

    Private Function GetQuestionnaireTranslationKey(quest As dtoUserQuest, Optional prefix As String = "ToolTip") As String
        Dim key As String = prefix
        If Not String.IsNullOrEmpty(key) AndAlso Not key.EndsWith(".") Then
            key &= "."
        End If
        key &= "StandardActionType.Play.QuizStatus." & quest.Status.ToString()
        Select Case quest.Status
            Case QuizStatus.Deleted, QuizStatus.NotCreated, QuizStatus.RetrieveErrors, QuizStatus.Unknown
                If Not String.IsNullOrEmpty(key & "." & quest.Type.ToString) Then
                    key &= "." & quest.Type.ToString
                End If
            Case QuizStatus.ToCompile
                If Not String.IsNullOrEmpty(key & "." & quest.Type.ToString) Then
                    key &= "." & quest.Type.ToString
                End If
                Select Case quest.Type
                    Case QuestionnaireType.RandomMultipleAttempts
                        key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts
                        If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 0 Then
                            key &= GetStringNumber(quest.MaxAttempts)
                        End If
                End Select
            Case QuizStatus.Compiled, QuizStatus.ViewCompiled
                If Not String.IsNullOrEmpty(key & "." & quest.Type.ToString) Then
                    key &= "." & quest.Type.ToString
                End If
                Select Case quest.Type
                    Case QuestionnaireType.RandomMultipleAttempts, QuestionnaireType.AutoEvaluation, QuestionnaireType.Random, QuestionnaireType.Standard
                        key &= ".DisplayScoreToUser." & quest.DisplayScoreToUser & ".DisplayResultsStatus." & quest.DisplayResultsStatus
                End Select

            Case QuizStatus.CompiledWithAttempts
                key &= ".DisplayScoreToUser." & quest.DisplayScoreToUser & ".DisplayResultsStatus." & quest.DisplayResultsStatus & ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts
                If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                    key &= GetStringNumber(quest.Attempts)
                End If

            Case QuizStatus.ToCompileWithAttempts
                key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()

                If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 0 Then
                    key &= GetStringNumber(quest.MaxAttempts)
                End If
                key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                    key &= GetStringNumber(quest.Attempts)
                End If

            Case QuizStatus.MaxAttempts
                key &= ".DisplayResultsStatus." & quest.DisplayResultsStatus.ToString() & ".DisplayScoreToUser." & quest.DisplayScoreToUser.ToString()

                If Not quest.DisplayResultsStatus Then
                    If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 0 Then
                        key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()
                        key &= GetStringNumber(quest.MaxAttempts)
                    End If
                Else
                    If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 0 Then
                        key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()
                        key &= GetStringNumber(quest.MaxAttempts)

                        key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                        If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                            key &= GetStringNumber(quest.Attempts)
                        End If
                    End If
                End If


            Case QuizStatus.ViewAttemptCompiled
                key &= ".DisplayResultsStatus." & quest.DisplayResultsStatus.ToString() & ".DisplayScoreToUser." & quest.DisplayScoreToUser.ToString()
                key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()

                'If (quest.DisplayResultsStatus AndAlso quest.DisplayResultsStatus) OrElse quest.DisplayResultsStatus Then
                If quest.DisplayAvailableAttempts Then
                    If Not quest.DisplayCurrentAttempts Then
                        key &= GetStringNumber(quest.MaxAttempts)
                    ElseIf quest.MaxAttempts > 1 Then
                        key = ".n"
                    End If
                    key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                    If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                        key &= GetStringNumber(quest.Attempts)
                    End If
                ElseIf quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                    key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                    key &= GetStringNumber(quest.Attempts)
                Else
                    key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                End If
                'Else

                'End If


            Case QuizStatus.NewAttempt
                key &= ".DisplayResultsStatus." & quest.DisplayResultsStatus.ToString()
                If quest.DisplayResultsStatus Then
                    key &= ".DisplayAttemptScoreToUser." & quest.DisplayAttemptScoreToUser.ToString()
                    key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()
                    If quest.DisplayAvailableAttempts Then
                        If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 1 Then
                            key &= GetStringNumber(quest.MaxAttempts)
                        End If
                        key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                        If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                            key &= GetStringNumber(quest.Attempts)
                        End If
                    Else
                        key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                        If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                            key &= GetStringNumber(quest.Attempts)
                        End If
                    End If
                Else
                    key &= ".DisplayAttemptScoreToUser." & quest.DisplayResultsStatus.ToString()
                    key &= ".DisplayAvailableAttempts." & quest.DisplayAvailableAttempts.ToString()

                    If quest.DisplayAvailableAttempts Then
                        If quest.DisplayAvailableAttempts AndAlso quest.MaxAttempts > 1 Then
                            key &= GetStringNumber(quest.MaxAttempts)
                        End If
                        key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                        If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                            key &= GetStringNumber(quest.Attempts)
                        End If
                    Else
                        key &= ".DisplayCurrentAttempts." & quest.DisplayCurrentAttempts.ToString()
                        If quest.DisplayCurrentAttempts AndAlso quest.Attempts > 0 Then
                            key &= GetStringNumber(quest.Attempts)
                        End If
                    End If
                End If
        End Select
        Return key
    End Function
    Private Function GetStringNumber(number As Integer, Optional ByVal zeroString As String = "") As String
        Dim result As String = ""
        Select Case number
            Case 0
                If Not String.IsNullOrEmpty(zeroString) AndAlso Not zeroString.StartsWith(".") Then
                    result &= "."
                End If
                result &= zeroString
            Case 1
                result &= ".1"
            Case Else
                If number > 1 Then
                    result &= ".n"
                End If
        End Select
        Return result
    End Function
    Private Sub DisplayAttemptItem(quest As COL_Questionario.dtoUserQuest) Implements COL_Questionario.IViewModuleQuizAction.DisplayAttemptItem
        DisplayAttemptItem(quest, "")
    End Sub
    Public Sub DisplayAttemptItem(quest As COL_Questionario.dtoUserQuest, url As String) Implements COL_Questionario.IViewModuleQuizAction.DisplayAttemptItem
        MLVcontrol.SetActiveView(VIWdata)
        LBscore.Visible = False
        ItemStatus = quest.Status
        LBname.Text = quest.Name

        InternalDisplayItem(quest, url)
    End Sub

#Region "Display Actions"
    Private Sub DisplayActions(actions As List(Of dtoModuleActionControl)) Implements IViewModuleQuizAction.DisplayActions
        If actions.Count = 0 Then
            RPTactions.Visible = False
        End If
        RPTactions.DataSource = actions
        RPTactions.DataBind()
        MLVcontrol.SetActiveView(VIWdata)
    End Sub
    Private Sub DisplayEmptyActions() Implements IViewModuleQuizAction.DisplayEmptyActions
        RPTactions.Visible = False
    End Sub
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As dtoModuleActionControl = DirectCast(e.Item.DataItem, dtoModuleActionControl)
            Dim link As HyperLink = e.Item.FindControl("HYPaction")
            link.Target = IIf(action.isPopupUrl, "_blank", "")
            link.CssClass = "action "
            link.ToolTip = Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString)
            Select Case action.ControlType
                Case StandardActionType.Play
                    link.CssClass &= "questionario" & IconSizeToString
                    Dim sAttempts As String = ""
                    Dim info As COL_Questionario.dtoUserQuest = CurrentInfo
                    If info.Status = QuizStatus.NewAttempt OrElse info.Status = QuizStatus.ToCompileWithAttempts OrElse info.Status = QuizStatus.ViewAttemptCompiled Then
                        sAttempts = IIf(info.MaxAttempts = 0, ".0", "")
                    End If
                    link.ToolTip = Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString & ".QuizStatus." & info.Status.ToString & sAttempts)

                    If String.IsNullOrEmpty(link.ToolTip) Then
                        link.ToolTip = String.Format(Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString), QuizName)
                    Else
                        Select Case info.Status
                            Case QuizStatus.ViewCompiled, QuizStatus.Compiled
                                link.ToolTip = String.Format(link.ToolTip, QuizName, Score)
                            Case QuizStatus.NewAttempt, QuizStatus.ToCompileWithAttempts, QuizStatus.ViewAttemptCompiled
                                If info.MaxAttempts = 0 Then
                                    link.ToolTip = String.Format(link.ToolTip, QuizName, info.Attempts)
                                Else
                                    link.ToolTip = String.Format(link.ToolTip, QuizName, info.Attempts, info.MaxAttempts)
                                End If
                            Case QuizStatus.ViewAttemptCompiled
                                If info.MaxAttempts = 0 Then
                                    link.ToolTip = String.Format(link.ToolTip, QuizName, info.Score, info.MaxScore, info.Attempts)
                                Else
                                    link.ToolTip = String.Format(link.ToolTip, QuizName, info.Score, info.MaxScore, info.Attempts, info.MaxAttempts)
                                End If
                            Case Else
                                link.ToolTip = String.Format(link.ToolTip, QuizName)
                        End Select
                    End If
                Case StandardActionType.ViewAdvancedStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.ViewPersonalStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.ViewUserStatistics
                    link.CssClass &= "stats" & IconSizeToString
                Case StandardActionType.Create
                    link.CssClass &= "add" & IconSizeToString
                Case StandardActionType.Edit
                    link.CssClass &= "edit" & IconSizeToString
                Case StandardActionType.ViewAdministrationCharts
                    link.CssClass &= "chart" & IconSizeToString
            End Select
            link.Text = " " ' action.ControlType.ToString()
            link.NavigateUrl = action.LinkUrl
        End If
    End Sub
#End Region

    Public Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewModuleQuizAction.DisplayPlaceHolders
        Dim places As New Dictionary(Of PlaceHolderType, Integer)
        places.Add(PlaceHolderType.zero, 0)
        places.Add(PlaceHolderType.one, 1)
        places.Add(PlaceHolderType.two, 2)
        places.Add(PlaceHolderType.three, 3)
        places.Add(PlaceHolderType.four, 4)

        For Each item As lm.Comol.Core.ModuleLinks.dtoPlaceHolder In items.Where(Function(i) i.Type <> PlaceHolderType.fullContainer AndAlso i.Type <> PlaceHolderType.none).ToList()
            Dim oLabel As Label = FindControl("LBplace" & places(item.Type))
            If Not IsNothing(oLabel) Then
                oLabel.Text = item.Text
                oLabel.Visible = True
                If Not String.IsNullOrEmpty(item.CssClass) Then
                    oLabel.CssClass = "plh plh" & places(item.Type).ToString() & " " & item.CssClass
                End If
            End If
        Next
    End Sub

    Public Function getDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, ByVal isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.getDescriptionByLink
        Dim result As String = CurrentPresenter.GetItemDescription(link, isGeneric)
        If String.IsNullOrEmpty(result) Then
            result = "&nbsp;"
        End If
        Return result
    End Function

    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, ByVal isGeneric As Boolean) As String Implements IGenericModuleDisplayAction.GetInLineDescriptionByLink
        Dim result As String = CurrentPresenter.GetItemDescription(link, isGeneric)
        If String.IsNullOrEmpty(result) Then
            result = "&nbsp;"
        End If
        Return result
    End Function

    Public Function GetQuestionnaireDescription(quest As COL_Questionario.dtoUserQuest) As String Implements COL_Questionario.IViewModuleQuizAction.GetQuestionnaireDescription
        Dim result As String = quest.Name
        'Dim score As String = ""
        'If (quest.Status = QuizStatus.Compiled OrElse quest.Status = QuizStatus.ViewCompiled) Then
        '    score = String.Format(Resource.getValue(LBscore.Text), quest.Score)
        'End If

        Return result
    End Function
#End Region

End Class