Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel
Imports NHibernate
Imports NHibernate.Linq
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports COL_BusinessLogic_v2.Comol.Manager
Imports System.Data.Common
Imports lm.Comol.Core.Communities

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerPermission
        Inherits COL_BusinessLogic_v2.ObjectBase
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
        Private _ConnectionDB As String
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property UserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal DBName As String)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
            _ConnectionDB = DBName
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub


#Region "Template"
        Public Sub ApplyTemplateToAll(ByVal CommunityTypeID As Integer, ByVal OrganizationID As Integer, ByVal ModuleID As Integer, ByVal permissions As List(Of dtoTemplateModulePermission))
            ApplyToOrganizationTemplate(CommunityTypeID, OrganizationID, ModuleID, permissions, True)
        End Sub
        Public Sub SaveTemplate(ByVal CommunityTypeID As Integer, ByVal OrganizationID As Integer, ByVal ModuleID As Integer, ByVal permissions As List(Of dtoTemplateModulePermission))
            ApplyToOrganizationTemplate(CommunityTypeID, OrganizationID, ModuleID, permissions, False)
        End Sub
        Private Sub ApplyToOrganizationTemplate(ByVal CommunityTypeID As Integer, ByVal OrganizationID As Integer, ByVal ModuleID As Integer, ByVal permissions As List(Of dtoTemplateModulePermission), ByVal ToAll As Boolean)
            Dim oType As CommunityType = DC.GetCurrentSession.Get(Of CommunityType)(CommunityTypeID)
            Dim IdOrganizations As New List(Of Integer)

            If ToAll Then
                IdOrganizations.Add(-1)
                IdOrganizations.AddRange((From c In DC.GetCurrentSession.Linq(Of Organization)() Select c.Id).ToList)
            Else
                IdOrganizations.Add(OrganizationID)
            End If
            Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(ModuleID)
            If Not IsNothing(oModuleDefinition) Then
                Dim EnabledForPortal As Boolean = oModuleDefinition.Available
                Dim DefaultOrganizationEnabled As Boolean = (From mt In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() _
                                                      Where mt.IdCommunityType = CommunityTypeID AndAlso mt.IdModule = ModuleID AndAlso mt.IdOrganization = -1 _
                                                      Select mt.Enabled).FirstOrDefault
                Dim OrganizationEnabled As Boolean = (From mt In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() _
                                                      Where mt.IdCommunityType = CommunityTypeID AndAlso mt.IdModule = ModuleID AndAlso mt.IdOrganization = OrganizationID _
                                                      Select mt.Enabled).FirstOrDefault

                If IdOrganizations.Count > 0 Then
                    UpdatePermissionTemplate(ModuleID, CommunityTypeID, IdOrganizations, permissions)

                    Dim query = (From mt In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                                                      Where mt.IdCommunityType = CommunityTypeID AndAlso mt.IdModule = ModuleID AndAlso IdOrganizations.Contains(mt.IdOrganization) _
                                                      Select mt.IdOrganization, mt.IdRole).ToList


                    Dim listToInsert As Dictionary(Of Integer, List(Of dtoTemplateModulePermission)) = IdOrganizations.ToDictionary(Function(id) id, _
                            Function(id) (From p In permissions Where (From q In query Where q.IdOrganization = id Select q.IdRole).ToList().Contains(p.RoleId) = False Select p).ToList)

                    If (From i In listToInsert Where i.Value.Count > 0 Select i).Count > 0 Then
                        InsertPermissionTemplate(ModuleID, CommunityTypeID, (From i In listToInsert Where i.Value.Count > 0 Select i).ToDictionary(Function(c) c.Key, Function(c) c.Value))
                    End If
                End If
            End If
        End Sub
        Private Function GetSQLupdateUserContext() As String
            Dim result As String = "_ModifiedOn='{0}', _ModifiedBy='{1}',_ModifiedIPaddress='{2}',_ModifiedProxyIPaddress='{3}' "
            Dim modifiedOn As DateTime = DateTime.Now

            result = String.Format(result, modifiedOn.ToString("s"), UserContext.CurrentUserID, UserContext.IpAddress, UserContext.ProxyIpAddress)
            Return result
        End Function
        Private Function GetSQLinsertUserContext() As String
            ',_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress)" _
            Dim result As String = "'{0}', '{1}','{2}','{3}',{4},'{5}','{6}','{7}' "
            Dim modifiedOn As DateTime = DateTime.Now

            result = String.Format(result, modifiedOn.ToString("s"), UserContext.CurrentUserID, UserContext.IpAddress, UserContext.ProxyIpAddress, UserContext.CurrentUserID, modifiedOn.ToString("s"), UserContext.IpAddress, UserContext.ProxyIpAddress)
            Return result
        End Function
        Private Sub InsertPermissionTemplate(ByVal IdModule As Integer, ByVal IdCommunityType As Integer, ByVal items As Dictionary(Of Integer, List(Of dtoTemplateModulePermission)))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim InsertInfo As String = GetSQLinsertUserContext()
                    Dim sqlInsertCommand As String = "INSERT into LK_PERMESSI_RUOLO_SERVIZIO (LPRS_valore, LPRS_TPRL_id, LPRS_SRVZ_id, LPRS_ORGN_ID, LPRS_TPCM_ID,_ModifiedOn,_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress)" _
                                                     & " VALUES ('{0}',{1}," & IdModule & ",{2}," & IdCommunityType & "," & InsertInfo & ") "

                    ' Dim SQLconcatenatedID As String = SQLconcatenateColumnId("LPRS_ORGN_ID", IdOrganizations)
                    For Each item In items
                        Dim builder As New Text.StringBuilder
                        Dim OrganizationId As Integer = item.Key
                        item.Value.ForEach(Function(permission) builder.Append(IIf(builder.Length > 0, vbCrLf & String.Format(sqlInsertCommand, permission.PermissionToString, permission.RoleId, OrganizationId), String.Format(sqlInsertCommand, permission.PermissionToString, permission.RoleId, OrganizationId))))
                        Dim SqlCommand As String = builder.ToString

                        Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(SqlCommand)
                        dbCommand.Connection = connection
                        dbCommand.CommandType = CommandType.Text
                        dbCommand.ExecuteNonQuery()

                    Next
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub UpdatePermissionTemplate(ByVal IdModule As Integer, ByVal IdCommunityType As Integer, ByVal IdOrganizations As List(Of Integer), ByVal permissions As List(Of dtoTemplateModulePermission))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = ""
                    Dim UpdateInfo As String = GetSQLupdateUserContext()
                    Dim sqlUpdateCommand As String = "UPDATE LK_PERMESSI_RUOLO_SERVIZIO set LPRS_valore='{2}', " & UpdateInfo & "  where ({0}) and LPRS_SRVZ_id=" & IdModule & " and LPRS_TPRL_id={1} and LPRS_valore<>'{2}' and LPRS_TPCM_ID=" & IdCommunityType & " "

                    Dim id As String = SQLconcatenateColumnId("LPRS_ORGN_ID", IdOrganizations)
                    For Each item In permissions
                        sqlCommand &= vbCrLf & String.Format(sqlUpdateCommand, id, item.RoleId, item.PermissionToString)
                    Next
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub

        Private Sub InsertRolePermissionTemplate(ByVal idRole As Integer, ByVal idModule As Integer, ByVal items As Dictionary(Of Integer, List(Of dtoTemplateRolePermission)))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim InsertInfo As String = GetSQLinsertUserContext()
                    Dim sqlInsertCommand As String = "INSERT into LK_PERMESSI_RUOLO_SERVIZIO (LPRS_valore, LPRS_TPRL_id, LPRS_SRVZ_id, LPRS_ORGN_ID, LPRS_TPCM_ID,_ModifiedOn,_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress)" _
                                                     & " VALUES ('{0}'," & idRole & "," & idModule & ",{2},{1}," & InsertInfo & ") "

                    ' Dim SQLconcatenatedID As String = SQLconcatenateColumnId("LPRS_ORGN_ID", IdOrganizations)
                    For Each item In items
                        Dim builder As New Text.StringBuilder
                        Dim OrganizationId As Integer = item.Key
                        item.Value.ForEach(Function(permission) builder.Append(IIf(builder.Length > 0, vbCrLf & String.Format(sqlInsertCommand, permission.PermissionToString, permission.IdCommunityType, OrganizationId), String.Format(sqlInsertCommand, permission.PermissionToString, permission.IdCommunityType, OrganizationId))))
                        Dim SqlCommand As String = builder.ToString

                        Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(SqlCommand)
                        dbCommand.Connection = connection
                        dbCommand.CommandType = CommandType.Text
                        dbCommand.ExecuteNonQuery()

                    Next
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub UpdateRolePermissionTemplate(ByVal idRole As Integer, ByVal idModule As Integer, ByVal idOrganizations As List(Of Integer), ByVal permissions As List(Of dtoTemplateRolePermission))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = ""
                    Dim UpdateInfo As String = GetSQLupdateUserContext()
                    Dim sqlUpdateCommand As String = "UPDATE LK_PERMESSI_RUOLO_SERVIZIO set LPRS_valore='{2}', " & UpdateInfo & "  where ({0}) and LPRS_SRVZ_id=" & idModule & " and LPRS_TPCM_ID={1} and LPRS_valore<>'{2}' and LPRS_TPRL_id=" & idRole & " "

                    Dim id As String = SQLconcatenateColumnId("LPRS_ORGN_ID", idOrganizations)
                    For Each item In permissions
                        sqlCommand &= vbCrLf & String.Format(sqlUpdateCommand, id, item.IdCommunityType, item.PermissionToString)
                    Next
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub

