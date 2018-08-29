Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerCommunity
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
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
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


#Region "Filters"
		Public Function GetFilter_Status(ByVal oSubscriber As Person, ByVal oService As ServiceElement, Optional ByVal ForceRetrieve As Boolean = False) As List(Of CommunityStatus)
			Dim oLista As New List(Of CommunityStatus)
			'Dim cacheKey As String = CachePolicy.DegreeType(oLanguage.ID)

			'If Me._UseCache Then
			'	If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
			'		oLista = GetList(oLanguage)
			'		ObjectBase.Cache.Insert(cacheKey, oLista, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.ScadenzaMensile)
			'	Else
			'		oLista = CType(ObjectBase.Cache(cacheKey), List(Of TypeDegree))
			'	End If
			'Else
			oLista = GetList(oSubscriber, oService)
			'End If

			Return oLista
		End Function

		Private Function GetList(ByVal oSubscriber As Person, ByVal oService As ServiceElement) As List(Of CommunityStatus)
			Dim oDALdegree As New DAL.StandardDB.DALcommunity(Me.CurrentDB)
			Return oDALdegree.FilterList_StatusForSubscripted(oSubscriber.ID, oService.ServizioCode)
		End Function
#End Region

		Public Function ListAllForAdmin(Optional ByVal ForceRetrieve As Boolean = False) As List(Of Community)
			Dim oList As New List(Of Community)

			If Me._UseCache Then
				Dim cacheKey As String = CachePolicyCommunity.CommunityAll()
				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
					oList = GetAllForAdminFromDal()
					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
				Else
					oList = CType(ObjectBase.Cache(cacheKey), List(Of Community))
				End If
			Else
				oList = GetAllForAdminFromDal()
			End If
			Return oList
		End Function
		Public Function ListOrganization(ByVal iLanguage As Lingua, ByVal isLazy As Boolean, Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "", Optional ByVal ForceRetrieve As Boolean = False) As List(Of Organization)
			Dim oList As New List(Of Organization)
			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If

			If Me._UseCache Then
				Dim cacheKey As String = CachePolicyCommunity.Organization(iLanguage.ID)
				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
					oList = GetOrganizationFromDal(isLazy)
					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza12Ore)
				Else
					oList = CType(ObjectBase.Cache(cacheKey), List(Of Organization))
				End If
			Else
				oList = GetOrganizationFromDal(isLazy)
			End If

			If (Not sortExpression Is Nothing AndAlso sortDirection <> String.Empty) Then
				oList.Sort(New GenericComparer(Of Subscription)(sortExpression))
			End If

			If (Not sortDirection Is Nothing AndAlso sortDirection = "desc") Then
				oList.Reverse()
			End If
			Return oList
		End Function

#Region "Common function to Retrieve from DAL"
		Private Function GetOrganizationFromDal(ByVal isLazy As Boolean) As List(Of Organization)
			Dim oDALorganization As New DAL.StandardDB.DALorganization(CurrentDB)
			If isLazy Then
				Return oDALorganization.ListLazyOrganization()
			Else
				Return New List(Of Organization)
			End If
		End Function
		Private Function GetAllForAdminFromDal() As List(Of Community)
			Dim oDALorganization As New DAL.StandardDB.DALorganization(CurrentDB)
			Return oDALorganization.GetAllForAdminFromDal()

		End Function
#End Region
	End Class
End Namespace