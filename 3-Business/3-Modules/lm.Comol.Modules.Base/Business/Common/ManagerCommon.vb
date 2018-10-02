Imports lm.Comol.Core.DomainModel
Imports NHibernate
Imports NHibernate.Linq
Imports NHibernate.Criterion

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerCommon

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub

        Public Function GetPerson(ByVal PersonID As Integer) As iPerson
            Dim oPerson As Person = Nothing

            Try
                oPerson = _Datacontext.GetById(Of Person)(PersonID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oPerson) Then
                oPerson = New Person
            End If
            Return oPerson
        End Function
        Public Function GetAnonymousUser() As Person
            Dim oPerson As Person = Nothing

            Try
                oPerson = (From p In DC.GetCurrentSession.Linq(Of Person)() Where p.TypeID = CInt(UserTypeStandard.Guest) Select p).Skip(0).Take(1).ToList().FirstOrDefault()
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oPerson) Then
                oPerson = New Person
            End If
            Return oPerson
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As Community
            Dim oCommunity As Community = Nothing

            Try
                oCommunity = _Datacontext.GetById(Of Community)(CommunityID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oCommunity) Then
                oCommunity = New Community
            End If
            Return oCommunity
        End Function

        Public Function GetSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer) As Subscription
            Dim oSubscription As Subscription = Nothing

            Try
                Dim oCommunity As Community = _Datacontext.GetById(Of Community)(CommunityID)
                Dim oPerson As Person = _Datacontext.GetById(Of Person)(PersonID)

                oSubscription = (From s In _Datacontext.GetCurrentSession.Linq(Of Subscription)() Where s.Community Is oCommunity And s.Person Is oPerson Select s).Skip(0).Take(1).ToList().FirstOrDefault()
                'oCriteria = _Datacontext.CreateCriteria(Of Subscription)()

                'oCriteria.Add(Where.Subscription.Community.Equal(oCommunity))
                'oCriteria.Add(Where.Subscription.Person.Equal(oPerson))


                'oSubscription = oCriteria.UniqueResult(Of iSubscription)()
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oSubscription
        End Function
        Public Function GetCommunitiesList(ByVal oListID As List(Of Integer)) As List(Of Community)
            Dim oList As List(Of Community) = New List(Of Community)

            Try
                For Each CommunityID As Integer In oListID
                    Dim oCommunity As iCommunity = Me.GetCommunity(CommunityID)
                    If Not IsNothing(oCommunity) Then : oList.Add(oCommunity)

                    End If
                Next
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function

        Public Function GetSystemModuleList() As List(Of COL_BusinessLogic_v2.PlainService)
            Return GetGenericModuleList(-1, -1)
        End Function
        Public Function GetCommunityModuleList(ByVal CommunityID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Return GetGenericModuleList(CommunityID, -1)
        End Function

        Public Function GetUserModuleList(ByVal CommunityID As Integer, ByVal PersonID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Dim oSubscription As Subscription = GetSubscription(CommunityID, PersonID)
            Dim RoleID As Integer = -1
            If Not IsNothing(oSubscription) Then
                RoleID = oSubscription.Role.Id
            End If
            Return GetGenericModuleList(CommunityID, RoleID)
        End Function
        Public Function GetGenericModuleList(ByVal CommunityID As Integer, ByVal RoleID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Dim oList As New List(Of COL_BusinessLogic_v2.PlainService)

            Try
                If CommunityID < 0 Then
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListSystemTranslated(CurrentUserContext.Language.Id)
                ElseIf RoleID < 0 Then
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListCommunityTranslated(CurrentUserContext.Language.Id, CommunityID)
                Else
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.RoleTranslated(RoleID, CommunityID, CurrentUserContext.Language.Id)
                End If

                Return oList
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try

            Return oList
        End Function

        Public Function GetLastSubscriptions(ByVal PersonID As Integer, ByVal number As Integer, ByVal AlsoWaiting As Boolean) As List(Of Subscription)
            Dim oList As List(Of Subscription)
            Dim oPerson As Person = GetPerson(PersonID)


            Dim oQuery = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Person Is oPerson _
                     AndAlso s.Accepted AndAlso s.Enabled AndAlso s.Role.Id > 0 Order By s.LastAccessOn Descending Select s)


            oList = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Person Is oPerson _
                     AndAlso s.Accepted AndAlso s.Enabled AndAlso s.Role.Id > 0 Order By s.LastAccessOn Descending Select s).Skip(0).Take(number).ToList

            If oList.Count < number AndAlso AlsoWaiting Then
                Dim oTemp As List(Of Subscription)
                oTemp = (From s As Subscription In DC.GetCurrentSession.Linq(Of Subscription)() Where s.Person Is oPerson _
                     AndAlso s.Accepted = False AndAlso s.Role.Id > 0 Order By s.SubscriptedOn Descending Select s).Skip(0).Take(number - oList.Count).ToList()
                If oTemp.Count > 0 Then
                    oList.AddRange(oTemp)
                End If
            End If

            Return oList
        End Function


        Public Function GetRolesAvailableID(ByVal CommunityID As Integer) As List(Of Integer)
            Dim oCommunity As Community = Nothing
            Try
                DC.BeginTransaction()
                oCommunity = DC.GetCurrentSession.Get(Of Community)(CommunityID)
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return GetRolesAvailableID(oCommunity)
        End Function
        Public Function GetRolesAvailableID(ByVal oCommunity As Community) As List(Of Integer)
            Dim iResponse As New List(Of Integer)

            Try
                DC.BeginTransaction()
                iResponse = (From t In DC.GetCurrentSession.Linq(Of RoleCommunityTypeTemplate)() Where t.Type Is oCommunity.TypeOfCommunity Select t.Role.Id).ToList
                DC.Commit()
            Catch ex As Exception
                If DC.isInTransaction Then
                    DC.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return iResponse
        End Function

        Public Function GetLanguage(ByVal LanguageID As Integer) As Language
            Dim oLanguage As Language = Nothing

            Try
                _Datacontext.BeginTransaction()
                oLanguage = _Datacontext.GetById(Of Language)(LanguageID)
                _Datacontext.Commit()
            Catch ex As Exception
                If _Datacontext.isInTransaction Then
                    _Datacontext.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oLanguage
        End Function
        Public Function GetLanguage(ByVal LanguageCode As String) As Language
            Dim oLanguage As Language = Nothing

            Try
                _Datacontext.BeginTransaction()
                oLanguage = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.Code = LanguageCode Select l).FirstOrDefault
                _Datacontext.Commit()
            Catch ex As Exception
                If _Datacontext.isInTransaction Then
                    _Datacontext.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oLanguage
        End Function
        Public Function GetDefaultLanguage() As Language
            Dim oLanguage As Language = Nothing

            Try
                _Datacontext.BeginTransaction()
                oLanguage = (From l In _Datacontext.GetCurrentSession.Linq(Of Language)() Where l.isDefault Select l).FirstOrDefault
                _Datacontext.Commit()
            Catch ex As Exception
                If _Datacontext.isInTransaction Then
                    _Datacontext.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return oLanguage
        End Function

        Public Function GetModuleID(ByVal ModuleCode As String) As Integer
            Dim iResponse As Integer = -1

            Try
                iResponse = (From m In _Datacontext.GetCurrentSession.Linq(Of ModuleDefinition)() Where m.Code.Equals(ModuleCode) Select m.Id).FirstOrDefault
            Catch ex As Exception
            End Try
            Return iResponse
        End Function
        Public Function GetModuleCode(ByVal ModuleID As Integer) As String
            Dim iResponse As String = ""

            Try
                _Datacontext.BeginTransaction()

                iResponse = (From m In _Datacontext.GetCurrentSession.Linq(Of ModuleDefinition)() Where m.Id.Equals(ModuleID) Select m.Code).FirstOrDefault
                _Datacontext.Commit()
            Catch ex As Exception
                If _Datacontext.isInTransaction Then
                    _Datacontext.Rollback()
                End If
                Debug.Write(ex.ToString)
            End Try
            Return iResponse
        End Function
        Public Function GetModuleLink(ByVal LinkId As Long) As ModuleLink
            Dim oLink As ModuleLink = Nothing

            Try
                oLink = DC.GetById(Of ModuleLink)(LinkId)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oLink
        End Function


        Public Function GetActiveSubscriptionRoleId(ByVal userID As Integer, ByVal communityId As Integer) As Integer
            Dim roleId As Integer = 0

            roleId = (From s In DC.GetCurrentSession.Linq(Of LazySubscription)()
                      Where s.IdCommunity.Equals(communityId) AndAlso s.IdPerson.Equals(userID) AndAlso s.Accepted AndAlso s.Enabled
                        Select s.IdRole).FirstOrDefault()

            Return roleId
        End Function

        Public Function GetModulePermission(ByVal userID As Integer, ByVal communityId As Integer, ByVal moduleId As Integer) As Long
            Dim permission As Long = 0
            Dim roleId As Integer = GetActiveSubscriptionRoleId(userID, communityId)
            If (roleId <> 0) Then
                Dim mDefinition As ModuleDefinition = (From cModule In DC.GetCurrentSession.Linq(Of CommunityModuleAssociation)()
                                           Where cModule.Enabled AndAlso cModule.Service.Available AndAlso cModule.Community.Id = communityId AndAlso cModule.Service.Id = moduleId
                                           Select cModule.Service).FirstOrDefault()

                If Not IsNothing(mDefinition) Then
                    Dim permissionValue As String = (From crmp In DC.GetCurrentSession.Linq(Of CommunityRoleModulePermission)()
                                                                    Where crmp.Community.Id = communityId AndAlso crmp.Service Is mDefinition AndAlso crmp.Role.Id = roleId
                                                                    Select crmp.PermissionString).FirstOrDefault()

                    If (String.IsNullOrEmpty(permissionValue)) Then
                        permission = 0
                    Else
                        permission = Convert.ToInt64(New String(permissionValue.Reverse().ToArray()), 2)
                    End If
                End If
            End If
            Return permission
        End Function

   

                   
                   

 
    End Class
End Namespace