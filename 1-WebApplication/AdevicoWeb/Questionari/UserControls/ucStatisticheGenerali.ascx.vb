Imports COL_Questionario

Partial Public Class ucStatisticheGenerali
    Inherits BaseControlQuestionario
    'Dim idQuestionario As String
    'Dim oQuest As New Questionario
    Public nRisposteQuest As Integer
    Dim iDom As Integer
    Dim oGestioneRisposte As New GestioneRisposte
    Dim oGestioneQuest As New GestioneQuestionario
    Private _SmartTagsAvailable As SmartTags

    Private Property isCompiling()
        Get
            Return Request.QueryString("comp")
        End Get
        Set(ByVal value)
            ViewState("comp") = Request.QueryString("comp")
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

    Private Property idQuestionario() As Integer
        Get
            If Request.QueryString("idq") = String.Empty Then
                Return Me.QuestionarioCorrente.id
            Else
                Return Request.QueryString("idq")
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("idq") = Request.QueryString("idq")
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetInternazionalizzazione()
        If isCompiling = "2" Then
            LNBTornaLista.Visible = False 'sostituire il linkbutton con un hyperlink e mantenerlo anche in questa pagina
        Else
            LNBTornaLista.Visible = String.IsNullOrEmpty(Request.QueryString("NoBack")) OrElse Request.QueryString("NoBack") <> "true"
        End If
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, idQuestionario, Me.LinguaID, True, True)

        Select Case Me.QuestionarioCorrente.tipo
            Case Questionario.TipoQuestionario.Random
                MLVDomande.SetActiveView(VIWQuestionari)
                oGestioneQuest.readDomandeQuestionarioRandom()
            Case QuestionnaireType.RandomMultipleAttempts
                MLVDomande.SetActiveView(VIWQuestionari)
                oGestioneQuest.readDomandeQuestionarioRandom()
            Case Questionario.TipoQuestionario.Autovalutazione
                MLVDomande.SetActiveView(VIWQuestionari)
                oGestioneQuest.readDomandeQuestionarioRandom()
            Case Questionario.TipoQuestionario.Sondaggio
                MLVDomande.SetActiveView(VIWQuestionari)
                LKBEsportaExcel.Visible = False
                IMGExcel.Visible = False
                LBNumeroRisposteQuest.Visible = False
            Case Questionario.TipoQuestionario.Meeting
                MLVDomande.SetActiveView(VIWMeeting)
                VIWMeeting_load()
            Case Else
                MLVDomande.SetActiveView(VIWQuestionari)
        End Select
        LBNumeroRisposteQuest.Text += Me.QuestionarioCorrente.risposteQuestionario.Count.ToString()
        DLDomande.DataSource = Me.QuestionarioCorrente.domande
        DLDomande.DataBind()
        'Me.QuestionarioCorrente = Nothing

    End Sub
    Protected Sub loadRisposteOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)

        iDom = e.Item.ItemIndex

        'oQuest = Session("oQuest")
        SetInternazionalizzazioneDLDomande(e)
        Dim tabellaRisposte As New Table
        tabellaRisposte = oGestioneRisposte.loadTabellaRisposte(Me.QuestionarioCorrente.domande(iDom))
        DLDomande.Controls(iDom).FindControl("PHOpzioni").Controls.Add(tabellaRisposte)

    End Sub
    Protected Sub visualizzaRisposte(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        Dim idDomanda As Integer = DLDomande.DataKeys.Item(e.Item.ItemIndex)
        'Session("RisposteLibere") = oQuest.risposteQuestionario(0).risposteDomande(iDom)
        Me.RedirectToUrl(RootObject.RisposteLibere + "?id=" + idDomanda.ToString() + "&idO=" + e.CommandArgument)
    End Sub
    Protected Sub LKBEsportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBEsportaExcel.Click

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("Content-Disposition", "attachment; filename=prova.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        Response.ContentEncoding = System.Text.Encoding.Default
        Me.EnableViewState = False
        Dim tw As New System.IO.StringWriter
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        DLDomande.RenderControl(hw)
        Response.Write(tw.ToString())
        Response.End()

    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        If Not Me.QuestionarioCorrente Is Nothing Then
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Sondaggio
                    MyBase.SetCulture("pg_ucStatisticheGeneraliSondaggi", "Questionari")
                Case Questionario.TipoQuestionario.Meeting
                    MyBase.SetCulture("pg_ucStatisticheGeneraliMeeting", "Questionari")
                Case Else
                    MyBase.SetCulture("pg_ucStatisticheGeneraliQuest", "Questionari")
            End Select
        Else
            MyBase.SetCulture("pg_ucStatisticheGeneraliQuest", "Questionari")
        End If

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LKBEsportaExcel, False, False)
            ' per i sondaggi devo /andare/ (non tornare) alla lista iniziale, perchè accedono a questa pagina gli utenti in compilazione
            If Me.isCompiling = "1" OrElse Me.isCompiling = "2" Then
                Me.Resource.setLinkButtonToValue(LNBTornaLista, "listaQuest", False, False)
            Else
                Me.Resource.setLinkButton(LNBTornaLista, False, False)
            End If

            .setLinkButton(LNBTornaLista, False, False)
            .setLabel(LBNumeroRisposteQuest)
        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLDomande(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBLnumeroRisposte"))
            .setLinkButton(LNBTornaLista, True, False, False, False)
        End With
    End Sub
    Protected Sub LNBTornaLista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTornaLista.Click
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            ' per i sondaggi devo tornare alla lista iniziale, perchè accedono a questa pagina gli utenti in compilazione
            If Me.isCompiling = "1" Then
                Response.Redirect("../" + RootObject.QuestionariList + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
            ElseIf isCompiling = "2" Then
                Response.Redirect(Request.UrlReferrer.PathAndQuery)
            Else
                Response.Redirect("../" + RootObject.QuestionariStatList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
            End If
        Else
            If String.IsNullOrEmpty(Request.QueryString("BackUrl")) Then
                'If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
                '    Me.RedirectToUrl(lm.Comol.Modules.EduPath.BusinessLogic.RootObject..EduPath_StatInActivity(Me.QuestionarioCorrente.ownerId))
                'ElseIf Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity Then
                '    Me.RedirectToUrl(RootObject.EduPath_StatInSubActivity(Me.QuestionarioCorrente.ownerId))
                'End If
            Else
                PageUtility.RedirectToUrl(Server.HtmlDecode(Request.QueryString("BackUrl")))
            End If

                'ElseIf Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity Then
                '    Me.RedirectToUrl(RootObject.EduPath_StatInActivity(Me.QuestionarioCorrente.ownerId))
                'ElseIf Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity Then
                '    Me.RedirectToUrl(RootObject.EduPath_StatInSubActivity(Me.QuestionarioCorrente.ownerId))
            End If
    End Sub

#Region "meeting"
    Class dtoMeetingStat
        Public meetingDate As Date
        Public ReadOnly Property meetingDateStr() As String
            Get
                Return meetingDate
            End Get
        End Property
        Public ReadOnly Property isAll() As Boolean
            Get
                If votes = maxVotes Then
                    Return True
                Else : Return False
                End If
            End Get
        End Property
        Public zoneIndex As Integer
        Public zone As String
        Public votes As String
        Public maxVotes As String
    End Class
    Private Sub VIWMeeting_load()
        Dim oStatList As New List(Of dtoMeetingStat)
        For Each oDomanda As Domanda In Me.QuestionarioCorrente.domande
            For Each oOpz As DomandaOpzione In oDomanda.domandaRating.opzioniRating
                For c As Integer = 1 To oDomanda.domandaRating.numeroMeeting
                    Dim oStat As New dtoMeetingStat
                    Dim count = (From ri In oDomanda.risposteDomanda Where ri.valore = (c) And ri.idDomandaOpzione = oOpz.id Select ri.id).ToList.Count
                    oStat.votes = count
                    oStat.maxVotes = Me.QuestionarioCorrente.risposteQuestionario.Count
                    oStat.meetingDate = oDomanda.domandaRating.intestazioniMeeting(c - 1)
                    oStat.zone = oOpz.testo
                    oStat.zoneIndex = oOpz.numero
                    oStatList.Add(oStat)
                Next
            Next
        Next
        'La lista viene ordinata per numero di risposte, quindi per data, quindi per indice della zona
        RPTRisposte.DataSource = oStatList.OrderByDescending(Function(c) c.votes).ThenBy(Function(c) c.meetingDate).ThenBy(Function(c) c.zoneIndex).ToList
        RPTRisposte.DataBind()
    End Sub
    Private Sub RPTRisposte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTRisposte.ItemDataBound
        If e.Item.ItemIndex < 0 Then
            With Me.Resource
                .setLabel(DirectCast(e.Item.FindControl("LBHeadDate"), Label))
                .setLabel(DirectCast(e.Item.FindControl("LBHeadZone"), Label))
                .setLabel(DirectCast(e.Item.FindControl("LBHeadVotes"), Label))
            End With
        Else
            If DirectCast(RPTRisposte.DataSource.item(e.Item.ItemIndex), dtoMeetingStat).isAll Then
                DirectCast(e.Item.FindControl("LBDate"), Label).Font.Bold = True
                DirectCast(e.Item.FindControl("LBZone"), Label).Font.Bold = True
                DirectCast(e.Item.FindControl("LBvotes"), Label).Font.Bold = True
            End If
        End If
    End Sub

#End Region

    Public Overrides Sub SetControlliByPermessi()
    End Sub

    Private Sub DLDomande_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLDomande.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType.AlternatingItem Then
            Dim domanda As Domanda = DirectCast(e.Item.DataItem, Domanda)
            Dim tRow As HtmlTableRow = e.Item.FindControl("TRlibraryName")
            Dim oLiteral As Literal = e.Item.FindControl("LTlibraryName")

            If domanda.numero = 1 AndAlso QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = domanda.idQuestionario).Any Then
                tRow.Visible = True
                oLiteral.Text = String.Format(Resource.getValue("libraryName"), QuestionarioCorrente.librerieQuestionario.Where(Function(l) l.idLibreria = domanda.idQuestionario).Select(Function(l) l.nomeLibreria).FirstOrDefault())
            End If
        End If
    End Sub
End Class