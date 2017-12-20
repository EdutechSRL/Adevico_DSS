Imports COL_Questionario

Partial Public Class WizardDomandaDropDown
    Inherits PageBaseQuestionario
    Dim oGestioneDomande As New GestioneDomande
    Dim oPagina As New QuestionarioPagina
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
            bindItems()
        End If

        addHandlerFRVDomanda()

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
    Protected Sub caricaDati()
        'oQuest = Session("oQuest")
        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        If Me.PaginaCorrenteID > 0 Then
            oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
        End If
    End Sub
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaDropDown(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindItems()
        oGestioneDomande.bindItemsDomandaDropDown(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then

            oGestioneDomande.eliminaOpzioneDropDown(Me.FRVDomanda, e.Item.ItemIndex)
            bindItems()

        End If

    End Sub
    Protected Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound
        oGestioneDomande.FRVDomandaDataBound(Me.FRVDomanda)

        addHandlerFRVDomanda()

        SetInternazionalizzazioneFRVDomanda()
    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        Dim oCheck As CheckBox
        Dim oText As TextBox

        oCheck = e.Item.FindControl("CBisCorretta")
        oText = e.Item.FindControl("TXBPesoRisposta")
        oCheck.Attributes.Add("onclick", "changeText(this,'" & oText.ClientID & "'); return true")

        SetInternazionalizzazioneDLOpzioni(e)
    End Sub
    Protected Sub selezionaOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaDropDown(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue, Nothing)
        addHandlerFRVDomanda()
        bindItems()
    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaDropDownEdit", "Questionari")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        Master.ServiceTitle = Resource.getValue("ServiceTitle")
        With Me.Resource
            .setLinkButton(LNBSalva2, False, False)
        End With
    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBLPaginaCorrente"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBNumeroOpzioni"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTestoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBPeso"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(FRVDomanda, "COVPesoInt"))
            .setCompareValidator(GestioneDomande.FindControlRecursive(FRVDomanda, "COVPesoNot0"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBEtichetta"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBNome"))
            .setCheckBox(GestioneDomande.FindControlRecursive(FRVDomanda, "CBOrdina"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBScelteMultiple"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBdifficolta"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBTestoDopoDomanda"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBAiuto"))
            .setCheckBox(GestioneDomande.FindControlRecursive(FRVDomanda, "CHKisValutabile"))
            .setCheckBox(GestioneDomande.FindControlRecursive(FRVDomanda, "CHKisObbligatoria"))
            .setLabel(GestioneDomande.FindControlRecursive(FRVDomanda, "LBSuggerimento"))
            .setCustomValidator(GestioneDomande.FindControlRecursive(FRVDomanda, "CUVvalutabile"))
            For Each item As ListItem In DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DDLDifficolta"), DropDownList), item.Value)
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
            .setLabel(e.Item.FindControl("LBPesoRisposta"))
            .setLabel(e.Item.FindControl("LBOpzioneTroppoLunga"))
            .setCheckBox(e.Item.FindControl("CBisCorretta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBScelta"))
            .setLabel(e.Item.FindControl("LBsuggestionOption"))

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

    Protected Sub LNBSalva2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBSalva2.Click

        salva()

    End Sub

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