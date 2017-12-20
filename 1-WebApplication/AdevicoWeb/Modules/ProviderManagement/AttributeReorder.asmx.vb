Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ScriptService()> _
<ToolboxItem(False)> _
Public Class AttributeReorder
    Inherits System.Web.Services.WebService

    Private _service As lm.Comol.Core.BaseModules.ProviderManagement.Business.ProviderManagementService
    Private CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Private ReadOnly Property Service As lm.Comol.Core.BaseModules.ProviderManagement.Business.ProviderManagementService
        Get
            If IsNothing(_service) Then
                _service = New lm.Comol.Core.BaseModules.ProviderManagement.Business.ProviderManagementService(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    <WebMethod(True), ScriptMethod()> _
    Public Function MacAttributeItemReorder(ByVal position As String) As String
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
        If Not CurrentContext.UserContext Is Nothing Then
            Dim orderedItems As List(Of Long) = Reorder(position)
            Dim mPermission As ModuleProviderManagement = Service.GetPermission()
            If (mPermission.EditProvider OrElse mPermission.Administration) Then
                Service.UpdateMacAttributeItemsDisplayOrder(orderedItems)
            End If

            CurrentContext.DataContext.Dispose()
            _service = Nothing
        End If

        Return position
    End Function

    '<WebMethod(True), ScriptMethod()> _
    'Public Function SubmittersReorder(ByVal position As String) As String
    '    CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

    '    Dim orderedSubmitters As List(Of Long) = Reorder(position)
    '    If Not CurrentContext.UserContext Is Nothing Then
    '        Service.UpdateSubmittersDisplayOrder(orderedSubmitters, CurrentContext.UserContext.RolesID(0))
    '        CurrentContext.DataContext.Dispose()
    '        _service = Nothing
    '    End If

    '    Return position
    'End Function

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