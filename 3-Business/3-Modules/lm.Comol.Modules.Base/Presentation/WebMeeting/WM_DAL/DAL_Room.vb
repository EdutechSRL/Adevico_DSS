Imports MySql.Data.MySqlClient

Namespace lm.Comol.Modules.Base.Presentation
    Public Class DAL_Room
#Region "isNullSomething"
        Public Shared Function isNullString(ByVal obj As Object) As String

            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return String.Empty
            Else
                Return obj.ToString()
            End If

        End Function
        Public Shared Function isNullDateMin(ByVal obj As Object) As Date

            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return Date.MinValue
            Else
                Return Convert.ToDateTime(obj)
            End If

        End Function
        Public Shared Function isNullDateMax(ByVal obj As Object) As Date

            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return Date.MaxValue
            Else
                Return Convert.ToDateTime(obj)
            End If

        End Function
        Public Shared Function isNullDecimal(ByVal obj As Object) As Decimal

            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return Decimal.MinValue
            Else
                Return Decimal.Parse(obj)
            End If

        End Function
        Public Shared Function isNullInt(ByVal obj As Object) As Integer
            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return 0 'con integer.minvalue si incasina tutta la gestione delle durate e non si hanno vantaggi
            Else
                Return Integer.Parse(obj)
            End If

        End Function
        Public Shared Function isNullDouble(ByVal obj As Object) As Double
            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return 0
            Else
                Return Double.Parse(obj)
            End If

        End Function
        Public Shared Function isNullBoolean(ByVal obj As Object) As Integer
            If obj Is Nothing Or obj Is System.DBNull.Value Then
                Return False
            Else
                If obj = 0 Then
                    Return False
                Else
                    Return True
                End If
                'Return Boolean.Parse(obj)
            End If

        End Function
#End Region
        'Private Shared ReadOnly Property SysID() As String
        '    Get
        '        Return "test_" 'da sostituire con parametro da config file
        '    End Get
        'End Property
        Public Shared Function loadRoomsByCommunity(ByVal idCommunity As Integer, ByVal dbConnString As String, ByVal RoomPrefix As String) As List(Of Room)
            Dim connection As New MySqlConnection(dbConnString)
            Dim cmd As New MySqlCommand("sp_RoomListByCommunityWithNLogged", connection)
            Dim oRoomList As New List(Of Room)
            cmd.CommandType = CommandType.StoredProcedure
            Dim parIdComm As New MySqlParameter("idCommunity", RoomPrefix & idCommunity)
            cmd.Parameters.Add(parIdComm)
            Using connection
                connection.Open()
                Using mysqlReader As IDataReader = cmd.ExecuteReader
                    While mysqlReader.Read()
                        Dim oRoom As New Room
                        oRoom.name = isNullString(mysqlReader.Item("name"))
                        oRoom.description = isNullString(mysqlReader.Item("comment"))
                        oRoom.isPublic = isNullBoolean(mysqlReader.Item("ispublic"))
                        oRoom.Type = isNullInt(mysqlReader.Item("roomtypes_id"))
                        oRoom.onLineUsers = isNullInt(mysqlReader.Item("nLogged"))
                        oRoom.maxLogged = isNullInt(mysqlReader.Item("numberOfPartizipants"))
                        oRoom.startTime = isNullDateMin(mysqlReader.Item("starttime"))
                        oRoom.demoTime = isNullInt(mysqlReader.Item("demo_time"))
                        oRoom.isDemoRoom = isNullBoolean(mysqlReader.Item("isdemoroom"))
                        oRoom.id = isNullInt(mysqlReader.Item("rooms_id"))

                        'oRoom.hasWhiteboard = mysqlReader.Item("showwhiteboard")
                        oRoomList.Add(oRoom)
                    End While
                    mysqlReader.Close()
                End Using
                connection.Close()
            End Using
            Return oRoomList
        End Function
        Public Shared Sub updateIdCommunity(ByRef idCommunity As Integer, ByRef idRoom As Integer, ByVal dbConnString As String, ByVal RoomPrefix As String)
            Dim conn As New MySqlConnection(dbConnString)
            Dim cmd As New MySqlCommand("sp_updateExternalRoomType", conn)
            Dim oRoomList As New List(Of Room)
            cmd.CommandType = CommandType.StoredProcedure
            Dim parIdComm As New MySqlParameter("idCommunity", RoomPrefix & idCommunity)
            Dim parIdRoom As New MySqlParameter("idRoom", idRoom)
            cmd.Parameters.Add(parIdComm)
            cmd.Parameters.Add(parIdRoom)
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
        End Sub
        Public Shared Sub delete(ByRef idRoomToDelete As Integer)
            Dim oUserService As New WCF_OMuserService.UserServicePortTypeClient
            Dim oRoomService As New WCF_OMroomService.RoomServicePortTypeClient
            ' SESSIONE DI LAVORO
            Dim oResponse As WCF_OMuserService.Sessiondata = oUserService.getSession()
            Dim SessionID As String = oResponse.session_id
            Dim UserID As Long
            Dim oError As WCF_OMuserService.ErrorResult = Nothing

            ' LOGIN DI UN ADMINISTRATOR
            UserID = oUserService.loginUser(SessionID, "gabriele.valentini", "trento")

            ' cancellazione stanza
            oRoomService.deleteRoom(SessionID, idRoomToDelete)
        End Sub
        Public Shared Function GetCommunityIdByRoomId(ByRef idRoom As Integer, ByVal dbConnString As String) As Integer
            Dim conn As New MySqlConnection(dbConnString)
            Dim cmd As New MySqlCommand("sp_CommunityIdByRoomId", conn)
            Dim oRoomList As New List(Of Room)
            cmd.CommandType = CommandType.StoredProcedure
            Dim parIdRoom As New MySqlParameter("idRoom", idRoom)
            cmd.Parameters.Add(parIdRoom)
            conn.Open()
            Dim value As String = cmd.ExecuteScalar()
            conn.Close()
            If value = String.Empty Then
                value = 0
            Else
                value = value.Split("_")(1)
            End If

            Return CInt(value)
        End Function
    End Class
End Namespace
