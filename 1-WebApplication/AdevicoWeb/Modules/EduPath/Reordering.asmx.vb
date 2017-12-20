Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ScriptService()> _
<ToolboxItem(False)> _
Public Class Reordering
    Inherits System.Web.Services.WebService

    Private _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service

    Private CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Private ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property


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

    <WebMethod(True), ScriptMethod()> _
    Public Function SubActivitiesReorder(ByVal position As String) As String


        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedSubActId As List(Of Long) = Reorder(position)

        Dim CRole As Integer = Me.CurrentContext.UserContext.RolesID(0)
        ServiceEP.UpdateSubActivityDisplayOrder(OrderedSubActId, Me.CurrentContext.UserContext.CurrentUserID, CRole, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)

        CurrentContext.DataContext.Dispose()
        _serviceEP = Nothing

        Return position
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function ActivitiesReorder(ByVal position As String) As String
        
        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedActId As List(Of Long) = Reorder(position)

        Dim CRole As Integer = Me.CurrentContext.UserContext.RolesID(0)
        ServiceEP.UpdateActivityDisplayOrder(OrderedActId, Me.CurrentContext.UserContext.CurrentUserID, CRole, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)
        CurrentContext.DataContext.Dispose()

        Return position
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function UnitsReorder(ByVal position As String) As String

        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedUnitsId As List(Of Long) = Reorder(position)

        Dim CRole As Integer = Me.CurrentContext.UserContext.RolesID(0)
        ServiceEP.UpdateUnitDisplayOrder(OrderedUnitsId, Me.CurrentContext.UserContext.CurrentUserID, CRole, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)

        CurrentContext.DataContext.Dispose()

        Return position
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function PathsReorder(ByVal position As String) As String

        CurrentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}

        Dim OrderedPathsId As List(Of Long) = Reorder(position)

        Dim CRole As Integer = Me.CurrentContext.UserContext.RolesID(0)
        ServiceEP.UpdatePathDisplayOrder(OrderedPathsId, Me.CurrentContext.UserContext.CurrentUserID, CRole, OLDpageUtility.ClientIPadress, OLDpageUtility.ProxyIPadress)

        CurrentContext.DataContext.Dispose()

        Return position
    End Function

End Class