#End Region

        Public Function GetModuleCode(ByVal ModuleID As Integer) As String
            Dim result As String = ""
            Try
                result = (From s In DC.GetCurrentSession().Linq(Of ModuleDefinition)() Where s.Id = ModuleID Select s.Code).Skip(0).Take(1).ToList().FirstOrDefault()
            Catch ex As Exception

            End Try
            Return result
        End Function

#Region "From modules"
        Public Sub ApplyToCommunities(ByVal CommunityTypeID As Integer, ByVal OrganizationID As Integer, ByVal ModuleID As Integer, ByVal oList As List(Of dtoTemplateModulePermission))
            Try
                Dim oType As CommunityType = DC.GetCurrentSession.Get(Of CommunityType)(CommunityTypeID)
                Dim oCommunities As List(Of Integer) = (From c In DC.GetCurrentSession.Linq(Of Community)() Where c.TypeOfCommunity Is oType AndAlso (OrganizationID = -1 OrElse c.IdOrganization = OrganizationID) Select c.Id).ToList
                Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(ModuleID)

                ' FIND DUPLICATE MODULE ASSOCIATION
                If Not IsNothing(oModuleDefinition) Then
                    Dim tmpCommunities As List(Of Integer) = oCommunities.Skip(0).Take(500).ToList()
                    Dim page As Integer = 0
                    While tmpCommunities.Count > 0
                        Dim query = (From mc In DC.GetCurrentSession().Linq(Of ModuleCommunity)() _
                                                                    Where tmpCommunities.Contains(mc.Community.Id) AndAlso mc.ModuleDefinition Is oModuleDefinition _
                                                                    Group mc By mc.Community.Id Into Group
                                                                    Select Id, Group.Count).ToList()
                        'Dim list As List(Of Integer)
                        'list = (From q In query Where q.Count = 1 Select q.Id).ToList

                        Dim IdCommunitiesToRemove As List(Of Integer) = (From q In query Where q.Count > 1 Select q.Id).ToList
                        If IdCommunitiesToRemove.Count > 0 Then
                            ClearCommunityPermission(ModuleID, IdCommunitiesToRemove, 0)
                        End If

                        Dim IdCommunitiesToAdd As List(Of Integer) = (From i In tmpCommunities Where Not (From q In query Select q.Id).ToList().Contains(i) Select i).ToList
                        If IdCommunitiesToAdd.Count > 0 Then
                            IdCommunitiesToAdd.AddRange(IdCommunitiesToRemove)
                            AddModuleToCommunity(ModuleID, oModuleDefinition.isNotificable, oModuleDefinition.Available, IdCommunitiesToAdd)
                        End If
                        page += 1
                        tmpCommunities = oCommunities.Skip(page * 500).Take(500).ToList()
                    End While




                End If

                'Dim ItemsToApply As New List(Of dtoDB)
                If oCommunities.Count > 0 Then
                    ' UPDATE ROLE / COmmUNITY / PERMISSION
                    UpdatePermission(ModuleID, oCommunities, oList)
                    ApplyPermission(ModuleID, oCommunities, oList)
                End If

                'For Each community In oCommunities
                '    Dim CommunityID As Integer = community
                '    Dim oCommunity As Community = DC.GetById(Of Community)(CommunityID)
                '    Dim ActiveModules As List(Of ModuleCommunity) = (From mc In DC.GetCurrentSession().Linq(Of ModuleCommunity)() Where mc.Community Is oCommunity AndAlso mc.ModuleDefinition Is oModuleDefinition Select mc).ToList()
                '    Dim oModules As List(Of dtoDB) = New List(Of dtoDB)
                '    If ActiveModules.Count > 1 Then
                '        ClearCommunityPermission(ModuleID, CommunityID, (From am In ActiveModules Order By am.CreatedOn Descending Select am.Id).FirstOrDefault())
                '    ElseIf ActiveModules.Count = 0 Then
                '        ClearCommunityPermission(ModuleID, CommunityID, 0)
                '        Dim oNewModule As New ModuleCommunity


                '        If Not IsNothing(oModuleDefinition) AndAlso Not IsNothing(oCommunity) Then
                '            oNewModule.Community = oCommunity
                '            oNewModule.ModuleDefinition = oModuleDefinition
                '            oNewModule.CreatedOn = DateTime.Now
                '            oNewModule.Enabled = oModuleDefinition.Available
                '            oNewModule.isNotificable = oModuleDefinition.isNotificable
                '            oNewModule.ModifiedOn = DateTime.Now
                '            DC.SaveOrUpdate(oNewModule)
                '        End If

                '    Else
                '        oModules = (From mp In DC.GetCurrentSession.Linq(Of LazyCommunityModulePermission)() _
                '                                                      Where mp.CommunityID = CommunityID AndAlso mp.ModuleID = ModuleID Select New dtoDB() With {.CommunityID = CommunityID, .Permission = mp.Permission, .RoleID = mp.RoleID}).ToList
                '    End If


                '    Dim oItemsToAdd As New List(Of dtoDB)
                '    Dim oItemsToUpdate As New List(Of dtoDB)
                '    Dim oRoles As List(Of Integer) = (From m In oModules Select m.RoleID).ToList
                '    oItemsToUpdate = (From m In oModules Join dt In oList On m.RoleID Equals dt.RoleId Where dt.PermissionToString <> m.Permission Select dtoDB.Update(m, dt.PermissionToString)).ToList

                '    If oItemsToUpdate.Count > 0 Then
                '        ApplyToCommunity(ModuleID, oItemsToUpdate)
                '    End If

                '    oItemsToAdd = (From dt In oList Where Not oRoles.Contains(dt.RoleId) Select New dtoDB() With {.RoleID = dt.RoleId, .Permission = dt.PermissionToString, .TypeAction = ActionDB.Insert, .CommunityID = CommunityID}).ToList
                '    If oItemsToAdd.Count > 0 Then
                '        ApplyToCommunity(ModuleID, oItemsToAdd)
                '    End If

                'Next
            Catch ex As Exception

            End Try
        End Sub


        Private Sub ApplyPermission(ByVal ModuleID As Integer, IdCommunities As List(Of Integer), ByVal permissions As List(Of dtoTemplateModulePermission))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim InsertInfo As String = GetSQLinsertUserContext()
                    Dim sqlInsertCommand As String = "INSERT INTO LK_SERVIZIO_COMUNITA (LKSC_CMNT_id, LKSC_SRVZ_id, LKSC_Permessi, LKSC_TPRL_id,_ModifiedOn,_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress) " _
                        & " values({0}," & ModuleID & ",'{1}',{2}," & InsertInfo & ") "
                    For Each item In permissions
                        Dim RoleId As Integer = item.RoleId
                        Dim PermissionToString As String = item.PermissionToString

                        If IdCommunities.Count > 0 Then
                            Dim tmpCommunities As List(Of Integer) = IdCommunities.Skip(0).Take(500).ToList()
                            Dim page As Integer = 0
                            While tmpCommunities.Count > 0
                                Dim Inserted As List(Of Integer) = (From mp In DC.GetCurrentSession.Linq(Of LazyCommunityModulePermission)() _
                                        Where tmpCommunities.Contains(mp.CommunityID) AndAlso mp.ModuleID = ModuleID AndAlso mp.RoleID = RoleId
                                        Select mp.CommunityID).ToList()

                                Dim IdCommunitiesForInsert As List(Of Integer) = (From id In tmpCommunities Where Inserted.Contains(id) = False Select id).ToList
                                Dim builder As New Text.StringBuilder
                                If IdCommunitiesForInsert.Count > 0 Then
                                    IdCommunitiesForInsert.ForEach(Function(id) builder.Append(IIf(builder.Length > 0, vbCrLf & String.Format(sqlInsertCommand, id, PermissionToString, RoleId), String.Format(sqlInsertCommand, id, PermissionToString, RoleId))))
                                    Dim SqlCommand As String = builder.ToString

                                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(SqlCommand)
                                    dbCommand.Connection = connection
                                    dbCommand.CommandType = CommandType.Text
                                    dbCommand.ExecuteNonQuery()
                                End If

                                page += 1
                                tmpCommunities = IdCommunities.Skip(page * 500).Take(500).ToList()
                            End While


                        End If
                    Next
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub ApplyPermission(ByVal idRole As Integer, ByVal idModule As Integer, idCommunities As List(Of Integer), ByVal permission As String)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim InsertInfo As String = GetSQLinsertUserContext()
                    Dim sqlInsertCommand As String = "INSERT INTO LK_SERVIZIO_COMUNITA (LKSC_CMNT_id, LKSC_SRVZ_id, LKSC_Permessi, LKSC_TPRL_id,_ModifiedOn,_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress) " _
                        & " values({0}," & idModule & ",'{1}',{2}," & InsertInfo & ") "


                    If idCommunities.Count > 0 Then
                        Dim tmpCommunities As List(Of Integer) = idCommunities.Skip(0).Take(500).ToList()
                        Dim page As Integer = 0
                        While tmpCommunities.Count > 0
                            Dim Inserted As List(Of Integer) = (From mp In DC.GetCurrentSession.Linq(Of LazyCommunityModulePermission)() _
                                    Where tmpCommunities.Contains(mp.CommunityID) AndAlso mp.ModuleID = idModule AndAlso mp.RoleID = idRole
                                    Select mp.CommunityID).ToList()

                            Dim IdCommunitiesForInsert As List(Of Integer) = (From id In tmpCommunities Where Inserted.Contains(id) = False Select id).ToList
                            Dim builder As New Text.StringBuilder
                            If IdCommunitiesForInsert.Count > 0 Then
                                IdCommunitiesForInsert.ForEach(Function(id) builder.Append(IIf(builder.Length > 0, vbCrLf & String.Format(sqlInsertCommand, id, permission, idRole), String.Format(sqlInsertCommand, id, permission, idRole))))
                                Dim SqlCommand As String = builder.ToString

                                Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(SqlCommand)
                                dbCommand.Connection = connection
                                dbCommand.CommandType = CommandType.Text
                                dbCommand.ExecuteNonQuery()
                            End If

                            page += 1
                            tmpCommunities = idCommunities.Skip(page * 500).Take(500).ToList()
                        End While


                    End If
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub UpdatePermission(ByVal ModuleID As Integer, ByVal IdCommunities As List(Of Integer), ByVal permissions As List(Of dtoTemplateModulePermission))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = ""
                    Dim UpdateInfo As String = GetSQLupdateUserContext()
                    Dim sqlUpdateCommand As String = "UPDATE LK_SERVIZIO_COMUNITA set LKSC_Permessi='{2}', " & UpdateInfo & "  where ({0}) and LKSC_SRVZ_id={1} and LKSC_TPRL_id={3} and LKSC_Permessi<>'{2}' "

                    Dim id As String = SQLconcatenateColumnId("LKSC_CMNT_id", IdCommunities)
                    For Each item In permissions
                        sqlCommand &= vbCrLf & String.Format(sqlUpdateCommand, id, ModuleID, item.PermissionToString, item.RoleId)
                    Next
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub UpdatePermission(ByVal idRole As Integer, ByVal idModule As Integer, ByVal idCommunities As List(Of Integer), ByVal permission As String)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = ""
                    Dim UpdateInfo As String = GetSQLupdateUserContext()
                    Dim sqlUpdateCommand As String = "UPDATE LK_SERVIZIO_COMUNITA set LKSC_Permessi='{2}', " & UpdateInfo & "  where ({0}) and LKSC_SRVZ_id={1} and LKSC_TPRL_id={3} and LKSC_Permessi<>'{2}' "

                    Dim id As String = SQLconcatenateColumnId("LKSC_CMNT_id", idCommunities)
                    sqlCommand &= vbCrLf & String.Format(sqlUpdateCommand, id, idModule, permission, idRole)
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Function SQLconcatenateColumnId(ByVal ColumnName As String, ByVal IdCommunities As List(Of Integer)) As String
            Dim builder As New Text.StringBuilder

            IdCommunities.ForEach(Function(id) builder.Append(IIf(builder.Length > 0, " OR " & ColumnName & "=" & id, ColumnName & "=" & id & "  ")))
            'Dim SqlCommand As String = builder.ToString


            'If IdCommunities.Count = 1 Then
            '    result = ColumnName & "=" & IdCommunities(0) & "  "
            'Else
            '    result = "( " & ColumnName & "=" & IdCommunities(0)
            '    For Each Id As Integer In IdCommunities.Skip(0).Take(IdCommunities.Count - 2).ToList
            '        result &= " OR " & ColumnName & "=" & Id

            '    Next
            '    result &= ")  "
            'End If
            Return builder.ToString
        End Function

        Private Sub ClearCommunityPermission(ByVal ModuleID As Integer, ByVal IdCommunities As List(Of Integer), ByVal PreserveID As Long)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim id As String = SQLconcatenateColumnId("LKSC_CMNT_id", IdCommunities)

                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand("DELETE FROM LK_SERVIZIO_COMUNITA where (" & id & ") and LKSC_SRVZ_id=" & ModuleID.ToString)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                    Dim IdCommunityForService As String = SQLconcatenateColumnId("SRVC_CMNT_ID", IdCommunities)
                    dbCommand = oDatabase.GetSqlStringCommand("DELETE FROM SERVIZIO_COMUNITA where (" & IdCommunityForService & ") and SRVC_ID<>" & PreserveID.ToString & " and SRVC_SRVZ_ID=" & ModuleID.ToString)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub AddModuleToCommunity(ByVal ModuleID As Integer, ByVal isNotificable As Boolean, ByVal isEnabled As Boolean, ByVal IdCommunities As List(Of Integer))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim oDate As DateTime = DateTime.Now
                    Dim SQLstring = "INSERT INTO SERVIZIO_COMUNITA (SRVC_SRVZ_ID,SRVC_CMNT_ID,SRVC_isAbilitato,SRVC_DataAssociazione,SRVC_DataModifica,SRVC_isNotificabile) "
                    SQLstring &= " VALUES(" & ModuleID & ",{0}," & IIf(isEnabled, 1, 0) & ",'" & oDate.ToString("s") & "','" & oDate.ToString("s") & "'," & IIf(isNotificable, 1, 0) & ") "

                    For Each IdCommunity In IdCommunities

                        Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(String.Format(SQLstring, IdCommunity))
                        dbCommand.Connection = connection
                        dbCommand.CommandType = CommandType.Text
                        dbCommand.ExecuteNonQuery()
                    Next
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub RemoveModuleFromCommunity(ByVal idModule As Integer, ByVal idItems As List(Of Integer), ByVal PreserveID As Long)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    'Dim id As String = SQLconcatenateColumnId("LKSC_CMNT_id", IdCommunities)

                    'Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand("DELETE FROM LK_SERVIZIO_COMUNITA where " & id & " and LKSC_SRVZ_id=" & ModuleID.ToString)
                    'dbCommand.Connection = connection
                    'dbCommand.CommandType = CommandType.Text
                    'dbCommand.ExecuteNonQuery()

                    Dim IdCommunityForService As String = SQLconcatenateColumnId("SRVC_CMNT_ID", idItems)
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand("DELETE FROM SERVIZIO_COMUNITA where (" & IdCommunityForService & ") and SRVC_ID<>" & PreserveID.ToString & " and SRVC_SRVZ_ID=" & idModule.ToString)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub

        Public Sub DefinePermissionFromTemplateToCommunity(ByVal communityType As CommunityType, ByVal IdOrganization As Integer, ByVal IdCommunity As Integer)
            Try
                Dim oCommunities As New List(Of Integer)
                oCommunities.Add(IdCommunity)
                ClearAllCommunityPermission(oCommunities)
                AddAllModulesToCommunity(oCommunities, IdOrganization, communityType.Id)
                ApplyAllPermissionToCommunity(oCommunities, IdOrganization, communityType.Id)
            Catch ex As Exception

            End Try
        End Sub

        Private Sub ClearAllCommunityPermission(ByVal IdCommunities As List(Of Integer))
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim id As String = SQLconcatenateColumnId("LKSC_CMNT_id", IdCommunities)

                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand("DELETE FROM LK_SERVIZIO_COMUNITA where " & id)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                    Dim IdCommunityForService As String = SQLconcatenateColumnId("SRVC_CMNT_ID", IdCommunities)
                    dbCommand = oDatabase.GetSqlStringCommand("DELETE FROM SERVIZIO_COMUNITA where " & IdCommunityForService)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub AddAllModulesToCommunity(ByVal IdCommunities As List(Of Integer), ByVal IdOrganization As Integer, ByVal IdCommunityType As Integer)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim oDate As DateTime = DateTime.Now
                    Dim SQLstring = "INSERT INTO SERVIZIO_COMUNITA (SRVC_SRVZ_ID,SRVC_CMNT_ID,SRVC_isAbilitato,SRVC_DataAssociazione,SRVC_DataModifica,SRVC_isNotificabile) "
                    SQLstring &= " SELECT SRVZ_ID,{0},LKST_default,'" & oDate.ToString("s") & "','" & oDate.ToString("s") & "',SRVZ_isNotificabile "
                    SQLstring &= "FROM LK_SRVZ_TPCM INNER JOIN SERVIZIO ON LKST_SRVZ_id = SRVZ_id" & vbCrLf
                    SQLstring &= " where LKST_TPCM_id= " & IdCommunityType.ToString & " AND LKST_ORGN_id=" & IdOrganization.ToString

                    For Each IdCommunity In IdCommunities

                        Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(String.Format(SQLstring, IdCommunity))
                        dbCommand.Connection = connection
                        dbCommand.CommandType = CommandType.Text
                        dbCommand.ExecuteNonQuery()
                    Next
                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
        Private Sub ApplyAllPermissionToCommunity(IdCommunities As List(Of Integer), ByVal IdOrganization As Integer, ByVal IdCommunityType As Integer)
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me._ConnectionDB)

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim InsertInfo As String = GetSQLinsertUserContext()
                    Dim sqlInsertCommand As String = "INSERT INTO LK_SERVIZIO_COMUNITA (LKSC_CMNT_id, LKSC_SRVZ_id, LKSC_Permessi, LKSC_TPRL_id,_ModifiedOn,_ModifiedBy,_ModifiedIPaddress,_ModifiedProxyIPaddress,_CreatedBy,_CreatedOn,_CreatedIPaddress,_CreatedProxyIPaddress) "

                    sqlInsertCommand &= " SELECT {0},LPRS_SRVZ_id,LPRS_valore,LPRS_TPRL_id, " & InsertInfo
                    sqlInsertCommand &= "FROM LK_SRVZ_TPCM INNER JOIN SERVIZIO ON LKST_SRVZ_id = SRVZ_id" & vbCrLf
                    sqlInsertCommand &= " where LPRS_TPCM_ID= " & IdCommunityType.ToString & " AND LPRS_ORGN_ID=" & IdOrganization.ToString


                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlInsertCommand)
                    dbCommand.Connection = connection
                    dbCommand.CommandType = CommandType.Text
                    dbCommand.ExecuteNonQuery()

                Catch ex As Exception
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub
#End Region


