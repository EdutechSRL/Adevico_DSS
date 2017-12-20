Imports COL_Questionario

Partial Public Class Risposte
    Inherits PageBaseQuestionario

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext, .DataContext = lm.Comol.UI.Presentation.SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Service As COL_Questionario.Business.ServiceQuestionnaire
    Private ReadOnly Property CurrentService() As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_Service) Then
                _Service = New COL_Questionario.Business.ServiceQuestionnaire(Me.CurrentContext)
            End If
            Return _Service
        End Get
    End Property
#End Region

    Public Shared iDom As Integer
    Dim oGestioneDomande As New GestioneDomande
    Dim oPagedDataSource As New PagedDataSource
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oQuestionariList As New List(Of Questionario)
    Private isInitialized As Boolean

    Private ReadOnly Property HasEditingPermissions As Boolean
        Get
            If isInitialized Then
                Return ViewStateOrDefault("HasEditingPermissions", False)
            Else
                isInitialized = True
                Dim editingPermission As Boolean = False
                If qs_ownerTypeId = OwnerType_enum.None Then
                    Dim moduleQ As ModuleQuestionnaire = CurrentService.GetCommunityPermission(Me.Request.QueryString("idq"))
                    editingPermission = moduleQ.Administration OrElse moduleQ.ViewStatistics
                Else
                    Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
                    oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)
                    Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()
                    oSourceObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_ownerId, qs_ownerTypeId, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
                    If qs_questId < 1 Then
                        editingPermission = False
                    Else
                        editingPermission = allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewAdvancedStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) OrElse allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Admin, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject)
                    End If
                End If
                ViewState("HasEditingPermissions") = editingPermission
                Return editingPermission
            End If
        End Get
    End Property
    Private Property iPag() As Integer
        Get
            Return ViewState("idPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("idPag") = value
        End Set
    End Property
    Protected Sub loadRisposte(ByVal idQuest As Integer, ByVal idPerson As Integer, ByVal idUser As Integer)
        Dim risposteList As New List(Of RispostaQuestionario)
        risposteList = DALRisposte.readRisposteBYIDQuestionario(idQuest, idPerson, idUser, True)
        Dim oQuest As Questionario = New Questionario 'MyBase.GetQuestionnaireById(idQuest)
        'ElseIf risposteList.Count = 0 Then
        '    oQuest = Me.QuestionarioCorrente
        'End If

        For Each oRisposta As RispostaQuestionario In risposteList
            Dim idQuestionario As Integer
            If oRisposta.idQuestionarioRandom = 0 Then
                idQuestionario = oRisposta.idQuestionario
            Else
                idQuestionario = oRisposta.idQuestionarioRandom
            End If

            oQuest = DALQuestionario.readQuestionarioByPersona(Me.PageUtility.CurrentContext, False, idQuestionario, LinguaID, oRisposta.idPersona, oRisposta.idUtenteInvitato, oRisposta.id)
            oQuest.rispostaQuest.oStatistica.nomeUtente = oRisposta.oStatistica.nomeUtente
            oQuestionariList.Add(oQuest)
        Next
        DLquestionari.DataSource = oQuestionariList
        DLquestionari.DataBind()

    End Sub
    Public Overrides Sub BindDati()
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_risposte", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBerrore)
        End With
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Public Overrides Sub BindNoPermessi()
        PNLDettagli.Visible = False
        PNLerrore.Visible = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        loadRisposte(Me.Request.QueryString("idq"), qs_PersonaId, 0)

        If qs_ownerTypeId = OwnerType_enum.None Then
            Dim moduleQ As ModuleQuestionnaire = CurrentService.GetCommunityPermission(Me.Request.QueryString("idq"))

            Return (moduleQ.Administration OrElse moduleQ.ViewStatistics)
        Else

            Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)
            Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()
            oSourceObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_ownerId, qs_ownerTypeId, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
            If qs_questId < 1 Then
                Return False
            Else
                Return allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.ViewAdvancedStatistics, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject) OrElse allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Admin, UtenteCorrente.ID, 0, oSourceObject, oDestinationObject)
            End If
        End If
    End Function
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Private Sub DLquestionari_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLquestionari.ItemDataBound
        Dim PHquestionario As New PlaceHolder
        PHquestionario = DLquestionari.Controls(e.Item.ItemIndex).FindControl("PHquestionario")
        PHquestionario.Controls.Clear()
        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucVisualizzaRisposta)
        ctrl.ID = "ucVisualizzaRisposta_" & e.Item.ItemIndex.ToString
        DirectCast(ctrl, ucVisualizzaRisposta).addUCpnlValutazione(oQuestionariList(e.Item.ItemIndex).rispostaQuest.oStatistica)
        Dim DLPagine As New DataList
        DLPagine = DirectCast(ctrl.FindControl("DLPagine"), DataList)
        DLPagine.DataSource = oQuestionariList(e.Item.ItemIndex).pagine
        DLPagine.DataBind()
        If oQuestionariList(e.Item.ItemIndex).visualizzaCorrezione OrElse (oQuestionariList(e.Item.ItemIndex).domande.Where(Function(d) d.isValutabile).Any()) Then
            DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, oQuestionariList(e.Item.ItemIndex).domande)
        Else
            DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, oQuestionariList(e.Item.ItemIndex).domande, , False)
        End If
        PHquestionario.Controls.Add(ctrl)

        Dim compilatore As String = Me.Resource.getValue("LBNomeUtente.text") & oQuestionariList(e.Item.ItemIndex).rispostaQuest.oStatistica.nomeUtente
        If oQuestionariList(e.Item.ItemIndex).risultatiAnonimi Then
            compilatore = Me.Resource.getValue("LBNomeUtente.text") & " ## " & oQuestionariList(e.Item.ItemIndex).rispostaQuest.id & " ##"
        End If
        DirectCast(e.Item.FindControl("LBnomeCompilatore"), Label).Text = compilatore
        DirectCast(e.Item.FindControl("LBnomeQuestionario"), Label).Text = Me.Resource.getValue("LBNomeQuestionario.text") & oQuestionariList(e.Item.ItemIndex).nome
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class