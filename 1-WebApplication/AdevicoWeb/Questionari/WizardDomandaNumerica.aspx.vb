Imports COL_Questionario

Partial Public Class WizardDomandaNumerica
    Inherits PageBaseQuestionario

    Dim oDomanda As New Domanda
    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    Public Shared isAperto As Boolean

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
        End If
        addHandlerFRVDomanda()
    End Sub
    Public Sub caricaDati()
        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)

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
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaNumerica(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaNumerica(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue, GestioneDomande.FindControlRecursive(FRVDomanda, "DLopzioni"))
        oGestioneDomande.bindOpzioniDomandaNumerica(FRVDomanda, Me.DomandaCorrente)
    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then

            oGestioneDomande.eliminaOpzioneNumerica(FRVDomanda, e.Item.ItemIndex)

            bindFields()
        End If

    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBLPaginaCorrente"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBTestoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBPeso"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "COVPesoInt"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "COVPesoNot0"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBNumeroOpzioni"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBdifficolta"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBTestoDopoDomanda"))
            .setRegularExpressionValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "REVTXBTestoDopoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBAiuto"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBSuggerimento"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisObbligatoria"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisValutabile"))
            For Each item As ListItem In DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLDifficolta"), DropDownList), item.Value)
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
            .setLabel(e.Item.FindControl("LBLRispostaCorretta"))
            .setLabel(e.Item.FindControl("LBLTestoPrima"))
            .setLabel(e.Item.FindControl("LBLTestoDopo"))
            .setLabel(e.Item.FindControl("LBLDimensione"))
            .setCompareValidator(e.Item.FindControl("COVRispostaNumerica_isDouble"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoPrima"))
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBTestoDopo"))
            .setLabel(e.Item.FindControl("LBPesoRisposta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))

        End With
    End Sub
    Private Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound

        oGestioneDomande.FRVDomandaDataBound(Me.FRVDomanda)

        addHandlerFRVDomanda()

        SetInternazionalizzazioneFRVDomanda()

    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLOpzioni(e)

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
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaNumericaEdit", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        Master.ServiceTitle = Resource.getValue("ServiceTitle")
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