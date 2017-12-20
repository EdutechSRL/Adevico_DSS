Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita

Public Class IscrizioneCorsiPresenter
	Inherits GenericPresenter
	Implements IPagingPresenter





	Public Enum Filtri As Integer
		Organizzazzione = 0
		AnnoAccademico = 1
		Periodo = 2
		Referente = 3

	End Enum
	'	Private view

	Private ReadOnly Property View() As IviewIscrizioneCorsi
		Get
			View = MyBase.view
		End Get
	End Property

	Public Sub New(ByVal view As IviewListGeneric)
		MyBase.view = view
	End Sub

	Public Sub Initialize()
		View.CaricaOrganizzazioni(Me.View.UtenteCorrente.LazyOrganizationByCorsi())
		Me.View.FiltroOrganizzazioneCorrente = New FilterElement(Me.View.UtenteCorrente.ORGNDefault_id)
		View.CaricaAnnoAccademico(Me.CaricaAnno)
		Me.View.FiltroAnnoAccademicoCorrente = New FilterElement(AnnoAccademico.GetAnnoAccademicoCorrente().Anno)
		View.CaricaPeriodi(Me.CaricaPeriodo)
		Me.View.FiltroPeriodoCorrente = New FilterElement(-1)
		View.CaricaReferenti(Me.CaricaReferente)
		Me.View.FiltroReferenteCorrente = New FilterElement(-1)
		Me.ShowFirstPage()

	End Sub

#Region "Caricamento Dati"
	Private Function CaricaAnno() As GenericCollection(Of AnnoAccademico)
		Dim oLista As GenericCollection(Of AnnoAccademico)
		oLista = COL_Corso.LazyAnniAccademici(Me.View.FiltroOrganizzazioneCorrente.Value, , FiltroStatoComunita.Attiva)
		oLista.Insert(0, New AnnoAccademico(-1, "Scegli l'anno"))
		Return oLista
	End Function
	Private Function CaricaPeriodo() As GenericCollection(Of COL_Periodo)
		Dim oLista As GenericCollection(Of COL_Periodo)
		oLista = COL_Periodo.FiltersList(Me.View.LinguaID, Me.View.FiltroOrganizzazioneCorrente.Value)
		oLista.Insert(0, New COL_Periodo(-1, "Scegli il periodo"))
		Return oLista
	End Function
    Private Function CaricaReferente() As List(Of COL_Persona)
        Dim oLista As List(Of COL_Persona)
        oLista = COL_Corso.LazyResponsabili(Me.View.FiltroOrganizzazioneCorrente.Value, , Me.View.FiltroAnnoAccademicoCorrente.Value).OrderBy(Function(p) p.Anagrafica).ToList()
        oLista.Insert(0, New COL_Persona(-1, "", "Scegli il referente"))
        Return oLista
    End Function
#End Region

#Region "Navigazione"
	Private Sub ShowCurrentPage() Implements IPagingPresenter.ShowCurrentPage
		View.RefreshPageCounter(Me.View.PageIndex, Me.View.PageCount, Me.View.ItemsForPage, Me.View.ItemsCount)
		'view.SetLista()
		Me.View.RefreshRiepilogo()
	End Sub
	Public Sub ShowFirstPage() Implements IPagingPresenter.ShowFirstPage
		Me.View.PageCount = GetPageCount()
		Me.View.PageIndex = 1
		ShowCurrentPage()
	End Sub
	Public Sub ShowPreviousPage() Implements IPagingPresenter.ShowPreviousPage
		Me.View.PageCount = GetPageCount()
		If Me.View.PageIndex > 0 Then
			Me.View.PageIndex -= 1
		End If
		ShowCurrentPage()
	End Sub
	Public Sub ShowNextPage() Implements IPagingPresenter.ShowNextPage
		Me.View.PageCount = GetPageCount()
		If Me.View.PageIndex < Me.View.PageCount Then
			Me.View.PageIndex += 1
		End If
		ShowCurrentPage()
	End Sub
	Public Sub ShowLastPage() Implements IPagingPresenter.ShowLastPage
		Me.View.PageCount = GetPageCount()
		Me.View.PageIndex = Me.View.PageCount
		ShowCurrentPage()
	End Sub
	Public Sub GoToPage(ByVal IndexPage As Integer) Implements IPagingPresenter.GoToPage
		Me.View.PageCount = GetPageCount()
		If IndexPage < 0 Then
			Me.View.PageIndex = 1
		ElseIf IndexPage < Me.View.PageCount Then
			Me.View.PageIndex = IndexPage
		Else
			Me.View.PageIndex = Me.View.PageCount
		End If
		ShowCurrentPage()
	End Sub
#End Region

