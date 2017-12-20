Imports COL_Questionario

Public Class UC_SelectQuiz
    Inherits BaseControlWithLoad

#Region "Implements"
    Private Property SourceModuleCode As String
        Get
            Return ViewStateOrDefault("SourceModuleCode", "")
        End Get
        Set(value As String)
            ViewState("SourceModuleCode") = value
        End Set
    End Property
    Private Property SourceModuleIdAction As Integer
        Get
            Return ViewStateOrDefault("SourceModuleIdAction", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("SourceModuleIdAction") = value
        End Set
    End Property
    Private Property SourceIdOwnerType As Long
        Get
            Return ViewStateOrDefault("SourceIdOwnerType", CLng(0))
        End Get
        Set(value As Long)
            ViewState("SourceIdOwnerType") = value
        End Set
    End Property
    Private Property SourceIdOwner As Long
        Get
            Return ViewStateOrDefault("SourceIdOwner", CLng(0))
        End Get
        Set(value As Long)
            ViewState("SourceIdOwner") = value
        End Set
    End Property
    Private Property IdCommunityQuestionnaire As Integer
        Get
            Return ViewStateOrDefault("IdCommunityQuestionnaire", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCommunityQuestionnaire") = value
        End Set
    End Property

    Public Event QuestionnaireImported()

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region



#Region "Inherited Method"
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ModuleToQuiz", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLiteral(LTnoQuiz)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(ByVal idCommunity As Integer, ByVal sModuleCode As String, ByVal sModuleIdAction As Integer, sIdOwner As Long, sOwnerIdType As Long)
        Me.IdCommunityQuestionnaire = idCommunity
        Me.SourceModuleIdAction = sModuleIdAction
        Me.SourceModuleCode = sModuleCode
        Me.SourceIdOwner = sIdOwner
        Me.SourceIdOwnerType = sOwnerIdType

        Dim quizList As List(Of Questionario)
        quizList = DALQuestionario.readQuestionariByComunita(idCommunity)
        If quizList.Count > 0 Then
            RPTquiz.DataSource = quizList
            RPTquiz.DataBind()
        Else
            Me.LTnoQuiz.Visible = True
        End If
    End Sub

    Public Sub goToOwner(ByRef idQuestionario As Integer, ByRef owType As Integer, ByRef owId As Long)
        Select Case owType
            Case OwnerType_enum.EduPathActivity
                RedirectToUrl(RootObject.EduPath_CreateInActivity(owId, idQuestionario, OwnerType_enum.EduPathSubActivity))
            Case OwnerType_enum.EduPathSubActivity
                RedirectToUrl(RootObject.EduPath_CreateInActivity(owId, idQuestionario, OwnerType_enum.EduPathSubActivity))
            Case Else
        End Select
    End Sub

 
#Region "Control"
    Private Sub RPTquiz_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTquiz.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oQuest As Questionario = DirectCast(e.Item.DataItem, Questionario)
            Dim oLinkButton As LinkButton = e.Item.FindControl("LNBimportQuestionnaire")
            oLinkButton.Visible = (Me.SourceIdOwner > 0)
            Resource.setLinkButton(oLinkButton, False, True)

            Dim oLiteral As Literal = e.Item.FindControl("LTquestionnaireType")
            oLiteral.Text = Resource.getValue("QuestionnaireType." & oQuest.tipo)
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTquestionnaireName_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTquestionnaireAction_t")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTquestionnaireType_t")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
    Private Sub RPTquiz_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTquiz.ItemCommand
        Dim idQuestionnaire As Integer = 0
        If IsNumeric(e.CommandArgument) Then
            Dim oGestioneQuest As New GestioneQuestionario
            idQuestionnaire = CInt(e.CommandArgument)
            goToOwner(oGestioneQuest.importaQuestionario(e.CommandArgument, OwnerType_enum.EduPathSubActivity, SourceIdOwner), OwnerType_enum.EduPathSubActivity, SourceIdOwner)
        End If
    End Sub
#End Region


   
End Class