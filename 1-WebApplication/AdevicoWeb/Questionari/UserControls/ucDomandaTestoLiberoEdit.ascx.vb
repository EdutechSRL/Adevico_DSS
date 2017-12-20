Imports COL_Questionario

Partial Public Class ucDomandaTestoLiberoEdit
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

        isAperto = Not Me.QuestionarioCorrente.isReadOnly
        oPagina = QuestionarioPagina.findPaginaBYID(Me.QuestionarioCorrente.pagine, Me.PaginaCorrenteID)
    End Sub
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaTestoLibero(FRVDomanda, Me.DomandaCorrente, oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.DomandaCorrente = oGestioneDomande.setDomandaTestoLibero(FRVDomanda, Me.DomandaCorrente, Me.QuestionarioCorrente, False)

        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue, FRVDomanda.FindControl("DLopzioni"))

        bindFields()

    End Sub
    Protected Sub eliminaOpzione(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

        If e.CommandName = "elimina" Then
            Dim n As Integer = e.Item.ItemIndex + 1

            DirectCast(FRVDomanda.FindControl("DLOpzioni").Controls(e.Item.ItemIndex).FindControl("TXBEtichetta"), TextBox).Text = String.Empty
            Me.DomandaCorrente.opzioniTestoLibero.RemoveAt(e.Item.ItemIndex)

            bindFields()
        End If

    End Sub
    Public Overrides Sub BindDati()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaTestoLiberoEdit", "Questionari")
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
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBdifficolta"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
            .setLabel(FRVDomanda.FindControl("LBSuggerimento"))
            .setCheckBox(GestioneDomande.FindControlRecursive(Me.FRVDomanda, "CHKisValutabile"))
            .setCheckBox(FRVDomanda.FindControl("CHKisObbligatoria"))
            For Each item As ListItem In DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList).Items
                item.Text = .getValue(DirectCast(FRVDomanda.FindControl("DDLDifficolta"), DropDownList), item.Value)
            Next
        End With
    End Sub
    Public Sub SetInternazionalizzazioneDLOpzioni(ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        With Me.Resource
            .setLabel(e.Item.FindControl("LBLEtichetta"))
            .setLabel(e.Item.FindControl("LBPesoRisposta"))
            .setImageButton(e.Item.FindControl("IMBElimina"), False, True, True, False)
            .setRegularExpressionValidator(e.Item.FindControl("REVTXBEtichetta"))
            .setCompareValidator(e.Item.FindControl("COVPesoIntOpzioni"))
            .setRangeValidator(e.Item.FindControl("RVPesoMin100"))
        End With
    End Sub
    Private Sub FRVDomanda_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FRVDomanda.DataBound
        Dim oDatalist As DataList
        oDatalist = Me.FRVDomanda.FindControl("DLOpzioni")
        AddHandler oDatalist.ItemDataBound, AddressOf DLOpzioni_DataBound
        Dim oHyperlink As HyperLink = Me.FRVDomanda.FindControl("HYPhelp")
        oHyperlink.Attributes.Add("onclick", RootObject.apriPopUp(RootObject.helpDomandaTestoLibero, "target", "yes", "yes"))
        SetInternazionalizzazioneFRVDomanda()
    End Sub
    Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        oGestioneDomande.DLOpzioniFreeText_DataBound(sender, e)
        SetInternazionalizzazioneDLOpzioni(e)
    End Sub
    Public Overrides Sub SetControlliByPermessi()
        If Not Me.QuestionarioCorrente.ownerType = OwnerType_enum.None Then
            Dim oLabel As Label = FRVDomanda.FindControl("LBTestoDopoDomanda")
            If Not IsNothing(oLabel) Then
                oLabel.Visible = False
            End If

            Dim oTextBox As TextBox = FRVDomanda.FindControl("TXBTestoDopoDomanda")

            If Not IsNothing(oTextBox) Then
                oTextBox.Visible = False
            End If

        End If
    End Sub
End Class