Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerLingua
		Inherits ObjectBase
		Implements iManager

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentUser As COL_Persona
		Private _CurrentCommunity As COL_Comunita
		Private _CurrentDB As ConnectionDB
#End Region
#Region "Public property"
		Public ReadOnly Property CurrentCommunity() As Comunita.COL_Comunita Implements iManager.CurrentCommunity
			Get
				Return _CurrentCommunity
			End Get
		End Property
		Public ReadOnly Property CurrentUser() As CL_persona.COL_Persona Implements iManager.CurrentUser
			Get
				Return _CurrentUser
			End Get
		End Property
		Private ReadOnly Property UseCache() As Boolean Implements iManager.UseCache
			Get
				Return _UseCache
			End Get
		End Property
		Private ReadOnly Property CurrentDB() As ConnectionDB Implements iManager.CurrentDB
			Get
				If IsNothing(_CurrentDB) Then
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.Esse3, ConnectionType.SQL)
				End If
				Return _CurrentDB
			End Get
		End Property
#End Region

		Public Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		Public Sub Add(ByVal oLingua As Lingua)
			Try
				Dim oLinguaCreata As Lingua
				Dim oDALlingua As New DAL.StandardDB.DALlingua(Me.currentDB)
				oLinguaCreata = oDALlingua.Add(oLingua)

				If IsNothing(oLinguaCreata) Then
					' generare errore
				Else

				End If
				PurgeCacheItems(CachePolicy.Lingua)
			Catch ex As Exception

			End Try

		End Sub
		Public Sub Save(ByVal oLingua As Lingua)
			Try
				Dim oDALlingua As New DAL.StandardDB.DALlingua(Me.currentDB)
				oDALlingua.Save(oLingua)
			Catch ex As Exception

			End Try
			PurgeCacheItems(CachePolicy.Lingua)
		End Sub
		Public Shared Function GetByID(ByVal LinguaID As Integer) As Lingua

			'Dim oList As List(Of Lingua) = ManagerLingua.List
			'Dim o As Lingua = (From oLingua As Lingua In ManagerLingua.List Where oLingua.ID = 1).FirstOrDefault

			'Dim oList2 As List(Of Lingua) = ManagerLingua.List

			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.ID = LinguaID).FirstOrDefault

			'Dim oLista As New List(Of Lingua)
			'oLista = 

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, Integer)(LinguaID, AddressOf Lingua.FindByID))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetByCode(ByVal Codice As String) As Lingua
			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.Codice = Codice).FirstOrDefault
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, String)(Codice, AddressOf Lingua.FindByCode))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetDefault() As Lingua
			Return (From oLingua As Lingua In ManagerLingua.List Where oLingua.isDefault).FirstOrDefault
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, Boolean)(True, AddressOf Lingua.FindByDefault))
			'	Return oLingua
			'End If
		End Function
		Public Shared Function GetByCodeOrDefault(ByVal Codice As String) As Lingua
			Dim oLingua As Lingua = ManagerLingua.GetByCode(Codice)
			If IsNothing(oLingua) Then
				Return ManagerLingua.GetDefault
			Else
				Return oLingua
			End If
			'Dim oLista As New List(Of Lingua)
			'oLista = ManagerLingua.List

			'If oLista.Count = 0 Then
			'	Return Nothing
			'Else
			'	Dim oLingua As Lingua
			'	oLingua = oLista.Find(New GenericPredicate(Of Lingua, String)(Codice, AddressOf Lingua.FindByCode))
			'	If IsNothing(oLingua) Then
			'		oLingua = ManagerLingua.GetDefault
			'	End If
			'	Return oLingua
			'End If
		End Function
		Public Shared Function FindByIDfromList(ByVal iLista As List(Of Lingua), ByVal iLinguaID As String) As Lingua
			'Dim oLingua As Lingua

			Return (From oLingua As Lingua In iLista Where oLingua.ID = iLinguaID).FirstOrDefault
			'oLingua = iLista.Find(New GenericPredicate(Of Lingua, Integer)(iLinguaID, AddressOf Lingua.FindByID))
			'Return oLingua
		End Function

		Public Shared Function List(Optional ByVal sortExpression As String = "", Optional ByVal oSortDirection As sortDirection = sortDirection.None, Optional ByVal ForceRetrieve As Boolean = False) As List(Of Lingua)
			Dim oLista As New List(Of Lingua)


			Dim cacheKey As String = CachePolicy.Lingua
			If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
				oLista = GetLanguages()
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of Lingua))
			End If

			If sortExpression = "ID" Then
				If oSortDirection = sortDirection.Ascending Then
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.ID Ascending
					Return oQuery.ToList
				Else
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.ID Descending
					Return oQuery.ToList
				End If
			ElseIf sortExpression = "Nome" Then
				If oSortDirection = sortDirection.Ascending Then
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.Nome Ascending
					Return oQuery.ToList
				Else
					Dim oQuery = From oLingua As Lingua In oLista Order By oLingua.Nome Descending
					Return oQuery.ToList
				End If
			End If
			Return oLista
		End Function
        Public Shared Function GetLanguages() As List(Of Lingua)
            Dim oDALlingua As New DAL.StandardDB.DALlingua(GetCurrentDB)
            Return oDALlingua.List
        End Function
	End Class
End Namespace