#Region "From Roles"
        Public Function GetAvailableCommunityTypeByRole(ByVal idRole As Integer) As List(Of Integer)
            Return (From m In DC.GetCurrentSession.Linq(Of _RoleCommunityTypeTemplate)()
                    Where m.IdRole = idRole Select m.IdCommunityType).Distinct().ToList()
        End Function
        Public Function GetAvailableModulesByCommunityType(ByVal idCommunityType As Integer) As List(Of Integer)
            Return (From m In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() Where m.IdCommunityType = idCommunityType Select m.IdModule).Distinct().ToList()
        End Function

        Public Function GetAvailableModulesByOrganizations(ByVal idRole As Integer, ByVal idOrganization As Integer) As List(Of Integer)
            Dim availableForModule As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() _
                                             Where m.IdOrganization = idOrganization Select m.IdCommunityType).ToList()
            Dim availableForRole As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of _RoleCommunityTypeTemplate)()
                    Where m.IdRole = idRole Select m.IdCommunityType).ToList()

            Dim availableTypes As List(Of Integer) = availableForModule.Where(Function(c) availableForRole.Contains(c)).ToList()


            Return (From m In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() Where availableTypes.Contains(m.IdCommunityType) AndAlso m.IdOrganization = idOrganization Select m.IdModule).Distinct().ToList()
        End Function

        Public Function GetPermissionByRole(ByVal idRole As Integer, ByVal idModule As Integer, ByVal idOrganization As Integer) As List(Of dtoTemplateRolePermission)
            Dim items As New List(Of dtoTemplateRolePermission)
            Dim availableForModule As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() _
                                               Where m.IdOrganization = idOrganization AndAlso idModule = m.IdModule Select m.IdCommunityType).ToList()
            Dim availableForRole As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of _RoleCommunityTypeTemplate)()
                    Where m.IdRole = idRole Select m.IdCommunityType).ToList()

            Dim availableTypes As List(Of Integer) = availableForModule.Where(Function(c) availableForRole.Contains(c)).ToList()

            Dim permissions As List(Of LazyModulePermissionTemplate) = (From m In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                                                                        Where m.IdOrganization = idOrganization AndAlso idModule = m.IdModule AndAlso idRole = m.IdRole Select m).ToList()


            For Each idType As Integer In availableTypes.Distinct.ToList
                Dim item As New dtoTemplateRolePermission() With {.IdCommunityType = idType}
                Dim permission As String = permissions.Where(Function(p) p.IdCommunityType = (item.IdCommunityType)).OrderByDescending(Function(p) p.Id).Select(Function(p) p.Permission).FirstOrDefault()
                If String.IsNullOrEmpty(permission) Then
                    item.Permission = 0
                Else
                    item.Permission = lm.Comol.Core.DomainModel.PermissionHelper.BinStrToLong(New String(permission.Reverse.ToArray))
                End If
                items.Add(item)
            Next
            Return items
        End Function

        Public Sub SaveRoleTemplate(ByVal idRole As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateRolePermission))
            ApplyToRoleTemplate(idRole, idOrganization, idModule, permissions, False)
        End Sub
        Private Sub ApplyToRoleTemplate(ByVal idRole As Integer, ByVal idOrg As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateRolePermission), ByVal ToAll As Boolean)
            Dim oRole As Role = DC.GetCurrentSession.Get(Of Role)(idRole)
            Dim IdOrganizations As New List(Of Integer)

            If ToAll Then
                IdOrganizations.Add(-1)
                IdOrganizations.AddRange((From c In DC.GetCurrentSession.Linq(Of Organization)() Select c.Id).ToList)
            Else
                IdOrganizations.Add(idOrg)
            End If
            Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(idModule)
            If Not IsNothing(oModuleDefinition) AndAlso IdOrganizations.Count > 0 Then
                Dim EnabledForPortal As Boolean = oModuleDefinition.Available

                ' UPDATE PERMISSION
                UpdateRolePermissionTemplate(idRole, idModule, IdOrganizations, permissions)



                Dim query = (From mt In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                                                       Where mt.IdRole = idRole AndAlso mt.IdModule = idModule AndAlso IdOrganizations.Contains(mt.IdOrganization) _
                                                       Select mt.IdOrganization, mt.IdCommunityType).ToList

                Dim listToInsert As Dictionary(Of Integer, List(Of dtoTemplateRolePermission)) = IdOrganizations.ToDictionary(Function(id) id, _
                           Function(id) (From p In permissions Where (From q In query Where q.IdOrganization = id Select q.IdCommunityType).ToList().Contains(p.IdCommunityType) = False Select p).ToList)

                If (From i In listToInsert Where i.Value.Count > 0 Select i).Count > 0 Then
                    InsertRolePermissionTemplate(idRole, idModule, (From i In listToInsert Where i.Value.Count > 0 Select i).ToDictionary(Function(c) c.Key, Function(c) c.Value))
                End If
            End If
        End Sub

        Public Sub ApplyRoleTemplateToAll(ByVal idRole As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateRolePermission))
            ApplyToRoleTemplate(idRole, idOrganization, idModule, permissions, True)
        End Sub

        Public Sub ApplyToCommunitiesByRole(ByVal idRole As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal templates As List(Of dtoTemplateRolePermission))
            Try
                Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(idModule)


                For Each template As dtoTemplateRolePermission In templates
                    Dim oType As CommunityType = DC.GetCurrentSession.Get(Of CommunityType)(template.IdCommunityType)
                    Dim idItems As List(Of Integer) = (From c In DC.GetCurrentSession.Linq(Of Community)() Where c.TypeOfCommunity Is oType AndAlso (idOrganization = -1 OrElse c.IdOrganization = idOrganization) Select c.Id).ToList

                    If Not IsNothing(oModuleDefinition) Then
                        Dim tmpCommunities As List(Of Integer) = idItems.Skip(0).Take(500).ToList()
                        Dim page As Integer = 0
                        While tmpCommunities.Count > 0
                            Dim query = (From mc In DC.GetCurrentSession().Linq(Of ModuleCommunity)() _
                                                                        Where tmpCommunities.Contains(mc.Community.Id) AndAlso mc.ModuleDefinition Is oModuleDefinition _
                                                                        Group mc By mc.Community.Id Into Group
                                                                        Select Id, Group.Count).ToList()
                            Dim IdCommunitiesToRemove As List(Of Integer) = (From q In query Where q.Count > 1 Select q.Id).ToList
                            If IdCommunitiesToRemove.Count > 0 Then
                                RemoveModuleFromCommunity(idModule, IdCommunitiesToRemove, 0)
                            End If

                            Dim IdCommunitiesToAdd As List(Of Integer) = (From i In tmpCommunities Where Not (From q In query Select q.Id).ToList().Contains(i) Select i).ToList
                            If IdCommunitiesToAdd.Count > 0 Then
                                IdCommunitiesToAdd.AddRange(IdCommunitiesToRemove)
                                AddModuleToCommunity(idModule, oModuleDefinition.isNotificable, oModuleDefinition.Available, IdCommunitiesToAdd)
                            End If
                            page += 1
                            tmpCommunities = tmpCommunities.Skip(page * 500).Take(500).ToList()
                        End While
                    End If
                    If idItems.Count > 0 Then
                        ' UPDATE ROLE / COmmUNITY / PERMISSION
                        UpdatePermission(idRole, idModule, idItems, template.PermissionToString)
                        ApplyPermission(idRole, idModule, idItems, template.PermissionToString)
                    End If
                Next

                'Dim ItemsToApply As New List(Of dtoDB)
               
            Catch ex As Exception

            End Try
        End Sub

