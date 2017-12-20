Imports COL_Questionario

Partial Public Class GruppiList
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

    ' indica il tipo di lista: 0=questionari, 2=sondaggi, 3=modelli
    Private Property groupType() As String
        Get
            Return Request.QueryString("type")
        End Get
        Set(ByVal value As String)
            ViewState("type") = Request.QueryString("type")
        End Set
    End Property

    Private Property listaGruppi() As list(Of QuestionarioGruppo)
        Get
            Return ViewState("listaGruppi")
        End Get
        Set(ByVal value As list(Of QuestionarioGruppo))
            ViewState("listaGruppi") = value
        End Set
    End Property

    Private Enum TipoLista
        Gestione = 0
        Statistiche = 1
    End Enum


    Dim oQuest As New COL_Questionario.Questionario
    Dim oGestioneDomande As New GestioneDomande

    'Eseguito SOLO al primo accesso alla pagina
    Public Overrides Sub BindDati()
        'carico il gruppo di default

        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        Select Case Me.TBSGruppi.SelectedTab.Value
            Case 0

                listaGruppi = DALQuestionarioGruppo.readGruppi(Me.ComunitaCorrenteID)

                Bind()

        End Select

    End Sub


    Private Sub Bind()

        Dim oImg As ImageButton

        oImg = Me.IMBHelp

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpQuestionariGestioneList, "target", "yes", "yes"))

        BindGRVElenco()

    End Sub

    Private Sub BindGRVElenco()
        GRVElenco.DataSource = listaGruppi
        GRVElenco.DataBind()

        If GRVElenco.Rows.Count > 0 Then
            Me.MLVquestionari.SetActiveView(Me.VIWdati)
        Else
            LBerrore.Text = Me.Resource.getValue("MSGnoQuestionari")
            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
        End If

    End Sub

    Protected Sub GRVElenco_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GRVElenco.RowCommand

        If CType(e.CommandSource, Control).NamingContainer.GetType() Is GetType(GridViewRow) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Me.GruppoCorrente = New QuestionarioGruppo
            Me.GruppoCorrente.id = GRVElenco.DataKeys(row.RowIndex).Value
            Me.GruppoCorrente.nome = DirectCast(GRVElenco.Rows(row.RowIndex).FindControl("LBLNomeQuestionario"), Label).Text

            Select Case e.CommandName
                Case "Gestione"
                    Me.RedirectToUrl(RootObject.GruppoAdmin + "?type=" + Me.groupType)
                Case "Argomenti"
                    Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=1")
                Case "Elimina"
                    'If Me.QuestionarioCorrente.linguePresenti.Count = 1 Then
                    '    DALQuestionario.DeleteQuestionarioBYPadre(oQuest)
                    '    Me.RedirectToUrl(RootObject.ModelliGestioneList + "&type=" + Me.groupType)
                    'Else
                    '    Me.MLVquestionari.SetActiveView(VIWCancellaQuestionario)
                    '    LBLNomeQuestionario.Text += Me.GruppoCorrente.questionari(indiceQuestionario).nome
                    '    DLLingue.DataSource = Me.QuestionarioCorrente.linguePresenti
                    '    DLLingue.DataBind()
                    'End If
            End Select
        End If


    End Sub

    Protected Sub GRVElenco_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GRVElenco.RowDataBound

        If e.Row.RowIndex >= 0 Then

            If TBSGruppi.SelectedTab.Value = 0 Then ' TAB MODELLI PUBBLICI
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

            setInternazionalizzaioneGRVElencoControls(e)

        End If

    End Sub

    Protected Sub LNBNuovoQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovoQuestionario.Click
        Me.GruppoCorrente = New QuestionarioGruppo
        Me.GruppoCorrente.id = 0
        Me.RedirectToUrl(RootObject.GruppoAdmin + "?type=" + Me.groupType)

    End Sub

    Protected Sub LNBNuovaCartella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNuovaCartella.Click
        Me.RedirectToUrl(RootObject.GruppoAdmin)
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
        MyBase.SetCulture("pg_GruppiList", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBNuovoQuestionario, False, False)
            .setLinkButton(LNBNuovaCartella, False, False)
            .setLinkButton(LNBCestino, False, False)
            .setLabel(LBerrore)
            .setLabel(LBLCartella)
            .setHeaderGridView(Me.GRVElenco, 0, "headerNome", True)
            .setHeaderGridView(Me.GRVElenco, 1, "headerModifica", True)
            .setHeaderGridView(Me.GRVElenco, 2, "headerElimina", True)
            .setImageButton(IMBHelp, False, False, True, False)
            .setLabel(LBHelp)
            .setLabel(LBLNomeQuestionario)
            .setLabel(LBLTestoDescrizione)
            .setButton(BTNEliminaLingua, False, False, True, False)

            For Each item As Telerik.Web.UI.RadTab In TBSGruppi.Tabs
                item.Text = .getValue(TBSGruppi, item.Value)
                item.ToolTip = item.Text
            Next

            SetServiceTitle(Master)
        End With
    End Sub

    Protected Sub setInternazionalizzaioneGRVElencoControls(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        With Me.Resource
            .setImageButton(e.Row.FindControl("IMBAnteprima"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBGestione"), False, True, True, False)
            .setImageButton(e.Row.FindControl("IMBElimina"), False, True, True, True)
            .setImageButton(e.Row.FindControl("IMBImporta"), False, True, True, True)
            .setLinkButton(e.Row.FindControl("LNKArgomenti"), False, False, False, False)
        End With

    End Sub

    Protected Sub GRVElenco_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GRVElenco.PageIndexChanging
        GRVElenco.PageIndex = e.NewPageIndex
        GRVElenco.DataSource = listaGruppi
        GRVElenco.DataBind()
    End Sub

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Protected Sub LNBCestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCestino.Click
        Me.RedirectToUrl(RootObject.QuestionariCestinoList + "?type=" + Me.groupType)
    End Sub

    Public Overrides Sub SetControlliByPermessi()

        LNBCestino.Visible = MyBase.Servizio.Admin

        LNBNuovoQuestionario.Visible = MyBase.Servizio.Admin
        LNBCestino.Visible = MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario

        ' opzioni al momento disattive
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

    Protected Sub TBSModelli_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSGruppi.TabClick
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