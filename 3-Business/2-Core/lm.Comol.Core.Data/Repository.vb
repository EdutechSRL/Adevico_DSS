Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NHibernate
Imports NHibernate.Criterion


Public Class Repository(Of T) : Implements IDisposable
    Private session As ISession
    Private tx As ITransaction

    'public Repository()
    '{
    '    //session = SessionHelper.GetNewSession();            
    '}
    Public Sub New(ByVal session As ISession)
        MyBase.New()
        Me.session = session
    End Sub

    ''' <summary>
    ''' Reports whether this <c>ObjectSpaceServices</c> contain any changes which must be synchronized with the database
    ''' </summary>
    Public ReadOnly Property IsDirty() As Boolean
        Get
            Return session.IsDirty
        End Get
    End Property

    ''' <summary>
    ''' Reports whether this <c>ObjectSpaceServices</c> is working transactionally
    ''' </summary>
    Public ReadOnly Property IsInTransaction() As Boolean
        Get
            Return ((Not (session.Transaction) Is Nothing) _
                        AndAlso session.Transaction.IsActive)
        End Get
    End Property

    ''' <summary>
    ''' Adds an object to the repository
    ''' </summary>
    ''' <param name="item">The object to add</param>
    Public Sub Add(ByVal item As T)
        If (item Is Nothing) Then
            Throw New ArgumentNullException
        End If
        session.Save(item)
        If Not Me.IsInTransaction Then
            session.Flush()
        End If
    End Sub

    ''' <summary>
    ''' Deletes an object from the repository
    ''' </summary>
    ''' <param name="item">The object to delete</param>
    Public Sub Delete(ByVal item As T)
        If (item Is Nothing) Then
            Throw New ArgumentNullException
        End If
        session.Delete(item)
        If Not Me.IsInTransaction Then
            session.Flush()
        End If
    End Sub

    ''' <summary>
    ''' Saves an object to the repository
    ''' </summary>
    ''' <param name="item">The object to save</param>
    Public Sub Save(ByVal item As T)
        If (item Is Nothing) Then
            Throw New ArgumentNullException
        End If
        session.Update(item)
        If Not Me.IsInTransaction Then
            session.Flush()
        End If
    End Sub

    ''' <summary>
    ''' Begins a transaction
    ''' </summary>
    ''' <exception cref="InvalidOperationException">Thrown if there is an already active transaction</exception>
    Public Sub BeginTransaction()
        If Me.IsInTransaction Then
            Throw New InvalidOperationException("A transaction is already opened")
        Else
            Try
                tx = session.BeginTransaction
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Commits the active transaction
    ''' </summary>
    ''' <exception cref="InvalidOperationException">Thrown if there isn't an active transaction</exception>
    Public Sub Commit()
        If Not Me.IsInTransaction Then
            Throw New InvalidOperationException("Operation requires an active transaction")
        Else
            Try
                tx.Commit()
                tx.Dispose()
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Rollbacks the active transaction
    ''' </summary>
    ''' <exception cref="InvalidOperationException">Thrown if there isn't an active transaction</exception>        
    Public Sub Rollback()
        If Not Me.IsInTransaction Then
            Throw New InvalidOperationException("Operation requires an active transaction")
        Else
            Try
                tx.Rollback()
                tx.Dispose()
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Retrieves all the persisted instances of a given type
    ''' </summary>
    ''' <typeparam name="T">The type of the object to retrieve</typeparam>
    ''' <returns>The list of persistent objects</returns>
    Public Overloads Function GetAll() As IList
        Return GetAll(0, 0)
    End Function

    ''' <summary>
    ''' Retrieves all the persisted instances of a given type
    ''' </summary>
    ''' <typeparam name="T">The type of the object to retrieve</typeparam>
    ''' <param name="pageIndex">The index of the page to retrieve</param>
    ''' <param name="pageSize">The size of the page to retrieve</param>
    ''' <returns>The list of persistent objects</returns>
    Public Overloads Function GetAll(ByVal pageIndex As Integer, ByVal pageSize As Integer) As IList
        Dim criteria As ICriteria = session.CreateCriteria(GetType(T))
        criteria.SetFirstResult((pageIndex * pageSize))
        If (pageSize > 0) Then
            criteria.SetMaxResults(pageSize)
        End If
        Return criteria.List(Of T)()
    End Function

    ''' <summary>
    ''' Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance.
    ''' </summary>
    ''' <typeparam name="T">The type of the object</typeparam>
    ''' <param name="id">The identifier of the object</param>
    ''' <returns>The persistent instance or null</returns>
    Public Overloads Function GetById(Of IdType)(ByVal key As IdType) As T
        Return session.Load(Of T)(key)
    End Function

    ''' <summary>
    ''' Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance.
    ''' </summary>
    ''' <typeparam name="T">The type of the object</typeparam>
    ''' <param name="id">The identifier of the object</param>
    ''' <returns>The persistent instance or null</returns>
    Public Overloads Function GetById(ByVal key As Object) As T
        Return session.Load(Of T)(key)
    End Function

    ''' <summary>
    ''' Returns the amount of objects of a given type
    ''' </summary>
    ''' <typeparam name="T">The type of the object</typeparam>
    ''' <returns>The amount of objects</returns>
    Public Function GetCount() As Integer
        Dim criteria As ICriteria = session.CreateCriteria(GetType(T))
        criteria.SetProjection(Projections.RowCount)
        Return CType(criteria.List()(0), Integer)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            session.Dispose()
        Finally

        End Try
    End Sub
End Class