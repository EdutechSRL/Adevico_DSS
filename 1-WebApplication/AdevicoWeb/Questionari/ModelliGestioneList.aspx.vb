Imports COL_Questionario

Partial Public Class ModelliGestioneList
    Inherits PageBaseQuestionario


    ' indica la modalità della pagina: 0=amministrazione, 1=statistiche
    Private Property pageMode() As String
        Get
            Return Request.QueryString("mode")
        End Get
        Set(ByVal value As String)
            ViewState("pageMode") = Request.QueryString("mode")
        End Set
    End Property


    Private Enum TipoLista
        Gestione = 0
        Statistiche = 1
    End Enum


    Dim oQuest As New COL_Questionario.Questionario
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneQuest As New GestioneQuestionario

    'Eseguito SOLO al primo accesso alla pagina
    Public Overrides Sub BindDati()
        'carico il gruppo di default

        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        If Me.GruppoCorrente Is Nothing Then
            Me.GruppoCorrente = New QuestionarioGruppo
        End If
        
        Select Case Me.TBSModelli.SelectedTab.Value
            Case 0
                Me.GruppoCorrente.id = -1 ' gruppo modelli pubblici
                Me.GruppoCorrente.questionari = DALQuestionario.readModelliPubblici()
                GRVElenco.PageSize = RootObject.nRighePaginaGridView
                GRVElenco.DataSource = Me.GruppoCorrente.questionari
                GRVElenco.DataBind()
            Case 1
                Me.GruppoCorrente = DALQuestionarioGruppo.GruppoPrincipaleByComunita(Me.ComunitaCorrenteID)
                If Me.GruppoCorrente.id = 0 Then
                    oGestioneQuest.creaGruppoDefault(Me.ComunitaCorrenteID)
                End If

                Me.GruppoCorrente = DALQuestionario.readQuestionariByGruppo(Me.GruppoCorrente.id, Me.qs_questIdType)

                Bind()

        End Select

    End Sub


    Private Sub Bind()

        If Not Me.pageMode Is Nothing Then

            Dim oImg As ImageButton

            oImg = Me.IMBHelp

            oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionariGestioneList, "target", "yes", "yes"))

            BindGRVElenco()


        End If


    End Sub

    Private Sub BindGRVElenco()
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
    End Sub

    Private Sub BindDLGruppi()
        DLGruppi.DataSource = DALQuestionarioGruppo.readSottoGruppi(Me.GruppoCorrente.id, Me.ComunitaCorrenteID)
        DLGruppi.DataBind()

        LBLCartella.Text = Me.GruppoCorrente.nome
    End Sub

    Protected Sub DLGruppiItemCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        Me.GruppoCorrente.id = DLGruppi.DataKeys.Item(e.Item.ItemIndex)

        If e.CommandName = "viewGruppo" Then
            Me.RedirectToUrl(RootObject.QuestionariGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
        End If

    End Sub

    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand

        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            oQuest.id = GRVElenco.DataKeys(row.RowIndex).Value
            oQuest.nome = DirectCast(GRVElenco.Rows(row.RowIndex).FindControl("LBLNomeQuestionario"), Label).Text
            oQuest.tipo = Me.qs_questIdType
            Me.QuestionarioCorrente = oQuest 'ok
            Me.LinguaQuestionario = 1
            Dim indiceQuestionario As Integer = row.RowIndex + (GRVElenco.PageIndex * RootObject.nRighePaginaGridView)
            oQuest.linguePresenti = Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti

            Select Case e.CommandName
                Case "Gestione"
                    Me.RedirectToUrl(RootObject.QuestionarioAdmin & "?" & qs_questType & CInt(Me.qs_questIdType) & "&IdQ=" & oQuest.id & "&idLanguage=1")
                Case "Anteprima"
                    Me.RedirectToUrl(RootObject.QuestionarioView + "?mode=0")
                Case "Domande"
                    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oQuest.id, 1, False)
                    Me.RedirectToUrl(RootObject.QuestionarioEdit & "?" & qs_questType & CInt(Me.qs_questIdType))
                Case "Elimina"
                    If Me.QuestionarioCorrente.linguePresenti.Count = 1 Then
                        DALQuestionario.DeleteQuestionarioBYPadre(oQuest)
                        Me.RedirectToUrl(RootObject.ModelliGestioneList & "&" & qs_questType & CInt(Me.qs_questIdType))
                    Else
                        Me.MLVquestionari.SetActiveView(VIWCancellaQuestionario)
                        LBLNomeQuestionario.Text += Me.GruppoCorrente.questionari(indiceQuestionario).nome
                        DLLingue.DataSource = Me.QuestionarioCorrente.linguePresenti
                        DLLingue.DataBind()
                    End If
                Case "Importa"
                    Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, oQuest.id, 1, False)
                    Me.QuestionarioCorrente = quest
                    Dim questNome As String
                    Dim copiePresenti As Integer = 1
                    'If DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia di " & Me.QuestionarioCorrente.nome) = 0 Then
                    If Not DALQuestionario.IsDuplicatedName(Me.ComunitaCorrenteID, quest.id, quest.tipo, "Copia di " & quest.nome) Then
                        questNome = "Copia di " & Me.QuestionarioCorrente.nome
                        Me.QuestionarioCorrente.nome = questNome
                    Else
                        Do
                            copiePresenti = copiePresenti + 1
                        Loop While Not DALQuestionario.IsDuplicatedName(Me.ComunitaCorrenteID, quest.id, quest.tipo, "Copia (" & copiePresenti & ") di " & quest.nome)
                        'Loop While Not DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome) = 0
                        Me.QuestionarioCorrente.nome = "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome
                        questNome = Me.QuestionarioCorrente.nome
                    End If
                    oGestioneQuest.copiaModello()
                    Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=0")
            End Select
        End If


    End Sub



    Protected Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound

        If e.Row.RowIndex >= 0 Then

            If TBSModelli.SelectedTab.Value = 0 Then ' TAB MODELLI PUBBLICI
                ' se è un amministratore può gestire i modelli pubblici altrimenti no
                If Me.TipoPersonaID = Main.TipoPersonaStandard.SysAdmin Then
                    DirectCast(e.Row.FindControl("IMBGestione"), System.Web.UI.WebControls.Image).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
                    DirectCast(e.Row.FindControl("LNKNomeQuestionario"), System.Web.UI.WebControls.LinkButton).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
                    DirectCast(e.Row.FindControl("LBLNomeQuestionario"), Label).Visible = Not (MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
                    GRVElenco.Columns(2).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario

                Else
                    DirectCast(e.Row.FindControl("IMBGestione"), System.Web.UI.WebControls.Image).Visible = False
                    DirectCast(e.Row.FindControl("LNKNomeQuestionario"), System.Web.UI.WebControls.LinkButton).Visible = False
                    DirectCast(e.Row.FindControl("LBLNomeQuestionario"), Label).Visible = True
                    GRVElenco.Columns(2).Visible = False

                End If
            Else
                DirectCast(e.Row.FindControl("IMBGestione"), System.Web.UI.WebControls.Image).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
                DirectCast(e.Row.FindControl("LNKNomeQuestionario"), System.Web.UI.WebControls.LinkButton).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito
                DirectCast(e.Row.FindControl("LBLNomeQuestionario"), Label).Visible = Not (MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
                GRVElenco.Columns(2).Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario
            End If

            Dim indiceQuestionario = e.Row.RowIndex + (GRVElenco.PageIndex * RootObject.nRighePaginaGridView)
            Dim lingue As New List(Of Lingua)
            If Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti.Count = 0 Then
                lingue = DALQuestionario.readLingueQuestionario(Me.GruppoCorrente.questionari(indiceQuestionario).id)
                Me.GruppoCorrente.questionari(indiceQuestionario).linguePresenti = lingue
            End If

            setInternazionalizzaioneGRVElencoControls(e)

        End If

    End Sub

    Protected Sub LNBNuovoQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovoQuestionario.Click
        'Session.Remove("idQuestionario")
        'Session.Remove("oQuest")
        Me.QuestionarioCorrente = New Questionario
        Me.RedirectToUrl(RootObject.QuestionarioAdmin & "?" & qs_questType & CInt(Me.qs_questIdType))

    End Sub

    Protected Sub LNBNuovaCartella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovaCartella.Click
        'Me.RedirectToUrl(RootObject.GruppoAdd)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'If Me.pageMode = TipoLista.Gestione Then
        '    Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande)
        'Else
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.VisualizzaStatistiche Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
        'End If
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_GestioneModelliList", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            Master.ServiceTitle = .getValue("ServiceTitle")
            .setLinkButton(LNBNuovoQuestionario, False, False)
            .setLinkButton(LNBNuovaCartella, False, False)
            .setLinkButton(LNBCestino, False, False)
            .setLabel(LBerrore)
            .setLabel(LBLCartella)
            .setLabel(LBTitoloSottoCartelle)
            .setHeaderGridView(Me.GRVElenco, 0, "headerEntra", True)
            .setHeaderGridView(Me.GRVElenco, 1, "headerNome", True)
            .setHeaderGridView(Me.GRVElenco, 2, "headerElimina", True)
            .setHeaderGridView(Me.GRVElenco, 3, "headerImporta", True)
            .setImageButton(IMBHelp, False, False, True, False)
            .setLabel(LBHelp)
            .setLabel(LBLNomeQuestionario)
            .setLabel(LBLTestoDescrizione)
            .setButton(BTNEliminaLingua, False, False, True, False)
            For Each item As Telerik.Web.UI.RadTab In TBSModelli.Tabs
                item.Text = .getValue(TBSModelli, item.Value)
                item.ToolTip = item.Text
            Next
        End With
    End Sub

    Protected Sub setInternazionalizzaioneGRVElencoControls(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImageButton(e.Row.FindControl("IMBAnteprima"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBGestione"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, True)
            .setImageButton(e.Row.FindControl("IMBImporta"), False, True, True, True)
        End With

    End Sub

    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        GRVElenco.DataSource = Me.GruppoCorrente.questionari
        GRVElenco.DataBind()
    End Sub

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Protected Sub LNBCestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCestino.Click
        Me.RedirectToUrl(RootObject.QuestionariCestinoList & "?" & qs_questType & CInt(Me.qs_questIdType))
    End Sub

    Public Overrides Sub SetControlliByPermessi()

        LNBCestino.Visible = MyBase.Servizio.Admin
        LNBNuovoQuestionario.Visible = False

        If Me.qs_questIdType = Questionario.TipoQuestionario.Modello Then
            LNBNuovoQuestionario.Visible = MyBase.Servizio.Admin
            LNBCestino.Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario
        End If

        ' opzioni al momento disattive
        PNLGruppi.Visible = False
        LNBNuovaCartella.Visible = False
        LBLCartella.Visible = False

        'BindDLGruppi()

    End Sub


    Protected Sub BTNEliminaLingua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNEliminaLingua.Click

        For Each it As DataListItem In DLLingue.Items
            If DirectCast(it.FindControl("CHKSelezionaLingua"), CheckBox).Checked Then
                Dim idLingua As Integer = DLLingue.DataKeys.Item(it.ItemIndex)
                Dim idQuest As Integer = Me.QuestionarioCorrente.id
                DALQuestionario.DeleteQuestionarioByIDLingua(idQuest, idLingua)
                Me.MLVquestionari.SetActiveView(Me.VIWdati)
                Bind()
            End If
        Next

    End Sub

    Protected Sub TBSModelli_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSModelli.TabClick
        BindDati()
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class