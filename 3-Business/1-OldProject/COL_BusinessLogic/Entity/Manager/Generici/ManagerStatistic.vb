Imports Comol.Entity

Public Class ManagerStatistic
	Inherits ObjectBase

#Region "Private property"
	Private _UseCache As Boolean
	Private _CurrentDB As ConnectionDB
	Private _Language As Lingua
#End Region

#Region "Public property"
	Private ReadOnly Property UseCache() As Boolean
		Get
			Return _UseCache
		End Get
	End Property
	Private ReadOnly Property CurrentDB() As ConnectionDB
		Get
			If IsNothing(_CurrentDB) Then
				_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.Statistiche, ConnectionType.SQL)
			End If
			Return _CurrentDB
		End Get
	End Property
	Private ReadOnly Property Language() As Lingua
		Get
			Return _Language
		End Get
	End Property
#End Region


	Public Sub New(ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True)
		Me._UseCache = UseCache
		Me._Language = oLanguage
	End Sub

	Public Shared Function GetCurrentDB() As ConnectionDB
		Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.Statistiche, ConnectionType.SQL)
	End Function
	Public Shared Function ComolGetCurrentDB() As ConnectionDB
		Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
	End Function

	Public Function GetSystemStatistics(ByVal oContext As DateTimeContext, ByVal PortalID As Integer, Optional ByVal PersonTypeID As Integer = -1, Optional ByVal OrganizationID As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As List(Of SummaryElement)
		Dim oList As New List(Of SummaryElement)

		'oList = RetrieveSystemStatistics(oContext, PortalID, PersonTypeID, OrganizationID, True)

		Return oList
	End Function

	'Public Function ElencaAccessiGiornalieri(Optional ByVal giorno As Integer = -1, Optional ByVal mese As Integer = -1, Optional ByVal anno As Integer = -1, Optional ByVal idTPPR As Integer = -1, Optional ByVal ORGN_Id As Integer = -1, Optional ByVal FiltroElencoTipiPersona As Integer = 1) As DataSet
	'	Dim oRequest As New COL_Request
	'	Dim oParam As New COL_Request.Parameter
	'	Dim dsTable As New DataSet
	'	Dim objAccesso As New COL_DataAccess
	'	'FiltroElencoTipiPersona int = 1 -- 0 = con Guest
	'	With oRequest
	'		.Command = "sp_Statistiche_ElencaAccessiGiornalieri"
	'		.CommandType = CommandType.StoredProcedure

	'		oParam = objAccesso.GetAdvancedParameter("@STAC_Giorno", giorno, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@STAC_Mese", mese, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@STAC_Anno", anno, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@STAC_TPPR_id", idTPPR, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@DTBS_ID", n_STAT_DTBS_ID, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@ORGN_Id", ORGN_Id, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)
	'		oParam = objAccesso.GetAdvancedParameter("@FiltroElencoTipiPersona", FiltroElencoTipiPersona, ParameterDirection.Input, SqlDbType.Int)
	'		.Parameters.Add(oParam)

	'		.Role = COL_Request.UserRole.Admin
	'		.transactional = False
	'	End With
	'	Try
	'		dsTable = objAccesso.GetdataSet(oRequest)
	'		Me.n_erroreDb = Errori_Db.None
	'	Catch ex As Exception
	'		Dim oMail As New MailDBerrori
	'		oMail.Oggetto = "Statistiche"
	'		oMail.Body = "Errore=" & ex.Message
	'		Me.n_erroreDb = Errori_Db.DBError
	'	End Try
	'	Return dsTable
	'End Function


	'Private Function RetrieveSystemStatistics(ByVal oContext As DateTimeContext, ByVal PortalID As Integer, Optional ByVal PersonTypeID As Integer = -1, Optional ByVal OrganizationID As Integer = -1, Optional ByVal ShowGuest As Boolean = False) As List(Of SummaryElement)
	'	Dim oList As New List(Of SummaryElement)
	'	Dim oDataContext As New StatisticDataContext(GetCurrentDB.ConnectionString)
	'	Dim oComolDataContext As New ComolSQLentityDataContext(ComolGetCurrentDB.ConnectionString)


	'	Dim oQuery = From o As LINQsql_StatisticheAccessi In oDataContext.LINQsql_StatisticheAccessis _
	'	  Where (oContext.Day = -1 OrElse o.Day = oContext.Day) AndAlso (oContext.Month = -1 OrElse o.Month = oContext.Month) AndAlso (oContext.Year = -1 OrElse o.Year = oContext.Year) AndAlso (oContext.Hour = -1 OrElse o.Hour = oContext.Hour) AndAlso (PersonTypeID = -1 OrElse o.PersonTypeID = PersonTypeID) AndAlso o.PortalID = PortalID AndAlso (ShowGuest OrElse (ShowGuest = False And o.PersonTypeID <> Main.TipoPersonaStandard.Guest)) _
	'	  AndAlso (OrganizationID = -1 Or (From uo As LINQsqlUserOrganization In oComolDataContext.LINQsqlUserOrganizations Where o.PersonID = uo.PersonID AndAlso uo.IsDefault = True And uo.OrganizationID = OrganizationID).Any) _
	'	  Group By Day = o.Day, Year = o.Year, Month = o.Month, UserTypeID = o.PersonTypeID Into g = Group Select Day, Year, Month, UserTypeID, g.Count

	'	'



	'	Dim oUserTypeList As IList(Of LINQsqlUserType) = oComolDataContext.GetPersonType(Me._Language.ID, 0).ToList

	'	For Each o In oQuery
	'		Dim ou As New UserStatistic
	'		ou.Day = o.Day
	'		ou.Month = o.Month
	'		ou.Year = o.Year
	'		ou.UserType = New GenericElement(o.UserTypeID, oUserTypeList.SingleOrDefault(Function(oUserType) oUserType.ID = o.UserTypeID).Description())
	'		oList.Add(New SummaryElement(ou, 1, o.Count))
	'	Next

	'	Return oList
	'End Function

	'Private Function RetrieveCommunityStatistics(ByVal oContext As DateTimeContext, ByVal PortalID As Integer, Optional ByVal PersonTypeID As Integer = -1, Optional ByVal OrganizationID As Integer = -1, Optional ByVal ShowGuest As Boolean = False) As List(Of SummaryElement)
	'	Dim oList As New List(Of SummaryElement)
	'	Dim oDataContext As New StatisticDataContext(GetCurrentDB.ConnectionString)
	'	Dim oQuery = From o As LINQsql_StatisticheAccessi In oDataContext.LINQsql_StatisticheAccessis _
	'	Where (oContext.Day = -1 OrElse o.Day = oContext.Day) AndAlso (oContext.Month = -1 OrElse o.Month = oContext.Month) AndAlso (oContext.Year = -1 OrElse o.Year = oContext.Year) AndAlso (oContext.Hour = -1 OrElse o.Hour = oContext.Hour) AndAlso (PersonTypeID = -1 OrElse o.PersonTypeID = PersonTypeID) And o.PortalID = PortalID _
	'	  Group By Day = o.Day, Year = o.Year, Month = o.Month, UserTypeID = o.PersonTypeID Into g = Group Select Day, Year, Month, UserTypeID, g.Count




	'	Dim oRoleDataContext As New ComolSQLentityDataContext(ComolGetCurrentDB.ConnectionString)

	'	Dim oUserTypeList As IList(Of LINQsqlUserType) = oRoleDataContext.GetPersonType(Me._Language.ID, 0).ToList

	'	Dim oRoleList As IList(Of LINQsqlRole) = oRoleDataContext.GetRoles(Me._Language.ID).ToList
	'	For Each o In oQuery
	'		Dim ou As New UserStatistic
	'		ou.Day = o.Day
	'		ou.Month = o.Month
	'		ou.Year = o.Year
	'		ou.UserType = New GenericElement(o.UserTypeID, oUserTypeList.SingleOrDefault(Function(oUserType) oUserType.ID = o.UserTypeID).Description())
	'		oList.Add(New SummaryElement(ou, 1, o.Count))
	'	Next

	'	Return oList
	'End Function
End Class