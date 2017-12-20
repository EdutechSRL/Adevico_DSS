Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Web.Services
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.GlossaryNew.Business
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.UI.Presentation

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace := "http://tempuri.org/")> _
<WebServiceBinding(ConformsTo := WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<ScriptService>
Public Class GlossaryService
    Inherits WebService

    Private _service As ServiceGlossary
    Private _Context As iApplicationContext

    Private ReadOnly Property CurrentContext As iApplicationContext
        Get
            If IsNothing(_Context) Then
                _Context = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _Context
        End Get
    End Property

    Private ReadOnly Property Service As ServiceGlossary
        Get
            If IsNothing(_service) Then
                _service = New ServiceGlossary(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    'ESEMPIO DI Metodo per restituire un'oggetto tramite JSON
    '  <WebMethod(True)> _
    '<ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    '  Public Function GetTag(id As Long, disabled As Boolean) As dtoTag
    '      Try
    '          If CurrentContext.UserContext.isAnonymous Then
    '              GenerateException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, False, True)
    '          Else
    '              Dim dto As dtoTag = Service.GetDtoTag(id, Not disabled, GetResource.getValue("GetDefaultLanguageName"), GetResource.getValue("GetDefaultLanguageCode"))
    '              If IsNothing(dto) Then
    '                  GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
    '              Else
    '                  Return dto
    '              End If
    '          End If

    '      Catch ex As Exception
    '          If String.IsNullOrEmpty(ex.Message) Then
    '              GenerateException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
    '          Else : Throw ex
    '          End If
    '      End Try
    '  End Function

    <WebMethod(True)> <ScriptMethod(ResponseFormat := ResponseFormat.Json, UseHttpGet := False)>
    Public Sub StatAddTermView(ByVal idCommunity As Long, ByVal idGlossary As Long, ByVal idTerm As Long, idPerson As Int32)
        Service.StatAddTermView(idCommunity, idGlossary, idTerm, idPerson)
    End Sub

    <WebMethod(True)> <ScriptMethod(ResponseFormat := ResponseFormat.Json, UseHttpGet := False)>
    Public Function GetDtoTermEmbed(ByVal idCommunity As Long, ByVal idGlossary As Long, ByVal idTerm As Long, idPerson As Int32) As DTO_TermEmbed
        Return Service.GetDTO_TermEmbed(idTerm)
    End Function

    <WebMethod(True)> <ScriptMethod(ResponseFormat := ResponseFormat.Json, UseHttpGet := False)>
    Public Function GetDtoTermsEmbed(ByVal idCommunity As Long, ByVal idGlossary As Long, ByVal idTermList As String, idPerson As Int32) As DTO_TermsEmbed
        Dim list As New List(Of Long)
        If Not String.IsNullOrWhiteSpace(idTermList) Then
            Dim ids = idTermList.Split("-")
            For Each id As String In ids
                Dim idValue As Long = - 1
                If Int64.TryParse(id, idValue) Then
                    If (idValue > 0) Then
                        list.Add(idValue)
                    End If
                End If
            Next
        End If
        Return Service.GetDTO_TermsEmbed(idCommunity, idGlossary, list)
    End Function
End Class