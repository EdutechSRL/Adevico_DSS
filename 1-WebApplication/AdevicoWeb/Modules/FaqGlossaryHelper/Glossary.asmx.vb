Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation
'USATO!: 'Imports COL_BusinessLogic_v2.UCServices  <-- RIVEDERE LOGICHE PERMESSI, PERCHE? USA COL_BUSINESS LOGIC V2 (Scopiazzato da MVC!!!)
Imports Gls = lm.Comol.Modules.Standard.Glossary

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<ScriptService()> _
Public Class Glossary
    Inherits System.Web.Services.WebService

#Region "Context"

    Private _currentContext As iApplicationContext
    Private _service As Gls.Business.ServiceGlossary
    Private _module As lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary

    Private ReadOnly Property CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_currentContext) Then
                _currentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _currentContext
        End Get
    End Property
    Private ReadOnly Property Service As Gls.Business.ServiceGlossary
        Get
            If IsNothing(_service) Then
                _service = New Gls.Business.ServiceGlossary(CurrentContext)
                ' CurrentContext.DataContext.GetCurrentSession())
            End If
            Return _service
        End Get
    End Property

    ' RIVEDERE CON LOGICHE PIU' COMOL - LIKE (Questo è scopiazzato da MVC...)
    Private ReadOnly Property GlsModule As lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary
        Get
            If IsNothing(_module) Then


                Dim oModule As lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary
                'ModuleFaq oModule = null;

                Dim UserTypeId As Int32 = -1
                'Int32 UserTypeId = -1;

                If (CurrentContext.UserContext.CurrentCommunityID = 0) Then
                    oModule = lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary.CreatePortalmodule(UserTypeId)

                Else
                    oModule = (From sb In COL_BusinessLogic_v2.Comol.Manager.ManagerPersona.GetPermessiServizio( _
                                   CurrentContext.UserContext.CurrentUser.Id, COL_BusinessLogic_v2.UCServices.Services_Faq.Codex) _
                                   Where (sb.CommunityID = CurrentContext.UserContext.CurrentCommunityID) _
                                   Select New lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary((New COL_BusinessLogic_v2.UCServices.Services_Glossary(sb.PermissionString)).ConvertToLong())
                                    ).FirstOrDefault()
                    If IsNothing(oModule) Then
                        oModule = New lm.Comol.Modules.Standard.Glossary.Domain.ModuleGlossary()
                    End If
                End If

                _module = oModule
            End If

            Return _module
        End Get
    End Property

#End Region



#Region "Web Method"

    ''' <summary>
    ''' Get specific glossary Term converted to an Html String
    ''' </summary>
    ''' <param name="ItemId">Glossary Term ID</param>
    ''' <param name="Htmltype">
    ''' ''' Render Type:
    ''' 1  Ul/li
    ''' 2  Dl/Dt
    ''' 3  span (con tag)
    ''' Default value = 1
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod(True), ScriptMethod()> _
    Public Function GetGlossaryString(ByVal ItemId As Int64, ByVal Htmltype As Integer) As String
        Dim HTML As String = ""

        If Not IsNothing(CurrentContext.UserContext) _
            AndAlso ItemId > 0 _
            AndAlso GlsModule.ViewTerm Then
            Dim _glsi As lm.Comol.Modules.Standard.Glossary.DTO_GlsItem = Service.GetItemDto(ItemId)

            If Not IsNothing(_glsi) AndAlso Not String.IsNullOrEmpty(_glsi.Term) AndAlso Not String.IsNullOrEmpty(_glsi.Definition) Then
                If (Htmltype < 1 OrElse Htmltype > 3) Then
                    Htmltype = 1
                End If

                HTML = Me.RenderItem(_glsi, Htmltype)
            End If
        End If

        Return HTML
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function GetGlossary(ByVal ItemId As Int64, ByVal Htmltype As Integer) As lm.Comol.Modules.Standard.Glossary.DTO_GlsItem

        Dim _glsi As lm.Comol.Modules.Standard.Glossary.DTO_GlsItem

        If Not IsNothing(CurrentContext.UserContext) _
            AndAlso ItemId > 0 _
            AndAlso GlsModule.ViewTerm Then
            _glsi = Service.GetItemDto(ItemId)

            If IsNothing(_glsi) Then
                _glsi = New lm.Comol.Modules.Standard.Glossary.DTO_GlsItem()
            End If
        End If

        Return _glsi
    End Function

#End Region


#Region "Internal"
    Private Function RenderItem(ByVal _glsi As lm.Comol.Modules.Standard.Glossary.DTO_GlsItem, ByVal Htmltype As Integer) As String
        Dim HTML As String = ""

        Select Case Htmltype
            Case 1
                HTML = "<ul>"
                HTML += "<li class=""module"">"
                HTML += "<span class=""title"">" & _glsi.Term & "</span>"
                HTML += "<ul>"
                HTML += "<li class=""description"">" & _glsi.Definition & "</li>"
                HTML += "</ul>"
                HTML += "</li>"
                HTML += "</ul>"
            Case 2
                HTML = "<dl>"
                HTML += "<dt>" & _glsi.Term & "</dt>"
                HTML += "<dd>" & _glsi.Definition & "</dd>"
                HTML += "</dl>"
            Case 3
                HTML &= "<span class=""item faq question"">" & _glsi.Term & "</span>" & vbCrLf

                If Not IsNothing(_glsi.Group) Then
                    HTML &= "<span class=""item glossary group"">" & vbCrLf
                    HTML &= _glsi.Group.Name
                    HTML &= "</span>"
                End If

                HTML &= "<span class=""item faq answer"">" & _glsi.Definition & "</span>"
        End Select

        Return HTML

    End Function
#End Region

End Class