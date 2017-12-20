Imports COL_Questionario

Partial Public Class ucVisualizzaRisposta
    Inherits BaseControlQuestionario

    Public Shared iDom As Integer
    Dim oGestioneDomande As New GestioneDomande
    Dim oPagedDataSource As New PagedDataSource
    Dim oGestioneRisposte As New GestioneRisposte
    'Dim oQuest As New Questionario

    Private Property iPag() As Integer
        Get
            Return ViewState("idPag")
        End Get
        Set(ByVal value As Integer)
            ViewState("idPag") = value
        End Set
    End Property
    Protected Sub bindDataList()
        'LBNomeUtente.Visible = True
        'DLPagine.DataBind()
        'If Me.QuestionarioCorrente.visualizzaCorrezione Then
        '    DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
        'Else
        '    DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)
        'End If
    End Sub
    'Protected Sub vediQuestionario(ByVal idRisposta As Integer, ByVal idUI As Integer, ByVal idP As Integer)

    '    BindDati()
    '    Dim oRis As New RispostaQuestionario

    '    Dim oPersona As New UtenteInvitato
    '    ''Dim idU As String = GRVUtenti.SelectedDataKey.Item("ID")
    '    ''Dim idP As String = GRVUtenti.SelectedDataKey.Item("PersonaID")
    '    'If idUI > 0 Then
    '    '    oPersona = oRis.findUtenteInvitatoBYID(GRVUtenti.DataSource, idUI)
    '    'Else
    '    '    oPersona = oRis.findUtenteInvitatoBYIDPersona(GRVUtenti.DataSource, idP)
    '    'End If


    '    'oQuest = DALQuestionario.readQuestionarioByPersona(False, oQuest.id, oQuest.idLingua, idP, idUI, idRisposta)

    '    'If oQuest.idFiglio > 0 Then
    '    '    oQuest = DALQuestionario.readQuestionarioByPersona(False, oQuest.idFiglio, oQuest.idLingua, idP, idUI, idRisposta)
    '    'End If

    '    Dim oPersone As New List(Of UtenteInvitato)
    '    oPersone.Add(oPersona)
    '    'oGestioneRisposte.calcoloPunteggio(oPersone)
    '    PNLDettagli.Visible = True

    '    If oQuest.rispostaQuest.oStatistica.punteggio = Decimal.MinValue Then
    '        PHStat.Controls.Clear()
    '    Else
    '        addUCpnlValutazione()
    '    End If


    '    With Me.Resource
    '        .setLabel(LBNomeUtente)
    '    End With
    '    LBNomeUtente.Text += "<b>" + oPersona.Cognome + " " + oPersona.Nome + "</b>"
    '    LBNomeQuestionario.Text = Me.QuestionarioCorrente.nome

    '    bindDataList(iPag)

    'End Sub
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound
        iPag = e.Item.ItemIndex
        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(iPag).FindControl("DLDomande")
        dlDomande.DataSource = DLPagine.DataSource.Item(iPag).domande
        dlDomande.DataBind()
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        iDom = e.Item.ItemIndex
        Dim oQuest As New Questionario
        oQuest.pagine = DLPagine.DataSource
        DLPagine.Controls(iPag).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(oQuest, iPag, iDom, True))
    End Sub
    Public Overrides Sub BindDati()
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucStatisticheUtentiQuest", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(Me.LBNomeUtente)
        End With
    End Sub
    'Protected Sub LNBStampaQuestionario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBStampaQuestionario.Click
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.AddHeader("Content-Disposition", "attachment; filename=prova.xls")
    '    Response.ContentType = "application/vnd.ms-excel"
    '    Response.Charset = ""
    '    Response.ContentEncoding = System.Text.Encoding.Default
    '    Me.EnableViewState = False
    '    Dim tw As New System.IO.StringWriter
    '    Dim hw As New System.Web.UI.HtmlTextWriter(tw)
    '    DLPagine.RenderControl(hw)
    '    Response.Write(tw.ToString())
    '    Response.End()
    'End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'bindDataList()

    End Sub
    Public Sub addUCpnlValutazione(ByVal oStat As Statistica)
        'PNLmenu.Visible = False
        PHStat.Controls.Clear()
        Dim ctrl As New Control
        ctrl = Page.LoadControl(RootObject.ucPNLValutazione)
        ctrl.ID = "ucPNLvaluta"
        'Me.QuestionarioCorrente.rispostaQuest.oStatistica = oStat
        DirectCast(ctrl, UCpnlValutazione).CaricaByRisposta(oStat)
        PHStat.Controls.Add(ctrl)
    End Sub
    Public Overrides Sub SetControlliByPermessi()
    End Sub
End Class

