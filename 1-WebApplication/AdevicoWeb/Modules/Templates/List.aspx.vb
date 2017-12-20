Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.TemplateMessages

Public Class TemplateList
    Inherits PageBase
    Implements IViewListTemplates

#Region "Context"
    Private _Presenter As ListPresenter
    Private ReadOnly Property CurrentPresenter() As ListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewListTemplates.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadOwnership As dtoBaseTemplateOwner Implements IViewBase.PreloadOwnership
        Get
            Dim item As New dtoBaseTemplateOwner()
            item.Type = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OwnerType).GetByString(Request.QueryString("ownType"), OwnerType.None)
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                item.IdCommunity = CInt(Request.QueryString("idCommunity"))
            Else
                item.IdCommunity = -1
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idPerson")) AndAlso IsNumeric(Request.QueryString("idPerson")) Then
                item.IdPerson = CInt(Request.QueryString("idPerson"))
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                item.IdModule = CInt(Request.QueryString("idModule"))
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("moduleCode")) Then
                item.ModuleCode = Request.QueryString("moduleCode")
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("idModuleP")) AndAlso IsNumeric(Request.QueryString("idModuleP")) Then
                item.ModulePermission = CLng(Request.QueryString("idModuleP"))
            End If

            Select Case item.Type
                Case OwnerType.Object
                    If Not String.IsNullOrEmpty(Request.QueryString("idObj")) AndAlso IsNumeric(Request.QueryString("idObj")) Then
                        item.IdObject = CLng(Request.QueryString("idObj"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjType")) AndAlso IsNumeric(Request.QueryString("idObjType")) Then
                        item.IdObjectType = CInt(Request.QueryString("idObjType"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjModule")) AndAlso IsNumeric(Request.QueryString("idObjModule")) Then
                        item.IdObjectModule = CInt(Request.QueryString("idObjModule"))
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("idObjCommunity")) AndAlso IsNumeric(Request.QueryString("idObjCommunity")) Then
                        item.IdObjectCommunity = CInt(Request.QueryString("idObjCommunity"))
                    End If
            End Select
            Return item
        End Get
    End Property
    Private ReadOnly Property PreloadTemplateType As TemplateType Implements IViewBase.PreloadTemplateType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateType).GetByString(Request.QueryString("type"), TemplateType.None)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewListTemplates.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunityCnt")) AndAlso IsNumeric(Request.QueryString("idCommunityCnt")) Then
                Return CLng(Request.QueryString("idCommunityCnt"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromCookies As Boolean Implements IViewListTemplates.PreloadFromCookies
        Get
            Return (Request.QueryString("reload") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadModuleCode As String Implements IViewListTemplates.PreloadModuleCode
        Get
            Return Request.QueryString("mCodeCnt")
        End Get
    End Property
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewListTemplates.PreloadIdTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdTemplate")) AndAlso IsNumeric(Request.QueryString("IdTemplate")) Then
                Return CLng(Request.QueryString("IdTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadModulePermissions As Long Implements IViewListTemplates.PreloadModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Public Property AllowAdd As Boolean Implements IViewListTemplates.AllowAdd
        Get
            Return CTRLtemplatesList.AllowAdd
        End Get
        Set(value As Boolean)
            CTRLtemplatesList.AllowAdd = value
        End Set
    End Property

    Private Property IdManagerCommunity As Integer Implements IViewListTemplates.IdManagerCommunity
        Get
            Return ViewStateOrDefault("IdManagerCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdManagerCommunity") = value
        End Set
    End Property
    Private Property CurrentType As TemplateType Implements IViewListTemplates.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", TemplateType.None)
        End Get
        Set(value As TemplateType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property Ownership As dtoBaseTemplateOwner Implements IViewBase.Ownership
        Get
            Return ViewStateOrDefault("Ownership", New dtoBaseTemplateOwner() With {.Type = OwnerType.Person, .IdCommunity = -1})
        End Get
        Set(value As dtoBaseTemplateOwner)
            ViewState("Ownership") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Master.ServiceTitle = .getValue("serviceTitle.TemplateList")
            Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(HYPaddTemplate, False, True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, "0"), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = IdManagerCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
#End Region

    Private Sub InitializeList(mContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext, filters As dtoBaseFilters, availableTypes As List(Of TemplateType), availableDisplay As List(Of TemplateDisplay)) Implements IViewListTemplates.InitializeList
        Me.CTRLtemplatesList.InitializeControl(mContext, filters, availableTypes, availableDisplay, Guid.Empty, PreloadIdTemplate)
    End Sub
    Private Function GetFromCookies() As dtoBaseFilters Implements IViewListTemplates.GetFromCookies
        Dim dto As New dtoBaseFilters
        Try
            Dim cookieName As String = "Templates_" & IdManagerCommunity & "_" & PreloadModuleCode & "_" & CurrentType
            With dto
                Dim value As String = Me.Request.Cookies(cookieName)("Ascending")
                If String.IsNullOrEmpty(value) Then
                    .Ascending = False
                Else
                    .Ascending = CBool(value)
                End If
                .OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateOrder).GetByString(Me.Request.Cookies(cookieName)("OrderBy"), TemplateOrder.ByName)
                .SearchForName = Me.Request.Cookies(cookieName)("SearchForName")
                .ModuleCode = Me.Request.Cookies(cookieName)("ModuleCode")
                .PageIndex = Me.Request.Cookies(cookieName)("PageIndex")
                .PageSize = Me.Request.Cookies(cookieName)("PageSize")
                .Status = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateStatus).GetByString(Me.Request.Cookies(cookieName)("Status"), TemplateStatus.Active)
                .TemplateType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateType).GetByString(Me.Request.Cookies(cookieName)("TemplateType"), CurrentType)
                .TemplateDisplay = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateDisplay).GetByString(Me.Request.Cookies(cookieName)("TemplateDisplay"), TemplateDisplay.OnlyVisible)
                .TranslationsStatus = GetTranslationsStatus()
                .TranslationsType = GetTranslationsTypes()
            End With
        Catch ex As Exception
            dto = Nothing
        End Try

        Return dto
    End Function
    Private Sub SaveToCookies(filter As dtoBaseFilters) Implements IViewListTemplates.SaveToCookies
        Try
            Dim cookieName As String = "Templates_" & IdManagerCommunity & "_" & PreloadModuleCode & "_" & CurrentType
            Me.Response.Cookies(cookieName)("SearchForName") = filter.SearchForName
            Me.Response.Cookies(cookieName)("Ascending") = filter.Ascending
            Me.Response.Cookies(cookieName)("ModuleCode") = filter.ModuleCode
            Me.Response.Cookies(cookieName)("OrderBy") = filter.OrderBy.ToString
            Me.Response.Cookies(cookieName)("Status") = filter.Status.ToString
            Me.Response.Cookies(cookieName)("TemplateType") = filter.TemplateType.ToString
            Me.Response.Cookies(cookieName)("PageIndex") = filter.PageIndex
            Me.Response.Cookies(cookieName)("PageSize") = filter.PageSize
            Me.Response.Cookies(cookieName)("TemplateDisplay") = filter.TemplateDisplay.ToString

        Catch ex As Exception

        End Try
    End Sub
    Private Function GetTranslationsStatus() As Dictionary(Of TemplateStatus, String) Implements IViewListTemplates.GetTranslationsStatus
        Dim translations As New Dictionary(Of TemplateStatus, String)
        For Each name As String In [Enum].GetNames(GetType(TemplateStatus))
            translations.Add([Enum].Parse(GetType(TemplateStatus), name), Me.Resource.getValue("TemplateStatus.Translations." & name))
        Next
    End Function
    Private Function GetTranslationsTypes() As Dictionary(Of TemplateType, String) Implements IViewListTemplates.GetTranslationsTypes
        Dim translations As New Dictionary(Of TemplateType, String)
        For Each name As String In [Enum].GetNames(GetType(TemplateType))
            translations.Add([Enum].Parse(GetType(TemplateType), name), Me.Resource.getValue("TemplateType.Translations." & name))
        Next
    End Function
    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer) As ModuleGenericTemplateMessages Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewListTemplates.GetModulePermissions
        Dim p As ModuleGenericTemplateMessages

        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Dim wModule As lm.Comol.Core.BaseModules.Tickets.ModuleTicket
                If idCommunity = 0 Then
                    wModule = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Core.BaseModules.Tickets.ModuleTicket(permissions)
                End If
                p = wModule.ToTemplateModule()
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim cModule As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper
                If idCommunity = 0 Then
                    cModule = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.CreatePortalmodule(profileType)
                Else
                    cModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper(permissions)
                End If
                p = cModule.ToTemplateModule()
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim rModule As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership
                If idCommunity = 0 Then
                    rModule = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.CreatePortalmodule(profileType)
                Else
                    rModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership(permissions)
                End If
                p = rModule.ToTemplateModule()
            Case Else
                p = New ModuleGenericTemplateMessages(moduleCode)

        End Select

        Return p
    End Function
    Private Sub SetAddUrl(url As String) Implements IViewListTemplates.SetAddUrl
        HYPaddTemplate.Visible = Not String.IsNullOrEmpty(url)
        HYPaddTemplate.NavigateUrl = BaseUrl & url
    End Sub
#End Region

    Private Sub AddTemplate_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

    Private Sub CTRLtemplatesList_ApplyFiltersEvent(filters As lm.Comol.Core.TemplateMessages.Domain.dtoBaseFilters) Handles CTRLtemplatesList.ApplyFiltersEvent
        Me.SaveToCookies(filters)
    End Sub

    Private Sub CTRLtemplatesList_PageChangedEvent(filters As lm.Comol.Core.TemplateMessages.Domain.dtoBaseFilters, pageIndex As Integer, pageSize As Integer) Handles CTRLtemplatesList.PageChangedEvent
        Me.SaveToCookies(filters)
    End Sub

    Private Sub CTRLtemplatesList_SessionTimeout() Handles CTRLtemplatesList.SessionTimeout
        Me.CurrentPresenter.SessionTimeout()
    End Sub


End Class