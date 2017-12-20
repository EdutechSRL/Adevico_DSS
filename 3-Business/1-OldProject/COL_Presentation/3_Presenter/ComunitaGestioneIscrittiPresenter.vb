Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.Comunita

Public Class ComunitaGestioneIscrittiPresenter
    Inherits GenericPresenter

    Public Sub New(ByVal view As IviewListGeneric)
        MyBase.view = view
    End Sub

    Private Shadows ReadOnly Property View() As IviewComunitaGestioneIScritti
        Get
            View = MyBase.view
        End Get
    End Property

    Public Sub Initialize()
        view.ShowMessageToPage("") 'svuota eventuali messaggi...
        View.CaricaRuoli(Me.caricaRuoli)
        'View.CaricaOrganizzazioni(Me.View.UtenteCorrente.LazyOrganizationByCorsi())
        'Me.View.FiltroOrganizzazioneCorrente = New FilterElement(Me.View.UtenteCorrente.ORGNDefault_id)
        'View.CaricaAnnoAccademico(Me.CaricaAnno)
        'Me.View.FiltroAnnoAccademicoCorrente = New FilterElement(AnnoAccademico.GetAnnoAccademicoCorrente().Anno)
        'View.CaricaPeriodi(Me.CaricaPeriodo)
        'Me.View.FiltroPeriodoCorrente = New FilterElement(-1)
        'View.CaricaReferenti(Me.CaricaReferente)
        'Me.View.FiltroReferenteCorrente = New FilterElement(-1)
        'Me.ShowFirstPage()
        Me.GotoPage(0)
        'Me.bindDatiTest()


    End Sub

    Private Sub ShowFirstPage()
        'Me.View.PageCount = GetPageCount()
        'Me.View.PageIndex = 1
        'ShowCurrentPage()
    End Sub

#Region "Bind Dati"
	Private Function caricaRuoli() As GenericCollection(Of Role)
		Dim oComunita As New COL_Comunita(View.ComunitaCorrenteID)
		'oComunita.Id = 
		Dim oLista As GenericCollection(Of Role)
		oLista = oComunita.RuoliAssociabiliByPersonaCol(View.UtenteCorrente.ID, Main.FiltroRuoli.ForTipoComunita_NoGuest)
		'RuoliAssociati
		oLista.Insert(0, New Role(-1, "Tutti", 0))
		Return oLista
	End Function

    'Public Function RuoliAssociati(ByVal LinguaID As Integer, ByVal oFiltro As FiltroRuoli, Optional ByVal oFiltroUtenti As FiltroUtenti = Main.FiltroUtenti.NoPassantiNoCreatori, Optional ByVal EsclusoUtenteID As Integer = 0) As DataSet
    '    'elenca in un dataset i ruoli che sono assegnati ad una comunità
    '    Dim oRequest As New COL_Request
    '    Dim oParam As New COL_Request.Parameter
    '    Dim oTable As New DataSet
    '    Dim objAccesso As New COL_DataAccess

    '    With oRequest
    '        .Command = "sp_Comunita_ElencaRuoliAssociati"
    '        .CommandType = CommandType.StoredProcedure
    '        oParam = objAccesso.GetParameter("@CMNT_Id", Me.n_CMNT_id, ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)

    '        oParam = objAccesso.GetAdvancedParameter("@Filtro", CType(oFiltro, FiltroRuoli), ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)

    '        oParam = objAccesso.GetAdvancedParameter("@FiltroUtente", CType(oFiltroUtenti, FiltroUtenti), ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)

    '        oParam = objAccesso.GetAdvancedParameter("@LinguaID", LinguaID, ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)

    '        oParam = objAccesso.GetAdvancedParameter("@EsclusoUtenteID", EsclusoUtenteID, ParameterDirection.Input, SqlDbType.Int)
    '        .Parameters.Add(oParam)

    '        .Role = COL_Request.UserRole.Admin
    '        .transactional = False
    '    End With
    '    Try
    '        oTable = objAccesso.GetdataSet(oRequest)
    '    Catch ex As Exception
    '        Me.n_Errore = Errori_Db.DBError
    '    End Try
    '    Return oTable
    'End Function

    'Private Function CaricaGriglia() As GenericCollection(Of COL_Persona)

    'End Function

    Public Sub GotoPage(ByVal PageNum As Integer)
        Dim Totale As Integer

        Dim oIscritti As New System.Collections.Generic.List(Of Iscritto)
        oIscritti = COL_Comunita.GetIscrittiLazy(Totale, _
                View.ComunitaCorrenteID, _
                View.LinguaID, _
                View.UtenteCorrente.Id, _
                Me.View.oFiltroAbilitazione, _
                FiltroUtenti.Tutti, _
                Me.View.TipoRuoloId, _
                Me.View.ValoreRicerca, _
                Me.View.oAnagrafica, _
                False, Me.View.oFiltroRicercaAnagrafica)

        Me.View.ItemsCount = Totale
        Me.View.ItemsForPage = Me.View.ItemsForPage

        If PageNum > (Totale / Me.View.ItemsForPage) + 1 Then
            PageNum = 0
        End If

        Me.View.PageIndex = PageNum
        Me.View.SetLista(Me.Paginazione(oIscritti, Me.View.ItemsForPage, PageNum))
        Me.View.ShowGriglia()
    End Sub

    Public Function Deiscrivi(ByVal IdPersona As Integer)
        'DEiscrizione
        Me.View.ShowMessageToPage(IdPersona.ToString)
        Me.View.ShowAlertMessage(IdPersona.ToString)
    End Function

    Public Function Modifica(ByVal IdIscritto As Integer)
        Dim oIscritto As Iscritto = COL_Comunita.GetIscrittoLazyByIscritto(IdIscritto, Me.View.ComunitaCorrenteID, Me.View.LinguaID)
        With Me.View
            .Iscritto_Anagrafica = oIscritto.Persona.Anagrafica
            .Iscritto_Id = IdIscritto
            .Iscritto_IdRuolo = oIscritto.Ruolo.Id
            .Iscritto_IsResponsabile = oIscritto.RLPC_Responsabile
            .ShowModifica()
        End With


        'Me.View.ShowMessageToPage(IdPersona.ToString)
        'Me.View.ShowAlertMessage(IdPersona.ToString)
    End Function

    Private Function Paginazione(ByVal oIscritti As System.Collections.Generic.List(Of Iscritto), ByVal RecordPerPage As Integer, ByVal PageIndex As Integer)
        Dim RowIndex As Integer
        RowIndex = RecordPerPage * PageIndex
        Return oIscritti.GetRange(RowIndex, RecordPerPage)
    End Function
#End Region
End Class