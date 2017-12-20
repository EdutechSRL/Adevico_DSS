Imports COL_Questionario

Partial Public Class WizardDomandaMultipla
    Inherits PageBaseQuestionario

    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    Public Shared isAperto As Boolean
    Dim idDomanda As String
    Dim idPagina As String
    Dim idQuestionario As String
    Dim oDomanda As New Domanda

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

            bindOpzioni()

        End If

        addHandlerFRVDomanda()
    End Sub

    Protected Sub caricaDati()
        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub

    Protected Sub bindFields()

        oGestioneDomande.bindFieldDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)


    End Sub
    Protected Sub bindOpzioni()

        oGestioneDomande.bindOpzioniDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)

    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        If e.CommandName = "elimina" Then
            Me.DomandaCorrente.domandaMultiplaOpzioni.RemoveAt(e.Item.ItemIndex)
            bindOpzioni()
        End If
    End Sub
    Protected Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound

        oGestioneDomande.FRVDomandaDataBound(Me.FRVDomanda)

        addHandlerFRVDomanda()

        SetInternazionalizzazioneFRVDomanda()

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
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        oGestioneDomande.DLOpzioniMultipla_DataBound(sender, e)

        Dim editor As Comunita_OnLine.UC_Editor = DirectCast(e.Item.FindControl("CTRLeditorScelta"), Comunita_OnLine.UC_Editor)
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            editor.HTML = Me.DomandaCorrente.domandaMultiplaOpzioni.Item(e.Item.ItemIndex).testo
        End If
        DirectCast(e.Item.FindControl("TXBPeso"), TextBox).Text = Me.DomandaCorrente.domandaMultiplaOpzioni.Item(e.Item.ItemIndex).peso
        DirectCast(e.Item.FindControl("CBisAltro"), CheckBox).Checked = Me.DomandaCorrente.domandaMultiplaOpzioni.Item(e.Item.ItemIndex).isAltro
        DirectCast(e.Item.FindControl("CBisCorretta"), CheckBox).Checked = Me.DomandaCorrente.domandaMultiplaOpzioni.Item(e.Item.ItemIndex).isCorretta
        SetInternazionalizzazioneDLOpzioni(e)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaMultipla(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente, False)
        Dim numeroOpzioni As Integer = DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, numeroOpzioni, Nothing)
        DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items.Clear()
        addHandlerFRVDomanda()
        bindOpzioni()
    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaMultiplaEdit", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        Master.ServiceTitle = Resource.getValue("ServiceTitle")
        'With Me.Resource
        '    .setLinkButton(LNBSalva2, False, False)
        'End With
    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBLPaginaCorrente"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBNumeroOpzioni"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBnumeroMaxRisposte"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBnumeroMaxRisposte2"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBTestoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBPesoDomanda"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "COVPesoInt"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "COVPesoNot0"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBScelteMultiple"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBdifficolta"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBTestoDopoDomanda"))
            .setRegularExpressionValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "REVTXBTestoDopoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBAiuto"))
            .setLabel(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "LBSuggerimento"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisValutabile"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisObbligatoria"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisObbligatoria"))
            .setCustomValidator(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CUVvalutabile"))
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
            .setLabel(e.Item.FindControl("LBScelta"))
            .setLabel(e.Item.FindControl("LBPeso"))
            .setCheckBox(e.Item.FindControl("CBisAltro"))
            .setCheckBox(e.Item.FindControl("CBisCorretta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setLabel(e.Item.FindControl("LBLunghezzaOpzione"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
        End With
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
    'Protected Sub LNBSalva2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva2.Click

    '    salva()

    'End Sub
    Protected Sub salva()
        'Dim returnValue As Integer = oGestioneDomande.salvaDomanda(Me.FRVDomanda)
        Dim returnValue As Integer

        If DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CUVvalutabile"), CustomValidator).IsValid Then
            returnValue = oGestioneDomande.salvaDomanda(Me.FRVDomanda)
        Else
            returnValue = 3
        End If

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
    Protected Sub CUVvalutabile_OnServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        'If DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "WIZDomanda"), Wizard).ActiveStepIndex = 2 Then
        Dim isValutabile As Boolean
        isValutabile = DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisValutabile"), CheckBox).Checked
        If isValutabile Then
            Dim DLopzioni As DataList
            DLopzioni = DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DLOpzioni"), DataList)
            args.IsValid = False
            For Each dlItem As DataListItem In DLopzioni.Items
                If DirectCast(dlItem.FindControl("CBisCorretta"), CheckBox).Checked Then
                    args.IsValid = True
                    Exit For
                End If
            Next
        Else
            args.IsValid = True
        End If
        'End If
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class