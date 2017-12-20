Imports System.Web.Services
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
Public Class EvaluationReordering
    Inherits System.Web.Services.WebService

    Private _service As lm.Comol.Modules.CallForPapers.Business.ServiceEvaluation
    Private CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Private ReadOnly Property Service As ServiceEvaluation
        Get
            If IsNothing(_service) Then
                _service = New ServiceEvaluation(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    <WebMethod(True), ScriptMethod()> _
    Public Function OptionsReorder(ByVal position As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim orderedItems As List(Of Long) = Reorder(position)
        If Not CurrentContext.UserContext Is Nothing Then
            Service.UpdateOptionsDisplayOrder(orderedItems)
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function

    <WebMethod(True), ScriptMethod()>
    Public Function CriteriaReorder(ByVal position As String, ByVal idCommittee As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim orderedItems As List(Of Long) = Reorder(position)
        idCommittee = idCommittee.Replace("committee_", "")
        If Not CurrentContext.UserContext Is Nothing AndAlso IsNumeric(idCommittee) Then
            Service.UpdateCriteriaAdvDisplayOrder(orderedItems, CLng(idCommittee))
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function

    <WebMethod(True), ScriptMethod()>
    Public Function StepsReorderAdv(ByVal position As String, ByVal callId As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim orderedItems As List(Of Long) = position.Split(",").Select(Function(x) Long.Parse(x)).ToList()
        callId = callId.Replace("callId_", "")
        If Not CurrentContext.UserContext Is Nothing AndAlso IsNumeric(callId) Then
            'Service.UpdateCriteriaAdvDisplayOrder(orderedItems, CLng(callId))

            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function

    <WebMethod(True), ScriptMethod()>
    Public Function CriteriaReorderAdv(ByVal position As String, ByVal idCommittee As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim orderedItems As List(Of Long) = Reorder(position)
        idCommittee = idCommittee.Replace("committee_", "")
        If Not CurrentContext.UserContext Is Nothing AndAlso IsNumeric(idCommittee) Then
            Service.UpdateCriteriaAdvDisplayOrder(orderedItems, CLng(idCommittee))
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function CommitteesReorder(ByVal position As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim orderedItems As List(Of Long) = Reorder(position)
        If Not CurrentContext.UserContext Is Nothing Then
            Service.UpdateCommitteesDisplayOrder(orderedItems)
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