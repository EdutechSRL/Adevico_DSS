Imports COL_Questionario
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web
Imports CrystalDecisions.Shared


Partial Public Class QuestionarioAdmin
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

    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneQuest As New GestioneQuestionario
    Public Shared isAperto As Boolean
    Private Property isAdmin As Boolean
        Get
            Dim isAdminMode As Boolean = False

            If IsNumeric(Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)) OrElse IsNumeric(ViewState("qsIsAdmin")) Then
                If Me.QuestionarioCorrente.id = 0 Then
                    Boolean.TryParse(ViewState("qsIsAdmin"), isAdminMode)
                Else
                    isAdminMode = Session("qsIsAdmin_" & Me.QuestionarioCorrente.id)
                End If
            Else
                isAdmin = isAdminMode
            End If
            Return isAdminMode
        End Get
        Set(ByVal value As Boolean)
            If Me.QuestionarioCorrente.id = 0 Then
                ViewState("qsIsAdmin") = value
            Else
                Session("qsIsAdmin_" & Me.QuestionarioCorrente.id) = value
            End If
        End Set
    End Property
    Public Sub goToOwner()
        If Not String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
            PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
        Else
            Select Case Me.QuestionarioCorrente.ownerType
                Case OwnerType_enum.EduPathActivity
                    RedirectToUrl(RootObject.EduPath_CreateInActivity(Me.QuestionarioCorrente.ownerId, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.ownerType))
                Case OwnerType_enum.EduPathSubActivity
                    RedirectToUrl(RootObject.EduPath_CreateInActivity(Me.QuestionarioCorrente.ownerId, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.ownerType))
                Case Else
            End Select
        End If
    End Sub


#Region "property ReadOnly"

    Public ReadOnly Property questionariSuInvito() As String
        Get
            Return (MyBase.Servizio.QuestionariSuInvito Or MyBase.Servizio.Admin).ToString
        End Get
    End Property

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

