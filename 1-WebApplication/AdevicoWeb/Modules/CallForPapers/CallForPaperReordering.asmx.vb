﻿Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports lm.Comol.Modules.CallForPapers.Business
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ScriptService()> _
<ToolboxItem(False)> _
Public Class CallForPaperReordering
    Inherits System.Web.Services.WebService

    Private _service As lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers
    Private CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Private ReadOnly Property Service As ServiceCallOfPapers
        Get
            If IsNothing(_service) Then
                _service = New ServiceCallOfPapers(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    <WebMethod(True), ScriptMethod()> _
    Public Function FieldsReorder(ByVal position As String, ByVal sectionId As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedSubActId As List(Of Long) = Reorder(position)
        sectionId = sectionId.Replace("sect-", "")
        If Not CurrentContext.UserContext Is Nothing AndAlso IsNumeric(sectionId) Then
            Service.UpdateFieldsDisplayOrder(OrderedSubActId, CLng(sectionId))
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function
    <WebMethod(True), ScriptMethod()> _
    Public Function SectionsReorder(ByVal position As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedSubActId As List(Of Long) = Reorder(position)
        If Not CurrentContext.UserContext Is Nothing Then
            Service.UpdateSectionsDisplayOrder(OrderedSubActId)
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If
        Return position
    End Function

    Private Function Reorder(ByVal position As String) As List(Of Long)
        Dim Subs As String()
        Dim OrderedId As New List(Of Long)
        Subs = position.Split("&")

        For Each item As String In Subs
            Dim values As String()
            values = item.Split("=")
            OrderedId.Add(Long.Parse(values(1)))
        Next

        Return OrderedId
    End Function

    '

    <WebMethod(True), ScriptMethod()> _
    Public Function PostJSON() As String
        Return "Ok"
    End Function


    <WebMethod(True), ScriptMethod()> _
    Public Function ExportJSON() As String
        Return "Ok"
    End Function

End Class