Imports COL_Questionario

Partial Public Class QuestionarioView
    Inherits PageBaseQuestionario

    'Public Shared oQuest As New Questionario
    Public Shared _iPag As Integer
    Public Shared iDom As Integer
    Dim oPagedDataSource As New PagedDataSource
    Dim bindDone As Boolean
    Dim oGestioneDomande As New GestioneDomande
    Dim oGestioneRisposte As New GestioneRisposte
    Private _SmartTagsAvailable As SmartTags
    Dim oGestioneQuest As New GestioneQuestionario
    Public Shared idCollection As New list(Of Integer)
    Public Shared isCorrezione As Boolean

    Public ReadOnly Property displayDifficulty() As String
        Get
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                Return "hide"   '"display:none;" 'False
            Else
                Return "show"   '"text-align: right;" 'True
            End If
        End Get
    End Property
    Public ReadOnly Property showDifficulty() As Boolean
        Get
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private Property iPag() As Integer
        Get
            Return _iPag
        End Get
        Set(ByVal value As Integer)
            _iPag = value
        End Set
    End Property
    ' indica la modalità della pagina: 0=gestione, 1=cestino
    Private Property pageMode() As String
        Get
            Return Request.QueryString("mode")
        End Get
        Set(ByVal value As String)
            ViewState("pageMode") = Request.QueryString("mode")
        End Set
    End Property
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.ApplicationUrlBase())
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentType As QuestionnaireType = Me.QuestionarioCorrente.tipo
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Random And Me.QuestionarioCorrente.idFiglio = 0 And Me.QuestionarioCorrente.pagine.Count = 0 Then
            Dim oGestioneQuestionario As New GestioneQuestionario
            oGestioneQuestionario.generaQuestionarioRandomDestinatario(True, currentType)
        ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione And Not Page.IsPostBack Then
            idCollection = DALDomande.readIdDomandeAutovalutazione(Me.QuestionarioCorrente.id, -1)
            isCorrezione = False 'serve per i test di autovalutazione
            vaiPaginaDopo()
            Exit Sub 'bindDataList(iPag) viene eseguito anche in vaiPaginaDopo()
        End If

        bindDataList()

    End Sub
    Protected Sub bindDataList()
        oPagedDataSource.DataSource = Me.QuestionarioCorrente.pagine
        oPagedDataSource.AllowPaging = True
        oPagedDataSource.PageSize = 1
        oPagedDataSource.CurrentPageIndex = iPag
        Dim counter As Int16 = 0
        If Me.QuestionarioCorrente.pagine.Count > 1 And iPag + 1 < Me.QuestionarioCorrente.pagine.Count Then
            LkbNext.Visible = True
            If iPag < 1 Then
                LkbBack.Visible = False
            Else
                LkbBack.Visible = True
            End If
        Else
            If Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                LkbNext.Visible = False
            Else
                If iPag < 1 Then
                    LkbBack.Visible = False
                Else
                    LkbBack.Visible = True
                End If
                LkbNext.Visible = True
            End If
            If iPag > 0 Then
                LkbBack.Visible = True
            End If
        End If
        If Not PHnumeroPagina.Controls.Count = (Me.QuestionarioCorrente.pagine.Count) Or PHnumeroPagina.Controls.Count = 0 Then
            PHnumeroPagina.Controls.Clear()
            For counter = 1 To Me.QuestionarioCorrente.pagine.Count
                Dim LKBpagina As New LinkButton
                LKBpagina.Text = counter.ToString + "&nbsp;&nbsp;&nbsp;"
                LKBpagina.ID = "LKBpagina_" + counter.ToString
                LKBpagina.Attributes.Add("click", "LKBpagina_OnClientClick()")
                AddHandler LKBpagina.Click, AddressOf LKBpagina_OnClientClick
                LKBpagina.ToolTip = "Pagina " + counter.ToString
                'If counter = iPag + 1 Then
                '    LKBpagina.CssClass = "Selected"
                'Else
                '    LKBpagina.CssClass = ""
                'End If
                Try
                    PHnumeroPagina.Controls.Add(LKBpagina)
                Catch ex As Exception
                    'inviaMailErrore(ex)
                End Try
            Next
        End If

        DLPagine.DataSource = oPagedDataSource
        DLPagine.DataBind()
        Try
            If (Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Autovalutazione) Then
                If Not iPag = Me.QuestionarioCorrente.pagine.Count - 1 Or Not isCorrezione Then
                    'l'and serve per fare in modo che cliccando indietro le pagine vengano visualizzate sempre in modalita' "correzione"
                    DLPagine = oGestioneRisposte.setRispostePaginaCorrette(DLPagine, Me.QuestionarioCorrente.domande)
                    DLPagine.Controls(0).FindControl("DLDomande").Controls(0).FindControl("LBSuggerimento").Visible = True
                Else
                    DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)
                End If
            Else
                DLPagine = oGestioneRisposte.setRispostePagina(DLPagine, Me.QuestionarioCorrente.domande)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub loadDomandeOpzioni(ByVal sender As Object, ByVal e As DataListItemEventArgs)

        iDom = e.Item.ItemIndex

        oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, False)
        DLPagine.Controls(0).FindControl("DLDomande").Controls(iDom).FindControl("PHOpzioni").Controls.Add(oGestioneDomande.loadDomandeOpzioni(Me.QuestionarioCorrente, iPag, iDom, False))

    End Sub
    Protected Sub DLPagine_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLPagine.ItemDataBound

        Dim dlDomande As New DataList
        dlDomande = DLPagine.Controls(0).FindControl("DLDomande")

        dlDomande.DataSource = Me.QuestionarioCorrente.pagine.Item(iPag).domande
        dlDomande.DataBind()

    End Sub
    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        With Me.QuestionarioCorrente
            If .tipo = .TipoQuestionario.Sondaggio OrElse .tipo = .TipoQuestionario.Meeting OrElse .tipo = .TipoQuestionario.LibreriaDiDomande OrElse .tipo = .TipoQuestionario.Autovalutazione Then
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=" + .tipo.ToString())
            ElseIf .tipo = .TipoQuestionario.Modello Then
                Me.RedirectToUrl(RootObject.ModelliGestioneList + "&type=3")
            Else
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=0")
            End If

        End With
    End Sub
    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        Me.RedirectToUrl(RootObject.QuestionarioEdit + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub
    Public Overrides Sub BindDati()
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            iPag = -1
        Else
            iPag = 0
        End If

        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)

        If Me.pageMode = 0 Then
            LNBGestioneDomande.Visible = True
            LNBCestino.Visible = False
        Else
            LNBGestioneDomande.Visible = False
            LNBCestino.Visible = True
        End If
        If Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
            bindDataList()
        End If


        LBTitolo.Text += Me.QuestionarioCorrente.nome
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.QuestionariSuInvito)
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_QuestionarioView", "Questionari")

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        SetServiceTitle(Master)
        With Me.Resource
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBImportaModello, False, False, False, True)

            If Me.pageMode = 0 Then
                .setLinkButton(LNBGestioneDomande, False, False)
            Else
                .setLinkButton(LNBCestino, False, False)
            End If
            .setLabel(LBTitolo)
            .setLabel(LBerrore)
        End With
    End Sub
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property
    Protected Sub LKBpagina_OnClientClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim isValida As Boolean = True
        Try
            'If isValida Then
            'LBTitolo.Text += Me.QuestionarioCorrente.nome
            Dim LKBpag As New LinkButton
            LKBpag = DirectCast(sender, LinkButton)
            iPag = Integer.Parse(LKBpag.Text.Substring(0, LKBpag.Text.IndexOf("&"))) - 1
            'IMBDopo.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
            LkbNext.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag + 2).ToString()
            LkbNext.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag + 2).ToString()

            'IMBprima.AlternateText = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
            LkbBack.ToolTip = Me.Resource.getValue("IMBprimaDopo") & (iPag).ToString()
            LkbBack.Text = Me.Resource.getValue("IMBprimaDopo") '& (iPag).ToString()
            'LBTroppeRispostePagina.Visible = False

            'LKBpag.CssClass = "Selected"

            'Dim counter As Int16 = 0
            'counter = CInt(LKBpag.ID.Substring(LKBpag.ID.LastIndexOf("_") + 1))
            'If counter = iPag + 1 Then
            '    LKBpag.ForeColor = Color.Red
            'End If
            'Else
            'LBTroppeRispostePagina.Visible = True
            'End If
            bindDataList()
        Catch ex As Exception

        End Try

    End Sub
    'Protected Sub IMBprima_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBprima.Click
    '    If iPag > 0 Then

    '        ' LBTitolo.Text += Me.QuestionarioCorrente.nome

    '        iPag = iPag - 1

    '    End If

    '    bindDataList()
    'End Sub

    Private Sub LkbBack_Click(sender As Object, e As EventArgs) Handles LkbBack.Click
        If iPag > 0 Then

            ' LBTitolo.Text += Me.QuestionarioCorrente.nome

            iPag = iPag - 1

        End If

        bindDataList()
    End Sub

    Private Sub LkbNext_Click(sender As Object, e As EventArgs) Handles LkbNext.Click
        vaiPaginaDopo()
    End Sub
    'Protected Sub IMBdopo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBdopo.Click
    '    vaiPaginaDopo()
    'End Sub
    Protected Sub vaiPaginaDopo()

        If iPag > -2 Then
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Autovalutazione Then
                If isCorrezione Then
                    isCorrezione = False
                Else
                    If iPag = Me.QuestionarioCorrente.domande.Count - 1 Or iPag = 0 Then
                        If idCollection.Count > 0 Then
                            Dim oGestioneQuestionario As New GestioneQuestionario
                            Dim oPagina As New QuestionarioPagina
                            oPagina.allaDomanda = Me.QuestionarioCorrente.pagine.Count + 1
                            oPagina.dallaDomanda = oPagina.allaDomanda
                            Dim oDomanda As New Domanda
                            oDomanda = oGestioneQuestionario.domandaSelect(idCollection, Me.QuestionarioCorrente.idLingua)
                            oDomanda.numero = oPagina.allaDomanda
                            oPagina.domande.Add(oDomanda)
                            Me.QuestionarioCorrente.pagine.Add(oPagina)
                            Me.QuestionarioCorrente.domande.Add(oDomanda)
                        Else
                            Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
                            LBerrore.Text = Me.Resource.getValue("MSGNoMoreQuestions")
                            Exit Sub
                        End If
                        isCorrezione = True

                    End If

                    iPag = iPag + 1


                End If
            ElseIf Me.QuestionarioCorrente.pagine.Count > iPag + 1 Then
                'senza questo controllo se si aggiorna la pagina va alla pagine seguente anche se non esiste
                iPag = iPag + 1
            End If

        End If
        bindDataList()
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If Me.pageMode = 0 Then
            LNBGestioneDomande.Visible = MyBase.Servizio.GestioneDomande
        Else
            LNBCestino.Visible = MyBase.Servizio.CancellaQuestionario
        End If

        If Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Modello Then
            LNBImportaModello.Visible = False
        End If

    End Sub
    Protected Sub LNBCestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCestino.Click
        Me.RedirectToUrl(RootObject.QuestionariCestinoList + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub
    Protected Sub LNBImportaModello_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBImportaModello.Click
        'Dim nomeValido As Boolean = oGestioneDomande.controllaNome(Me.QuestionarioCorrente.nome)
        Dim quest As Questionario = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, 1, False)
        Me.QuestionarioCorrente = quest
        Dim questNome As String
        Dim copiePresenti As Integer = 1
        'If DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia di " & Me.QuestionarioCorrente.nome) = 0 Then
        If Not DALQuestionario.IsDuplicatedName(Me.ComunitaCorrenteID, quest.id, quest.tipo, "Copia di " & quest.nome) = 0 Then
            questNome = "Copia di " & Me.QuestionarioCorrente.nome
            Me.QuestionarioCorrente.nome = questNome
        Else
            Do
                copiePresenti = copiePresenti + 1
            Loop While DALQuestionario.IsDuplicatedName(Me.ComunitaCorrenteID, quest.id, quest.tipo, "Copia (" & copiePresenti & ") di " & quest.nome)

            ' Loop While Not DALQuestionario.controllaNome(Me.ComunitaCorrenteID, "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome) = 0
            Me.QuestionarioCorrente.nome = "Copia (" & copiePresenti & ") di " & Me.QuestionarioCorrente.nome
            questNome = Me.QuestionarioCorrente.nome
        End If
        oGestioneQuest.copiaModello()
        Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=0")

        'If nomeValido Then
        '    Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.QuestionarioCorrente.id, 1, False)
        '    Dim urlQuestionarioAdmin As String = oGestioneDomande.copiaModello()
        '    If Not urlQuestionarioAdmin = String.Empty Then
        '        Me.RedirectToUrl(urlQuestionarioAdmin)
        '    End If
        'Else
        '    MLVquestionari.SetActiveView(VIWmessaggi)
        '    LBerrore.Text = Me.Resource.getValue("LBerroreModello")
        'End If
    End Sub

    Private Sub QuestionarioView_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If String.IsNullOrEmpty(LkbNext.Text) Then
            LkbNext.Text = "&gt;"
        End If

        If String.IsNullOrEmpty(LkbBack.Text) Then
            LkbBack.Text = "&lt;"
        End If

        If String.IsNullOrEmpty(LkbNext.ToolTip) Then
            LkbNext.ToolTip = "Avanti"
        End If

        If String.IsNullOrEmpty(LkbBack.ToolTip) Then
            LkbBack.ToolTip = "Indietro"
        End If
    End Sub



    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class