#End Region
#Region "Button Click Events"
    Protected Sub BTNCopiaCartella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCopiaCartella.Click
        'oQuest = Me.QuestionarioCorrente
        Me.QuestionarioCorrente.idGruppo = DDLCartelle.SelectedItem.Value
        Dim redUrl As String = oGestioneQuest.copiaQuestionario(FRVQuestionario)
    End Sub
    Protected Sub BTNCopiaComunita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCopiaComunita.Click
        If Not DDLComunita.SelectedItem.Value = Integer.MinValue Then
            Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(DDLComunita.SelectedItem.Value)
            If Me.QuestionarioCorrente.idGruppo = 0 Then
                oGestioneQuest.creaGruppoDefault(DDLComunita.SelectedItem.Value)
                Me.QuestionarioCorrente.idGruppo = DALQuestionarioGruppo.GruppoPrincipaleByComunita_Id(DDLComunita.SelectedItem.Value)
            End If
            LBCopiaComunita.Visible = True
            LBerrore.Visible = False
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
            SetInternazionalizzazione()
            Dim redUrl As String = oGestioneQuest.copiaQuestionario(FRVQuestionario)
            If Not redUrl = String.Empty Then
                Me.RedirectToUrl(redUrl)
            Else
                LNBTornaLista.Visible = True
                Resource.setLinkButtonForName(LNBTornaLista, "LNBCartellaPrincipale", False, True)
            End If
        Else
            LBerrore.Text = LBerrore.Text & Me.Resource.getValue("MSGNoComunita")
            MLVquestionari.SetActiveView(VIWmessaggi)
        End If

    End Sub
    Protected Sub BTNComunitaGruppo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNComunitaGruppo.Click
        DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Enabled = False
        DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Enabled = False

        DDLComunitaGruppo.DataSource = DALQuestionarioGruppo.readGruppi(DDLComunita.SelectedValue)
        DDLComunitaGruppo.Visible = True
        DDLComunitaGruppo.DataBind()
    End Sub
    Protected Sub BTNSalvaAltraLingua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSalvaAltraLingua.Click
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, 0, False)
        Me.QuestionarioCorrente.isReadOnly = True
        DALQuestionario.chiudiQuestionario(Me.QuestionarioCorrente)
        If Not DDLNuovaLingua.SelectedValue = String.Empty Then
            Me.QuestionarioCorrente.idLingua = DDLNuovaLingua.SelectedValue
            Me.QuestionarioCorrente.nome = Me.QuestionarioCorrente.nome & " (" & DDLNuovaLingua.SelectedItem.Text & ")"
            Me.RedirectToUrl(oGestioneQuest.copiaQuestionarioMultilingua(FRVQuestionario))
        End If
    End Sub
    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Try
            tornaAllaLista()
        Catch ex As Exception
            BindDati()
        End Try
    End Sub
    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        Try
            If Me.QuestionarioCorrente Is Nothing Then
                Me.QuestionarioCorrente = New Questionario
            End If
            Me.QuestionarioCorrente.tipo = Me.qs_questIdType
            If Not DDLGruppo.SelectedValue = String.Empty Then
                Me.QuestionarioCorrente.idGruppo = Integer.Parse(DDLGruppo.SelectedValue)
            End If
            Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita.Checked
            Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita.Checked
            'If Me.ComunitaCorrenteID = 0 Then
            '    Me.QuestionarioCorrente.forUtentiPortale = True
            'Else
            '    Me.QuestionarioCorrente.forUtentiPortale = False
            'End If

            Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati.Checked
            Me.QuestionarioCorrente.forUtentiEsterni = CHKUtentiEsterni.Checked
            Me.QuestionarioCorrente.idPersonaEditor = Me.UtenteCorrente.ID
            Me.QuestionarioCorrente.dataModifica = Now

            If Me.qs_questIdType = COL_Questionario.Questionario.TipoQuestionario.Modello Then
                If Me.QuestionarioCorrente.forUtentiComunita And Not Me.QuestionarioCorrente.forUtentiPortale Then
                    If Not DDLGruppo.SelectedValue = String.Empty Then
                        Me.QuestionarioCorrente.idGruppo = DDLGruppo.SelectedValue
                    End If
                Else
                    Me.QuestionarioCorrente.idGruppo = 0
                End If
            End If
            Dim saveOK As Boolean
            saveOK = oGestioneQuest.salvaQuestionario(FRVQuestionario, Me.QuestionarioCorrente)
            If saveOK Then
                SaveSkinSelection()
                isAdmin = True
                Me.RedirectToUrl(RootObject.QuestionarioEdit & "?" & qs_questType & Me.QuestionarioCorrente.tipo & "&" & qs_owner & qs_ownerId & "&" & qs_ownerType & qs_ownerTypeId)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub tornaAllaLista()
        Try
            If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
                Dim qType As COL_Questionario.Questionario.TipoQuestionario = qs_questIdType

                If qType = COL_Questionario.Questionario.TipoQuestionario.Compilabili OrElse qType = COL_Questionario.Questionario.TipoQuestionario.RandomRepeat OrElse qType = COL_Questionario.Questionario.TipoQuestionario.Random OrElse qType = COL_Questionario.Questionario.TipoQuestionario.Autovalutazione Then
                    Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&type=0")
                ElseIf qType = COL_Questionario.Questionario.TipoQuestionario.Modello Then
                    Me.RedirectToUrl(RootObject.ModelliGestioneList & "mode=0&type=3")
                    '   questionari/ModelliGestioneList.aspx?mode=0&type=3
                Else
                    Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & qType)
                End If
            Else
                goToOwner()
            End If
        Catch ex As Exception
            BindDati()
        End Try
    End Sub
    Protected Sub LNBSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva.Click
        Dim saveOK As Boolean
        If Me.QuestionarioCorrente Is Nothing Then
            Me.QuestionarioCorrente = New Questionario
        End If
        Me.QuestionarioCorrente.tipo = Me.qs_questIdType
        If Not DDLGruppo.SelectedValue = String.Empty Then
            Me.QuestionarioCorrente.idGruppo = DDLGruppo.SelectedValue
        End If

        Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita.Checked
        Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita.Checked
        'If Me.ComunitaCorrenteID = 0 Then
        '    Me.QuestionarioCorrente.forUtentiPortale = True
        'Else
        '    Me.QuestionarioCorrente.forUtentiPortale = False
        'End If
        Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati.Checked
        Me.QuestionarioCorrente.forUtentiEsterni = CHKUtentiEsterni.Checked
        Me.QuestionarioCorrente.idPersonaEditor = Me.UtenteCorrente.ID
        Me.QuestionarioCorrente.dataModifica = Now

        If Me.qs_questIdType = COL_Questionario.Questionario.TipoQuestionario.Modello Then
            If Me.QuestionarioCorrente.forUtentiComunita And Not Me.QuestionarioCorrente.forUtentiPortale Then
                If Not DDLGruppo.SelectedValue = String.Empty Then
                    Me.QuestionarioCorrente.idGruppo = DDLGruppo.SelectedValue
                End If
            Else
                Me.QuestionarioCorrente.idGruppo = 0
            End If
        End If

        saveOK = oGestioneQuest.salvaQuestionario(FRVQuestionario, Me.QuestionarioCorrente)

        If Not DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList).SelectedValue = Me.QuestionarioCorrente.idLingua Then
            DALQuestionario.updateQuestionarioDefault(Me.QuestionarioCorrente.id, Integer.Parse(DirectCast(FRVQuestionario.FindControl("DDLLingua"), DropDownList).SelectedValue))
        End If
        If saveOK Then
            If Me.QuestionarioCorrente.tipo = QuestionnaireType.QuestionLibrary Then
                Me.SaveLibrarySettings(Me.QuestionarioCorrente.id)
            End If
            Me.SaveSkinSelection()

            SaveSkinSelection()
            If Not Me.QuestionarioCorrente.isBloccato Then
                oGestioneQuest.notifyCurrentQuestionnaire()
            End If
            tornaAllaLista() '.RedirectToUrl(RootObject.QuestionariGestioneList)
        End If
    End Sub
    Protected Sub BTNInvitaUtenti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNInvitaUtenti.Click
        If Me.QuestionarioCorrente Is Nothing Then
            Me.QuestionarioCorrente = New Questionario
        End If
        If Not DDLGruppo.SelectedValue = String.Empty Then
            Me.QuestionarioCorrente.idGruppo = DDLGruppo.SelectedValue
        End If
        Me.QuestionarioCorrente.forUtentiComunita = CHKUtentiComunita.Checked
        Me.QuestionarioCorrente.forUtentiPortale = CHKUtentiNonComunita.Checked
        'If Me.ComunitaCorrenteID = 0 Then
        '    Me.QuestionarioCorrente.forUtentiPortale = True
        'Else
        '    Me.QuestionarioCorrente.forUtentiPortale = False
        'End If
        Me.QuestionarioCorrente.forUtentiInvitati = CHKUtentiInvitati.Checked
        Me.QuestionarioCorrente.forUtentiEsterni = CHKUtentiEsterni.Checked
        Dim saveOK As Boolean
        saveOK = oGestioneQuest.salvaQuestionario(FRVQuestionario, Me.QuestionarioCorrente)
        If saveOK Then
            SaveSkinSelection()
            Me.RedirectToUrl(RootObject.UtentiInvitati & "?IdQ=" & QuestionarioCorrente.id)
        End If

    End Sub
    Protected Sub LNBImporta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBImporta.Click
        Select Case Me.qs_questIdType
            Case COL_Questionario.Questionario.TipoQuestionario.Modello
                Dim questNome As String
                Dim copiePresenti As Integer = 1
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, 1, False)
                Dim idQuest As Integer = Me.QuestionarioCorrente.id
                Dim idTypeQuest As Integer = Me.QuestionarioCorrente.tipo
                'If DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia di " & Me.QuestionarioCorrente.nome) = 0 Then
                If Not DALQuestionario.GetDuplicatedItemsByName(Me.ComunitaCorrenteID, idQuest, idTypeQuest, "Copia di " & Me.QuestionarioCorrente.nome).Any Then
                    questNome = "Copia di " & Me.QuestionarioCorrente.nome
                    Me.QuestionarioCorrente.nome = questNome
                Else
                    Do
                        copiePresenti = copiePresenti + 1
                    Loop While Not DALQuestionario.GetDuplicatedItemsByName(Me.ComunitaCorrenteID, idQuest, idTypeQuest, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome).Any
                    'Loop While Not DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome) = 0
                    Me.QuestionarioCorrente.nome = "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome
                    questNome = Me.QuestionarioCorrente.nome
                End If
                oGestioneQuest.copiaModello()
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=0")
            Case Else
                Me.RedirectToUrl(RootObject.ModelliGestioneList + "&type=3")
        End Select
    End Sub


#End Region
#Region "Bind Controls"
    Protected Sub bindDateTime()
        Dim counter As Int32
        For counter = 0 To 9
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items(counter).Text = "0" & counter
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items(counter).Value = counter
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items(counter).Text = "0" & counter
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items(counter).Value = counter
        Next
        For counter = 10 To 23
            DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Items.Add(counter)
        Next
        For counter = 0 To 1
            'visualizza 00 e 05 per i minuti
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Text = "0" & counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Value = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Text = "0" & counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Value = counter * 5
        Next
        For counter = 2 To 11
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Text = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Items(counter).Value = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items.Add(counter)
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Text = counter * 5
            DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Items(counter).Value = counter * 5
        Next

        DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Date
        DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).SelectedDate = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Date
        DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Hour
        DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Hour


        Dim minIndex As Int16
        minIndex = DateTime.Parse(Me.QuestionarioCorrente.dataInizio).Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedIndex = minIndex
        minIndex = DateTime.Parse(Me.QuestionarioCorrente.dataFine).Minute / 5
        If minIndex = 12 Then
            minIndex = 11
        End If
        DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedIndex = minIndex
    End Sub
    Protected Sub bindDDLCartelle()
        'temporaneamente disattivo
        LBAltraCartella.Visible = False
        BTNCopiaCartella.Visible = False
        LBGruppo.Visible = False
        DDLCartelle.Visible = False
        DDLGruppo.Visible = False
        BTNComunitaGruppo.Visible = False
        DDLComunitaGruppo.Visible = False
        'DDLCartelle.DataSource = DALQuestionarioGruppo.readGruppi(Me.ComunitaCorrenteID)
        'DDLCartelle.DataBind()
    End Sub
    Protected Sub bindDDLComunita()
        DDLComunita.DataSource = DALQuestionario.readComunitaByIDPersona(Me.UtenteCorrente.ID, Me.ComunitaCorrente)
        DDLComunita.DataBind()
    End Sub
    Protected Sub bindDDLGruppo()
        Try
            DDLGruppo.DataSource = DALQuestionarioGruppo.readGruppi(Me.ComunitaCorrenteID)
            DDLGruppo.DataBind()
            DDLGruppo.SelectedValue = Me.GruppoCorrente.id
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub bindDDLLingua()
        DDLNuovaLingua.DataSource = DALQuestionario.readLingueNonPresentiQuestionario(Me.QuestionarioCorrente.id)
        DDLNuovaLingua.DataBind()
    End Sub
