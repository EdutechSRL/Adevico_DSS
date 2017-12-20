Imports lm.Comol.Modules.Base.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports COL_Questionario

Public Class UC_ModuleToQuiz
    Inherits BaseControl
    Implements IViewModuleToQuestionnaire

#Region "Context"
    Private _Presenter As ModuleToQuestionnairePresenter
    Private ReadOnly Property CurrentPresenter() As ModuleToQuestionnairePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleToQuestionnairePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property CurrentAction As COL_Questionario.QuestionnaireAction Implements COL_Questionario.IViewModuleToQuestionnaire.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", COL_Questionario.QuestionnaireAction.SelectAction)
        End Get
        Set(value As COL_Questionario.QuestionnaireAction)
            ViewState("CurrentAction") = value
        End Set
    End Property
    Private Property SourceModuleCode As String Implements COL_Questionario.IViewModuleToQuestionnaire.SourceModuleCode
        Get
            Return ViewStateOrDefault("SourceModuleCode", "")
        End Get
        Set(value As String)
            ViewState("SourceModuleCode") = value
        End Set
    End Property
    Private Property SourceModuleIdAction As Integer Implements COL_Questionario.IViewModuleToQuestionnaire.SourceModuleIdAction
        Get
            Return ViewStateOrDefault("SourceModuleIdAction", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("SourceModuleIdAction") = value
        End Set
    End Property
    Private Property SourceIdOwnerType As Long Implements COL_Questionario.IViewModuleToQuestionnaire.SourceIdOwnerType
        Get
            Return ViewStateOrDefault("SourceIdOwnerType", CLng(0))
        End Get
        Set(value As Long)
            ViewState("SourceIdOwnerType") = value
        End Set
    End Property
    Private Property SourceIdOwner As Long Implements COL_Questionario.IViewModuleToQuestionnaire.SourceIdOwner
        Get
            Return ViewStateOrDefault("SourceIdOwner", CLng(0))
        End Get
        Set(value As Long)
            ViewState("SourceIdOwner") = value
        End Set
    End Property
    Private Property IdCommunityQuestionnaire As Integer Implements COL_Questionario.IViewModuleToQuestionnaire.IdCommunityQuestionnaire
        Get
            Return ViewStateOrDefault("IdCommunityQuestionnaire", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCommunityQuestionnaire") = value
        End Set
    End Property
    Public Property AjaxViewUpdate As Boolean Implements COL_Questionario.IViewModuleToQuestionnaire.AjaxViewUpdate
        Get
            Return ViewStateOrDefault("AjaxViewUpdate", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AjaxViewUpdate") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "UC property / events"
    Public Event CloseClientWindow()
    Public Event ContainerCommands(ByVal show As Boolean)
#End Region
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

#Region "inherited"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToQuiz", "Questionari")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNcloseAddActionWindowTop, True)
            .setButton(BTNcloseAddActionWindowBottom, True)
            .setButton(BTNselectActionTop, True)
            .setButton(BTNselectActionBottom, True)
            .setButton(BTNLinkToModuleTop, True)
            .setButton(BTNLinkToModuleBottom, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(ByVal idCommunity As Integer, ByVal sourceModuleCode As String, ByVal sourceModuleIdAction As Integer, sourceIdOwner As Long, sourceOwnerIdType As Long) Implements COL_Questionario.IViewModuleToQuestionnaire.InitializeControl
        Me.SetInternazionalizzazione()
        Me.CurrentPresenter.InitView(idCommunity, sourceModuleCode, sourceModuleIdAction, sourceIdOwner, sourceOwnerIdType)
    End Sub
    Private Sub LoadAvailableActions(actions As List(Of COL_Questionario.QuestionnaireAction), dAction As COL_Questionario.QuestionnaireAction) Implements COL_Questionario.IViewModuleToQuestionnaire.LoadAvailableActions
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.DisplayAction(dAction)
    End Sub
    Private Sub ChangeDisplayAction(action As COL_Questionario.QuestionnaireAction) Implements COL_Questionario.IViewModuleToQuestionnaire.ChangeDisplayAction
        Me.DisplayAction(action)
    End Sub
    Private Sub DisplayAction(action As COL_Questionario.QuestionnaireAction) Implements COL_Questionario.IViewModuleToQuestionnaire.DisplayAction
        Me.BTNLinkToModuleBottom.Visible = False
        Me.BTNLinkToModuleTop.Visible = False
        Select Case action
            Case QuestionnaireAction.ImportFromCommunity
                MLVcontrol.SetActiveView(VIWaddCommunityQuizAction)
                Me.BTNLinkToModuleTop.Visible = True
                Me.BTNLinkToModuleBottom.Visible = True
                Me.BTNselectActionTop.Visible = True
                Me.BTNselectActionBottom.Visible = True
            Case QuestionnaireAction.None
                MLVcontrol.SetActiveView(VIWempty)
            Case Else
                MLVcontrol.SetActiveView(VIWactionSelector)
                Me.BTNselectActionTop.Visible = False
                Me.BTNselectActionBottom.Visible = False
        End Select
        CurrentAction = action
        LTcurrentAction.Text = GetActionTitle(action)
        If Me.AjaxViewUpdate Then
            Me.DVcommandsBottom.Visible = (action <> COL_Questionario.QuestionnaireAction.SelectAction)
            Me.DVcommandsTop.Visible = (action <> COL_Questionario.QuestionnaireAction.SelectAction)
            RaiseEvent ContainerCommands(action = COL_Questionario.QuestionnaireAction.SelectAction)
        End If

    End Sub

    Private Sub DisplaySessionTimeout(idCommunity As Integer, idModule As Integer) Implements COL_Questionario.IViewModuleToQuestionnaire.DisplaySessionTimeout
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub

    Private Function GetActionTitle(action As COL_Questionario.QuestionnaireAction) As String Implements COL_Questionario.IViewModuleToQuestionnaire.GetActionTitle
        Return Resource.getValue("QuestionnaireAction.ActionTitle." & action.ToString)
    End Function
    Private Sub InitializeQuestionnaireSelector(idCommunity As Integer, sourceModuleCode As String, sourceModuleIdAction As Integer) Implements COL_Questionario.IViewModuleToQuestionnaire.InitializeQuestionnaireSelector
        Me.CTRLcloneCommQuiz.InitializeControl(idCommunity, sourceModuleCode, sourceModuleIdAction, SourceIdOwner, SourceIdOwnerType)
    End Sub
    Private Sub CTRLcloneCommQuiz_QuestionnaireImported() Handles CTRLcloneCommQuiz.QuestionnaireImported
        Me.ChangeDisplayAction(QuestionnaireAction.SelectAction)
    End Sub
#End Region

#Region ""
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As QuestionnaireAction = DirectCast(e.Item.DataItem, QuestionnaireAction)

            Dim oButton As Button = e.Item.FindControl("BTNaddAction")
            oButton.Text = Resource.getValue("QuestionnaireAction.ButtonText." & action.ToString())
            oButton.ToolTip = Resource.getValue("QuestionnaireAction.ToolTip." & action.ToString())
            oButton.CommandName = action.ToString
            Dim oLiteral As Literal = e.Item.FindControl("LTaddAction")
            oLiteral.Text = Resource.getValue("QuestionnaireAction.Description." & action.ToString())
        End If
    End Sub
    Private Sub RPTactions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTactions.ItemCommand
        Dim action As QuestionnaireAction
        action = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of QuestionnaireAction).GetByString(e.CommandName, QuestionnaireAction.SelectAction)
        Select Case action
            Case QuestionnaireAction.AddRandomRepeat
                PageUtility.RedirectToUrl(RootObject.CreateQuiz(SourceIdOwner, SourceIdOwnerType, QuestionnaireType.RandomMultipleAttempts))
            Case QuestionnaireAction.AddRandom
                PageUtility.RedirectToUrl(RootObject.CreateQuiz(SourceIdOwner, SourceIdOwnerType, QuestionnaireType.Random))
            Case QuestionnaireAction.AddStandard
                PageUtility.RedirectToUrl(RootObject.CreateQuiz(SourceIdOwner, SourceIdOwnerType, QuestionnaireType.Standard))
            Case QuestionnaireAction.ImportFromCommunity
                Me.CurrentPresenter.ChangeAction(action)
            Case Else
                Me.CurrentPresenter.ChangeAction(action)
        End Select
    End Sub

    'Private Sub BTNaddCommunityQuiz_toEP_Click(sender As Object, e As System.EventArgs) Handles BTNimportQuiz.Click
    '    Me.MLVcontrol.SetActiveView(Me.VIWaddCommunityQuizAction)
    '    Me.CTRLcloneCommQuiz.InitializeControl(IdCommunityQuestionnaire, SourceModuleCode, SourceModuleIdAction)
    'End Sub

    Private Sub BTNselectActionBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNselectActionBottom.Click, BTNselectActionTop.Click
        Me.CurrentPresenter.ChangeAction(QuestionnaireAction.SelectAction)
    End Sub
    Private Sub BTNcloseAddActionWindowBottom_Click(sender As Object, e As System.EventArgs) Handles BTNcloseAddActionWindowBottom.Click, BTNcloseAddActionWindowTop.Click
        RaiseEvent CloseClientWindow()
    End Sub
#End Region

End Class