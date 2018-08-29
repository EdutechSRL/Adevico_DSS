Imports COL_BusinessLogic_v2.Comol.DAL.StandardDB

Imports COL_BusinessLogic_v2.Comol.Manager

Namespace Comol.Manager
	Public Class ManagerPersona
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

		Public Sub New(ByVal oPersona As COL_Persona, ByVal oComunita As COL_Comunita, Optional ByVal UseCache As Boolean = True)
			Me._UseCache = UseCache
			Me._CurrentUser = oPersona
			Me._CurrentCommunity = oComunita
		End Sub

		Protected Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		

		Public Shared Function GetDefaultRoleID(ByVal PersonID As Integer) As Integer
			Dim oRoleID As Integer = Main.TipoRuoloStandard.AccessoNonAutenticato
			Dim oDal As New DALpersona(ManagerPersona.GetCurrentDB)

			Try
				oRoleID = oDal.GetDefaultRoleID(PersonID)
			Catch ex As Exception

			End Try

			Return oRoleID
		End Function


		Public Shared Function FindLoginAccessByEmail(ByVal UserMail As MailAddress) As COL_Persona
			Dim oRequest As New COL_Request
			Dim oParam As New COL_Request.Parameter
			Dim objAccesso As New COL_DataAccess

			With oRequest
				.Command = "sp_Persona_FindLoginAccessByEmailLazy"
				.CommandType = CommandType.StoredProcedure
				oParam = objAccesso.GetAdvancedParameter("@PRSN_ID", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_login", "", ParameterDirection.Output, SqlDbType.VarChar, False, 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_Nome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_Cognome", "", ParameterDirection.Output, SqlDbType.VarChar, False, 50)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_mail", UserMail.Address, ParameterDirection.Input, SqlDbType.VarChar, False, 255)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_pwd", "", ParameterDirection.Output, SqlDbType.VarChar, False, 255)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_AUTN_ID", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_AUTN_RemoteUniqueID", "", ParameterDirection.Output, SqlDbType.VarChar, True, 255)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_invisible", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_TPPR_Id", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_mostraMail", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@LinguaID", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)
				oParam = objAccesso.GetAdvancedParameter("@PRSN_sesso", "", ParameterDirection.Output, SqlDbType.Int)
				.Parameters.Add(oParam)

				.Role = COL_Request.UserRole.Admin
				.transactional = False
			End With
			Try	'lettura login e pwd da db
				objAccesso.GetExecuteNotQuery(oRequest)

				If oRequest.GetValueFromParameter(2) <> "" Then
					Dim oPersona As New COL_Persona
					oPersona.ID = oRequest.GetValueFromParameter(1)
					oPersona.Login = oRequest.GetValueFromParameter(2)
					oPersona.Nome = oRequest.GetValueFromParameter(3)
					oPersona.Cognome = oRequest.GetValueFromParameter(4)
					oPersona.Mail = UserMail.Address
					oPersona.Pwd = oRequest.GetValueFromParameter(6)
					oPersona.AUTN_ID = oRequest.GetValueFromParameter(7)
					oPersona.AUTN_RemoteUniqueID = oRequest.GetValueFromParameter(8)
					oPersona.Bloccata = (oRequest.GetValueFromParameter(9) = 1)
					oPersona.TipoPersona.ID = oRequest.GetValueFromParameter(10)
					oPersona.MostraMail = (oRequest.GetValueFromParameter(11) = 1)
					oPersona.Lingua = ManagerLingua.GetByID(oRequest.GetValueFromParameter(12))
					oPersona.Sesso = oRequest.GetValueFromParameter(13)

					Return oPersona
				End If

			Catch ex As Exception
				Return Nothing
			End Try
			Return Nothing
		End Function

#Region "Filters"
		Public Function GetCommunityFilter_Status(ByVal FacoltaID As Integer, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "", Optional ByVal ForceRetrieve As Boolean = False) As List(Of CommunityStatus)
			Dim oLista As New List(Of CommunityStatus)
			'Dim cacheKey As String = CachePolicy.DegreeType(oLanguage.ID)

			'If sortDirection <> String.Empty Then
			'	sortDirection = sortDirection.ToLower
			'End If

			'If Me._UseCache Then
			'	If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
			'		oLista = GetList(oLanguage)
			'		ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaMensile)
			'	Else
			'		oLista = CType(ObjectBase.Cache(cacheKey), List(Of TypeDegree))
			'	End If
			'Else
			'	oLista = GetList(oLanguage)
			'End If

			'If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
			'	oLista.Sort(New GenericComparer(Of TypeDegree)(sortExpression))
			'End If

			'If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
			'	oLista.Reverse()
			'End If
			Return oLista
		End Function
#End Region

		Public Shared Function GetPermessiServizio(ByVal PersonaID As Integer, ByVal ServiceCode As String, Optional ByVal ForceRetrieve As Boolean = False) As IList(Of ServiceBase)
			Dim oDal As New DALpersona(GetCurrentDB)
			Dim oLista As New List(Of ServiceBase)
            Dim cacheKey As String = CachePolicy.PermessiServizioUtente(ServiceCode, PersonaID)

			If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
				oLista = oDal.GetCommunitiesPermessionForService(PersonaID, ServiceCode)
				ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
			Else
				oLista = CType(ObjectBase.Cache(cacheKey), List(Of ServiceBase))
			End If
			Return oLista
		End Function

        Public Shared Sub PurgeServiceCache(ByVal PersonID As Integer, ByVal ServiceCode As String)
            ObjectBase.PurgeCacheItems(CachePolicy.PermessiServizioUtente(ServiceCode, PersonID))
        End Sub

        Public Shared Sub PurgeAllServicesCache(ByVal PersonID As Integer)
            ObjectBase.PurgeCacheItems(CachePolicy.PermessiServizioUtente(), "_" & PersonID.ToString)
        End Sub
	End Class
End Namespace