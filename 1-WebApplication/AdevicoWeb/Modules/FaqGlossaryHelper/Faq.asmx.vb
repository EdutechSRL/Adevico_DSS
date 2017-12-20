Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation

Imports lm.Comol.Modules.Standard.Faq

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<ScriptService()> _
Public Class Faq
    Inherits System.Web.Services.WebService


#Region "NOTE E REVISIONI"
    ' Sono da rivedere alcune cose:
    ' Posizione del service all'interno di Comol, nello specifico:
    '   - Se va bene un service per modulo o si possono centralizzare in un unico service.
    '   - SE creare opportune classi nella business



#End Region


#Region "Context"

    Private _currentContext As lm.Comol.Core.DomainModel.iApplicationContext

    Private _service As lm.Comol.Modules.Standard.Faq.FAQService

    Private ReadOnly Property Service As lm.Comol.Modules.Standard.Faq.FAQService
        Get
            If IsNothing(_service) Then
                _service = New lm.Comol.Modules.Standard.Faq.FAQService(CurrentContext, CurrentContext.DataContext.GetCurrentSession())
            End If
            Return _service
        End Get
    End Property

    Private ReadOnly Property CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_currentContext) Then
                _currentContext = New ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _currentContext
        End Get
    End Property

    Private _module As lm.Comol.Modules.Base.DomainModel.ModuleFaq
    ' RIVEDERE CON LOGICHE PIU' COMOL - LIKE (Questo è scopiazzato da MVC...)
    Private ReadOnly Property FaqModule As lm.Comol.Modules.Base.DomainModel.ModuleFaq
        Get
            If IsNothing(_module) Then


                Dim oModule As lm.Comol.Modules.Base.DomainModel.ModuleFaq
                'ModuleFaq oModule = null;

                Dim UserTypeId As Int32 = -1
                'Int32 UserTypeId = -1;

                If (CurrentContext.UserContext.CurrentCommunityID = 0) Then
                    oModule = lm.Comol.Modules.Base.DomainModel.ModuleFaq.CreatePortalmodule(UserTypeId)
                Else
                    oModule = (From sb In COL_BusinessLogic_v2.Comol.Manager.ManagerPersona.GetPermessiServizio( _
                                   CurrentContext.UserContext.CurrentUser.Id, COL_BusinessLogic_v2.UCServices.Services_Faq.Codex) _
                                   Where (sb.CommunityID = CurrentContext.UserContext.CurrentCommunityID) _
                                   Select New lm.Comol.Modules.Base.DomainModel.ModuleFaq(New COL_BusinessLogic_v2.UCServices.Services_Faq(sb.PermissionString)) _
                                    ).FirstOrDefault()
                    If IsNothing(oModule) Then
                        oModule = New lm.Comol.Modules.Base.DomainModel.ModuleFaq()
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
    ''' Get a rendered FAQ in an HTML String
    ''' </summary>
    ''' <param name="ItemId">FAQ Id</param>
    ''' <param name="Htmltype">
    ''' Render Type:
    ''' 1  Ul/li
    ''' 2  Dl/Dt
    ''' 3  span (con categoria)
    ''' Default value = 1
    ''' </param>
    ''' <returns>Html as String</returns>
    ''' <remarks></remarks>
    <WebMethod(True), ScriptMethod()> _
    Public Function GetFaqString(ByVal ItemId As Int64, ByVal Htmltype As Integer) As String

        Dim HTML As String = ""

        If Not IsNothing(CurrentContext.UserContext) _
            AndAlso ItemId > 0 _
            AndAlso FaqModule.ViewFaq Then
            Dim _faq As lm.Comol.Modules.Standard.Faq.DTO_Faq = Service.GetFaq(ItemId)

            If Not IsNothing(_faq) AndAlso Not String.IsNullOrEmpty(_faq.Question) AndAlso Not String.IsNullOrEmpty(_faq.Answer) Then
                If (Htmltype < 1 OrElse Htmltype > 4) Then
                    Htmltype = 1
                End If

                HTML = Me.RenderFaq(_faq, Htmltype)
            End If
        End If

        Return HTML
    End Function

    ''' <summary>
    ''' Get a rendered FAQ in an HTML String
    ''' </summary>
    ''' <param name="ItemId">FAQ Id</param>
    ''' <param name="Htmltype">
    ''' Render Type:
    ''' 1  Ul/li
    ''' 2  Dl/Dt
    ''' 3  span (con categoria)
    ''' Default value = 1
    ''' </param>
    ''' <returns>Html as String</returns>
    ''' <remarks></remarks>
    <WebMethod(True), ScriptMethod()> _
    Public Function GetFaqCategoryString(ByVal ItemId As Int64, ByVal Htmltype As Integer) As String

        Dim HTML As String = ""

        If Htmltype = 4 Then
            HTML = "<dl>"
        End If

        If Not IsNothing(CurrentContext.UserContext) _
            AndAlso ItemId > 0 _
            AndAlso FaqModule.ViewFaq Then
            Dim _faqs As IList(Of lm.Comol.Modules.Standard.Faq.DTO_Faq) = Service.GetFaqList(ItemId)

            For Each _faq As DTO_Faq In _faqs
                If Not IsNothing(_faq) AndAlso Not String.IsNullOrEmpty(_faq.Question) AndAlso Not String.IsNullOrEmpty(_faq.Answer) Then
                    If (Htmltype < 1 OrElse Htmltype > 4) Then
                        Htmltype = 1
                    End If

                    HTML += Me.RenderFaq(_faq, Htmltype)
                End If
            Next


        End If

        If Htmltype = 4 Then
            HTML += "</dl>"
        End If

        Return HTML
    End Function

    <WebMethod(True), ScriptMethod()> _
    Public Function GetFaq(ByVal ItemId As Int64, ByVal ComID As Integer) As lm.Comol.Modules.Standard.Faq.DTO_Faq

        If Not IsNothing(CurrentContext.UserContext) _
             AndAlso ItemId > 0 _
             AndAlso FaqModule.ViewFaq Then

            Dim _faq As lm.Comol.Modules.Standard.Faq.DTO_Faq = Service.GetFaq(ItemId)

            Return _faq
        End If

        Return New lm.Comol.Modules.Standard.Faq.DTO_Faq
    End Function



#End Region

#Region "Internal"
    Private Function RenderFaq(ByVal _faq As lm.Comol.Modules.Standard.Faq.DTO_Faq, ByVal Htmltype As Integer) As String
        Dim HTML As String = ""

        Select Case Htmltype
            Case 1
                HTML = "<ul>"
                HTML += "<li class=""module"">"
                HTML += "<span class=""title"">" & _faq.Question & "</span>"
                HTML += "<ul>"
                HTML += "<li class=""description"">" & _faq.Answer & "</li>"
                HTML += "</ul>"
                HTML += "</li>"
                HTML += "</ul>"
            Case 2
                HTML = "<dl>"
                HTML += "<dt>" & _faq.Question & "</dt>"
                HTML += "<dd>" & _faq.Answer & "</dd>"
                HTML += "</dl>"
            Case 3
                HTML &= "<span class=""item faq question"">" & _faq.Question & "</span>" & vbCrLf

                If Not IsNothing(_faq.Categories) AndAlso _faq.Categories.Count() > 0 Then
                    HTML &= "<span class=""item faq categories"">" & vbCrLf
                    HTML &= "	<ul>" & vbCrLf

                    For Each cat As DTO_Category In _faq.Categories
                        HTML &= "		<li>" & cat.Name & "</li>"
                    Next

                    HTML &= "	</ul>" & vbCrLf
                    HTML &= "</span>"
                End If

                HTML &= "<span class=""item faq answer"">" & _faq.Answer & "</span>"
            Case 4
                HTML = "<dt>" & _faq.Question & "</dt>"
                HTML += "<dd>" & _faq.Answer & "</dd>"                
        End Select

        Return HTML

    End Function
#End Region

End Class