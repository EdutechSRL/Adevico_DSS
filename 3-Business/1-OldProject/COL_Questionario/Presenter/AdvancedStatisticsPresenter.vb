Imports COL_Questionario.Business
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel
Imports System.Linq

Public Class AdvancedStatisticsPresenter
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
    Protected Overridable ReadOnly Property View() As IViewAdvancedStatistics
        Get
            Return DirectCast(MyBase.View, IViewAdvancedStatistics)
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
    Public Sub New(oContext As iApplicationContext, view As IViewAdvancedStatistics)
        MyBase.New(oContext, view)
        Me.CurrentManager = New BaseModuleManager(oContext)
    End Sub
#End Region

    Public Sub InitView()
        Dim context As dtoContext = GenerateContext()
        If UserContext.isAnonymous Then
            View.DisplaySessionTimeout()
        Else
            Dim idQuest As Integer = context.IdQuestionnaire
            Dim item As LazyQuestionnaire = Service.GetItem(Of LazyQuestionnaire)(idQuest)

            If IsNothing(item) Then
                View.DisplaySessionTimeout()
            Else
                Dim group As LazyGroup = Service.GetItem(Of LazyGroup)(item.IdGroup)
                If IsNothing(group) Then
                    context.IdCommunity = IIf(context.IdCommunity = 0, UserContext.CurrentCommunityID, context.IdCommunity)
                    View.QuestContext = context
                    View.DisplayUnknownItem()
                Else
                    Dim idCommunity As Integer = group.IdCommunity
                    context.IdCommunity = idCommunity
                    View.QuestContext = context

                    Dim moduleQ As ModuleQuestionnaire
                    If context.IdOwner > 0 Then
                        moduleQ = View.GetModuleByLink(idCommunity, UserContext.CurrentUserID, Service.GetItem(Of ModuleLink)(context.IdLink))
                    Else
                        moduleQ = ModuleQuestionnaire.CreateByPermission(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, Service.ServiceModuleID))
                    End If
                    If moduleQ.Administration OrElse moduleQ.ViewStatistics Then
                        InitializePage(item, context)
                    ElseIf moduleQ.ViewPersonalStatistics Then
                        View.LoadUserStatisticsPage(context, UserContext.CurrentUserID)
                    Else
                        View.DisplayNoPermission()
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GenerateContext() As dtoContext
        Dim context As New dtoContext
        context.BackUrl = View.PreloadedBackUrl
        context.IdCommunity = View.PreloadedIdCommunity
        context.IdLink = View.PreloadedIdLink
        context.IdOwner = View.PreloadedIdOwner
        context.IdOwnerGuid = View.PreloadedIdOwnerGuid
        context.IdOwnerType = View.PreloadedIdOwnerType
        context.IdQuestionnaire = View.PreloadedIdQuestionnaire

        Return context
    End Function

    Private Sub InitializePage(item As LazyQuestionnaire, ByVal context As dtoContext)
        Dim participants As List(Of participantType) = Service.GetAvailableParticipantTypes(item, context.IdCommunity)
        View.LoadAvailableParticipant(participants)
        If (participants.Count > 0) Then
            View.SelectedParticipantTypes.Add(participants(0))
        End If


    End Sub
End Class