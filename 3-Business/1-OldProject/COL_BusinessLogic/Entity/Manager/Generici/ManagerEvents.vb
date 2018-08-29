Imports Comol.Entity
Imports Comol.Entity.Events

Namespace Comol.Manager
	Public Class ManagerEvents
		Inherits ObjectBase
		Implements iManagerAdvanced

#Region "Private property"
		Private _UseCache As Boolean
		Private _CurrentDB As ConnectionDB
		Private _CurrentUser As Person
		Private _CurrentCommunity As Community
		Private _Language As Lingua
#End Region

#Region "Public property"
		Public ReadOnly Property CurrentCommunity() As Community Implements iManagerAdvanced.CurrentCommunity
			Get
				Return _CurrentCommunity
			End Get
		End Property
		Public ReadOnly Property CurrentUser() As Person Implements iManagerAdvanced.CurrentUser
			Get
				Return _CurrentUser
			End Get
		End Property
		Private ReadOnly Property UseCache() As Boolean Implements iManagerAdvanced.UseCache
			Get
				Return _UseCache
			End Get
		End Property
		Private ReadOnly Property CurrentDB() As ConnectionDB Implements iManagerAdvanced.CurrentDB
			Get
				If IsNothing(_CurrentDB) Then
					_CurrentDB = ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.Esse3, ConnectionType.SQL)
				End If
				Return _CurrentDB
			End Get
		End Property
		Private ReadOnly Property Language() As Lingua Implements iManagerAdvanced.Language
			Get
				Return _Language
			End Get
		End Property
#End Region

		Public Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		Public Sub New(ByVal oPerson As Person, ByVal oCommunity As Community, ByVal oLanguage As Lingua, Optional ByVal UseCache As Boolean = True)
			Me._UseCache = UseCache
			Me._CurrentUser = oPerson
			Me._CurrentCommunity = oCommunity
			Me._Language = oLanguage
		End Sub

		Public Function PersistEventToDB(ByVal oEvent As CommunityEvent) As CommunityEvent
			Dim oDALevents As New DAL.StandardDB.DALevents(GetCurrentDB)

			If oEvent.ID = 0 Then
				oDALevents.Add(oEvent)
			Else
				oDALevents.ChangeEvent(oEvent)
			End If
			If oEvent.ID > 0 Then
				ObjectBase.PurgeCacheItems(CachePolicyEvent.CommunityEvents(Me._CurrentCommunity.ID))
			End If
			Return oEvent
		End Function

		Public Function GetEvent(ByVal EventID As Integer) As CommunityEvent
			Return GetEventByIDfromDAL(EventID)
		End Function
		Public Function GetEsse3Events(Optional ByVal sortExpression As String = "", Optional ByVal sortDirection As String = "", Optional ByVal ForceRetrieve As Boolean = False) As List(Of CommunityEvent)
			Dim oList As New List(Of CommunityEvent)
			If sortDirection <> String.Empty Then
				sortDirection = sortDirection.ToLower
			End If

			If Me._UseCache Then
				Dim cacheKey As String = CachePolicyEvent.CommunityEvents(Me._CurrentCommunity.ID, Me._Language.ID)
				If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
					oList = GetEventsImportedFromEsse3FromDal()
					ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
				Else
					oList = CType(ObjectBase.Cache(cacheKey), List(Of CommunityEvent))
				End If
			Else
				oList = GetEventsImportedFromEsse3FromDal()
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
		Private Function GetEventByIDfromDAL(ByVal EventID As Integer) As CommunityEvent
			Dim oDALevents As New DAL.StandardDB.DALevents(GetCurrentDB)
			Return oDALevents.GetEventByID(EventID, Me._Language.ID)
		End Function
		Private Function GetEventsImportedFromEsse3FromDal() As List(Of CommunityEvent)
			Dim oDALevents As New DAL.StandardDB.DALevents(GetCurrentDB)
			Return oDALevents.ListEventsImportedFromEsse3(Me._CurrentCommunity.ID, Me._Language.ID)
		End Function
#End Region

	End Class

End Namespace