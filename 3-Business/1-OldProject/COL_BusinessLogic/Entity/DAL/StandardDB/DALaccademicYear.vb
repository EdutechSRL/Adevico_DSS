Imports Comol.Entity

Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Namespace Comol.DAL.StandardDB
	Public Class DALaccademicYear
		Inherits DALabstract
		Implements iDALbase(Of AcademicYear)

		Public Sub New(ByVal oDbconnection As ConnectionDB)
			MyBase.new(oDbconnection)
		End Sub

		Public Function List(Optional ByVal Lingua As Lingua = Nothing) As System.Collections.Generic.List(Of AcademicYear) Implements Comol.DAL.iDALbase(Of AcademicYear).List
			Return Nothing
		End Function
		Public Function Add(ByVal oObject As AcademicYear) As AcademicYear Implements Comol.DAL.iDALbase(Of AcademicYear).Add
			Return Nothing
		End Function
		Public Sub Save(ByVal oObject As AcademicYear) Implements Comol.DAL.iDALbase(Of AcademicYear).Save

		End Sub
		Public Sub Delete(ByVal oObject As AcademicYear) Implements Comol.DAL.iDALbase(Of AcademicYear).Delete

		End Sub

		
	End Class
End Namespace