#End Region

#Region "From Community"
        Public Function GetAvailableRolesByCommunityType(ByVal idCommunityType As Integer) As List(Of Integer)
            Return (From m In DC.GetCurrentSession.Linq(Of _RoleCommunityTypeTemplate)()
                    Where m.IdCommunityType = idCommunityType Select m.IdRole).Distinct().ToList()
        End Function
        Public Function GetAvailableModulesByCommunityType(ByVal idCommunityType As Integer, ByVal idOrganization As Integer) As List(Of Integer)
            Dim availableForModule As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of LazyModuleTemplate)() _
                                             Where m.IdOrganization = idOrganization AndAlso m.IdCommunityType = idCommunityType Select m.IdModule).ToList()

            Return availableForModule.Distinct().ToList()
        End Function

        Public Function GetPermissionByCommunityType(ByVal idCommunityType As Integer, ByVal idModule As Integer, ByVal idOrganization As Integer) As List(Of dtoTemplateModulePermission)
            Dim items As New List(Of dtoTemplateModulePermission)

            Dim availableRoles As List(Of Integer) = (From m In DC.GetCurrentSession.Linq(Of _RoleCommunityTypeTemplate)()
                    Where m.IdCommunityType = idCommunityType Select m.IdRole).ToList()

            Dim permissions As List(Of LazyModulePermissionTemplate) = (From m In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                                                                        Where m.IdOrganization = idOrganization AndAlso idModule = m.IdModule AndAlso idCommunityType = m.IdCommunityType Select m).ToList()


            For Each idRole As Integer In availableRoles.Distinct.ToList
                Dim item As New dtoTemplateModulePermission() With {.RoleId = idRole}
                Dim permission As String = permissions.Where(Function(p) p.IdRole = (item.RoleId)).OrderByDescending(Function(p) p.Id).Select(Function(p) p.Permission).FirstOrDefault()
                If String.IsNullOrEmpty(permission) Then
                    item.Permission = 0
                Else
                    item.Permission = lm.Comol.Core.DomainModel.PermissionHelper.BinStrToLong(New String(permission.Reverse.ToArray))
                End If
                items.Add(item)
            Next
            Return items
        End Function

        Public Sub SaveCommunityTemplate(ByVal idCommunityType As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateModulePermission))
            ApplyToCommunityTemplate(idCommunityType, idOrganization, idModule, permissions, False)
        End Sub
        Private Sub ApplyToCommunityTemplate(ByVal idCommunityType As Integer, ByVal idOrg As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateModulePermission), ByVal ToAll As Boolean)
            Dim oCommunityType As CommunityType = DC.GetCurrentSession.Get(Of CommunityType)(idCommunityType)
            Dim IdOrganizations As New List(Of Integer)

            If ToAll Then
                IdOrganizations.Add(-1)
                IdOrganizations.AddRange((From c In DC.GetCurrentSession.Linq(Of Organization)() Select c.Id).ToList)
            Else
                IdOrganizations.Add(idOrg)
            End If
            Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(idModule)
            If Not IsNothing(oModuleDefinition) AndAlso IdOrganizations.Count > 0 Then
                Dim EnabledForPortal As Boolean = oModuleDefinition.Available

                ' UPDATE PERMISSION
                UpdatePermissionTemplate(idModule, idCommunityType, IdOrganizations, permissions)


                Dim query = (From mt In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                                                     Where mt.IdCommunityType = idCommunityType AndAlso mt.IdModule = idModule AndAlso IdOrganizations.Contains(mt.IdOrganization) _
                                                     Select mt.IdOrganization, mt.IdRole).ToList


                Dim listToInsert As Dictionary(Of Integer, List(Of dtoTemplateModulePermission)) = IdOrganizations.ToDictionary(Function(id) id, _
                        Function(id) (From p In permissions Where (From q In query Where q.IdOrganization = id Select q.IdRole).ToList().Contains(p.RoleId) = False Select p).ToList)

                If (From i In listToInsert Where i.Value.Count > 0 Select i).Count > 0 Then
                    InsertPermissionTemplate(idModule, idCommunityType, (From i In listToInsert Where i.Value.Count > 0 Select i).ToDictionary(Function(c) c.Key, Function(c) c.Value))
                End If


                'Dim query = (From mt In DC.GetCurrentSession.Linq(Of LazyModulePermissionTemplate)() _
                '                                       Where mt.IdRole = idRole AndAlso mt.IdModule = idModule AndAlso IdOrganizations.Contains(mt.IdOrganization) _
                '                                       Select mt.IdOrganization, mt.IdCommunityType).ToList

                'Dim listToInsert As Dictionary(Of Integer, List(Of dtoTemplateRolePermission)) = IdOrganizations.ToDictionary(Function(id) id, _
                '           Function(id) (From p In permissions Where (From q In query Where q.IdOrganization = id Select q.IdCommunityType).ToList().Contains(p.IdCommunityType) = False Select p).ToList)

                'If (From i In listToInsert Where i.Value.Count > 0 Select i).Count > 0 Then
                '    InsertRolePermissionTemplate(idRole, idModule, (From i In listToInsert Where i.Value.Count > 0 Select i).ToDictionary(Function(c) c.Key, Function(c) c.Value))
                'End If
            End If
        End Sub

        Public Sub ApplyToCommunityTemplateToAll(ByVal idCommunityType As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal permissions As List(Of dtoTemplateModulePermission))
            ApplyToCommunityTemplate(idCommunityType, idOrganization, idModule, permissions, True)
        End Sub

        'Public Sub ApplyToCommunitiesByRole(ByVal idRole As Integer, ByVal idOrganization As Integer, ByVal idModule As Integer, ByVal templates As List(Of dtoTemplateRolePermission))
        '    Try
        '        Dim oModuleDefinition As ModuleDefinition = DC.GetById(Of ModuleDefinition)(idModule)


        '        For Each template As dtoTemplateRolePermission In templates
        '            Dim oType As CommunityType = DC.GetCurrentSession.Get(Of CommunityType)(template.IdCommunityType)
        '            Dim idItems As List(Of Integer) = (From c In DC.GetCurrentSession.Linq(Of Community)() Where c.TypeOfCommunity Is oType AndAlso (idOrganization = -1 OrElse c.IdOrganization = idOrganization) Select c.Id).ToList

        '            If Not IsNothing(oModuleDefinition) Then
        '                Dim tmpCommunities As List(Of Integer) = idItems.Skip(0).Take(500).ToList()
        '                Dim page As Integer = 0
        '                While tmpCommunities.Count > 0
        '                    Dim query = (From mc In DC.GetCurrentSession().Linq(Of ModuleCommunity)() _
        '                                                                Where tmpCommunities.Contains(mc.Community.Id) AndAlso mc.ModuleDefinition Is oModuleDefinition _
        '                                                                Group mc By mc.Community.Id Into Group
        '                                                                Select Id, Group.Count).ToList()
        '                    Dim IdCommunitiesToRemove As List(Of Integer) = (From q In query Where q.Count > 1 Select q.Id).ToList
        '                    If IdCommunitiesToRemove.Count > 0 Then
        '                        RemoveModuleFromCommunity(idModule, IdCommunitiesToRemove, 0)
        '                    End If

        '                    Dim IdCommunitiesToAdd As List(Of Integer) = (From i In tmpCommunities Where Not (From q In query Select q.Id).ToList().Contains(i) Select i).ToList
        '                    If IdCommunitiesToAdd.Count > 0 Then
        '                        IdCommunitiesToAdd.AddRange(IdCommunitiesToRemove)
        '                        AddModuleToCommunity(idModule, oModuleDefinition.isNotificable, oModuleDefinition.Available, IdCommunitiesToAdd)
        '                    End If
        '                    page += 1
        '                    tmpCommunities = tmpCommunities.Skip(page * 500).Take(500).ToList()
        '                End While
        '            End If
        '            If idItems.Count > 0 Then
        '                ' UPDATE ROLE / COmmUNITY / PERMISSION
        '                UpdatePermission(idRole, idModule, idItems, template.PermissionToString)
        '                ApplyPermission(idRole, idModule, idItems, template.PermissionToString)
        '            End If
        '        Next

        '        'Dim ItemsToApply As New List(Of dtoDB)

        '    Catch ex As Exception

        '    End Try
        'End Sub

#End Region
    End Class
End Namespace