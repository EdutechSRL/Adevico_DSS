Imports COL_Questionario

Partial Public Class ucDomandaNumericaEdit
	Inherits BaseControlQuestionario

    'Dim idQuestionario As String
    'Dim oDomanda As New Domanda
    'Dim oQuest As New Questionario
    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    'Public Shared isAperto As Boolean

    Public ReadOnly Property visibilityValutazione() As String
        Get
            Select Case Me.QuestionarioCorrente.tipo
                Case Questionario.TipoQuestionario.Questionario
                    Return "inline"
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    Return "inline"
                Case Else
                    Return "none"
            End Select
        End Get
    End Property

    Public ReadOnly Property isDomandaReadOnly() As Boolean
        Get
            If Me.DomandaCorrente.isReadOnly Then
                Return Me.DomandaCorrente.isReadOnly
            Else
                Return Me.QuestionarioCorrente.isReadOnly
            End If
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        caricaDati()

        bindFields()

    End Sub

    Public Sub caricaDati()
        'isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaNumerica(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaNumerica(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue, FRVDomanda.FindControl("DLopzioni"))
        bindFields()
    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then
            Dim n As Integer = e.Item.ItemIndex + 1

            Dim editor As Comunita_OnLine.UC_Editor = DirectCast(FRVDomanda.FindControl("DLOpzioni").Controls(e.Item.ItemIndex).FindControl("CTRLeditorTestoPrima"), Comunita_OnLine.UC_Editor)

            If Not IsNothing(editor) Then
                editor.HTML = ""
            End If
            Me.DomandaCorrente.opzioniNumerica.RemoveAt(e.Item.ItemIndex)
            bindFields()
        End If

    End Sub


    Public Overrides Sub BindDati()

    End Sub


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaNumericaEdit", "Questionari")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(FRVDomanda.FindControl("LBLPaginaCorrente"))
            .setLabel(FRVDomanda.FindControl("LBTestoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBPeso"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoInt"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoNot0"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBdifficolta"))
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
            .setLabel(FRVDomanda.FindControl("LBSuggerimento"))
            .setCheckBox(FRVDomanda.FindControl("CHKisObbligatoria"))
            .setCheckBox(FRVDomanda.FindControl("CHKisValutabile"))
            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList), item.Value)
            Next
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
        Dim oDatalist As DataList
        oDatalist = Me.FRVDomanda.FindControl("DLOpzioni")
        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound

        Dim oImg As ImageButton

        oImg = Me.FRVDomanda.FindControl("IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaNumerica, "target", "yes", "yes"))

        SetInternazionalizzazioneFRVDomanda()

    End Sub

    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLOpzioni(e)

        Dim editor As Comunita_OnLine.UC_Editor = e.Item.FindControl("CTRLeditorTestoPrima")
        If Not IsNothing(editor) Then
            If Not editor.isInitialized Then
                editor.InitializeControl(COL_Questionario.ModuleQuestionnaire.UniqueID)
            End If
            'editor.HTML = oDomanda.testo
        End If
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If Not IsNothing(Me.QuestionarioCorrente) Then
            If Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
                Dim oLabel As Label = FRVDomanda.FindControl("LBTestoDopoDomanda")
                If Not IsNothing(oLabel) Then
                    oLabel.Visible = False
                End If
                Dim oTextbox As TextBox = FRVDomanda.FindControl("TXBTestoDopoDomanda")
                If Not IsNothing(oTextbox) Then
                    oTextbox.Visible = False
                End If
            End If
        End If
        
    End Sub
End Class