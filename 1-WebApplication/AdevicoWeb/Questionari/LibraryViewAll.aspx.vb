Imports COL_Questionario

Partial Public Class LibraryViewAll
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
    Private _CurrentLibrary As dtoWorkingLibrary
    Private ReadOnly Property CurrentLibrary() As dtoWorkingLibrary
        Get
            If IsNothing(_CurrentLibrary) Then
                _CurrentLibrary = LoadedLibraries.Where(Function(l) l.IdLibrary.Equals(qs_questId)).FirstOrDefault
                If IsNothing(_CurrentLibrary) Then
                    _CurrentLibrary = New dtoWorkingLibrary(New TimeSpan(0, 5, 0))
                    _CurrentLibrary.Library = DALQuestionario.readFullLibraryBYLingua(PageUtility.CurrentContext, qs_questId, qs_LanguageId)
                    _CurrentLibrary.IdLibrary = qs_questId
                    LoadedLibraries.Add(_CurrentLibrary)
                ElseIf Not _CurrentLibrary.IsValid Then
                    _CurrentLibrary.Library = DALQuestionario.readFullLibraryBYLingua(PageUtility.CurrentContext, qs_questId, qs_LanguageId)
                    _CurrentLibrary.LoadedOn = DateTime.Now
                End If
            End If
           
            Return _CurrentLibrary
        End Get
    End Property
