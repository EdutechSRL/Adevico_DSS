Imports COL_BusinessLogic_v2.Comunita

Public Class PresenterPermissionDetails
	Inherits GenericPresenter

	Public Sub New(ByVal view As IviewPermissionDetails)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewPermissionDetails
		Get
			View = MyBase.view
		End Get
	End Property
	Public Sub Init()
		Me.Bind_PermessiRuolo()
	End Sub

	Private Sub Bind_PermessiRuolo()
		If Me.View.ServizioID = 0 Then
			Me.View.ShowNoServizio()
		Else
			Dim oServizio As New COL_Servizio
			oServizio.ID = Me.View.ServizioID
			oServizio.EstraiByLingua(Me.View.LinguaCorrenteID)

			If oServizio.Errore = Errori_Db.None Then
				Dim oLista As New List(Of RuoloServizio)

				If Me.View.RuoloID = -1 Then
					oLista = COL_Servizio.ListRuoliServizio(Me.View.ComunitaID, Me.View.ServizioID, Me.View.LinguaCorrenteID, True)
				Else
					oLista = COL_Servizio.ListRuoliServizio(Me.View.ComunitaID, Me.View.ServizioID, Me.View.LinguaCorrenteID, True).FindAll(New GenericPredicate(Of RuoloServizio, Integer)(Me.View.RuoloID, AddressOf RuoloServizio.findByRole))
				End If
				If oLista.Count = 0 Then
					Me.View.ShowNoServizio()
				Else
					Me.View.ChangeTitoloDettaglio(oServizio.Nome)

					Me.View.PopulateDetails(oLista)
				End If
			Else
				Me.View.ShowNoServizio()
			End If
		End If
	End Sub
End Class