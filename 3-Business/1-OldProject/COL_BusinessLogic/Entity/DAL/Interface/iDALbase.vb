Namespace Comol.DAL
	Public Interface iDALbase(Of T)
		Function Add(ByVal oObject As T) As T
		Sub Save(ByVal oObject As T)
		Sub Delete(ByVal oObject As T)
		Function List(Optional ByVal Lingua As Lingua = Nothing) As List(Of T)
	End Interface
End Namespace