
Imports Comol.Entity
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.DomainModel

Imports NHibernate
Imports NHibernate.Linq
Imports Comol.Entity.Configuration.Components
Imports COL_BusinessLogic_v2.Comol.Manager
Imports Comol.Entity.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Namespace lm.Comol.Modules.Base.BusinessLogic
    Public Class ManagerNoticeBoardSQL

        Public Sub New()
        End Sub

        Private ReadOnly Property GetCurrentDB() As ConnectionDB
            Get
                Return ManagerConfiguration.GetInstance.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL)
            End Get
        End Property

        Public Function GetByID(ByVal NoticeboardID As Long) As NoticeBoard
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase(Me.GetCurrentDB.Name)
            Dim oNoticeBoard As NoticeBoard = Nothing

            Using connection As DbConnection = oDatabase.CreateConnection()
                connection.Open()

                Try
                    Dim sqlCommand As String = "SELECT NTCB_ID, NTCB_CMNT_ID from Noticeboard where NTCB_ID=" & NoticeboardID
                    Dim dbCommand As DbCommand = oDatabase.GetSqlStringCommand(sqlCommand)
                    dbCommand.Connection = connection

                    Using oDatareader As IDataReader = dbCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                        While oDatareader.Read()
                            oNoticeBoard = New NoticeBoard With {.Id = oDatareader("NTCB_ID")}
                            If IsDBNull(oDatareader("NTCB_CMNT_ID")) Then
                                oNoticeBoard.CommunityOwner = Nothing
                            Else
                                oNoticeBoard.CommunityOwner = New lm.Comol.Core.DomainModel.Community

                                oNoticeBoard.CommunityOwner.Id = oDatareader("NTCB_CMNT_ID")
                            End If
                        End While
                        oDatareader.Close()
                    End Using
                Catch ex As Exception

                Finally
                    If connection.State <> ConnectionState.Closed Then
                        connection.Close()
                    End If
                End Try
            End Using
            Return oNoticeBoard
        End Function

    End Class
End Namespace