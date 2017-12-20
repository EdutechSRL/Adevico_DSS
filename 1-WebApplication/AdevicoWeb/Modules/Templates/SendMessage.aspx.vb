Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Languages

Public Class SendMessage
    Inherits PageBase
    Implements IViewSendMessage

#Region "Context"
    Private _Presenter As SendMessagePresenter
    Private ReadOnly Property CurrentPresenter() As SendMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SendMessagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Preload"
    Private ReadOnly Property PreloadModuleObject As ModuleObject Implements IViewBaseModuleMessage.PreloadModuleObject
        Get
            Dim item As New ModuleObject()

            If Not String.IsNullOrEmpty(Request.QueryString("oId")) AndAlso IsNumeric(Request.QueryString("oId")) Then
                item.ObjectLongID = CLng(Request.QueryString("oId"))
            End If
            If item.ObjectLongID > 0 Then
                If Not String.IsNullOrEmpty(Request.QueryString("oType")) AndAlso IsNumeric(Request.QueryString("oType")) Then
                    item.ObjectTypeID = CInt(Request.QueryString("oType"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oIdModule")) AndAlso IsNumeric(Request.QueryString("oIdModule")) Then
                    item.ServiceID = CInt(Request.QueryString("oIdModule"))
                End If
                If Not String.IsNullOrEmpty(Request.QueryString("oCommunity")) AndAlso IsNumeric(Request.QueryString("oCommunity")) Then
                    item.CommunityID = CInt(Request.QueryString("oCommunity"))
                End If
                item.ServiceCode = Request.QueryString("oMcode")
                Return item
            Else
                Return Nothing
            End If

        End Get
    End Property
    Private ReadOnly Property PreloadTemplateType As TemplateType Implements IViewBaseModuleMessage.PreloadTemplateType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateType).GetByString(Request.QueryString("type"), TemplateType.None)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseModuleMessage.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                Return CLng(Request.QueryString("idCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromCookies As Boolean Implements IViewSendMessage.PreloadFromCookies
        Get
            Return (Request.QueryString("reload") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadModuleCode As String Implements IViewBaseModuleMessage.PreloadModuleCode
        Get
            Return Request.QueryString("code")
        End Get
    End Property
    Private ReadOnly Property PreloadIdModule As Integer Implements IViewBaseModuleMessage.PreloadIdModule
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewBaseModuleMessage.PreloadIdTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdTemplate")) AndAlso IsNumeric(Request.QueryString("IdTemplate")) Then
                Return CLng(Request.QueryString("IdTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdVersion As Long Implements IViewBaseModuleMessage.PreloadIdVersion
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idVersion")) AndAlso IsNumeric(Request.QueryString("idVersion")) Then
                Return CLng(Request.QueryString("idVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadBackUrl As String Implements IViewBaseModuleMessage.PreloadBackUrl
        Get
            Return Request.QueryString("back")
        End Get
    End Property
    Private ReadOnly Property PreloadIdAction As Long Implements IViewBaseModuleMessage.PreloadIdAction
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("action")) AndAlso IsNumeric(Request.QueryString("action")) Then
                Return CLng(Request.QueryString("action"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadSelectedTab As DisplayTab Implements IViewBaseModuleMessage.PreloadSelectedTab
        Get
            Dim rTab As DisplayTab
            Try
                rTab = CInt(Request.QueryString("tab"))
            Catch ex As Exception
                rTab = DisplayTab.Sent
            End Try
            Return rTab
        End Get
    End Property
    Private ReadOnly Property PreloadSelectionMode As UserSelection Implements IViewBaseModuleMessage.PreloadSelectionMode
        Get
            If String.IsNullOrEmpty(Request.QueryString("sl")) OrElse Not IsNumeric(Request.QueryString("sl")) Then
                Return UserSelection.FromModule
            Else
                Try
                    Return CInt(Request.QueryString("sl"))
                Catch ex As Exception
                    Return UserSelection.FromModule
                End Try
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadTabs As DisplayTab Implements IViewBaseModuleMessage.PreloadTabs
        Get
            Dim rTab As DisplayTab
            Try
                rTab = CInt(Request.QueryString("tabs"))
            Catch ex As Exception
                rTab = DisplayTab.List Or DisplayTab.Send Or DisplayTab.Sent
            End Try
            Return rTab

            '  Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplayTab).GetByString(Request.QueryString("tabs"), DisplayTab.List Or DisplayTab.Send Or DisplayTab.Sent)
        End Get
    End Property
    Private ReadOnly Property PreloadAllowSelectTemplate As Boolean Implements IViewBaseModuleMessage.PreloadAllowSelectTemplate
        Get
            If String.IsNullOrEmpty(Request.QueryString("dtsl")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModulePermissions As Long Implements IViewBaseModuleMessage.PreloadFromModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadWithEmptyActions As Boolean Implements IViewSendMessage.PreloadWithEmptyActions
        Get
            Return (Request.QueryString("aEmpty") = "true")
        End Get
    End Property
#End Region
#Region "Current"
    Private Property CurrentModuleObject As ModuleObject Implements IViewBaseModuleMessage.CurrentModuleObject
        Get
            If Not IsNothing(ViewState("CurrentModuleObject")) Then
                Try
                    Return DirectCast(ViewState("CurrentModuleObject"), ModuleObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Get
        Set(value As ModuleObject)
            ViewState("CurrentModuleObject") = value
        End Set
    End Property
    Private Property CurrentTemplateType As TemplateType Implements IViewSendMessage.CurrentTemplateType
        Get
            Return ViewStateOrDefault("CurrentTemplateType", TemplateType.Module)
        End Get
        Set(value As TemplateType)
            ViewState("CurrentTemplateType") = value
        End Set
    End Property
    Private Property CurrentIdCommunity As Integer Implements IViewBaseModuleMessage.CurrentIdCommunity
        Get
            Return ViewStateOrDefault("CurrentIdCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdCommunity") = value
        End Set
    End Property
    Private Property CurrentModuleCode As String Implements IViewBaseModuleMessage.CurrentModuleCode
        Get
            Return ViewStateOrDefault("CurrentModuleCode", "")
        End Get
        Set(value As String)
            ViewState("CurrentModuleCode") = value
        End Set
    End Property
    Private Property CurrentIdModule As Integer Implements IViewBaseModuleMessage.CurrentIdModule
        Get
            Return ViewStateOrDefault("CurrentIdModule", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdModule") = value
        End Set
    End Property
    Private Property CurrentIdTemplate As Long Implements IViewSendMessage.CurrentIdTemplate
        Get
            Return ViewStateOrDefault("CurrentIdTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdTemplate") = value
        End Set
    End Property
    Private Property CurrentIdVersion As Long Implements IViewSendMessage.CurrentIdVersion
        Get
            Return ViewStateOrDefault("CurrentIdVersion", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdVersion") = value
        End Set
    End Property
    Private Property CurrentIdAction As Long Implements IViewSendMessage.CurrentIdAction
        Get
            Return ViewStateOrDefault("CurrentIdAction", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdAction") = value
        End Set
    End Property
    Private Property CurrentSessionId As Guid Implements IViewBaseModuleMessage.CurrentSessionId
        Get
            Return ViewStateOrDefault("CurrentSessionId", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("CurrentSessionId") = value
        End Set
    End Property
    Private Property SelectedTab As DisplayTab Implements IViewBaseModuleMessage.SelectedTab
        Get
            Return ViewStateOrDefault("SelectedTab", DisplayTab.List)
        End Get
        Set(value As DisplayTab)
            ViewState("SelectedTab") = value
        End Set
    End Property
    Private Property SelectionMode As UserSelection Implements IViewSendMessage.SelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", UserSelection.FromModule)
        End Get
        Set(value As UserSelection)
            ViewState("SelectionMode") = value
        End Set
    End Property
    Private Property AvailableTabs As DisplayTab Implements IViewBaseModuleMessage.AvailableTabs
        Get
            Return ViewStateOrDefault("AvailableTabs", DisplayTab.List Or DisplayTab.Send Or DisplayTab.Sent)
        End Get
        Set(value As DisplayTab)
            ViewState("AvailableTabs") = value
        End Set
    End Property
    Private Property AllowSelectTemplate As Boolean Implements IViewSendMessage.AllowSelectTemplate
        Get
            Return ViewStateOrDefault("AllowSelectTemplate", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSelectTemplate") = value
        End Set
    End Property
    Private Property AlsoWithEmptyActions As Boolean Implements IViewSendMessage.AlsoWithEmptyActions
        Get
            Return ViewStateOrDefault("AlsoWithEmptyActions", False)
        End Get
        Set(value As Boolean)
            ViewState("AlsoWithEmptyActions") = value
        End Set
    End Property
    Private Property IsInitializedList As Boolean Implements IViewSendMessage.IsInitializedList
        Get
            Return ViewStateOrDefault("IsInitializedList", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitializedList") = value
        End Set
    End Property
    Private Property IsInitializedSender As Boolean Implements IViewSendMessage.IsInitializedSender
        Get
            Return ViewStateOrDefault("IsInitializedSender", False)
        End Get
        Set(value As Boolean)
            ViewState("IsInitializedSender") = value
        End Set
    End Property
    Private Property ModulePermissions As Long Implements IViewSendMessage.ModulePermissions
        Get
            Return ViewStateOrDefault("ModulePermissions", 0)
        End Get
        Set(value As Long)
            ViewState("ModulePermissions") = value
        End Set
    End Property
#End Region


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
            Master.ServiceTitle = .getValue("serviceTitle.TemplateList." & PreloadModuleCode)
            Master.ServiceNopermission = .getValue("nopermission")
            .setHyperLink(HYPbackUrl, False, True)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    'Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
    '    PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, "0"), InteractionType.UserWithLearningObject)
    'End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBaseModuleMessage.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer, moduleCode As String) Implements IViewBaseModuleMessage.DisplayNoPermission
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        End Select

        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(idCommunity As Integer, url As String) Implements IViewBaseModuleMessage.DisplaySessionTimeout
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

    Private Sub InitializeList(mContext As lm.Comol.Core.TemplateMessages.Domain.dtoModuleContext, filters As dtoBaseFilters, availableTypes As List(Of TemplateType), availableDisplay As List(Of TemplateDisplay), allowAdd As Boolean, Optional addUrl As String = "", Optional addPersonalUrl As String = "", Optional addObjectUrl As String = "") Implements IViewSendMessage.InitializeList
        Me.CTRLtemplatesList.InitializeControl(mContext, filters, availableTypes, availableDisplay, CurrentSessionId, PreloadIdTemplate, allowAdd, addUrl, addPersonalUrl, addObjectUrl, Resource.getValue(CurrentModuleCode & ".DisplayTab." & DisplayTab.List.ToString))
    End Sub
    Private Function GetEncodedBackUrl(url As String) As String Implements IViewBaseModuleMessage.GetEncodedBackUrl
        If String.IsNullOrEmpty(url) Then
            Return ""
        Else
            Return Server.UrlEncode(url)
        End If
    End Function

    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewBaseModuleMessage.GetModulePermissions
        Dim p As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages
        'If Not IsNothing(obj) Then
        '    p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(obj.ServiceCode)

        'Else
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
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
                p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(moduleCode)

        End Select

        'End If

        Return p
    End Function

    Private Sub LoadTabs(tabs As List(Of DisplayTab), selected As DisplayTab) Implements IViewBaseModuleMessage.LoadTabs
        If IsNothing(tabs) OrElse Not tabs.Any() Then
            Me.DVtab.Visible = False
        Else
            Dim rTab As Telerik.Web.UI.RadTab = Nothing
            Me.DVtab.Visible = True
            For Each t As DisplayTab In tabs
                rTab = Me.TBSmessages.FindTabByValue(t.ToString)
                If Not IsNothing(rTab) Then
                    rTab.Visible = True
                    rTab.PostBack = (t <> DisplayTab.Sent)
                    rTab.Text = Resource.getValue("DisplayTab." & t.ToString())
                    If t = DisplayTab.Sent Then
                        rTab.NavigateUrl = BaseUrl & lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.MessagesSent(CurrentModuleCode, AvailableTabs, SelectionMode, AllowSelectTemplate, GetEncodedBackUrl(PreloadBackUrl), CurrentIdAction, AlsoWithEmptyActions, CurrentIdTemplate, CurrentIdVersion, CurrentIdCommunity, CurrentIdModule, CurrentModuleObject)

                    End If
                End If
            Next
            rTab = Me.TBSmessages.FindTabByValue(selected.ToString)
            If Not IsNothing(rTab) Then
                rTab.Selected = True
            End If
        End If
        Me.SelectedTab = selected
    End Sub
    Private Sub DisplayList() Implements IViewSendMessage.DisplayList
        MLVcontent.SetActiveView(VIWcontent)
        MLVtabcontent.SetActiveView(VIWlist)
    End Sub
    Private Function GetFromCookies() As dtoBaseFilters Implements IViewSendMessage.GetFromCookies
        Dim dto As New dtoBaseFilters
        Try
            Dim cookieName As String = "SendMessage_" & CurrentIdCommunity & "_" & PreloadModuleCode & "_" & CurrentTemplateType
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
                .TemplateType = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateType).GetByString(Me.Request.Cookies(cookieName)("TemplateType"), CurrentTemplateType)
                .TemplateDisplay = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TemplateDisplay).GetByString(Me.Request.Cookies(cookieName)("TemplateDisplay"), TemplateDisplay.OnlyVisible)
                .TranslationsStatus = GetTranslationsStatus()
                .TranslationsType = GetTranslationsTypes()
            End With
        Catch ex As Exception
            dto = Nothing
        End Try

        Return dto
    End Function
    Private Sub SaveToCookies(filter As dtoBaseFilters) Implements IViewSendMessage.SaveToCookies
        Try
            Dim cookieName As String = "SendMessage_" & CurrentIdCommunity & "_" & PreloadModuleCode & "_" & CurrentTemplateType
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
    Private Function GetTranslationsStatus() As Dictionary(Of TemplateStatus, String) Implements IViewSendMessage.GetTranslationsStatus
        Dim translations As New Dictionary(Of TemplateStatus, String)
        For Each name As String In [Enum].GetNames(GetType(TemplateStatus))
            translations.Add([Enum].Parse(GetType(TemplateStatus), name), Me.Resource.getValue("TemplateStatus.Translations." & name))
        Next
    End Function
    Private Function GetTranslationsTypes() As Dictionary(Of TemplateType, String) Implements IViewSendMessage.GetTranslationsTypes
        Dim translations As New Dictionary(Of TemplateType, String)
        For Each name As String In [Enum].GetNames(GetType(TemplateType))
            translations.Add([Enum].Parse(GetType(TemplateType), name), Me.Resource.getValue("TemplateType.Translations." & name))
        Next
    End Function
    Private Sub SetBackUrl(url As String, sessionId As Guid, currentUrl As String) Implements IViewBaseModuleMessage.SetBackUrl
        HYPbackUrl.Visible = Not String.IsNullOrEmpty(url)
        HYPbackUrl.NavigateUrl = BaseUrl & url
        Me.Session(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.SessionName(sessionId)) = currentUrl
    End Sub

    Private Sub DisplaySendMessage() Implements IViewSendMessage.DisplaySendMessage
        MLVcontent.SetActiveView(VIWcontent)
        MLVtabcontent.SetActiveView(VIWsend)
        ' Me.CTRLsend.CurrentMode = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.Edit
    End Sub
    'availableSaveAs As List(Of OwnerType),
    Private Sub InitializeSendMessage(allowSelectTemplate As Boolean, containerIdCommunity As Integer, Optional currentCode As String = "", Optional idTemplate As Long = 0, Optional idVersion As Long = 0, Optional idAction As Long = 0) Implements IViewSendMessage.InitializeSendMessage
        'availableSaveAs
        Me.CTRLsend.CurrentMode = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.Edit
        Me.CTRLsend.InitializeControl(SelectionMode, allowSelectTemplate, containerIdCommunity, currentCode, CurrentModuleObject, Resource.getValue("SaveAsObjectName." & currentCode), idTemplate, idVersion, idAction)
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

    Private Sub CTRLtemplatesList_SelectedTemplate(idTemplate As Long, idversion As Long) Handles CTRLtemplatesList.SelectedTemplate
        Dim rTab As Telerik.Web.UI.RadTab = TBSmessages.Tabs.FindTabByValue(DisplayTab.Send.ToString())
        If Not IsNothing(rTab) Then
            rTab.Selected = True
        End If

        Me.CurrentPresenter.LoadSendMessage(DisplayTab.Send, idTemplate, idversion)
    End Sub

    Private Sub CTRLtemplatesList_SessionTimeout() Handles CTRLtemplatesList.SessionTimeout
        Me.CurrentPresenter.SessionTimeout(CurrentIdCommunity, SelectedTab)
    End Sub

    Private Sub TBSmessages_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSmessages.TabClick
        Dim t As DisplayTab = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of DisplayTab).GetByString(e.Tab.Value, DisplayTab.None)
        If t <> SelectedTab Then
            Me.CurrentPresenter.LoadTab(t)
        End If
    End Sub
End Class