#End Region

    'Dim oQuest As New Questionario
    Public Shared isAperto As Boolean
    Public Shared _iPag As Integer ' indice pagina
    Public Shared iDom As Integer ' indice domanda
    'Public Shared idQ As String
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneQuest As New GestioneQuestionario
    Private _SmartTagsAvailable As SmartTags
    Private Property isAdmin As Boolean
        Get
            If IsNumeric(Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)) Then
                Return Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)
            Else
                isAdmin = False
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("qsIsAdmin_" & Me.QuestionarioCorrente.id) = value
        End Set
    End Property
    Private Property iPag() As Integer
        Get
            Return _iPag
        End Get
        Set(ByVal value As Integer)
            _iPag = value
        End Set
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Private Property HasUserResponses() As Boolean
        Get
            Return ViewStateOrDefault("HasUserResponses", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("HasUserResponses") = value
        End Set
    End Property
    Private Property CurrentFilterQuestion() As Integer
        Get
            Return ViewStateOrDefault("CurrentFilterQuestion", 1)
        End Get
        Set(ByVal value As Integer)
            ViewState("CurrentFilterQuestion") = value
        End Set
    End Property
    Private Sub caricaDati(Optional ByRef oQuestPagine As List(Of QuestionarioPagina) = Nothing)
        If Not IsNothing(CurrentLibrary) Then
            isAperto = Not CurrentLibrary.Library.isReadOnly
            If oQuestPagine Is Nothing Then
                DLPagine.DataSource = CurrentLibrary.Library.pagine
            Else
                DLPagine.DataSource = oQuestPagine
            End If

            DLPagine.DataBind()
            DLPagine = oGestioneDomande.setDomandePaginaEdit(DLPagine, CurrentLibrary)
            LBTitolo.Text = CurrentLibrary.Library.nome
         
        End If
    End Sub

    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        iDom = e.Item.ItemIndex
        'oQuest = Session("oQuest")
        Dim oQuestionario As New Questionario
        oQuestionario.pagine = DLPagine.DataSource
        'LBremovedQuestion()
      
        If Not IsNothing(CurrentLibrary) Then
            DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(oQuestionario, iPag, iDom, True))
            Dim LBtestoDopoDomanda As New Label
            LBtestoDopoDomanda.Text = Me.CurrentLibrary.Library.pagine(iPag).domande(iDom).testoDopo
            If Not LBtestoDopoDomanda.Text Is String.Empty Then
                Dim aCapo As New LiteralControl
                aCapo.Text = "<br>"
                DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(LBtestoDopoDomanda)
            End If
            If DirectCast(e.Item.DataItem, Domanda).VirtualNumber > 0 Then
                e.Item.FindControl("LBremovedQuestion").Visible = True
                e.Item.FindControl("LBremovedQuestionNumber").Visible = True
                e.Item.FindControl("LBquestionNumber").Visible = False
            End If
        End If
        SetInternazionalizzazioneDLDomande(e)
    End Sub
    Protected ReadOnly Property MandatoryDisplay(ByVal question As Domanda) As String
        Get
            Dim mandatory As String = ""
            If question.isObbligatoria Then
                'mandatory = "<span class=""mandatory"" title=""" & Me.Resource.getValue("isMandatory.Title") & """>" & Me.Resource.getValue("isMandatory") & "</span>"
                mandatory = "<span class=""mandatory"">" & Me.Resource.getValue("isMandatory") & "</span>"
            End If
            Return mandatory
        End Get
    End Property
    Protected ReadOnly Property MandatoryToolTip(ByVal question As Domanda) As String
        Get
            Return IIf(question.isObbligatoria, Me.Resource.getValue("isMandatory.Title"), "")
        End Get
    End Property
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        iPag = e.Item.ItemIndex
     
        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")
        'dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        Dim oPagineList As New List(Of QuestionarioPagina)
        oPagineList = DLPagine.DataSource
        dlDomande.DataSource = oPagineList.Item(iPag).domande
        dlDomande.DataBind()
        SetInternazionalizzazioneDLPagine(e)

        Try
            DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).SelectedValue = CurrentFilterQuestion
        Catch ex As Exception

        End Try
      
    End Sub
    'Protected Sub caricaQuestionario()
    '    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)

    '    caricaDati()
    'End Sub
    Protected Sub filtraDomande(ByVal RBLfiltro As RadioButtonList, ByVal e As System.EventArgs)
        CurrentFilterQuestion = RBLfiltro.SelectedValue
        Select Case RBLfiltro.SelectedValue
            Case 1
                caricaDati()
            Case 2
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In CurrentLibrary.Library.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 0 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
            Case 3
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In CurrentLibrary.Library.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 1 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
            Case 4
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In CurrentLibrary.Library.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.difficolta = 2 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
            Case 5
                Dim oPaginaList As New List(Of QuestionarioPagina)
                For Each oPagina As QuestionarioPagina In CurrentLibrary.Library.pagine
                    Dim oPaginaNew As New QuestionarioPagina
                    For Each oDomanda As Domanda In oPagina.domande
                        If oDomanda.numero = 0 Then
                            oPaginaNew.domande.Add(oDomanda)
                        End If
                    Next
                    oPaginaList.Add(oPaginaNew)
                Next
                caricaDati(oPaginaList)
        End Select
    End Sub
    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        'Select Case Me.QuestionarioCorrente.tipo
        '    Case COL_Questionario.Questionario.TipoQuestionario.Modello
        '        Me.RedirectToUrl(RootObject.ModelliGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Modello, String))
        '    Case COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande
        Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande, String))
        '    Case COL_Questionario.Questionario.TipoQuestionario.Sondaggio
        'Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Sondaggio, String))
        '    Case COL_Questionario.Questionario.TipoQuestionario.Meeting
        'Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Meeting, String))
        '    Case Else
        'Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&" & qs_questType + CType(COL_Questionario.Questionario.TipoQuestionario.Questionario, String))
        'End Select
    End Sub
    Protected Sub LNBGestioneQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneQuestionario.Click
        Me.RedirectToUrl(RootObject.QuestionarioAdmin + "?" & qs_questType + qs_questIdType.ToString() & "&IdQ=" & qs_questId & "&idLanguage=" & qs_LanguageId)
    End Sub
    Public Overrides Sub BindDati()
        Dim nRisposte As Integer = DALRisposte.countRisposteBYIDQuestionario(qs_questId)
        HasUserResponses = (nRisposte > 0)

        Me.MLVquestionari.SetActiveView(Me.VIWdati)
        caricaDati()
    End Sub
    Public Overrides Sub BindNoPermessi()
        LBerrore.Visible = True
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        If MyBase.Servizio.Admin Then
            isAdmin = True
        End If
        Return (isAdmin Or MyBase.Servizio.GestioneDomande)
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_LibreriaDomandeEdit", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBGestioneQuestionario, False, False)
            .setLabel(LBTitolo)
            .setLabel(LBMessage)
            .setLabel(LBerrore)
            .setLabel(LBIsRisposte)
        End With
        SetServiceTitle(Master)
    End Sub
    Public Sub SetInternazionalizzazioneDLPagine(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource


            If qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
                DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).Visible = False
            Else
                For Each item As ListItem In DirectCast(e.Item.FindControl("RBLfiltraDomande"), RadioButtonList).Items
                    item.Text = .getValue(item.Value & ".RBLfiltraDomande")
                Next
            End If
        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLDomande(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBremovedQuestion"))
        End With
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Private Function HasPermissionByExternalObject() As Boolean
        Dim result As Boolean = False
        Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()

        oSourceObject.ObjectTypeID = qs_ownerTypeId
        oSourceObject.ObjectLongID = qs_ownerId
        oSourceObject.ServiceCode = COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex
        oSourceObject.CommunityID = ComunitaCorrenteID

        Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
        oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)

        result = allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Edit, UtenteCorrente.ID, ComunitaCorrenteID, oSourceObject, oDestinationObject)
        Return result
    End Function
   
    Public Overrides Sub SetControlliByPermessi()
        'If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity OrElse Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
        '    LNBCartellaPrincipale.Visible = False
        'End If
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case Request.QueryString("confirm")
            Case "1"
                LBMessage.Text = Me.Resource.getValue("MSGconfermaCopiaQuestionario")
                LBMessage.Visible = True
            Case "2"
                LBMessage.Text = Me.Resource.getValue("MSGconfermaCopiaLibreria")
                LBMessage.Visible = True
        End Select

    End Sub
  
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType, type As QuestionnaireType)
        Select Case type
            Case QuestionnaireType.QuestionLibrary
                Select Case action
                    Case ModuleQuestionnaire.ActionType.QuestionAdd
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionAddToLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionDelete
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionDeleteFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionEdit
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionEditFromLibrary)
                    Case ModuleQuestionnaire.ActionType.QuestionVirtualRemove
                        AddAction(idItem, oType, ModuleQuestionnaire.ActionType.QuestionVirtualRemoveFromLibrary)
                    Case Else
                        AddAction(idItem, oType, action)
                End Select
            Case Else
                AddAction(idItem, oType, action)
        End Select
    End Sub
    Public Sub AddAction(idItem As Integer, ByVal oType As COL_Questionario.ModuleQuestionnaire.ObjectType, ByVal action As COL_Questionario.ModuleQuestionnaire.ActionType)
        Me.PageUtility.AddAction(action, PageUtility.CreateObjectsList(oType, idItem), lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class
