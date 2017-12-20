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
Public Class PathForMoocs
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

    <WebMethod(True), ScriptMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

End Class