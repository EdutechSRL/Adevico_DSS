Imports Comol.Entity.Configuration.Components

Namespace Configuration
	<Serializable(), CLSCompliant(True)> Public Class DBconnectionSettings
		Private _HashConnectionDB As Hashtable
		Private _HashConnectionHibernate As Hashtable
		Public Enum DBsetting
			Errori = 0
			COMOL = 1
			Statistiche = 2
			KnowledgeTutor = 3
			Mail = 4
			Esse3 = 5
            Questionari = 6
            ScormPlayer = 7
		End Enum

		Sub New()
			_HashConnectionDB = New Hashtable
			_HashConnectionHibernate = New Hashtable
		End Sub

		Public Sub AddConnection(ByVal oConnection As ConnectionDB, ByVal oDBsetting As DBsetting)
			If oConnection.DBtype = ConnectionType.Hybernate Then
				_HashConnectionHibernate.Add(oDBsetting, oConnection)
			Else
				_HashConnectionDB.Add(oDBsetting, oConnection)
			End If
		End Sub

		Public Function GetConnections(ByVal oDBsetting As DBsetting) As List(Of ConnectionDB)
			Dim oLista As New List(Of ConnectionDB)
			Dim oConnectionDB As ConnectionDB
			oConnectionDB = DirectCast(_HashConnectionHibernate.Item(oDBsetting), ConnectionDB)
			If Not IsNothing(oConnectionDB) Then
				oLista.Add(oConnectionDB)
			End If
			oConnectionDB = DirectCast(_HashConnectionDB.Item(oDBsetting), ConnectionDB)
			If Not IsNothing(oConnectionDB) Then
				oLista.Add(oConnectionDB)
			End If
			Return oLista
		End Function

		Public Function GetConnection(ByVal oDBsetting As DBsetting, ByVal oType As ConnectionType) As ConnectionDB
			Dim oConnectionDB As ConnectionDB
			If oType = ConnectionType.Hybernate Then
				oConnectionDB = DirectCast(_HashConnectionHibernate.Item(oDBsetting), ConnectionDB)
			Else
				oConnectionDB = DirectCast(_HashConnectionDB.Item(oDBsetting), ConnectionDB)
			End If
			Return oConnectionDB
		End Function
	End Class
End Namespace