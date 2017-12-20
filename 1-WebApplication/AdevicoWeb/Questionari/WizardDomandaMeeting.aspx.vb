Imports COL_Questionario

Partial Public Class WizardDomandaMeeting
    Inherits PageBaseQuestionario

    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    Dim isChangingTable As Boolean = False
    Public isAperto As Boolean

    Public ReadOnly Property visibilityValutazione() As String
        Get
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Questionario
                    Return "block"
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Return "block"
                Case Else
                    Return "none"
            End Select
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            caricaDati()
            bindFields()
            bindIntestazioni()
            bindOpzioni()
        End If
        addHandlerFRVDomanda()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)
    End Sub
    Protected Sub addHandlerFRVDomanda()
        Try
            Dim oDatalist As DataList
            oDatalist = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
            AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound
            Dim oWizard As Wizard
            oWizard = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "WIZDomanda"), Wizard)
            AddHandler oWizard.FinishButtonClick, AddressOf WIZ_FinishButtonClick
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub WIZ_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs)
        salva()
    End Sub
    Public Sub caricaDati()
        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub
    Protected Sub bindFields()

        oGestioneDomande.bindFieldDomandaRating(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)

    End Sub
    Protected Sub bindOpzioni()
        oGestioneDomande.bindZoneDomandaMeeting(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindIntestazioni()
        oGestioneDomande.bindIntestazioniDomandaRating(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True
        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue, Nothing)
        'addHandlerFRVDomanda()
        bindOpzioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)
    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        isChangingTable = True
        If e.CommandName = "elimina" Then
            Dim n As Integer = e.Item.ItemIndex + 1
            oGestioneDomande.eliminaOpzioneRating(Me.FRVDomanda, e.Item.ItemIndex)
            bindOpzioni()
        End If
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)
    End Sub
    Protected Sub selezionaNumeroIntestazioni(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True
        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroIntestazioni(Me.DomandaCorrente, DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroColonne"), DropDownList))
        bindIntestazioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)
    End Sub
    Protected Sub selezionaTipoIntestazione(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True
        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)

        oGestioneDomande.selezionaNumeroIntestazioni(Me.DomandaCorrente, GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroColonne"))

        bindIntestazioni()
        bindOpzioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaRatingEdit", "Questionari")

    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        Master.ServiceTitle = Resource.getValue("ServiceTitle.Meeting")
    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBLPaginaCorrente"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTestoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTitoloOpzioni"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTipoIntestazione"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBIntestazione"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBNumeroOpzioni"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBNumeroColonne"))
            .setCheckBox(GestioneDomande.FindControlRecursive(FRVDomanda, "CBmostraND"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTestoND"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTestoDopoDomanda"))
            .setRegularExpressionValidator(GestioneDomande.FindControlRecursive(FRVDomanda, "REVTXBTestoDopoDomanda"))
            .setRegularExpressionValidator(GestioneDomande.FindControlRecursive(FRVDomanda, "REVTXBTestoND"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBAiuto"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBSuggerimento"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisObbligatoria"))
            For Each item As ListItem In DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "RBLTipoIntestazione"), RadioButtonList).Items
                item.Text = .getValue(DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "RBLTipoIntestazione"), RadioButtonList), item.Value)
            Next
            Dim wizard As System.Web.UI.WebControls.Wizard = GestioneDomande.FindControlRecursive(FRVDomanda, "WIZDomanda")
            wizard.StartNextButtonText = .getValue("StartNextButtonText")
            wizard.StepNextButtonText = .getValue("StepNextButtonText")
            wizard.StepPreviousButtonText = .getValue("StepPreviousButtonText")
            wizard.FinishPreviousButtonText = .getValue("StepPreviousButtonText")
            wizard.FinishCompleteButtonText = .getValue("FinishCompleteButtonText")

        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLOpzioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setCheckBox(e.Item.FindControl("CBisAltro"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setLabel(e.Item.FindControl("LBTestoMax"))
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoMin"))
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoMax"))

        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLIntestazioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBIntestazione"))
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBIntestazione"))
        End With
    End Sub
    Private Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound

        oGestioneDomande.FRVDomandaDataBound(Me.FRVDomanda)

        addHandlerFRVDomanda()

        SetInternazionalizzazioneFRVDomanda()
    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        Dim oDatalist As DataList

        oDatalist = GestioneDomande.FindControlRecursive(FRVDomanda, "DLIntestazioni")

        AddHandler oDatalist.ItemDataBound, AddressOf DLIntestazioni_DataBound

        SetInternazionalizzazioneDLOpzioni(e)

    End Sub
    Protected Sub DLIntestazioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLIntestazioni(e)

    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub
    Public Overrides Function HasPermessi() As Boolean

    End Function
    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetControlliByPermessi()

    End Sub
    Protected Sub salva()
        Dim returnValue As Integer = oGestioneDomande.salvaDomanda(Me.FRVDomanda)

        Select Case returnValue
            Case 0
                Me.RedirectToUrl(RootObject.QuestionarioEdit + "?type=" + Me.QuestionarioCorrente.tipo.ToString())
            Case 1
                LBErroreSalvataggio.Text = Me.Resource.getValue("ErroreOpzioni")
                LBErroreSalvataggio.Visible = True
            Case 2
                LBErroreSalvataggio.Text = Me.Resource.getValue("ErroreTestoDomanda")
                LBErroreSalvataggio.Visible = True
        End Select
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class