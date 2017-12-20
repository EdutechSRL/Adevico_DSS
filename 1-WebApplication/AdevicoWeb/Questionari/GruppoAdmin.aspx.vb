Imports COL_Questionario

Partial Public Class GruppoAdmin
    Inherits PageBaseQuestionario

    ' indica il tipo di lista: 0=questionari, 2=sondaggi
    Private Property groupType() As String
        Get
            Return Request.QueryString("type")
        End Get
        Set(ByVal value As String)
            ViewState("type") = Request.QueryString("type")
        End Set
    End Property

    Protected Sub BTNSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNSalva.Click

        Me.GruppoCorrente.nome = TXBNomeGruppo.Text
        Me.GruppoCorrente.dataCreazione = Now()
        Me.GruppoCorrente.dataModifica = Now()
        Me.GruppoCorrente.descrizione = TXBDescrizione.Text
        Me.GruppoCorrente.idGruppoPadre = Me.GruppoDefaultID
        Me.GruppoCorrente.idComunita = Me.ComunitaCorrenteID
        Me.GruppoCorrente.idPersona = Me.UtenteCorrente.ID
        Me.GruppoCorrente.isCancellato = False
        Me.GruppoCorrente.isComunita = True
        Me.GruppoCorrente.isCondiviso = False
        Me.GruppoCorrente.isPersonale = False
        Me.GruppoCorrente.isPubblico = False

        DALQuestionarioGruppo.InsertGruppo(Me.GruppoCorrente)
        Me.RedirectToUrl(RootObject.GruppiList + "?type=" + Me.groupType)

    End Sub

    Public Overrides Sub BindDati()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)

        If Me.GruppoCorrente.id > 0 Then
            TXBNomeGruppo.Text = Me.GruppoCorrente.nome
            TXBDescrizione.Text = Me.GruppoCorrente.descrizione
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_GruppoAdmin", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBNuovoGruppo)
            .setLabel(LBDescrizione)
            .setLabel(LBerrore)
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBGestioneDomande, False, False)
            .setButton(BTNSalva, False, False, False, False)
        End With
        SetServiceTitle(Master)
    End Sub

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Me.RedirectToUrl(RootObject.GruppiList + "?type=" + Me.groupType)
    End Sub


    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=1")
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