Imports NHibernate
Imports NHibernate.Criterion
Imports NHibernate.Criterion.Expression
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel


'Imports log4net


Public Class DataContext
    Implements IDisposable, iDataContext
    Private session As ISession
    Private tx As ITransaction

    'Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()
        'session = SharedSession.GetNewSession
        session = SessionHelper.GetNewSession
        If session Is Nothing Then
            Throw New Exception("Session NHibernate non inizializzata")
        End If

    End Sub

    Public Sub New(ByVal session As ISession)
        'session = SharedSession.GetNewSession
        Me.session = session
        If session Is Nothing Then
            Throw New Exception("Session NHibernate non inizializzata")
        End If
    End Sub

#Region "CRUD"
    Public Sub Add(ByVal item As Object) Implements iDataContext.Add
        If item Is Nothing Then
            Throw New ArgumentNullException
        End If
        session.Save(item)
        If Not Me.isInTransaction Then
            session.Flush()
        End If
    End Sub

    Public Sub Delete(ByVal item As Object) Implements iDataContext.Delete
        If item Is Nothing Then
            Throw New ArgumentNullException
        End If
        session.Delete(item)
        If Not Me.isInTransaction Then
            session.Flush()
        End If
    End Sub

    Public Sub Save(ByVal item As Object) Implements iDataContext.Save
        If item Is Nothing Then
            Throw New ArgumentNullException
        End If
        session.Update(item)
        If Not Me.isInTransaction Then
            session.Flush()
        End If
    End Sub

    Public Sub SaveOrUpdate(ByVal item As Object) Implements iDataContext.SaveOrUpdate
        If item Is Nothing Then
            Throw New ArgumentNullException
        End If
        session.SaveOrUpdate(item)
        If Not Me.isInTransaction Then
            session.Flush()
        End If
    End Sub
#End Region

    Public ReadOnly Property isDirty() As Boolean Implements iDataContext.isDirty
        Get
            Return session.IsDirty
        End Get
    End Property
    Public ReadOnly Property isInTransaction() As Boolean Implements iDataContext.isInTransaction
        Get
            If session.Transaction IsNot Nothing Then
                Return session.Transaction.IsActive
            Else
                Return False
            End If
        End Get
    End Property

#Region "Transaction"
    Public Sub BeginTransaction() Implements iDataContext.BeginTransaction
        If Me.isInTransaction Then
            Throw New InvalidOperationException("A transaction already open")
        Else
            Try
                tx = session.BeginTransaction()
            Catch ex As Exception
                Throw New TransactionException(ex.Message)
            End Try
        End If
    End Sub
    Public Sub Commit() Implements iDataContext.Commit
        If Not Me.isInTransaction Then
            Throw New InvalidOperationException("Operation requires an active transaction")
        Else
            Try
                tx.Commit()
                tx.Dispose()
            Catch ex As Exception
                Throw New TransactionException(ex.Message)
            End Try
        End If
    End Sub
    Public Sub Rollback() Implements iDataContext.Rollback
        If Not Me.isInTransaction Then
            Throw New InvalidOperationException("Operation requires an active transaction")
        Else
            Try
                tx.Rollback()
                tx.Dispose()
            Catch ex As Exception
                Throw New TransactionException(ex.Message)
            End Try
        End If
    End Sub
#End Region

    Public Function AddPaging(ByVal criteria As ICriteria, ByVal pageIndex As Integer, ByVal pageSize As Integer) As ICriteria Implements iDataContext.AddPaging
        criteria.SetFirstResult(pageIndex * pageSize)
        If pageSize > 0 Then
            criteria.SetMaxResults(pageSize)
        End If
        Return criteria
    End Function

    Public Function AddPaging(ByVal query As IQuery, ByVal pageIndex As Integer, ByVal pageSize As Integer) As IQuery Implements iDataContext.AddPaging
        query.SetFirstResult(pageIndex * pageSize)
        If pageSize > 0 Then
            query.SetMaxResults(pageSize)
        End If
        Return query
    End Function

    Public Function GetAll(Of T)(Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetAll
        Return GetAll(Of T)(0, 0, fetchplan)
    End Function

    Public Function GetAll(Of T)(ByVal pageIndex As Integer, ByVal pageSize As Integer, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetAll
        Dim criteria As ICriteria = session.CreateCriteria(GetType(T))
        AddPaging(criteria, pageIndex, pageSize)
        Return criteria.List(Of T)()
    End Function

    Public Function GetByCriteria(Of T)(ByVal criteria As ICriteria, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetByCriteria
        Return GetByCriteria(Of T)(criteria, 0, 0, fetchplan)
    End Function

    Public Function GetByCriteria(Of T)(ByVal criteria As ICriteria, ByVal pageIndex As Integer, ByVal pageSize As Integer, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetByCriteria
        AddPaging(criteria, pageIndex, pageSize)
        Return criteria.List(Of T)()
    End Function

    Public Function GetByCriteriaUnique(Of T)(ByVal criteria As ICriteria, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As T Implements iDataContext.GetByCriteriaUnique
        Return criteria.UniqueResult(Of T)()
    End Function

    Public Function GetByQuery(Of T)(ByVal query As IQuery, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetByQuery
        Return GetByQuery(Of T)(query, 0, 0, fetchplan)
    End Function

    Public Function GetByQuery(Of T)(ByVal query As IQuery, ByVal pageIndex As Integer, ByVal pageSize As Integer, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As IList(Of T) Implements iDataContext.GetByQuery
        AddPaging(query, pageIndex, pageSize)
        Return query.List(Of T)()
    End Function

    Public Function GetByQueryUnique(Of T)(ByVal query As IQuery, Optional ByVal fetchplan As FetchMode = FetchMode.Default) As T Implements iDataContext.GetByQueryUnique
        Return query.UniqueResult(Of T)()
    End Function

    Public Function GetCount(Of T)() As Integer Implements iDataContext.GetCount
        Dim criteria As ICriteria = session.CreateCriteria(GetType(T))
        criteria.SetProjection(Projections.RowCount())
        Return criteria.UniqueResult
    End Function

    Public Function GetCount(Of T)(ByVal criteria As ICriteria) As Integer Implements iDataContext.GetCount
        criteria.SetProjection(Projections.RowCount())
        Return criteria.UniqueResult
    End Function

    Public Function GetById(Of T)(ByVal id As Object) As T Implements iDataContext.GetById
        'Return session.Load(Of T)(id)
        Return session.Get(Of T)(id)
    End Function

    Public Sub Update(Of t)(ByVal item As t) Implements iDataContext.Update
        session.Update(item)
    End Sub

    Public Sub Refresh(Of t)(ByVal item As t) Implements iDataContext.Refresh
        session.Refresh(item)
    End Sub

    Public Function CreateCriteria(Of T)() As ICriteria Implements iDataContext.CreateCriteria
        Return session.CreateCriteria(GetType(T))
    End Function

    Public Function CreateQuery(ByVal query As String) As IQuery Implements iDataContext.CreateQuery
        Return session.CreateQuery(query)
    End Function

#Region "iDisposable Members"
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' NO_TODO: free other state (managed objects).

                session.Flush()
                session.Close()
            End If

            ' NO_TODO: free your own state (unmanaged objects).
            ' NO_TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
#End Region

    Public Function GetCurrentSession() As NHibernate.ISession Implements DomainModel.iDataContext.GetCurrentSession
        Return session
    End Function
End Class