Imports Comol.Entity

Namespace Comol.Manager
	Public Class ManagerService
		Inherits ObjectBase

		Public Shared Function GetCurrentDB() As ConnectionDB
			Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
		End Function

		Public Shared Function List(Optional ByVal ForceRetrieve As Boolean = False) As List(Of PlainService)
			Dim o As New List(Of PlainService)
            Dim cacheKey As String = CachePolicyServiceTemporary.PlainServiceSmall

			If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
				o = GetPlainServiceSmall()
                ObjectBase.Cache.Insert(cacheKey, o, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
			Else
				o = CType(ObjectBase.Cache(cacheKey), List(Of PlainService))
			End If
			Return o
		End Function

		Private Shared Function GetPlainServiceSmall() As List(Of PlainService)
			Dim oDAL As New DAL.StandardDB.DALservice(GetCurrentDB)
			Return oDAL.SmallList
        End Function

        Public Shared Function ListCommunityTranslated( _
                                                      ByVal LanguageID As Integer, _
                                                      ByVal CommunityID As Integer, _
                                                      Optional ByVal ForceRetrieve As Boolean = False) As List(Of PlainService)

            'If (nocache) Then
            '    Return GetCommunityTranslated(LanguageID, CommunityID)
            'End If

            Dim o As New List(Of PlainService)
            Dim cacheKey As String = CachePolicyServiceTemporary.ListCommunityTranslated(CommunityID, LanguageID)

            If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                o = GetCommunityTranslated(LanguageID, CommunityID)
                ObjectBase.Cache.Insert(cacheKey, o, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                o = CType(ObjectBase.Cache(cacheKey), List(Of PlainService))
            End If
            Return o
        End Function
        Public Shared Function LoadModulePermissions(ByVal IdLanguage As Integer, ByVal IdModule As Integer) As List(Of PlainPermission)
            Dim oDAL As New DAL.StandardDB.DALservice(GetCurrentDB)
            Return oDAL.LoadModulePermissions(IdLanguage, IdModule)
        End Function


        Private Shared Function GetCommunityTranslated(ByVal LanguageID As Integer, ByVal CommunityID As Integer) As List(Of PlainService)
            Dim oDAL As New DAL.StandardDB.DALservice(GetCurrentDB)
            Return oDAL.TranslatedList(CommunityID, LanguageID)
        End Function

        Public Shared Function ListSystemTranslated(ByVal LanguageID As Integer, Optional ByVal ForceRetrieve As Boolean = False) As List(Of PlainService)
            Dim o As New List(Of PlainService)
            Dim cacheKey As String = CachePolicyServiceTemporary.SystemTranslated(LanguageID)

            If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                o = GetSystemTranslated(LanguageID)
                ObjectBase.Cache.Insert(cacheKey, o, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                o = CType(ObjectBase.Cache(cacheKey), List(Of PlainService))
            End If
            Return o
        End Function

        Private Shared Function GetSystemTranslated(ByVal LanguageID As Integer) As List(Of PlainService)
            Dim oDAL As New DAL.StandardDB.DALservice(GetCurrentDB)
            Return oDAL.SystemTranslated(LanguageID)
        End Function
        Public Shared Function RoleTranslated(ByVal RoleID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer, Optional ByVal ForceRetrieve As Boolean = False) As List(Of PlainService)
            Dim o As New List(Of PlainService)
            Dim cacheKey As String = CachePolicyServiceTemporary.ListTranslatedRole(CommunityID, RoleID, LanguageID)

            If ObjectBase.Cache(cacheKey) Is Nothing Or ForceRetrieve Then
                o = GetRoleTranslated(RoleID, CommunityID, LanguageID)
                ObjectBase.Cache.Insert(cacheKey, o, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                o = CType(ObjectBase.Cache(cacheKey), List(Of PlainService))
            End If
            Return o
        End Function

        Private Shared Function GetRoleTranslated(ByVal RoleID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer) As List(Of PlainService)
            Dim oDAL As New DAL.StandardDB.DALservice(GetCurrentDB)
            Return oDAL.RoleTranslatedList(RoleID, CommunityID, LanguageID)
        End Function
	End Class
End Namespace