Imports NHibernate
Imports NHibernate.Cfg
Imports System.Web
'Imports log4net


Public Class SessionHelper

    Private Shared cfg As Configuration
    Private Shared sessionfactory As ISessionFactory
    Private Shared IcodeonFactory As Dictionary(Of String, ISessionFactory)

    'Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Shared Sub New()

        If HttpContext.Current IsNot Nothing Then
            sessionfactory = FactoryBuilder(HttpContext.Current.Server.MapPath("~/hibernate.cfg.xml.config"))
        Else
            sessionfactory = FactoryBuilder()
        End If

    End Sub

    Public Shared Function FactoryBuilder() As ISessionFactory
        Try
            cfg = New Configuration
            cfg.Configure()
            Return cfg.BuildSessionFactory
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Shared Function FactoryBuilder(ByVal filename As String) As ISessionFactory
        Try
            cfg = New Configuration
            cfg.Configure(filename)
            Return cfg.BuildSessionFactory
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetNewSession() As ISession
        Try

            'test:

            If IsNothing(sessionfactory) Then
                If HttpContext.Current IsNot Nothing Then
                    sessionfactory = FactoryBuilder(HttpContext.Current.Server.MapPath("~/hibernate.cfg.xml.config"))
                Else
                    sessionfactory = FactoryBuilder()
                End If
            End If

            Return sessionfactory.OpenSession
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try

    End Function

    Public Shared Function GetCurrentFactory() As ISessionFactory
        Try
            Return sessionfactory
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try

    End Function

    Public Shared Function GetFactory() As ISessionFactory
        Try
            Return sessionfactory
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try

    End Function

    Public Shared Function GetIcodeonSession(ByVal IcodeonPath) As ISession
        Try
            If IcodeonFactory Is Nothing Then
                IcodeonFactory = New Dictionary(Of String, ISessionFactory)()
                IcodeonFactory(IcodeonPath) = FactoryBuilder(IcodeonPath)
            ElseIf Not IcodeonFactory.ContainsKey(IcodeonPath) Then
                IcodeonFactory(IcodeonPath) = FactoryBuilder(IcodeonPath)
            End If
            Return IcodeonFactory(IcodeonPath).OpenSession
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try

    End Function

    Public Shared Function GetNewIcodeonSession(ByVal IcodeonPath) As ISession
        Try
            If IcodeonFactory Is Nothing Then
                IcodeonFactory = New Dictionary(Of String, ISessionFactory)()
                IcodeonFactory(IcodeonPath) = FactoryBuilder(IcodeonPath)
            ElseIf Not IcodeonFactory.ContainsKey(IcodeonPath) Then
                IcodeonFactory(IcodeonPath) = FactoryBuilder(IcodeonPath)
            End If
            Return IcodeonFactory(IcodeonPath).OpenSession
        Catch ex As Exception
            Debug.Write(ex.ToString)
            'log.Error(ex.ToString)
            Return Nothing
        End Try

    End Function
End Class