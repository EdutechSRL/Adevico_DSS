Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerProvincia
		Inherits ObjectBase

		Public Shared Function GetByID(ByVal ProvinciaID As Integer) As Provincia
			Dim oLista As New List(Of Provincia)
			oLista = ManagerProvincia.List()

			If oLista.Count = 0 Then
				Return Nothing
			Else
				Dim oProvincia As Provincia
				oProvincia = oLista.Find(New GenericPredicate(Of Provincia, Integer)(ProvinciaID, AddressOf Provincia.FindByID))
				Return oProvincia
			End If
		End Function
		Public Shared Function List(Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "") As List(Of Provincia)
			Dim oLista As New List(Of Provincia)
			Dim cacheKey As String = CachePolicy.Provincia

			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If

			If ObjectBase.Cache(cacheKey) Is Nothing Then
				Dim oDALprovincia As New DAL.StandardDB.DALprovincia
				oLista = oDALprovincia.List()
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaMensile)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of Provincia))
			End If


			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				oLista.Sort(New GenericComparer(Of Provincia)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				oLista.Reverse()
			End If
			Return oLista
		End Function
	End Class
End Namespace