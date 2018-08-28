Imports COL_Questionario.Business
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.ModuleLinks
Imports System.Linq

Public Class ModuleToQuestionnairePresenter
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
    Protected Overridable ReadOnly Property View() As IViewModuleToQuestionnaire
        Get
            Return DirectCast(MyBase.View, IViewModuleToQuestionnaire)
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
    Public Sub New(oContext As iApplicationContext, view As IViewModuleToQuestionnaire)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView(idCommunity As Integer, sourceModuleCode As String, sourceModuleIdAction As Integer, sourceIdOwner As Long, sourceOwnerIdType As Long)
        View.IdCommunityQuestionnaire = idCommunity
        View.SourceModuleIdAction = sourceModuleIdAction
        View.SourceModuleCode = sourceModuleCode
        View.CurrentAction = QuestionnaireAction.None
        View.SourceIdOwner = sourceIdOwner
        View.SourceIdOwnerType = sourceOwnerIdType
        If (UserContext.isAnonymous) OrElse sourceIdOwner = 0 Then
            View.DisplaySessionTimeout(idCommunity, CurrentManager.GetModuleID(sourceModuleCode))
        Else
            View.LoadAvailableActions(GetAvailableActions(idCommunity), QuestionnaireAction.SelectAction)
        End If
    End Sub

    Private Function GetAvailableActions(idCommunity As Integer) As List(Of QuestionnaireAction)
        Dim mQuestionnaire As ModuleQuestionnaire = Service.GetModulePermission(idCommunity, UserContext.CurrentUserID, UserContext.UserTypeID)
        If (UserContext.isAnonymous) Then
            View.DisplaySessionTimeout(idCommunity, CurrentManager.GetModuleID(View.SourceModuleCode))
            Return New List(Of QuestionnaireAction)
        Else
            Return GetAvailableActions(mQuestionnaire, idCommunity)
        End If
    End Function
    Private Function GetAvailableActions(mQuestionnaire As ModuleQuestionnaire, idCommunity As Integer) As List(Of QuestionnaireAction)
        Dim items As New List(Of QuestionnaireAction)
        items.Add(QuestionnaireAction.AddStandard)
        items.Add(QuestionnaireAction.AddRandom)
        items.Add(QuestionnaireAction.AddRandomRepeat)

        If (mQuestionnaire.Administration OrElse mQuestionnaire.ManageInvitedQuestionnaire) AndAlso Service.HasSelectableQuestionnaire(idCommunity) Then
            items.Add(QuestionnaireAction.ImportFromCommunity)
        End If

        Return items
    End Function

    Public Sub ChangeAction(action As QuestionnaireAction)
        Select Case action
            Case QuestionnaireAction.ImportFromCommunity
                View.InitializeQuestionnaireSelector(View.IdCommunityQuestionnaire, View.SourceModuleCode, View.SourceModuleIdAction)
        End Select

        View.DisplayAction(action)
    End Sub
End Class