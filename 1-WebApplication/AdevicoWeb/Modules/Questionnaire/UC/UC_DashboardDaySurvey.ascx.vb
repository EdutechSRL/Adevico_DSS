Imports COL_Questionario

Partial Public Class UC_DashboardDaySurvey
    Inherits BaseControlQuestionario

#Region "Internal"
    Public Property CurrentSurvey() As Questionario
        Get
            Return ViewStateOrDefault("CurrentSurvey", New Questionario)
        End Get
        Set(ByVal value As Questionario)
            Me.ViewState("CurrentSurvey") = value
        End Set
    End Property
    Public Property SurveyType() As QuestionnaireType
        Get
            Return ViewStateOrDefault("SurveyType", QuestionnaireType.Poll)
        End Get
        Set(ByVal value As QuestionnaireType)
            Me.ViewState("SurveyType") = value
        End Set
    End Property
    Public Property IsActive() As Boolean
        Get
            Return ViewStateOrDefault("IsActive", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("IsActive") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDaySurvey", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBdaySurvey)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(idCommunity As Integer)
        InitializeControl(IsActive, idCommunity)
    End Sub
    Public Sub InitializeControl(active As Boolean, idCommunity As Integer)
        If active Then
            Dim manager As New ManagerSurveys()

            Dim surveys As List(Of Questionario) = manager.GetDaySurveys(SurveyType, idCommunity, PageUtility.LinguaID, PageUtility.CurrentUser.ID, 1)
            If surveys.Count > 0 Then
                Dim survey As Questionario = manager.GetDaySurveyByUser(PageUtility.CurrentContext, surveys.FirstOrDefault().id, PageUtility.LinguaID, PageUtility.CurrentUser.ID)
                CurrentSurvey = survey
                Me.DVdaySurvey.Visible = True
                LNBviewDaySurvey.Text = survey.nome
                LNBviewDaySurvey.CommandArgument = surveys(0).id
                Try
                    LBisNewSurvay.Visible = (DateDiff(DateInterval.Day, Now, DateTime.Parse(survey.dataInizio)) > -7)
                Catch ex As Exception
                    LBisNewSurvay.Visible = (survey.rispostaQuest.id = 0)
                End Try

            Else
                Me.DVdaySurvey.Visible = False
            End If
        Else
            Me.DVdaySurvey.Visible = False
        End If
    End Sub

    Private Sub LNBviewDaySurvey_Click(sender As Object, e As EventArgs) Handles LNBviewDaySurvey.Click
        RedirectToPath(DirectCast(sender, LinkButton).CommandArgument, 1)
    End Sub

    Public Sub RedirectToPath(ByVal idQuestionnaire As Integer, number As Integer)
        If number = 1 Then
            Dim s As Questionario = CurrentSurvey
            s.id = idQuestionnaire
            s.tipo = SurveyType
            s.url = Me.EncryptedUrl(RootObject.compileUrl, "idq=" & idQuestionnaire, SecretKeyUtil.EncType.Questionario)
            If s.rispostaQuest.id > 0 Then
                Me.QuestionarioCorrente = s
                Me.QuestionarioCorrente.url = s.url
                Me.RedirectToUrl(RootObject.QuestionarioStatisticheGenerali + "&comp=1")
            Else
                Response.Redirect(s.url)
            End If
        Else
            RedirectToUrl(RootObject.QuestionariList & "?type=" & Me.SurveyType)
        End If
    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub
End Class