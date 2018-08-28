Imports COL_Questionario.Business
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.ModuleLinks
Imports System.Linq

Public Class GenerateQuestionnaireUrlPresenter
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
    Protected Overridable ReadOnly Property View() As IViewGenerateQuesionnaireUrl
        Get
            Return DirectCast(MyBase.View, IViewGenerateQuesionnaireUrl)
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
    Public Sub New(oContext As iApplicationContext, view As IViewGenerateQuesionnaireUrl)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout(Service.GetQuestionnaireIdCommunity(View.PreloadedIdQuestionnnaire))
        Else
            Dim q As LazyQuestionnaire = Service.GetItem(Of LazyQuestionnaire)(View.PreloadedIdQuestionnnaire)
            If IsNothing(q) Then
                View.DisplayUnknownQuesionnaire()
            Else
                Dim p As Person = Service.GetItem(Of Person)(UserContext.CurrentUserID)
                If IsNothing(p) OrElse (p.TypeID = UserTypeStandard.Guest OrElse p.TypeID = UserTypeStandard.PublicUser) Then
                    View.DisplayAccessError(Service.GetItemName(q.Id, UserContext.Language.Id))
                Else
                    Dim permission As ModuleQuestionnaire = Service.GetCommunityPermission(q.Id)
                    If permission.Compile AndAlso (q.ForCommunityUsers OrElse q.ForCommunityUsers) Then
                        View.GotoQuestionnaire(q.Id, UserContext.CurrentUserID)
                    ElseIf q.ForInvitedUsers Then
                        If Service.ExistInvitedUser(q.Id, UserContext.CurrentUserID) Then
                            View.GotoQuestionnaire(q.Id, UserContext.CurrentUserID)
                        Else
                            View.DisplayAccessError(Service.GetItemName(q.Id, UserContext.Language.Id), p.Name, p.Surname)
                        End If
                    Else
                        View.DisplayAccessError(Service.GetItemName(q.Id, UserContext.Language.Id), p.Name, p.Surname)
                    End If
                End If
            End If
        End If
    End Sub

End Class