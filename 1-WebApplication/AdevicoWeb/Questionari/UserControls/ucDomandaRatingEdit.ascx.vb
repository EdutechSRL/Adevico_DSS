Imports COL_Questionario

Partial Public Class ucDomandaRatingEdit
    Inherits BaseControlQuestionario
    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    Dim isChangingTable As Boolean = False

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
        caricaDati()
        bindFields()
        bindIntestazioni()
        bindOpzioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)
    End Sub
    Public Sub caricaDati()
        'isAperto = Not Me.QuestionarioCorrente.isReadOnly

        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaRating(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindOpzioni()
        oGestioneDomande.bindOpzioniDomandaRating(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindIntestazioni()
        oGestioneDomande.bindIntestazioniDomandaRating(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True
        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)

        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue, Nothing)

        bindOpzioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)

    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        isChangingTable = True
        If e.CommandName = "elimina" Then
            Dim n As Integer = e.Item.ItemIndex + 1

            DirectCast(FRVDomanda.FindControl("DLOpzioni").Controls(e.Item.ItemIndex).FindControl("TXBTestoMin"), TextBox).Text = String.Empty
            Me.DomandaCorrente.domandaRating.opzioniRating.RemoveAt(e.Item.ItemIndex)

            bindOpzioni()
        End If
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)

    End Sub
    Protected Sub selezionaNumeroIntestazioni(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True

        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)

        oGestioneDomande.selezionaNumeroIntestazioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroColonne"), DropDownList))

        bindIntestazioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)


    End Sub
    Protected Sub selezionaTipoIntestazione(ByVal sender As Object, ByVal e As System.EventArgs)
        isChangingTable = True
        Me.DomandaCorrente = oGestioneDomande.setDomandaRating(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente)

        oGestioneDomande.selezionaNumeroIntestazioni(Me.DomandaCorrente, FRVDomanda.FindControl("DDLNumeroColonne"))

        bindIntestazioni()
        bindOpzioni()
        oGestioneDomande.creaTabellaRadioButton(FRVDomanda, isChangingTable)

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaRatingEdit", "Questionari")

    End Sub
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(FRVDomanda.FindControl("LBLPaginaCorrente"))
            .setLabel(FRVDomanda.FindControl("LBTestoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBTitoloOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBTipoIntestazione"))
            .setLabel(FRVDomanda.FindControl("LBIntestazione"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBNumeroColonne"))
            .setCheckBox(FRVDomanda.FindControl("CBmostraND"))
            .setLabel(FRVDomanda.FindControl("LBTestoND"))
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoDopoDomanda"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoND"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
            .setLabel(FRVDomanda.FindControl("LBSuggerimento"))

            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("RBLTipoIntestazione"), RadioButtonList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("RBLTipoIntestazione"), RadioButtonList), item.Value)
            Next
            .setLabel(FRVDomanda.FindControl("LBDifficolta"))
            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList), item.Value)
            Next
            .setCheckBox(FRVDomanda.FindControl("CHKisObbligatoria"))
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

        Dim oDatalist As DataList

        oDatalist = Me.FRVDomanda.FindControl("DLOpzioni")

        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound

        Dim oImg As ImageButton

        oImg = Me.FRVDomanda.FindControl("IMBHelp")

        oImg.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaRating, "target", "yes", "yes"))

        SetInternazionalizzazioneFRVDomanda()
    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        Dim oDatalist As DataList

        oDatalist = Me.FRVDomanda.FindControl("DLIntestazioni")

        AddHandler oDatalist.ItemDataBound, AddressOf DLIntestazioni_DataBound

        SetInternazionalizzazioneDLOpzioni(e)

    End Sub
    Protected Sub DLIntestazioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLIntestazioni(e)

    End Sub
    Public Overrides Sub BindDati()
    End Sub
    Public Overrides Sub SetControlliByPermessi()
    End Sub
End Class