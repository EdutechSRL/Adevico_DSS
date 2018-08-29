Imports NHibernate
Imports NHibernate.Cfg
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public NotInheritable Class SessionDispatcher
    Private Const ConnectionStringProperty As String = "connection.connection_string"
    Private Shared Factories As New Dictionary(Of String, ISessionFactory)

    Public Shared Function Factory(ConnectionString As String) As ISessionFactory
        Try
            Dim _factory As ISessionFactory 'new Configuration().Configure().BuildSessionFactory();
            If (Factories.ContainsKey(ConnectionString)) Then
                _factory = Factories(ConnectionString)
                If (_factory Is Nothing) Then '//|| factory.IsClosed == true)
                    Factories.Remove(ConnectionString)
                    _factory = GenerateFactory(ConnectionString)
                End If
            Else
                '//factory = new Configuration().Configure().SetProperty(ConnectionStringProperty, ConnectionString).BuildSessionFactory();
                '//Factories.Add(ConnectionString, factory)
                _factory = GenerateFactory(ConnectionString)
            End If
            Return _factory
        Catch ex As Exception
            If (Factories.ContainsKey(ConnectionString)) Then
                Factories.Remove(ConnectionString)
            End If
            Throw ex
        End Try
    End Function

    '     /// <summary>
    '    /// Generate factory for specific DB
    '   /// </summary>
    '  /// <param name="connectionString"></param>
    ' /// <returns></returns>
    Private Shared Function GenerateFactory(connectionString As String) As ISessionFactory
        Dim factory As ISessionFactory = New Configuration().Configure().SetProperty(ConnectionStringProperty, connectionString).BuildSessionFactory()
        Factories.Add(connectionString, factory)
        Return factory
    End Function

    '   /// <summary>
    '  /// Create Session for specific connection
    ' /// </summary>
    '/// <param name="connectionString"></param>
    '/// <returns></returns>
    Public Shared Function NewSession(connectionString As String) As ISession
        Try
            Return Factory(connectionString).OpenSession()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
