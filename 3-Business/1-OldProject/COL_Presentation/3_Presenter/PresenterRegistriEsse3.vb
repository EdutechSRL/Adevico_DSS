Imports Comol.Entity.ModuloEsse3
Imports Comol.Entity.Events
Imports Comol.Entity.ModuloEsse3.OffertaFormativa
Imports Comol.Entity.ModuloEsse3.Registro

Imports COL_BusinessLogic_v2.Comol.Manager.Esse3

Public Class PresenterRegistriEsse3
	Inherits GenericPresenter

	Private _Manager As iManagerRegistriEsse3
	Private _ManagerEvent As iManagerAdvanced
	Private _RegisterEsse3 As RegistroEsse3


	Public Sub New(ByVal view As IviewRegistriEsse3)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewRegistriEsse3
		Get
			View = MyBase.view
		End Get
	End Property
	Private ReadOnly Property Manager() As iManagerRegistriEsse3
		Get
			If IsNothing(_Manager) Then
				_Manager = FactoryEsse3.CreateManager(ConnectionType.SQL, Me.View.CurrentTheacher, Nothing, True)
			End If
			Return _Manager
		End Get
	End Property
	Private ReadOnly Property EventManager() As ManagerEvents
		Get
			If IsNothing(_ManagerEvent) Then
				_ManagerEvent = New ManagerEvents(Me.View.CurrentTheacher, Me.View.CommunityToImport, Me.View.CurrentLanguage)
			End If
			Return _ManagerEvent
		End Get
	End Property
	Private ReadOnly Property CurrentRegister(ByVal RegistroID As Int64) As RegistroEsse3
		Get
			If IsNothing(_RegisterEsse3) Then
				_RegisterEsse3 = Manager.getRegistro(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, RegistroID)
			End If
			Return _RegisterEsse3
		End Get
	End Property

	Public Sub Init()
		If Not IsNothing(Manager) Then
			Me.View.GridCurrentPage = 1
			Me.View.Direction = Comol.Entity.sortDirection.Ascending
			Me.View.OrderBy = "AttivitaDidattica.CompleteName"
			Me.RefreshFacolta()
			Me.RefreshAnniAccademici()
			Me.View.Lastupdate = Me.Manager.UltimoAggiornamento.LastUpdateAt
		End If
	End Sub

	Public Sub RefreshFacolta()
		Dim oLista As IList = Manager.ListFacolta
		If oLista.Count = 0 Then
			' segnalare errore mancanza facoltà
		ElseIf oLista.Count > 1 Then
			'				me.View.getAllFacultyString
		End If
		Me.View.CaricaFacolta(oLista)
	End Sub
	Public Sub RefreshAnniAccademici()
		Dim oListaAnni As IList = Manager.ListAnniAccademici(Me.View.FacoltaCorrente)
		If oListaAnni.Count = 0 Then
			' segnalare errore mancanza facoltà
		ElseIf oListaAnni.Count > 1 Then
			'				me.View.getAllFacultyString
		End If
		Me.View.CaricaAnniAccademici(oListaAnni)
		Me.RefreshRegistri()
	End Sub
	Public Sub RefreshRegistri()
		Me.GoToPage(1)
	End Sub

	Public Sub GoToPage(ByVal PageIndex As Integer)
		Dim oLista As IList = Manager.ListRegistri(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, Me.View.OrderBy, Me.View.Direction)
		If oLista.Count = 0 Then
			Me.View.GridCurrentPage = 1
		Else
			Me.View.GridMaxPage = Math.Ceiling(oLista.Count / Me.View.GridPageSize)
			If PageIndex > Me.View.GridMaxPage Then
				PageIndex = Me.View.GridMaxPage
			End If
			Me.View.GridCurrentPage = PageIndex
		End If

		Me.View.CaricaRegistri(oLista)
	End Sub
	Public Sub LoadRegistroSemplificato(ByVal RegistroID As Int64)
		Dim oRegistro As RegistroEsse3 = Manager.getRegistro(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, RegistroID)

		Me.View.CaricaRegistro(oRegistro)
		If Not IsNothing(oRegistro) Then
			Me.View.CaricaDettagliSemplificati(oRegistro.Dettagli)
		End If
	End Sub

	Public Sub LoadRegistroEsteso(ByVal RegistroID As Int64)
		Dim oRegistro As RegistroEsse3 = Manager.getRegistro(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, RegistroID)

		Me.View.CaricaRegistro(oRegistro)
		If Not IsNothing(oRegistro) Then
			Me.View.CaricaDettagliEstesi(oRegistro.Dettagli)
		End If
	End Sub


	Public Function CompresenzeToString(ByVal oLista As List(Of Docente_Esse3)) As String
		Dim iResponse As String = ""
		Dim Index As Integer = 0

		For Each oCompresenza As Docente_Esse3 In oLista
			If Index = 0 Then
				iResponse = oCompresenza.NameAndSurname
				Index += 1
			Else
				iResponse &= "," & oCompresenza.NameAndSurname
			End If
		Next
		Return iResponse
	End Function
	Public Function GruppiToString(ByVal oLista As List(Of GruppoLavoro_Esse3)) As String
		Dim iResponse As String = ""
		Dim Index As Integer = 0

		For Each oGruppo As GruppoLavoro_Esse3 In oLista
			If Index = 0 Then
				iResponse = oGruppo.Name
				Index += 1
			Else
				iResponse &= "," & oGruppo.Name
			End If
		Next
		Return iResponse
	End Function

	Public Sub LoadEventsForSimpleImport()
		Dim oRegistro As RegistroEsse3 = Manager.getRegistro(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, Me.View.CurrentRegistro)

		If Not IsNothing(oRegistro) Then
			Dim oSelectedItems As IList(Of Int64) = Me.View.SelectedEventsToImport
			Me.View.LoadEsse3DetailsForSelect(oRegistro.Dettagli)
			Me.View.SelectEsse3Events(oSelectedItems)
			Me.View.EnableInvertSelection = oRegistro.Dettagli.Count > 1
			Me.View.EnableSelectAll = Me.View.EnableInvertSelection
			Me.View.EnableUnselectAll = Me.View.EnableInvertSelection
		End If
	End Sub

	Public Sub LoadEventsForVerboseImport()
		Dim oRegistro As RegistroEsse3 = Manager.getRegistro(Me.View.FacoltaCorrente, Me.View.AnnoAccademicoCorrente, Me.View.CurrentRegistro)

		If Not IsNothing(oRegistro) Then
			Dim oSelectedItems As IList(Of Int64) = Me.View.SelectedEventsToImport
			Me.View.LoadVerboseEsse3DetailsForSelect(oRegistro.Dettagli)
			Me.View.SelectEsse3Events(oSelectedItems)
			Me.View.EnableInvertSelection = oRegistro.Dettagli.Count > 1
			Me.View.EnableSelectAll = Me.View.EnableInvertSelection
			Me.View.EnableUnselectAll = Me.View.EnableInvertSelection
		End If
	End Sub

	Public Sub FindConflictEvents(ByVal CommunityID As Integer)
		Dim oEventsManager As ManagerEvents = Me.EventManager

		Dim oImportedList As List(Of CommunityEvent) = oEventsManager.GetEsse3Events
		If oImportedList.Count = 0 Then
			Me.View.ShowConfirmImport()
		Else
			Dim oRegister As RegistroEsse3 = Me.CurrentRegister(Me.View.CurrentRegistro)
			Dim oSelectedItems As IList(Of Int64) = Me.View.SelectedEventsToImport
			Dim oConflicts As New List(Of EventConflict)

			For Each oEvent As CommunityEvent In oImportedList
				If oSelectedItems.Contains(oEvent.ExternalPlannerID) Then
					Dim oDetail As New DettaglioRegistro_Esse3
					oDetail.ID = oEvent.ExternalPlannerID

					oDetail = oRegister.Dettagli.Find(New GenericPredicate(Of DettaglioRegistro_Esse3, Long)(CLng(oEvent.ExternalPlannerID), AddressOf DettaglioRegistro_Esse3.FindByID))
					If Not IsNothing(oDetail) Then
						oConflicts.Add(New EventConflict(oEvent, oDetail))
					End If
				End If
			Next
			If oConflicts.Count > 0 Then
				oConflicts.Sort(New GenericComparer(Of EventConflict)("ExternalEvent.DataInizioCompleta"))
				Me.View.ShowConflicts(oConflicts)
			Else
				Me.View.ShowConfirmImport()
			End If
		End If
	End Sub

	Public Sub ImportEsse3Events()
		Dim oEventsManager As ManagerEvents = Me.EventManager
		Dim oAvailableDetails As New List(Of DettaglioRegistro_Esse3)
		Dim oSelectedItems As List(Of Int64) = Me.View.SelectedEventsToImport
		Dim oConflicts As List(Of EventConflict) = Me.View.ConflictsSolution
		Dim oEventErrors As New List(Of DettaglioRegistro_Esse3)

		oAvailableDetails = Me.CurrentRegister(Me.View.CurrentRegistro).Dettagli

		If oConflicts.Count > 0 Then
			For Each oConflict As EventConflict In oConflicts
				oSelectedItems.Remove(DirectCast(oConflict.ExternalEvent, DettaglioRegistro_Esse3).ID)
			Next
		End If
		For Each oID As Long In oSelectedItems
			Dim oDetails As DettaglioRegistro_Esse3 = oAvailableDetails.Find(New GenericPredicate(Of DettaglioRegistro_Esse3, Long)(oID, AddressOf DettaglioRegistro_Esse3.FindByID))
			If Not IsNothing(oDetails) Then
				Dim oEvent As CommunityEvent = CreateCommunityEventFromEsse3(oDetails)
				oEventsManager.PersistEventToDB(oEvent)
				If oEvent.ID = 0 Then
					oEventErrors.Add(oDetails)
				End If
			End If
		Next
		Dim SolvedConflict As Integer = 0
		For Each oConflict As EventConflict In oConflicts
			If oConflict.Action = EventConflictAction.OverWrite Then
				Dim oEvent As CommunityEvent = UpdateCommunityEventFromEsse3(oConflict.InternalEvent, oAvailableDetails.Find(New GenericPredicate(Of DettaglioRegistro_Esse3, Long)(DirectCast(oConflict.ExternalEvent, DettaglioRegistro_Esse3).ID, AddressOf DettaglioRegistro_Esse3.FindByID)))
				oEventsManager.PersistEventToDB(oEvent)
				SolvedConflict += 1
			End If
		Next
		Me.View.SelectEsse3Events(New List(Of Long))
		Me.View.UnloadImportData()
		If oEventErrors.Count = 0 Then
			Me.View.ShowImportResult(String.Format(My.Resources.Resource.Esse3ImportToComol_OK, Me.View.CommunityToImport))
		ElseIf oEventErrors.Count = oSelectedItems.Count + SolvedConflict Then
			Me.View.ShowImportResult(String.Format(My.Resources.Resource.Esse3ImportToComol_Error, Me.View.CommunityToImport))
		Else
			Dim DetailsError As String = ""
			For Each oEvent As DettaglioRegistro_Esse3 In oEventErrors
				DetailsError = "- " & oEvent.Titolo & "( " & oEvent.DataSvolgimento.ToString("dd/MM/yy") & " " & oEvent.OraInizio.ToString("hh:mm") & " " & oEvent.OraFine.ToString("hh:mm") & ")"
			Next
			Me.View.ShowImportResult(String.Format(My.Resources.Resource.Esse3ImportToComol_PartialOk, Me.View.CommunityToImport, DetailsError))
		End If

	End Sub

	Private Function UpdateCommunityEventFromEsse3(ByVal oEvent As CommunityEvent, ByVal oDetails As DettaglioRegistro_Esse3) As CommunityEvent
		oEvent = Me.EventManager.GetEvent(oEvent.ID)
		If oEvent Is Nothing Then
			Return Nothing
		End If

		With oEvent
			.CommunityOwner = Me.View.CommunityToImport
			.ModifiedAt = Now
			.ModifiedBy = Me.View.CurrentTheacher
			.ExternalPlannerID = oDetails.ID
			.Isvisible = True
			.Name = oDetails.Titolo
			.Place = ""
			.Year = New AcademicYear(IIf(Month(Now) > 9, Year(Now), Year(Now) - 1))
			.Type.ID = EventTypeStandard.Lesson

			If .Items.Count = 0 Then
				Dim oItem As New EventItem
				oItem.ModifiedAt = oEvent.ModifiedAt
				oItem.ModifiedBy = Me.View.CurrentTheacher
				oItem.ExternalPlannerID = oEvent.ExternalPlannerID
				oItem.Isvisible = oEvent.Isvisible
				oItem.StartDate = oDetails.DataSvolgimento
				oItem.StartDate = oItem.StartDate.AddHours(oDetails.OraInizio.Hour)
				oItem.StartDate = oItem.StartDate.AddMinutes(oDetails.OraInizio.Minute)
				oItem.EndDate = oDetails.DataSvolgimento
				oItem.EndDate = oItem.EndDate.AddHours(oDetails.OraFine.Hour)
				oItem.EndDate = oItem.EndDate.AddMinutes(oDetails.OraFine.Minute)
				oItem.Text = oDetails.Descrizione
				oItem.Title = oDetails.Titolo
				.Items.Add(oItem)
			Else
				Dim oList As New List(Of EventItem)
				oList = oEvent.Items.FindAll(New GenericPredicate(Of EventItem, String)(oDetails.ID, AddressOf EventItem.FindByExternalPlannerID))

				For Each oItem As EventItem In oList
					oItem.ModifiedAt = oEvent.ModifiedAt
					oItem.ModifiedBy = Me.View.CurrentTheacher
					oItem.Isvisible = oEvent.Isvisible
					oItem.StartDate = oDetails.DataSvolgimento
					oItem.StartDate = oItem.StartDate.AddHours(oDetails.OraInizio.Hour)
					oItem.StartDate = oItem.StartDate.AddMinutes(oDetails.OraInizio.Minute)
					oItem.EndDate = oDetails.DataSvolgimento
					oItem.EndDate = oItem.EndDate.AddHours(oDetails.OraFine.Hour)
					oItem.EndDate = oItem.EndDate.AddMinutes(oDetails.OraFine.Minute)
					oItem.Text = oDetails.Descrizione
					oItem.Title = oDetails.Titolo
				Next
			End If
		End With
		Return oEvent
	End Function
	Private Function CreateCommunityEventFromEsse3(ByVal oDetails As DettaglioRegistro_Esse3) As CommunityEvent
		Dim oEvent As New CommunityEvent
		With oEvent
			.CommunityOwner = Me.View.CommunityToImport
			.CreatedAt = Now
			.CreatedBy = Me.View.CurrentTheacher
			.ExternalPlannerID = oDetails.ID
			.FatherEventID = 0
			.Isvisible = True
			.Name = oDetails.Titolo
			.Place = ""
			.Type = New EventType()
			.Year = New AcademicYear(IIf(Month(Now) > 9, Year(Now), Year(Now) - 1))
			.Type.ID = EventTypeStandard.Lesson

			Dim oItem As New EventItem
			oItem.CreatedAt = oEvent.CreatedAt
			oItem.CreatedBy = oEvent.CreatedBy
			oItem.ExternalPlannerID = oEvent.ExternalPlannerID
			oItem.Isvisible = oEvent.Isvisible
			oItem.StartDate = oDetails.DataSvolgimento
			oItem.StartDate = oItem.StartDate.AddHours(oDetails.OraInizio.Hour)
			oItem.StartDate = oItem.StartDate.AddMinutes(oDetails.OraInizio.Minute)
			oItem.EndDate = oDetails.DataSvolgimento
			oItem.EndDate = oItem.EndDate.AddHours(oDetails.OraFine.Hour)
			oItem.EndDate = oItem.EndDate.AddMinutes(oDetails.OraFine.Minute)
			oItem.Text = oDetails.Descrizione
			oItem.Title = oDetails.Titolo
			.Items.Add(oItem)
		End With
		Return oEvent
	End Function
End Class