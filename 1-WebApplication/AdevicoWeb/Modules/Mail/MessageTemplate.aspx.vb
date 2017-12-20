Imports lm.Comol.Core.BaseModules.MailSender
Imports lm.Comol.Core.BaseModules.MailSender.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.TemplateMessages.Domain

Public Class MailMessageTemplate
    Inherits PageBase
    Implements lm.Comol.Core.BaseModules.MailSender.Presentation.IViewMessageTemplate

#Region "Context"
    Private _Presenter As MessageTemplatePresenter
    Private ReadOnly Property CurrentPresenter() As MessageTemplatePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New MessageTemplatePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

#Region "Preload"
    Private ReadOnly Property PreloadModuleObject As ModuleObject Implements IViewMessageTemplate.PreloadModuleObject
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
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewMessageTemplate.PreloadIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunity")) AndAlso IsNumeric(Request.QueryString("idCommunity")) Then
                Return CLng(Request.QueryString("idCommunity"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadModuleCode As String Implements IViewMessageTemplate.PreloadModuleCode
        Get
            Return Request.QueryString("code")
        End Get
    End Property
    Private ReadOnly Property PreloadIdModule As Integer Implements IViewMessageTemplate.PreloadIdModule
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idModule")) AndAlso IsNumeric(Request.QueryString("idModule")) Then
                Return CInt(Request.QueryString("idModule"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdMessageTemplate As Long Implements IViewMessageTemplate.PreloadIdMessageTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idT")) AndAlso IsNumeric(Request.QueryString("idT")) Then
                Return CLng(Request.QueryString("idT"))
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Current"
    Private Property CurrentTranslations As List(Of lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation) Implements IViewMessageTemplate.CurrentTranslations
        Get
            Return ViewStateOrDefault("CurrentTranslations", New List(Of lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation))
        End Get
        Set(value As List(Of lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation))
            ViewState("CurrentTranslations") = value
        End Set
    End Property
    Private Property ContainerContext As lm.Comol.Core.Mail.Messages.dtoTemplateMessageContext Implements IViewMessageTemplate.ContainerContext
        Get
            Return ViewStateOrDefault("ContainerContext", New lm.Comol.Core.Mail.Messages.dtoTemplateMessageContext())
        End Get
        Set(value As lm.Comol.Core.Mail.Messages.dtoTemplateMessageContext)
            ViewState("ContainerContext") = value
        End Set
    End Property
    Private Property ContentModules As List(Of String) Implements IViewMessageTemplate.ContentModules
        Get
            Return ViewStateOrDefault("ContentModules", New List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("ContentModules") = value
        End Set
    End Property
    Private ReadOnly Property TagTranslation As String Implements IViewMessageTemplate.TagTranslation
        Get
            Return LTplaceHolder.Text
        End Get
    End Property
#End Region

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private ReadOnly Property RemovedUserName As String
        Get
            Return Resource.getValue("RemovedUserName")
        End Get
    End Property
    Private ReadOnly Property UnknownUserName As String
        Get
            Return Resource.getValue("UnknownUserName")
        End Get
    End Property
    Private Property TranslatedModules As List(Of PlainService)
        Get
            Return ViewStateOrDefault("TranslatedModules", ManagerService.ListSystemTranslated(PageUtility.LinguaID))
        End Get
        Set(value As List(Of PlainService))
            ViewState("TranslatedModules") = value
        End Set
    End Property
    Private _Translations As List(Of TranslatedItem(Of String))
    Private ReadOnly Property TranslateNotificationModules As List(Of TranslatedItem(Of String))
        Get
            If _Translations Is Nothing Then
                Dim codes As List(Of String) = ContentModules
                _Translations = TranslatedModules.Where(Function(m) codes.Contains(m.Code)).Select(Function(m) New TranslatedItem(Of String) With {.Id = m.Code, .Translation = m.Name}).ToList()
                For Each code As String In codes.Where(Function(c) Not _Translations.Where(Function(t) t.Id = c).Any()).ToList()
                    _Translations.Add(New TranslatedItem(Of String) With {.Id = code, .Translation = Resource.getValue("Module." & code)})
                Next
            End If
            Return _Translations
        End Get
    End Property
    Private Property PlaceHolders As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder)
        Get
            Return ViewStateOrDefault("PlaceHolders", New List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder))
        End Get
        Set(value As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder))
            ViewState("PlaceHolders") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Messages", "Modules", "Mail")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBmessageSubject_t)
            .setLabel(LBnoTemplateFound)
            Master.ServiceTitle = .getValue("serviceTitle.ModuleTemplateMessage")
            Master.ServiceTitleToolTip = .getValue("serviceTitle.ModuleTemplateMessage")
            Master.ServiceNopermission = .getValue("nopermission")
        End With
    End Sub
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

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplaySessionTimeout() Implements IViewModuleMessages.DisplaySessionTimeout
        Dim cContext As lm.Comol.Core.Mail.Messages.dtoTemplateMessageContext = ContainerContext
        Dim idCommunity As Integer = cContext.IdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Core.Mail.Messages.RootObject.ViewMessageTemplate(cContext.IdMessageTemplate, cContext.ModuleCode, cContext.IdCommunity, cContext.IdModule, cContext.ModuleObject)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer, moduleCode As String) Implements IViewModuleMessages.DisplayNoPermission
        Dim idAction As Integer = CInt(lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission)
        Select Case moduleCode
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                idAction = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                idAction = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.ActionType.NoPermission
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                idAction = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.ActionType.NoPermission
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                idAction = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.ActionType.NoPermission
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                idAction = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.ActionType.NoPermission
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                idAction = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType.NoPermission
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                idAction = COL_Questionario.ModuleQuestionnaire.ActionType.NoPermission
        End Select
        Me.PageUtility.AddActionToModule(idCommunity, idModule, idAction, , InteractionType.UserWithUser)
        Me.BindNoPermessi()
    End Sub

    Private Function HasModulePermissions(moduleCode As String, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewModuleMessages.HasModulePermissions
        Dim result As Boolean = False
        Select Case moduleCode
            Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing(permissions)
                End If
                result = wModule.ManageRoom
            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                Dim wModule As lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement
                If idCommunity = 0 Then
                    wModule = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement(permissions)
                End If
                result = wModule.Administration
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Dim wModule As lm.Comol.Core.BaseModules.Tickets.ModuleTicket
                If idCommunity = 0 Then
                    wModule = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.CreatePortalmodule(profileType)
                Else
                    wModule = New lm.Comol.Core.BaseModules.Tickets.ModuleTicket(permissions)
                End If
                result = wModule.Administration
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim cModule As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper
                If idCommunity = 0 Then
                    cModule = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.CreatePortalmodule(profileType)
                Else
                    cModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper(permissions)
                End If
                result = cModule.Administration OrElse cModule.ManageCallForPapers
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim rModule As lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership
                If idCommunity = 0 Then
                    rModule = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.CreatePortalmodule(profileType)
                Else
                    rModule = New lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership(permissions)
                End If
                result = rModule.Administration OrElse rModule.ManageBaseForPapers
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                Dim epModule As lm.Comol.Modules.EduPath.Domain.ModuleEduPath
                If idCommunity = 0 Then
                    epModule = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.CreatePortalmodule(profileType)
                Else
                    epModule = New lm.Comol.Modules.EduPath.Domain.ModuleEduPath(permissions)
                End If
                result = epModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
            Case COL_Questionario.ModuleQuestionnaire.UniqueID
                Dim qModule As COL_Questionario.ModuleQuestionnaire
                If idCommunity = 0 Then
                    qModule = COL_Questionario.ModuleQuestionnaire.CreatePortalmodule(profileType)
                Else
                    qModule = New COL_Questionario.ModuleQuestionnaire(permissions)
                End If
                result = qModule.Administration
                If Not IsNothing(obj) AndAlso Not result Then

                End If
        End Select
        Return result
    End Function
    Private Function GetContentPlaceHolders(modulesCodes As List(Of String)) As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder) Implements IViewMessageTemplate.GetContentPlaceHolders
        Dim tags As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder) = PlaceHolders
        If Not tags.Any() Then
            '<item name="Module.PlaceHoldersType.Tooltip">Servizio "{0}"</item>

            Dim tModules As List(Of TranslatedItem(Of String)) = TranslateNotificationModules

            tags.AddRange((From e In GetCommonPlaceHolders(modulesCodes)
                                    Select CreatePlaceholder(CInt(e), e.ToString, lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.GetPlaceHolder(e), "Common", "", True)).ToList().OrderBy(Function(t) t.Name).ToList())

            For Each Code As String In modulesCodes
                Dim moduleName As String = ""
                Select Case Code
                    Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Core.BaseModules.ProfileManagement.TemplatePlaceHolders.PlaceHolders _
                                        Where e.Key <> lm.Comol.Core.BaseModules.ProfileManagement.PlaceHoldersType.None _
                                        Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleProfileManagement", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                    Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.Standard.WebConferencing.Domain.TemplatePlaceHolders.PlaceHolders _
                                      Where e.Key <> lm.Comol.Modules.Standard.WebConferencing.Domain.PlaceHoldersType.None _
                                      Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleWebConferencing", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                    Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.Standard.ProjectManagement.Domain.TemplatePlaceHolders.PlaceHolders() _
                                      Where e.Key <> lm.Comol.Modules.Standard.ProjectManagement.Domain.PlaceHoldersType.None _
                                      Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleProjectManagement", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())


                    Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Core.BaseModules.Tickets.Domain.TemplatePlaceHolders.PlaceHolders _
                                      Where e.Key <> lm.Comol.Core.BaseModules.Tickets.Domain.PlaceHoldersType.None _
                                      Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleTicket", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                    Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.EduPath.TemplateEduPathPlaceHolders.PlaceHolders _
                                        Where e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.None _
                                        Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleEduPath", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                        'AndAlso e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizList _
                        'AndAlso e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizTable _

                    Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.PlaceHolders(True) _
                                     Where e.Key <> lm.Comol.Modules.CallForPapers.Domain.PlaceHoldersType.None _
                                     Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleCallForPaper", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                    Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.PlaceHolders(True) _
                                       Where e.Key <> lm.Comol.Modules.CallForPapers.Domain.PlaceHoldersType.None _
                                       Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleRequestForMembership", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())

                        'tags.AddRange((From e In [Enum].GetValues(GetType(lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.)).Cast(Of PlaceHoldersType).ToList() _
                        '               Where e <> PlaceHoldersType.None Select New TranslatedItem(Of String) With {.Id = e.ToString, .Translation = Me.Resource.getValue(CallType.ToString & ".PlaceHoldersType." & e.ToString)}).ToList().OrderBy(Function(t) t.Translation).ToList()))

                End Select
            Next
            PlaceHolders = tags
        End If

        Return tags
    End Function
    Private Function GetCommonPlaceHolders(modulesCodes As List(Of String)) As List(Of Helpers.CommonPlaceHoldersType)
        Dim placeHolders As New List(Of Helpers.CommonPlaceHoldersType)
        If IsNothing(modulesCodes) OrElse Not modulesCodes.Any() OrElse (Not modulesCodes.Contains(lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode) AndAlso Not modulesCodes.Contains(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode)) Then
            placeHolders = (lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.PlaceHolders.Where(Function(p) p.Key <> Helpers.CommonPlaceHoldersType.None).Select(Function(p) p.Key).ToList())
        End If
        Return placeHolders
    End Function
    Private Function CreatePlaceholder(id As Integer, key As String, tag As String, prefix As String, moduleName As String, Optional isCommon As Boolean = False) As lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder
        Dim item As New lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder()
        item.Id = id
        item.Name = Me.Resource.getValue(prefix & ".PlaceHoldersType." & key)
        item.Description = Me.Resource.getValue(prefix & ".PlaceHoldersType.Description." & key)
        item.ToolTip = Me.Resource.getValue(prefix & ".PlaceHoldersType.ToolTip." & key)
        item.ModuleName = moduleName
        item.IsCommon = isCommon
        item.Tag = tag

        Return item
    End Function
    Private Sub DisplayNoTemplateFound() Implements IViewMessageTemplate.DisplayNoTemplateFound
        MLVtemplate.SetActiveView(VIWunknownTemplate)

    End Sub
    Public Sub DisplayMessageInfo(createdBy As lm.Comol.Core.DomainModel.litePerson, createdOn As DateTime) Implements IViewMessageTemplate.DisplayMessageInfo
        Resource.setLiteral(LTmessageInfo)
        If Not String.IsNullOrEmpty(LTmessageInfo.Text) Then
            If IsNothing(createdBy) OrElse createdBy.TypeID = UserTypeStandard.Guest Then
                LTmessageInfo.Text = String.Format(LTmessageInfo.Text, UnknownUserName)
            Else
                LTmessageInfo.Text = String.Format(LTmessageInfo.Text, createdBy.SurnameAndName)
            End If
            Resource.setLiteral(LTmessageInfoSentOn)
            LTmessageInfoSentOn.Text = String.Format(LTmessageInfoSentOn.Text, createdOn.ToString("D", Resource.CultureInfo))
            Resource.setLiteral(LTmessageInfoSentAt)
            LTmessageInfoSentAt.Text = String.Format(LTmessageInfoSentAt.Text, createdOn.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern))
        End If
    End Sub
    Private Sub InitializeControls(tTemplates As List(Of dtoTemplateTranslation), inUse As List(Of lm.Comol.Core.DomainModel.Languages.LanguageItem), current As lm.Comol.Core.DomainModel.Languages.LanguageItem) Implements IViewMessageTemplate.InitializeControls
        Me.CurrentTranslations = tTemplates
        If Not IsNothing(current) Then
            LBmessageSubjectLanguage.Text = current.ShortCode
        End If
        Me.CTRLlanguageSelector.InitializeControl(New List(Of lm.Comol.Core.DomainModel.Languages.BaseLanguageItem), inUse, current)
    End Sub

    Private Sub InitializeMailSettings(settings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, editSender As Boolean, editSubject As Boolean, editSignature As Boolean) Implements IViewMessageTemplate.InitializeMailSettings
        DVmailSettings.Visible = True
        Me.CTRLmailSettings.InitializeControl(settings, editSender, editSubject, editSignature, False)
    End Sub

    Private Sub LoadMessage(content As dtoTemplateTranslation) Implements IViewMessageTemplate.LoadMessage
        If IsNothing(content) Then
            Me.LBmessage.Text = ""
            Me.LBmessageSubject.Text = ""
        Else
            Me.LBmessage.Text = content.Translation.Body
            Me.LBmessageSubject.Text = content.Translation.Subject
        End If
    End Sub
#End Region

#Region "Control"
    Private Sub UserMessages_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Private Sub CTRLlanguageSelector_SelectedLanguage(l As lm.Comol.Core.DomainModel.Languages.LanguageItem) Handles CTRLlanguageSelector.SelectedLanguage
        LBmessageSubjectLanguage.Text = l.ShortCode
        LoadMessage(CurrentTranslations.Where(Function(t) t.IdLanguage = l.Id AndAlso t.LanguageCode = l.Code).FirstOrDefault())
    End Sub
#End Region

    
End Class