#End Region

    Private Sub caricaDati()
        If Not Me.QuestionarioCorrente Is Nothing Then
            If Me.QuestionarioCorrente.id > 0 Then
                Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
                Me.QuestionarioCorrente.url = Me.EncryptedUrl(RootObject.compileUrlUI, "idq=" & Me.QuestionarioCorrente.id & "&idl=" & Me.QuestionarioCorrente.idLingua & "&ida=1", SecretKeyUtil.EncType.Questionario)
                isAperto = Not Me.QuestionarioCorrente.isReadOnly
                HYPUrl.NavigateUrl = Me.QuestionarioCorrente.url
                CHKUtentiComunita.Checked = Me.QuestionarioCorrente.forUtentiComunita
                CHKUtentiNonComunita.Checked = Me.QuestionarioCorrente.forUtentiPortale
                CHKUtentiInvitati.Checked = Me.QuestionarioCorrente.forUtentiInvitati
                CHKUtentiEsterni.Checked = Me.QuestionarioCorrente.forUtentiEsterni
            Else 'creo un nuovo questionario
                isAperto = True
                With Me.QuestionarioCorrente
                    .dataCreazione = Now
                    .dataInizio() = Now
                    .dataFine() = Date.MaxValue
                    .idPersonaCreator = Me.UtenteCorrente.ID
                    CHKUtentiComunita.Checked = True
                    .visualizzaRisposta = False
                    .editaRisposta = False
                    .visualizzaCorrezione = False
                    .visualizzaSuggerimenti = False
                    .ownerType = qs_ownerTypeId
                    .ownerGUID = qs_ownerGUID
                    .ownerId = qs_ownerId
                    If .ownerType = OwnerType_enum.None Then
                        .isBloccato = True
                        .scalaValutazione() = RootObject.scalaValutazione
                    Else
                        .isBloccato = False
                        .scalaValutazione() = 100
                    End If

                End With
            End If
        Else 'If qs_ownerId > 0 Then
            Me.QuestionarioCorrente = New Questionario
            Me.QuestionarioCorrente.ownerType = qs_ownerTypeId
            Me.QuestionarioCorrente.ownerGUID = qs_ownerGUID
            Me.QuestionarioCorrente.ownerId = qs_ownerId
            'Else

        End If
        If Not Me.qs_questIdType = COL_Questionario.Questionario.TipoQuestionario.Modello Then
            bindDDLCartelle()
            bindDDLComunita()
            bindDDLLingua()
        End If
        Dim idGruppo As Integer
        Try
            idGruppo = Me.GruppoCorrente.id
            bindDDLGruppo()
        Catch ex As Exception
            idGruppo = 0
        End Try

        oGestioneQuest.bindFieldQuestionario(FRVQuestionario, PNLDestinatari, Me.QuestionarioCorrente, idGruppo)
        bindDateTime()

    End Sub
    Protected Sub CUVNome_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim name As String = DirectCast(FRVQuestionario.FindControl("TXBNome"), TextBox).Text
        Dim oQuest As Questionario = Me.QuestionarioCorrente

        If QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            args.IsValid = Not oGestioneQuest.IsDuplicatedName(oQuest.id, oQuest.tipo, name) ' oGestioneQuest.controllaNome(Nome)
        Else
            args.IsValid = True
        End If
    End Sub
    Protected Sub CUVdate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim DataInizio As Date
        Dim DataFine As Date
        DataInizio = DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).SelectedValue)
        DataFine = DirectCast(FRVQuestionario.FindControl("RDPDataFine"), Telerik.WebControls.RadDatePicker).DateInput.SelectedDate.AddHours(DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).SelectedValue).AddMinutes(DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).SelectedValue)
        If DateDiff(DateInterval.Second, DataInizio, DataFine) < 0 And Not DataFine = Date.MaxValue Then
            args.IsValid = False
            Exit Sub
        End If
    End Sub
    Public Overrides Sub BindDati()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)
        If Not qs_ownerTypeId = OwnerType_enum.None AndAlso qs_questId = 0 Then
            Me.QuestionarioCorrente = New Questionario With {.tipo = qs_questIdType, .DisplayAvailableAttempts = False, .DisplayResultsStatus = True, .DisplayCurrentAttempts = True, .DisplayScoreToUser = True, .DisplayAttemptScoreToUser = True}

        End If
        Me.PaginaCorrenteID = 0
        caricaDati()
        SetCampiVisibili()

        If Me.QuestionarioCorrente.linguePresenti.Count > 1 Then
            DirectCast(FRVQuestionario.FindControl("CKisChiuso"), CheckBox).Enabled = False
        End If
        If qs_questIdType = Questionario.TipoQuestionario.Sondaggio OrElse qs_questIdType = Questionario.TipoQuestionario.Meeting Then
            FRVQuestionario.FindControl("LBDescrizioneQuestionario").Visible = False
            FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario").Visible = False
            Me.DomandaCorrente = New Domanda
            Me.DomandaCorrente.id = 0
            If qs_questIdType = Questionario.TipoQuestionario.Sondaggio Then
                Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
            ElseIf qs_questIdType = Questionario.TipoQuestionario.Meeting Then
                Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Meeting
            End If
        End If
    End Sub
    Public Sub SetCampiVisibili()
        If Me.QuestionarioCorrente.id > 0 And Me.QuestionarioCorrente.domande.Count > 0 Then
            LNBImporta.Visible = False
        Else
            PNLCopiaQuestionario.Style.Item("display") = "none"
            LBGestioneAvanzata.Visible = False
        End If
        DirectCast(FRVQuestionario.FindControl("FLSresults"), HtmlControl).Visible = False
        Select Case Me.qs_questIdType
            Case COL_Questionario.Questionario.TipoQuestionario.Modello
                If Me.QuestionarioCorrente.id > 0 Then
                    LNBImporta.Visible = True
                Else
                    PNLCopiaQuestionario.Visible = False
                    LBGestioneAvanzata.Visible = False
                    LNBImporta.Visible = False
                End If
                CHKUtentiEsterni.Visible = False
                CHKUtentiInvitati.Visible = False
                BTNInvitaUtenti.Visible = False
                HYPUrl.Visible = False
                PNLCopiaQuestionario.Visible = False
                If Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin Then
                    CHKUtentiNonComunita.Visible = True
                Else
                    CHKUtentiNonComunita.Visible = False
                End If
                DirectCast(FRVQuestionario.FindControl("LBDataInizioTitolo"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), RadDatePicker).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBDataFineTitolo"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("RDPDataFine"), RadDatePicker).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Visible = False
                LBGestioneAvanzata.Visible = False
                PNLCopiaQuestionario.Visible = False
                DirectCast(FRVQuestionario.FindControl("PNLModalita"), Panel).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBModalita"), Label).Visible = False
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                DirectCast(FRVQuestionario.FindControl("LBDataInizioTitolo"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("RDPDataInizio"), RadDatePicker).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLOraInizio"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLMinutiInizio"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBDataFineTitolo"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("RDPDataFine"), RadDatePicker).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLOraFine"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("DDLMinutiFine"), DropDownList).Visible = False
                DirectCast(FRVQuestionario.FindControl("PNLModalita"), Panel).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBModalita"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("CKisBloccato"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBisBloccato"), Label).Visible = False
                LBDestinatari.Visible = False
                PNLDestinatari.Visible = False
                PNLCopiaQuestionario.Visible = True
                LBGestioneAvanzata.Visible = False
                LNBImporta.Visible = False
                InitializeLibrarySettings()
            Case Questionario.TipoQuestionario.Autovalutazione
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Checked = True
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaRisposta"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaCorrezione"), Label).Text = Me.Resource.getValue("LBvisualizzaCorrezioneAutovalutazione.text")
                DirectCast(FRVQuestionario.FindControl("LBpermessiCompilatore"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Checked = False
                DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBeditaRisposta"), Label).Visible = False
                LNBImporta.Visible = False
            Case Questionario.TipoQuestionario.Sondaggio
                DirectCast(FRVQuestionario.FindControl("PNLModalita"), Panel).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBModalita"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBDurata"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("TBDurata"), TextBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Visible = True
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Checked = True
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaRisposta"), Label).Visible = True
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaCorrezione"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBpermessiCompilatore"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBpermessiCompilatore"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Checked = False
                DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBeditaRisposta"), Label).Visible = False
                LNBImporta.Visible = False
            Case Questionario.TipoQuestionario.RandomRepeat
                DirectCast(FRVQuestionario.FindControl("FLSresults"), HtmlControl).Visible = True
                DirectCast(FRVQuestionario.FindControl("DVdisplayScoreToUser"), HtmlControl).Visible = (Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)
                DirectCast(FRVQuestionario.FindControl("DVdisplayAttemptScoreToUser"), HtmlControl).Visible = (Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)
                DirectCast(FRVQuestionario.FindControl("DVdisplayAvailableAttemptsToUser"), HtmlControl).Visible = True
                DirectCast(FRVQuestionario.FindControl("DVdisplayCurrentAttemptToUser"), HtmlControl).Visible = True
                DirectCast(FRVQuestionario.FindControl("DVminScore"), HtmlControl).Visible = True
                DirectCast(FRVQuestionario.FindControl("DVmaxAttempts"), HtmlControl).Visible = True
                DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom_t"), Label).Visible = False
                LNBImporta.Visible = False
            Case Questionario.TipoQuestionario.Random
                'DirectCast(FRVQuestionario.FindControl("FLSresults"), HtmlControl).Visible = (Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)
                'DirectCast(FRVQuestionario.FindControl("DVdisplayScoreToUser"), HtmlControl).Visible = (Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)

                '   DirectCast(FRVQuestionario.FindControl("DVdisplayAttemptScoreToUser"), HtmlControl).Visible = (Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)
                DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom_t"), Label).Visible = False
                LNBImporta.Visible = False
            Case Questionario.TipoQuestionario.Meeting
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Visible = True
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaRisposta"), CheckBox).Checked = True
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaCorrezione"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaCorrezione"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("DIVDurata"), HtmlControls.HtmlControl).Style("display") = "none"
                DirectCast(FRVQuestionario.FindControl("LBScalaValutazione"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("TXBScalaValutazione"), TextBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom_t"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKvisualizzaSuggerimenti"), CheckBox).Visible = False
                DirectCast(FRVQuestionario.FindControl("LBvisualizzaSuggerimenti"), Label).Visible = False
                DirectCast(FRVQuestionario.FindControl("CHKeditaRisposta"), CheckBox).Checked = True
            Case Else
                '(Me.QuestionarioCorrente.ownerType <> OwnerType_enum.None)
        End Select
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity OrElse Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
            'Select Case Me.qs_questIdType
            'Case COL_Questionario.Questionario.TipoQuestionario.Questionario
            DIVdestinatari.Style("display") = "none"
            DirectCast(FRVQuestionario.FindControl("DIVAdvancedParameters"), HtmlControls.HtmlControl).Style("display") = "none"
            DirectCast(FRVQuestionario.FindControl("CHKOrdineDomandeRandom"), CheckBox).Visible = False
            'DirectCast(FRVQuestionario.FindControl("LBOrdineDomandeRandom"), Label).Visible = False

            'If Me.QuestionarioCorrente.tipo <> QuestionnaireType.RandomMultipleAttempts Then
            '    DirectCast(FRVQuestionario.FindControl("LBScalaValutazione"), Label).Visible = False
            '    DirectCast(FRVQuestionario.FindControl("TXBScalaValutazione"), TextBox).Visible = False
            'End If
            LNBCartellaPrincipale.Visible = False
            LBGestioneAvanzata.Visible = False
            PNLCopiaQuestionario.Visible = False
            LNBUndo.Visible = True
            LNBImporta.Visible = False 'oppure si devono fare tutti i dovuti controlli anche sulle pagine dei modelli e cambiarne il sistema di importazione
            'End Select
        End If
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If MLVquestionari.ActiveViewIndex = 0 Then
            LNBGestioneDomande.Visible = (isAdmin Or MyBase.Servizio.GestioneDomande)
            HYPviewAllQuestions.Visible = (isAdmin OrElse Servizio.GestioneDomande) AndAlso qs_questIdType = QuestionnaireType.QuestionLibrary
            If HYPviewAllQuestions.Visible Then
                HYPviewAllQuestions.NavigateUrl = PageUtility.BaseUrl & RootObject.LibraryViewAllQuestions(qs_questId, qs_LanguageId, qs_questIdType)
            End If
            LNBSalva.Visible = (isAdmin)
            PNLQuestionario.Enabled = (isAdmin)
            PNLDestinatari.Enabled = (isAdmin)
            PNLTipoGrafico.Enabled = (isAdmin)
            PNLCopiaQuestionario.Enabled = (isAdmin Or MyBase.Servizio.CopiaQuestionario)
            BTNSalvaAltraLingua.Enabled = (isAdmin)
            'solo il sysadmin può creare modelli pubblici, e li puo' creare solo prima di accedere a una comunita'
            'non esiste una comunita', quindi non ha senso "compilabile dagli utenti della comunita'"
            If Me.ComunitaCorrenteID = 0 And isAdmin Then
                DIVutentiNonComunita.Style("display") = "block"
                DIVutentiComunita.Style("display") = "none"
                If Me.QuestionarioCorrente.id = 0 Then
                    'viene impostato solo se il questionario e' appena stato creato
                    CHKUtentiNonComunita.Checked = True
                End If
            Else
                DIVutentiNonComunita.Style("display") = "none"
                DIVutentiComunita.Style("display") = "block"
                CHKUtentiNonComunita.Checked = False
            End If
            DirectCast(FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario"), Comunita_OnLine.UC_Editor).IsEnabled = (isAdmin)
        End If

    End Sub
    Public Overrides Sub BindNoPermessi()
        LBerrore.Visible = True
        LBerrore.Text = LBerrore.Text & Me.Resource.getValue("MSGNoPermessi")
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        If qs_ownerTypeId = OwnerType_enum.None Then
            isAdmin = MyBase.Servizio.Admin
            If Not qs_questId < 1 Then
                'se il questionario con id in querystring non e' della comunita' corrente ritorna false
                Dim oQuest As New Questionario
                oQuest = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, qs_questId, LinguaID, False)
                'se il quest e' della com corrente...
                If DALQuestionarioGruppo.ComunitaByGruppo(oQuest.idGruppo) = ComunitaCorrenteID Then
                    '..e ho i permessi necessari nella comunita'
                    If (isAdmin OrElse MyBase.Servizio.GestioneDomande OrElse MyBase.Servizio.CancellaQuestionario OrElse MyBase.Servizio.CopiaQuestionario OrElse MyBase.Servizio.QuestionariSuInvito) Then
                        'metto il quest in sessione e vado avanti
                        Me.QuestionarioCorrente = oQuest
                        Return True
                    End If
                    'altrimenti nego i permessi
                    Return False
                Else
                    'altrimenti nego i permessi
                    Return False
                End If
            Else
                'se non ci sono id in querystring ritrono i permessi di comunita' (e' nuovo o gia' caricato)
                Return isAdmin OrElse MyBase.Servizio.GestioneDomande OrElse MyBase.Servizio.CancellaQuestionario OrElse MyBase.Servizio.CopiaQuestionario OrElse MyBase.Servizio.QuestionariSuInvito
            End If
        Else
            Dim oSourceObject As New lm.Comol.Core.DomainModel.ModuleObject()
            If IsNumeric(qs_questId) AndAlso qs_questId = 0 Then
                oSourceObject.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity
            Else
                oSourceObject.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity
            End If
            'If qs_ownerTypeId = OwnerType_enum.EduPathSubActivity Then
            'oSourceObject.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity
            'ElseIf qs_ownerTypeId = OwnerType_enum.EduPathActivity Then
            'oSourceObject.ObjectTypeID = COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity
            'End If
            oSourceObject.ObjectLongID = qs_ownerId
            oSourceObject.ServiceCode = COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex
            oSourceObject.CommunityID = ComunitaCorrenteID

            Dim allow As Boolean = False
            If qs_questId < 1 Then 'i permessi per creazione ed edit sono diversi
                If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Create, UtenteCorrente.ID, ComunitaCorrenteID, oSourceObject, Nothing) Then
                    isAdmin = True
                    Me.QuestionarioCorrente = New Questionario 'altrimenti non si possono creare nuovi quest
                    Return True
                End If
            Else
                Dim oDestinationObject As New lm.Comol.Core.DomainModel.ModuleObject()
                oDestinationObject = lm.Comol.Core.DomainModel.ModuleObject.CreateLongObject(qs_questId, COL_BusinessLogic_v2.UCServices.Services_Questionario.ObjectType.Questionario, ComunitaCorrenteID, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)

                If allowStandardAction(lm.Comol.Core.DomainModel.StandardActionType.Edit, UtenteCorrente.ID, ComunitaCorrenteID, oSourceObject, oDestinationObject) Then
                    isAdmin = True
                    QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, qs_questId, LinguaID, False)
                    Return True
                End If
            End If
            'If allow Then
            '    isAdmin = True
            '    If qs_questId < 1 Then

            '    Else
            '        Dim oQuest As New Questionario
            '        oQuest = DALQuestionario.readQuestionarioBYLingua(qs_questId, LinguaID, False)

            '        'If oQuest.ownerType = qs_ownerTypeId AndAlso oQuest.ownerId = qs_ownerId Then
            '        Me.QuestionarioCorrente = oQuest
            '        'Else : Return False
            '        'End If
            '    End If
            '    Return True
            'End If
        End If
        Return False
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        Select Case Me.qs_questIdType
            Case COL_Questionario.Questionario.TipoQuestionario.Sondaggio
                MyBase.SetCulture("pg_SondaggioAdmin", "Questionari")
            Case COL_Questionario.Questionario.TipoQuestionario.Modello
                MyBase.SetCulture("pg_ModelloAdmin", "Questionari")
            Case Questionario.TipoQuestionario.LibreriaDiDomande
                MyBase.SetCulture("pg_LibreriaAdmin", "Questionari")
            Case Else
                MyBase.SetCulture("pg_QuestionarioAdmin", "Questionari")
        End Select
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            If qs_questId > 0 Then
                Master.ServiceTitle = .getValue("ServiceTitle.Edit." & qs_questIdType.ToString)
            Else
                Master.ServiceTitle = .getValue("ServiceTitle." & qs_questIdType.ToString)
            End If

            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBImporta, False, False, False, True)
            .setLinkButton(LNBGestioneDomande, False, False)
            If qs_questIdType = Questionario.TipoQuestionario.Autovalutazione OrElse qs_questIdType = Questionario.TipoQuestionario.RandomRepeat OrElse qs_questIdType = Questionario.TipoQuestionario.Random Then
                LNBGestioneDomande.Text = Resource.getValue("LibrarySelect.Text")
                LNBGestioneDomande.ToolTip = Resource.getValue("LibrarySelect.ToolTip")
            End If
            HYPviewAllQuestions.Text = Resource.getValue("LibraryViewAllQuestions.Text")
            HYPviewAllQuestions.ToolTip = Resource.getValue("LibraryViewAllQuestions.ToolTip")
            .setLinkButton(LNBSalva, False, False)
            .setLinkButton(LNBTornaGestioneQuestionari, False, False)
            .setLinkButton(LNBUndo, False, False)

            .setLabel(LBDestinatari)
            .setLabel(LBDescrizione)
            .setLabel(LBGestioneAvanzata)
            .setLabel(LBGruppo)
            .setLabel(LBAltraCartella)
            .setLabel(LBAltraComunita)
            .setLabel(LBNuovaLingua)
            .setLabel(LBinserireNome)
            .setLabel(LBerrore)
            .setLabel(LBCopiaComunita)
            .setLabel(LBCopiaSottocartella)
            .setLabel(LBTipoGrafico)
            .setLabel(LBTitoloUrl)

            .setCheckBox(CHKUtentiComunita)
            .setCheckBox(CHKUtentiNonComunita)
            .setCheckBox(CHKUtentiInvitati)
            .setCheckBox(CHKUtentiEsterni)

            .setButton(BTNInvitaUtenti, False, False, False, False)
            .setButton(BTNCopiaCartella, False, False, False, False)
            .setButton(BTNComunitaGruppo, False, False, False, False)
            .setButton(BTNCopiaComunita, False, False, False, False)
            .setButton(BTNSalvaAltraLingua, False, False, False, False)
        End With
        'LNBElimina.OnClientClick = Me.Resource.getValue("LNBElimina.OnClientClick")
    End Sub
    Private Sub FRVQuestionario_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVQuestionario.DataBound
        Dim oImg As ImageButton
        Dim oQuest As Questionario = Nothing
        Try
            If Not IsNothing(FRVQuestionario.DataItem) Then
                oQuest = DirectCast(FRVQuestionario.DataItem, Questionario)
            End If
        Catch ex As Exception

        End Try

        oImg = Me.FRVQuestionario.FindControl("IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionario, "target", "yes", "yes"))

        With Me.Resource
            Select Case Me.qs_questIdType
                Case Questionario.TipoQuestionario.Autovalutazione
                    DirectCast(FRVQuestionario.FindControl("LBDatiQuestionario"), Label).Text = .getValue("LBDatiQuestionarioAutovalutazione")
                Case Questionario.TipoQuestionario.Questionario
                    DirectCast(FRVQuestionario.FindControl("LBDatiQuestionario"), Label).Text = .getValue("LBDatiQuestionarioStatico")
                Case Questionario.TipoQuestionario.Random
                    DirectCast(FRVQuestionario.FindControl("LBDatiQuestionario"), Label).Text = .getValue("LBDatiQuestionarioRandom")
                Case Else
                    .setLabel(FRVQuestionario.FindControl("LBDatiQuestionario"))
            End Select
            .setLabel(FRVQuestionario.FindControl("LBNome"))
            .setCustomValidator(FRVQuestionario.FindControl("CUVNome"))
            .setRequiredFieldValidator(FRVQuestionario.FindControl("RFVNome"), True, False)
            .setLabel(FRVQuestionario.FindControl("LBLingua"))
            .setLabel(FRVQuestionario.FindControl("LBdescrizioneQuestionario"))
            .setLabel(FRVQuestionario.FindControl("LBTitoloDataCreazione"))
            .setLabel(FRVQuestionario.FindControl("LBDataInizioTitolo"))
            .setLabel(FRVQuestionario.FindControl("LBDataFineTitolo"))
            .setCustomValidator(FRVQuestionario.FindControl("CUVdate"))
            .setLabel(FRVQuestionario.FindControl("LBisBloccato"))
            .setLabel(FRVQuestionario.FindControl("LBisChiuso"))
            .setLabel(FRVQuestionario.FindControl("LBModalita"))
            .setLabel(FRVQuestionario.FindControl("LBDurata"))
            .setLabel(FRVQuestionario.FindControl("LBScalaValutazione"))
            '.setLabel(FRVQuestionario.FindControl("LBRisultatiAnonimi"))
            .setLabel(FRVQuestionario.FindControl("LBTitoloUrl"))
            .setLabel(FRVQuestionario.FindControl("LBLinguaDefault"))
            .setLabel(FRVQuestionario.FindControl("LBpermessiCompilatore"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaRisposta"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaCorrezione"))
            .setLabel(FRVQuestionario.FindControl("LBvisualizzaSuggerimenti"))
            .setLabel(FRVQuestionario.FindControl("LBeditaRisposta"))
            .setLabel(FRVQuestionario.FindControl("LBAiuto"))
            .setCompareValidator(FRVQuestionario.FindControl("COVDurataInt"))
            .setCompareValidator(FRVQuestionario.FindControl("COVScalaValutazioneInt"))
            .setLabel(FRVQuestionario.FindControl("LBOrdineDomandeRandom"))
            .setLabel(FRVQuestionario.FindControl("LBOrdineDomandeRandom_t"))
            .setLabel(FRVQuestionario.FindControl("LBanonymousResults_t"))
            .setLabel(FRVQuestionario.FindControl("LBanonymousResults"))

            .setLabel(FRVQuestionario.FindControl("LBminScore_t"))
            .setCompareValidator(FRVQuestionario.FindControl("CMVminScore"))
            .setLabel(FRVQuestionario.FindControl("LBmaxAttempts_t"))
            .setCompareValidator(FRVQuestionario.FindControl("CMVmaxAttempts"))

            .setLabel(FRVQuestionario.FindControl("LBdisplayScoreToUser"))
            .setLabel(FRVQuestionario.FindControl("LBdisplayScoreToUser_t"))
            .setLabel(FRVQuestionario.FindControl("LBdescriptionScoreToUser"))
  
            .setLabel(FRVQuestionario.FindControl("LBdisplayAttemptScoreToUser"))
            .setLabel(FRVQuestionario.FindControl("LBdisplayAttemptScoreToUser_t"))
            .setLabel(FRVQuestionario.FindControl("LBdescriptionAttemptScore"))

            .setLabel(FRVQuestionario.FindControl("LBdisplayAvailableAttemptsToUser_t"))
            .setLabel(FRVQuestionario.FindControl("LBdisplayAvailableAttemptsToUser"))
            .setLabel(FRVQuestionario.FindControl("LBdescriptionAvailableAttempts"))

            .setLabel(FRVQuestionario.FindControl("LBdisplayCurrentAttemptToUser_t"))
            .setLabel(FRVQuestionario.FindControl("LBdisplayCurrentAttemptToUser"))
            .setLabel(FRVQuestionario.FindControl("LBdescriptionCurrentAttempt"))


            .setLiteral(FRVQuestionario.FindControl("LTresultsSettingsLegend"))
        End With
        Dim CHKvisualizzaCorrezione As CheckBox
        Dim CHKvisualizzaRisposta As CheckBox
        Dim CHKeditaRisposta As CheckBox
        Dim CHKvisualizzaSuggerimenti As CheckBox

        Dim CBXdisplayScoreToUser As CheckBox
        Dim CBXdisplayAttemptScoreToUser As CheckBox
        Dim oCheckbox As CheckBox = FRVQuestionario.FindControl("CBXanonymousResults")
        oCheckbox.Enabled = (Me.QuestionarioCorrente.id = 0) OrElse (CurrentService.CanChangeAnonymousStatus(Me.QuestionarioCorrente.id) AndAlso Me.QuestionarioCorrente.tipo <> QuestionnaireType.Random AndAlso Me.QuestionarioCorrente.tipo <> QuestionnaireType.RandomMultipleAttempts)

        Dim extModuleSkin As Comunita_OnLine.UC_ModuleSkins = FRVQuestionario.FindControl("CTRLmoduleSkin")


        Dim editor As Comunita_OnLine.UC_Editor = FRVQuestionario.FindControl("CTRLeditorDescrizioneQuestionario")
        If Not editor.isInitialized Then
            editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
        End If

        If Not IsNothing(oQuest) AndAlso Not String.IsNullOrEmpty(oQuest.descrizione) Then
            editor.HTML = oQuest.descrizione
        Else
            editor.HTML = ""
        End If

        Dim isInternalItem As Boolean = (Me.qs_ownerId <> 0)
        Dim idCommunity As Integer = CurrentContext.UserContext.WorkingCommunityID

        If Not isInternalItem AndAlso Not extModuleSkin.isInitialized AndAlso ((QuestionarioCorrente.forUtentiEsterni OrElse QuestionarioCorrente.forUtentiInvitati OrElse extModuleSkin.ObjectHasSkinAssociation(CurrentService.ServiceModuleID, idCommunity, Me.QuestionarioCorrente.id, ModuleQuestionnaire.ObjectType.Questionario, GetType(Questionario).FullName))) Then
            extModuleSkin.Visible = True
            Dim mQuest As ModuleQuestionnaire = CurrentService.GetCommunityPermission(Me.QuestionarioCorrente.id, idCommunity)
            Dim loadModuleSkinBy As lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy

            If mQuest.Administration OrElse mQuest.ManageInvitedQuestionnaire Then
                loadModuleSkinBy = lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Module Or lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object
            Else
                loadModuleSkinBy = lm.Comol.Modules.Standard.Skin.Domain.LoadItemsBy.Object
            End If
            extModuleSkin.InitializeControl(CurrentService.ServiceModuleID, idCommunity, Me.QuestionarioCorrente.id, ModuleQuestionnaire.ObjectType.Questionario, GetType(Questionario).FullName, mQuest.Administration OrElse mQuest.ManageInvitedQuestionnaire, mQuest.Administration OrElse mQuest.ManageInvitedQuestionnaire, loadModuleSkinBy)
        Else
            extModuleSkin.Visible = False
        End If


        CHKvisualizzaCorrezione = FRVQuestionario.FindControl("CHKvisualizzaCorrezione")
        CHKvisualizzaRisposta = FRVQuestionario.FindControl("CHKvisualizzaRisposta")
        CHKeditaRisposta = FRVQuestionario.FindControl("CHKeditaRisposta")
        CHKvisualizzaSuggerimenti = FRVQuestionario.FindControl("CHKvisualizzaSuggerimenti")
        CBXdisplayScoreToUser = FRVQuestionario.FindControl("CBXdisplayScoreToUser")
        CBXdisplayAttemptScoreToUser = FRVQuestionario.FindControl("CBXdisplayAttemptScoreToUser")

        CHKvisualizzaRisposta.Attributes.Add("onclick", "JSvisualizzaRisposta(this,'" & CHKvisualizzaCorrezione.ClientID & "','" & CHKeditaRisposta.ClientID & "','" & CHKvisualizzaSuggerimenti.ClientID & "'); return true;")
        CHKvisualizzaCorrezione.Attributes.Add("onclick", "JSvisualizzaCorrezione(this,'" & CHKvisualizzaRisposta.ClientID & "','" & CHKeditaRisposta.ClientID & "','" & CHKvisualizzaSuggerimenti.ClientID & "'); return true;")
        CHKvisualizzaSuggerimenti.Attributes.Add("onclick", "JSvisualizzaSuggerimenti(this,'" & CHKvisualizzaRisposta.ClientID & "','" & CHKvisualizzaCorrezione.ClientID & "','" & CHKeditaRisposta.ClientID & "'); return true;")
        CHKeditaRisposta.Attributes.Add("onclick", "JSeditaRisposta(this,'" & CHKvisualizzaSuggerimenti.ClientID & "','" & CHKvisualizzaCorrezione.ClientID & "','" & CHKvisualizzaRisposta.ClientID & "'); return true;")

        If Not oQuest.DisplayResultsStatus Then
            CBXdisplayScoreToUser.Enabled = False
            CBXdisplayAttemptScoreToUser.Enabled = False
        End If

        CHKUtentiInvitati.Visible = Me.questionariSuInvito
        BTNInvitaUtenti.Visible = Me.questionariSuInvito
        If Me.QuestionarioCorrente.id > 0 AndAlso Me.questionariSuInvito AndAlso (Me.QuestionarioCorrente.tipo = QuestionnaireType.Standard OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.Random OrElse Me.QuestionarioCorrente.tipo = QuestionnaireType.RandomMultipleAttempts) Then
            Me.LBinvitedUrl.Visible = True
            Me.LBinvitedUrl.Text = String.Format(Resource.getValue("InvitedUrl"), ApplicationUrlBase & RootObject.GenerateQuestionnaireUrl(Me.QuestionarioCorrente.id))
        End If
    End Sub
    Private Sub LNBTornaGestioneQuestionari_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBTornaGestioneQuestionari.Click
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Private Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("isZero") = "true" And Me.ComunitaCorrenteID > 0 Then
            GoToPortale()
        End If
        'o cosi', o in quelle due pagine si gestisce il fatto che le risposte vengono cancellate quando si fa update delle opzioni
        'Me.QuestionarioCorrente.id = 0 serve solo per escludere le risp. orfane, che bloccherebbero il sistema per tutti i nuovi
        If DALRisposte.countRisposteBYIDQuestionario(Me.QuestionarioCorrente.id) = 0 OrElse Me.QuestionarioCorrente.id = 0 Then
            Select Case qs_questIdType
                Case Questionario.TipoQuestionario.Sondaggio
                    Me.RedirectToUrl(RootObject.SondaggioAdmin)
                Case Questionario.TipoQuestionario.Meeting
                    Me.RedirectToUrl(RootObject.MeetingWiz)
            End Select
        End If
    End Sub

    Private Sub LNBUndo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBUndo.Click
        goToOwner()
    End Sub
    Private Sub LNBTornaLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBTornaLista.Click
        LNBCartellaPrincipale_Click(sender, e)
    End Sub

    Private Function SaveSkinSelection() As Boolean
        Dim extModuleSkin As Comunita_OnLine.UC_ModuleSkins = FRVQuestionario.FindControl("CTRLmoduleSkin")
        Dim isInternalItem As Boolean = (Me.qs_ownerId <> 0)
        If Not isInternalItem AndAlso Not IsNothing(extModuleSkin) AndAlso extModuleSkin.isInitialized Then
            Return extModuleSkin.SaveSkinAssociation()
        Else
            Return True
        End If
    End Function
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            If Me.qs_questId > 0 AndAlso Me.qs_questId <> Me.QuestionarioCorrente.id Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

#Region "Library Settings"
    Private Sub InitializeLibrarySettings()
        Me.PNLlibrarySettings.Visible = True
        Resource.setLabel(LBlibraryVisibility_t)
        Resource.setLabel(LBusers_t)
        Resource.setButton(BTNaddUser, True)
        Resource.setRadioButtonList(RBLlibraryVisibility, LibraryAccessibility.currentCommunity.ToString)
        Resource.setRadioButtonList(RBLlibraryVisibility, LibraryAccessibility.currentAndChildren.ToString)
        Resource.setRadioButtonList(RBLlibraryVisibility, LibraryAccessibility.someUser.ToString)
        Resource.setButton(BTNunsavePerson, True)
        Resource.setButton(BTNsavePerson, True)

        Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)

        Dim q As LazyQuestionnaire = s.GetItem(Of LazyQuestionnaire)(QuestionarioCorrente.id)
        If Not IsNothing(q) Then
            Dim forPortal As Boolean = True
            Dim groupOwner As LazyGroup = s.GetItem(Of LazyGroup)(q.IdGroup)
            forPortal = IsNothing(groupOwner) OrElse (Not IsNothing(groupOwner) AndAlso groupOwner.IdCommunity = 0)
            If forPortal Then
                Me.RBLlibraryVisibility.Items.Remove(Me.RBLlibraryVisibility.Items.FindByValue(LibraryAccessibility.currentAndChildren.ToString))
            End If

            If Me.RBLlibraryVisibility.Items.FindByValue(q.LibraryAccessibility.ToString) Is Nothing Then
                q.LibraryAccessibility = LibraryAccessibility.currentCommunity
            End If
            Me.RBLlibraryVisibility.SelectedValue = q.LibraryAccessibility.ToString

            Dim assignments As List(Of dtoQuestionnaireAssignment) = s.GetLibraryAssignments(q.Id, forPortal)
            Select Case q.LibraryAccessibility
                Case LibraryAccessibility.someUser
                    Me.DVusers.Visible = True
                    Me.DVprofileType.Visible = False
                    LoadPersonAssignments(assignments.Where(Function(a) a.Type = AssignmentType.person).Select(Function(a) DirectCast(a, dtoQuestionnairePersonAssignment)).ToList)
                    Me.MaintainScrollPositionOnPostBack = (Me.RBLlibraryVisibility.SelectedValue <> LibraryAccessibility.someUser.ToString)
                Case LibraryAccessibility.someProfileType
                    Me.DVusers.Visible = False
                    Me.DVprofileType.Visible = True
                    LoadPersonTypeAssignments(assignments.Where(Function(a) a.Type = AssignmentType.personType).Select(Function(a) DirectCast(a, dtoQuestionnairePersonTypeAssignment)).ToList)
            End Select
        End If
    End Sub

    Private Sub LoadPersonAssignments(items As List(Of dtoQuestionnairePersonAssignment))
        AvailableUsers = items.Select(Function(i) i.AssignedTo).Distinct.Select(Function(p) New KeyValuePair(Of Integer, String)(p.Id, p.SurnameAndName)).ToList()
        Me.CBLusers.DataSource = items.Select(Function(i) i.AssignedTo).Distinct.OrderBy(Function(p) p.SurnameAndName).ToList
        Me.CBLusers.DataValueField = "ID"
        Me.CBLusers.DataTextField = "SurnameAndName"
        Me.CBLusers.DataBind()
        For Each item As ListItem In Me.CBLusers.Items
            item.Selected = True
        Next
        Me.DVusers.Visible = True
    End Sub

    Private Sub LoadPersonTypeAssignments(items As List(Of dtoQuestionnairePersonTypeAssignment))
        Dim idProfileTypes As List(Of Integer) = items.Select(Function(i) i.AssignedTo).ToList()

        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Select o).ToList
        Me.CBLprofileType.DataSource = oUserTypes
        Me.CBLprofileType.DataValueField = "ID"
        Me.CBLprofileType.DataTextField = "Descrizione"
        Me.CBLprofileType.DataBind()

        For Each item As ListItem In Me.CBLprofileType.Items
            item.Selected = idProfileTypes.Contains(item.Value)
        Next
        Me.DVprofileType.Visible = True
    End Sub

    Private Sub RBLlibraryVisibility_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLlibraryVisibility.SelectedIndexChanged
        Me.DVusers.Visible = (Me.RBLlibraryVisibility.SelectedValue = LibraryAccessibility.someUser.ToString)
        Me.MaintainScrollPositionOnPostBack = (Me.RBLlibraryVisibility.SelectedValue <> LibraryAccessibility.someUser.ToString)
        If Me.RBLlibraryVisibility.SelectedValue = LibraryAccessibility.someUser.ToString AndAlso AvailableUsers.Count = 0 AndAlso Not IsNothing(UtenteCorrente) Then
            Dim users As List(Of KeyValuePair(Of Integer, String)) = AvailableUsers

            users.Add(New KeyValuePair(Of Integer, String)(Me.UtenteCorrente.ID, UtenteCorrente.Cognome & " " & UtenteCorrente.Nome))
            AvailableUsers = users
            Me.CBLusers.DataSource = users
            Me.CBLusers.DataValueField = "Key"
            Me.CBLusers.DataTextField = "Value"
            Me.CBLusers.DataBind()
            Me.CBLusers.SelectedIndex = 0
        End If
    End Sub
    Private Sub BTNaddUser_Click(sender As Object, e As System.EventArgs) Handles BTNaddUser.Click
        Dim oCommunities As New List(Of Integer)

        oCommunities.Add(Me.ComunitaCorrenteID)
        Me.CTRLuserList.CurrentPresenter.Init(oCommunities, ListSelectionMode.Multiple, AllUsers())
        Me.SetFocus(Me.CTRLuserList.GetSearchButtonControl)
        Me.UDPperson.Update()

        Me.LTscriptOpen.Visible = True
    End Sub

    Private Sub BTNsavePerson_Click(sender As Object, e As System.EventArgs) Handles BTNsavePerson.Click
        Dim selItems As List(Of Int32) = SelectedUsers()
        Dim items As List(Of MemberContact) = Me.CTRLuserList.CurrentPresenter.GetConfirmedUsers()
        Dim users As List(Of KeyValuePair(Of Integer, String)) = AvailableUsers
        For Each member As MemberContact In items
            Dim idPerson As Integer = member.Id
            If Not users.Where(Function(u) u.Key = idPerson).Any() Then
                users.Add(New KeyValuePair(Of Integer, String)(member.Id, member.FullName))
            End If
        Next
        AvailableUsers = users
        Me.CBLusers.DataSource = users
        Me.CBLusers.DataValueField = "Key"
        Me.CBLusers.DataTextField = "Value"
        Me.CBLusers.DataBind()
        For Each item As ListItem In Me.CBLusers.Items
            item.Selected = selItems.Contains(item.Value) OrElse items.Where(Function(i) i.Id = CInt(item.Value)).Any()
        Next
        Me.UDPperson.Update()
        Me.LTscriptOpen.Visible = False
    End Sub

    Private Sub BTNunsavePerson_Click(sender As Object, e As System.EventArgs) Handles BTNunsavePerson.Click
        Me.LTscriptOpen.Visible = False
    End Sub
    Private Function AllUsers() As List(Of Integer)
        Dim users As New List(Of Int32)
        If (DVusers.Visible) Then
            users = (From item As ListItem In CBLusers.Items Select CInt(item.Value)).ToList
        End If
        Return users
    End Function
    Private Property AvailableUsers As List(Of KeyValuePair(Of Integer, String))
        Get
            Return ViewStateOrDefault("AvailableUsers", New List(Of KeyValuePair(Of Integer, String)))
        End Get
        Set(value As List(Of KeyValuePair(Of Integer, String)))
            ViewState("AvailableUsers") = value
        End Set
    End Property
    Private Function SelectedUsers() As List(Of Integer)
        Dim users As New List(Of Int32)
        If (DVusers.Visible) Then
            users = (From item As ListItem In CBLusers.Items Where item.Selected Select CInt(item.Value)).ToList
        End If
        Return users
    End Function

    Private Sub SaveLibrarySettings(ByVal idQuestionnaire As Integer)
        Dim users As List(Of Integer) = SelectedUsers()
        Dim roles As New List(Of Integer) ' View.SelectedRoles();
        Dim profileTypes As New List(Of Integer) 'View.SelectedProfileTypes();

        Dim s As New COL_Questionario.Business.ServiceQuestionnaire(PageUtility.CurrentContext)
        s.SaveLibraryAvailability(idQuestionnaire, lm.Comol.Core.DomainModel.Helpers.EnumParser(Of LibraryAccessibility).GetByString(Me.RBLlibraryVisibility.SelectedValue, LibraryAccessibility.currentCommunity), users, roles, profileTypes)


    End Sub
#End Region

  
End Class