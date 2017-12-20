Imports COL_Questionario

Partial Public Class PaginaEdit
    Inherits PageBaseQuestionario



    'Dim oQuest As New Questionario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Select Case Me.QuestionarioCorrente.tipo
            Case COL_Questionario.Questionario.TipoQuestionario.Modello
                Me.RedirectToUrl(RootObject.ModelliGestioneList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
            Case COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
            Case COL_Questionario.Questionario.TipoQuestionario.Sondaggio
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
            Case Else
                Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=0")
        End Select
    End Sub

    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        If Me.QuestionarioCorrente.pagine.Count = 0 Then
            Me.RedirectToUrl(RootObject.QuestionarioAdmin & "?type=" & Me.QuestionarioCorrente.tipo & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
        Else
            Me.RedirectToUrl(RootObject.QuestionarioEdit & "?type=" & Me.QuestionarioCorrente.tipo & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
        End If
    End Sub

    Protected Sub LNBSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva.Click
        'oQuest = Me.QuestionarioCorrente
        Dim oPagina As New QuestionarioPagina
        If Not ViewState("oPagina") Is Nothing Then
            oPagina = ViewState("oPagina")
        End If
        setPagina(oPagina)
        saveRedirect(oPagina)
    End Sub

    Private Sub saveRedirect(ByVal oPagina As QuestionarioPagina)
        DALPagine.Pagina_Salva(oPagina)
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.QuestionarioCorrente.idLingua, False)
        Me.RedirectToUrl(RootObject.QuestionarioEdit + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub

    Private Function setPagina(ByVal oPagina As QuestionarioPagina)
        oPagina.id = Me.PaginaCorrenteID
        oPagina.idQuestionarioMultilingua = Me.QuestionarioCorrente.idQuestionarioMultilingua

        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.LibreriaDiDomande OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            oPagina.descrizione = ""
            oPagina.nomePagina = Me.QuestionarioCorrente.nome
            'Dim _NomePagina As String = DirectCast(FRVPagina.FindControl("TXBNomePagina"), TextBox).Text
            'If String.IsNullOrEmpty(_NomePagina) Then
            'oPagina.nomePagina = Me.QuestionarioCorrente.nome
            'Else
            '    oPagina.nomePagina = _NomePagina 'Me.QuestionarioCorrente.nome
            'End If
        Else
        oPagina.descrizione = DirectCast(FRVPagina.FindControl("TXBDescrizione"), TextBox).Text
        oPagina.nomePagina = DirectCast(FRVPagina.FindControl("TXBNomePagina"), TextBox).Text
        End If

        oPagina.randomOrdineDomande = 0 'DirectCast(FRVPagina.FindControl("CBRandomOrdine"), CheckBox).Checked
        If oPagina.id = 0 Then
            oPagina.numeroPagina = Me.QuestionarioCorrente.pagine.Count + 1
            oPagina.dallaDomanda = 0
            oPagina.allaDomanda = 0
        End If
        Return oPagina
    End Function

    Public Overrides Sub BindDati()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        'Dim idPagina As String = Me.PaginaCorrenteID

        Dim oPagina As New QuestionarioPagina
        'oQuest = Me.QuestionarioCorrente

        If Me.PaginaCorrenteID > 0 Then
            oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
            ViewState("oPagina") = oPagina
        End If

        ' se è una libreria di domande inserisco automaticamente la prima (ed unica) pagina
        If (Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting OrElse Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None OrElse Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande) And Me.PaginaCorrenteID = 0 Then
            setPagina(oPagina)
            saveRedirect(oPagina)
        Else
            Dim listaPag As New List(Of QuestionarioPagina)
            listaPag.Add(oPagina)
            FRVPagina.DataSource = listaPag
            FRVPagina.DataBind()
        End If

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        'oltre ai permessi di comunita', verifica se e' stato impostato il permesso di amministrazione nella pagina precedente (normalmente accade se owned)
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande Or IIf(Session("qsIsAdmin_" & Me.QuestionarioCorrente.id) Is Nothing, False, IIf(Me.QuestionarioCorrente.id > 0, Session("qsIsAdmin_" & Me.QuestionarioCorrente.id), False)))
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_PaginaEdit", "Questionari")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        SetServiceTitle(Master)
        With Me.Resource
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBGestioneDomande, False, False)
            .setLinkButton(LNBSalva, False, False)
            .setLabel(LBerrore)
            .setButton(BTNContinua)
        End With
    End Sub

    Private Sub FRVPagina_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVPagina.DataBound
        With Me.Resource
            .setLabel(FRVPagina.FindControl("LBNomePagina"))
            .setLabel(FRVPagina.FindControl("LBDescrizione"))
        End With
    End Sub

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Private Sub BTNContinua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNContinua.Click
        LNBSalva_Click(sender, e)

    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class