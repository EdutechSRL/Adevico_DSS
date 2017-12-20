Imports COL_Questionario

Partial Public Class ucDomandaMultiplaEdit
    Inherits BaseControlQuestionario
    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande

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
        If Not (qs_questTypeId = Questionario.TipoQuestionario.Sondaggio AndAlso Page.IsPostBack) Then 'senza questo controllo quando cambi il numero di opzioni non memorizza i nuovi valori, se il sondaggio e' nuovo
            caricaDati()
        End If
        If Not Page.IsPostBack Then
            BindDati()
        End If

        bindFields()

        bindOpzioni()

    End Sub
    Protected Sub caricaDati()
        If Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            oPagina = Me.QuestionarioCorrente.pagine(0)
            If oPagina.domande.Count = 0 Then
                Me.DomandaCorrente = New Domanda
                Me.DomandaCorrente.tipo = Domanda.TipoDomanda.Multipla
            Else
                Me.DomandaCorrente = oPagina.domande(0)
            End If
        Else
            oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
        End If
    End Sub

    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindOpzioni()

        oGestioneDomande.bindOpzioniDomandaMultipla(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)

    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then

            oGestioneDomande.eliminaOpzioneMultipla(Me.FRVDomanda, e.Item.ItemIndex)

            bindOpzioni()
        End If

    End Sub
    Protected Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound

        oGestioneDomande.FRVDomandaDataBound(Me.FRVDomanda)

        addHandlerFRVDomanda()

        SetInternazionalizzazioneFRVDomanda()

    End Sub
    Protected Sub addHandlerFRVDomanda()
        Dim oDatalist As DataList
        oDatalist = DirectCast(GestioneDomande.FindControlRecursive(FRVDomanda, "DLOpzioni"), DataList)
        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound
    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        oGestioneDomande.DLOpzioniMultipla_DataBound(sender, e)

        SetInternazionalizzazioneDLOpzioni(e)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaMultipla(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente, False)
        Dim numeroOpzioni As Integer = DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, numeroOpzioni, Nothing)
        DirectCast(FRVDomanda.FindControl("DDLnumeroMaxRisposte"), DropDownList).Items.Clear()
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
            .setLabel(FRVDomanda.FindControl("LBLPaginaCorrente"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBnumeroMaxRisposte"))
            .setLabel(FRVDomanda.FindControl("LBnumeroMaxRisposte2"))
            .setLabel(FRVDomanda.FindControl("LBTestoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBPesoDomanda"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoInt"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoNot0"))
            .setLabel(FRVDomanda.FindControl("LBScelteMultiple"))
            .setLabel(FRVDomanda.FindControl("LBdifficolta"))
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
            .setLabel(FRVDomanda.FindControl("LBSuggerimento"))
            .setCheckBox(FRVDomanda.FindControl("CHKisValutabile"))
            .setCheckBox(FRVDomanda.FindControl("CHKisObbligatoria"))
            .setCustomValidator(FRVDomanda.FindControl("CUVvalutabile"))
            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList), item.Value)
            Next
        End With

    End Sub
    Public Sub SetInternazionalizzazioneDLOpzioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBScelta"))
            .setLabel(e.Item.FindControl("LBPeso"))
            .setLabel(e.Item.FindControl("LBsuggerimentoOpzione"))
            .setCheckBox(e.Item.FindControl("CBisAltro"))
            .setCheckBox(e.Item.FindControl("CBisCorretta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setLabel(e.Item.FindControl("LBLunghezzaOpzione"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)

        End With
    End Sub
    Protected Sub CUVvalutabile_OnServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim isValutabile As Boolean
        isValutabile = DirectCast(FRVDomanda.FindControl("CHKisValutabile"), CheckBox).Checked
        If isValutabile And Not Me.QuestionarioCorrente.tipo = Questionario.TipoQuestionario.Sondaggio Then
            Dim DLopzioni As DataList
            DLopzioni = DirectCast(FRVDomanda.FindControl("DLOpzioni"), DataList)
            args.IsValid = False
            For Each dlItem As DataListItem In DLopzioni.Items
                If DirectCast(dlItem.FindControl("CBisCorretta"), CheckBox).Checked Then
                    args.IsValid = Not (RootObject.removeBRfromStringEnd(DirectCast(dlItem.FindControl("CTRLeditorScelta"), Comunita_OnLine.UC_Editor).HTML) = String.Empty)  'True
                    Exit For
                End If
            Next
        Else
            args.IsValid = True
        End If
    End Sub

    Public Overrides Sub SetControlliByPermessi()
        If Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathActivity OrElse Me.QuestionarioCorrente.ownerType = OwnerType_enum.EduPathSubActivity Then
            Dim oLbtesto As Control = FRVDomanda.FindControl("LBTestoDopoDomanda")
            If Not oLbtesto Is Nothing Then
                oLbtesto.Visible = False
                FRVDomanda.FindControl("TXBTestoDopoDomanda").Visible = False
            End If

        End If
    End Sub
End Class