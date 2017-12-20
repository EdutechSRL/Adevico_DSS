Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Xml
Imports System.ComponentModel
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel

Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.Tag.Domain

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<System.Web.Script.Services.ScriptService> _
Public Class Tags
    Inherits System.Web.Services.WebService

    Private _service As lm.Comol.Core.BaseModules.Tags.Business.ServiceTags
    Private _Context As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_Context) Then
                _Context = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _Context
        End Get
    End Property
    Private ReadOnly Property Service As lm.Comol.Core.BaseModules.Tags.Business.ServiceTags
        Get
            If IsNothing(_service) Then
                _service = New lm.Comol.Core.BaseModules.Tags.Business.ServiceTags(CurrentContext)
            End If
            Return _service
        End Get
    End Property

    Private _Resource As ResourceManager
    Protected Function GetResource() As ResourceManager
        If Not IsNothing(_Resource) Then
            Return _Resource
        Else
            _Resource = New ResourceManager
            _Resource.UserLanguages = CurrentContext.UserContext.Language.Code
            _Resource.ResourcesName = "pg_Dashboard"
            _Resource.Folder_Level1 = "Modules"
            _Resource.Folder_Level2 = "Dashboard"
            _Resource.setCulture()
            Return _Resource
        End If
    End Function


    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod(True)> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetTagLink(id As Int64, idLanguage As Integer) As dtoTagApply
        Dim dto As dtoTagApply
        Try
            If CurrentContext.UserContext.isAnonymous Then
                GenerateLinkException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, False, True)
            Else

                dto = Service.GetDtoTagApply(id, idLanguage, GetResource.getValue("GetDefaultLanguageName"), GetResource.getValue("GetDefaultLanguageCode"))
                If IsNothing(dto) Then
                    GenerateLinkException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
                Else
                    Return dto
                End If
            End If
        Catch exTag As lm.Comol.Core.BaseModules.Tags.Business.TagException
            Dim err As lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTag) = exTag.Error
            err.Message = GetResource.getValue("TagException.ErrorMessage." & exTag.ErrorType.ToString)
            err.Level = GetLevel(exTag.ErrorType)
            err.MessageDialog = True
            Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)

        Catch exJson As lm.Comol.Core.DomainModel.Domain.JsonException
            Throw exJson
        Catch ex As Exception
            If String.IsNullOrEmpty(ex.Message) Then
                GenerateLinkException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
            Else : Throw ex
            End If
        End Try

        If IsNothing(dto) Then
            dto = New dtoTagApply() With {.AllCommunityTypes = False, .OnlyCommunityWithoutTag = True}
            dto.IsReadonly = True
        End If

        Return dto
    End Function

    <WebMethod(True)> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SetTagLink(dto As dtoTagApply) As dtoTagApply
        Try
            If CurrentContext.UserContext.isAnonymous Then
                GenerateLinkException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, False, True)
            Else
                If Not (dto.OnlyCommunityWithoutTag OrElse dto.OnlyCommunityWithoutThisTag) Then
                    Return dto
                Else
                    Dim result As dtoTagApply = Service.ApplyTagTo(dto)
                    If IsNothing(result) Then
                        GenerateLinkException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.GenericError, True)
                    Else
                        Return dto
                    End If
                End If
            End If
        Catch exTag As lm.Comol.Core.BaseModules.Tags.Business.TagException
            Dim err As lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTag) = exTag.Error
            err.Message = GetResource.getValue("TagException.ErrorMessage." & exTag.ErrorType.ToString)
            err.Level = GetLevel(exTag.ErrorType)
            err.MessageDialog = True
            Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)

        Catch exJson As lm.Comol.Core.DomainModel.Domain.JsonException
            Throw exJson

        Catch ex As Exception
            If String.IsNullOrEmpty(ex.Message) Then
                GenerateLinkException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.GenericError, True)
            Else : Throw ex
            End If
        End Try
        Return dto
    End Function

    <WebMethod(True)> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetTag(id As Long, disabled As Boolean) As dtoTag
        Try
            If CurrentContext.UserContext.isAnonymous Then
                GenerateException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, False, True)
            Else
                Dim dto As dtoTag = Service.GetDtoTag(id, Not disabled, GetResource.getValue("GetDefaultLanguageName"), GetResource.getValue("GetDefaultLanguageCode"))
                If IsNothing(dto) Then
                    GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
                Else
                    Return dto
                End If
            End If

        Catch ex As Exception
            If String.IsNullOrEmpty(ex.Message) Then
                GenerateException(Nothing, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingTag, False, True)
            Else : Throw ex
            End If
        End Try
    End Function

    <WebMethod(True)> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SetTag(dto As dtoTag, search As Dictionary(Of String, String)) As dtoTag
        Try
            'GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, True, True)
            If CurrentContext.UserContext.isAnonymous Then
                GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SessionTimeout, False, True)
            Else
                Dim forOrganization As Boolean = False
                If search.ContainsKey(UrlKeyTags.forOrganization.ToString) AndAlso Not String.IsNullOrEmpty(search(UrlKeyTags.forOrganization.ToString)) Then
                    Boolean.TryParse(search(UrlKeyTags.forOrganization.ToString), forOrganization)
                End If
                Dim idOrganization As Integer = 0
                If forOrganization Then
                    idOrganization = Service.GetIdOrganization(CurrentContext.UserContext.CurrentCommunityID)
                Else
                    idOrganization = -1
                End If
                If idOrganization = 0 Then
                    GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.MissingIdOrganization, False, True)
                Else
                    Dim result As dtoTag = Service.SaveTag(dto, TagType.Community, GetResource.getValue("GetDefaultLanguageName"), GetResource.getValue("GetDefaultLanguageCode"))
                    If IsNothing(result) Then
                        GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.GenericError, True)
                    Else
                        Return result
                    End If
                End If
            End If
        Catch exTag As lm.Comol.Core.BaseModules.Tags.Business.TagException
            'Dim err As lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTag) = exTag.Error
            'err.Message = GetResource.getValue("TagException.ErrorMessage." & exTag.ErrorType.ToString)
            'err.Level = GetLevel(exTag.ErrorType)
            'err.MessageDialog = True
            'Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)

            GenerateException(exTag.Error.ReturnObject, exTag.ErrorType, True, False)

            'Dim err As New lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTag) With {.ReturnObject = dto}
            'err.Message = GetResource.getValue("TagException.ErrorMessage." & t.ToString)
            'err.Level = GetLevel(t)
            'err.MessageDialog = inDialog
            'err.CloseDialog = closeDialog
            'Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)


        Catch exJson As lm.Comol.Core.DomainModel.Domain.JsonException
            Throw exJson

        Catch ex As Exception
            If String.IsNullOrEmpty(ex.Message) Then
                GenerateException(dto, lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.GenericError, True)
            Else : Throw ex
            End If
        End Try
    End Function
    Private Enum UrlKeyTags
        recycle = 0
        forOrganization = 1
    End Enum
    Private Sub GenerateException(dto As dtoTag, t As lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType, Optional inDialog As Boolean = True, Optional closeDialog As Boolean = False)
        Dim err As New lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTag) With {.ReturnObject = dto}
        err.Message = GetResource.getValue("TagException.ErrorMessage." & t.ToString)
        err.Level = GetLevel(t)
        err.MessageDialog = inDialog
        err.CloseDialog = closeDialog
        Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)
    End Sub
    Private Sub GenerateLinkException(dto As dtoTagApply, t As lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType, Optional inDialog As Boolean = True, Optional closeDialog As Boolean = False)
        Dim err As New lm.Comol.Core.DomainModel.Domain.JsonError(Of dtoTagApply) With {.ReturnObject = dto}
        err.Message = GetResource.getValue("TagException.ErrorMessage." & t.ToString)
        'err.Level = GetLevel(t)
        err.MessageDialog = inDialog
        err.CloseDialog = closeDialog
        Throw New lm.Comol.Core.DomainModel.Domain.JsonException(err)
    End Sub
    Private Function GetLevel(t As lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType) As String
        Dim cssCLass As String
        Select Case t
            Case lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.DefaultTranslationDuplicate,
                 lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.DefaultTranslationMissing,
                lm.Comol.Core.BaseModules.Tags.Business.ErrorMessageType.SavingTileTranslations
                cssCLass = lm.Comol.Core.DomainModel.Helpers.MessageType.alert.ToString
            Case Else
                cssCLass = lm.Comol.Core.DomainModel.Helpers.MessageType.error.ToString
        End Select


        Return cssCLass
    End Function
End Class