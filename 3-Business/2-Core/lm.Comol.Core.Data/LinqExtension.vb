Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NHibernate
Imports NHibernate.Linq
Imports System.Linq.Expressions
Imports System.Runtime.CompilerServices

Public Module LinqExtension
    <System.Runtime.CompilerServices.Extension()> _
         Public Function [AndAlso](Of T)(ByVal predicate1 As Expression(Of Func(Of T, Boolean)), ByVal predicate2 As Expression(Of Func(Of T, Boolean))) As Expression(Of Func(Of T, Boolean))
        Return Expression.Lambda(Of Func(Of T, Boolean))(Expression.[AndAlso](predicate1, predicate2))
    End Function
    <System.Runtime.CompilerServices.Extension()> _
    Public Function [OrElse](Of T)(ByVal predicate1 As Expression(Of Func(Of T, Boolean)), ByVal predicate2 As Expression(Of Func(Of T, Boolean))) As Expression(Of Func(Of T, Boolean))
        Return Expression.Lambda(Of Func(Of T, Boolean))(Expression.[OrElse](predicate1, predicate2))
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function OrderBy(Of TEntity As Class)(ByVal source As IQueryable(Of TEntity), ByVal orderByProperty As String, ByVal desc As Boolean) As IQueryable(Of TEntity)

        Dim command As String = If(desc, "OrderByDescending", "OrderBy")

        Dim type = GetType(TEntity)

        Dim [property] = type.GetProperty(orderByProperty)

        Dim parameter = Expression.Parameter(type, "p")

        Dim propertyAccess = Expression.MakeMemberAccess(parameter, [property])

        Dim orderByExpression = Expression.Lambda(propertyAccess, parameter)


        Dim resultExpression = Expression.[Call](GetType(Queryable), command, New Type() {type, [property].PropertyType}, source.Expression, Expression.Quote(orderByExpression))

        Return source.Provider.CreateQuery(Of TEntity)(resultExpression)

    End Function



    ''' <summary>
    ''' It isn't pure functional programming
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ForEach(Of T)(ByVal source As IEnumerable(Of T), ByVal act As Action(Of T)) As IEnumerable(Of T)
        For Each element As T In source
            act(element)
        Next
        Return source
    End Function

    ''' <summary>
    ''' It isn't pure functional programming
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="act"></param>
    <Extension()> _
    Public Sub ForEachAndStop(Of T)(ByVal source As IEnumerable(Of T), ByVal act As Action(Of T))
        For Each element As T In source
            act(element)
        Next
    End Sub

End Module