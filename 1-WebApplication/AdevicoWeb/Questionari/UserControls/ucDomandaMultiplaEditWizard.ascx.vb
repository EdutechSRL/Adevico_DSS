Imports COL_Questionario

Partial Public Class ucDomandaMultiplaEditWizard
    Inherits BaseControlQuestionario
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        caricaDati()

        bindFields()

        bindOpzioni()

        'If Not Page.IsPostBack Then

        '	oGestioneDomande.addRadToolbarButton(DirectCast(FRVDomanda.FindControl("TXBTestoDomanda"), RadEditor))

        'End If

    End Sub

    Protected Sub caricaDati()
        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub

    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub

    Protected Sub bindOpzioni()
        Dim counter As Int32
        Dim nOpzioni As Integer = Me.DomandaCorrente.domandaMultiplaOpzioni.Count
        ' se non ho opzioni (domanda nuova) allora inserisco quelle di default (3)
        If nOpzioni = 0 Then
            nOpzioni = 3
        End If
        For counter = 0 To nOpzioni
            DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items.Add(counter)
        Next
        DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items(0).Text = Me.Resource.getValue(DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList), 0)
        oGestioneDomande.bindOpzioniDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)

    End Sub

    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then
            Dim n As Integer = e.Item.ItemIndex + 1

            DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DLOpzioni").Controls(e.Item.ItemIndex).FindControl("TXBScelta"), TextBox).Text = String.Empty
            Me.DomandaCorrente.domandaMultiplaOpzioni.RemoveAt(e.Item.ItemIndex)

            bindOpzioni()
        End If

    End Sub

    Protected Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound
        Dim oDatalist As DataList

        oDatalist = GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DLOpzioni")

        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound

        Dim oWizard As Wizard

        oWizard = GestioneDomande.FindControlRecursive(Me.FRVDomanda, "WIZDomandaMultipla")

        AddHandler oWizard.NextButtonClick, AddressOf WIZDomandaMultipla_NextButtonClick

        Dim oImg As System.Web.UI.WebControls.ImageButton

        oImg = GestioneDomande.FindControlRecursive(Me.FRVDomanda, "IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaMultipla, "target", "yes", "yes"))

        SetInternazionalizzazioneFRVDomanda()

    End Sub

    Protected Sub WIZDomandaMultipla_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs)

        Dim aa As Object = GestioneDomande.FindControlRecursive(FRVDomanda, "CTRLeditorTestoDomanda")



    End Sub

    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        Dim oCheck As CheckBox
        Dim oCheckIsAltro As CheckBox
        Dim oText As TextBox

        oCheck = e.Item.FindControl("CBisCorretta")
        oCheckIsAltro = e.Item.FindControl("CBisAltro")
        oText = e.Item.FindControl("TXBPeso")
        oCheck.Attributes.Add("onclick", "changeText(this,'" & oText.ClientID & "'); return true")

        SetInternazionalizzazioneDLOpzioni(e)

    End Sub

    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.DomandaCorrente = oGestioneDomande.setDomandaMultipla(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        Dim numeroOpzioni As Integer = DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLNumeroOpzioni"), DropDownList).SelectedValue
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, numeroOpzioni, Nothing)
        DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLnumeroMaxRisposte"), DropDownList).Items.Clear()
        bindOpzioni()

    End Sub


    Public Overrides Sub BindDati()

    End Sub


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaMultiplaEdit", "Questionari")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

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
            For Each item As ListItem In DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "DDLDifficolta"), DropDownList), item.Value)
            Next
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
    Public Overrides Sub SetControlliByPermessi()
        If Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            FRVDomanda.FindControl("LBTestoDopoDomanda").Visible = False
            FRVDomanda.FindControl("TXBTestoDopoDomanda").Visible = False
        End If
    End Sub
End Class