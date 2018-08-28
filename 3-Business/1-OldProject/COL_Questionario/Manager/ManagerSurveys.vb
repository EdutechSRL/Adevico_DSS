Imports Comol.Entity
Imports COL_BusinessLogic_v2

'Namespace COL_BusinessLogic_v2
Public Class ManagerSurveys
    Inherits ObjectBase
    Implements iManagerAdvanced

    Private _UseCache As Boolean
    Private _CurrentDB As Configuration.Components.ConnectionDB
    Private _CurrentUser As Person
    Private _CurrentCommunity As Community
    Private _Language As Lingua

    Public ReadOnly Property UseCache() As Boolean Implements iManagerAdvanced.UseCache
        Get
            Return _UseCache
        End Get
    End Property


    Public ReadOnly Property CurrentCommunity() As Community Implements iManagerAdvanced.CurrentCommunity
        Get
            Return _CurrentCommunity
        End Get
    End Property

    Public ReadOnly Property CurrentDB() As Configuration.Components.ConnectionDB Implements iManagerAdvanced.CurrentDB
        Get
            If IsNothing(_CurrentDB) Then
                _CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(Configuration.DBconnectionSettings.DBsetting.Esse3, Configuration.Components.ConnectionType.SQL)
            End If
            Return _CurrentDB
        End Get
    End Property

    Public ReadOnly Property CurrentUser() As Person Implements iManagerAdvanced.CurrentUser
        Get
            If Not _CurrentUser Is Nothing Then
                Return _CurrentUser
            Else
                ''da eliminare quando si inserisce la pagina nella masterpage e ComOl
                'Dim oPersona As New Person(123456, "NomeMio", "CognomeMio")
                Return New Person(0, "", "")
            End If
        End Get
    End Property

    Public ReadOnly Property Language() As Lingua Implements iManagerAdvanced.Language
        Get
            Return _Language
        End Get
    End Property

    Public Function GetDaySurveys(ByVal SurveyType As COL_Questionario.Questionario.TipoQuestionario, ByVal idCommunity As Integer, ByVal SurveyLanguageId As Integer, ByVal UserId As Integer, ByVal numberOfSurveys As Integer) As List(Of Questionario)

        Dim oQuestList As New List(Of Questionario)
        Dim oTopQuestList As New List(Of Questionario)
        Dim oTempQuestList As New List(Of Questionario)

        Try
            oQuestList = DALQuestionario.readDaySurveys(SurveyType, idCommunity)

            If numberOfSurveys = 1 Then

                ' se prendo solo un questionario, prelevo l'id del primo (sono già ordinati)

                Dim idFirstQuest As Integer = oQuestList(0).id

                ' controllo se esiste il questionario nella lingua dell'utente
                For Each oQ As Questionario In oQuestList
                    If oQ.id = idFirstQuest Then
                        If oQ.idLingua = SurveyLanguageId Then
                            oTopQuestList.Add(oQ)
                            Exit For
                        End If
                    End If
                Next

                ' se non esiste il questionario nella lingua dell'utente prendo quello nella lingua di default
                If oTopQuestList.Count = 0 Then
                    For Each oQ As Questionario In oQuestList
                        If oQ.id = idFirstQuest Then
                            If oQ.isDefault Then
                                oTopQuestList.Add(oQ)
                                Exit For
                            End If
                        End If
                    Next
                End If

            Else

                ' al momento ritorna una lista di tutti i questionari, mettere a posto per eliminare i duplicati multilingua

                For Each oQ As Questionario In oQuestList
                    'For Each oQ2 As Questionario In oQuestList
                    If oQ.isDefault Then
                        oTempQuestList.Add(oQ)
                    End If
                    'Next
                Next

                oQuestList = oTempQuestList

                Return oQuestList

            End If



        Catch ex As Exception

        End Try

        Return oTopQuestList

    End Function

    Public Function GetDaySurveyByUser(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal idSurvey As Integer, ByVal idSurveyLanguage As Integer, ByVal idUser As Integer) As Questionario

        Dim oquest As New Questionario

        Try
            If idSurvey > 0 Then
                oquest = DALQuestionario.readQuestionarioByPersona(appContext, True, idSurvey, idSurveyLanguage, idUser, 0)
            End If
        Catch ex As Exception

        End Try

        Return oquest

    End Function

    Public Function GetSurveyByLanguage(appContext As lm.Comol.Core.DomainModel.iApplicationContext, ByVal SurveyId As Integer, ByVal LanguageId As Integer, ByVal LoadAnswers As Boolean) As Questionario

        Dim oQuest As New Questionario
        Try
            oQuest = DALQuestionario.readQuestionarioBYLingua(appContext, SurveyId, LanguageId, LoadAnswers)
        Catch ex As Exception

        End Try
        Return oQuest

    End Function

    Public Function CountCommunitySurveys(ByVal CommunityId As Integer, ByVal SurveyType As Integer) As Integer

        Dim nQuest As Integer
        Try
            nQuest = DALQuestionario.CountQuestionariComunita(CommunityId, SurveyType)
        Catch ex As Exception

        End Try
        Return nQuest

    End Function

End Class
'End Namespace

