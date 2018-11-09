Imports COL_Questionario

Partial Public Class DomandaEdit
    Inherits PageBaseQuestionario

    Dim idDomanda As String
    Dim idPagina As String
    Dim idQuestionario As String
    Dim oDomanda As New Domanda
    'Dim oQuest As New Questionario
    Dim oGestioneDomande As New GestioneDomande

    Public ReadOnly Property visibilityValutazione() As String
        Get
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Questionario
                    Return "block"
                Case Else
                    Return "none"
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get
            isCompileForm = False
        End Get
    End Property

    'Private Property mode() As String
    '    Get
    '        Return Request.QueryString("mode")
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("mode") = value
    '    End Set
    'End Property

    Private Property DomandaModificata() As Domanda
        Get
            Return ViewState("DomandaModificata")
        End Get
        Set(ByVal value As Domanda)
            ViewState("DomandaModificata") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.LibreriaDiDomande Then
                LNBAggiungiLibreria.Visible = False
            End If
            If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
                Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
            ElseIf Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting Then
                Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Meeting
            Else

            End If
            addUserControl()
        Catch ex As Exception
            MLVquestionari.SetActiveView(VIWmessaggi)
            LBerrore.Visible = True
        End Try
    End Sub

    Private Sub addUserControl()

        PHOpzioni.Controls.Clear()
        PHOpzioni.Controls.Add(oGestioneDomande.addUCDomandaEdit(Me.DomandaCorrente.tipo))

    End Sub

    Protected Sub BTNConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNConferma.Click
        oDomanda = Me.DomandaCorrente
        oDomanda.id = 0
        oDomanda.idPagina = 0
        oDomanda.idLingua = 0
        oDomanda.idQuestionario = DDLLibrerie.SelectedItem.Value
        DALDomande.Salva(oDomanda, Me.QuestionarioCorrente.isReadOnly, Me.QuestionarioCorrente.tipo = COL_Questionario.Questionario.TipoQuestionario.LibreriaDiDomande)
        LBConferma.Visible = True
        PNLSelectLibreria.Visible = False
    End Sub

    Protected Sub LNBCartellaPrincipale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBCartellaPrincipale.Click
        Me.RedirectToUrl(RootObject.QuestionariGestioneList + "&type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub

    Protected Sub LNBGestioneDomande_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBGestioneDomande.Click
        Me.QuestionarioCorrente = DALQuestionario.readQuestionarioBYLingua(Me.PageUtility.CurrentContext, Me.QuestionarioCorrente.id, Me.LinguaQuestionario, False)
        Me.RedirectToUrl(RootObject.QuestionarioEdit + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
    End Sub

    Protected Sub LNBSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva.Click
        Dim returnValue As Integer
        If Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla Or Me.DomandaCorrente.tipo = Domanda.TipoDomanda.DropDown Then
            If DirectCast(GestioneDomande.FindControlRecursive(PHOpzioni, "CUVvalutabile"), CustomValidator).IsValid Then
                returnValue = oGestioneDomande.salvaDomanda(DirectCast(GestioneDomande.FindControlRecursive(PHOpzioni, "FRVDomanda"), FormView))
            Else
                returnValue = 3
            End If
        Else
            returnValue = oGestioneDomande.salvaDomanda(DirectCast(GestioneDomande.FindControlRecursive(PHOpzioni, "FRVDomanda"), FormView))
        End If

        Select Case returnValue
            Case 0
                Me.RedirectToUrl(RootObject.QuestionarioEdit + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
            Case 1
                CTRLmessages.InitializeControl(Resource.getValue("ErroreOpzioni"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                CTRLmessages.Visible = True
            Case 2
                CTRLmessages.InitializeControl(Resource.getValue("ErroreTestoDomanda"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                CTRLmessages.Visible = True
            Case 3
                CTRLmessages.InitializeControl(Resource.getValue("ErroreNoCorretta"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
                CTRLmessages.Visible = True
        End Select

    End Sub

    Protected Sub LNBAggiungiLibreria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBAggiungiLibreria.Click
        'DDLLibrerie.DataSource = DALQuestionario.readLibrerieDomande
        'DDLLibrerie.DataBind()
        'PNLSelectLibreria.Visible = True
    End Sub

    Public Overrides Sub BindDati()
        'sondaggi e meeting hanno un solo tipo di domanda, quindi no scelta
        If Me.DomandaCorrente.id = 0 AndAlso Not (Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio OrElse Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Meeting) Then
            Me.MLVquestionari.SetActiveView(Me.VIWSceltaTipoDomanda)
        Else
            Me.MLVquestionari.SetActiveView(Me.VIWdati)
        End If

        Dim oDomanda As New Domanda
        oDomanda = Me.DomandaCorrente

        idQuestionario = Me.QuestionarioCorrente.id
        idDomanda = oDomanda.id
        idPagina = oDomanda.idPagina
        Me.DomandaCorrente.tipo = oDomanda.tipo
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVquestionari.SetActiveView(Me.VIWmessaggi)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_DomandaEdit", "Questionari")
    End Sub


    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBCartellaPrincipale, False, False)
            .setLinkButton(LNBGestioneDomande, False, False)
            .setLinkButton(LNBAggiungiLibreria, False, False)
            .setLinkButton(LNBSalva, False, False)
            .setLinkButton(LNBSalva2, False, False)
            .setLabel(LBConferma)
            .setLabel(LBMessage)
            .setLabel(LBerrore)
            .setButton(BTNConferma, False, False, False, False)
            LBSceltaMultipla.Text = .getValue("3.MNUTipoDomanda")
            LBTestoLibero.Text = .getValue("5.MNUTipoDomanda")
            LBRating.Text = .getValue("1.MNUTipoDomanda")
            LBNumerica.Text = .getValue("2.MNUTipoDomanda")
            LBDropDown.Text = .getValue("4.MNUTipoDomanda")
            .setLabel(LBSceltaDomanda)
            LNBMultiplaWIZ.Text = .getValue("LNBwizard")
            LNBMultiplaStandard.Text = .getValue("LNBavanzato")
            LNBTestoLiberoWIZ.Text = .getValue("LNBwizard")
            LNBTestoLiberoStandard.Text = .getValue("LNBavanzato")
            LNBNumericaStandard.Text = .getValue("LNBavanzato")
            LNBNumericaWIZ.Text = .getValue("LNBwizard")
            LNBRatingStandard.Text = .getValue("LNBavanzato")
            LNBRatingWIZ.Text = .getValue("LNBwizard")
            LNBDropDownStandard.Text = .getValue("LNBavanzato")
            LNBDropDownWIZ.Text = .getValue("LNBwizard")

            Master.ServiceTitle = .getValue("ServiceTitle")

            LBRatingStars.Text = .getValue("10.MNUTipoDomanda")
            LNBRatingStarsStandard.Text = .getValue("LNBavanzato")
            LNBRatingStarsWIZ.Text = .getValue("LNBwizard")

        End With
    End Sub

   

    Protected Sub mostraViewDati()
        Me.MLVquestionari.SetActiveView(Me.VIWdati)
        addUserControl()
    End Sub
   

    'Protected Sub IMBScegliNumerica_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBScegliNumerica.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Numerica
    '    mostraViewDati()
    'End Sub

    'Protected Sub IMBScegliRating_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBScegliRating.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Rating
    '    mostraViewDati()
    'End Sub

    'Protected Sub IMBScegliDropDown_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBScegliDropDown.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.DropDown
    '    mostraViewDati()
    'End Sub

    'Protected Sub IMBNumerica_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBNumerica.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Numerica
    '    mostraViewDati()
    'End Sub

    'Protected Sub IMGRating_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMGRating.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Rating
    '    mostraViewDati()
    'End Sub

    Protected Sub IMBDropDown_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBDropDown.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.DropDown
        mostraViewDati()
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        LNBCartellaPrincipale.Visible = (MyBase.Servizio.Admin Or MyBase.Servizio.CancellaQuestionario Or MyBase.Servizio.CopiaQuestionario Or MyBase.Servizio.GestioneDomande Or MyBase.Servizio.QuestionariSuInvito)
        LNBGestioneDomande.Visible = MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande
        LNBSalva.Visible = MyBase.Servizio.Admin Or MyBase.Servizio.GestioneDomande
        If Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            'TBLSceltaDomanda.Rows(3).Visible = False
            LNBCartellaPrincipale.Visible = False
        End If
    End Sub
    Protected Sub LNBSalva2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva2.Click
        LNBSalva_Click(sender, e)
    End Sub
    Protected Sub LNBMultiplaWIZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBMultiplaWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
        Me.RedirectToUrl(RootObject.WizardDomandaMultipla)
    End Sub
    Protected Sub LNBMultiplaStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBMultiplaStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
        mostraViewDati()
    End Sub
    Protected Sub LNBTestoLiberoWIZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTestoLiberoWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.TestoLibero
        Me.RedirectToUrl(RootObject.WizardDomandaTestoLibero)
    End Sub
    Protected Sub LNBTestoLiberoStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBTestoLiberoStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.TestoLibero
        mostraViewDati()
    End Sub
    Protected Sub LNBNumericaWIZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNumericaWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Numerica
        Me.RedirectToUrl(RootObject.WizardDomandaNumerica)
    End Sub
    Protected Sub LNBNumericaStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBNumericaStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Numerica
        mostraViewDati()
    End Sub
    Protected Sub LNBRatingWIZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBRatingWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Rating
        Me.RedirectToUrl(RootObject.WizardDomandaRating)
    End Sub
    Protected Sub LNBRatingStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBRatingStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Rating
        mostraViewDati()
    End Sub

    Private Sub LNBRatingStarsStandard_Click(sender As Object, e As EventArgs) Handles LNBRatingStarsStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.RatingStars
        mostraViewDati()
    End Sub
    
    Private Sub LNBRatingStarsWIZ_Click(sender As Object, e As EventArgs) Handles LNBRatingStarsWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Rating
        Me.RedirectToUrl(RootObject.WizardDomandaRatingStars)
    End Sub

    Protected Sub LNBDropDownWIZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBDropDownWIZ.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.DropDown
        Me.RedirectToUrl(RootObject.WizardDomandaDropDown)
    End Sub
    Protected Sub LNBDropDownStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBDropDownStandard.Click
        Me.DomandaCorrente.tipo = Domanda.TipoDomanda.DropDown
        mostraViewDati()
    End Sub


    'Protected Sub IMBScegliMultipla_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBScegliMultipla.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
    '    mostraViewDati()
    'End Sub

    'Protected Sub IMBScegliTestoLibero_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBScegliTestoLibero.Click
    '    Me.DomandaCorrente.tipo = Domanda.TipoDomanda.TestoLibero
    '    mostraViewDati()
    'End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property

    Private Sub Page_PreLoad1(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        'PageUtility.CurrentModule = PageUtility.GetModule(Services_Questionario.Codex)
        MyBase.Page_PreLoad(sender, e)
        Me.Master.ShowDocType = True
    End Sub

    Private Sub DomandaEdit_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If MLVquestionari.GetActiveView().ID = VIWSceltaTipoDomanda.ID _
            OrElse MLVquestionari.GetActiveView().ID = VIWmessaggi.ID Then
            Me.LNBSalva.Visible = False
        Else
            Me.LNBSalva.Visible = True
        End If
    End Sub
End Class