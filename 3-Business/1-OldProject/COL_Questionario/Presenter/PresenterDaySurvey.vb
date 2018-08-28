Imports lm.Comol.Core.DomainModel

Public Class PresenterDaySurvey
    Private _View As IviewDaySurvey
    Private _Manager As ManagerSurveys

    Public Sub New(ByVal view As IviewDaySurvey)
        _View = view
    End Sub
    Private Shadows ReadOnly Property View() As IviewDaySurvey
        Get
            View = _View
        End Get
    End Property

    Private ReadOnly Property CurrentManager() As ManagerSurveys
        Get
            If _Manager Is Nothing Then
                _Manager = New ManagerSurveys
            End If
            Return _Manager
        End Get
    End Property

    Public Sub Init(appContext As lm.Comol.Core.DomainModel.iApplicationContext)

        If View.IsActive Then
            Dim oQuestList As New List(Of Questionario)
            oQuestList = Me.CurrentManager.GetDaySurveys(Me.View.SurveyType, Me.View.CurrentCommunity.ID, Me.View.SurveyLanguageId, Me.View.CurrentUser.ID, Me.View.NumberOfDaySurveys)

            If oQuestList.Count = 1 Then
                'Dim oQuest As New Questionario
                Me.View.CurrentSurvey = Me.CurrentManager.GetDaySurveyByUser(appContext, oQuestList(0).id, Me.View.SurveyLanguageId, Me.View.CurrentUser.ID)

                If Me.View.CurrentSurvey.rispostaQuest.id > Integer.MinValue Then
                    If Me.View.ShowResults Then
                        LoadSurveyResults(appContext, Me.View.CurrentSurvey)
                    Else
                        Me.View.BindLinks(oQuestList)
                    End If
                Else
                    If Me.View.ShowPreview Then
                        Me.View.BindDataList(Me.View.CurrentSurvey)
                    Else
                        Me.View.BindLinks(oQuestList)
                    End If
                End If
            Else
                Me.View.BindLinks(oQuestList)
            End If
        End If

    End Sub

    Public Sub LoadSurveyResults(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal oQuest As Questionario)
        oQuest = Me.CurrentManager.GetSurveyByLanguage(appContext, oQuest.id, oQuest.idLingua, True)
        Me.View.BindPollResults(oQuest)
    End Sub

    Public Sub CountCommunitySurveys()
        Dim nQuest As Integer = 1
        nQuest = Me.CurrentManager.CountCommunitySurveys(Me.View.CurrentCommunity.ID, Me.View.SurveyType)
        Me.View.RedirectToPath(nQuest)
    End Sub


End Class
