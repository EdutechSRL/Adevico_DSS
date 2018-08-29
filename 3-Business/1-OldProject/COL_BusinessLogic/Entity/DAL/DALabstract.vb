Imports Comol.Entity.Configuration

Namespace Comol.DAL
	Public MustInherit Class DALabstract
		Private _ConnectionDB As ConnectionDB

		Protected ReadOnly Property ConnectionDB() As ConnectionDB
			Get
				Return _ConnectionDB
			End Get
		End Property

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			_ConnectionDB = oDbconnection
		End Sub
	End Class
End Namespace