#Region "Filtri"
	Public Sub Applyfilters() Implements IPagingPresenter.Applyfilters
		With Me.View
			.AnnoAccademicoCorrente = .FiltroAnnoAccademicoCorrente
			.OrganizzazioneCorrente = .FiltroOrganizzazioneCorrente
			.PeriodoCorrente = .FiltroPeriodoCorrente
			.ReferenteCorrente = .FiltroReferenteCorrente
		End With
		Me.ShowCurrentPage()
	End Sub
	Public Sub Applyfilter(ByVal FilterIdentifier As Integer, Optional ByVal filter As FilterElement = Nothing) Implements IPagingPresenter.Applyfilter
		Dim AnnoAccademico, Periodo, Referente As FilterElement

		AnnoAccademico = Me.View.FiltroAnnoAccademicoCorrente
		Periodo = Me.View.FiltroPeriodoCorrente
		Referente = Me.View.FiltroReferenteCorrente

		Select Case FilterIdentifier
			Case Filtri.Organizzazzione
				'Me.View.FiltroOrganizzazioneCorrente = filter
				View.CaricaAnnoAccademico(Me.CaricaAnno)
				Me.View.FiltroAnnoAccademicoCorrente = AnnoAccademico
				View.CaricaPeriodi(Me.CaricaPeriodo)
				Me.View.FiltroPeriodoCorrente = Periodo
				View.CaricaReferenti(Me.CaricaReferente)
				Me.View.FiltroReferenteCorrente = Referente
				If Me.View.AutomaticFilterUpdate Then
					Me.View.OrganizzazioneCorrente = filter
				End If
			Case Filtri.AnnoAccademico
				View.CaricaReferenti(Me.CaricaReferente)
				Me.View.FiltroReferenteCorrente = Referente
				If Me.View.AutomaticFilterUpdate Then
					Me.View.AnnoAccademicoCorrente = filter
				End If
			Case Filtri.Periodo
				If Me.View.AutomaticFilterUpdate Then
					Me.View.PeriodoCorrente = filter
				End If
			Case Filtri.Referente
				If Me.View.AutomaticFilterUpdate Then
					Me.View.ReferenteCorrente = filter
				End If
		End Select
		Me.ShowFirstPage()
	End Sub
	Public Sub RemoveFilter(ByVal FilterIdentifier As Integer) Implements IPagingPresenter.RemoveFilter
		Dim AnnoAccademico, Periodo, Referente As FilterElement

		AnnoAccademico = Me.View.FiltroAnnoAccademicoCorrente
		Periodo = Me.View.FiltroPeriodoCorrente
		Referente = Me.View.FiltroReferenteCorrente

		Select Case FilterIdentifier
			Case Filtri.Organizzazzione
				If Me.View.AutomaticFilterUpdate Then
					Me.View.FiltroOrganizzazioneCorrente = New FilterElement(-1)
				Else
					Me.View.OrganizzazioneCorrente = Nothing
				End If
			Case Filtri.AnnoAccademico
				If Me.View.AutomaticFilterUpdate Then
					Me.View.FiltroAnnoAccademicoCorrente = New FilterElement(-1)
				Else
					Me.View.AnnoAccademicoCorrente = Nothing
				End If
			Case Filtri.Periodo
				If Me.View.AutomaticFilterUpdate Then
					Me.View.FiltroPeriodoCorrente = New FilterElement(-1)
				Else
					Me.View.PeriodoCorrente = Nothing
				End If
			Case Filtri.Referente
				If Me.View.AutomaticFilterUpdate Then
					Me.View.FiltroReferenteCorrente = New FilterElement(-1)
				Else
					Me.View.ReferenteCorrente = Nothing
				End If
		End Select
		Me.ShowFirstPage()
	End Sub


	Public Sub ChangeRecordnumber(ByVal Recordnumber As Integer) Implements IPagingPresenter.ChangeRecordnumber
		Me.View.PageCount = GetPageCount()
		If Me.View.PageIndex > Me.View.PageCount Then
			Me.View.PageIndex = Me.View.PageCount
		End If
		ShowCurrentPage()
	End Sub

	Private Function GetPageCount() As Integer
		Dim ItemCount As Integer
		Dim PageCount As Integer

		Try
			'ItemCount = Me.ItemCount
			PageCount = ItemCount / Me.View.ItemsForPage
			If ItemCount Mod Me.View.ItemsForPage <> 0 Then
				PageCount += 1
			End If
		Catch ex As Exception

		End Try
	End Function

	Public Sub QuickSearch(ByVal elemento As String, ByVal Indice As Integer) Implements IPagingPresenter.QuickSearch

	End Sub

	Public Sub SetQuickSearchElement() Implements IPagingPresenter.SetQuickSearchElement

	End Sub

	Public Sub ShowByLetter(ByVal Lettera As COL_BusinessLogic_v2.Main.FiltroComunita) Implements IPagingPresenter.ShowByLetter

	End Sub
#End Region




End Class
