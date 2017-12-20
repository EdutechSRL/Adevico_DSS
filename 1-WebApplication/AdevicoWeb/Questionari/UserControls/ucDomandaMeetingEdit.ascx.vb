Imports COL_Questionario

Partial Public Class ucDomandaMeetingEdit
    Inherits BaseControlQuestionario
    Dim oPagina As New QuestionarioPagina
    Dim oGestioneDomande As New GestioneDomande
    Dim isFirstStart As Boolean = False

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
        isFirstStart = True
        caricaDati()
        bindFields()
        bindIntestazioni()
        bindZone()
        DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue = Me.DomandaCorrente.domandaRating.opzioniRating.Count
    End Sub
    Public Sub caricaDati()
        oPagina = Me.QuestionarioCorrente.pagine(0)
        If oPagina.domande.Count = 0 Then
            oPagina.domande.Add(Me.DomandaCorrente)
        End If
    End Sub
    Protected Sub bindFields()
        oGestioneDomande.bindFieldDomandaMeeting(FRVDomanda, oPagina.domande(0), oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindZone()
        oGestioneDomande.bindZoneDomandaMeeting(FRVDomanda, oPagina.domande(0), oPagina, Me.QuestionarioCorrente)
    End Sub
    Protected Sub bindIntestazioni()
        Me.DomandaCorrente.domandaRating.tipoIntestazione = DomandaRating.TipoIntestazioneRating.Testi
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ucDomandaMeetingEdit", "Questionari")

    End Sub
    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Sub SetInternazionalizzazioneFRVDomanda()
        With Me.Resource
            .setLabel(FRVDomanda.FindControl("LBZone"))
            .setLabel(FRVDomanda.FindControl("LBLPaginaCorrente"))
            .setLabel(FRVDomanda.FindControl("LBTestoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBNumeroOpzioni"))
            .setLabel(FRVDomanda.FindControl("LBTestoDopoDomanda"))
            .setRegularExpressionValidator(FRVDomanda.FindControl("REVTXBTestoDopoDomanda"))
            .setLabel(FRVDomanda.FindControl("LBAiuto"))
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
        SetInternazionalizzazioneFRVDomanda()
    End Sub
    'Protected Sub DLOpzioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

    '    Dim oDatalist As DataList

    '    oDatalist = Me.FRVDomanda.FindControl("DLIntestazioni")

    '    AddHandler oDatalist.ItemDataBound, AddressOf DLIntestazioni_DataBound

    '    SetInternazionalizzazioneDLOpzioni(e)

    'End Sub
    Protected Sub DLIntestazioni_DataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)

        SetInternazionalizzazioneDLIntestazioni(e)

    End Sub
    Public Overrides Sub BindDati()
    End Sub
    Protected Sub selezionaNumeroOpzioni(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DomandaCorrente = oGestioneDomande.setDomandaMeeting(FRVDomanda, Me.QuestionarioCorrente.pagine(0).domande(0), Me.QuestionarioCorrente)
        oGestioneDomande.selezionaNumeroOpzioni(Me.DomandaCorrente, DirectCast(FRVDomanda.FindControl("DDLNumeroOpzioni"), DropDownList).SelectedValue, Nothing)
        bindZone()
    End Sub
    Public Sub RDCLCalendar_SelectionChanged(ByVal sender As Object, ByVal e As Telerik.WebControls.Base.Calendar.Events.SelectedDatesEventArgs)
        If isFirstStart Then
            Dim RDCLCalendar As New RadCalendar
            RDCLCalendar = DirectCast(FRVDomanda.FindControl("RDCLCalendar"), RadCalendar)
            RDCLCalendar.SelectedDates.Clear()
            isFirstStart = False
        End If

    End Sub
    Public Overrides Sub SetControlliByPermessi()
    End Sub
End Class