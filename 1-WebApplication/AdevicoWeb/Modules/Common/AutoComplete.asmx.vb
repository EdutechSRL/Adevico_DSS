Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.BaseModules.ProfileManagement.Business

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False), ScriptService()> _
Public Class CommonAutoComplete
    Inherits System.Web.Services.WebService

    Private CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _service As ProfileManagementService

    Private ReadOnly Property Service As ProfileManagementService
        Get
            If IsNothing(_service) Then
                _service = New ProfileManagementService(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    <WebMethod(True), ScriptMethod()> _
    Public Function AgencyNames(filter As String, idOrganization As String) As List(Of StringItem(Of String))
        Dim results As List(Of StringItem(Of String))
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        If Not CurrentContext.UserContext Is Nothing Then
            Dim idOrgn As Integer = 0
            If Not String.IsNullOrEmpty(idOrganization) AndAlso IsNumeric(idOrganization) Then
                Integer.TryParse(idOrganization, idOrgn)
            End If
            results = Service.GetAgenciesByName(idOrgn, filter)
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        Else
            results = New List(Of StringItem(Of String))
        End If
        Return results
    End Function
    <WebMethod(True), ScriptMethod()> _
    Public Function AgencyNamesByUser(filter As String, idUser As String) As List(Of StringItem(Of String))
        Dim results As List(Of StringItem(Of String))
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        If Not CurrentContext.UserContext Is Nothing Then
            Dim idPerson As Integer = 0
            If Not String.IsNullOrEmpty(idUser) AndAlso IsNumeric(idUser) Then
                Integer.TryParse(idUser, idPerson)
            End If
            results = Service.GetAgenciesByUser(idPerson, filter)
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        Else
            results = New List(Of StringItem(Of String))
        End If
        Return results
    End Function
    <WebMethod(True), ScriptMethod()> _
    Public Function AgencySystemNames(filter As String) As List(Of StringItem(Of String))
        Dim results As List(Of StringItem(Of String))
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        If Not CurrentContext.UserContext Is Nothing Then
            results = Service.GetAgenciesByUser(CurrentContext.UserContext.CurrentUserID, filter)
            CurrentContext.DataContext.Dispose()
            _service = Nothing
        Else
            results = New List(Of StringItem(Of String))
        End If
        Return results
    End Function
End Class