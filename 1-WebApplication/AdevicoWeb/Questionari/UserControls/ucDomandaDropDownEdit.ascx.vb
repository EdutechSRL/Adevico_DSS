Imports COL_Questionario

Partial Public Class ucDomandaDropDownEdit
    Inherits BaseControlQuestionario

    'Dim oDomanda As New Domanda
    Dim oGestioneDomande As New GestioneDomande
    'Dim oQuest As New Questionario
    Dim oPagina As New QuestionarioPagina
    'Public Shared isAperto As Boolean

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

        'If Not Me.DomandaCorrente Is Nothing Then
        '    oDomanda = Session("oDomanda")
        'End If

        caricaDati()
        bindFields()
        bindItems()

    End Sub

    Protected Sub caricaDati()
        'oQuest = Session("oQuest")
        'isAperto = Not Me.QuestionarioCorrente.isReadOnly

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
            Dim n As Integer = e.Item.ItemIndex + 1
            'oDomanda = Session("oDomanda")

            DirectCast(FRVDomanda.FindControl("DLOpzioni").Controls(e.Item.ItemIndex).FindControl("TXBScelta"), TextBox).Text = String.Empty
            Me.DomandaCorrente.domandaDropDown.dropDownItems.RemoveAt(e.Item.ItemIndex)

            bindItems()
        End If

    End Sub

    Protected Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound
        Dim oDatalist As DataList

        oDatalist = Me.FRVDomanda.FindControl("DLOpzioni")

        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound

        Dim oImg As ImageButton

        oImg = Me.FRVDomanda.FindControl("IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaDropDown, "target", "yes", "yes"))

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

        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue, Nothing)

        'Session("oDomanda") = oDomanda

        bindItems()

    End Sub

    Public Overrides Sub BindDati()

    End Sub


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaDropDownEdit", "Questionari")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        
    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(FRVDomanda.FindControl("LBLPaginaCorrente"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBTestoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBPeso"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoInt"))
            .setCompareValidator(FRVDomanda.FindControl("COVPesoNot0"))
            .setLabel(FRVDomanda.FindControl("LBEtichetta"))
            .setLabel(FRVDomanda.FindControl("LBNome"))
            .setCheckBox(FRVDomanda.FindControl("CBOrdina"))
            .setLabel(FRVDomanda.FindControl("LBScelteMultiple"))
            .setLabel(FRVDomanda.FindControl("LBdifficolta"))
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
            .setCheckBox(FRVDomanda.FindControl("CHKisValutabile"))
            .setCheckBox(FRVDomanda.FindControl("CHKisObbligatoria"))
            .setLabel(FRVDomanda.FindControl("LBSuggerimento"))

            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList), item.Value)
            Next
        End With

    End Sub
    Public Sub SetInternazionalizzazioneDLOpzioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBScelta"))
            .setLabel(e.Item.FindControl("LBPesoRisposta"))
            .setLabel(e.Item.FindControl("LBOpzioneTroppoLunga"))
            .setLabel(e.Item.FindControl("LBsuggestionOption"))
            .setCheckBox(e.Item.FindControl("CBisCorretta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBScelta"))
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
                    args.IsValid = True
                    Exit For
                End If
            Next
        Else
            args.IsValid = True
        End If
    End Sub
    Public Overrides Sub SetControlliByPermessi()
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
    End Sub
End Class