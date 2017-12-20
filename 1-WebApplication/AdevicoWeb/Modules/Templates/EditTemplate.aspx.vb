Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.DomainModel.Languages

Public Class EditTemplate
    Inherits PageBase
    Implements IViewEditTranslations

#Region "Context"
    Private _Presenter As EditTranslationsPresenter
    Private ReadOnly Property CurrentPresenter() As EditTranslationsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditTranslationsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseEdit.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadBackUrl As String Implements IViewBaseEdit.PreloadBackUrl
        Get
            If PreloadCurrentSessionId = Guid.Empty Then
                Return Request.QueryString("BackUrl")
            Else
                Return Me.Session(lm.Comol.Core.TemplateMessages.ExternalModuleRootObject.SessionName(PreloadCurrentSessionId))
            End If

        End Get
    End Property
    Private ReadOnly Property PreloadPreview As Boolean Implements IViewBaseEdit.PreloadPreview
        Get
            Return (Request.QueryString("preview") = "true")
        End Get
    End Property
    Private ReadOnly Property PreloadIdTemplate As Long Implements IViewBaseEdit.PreloadIdTemplate
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdTemplate")) AndAlso IsNumeric(Request.QueryString("IdTemplate")) Then
                Return CLng(Request.QueryString("IdTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdVersion As Long Implements IViewBaseEdit.PreloadIdVersion
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("IdVersion")) AndAlso IsNumeric(Request.QueryString("IdVersion")) Then
                Return CLng(Request.QueryString("IdVersion"))
            Else
                Return 0
            End If
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
    Private ReadOnly Property PreloadFromIdCommunity As Integer Implements IViewBaseEdit.PreloadFromIdCommunity
        Get
            If Not String.IsNullOrEmpty(Request.QueryString("idCommunityCnt")) AndAlso IsNumeric(Request.QueryString("idCommunityCnt")) Then
                Return CLng(Request.QueryString("idCommunityCnt"))
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadFromModuleCode As String Implements IViewBaseEdit.PreloadFromModuleCode
        Get
            Return Request.QueryString("mCodeCnt")
        End Get
    End Property
    Private ReadOnly Property PreloadFromModulePermissions As Long Implements IViewBaseEdit.PreloadFromModulePermissions
        Get
            If String.IsNullOrEmpty(Request.QueryString("mPrmCnt")) OrElse Not IsNumeric(Request.QueryString("mPrmCnt")) Then
                Return 0
            Else
                Return CLng(Request.QueryString("mPrmCnt"))
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadCurrentSessionId As Guid Implements IViewBaseEdit.PreloadCurrentSessionId
        Get
            If String.IsNullOrEmpty(Request.QueryString("sId")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Request.QueryString("sId"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Private Property InputReadOnly As Boolean Implements IViewBaseEdit.InputReadOnly
        Get
            Return ViewStateOrDefault("InputReadOnly", False)
        End Get
        Set(value As Boolean)
            ViewState("InputReadOnly") = value
            If value Then
                Me.BTNsaveTranslationBottom.Visible = Not value
                Me.BTNdeleteTranslationBottom.Visible = Not value
            End If
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseEdit.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
            Me.BTNsaveTranslationBottom.Visible = value
        End Set
    End Property
    Private Property AllowDelete As Boolean Implements IViewEditTranslations.AllowDelete
        Get
            Return ViewStateOrDefault("AllowDelete", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowDelete") = value
            Me.BTNdeleteTranslationBottom.Visible = value
        End Set
    End Property
    Private Property AllowPreview As Boolean Implements IViewEditTranslations.AllowPreview
        Get
            Return ViewStateOrDefault("AllowPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowPreview") = value
            Me.BTNtemplateMessagePreview.Visible = value
        End Set
    End Property
    Private Property IdTemplateCommunity As Integer Implements IViewBaseEdit.IdTemplateCommunity
        Get
            Return ViewStateOrDefault("IdTemplateCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateCommunity") = value
        End Set
    End Property
    Private Property IdTemplateModule As Integer Implements IViewBaseEdit.IdTemplateModule
        Get
            Return ViewStateOrDefault("IdTemplateModule", 0)
        End Get
        Set(value As Integer)
            ViewState("IdTemplateModule") = value
        End Set
    End Property
    Private Property IdTemplate As Long Implements IViewBaseEdit.IdTemplate
        Get
            Return ViewStateOrDefault("IdTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("IdTemplate") = value
        End Set
    End Property
    Private Property IdVersion As Long Implements IViewBaseEdit.IdVersion
        Get
            Return ViewStateOrDefault("IdVersion", 0)
        End Get
        Set(value As Long)
            ViewState("IdVersion") = value
        End Set
    End Property
    Private Property CurrentType As TemplateType Implements IViewBaseEdit.CurrentType
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
    Public Property ContentModules As List(Of String) Implements IViewEditTranslations.ContentModules
        Get
            Return ViewStateOrDefault("ContentModules", New List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("ContentModules") = value
        End Set
    End Property
    Private ReadOnly Property IdTranslation As Long Implements IViewEditTranslations.IdTranslation
        Get
            Return CTRLtranslator.Content.Id
        End Get
    End Property
    Private ReadOnly Property CurrentTranslation As LanguageItem Implements IViewEditTranslations.CurrentTranslation
        Get
            Return CTRLlanguageSelector.SelectedItem
        End Get
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
    Private ReadOnly Property InEditing As Boolean
        Get
            Return Not InputReadOnly AndAlso (AllowSave)
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
            Master.ServiceTitle = .getValue("serviceTitle.EditTranslations")
            Master.ServiceNopermission = .getValue("nopermission")
            .setLabel(LBnoTemplate)
            .setHyperLink(HYPbackTop, False, True)
            .setHyperLink(HYPbackBottom, False, True)
            .setButton(BTNdeleteTranslationBottom, True)
            .setButton(BTNsaveTranslationBottom, True)
            .setButton(BTNtemplateMessagePreview, True)
            .setButton(BTNcloseMailMessageWindow, True)
            DVpreview.Attributes.Add("title", Resource.getValue("MailMessagePreview.Title"))
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType) Implements IViewBase.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ObjectType.Template, IdTemplate), InteractionType.UserWithLearningObject)
    End Sub

#Region "Display"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewBase.DisplayNoPermission
        Me.DVpreview.Visible = False
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Core.TemplateMessages.ModuleTemplateMessages.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadFromIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub UnableToReadUrlSettings() Implements IViewBaseEdit.UnableToReadUrlSettings
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.DVpreview.Visible = False
        Me.LBnoTemplate.Text = Resource.getValue("UnableToReadUrlSettings")
    End Sub
    Private Sub DisplayUnknownTemplate() Implements IViewBaseEdit.DisplayUnknownTemplate
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayUnknownTemplate"), Helpers.MessageType.alert)
        Me.MLVsettings.SetActiveView(VIWtranslations)
        CTRLlanguageSelector.Visible = False
        CTRLtranslator.Visible = False
    End Sub
    Private Sub DisplayTemplateTranslationDeleted() Implements IViewEditTranslations.DisplayTemplateTranslationDeleted
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateTranslationDeleted"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWtranslations)
    End Sub
    Private Sub DisplayTemplateTranslationErrorDeleting() Implements IViewEditTranslations.DisplayTemplateTranslationErrorDeleting
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateTranslationErrorDeleting"), Helpers.MessageType.alert)
        Me.MLVsettings.SetActiveView(VIWtranslations)
    End Sub
    Private Sub DisplayTemplateTranslationErrorSaving() Implements IViewEditTranslations.DisplayTemplateTranslationErrorSaving
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateTranslationErrorSaving"), Helpers.MessageType.error)
        Me.MLVsettings.SetActiveView(VIWtranslations)
    End Sub
    Private Sub DisplayTemplateTranslationSaved(idTranslation As Long) Implements IViewEditTranslations.DisplayTemplateTranslationSaved
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateTranslationSaved"), Helpers.MessageType.success)
        Me.MLVsettings.SetActiveView(VIWtranslations)
        Me.CTRLtranslator.Content.Id = idTranslation
    End Sub

    Private Sub HideUserMessage() Implements IViewBaseEdit.HideUserMessage
        Me.CTRLmessages.Visible = False
        Me.DVpreview.Visible = False
    End Sub
#End Region

    Private Sub SetBackUrl(url As String) Implements IViewBaseEdit.SetBackUrl
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub LoadWizardSteps(idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep))) Implements IViewBaseEdit.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(IdTemplate, IdVersion, IdTemplateCommunity, PreloadFromIdCommunity, PreloadFromModuleCode, PreloadFromModulePermissions, CurrentType, Ownership, PreloadCurrentSessionId, GetEncodedBackUrl, steps, PreloadPreview)
    End Sub

    Private Function HasPermissionForObject(source As lm.Comol.Core.DomainModel.ModuleObject) As Boolean Implements IViewBaseEdit.HasPermissionForObject
        Return False
    End Function
    Private Function GetModulePermissions(moduleCode As String, idModule As Integer, permissions As Long, idCommunity As Integer, profileType As Integer, obj As lm.Comol.Core.DomainModel.ModuleObject) As lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Implements IViewBaseEdit.GetModulePermissions
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
                p = New lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(moduleCode)

        End Select

        'End If

        Return p
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
    Private Function GetContentPlaceHolders(modulesCodes As List(Of String)) As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder) Implements IViewEditTranslations.GetContentPlaceHolders
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
    Private Sub GoToUrl(url As String) Implements IViewBaseEdit.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub

    Private Sub LoadTranslation(content As dtoTemplateTranslation, displayShortText As Boolean, onlyShortText As Boolean, availableLanguages As List(Of BaseLanguageItem), inUse As List(Of LanguageItem), current As LanguageItem) Implements IViewEditTranslations.LoadTranslation
        Dim isInEditing As Boolean = InEditing
        Me.DVpreview.Visible = False
        CTRLlanguageSelector.InEditing = isInEditing
        Me.CTRLlanguageSelector.InitializeControl(availableLanguages, inUse, current)
        CTRLtranslator.ShowShortText = displayShortText
        CTRLtranslator.ShowBody = Not onlyShortText
        CTRLtranslator.InEditing = isInEditing
        CTRLtranslator.BodyCssClass = IIf(isInEditing, "", "mailbody")
        Me.CTRLtranslator.InitializeControl(content, current, GetContentPlaceHolders(ContentModules), False)
    End Sub
    Private Sub LoadTranslation(content As dtoTemplateTranslation, displayShortText As Boolean, onlyShortText As Boolean, current As LanguageItem) Implements IViewEditTranslations.LoadTranslation
        Dim isInEditing As Boolean = InEditing
        Me.DVpreview.Visible = False
        CTRLlanguageSelector.InEditing = InEditing
        CTRLtranslator.ShowShortText = displayShortText
        CTRLtranslator.ShowBody = Not onlyShortText
        CTRLtranslator.InEditing = isInEditing
        CTRLtranslator.BodyCssClass = IIf(isInEditing, "", "mailbody")
        Me.CTRLtranslator.InitializeControl(content, current, GetContentPlaceHolders(ContentModules), False)
    End Sub
    Private Sub UpdateTranslationSelector(availableLanguages As List(Of BaseLanguageItem), inUse As List(Of LanguageItem), current As LanguageItem) Implements IViewEditTranslations.UpdateTranslationSelector
        Me.CTRLlanguageSelector.InitializeControl(availableLanguages, inUse, current)
    End Sub
    Private Function RemoveCurrentTranslation() As LanguageItem Implements IViewEditTranslations.RemoveCurrentTranslation
        Me.DVpreview.Visible = False
        Return CTRLlanguageSelector.RemoveCurrent
    End Function
    Private Sub DisplayMessagePreview(allowSendMail As Boolean, languageCode As String, content As ItemObjectTranslation, modules As List(Of String), cSettings As List(Of ChannelSettings), idCommunity As Integer, Optional obj As ModuleObject = Nothing) Implements IViewEditTranslations.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.AllowSendMail = allowSendMail
        Me.CTRLmailpreview.InitializeControlForPreview(languageCode, content, modules, cSettings, idCommunity, obj)
    End Sub
    Private Function GetEncodedBackUrl() As String Implements IViewBaseEdit.GetEncodedBackUrl
        If String.IsNullOrEmpty(PreloadBackUrl) Then
            Return ""
        Else
            Return Server.UrlEncode(PreloadBackUrl)
        End If
    End Function
#End Region

    Private Sub AddTemplate_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

    Private Sub CTRLlanguageSelector_LanguageAdded(l As BaseLanguageItem) Handles CTRLlanguageSelector.LanguageAdded
        Me.DVpreview.Visible = False
        Me.CurrentPresenter.LoadTranslation(New LanguageItem(l), True)
    End Sub
    Private Sub CTRLlanguageSelector_SelectedLanguage(l As LanguageItem) Handles CTRLlanguageSelector.SelectedLanguage
        Me.DVpreview.Visible = False
        Me.CurrentPresenter.LoadTranslation(l)
    End Sub
    Private Sub BTNsaveTranslationBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveTranslationBottom.Click
        Me.DVpreview.Visible = False
        Me.CurrentPresenter.SaveTranslation(CTRLtranslator.Content)
    End Sub

    Private Sub BTNdeleteTranslationBottom_Click(sender As Object, e As System.EventArgs) Handles BTNdeleteTranslationBottom.Click
        Me.DVpreview.Visible = False
        Me.CurrentPresenter.VirtualDeleteTranslation(IdTranslation, CTRLlanguageSelector.SelectedItem.Id, CTRLlanguageSelector.SelectedItem.Code)
    End Sub


    Private Sub BTNtemplateMessagePreview_Click(sender As Object, e As System.EventArgs) Handles BTNtemplateMessagePreview.Click
        Me.CurrentPresenter.PreviewMessage(CTRLtranslator.Content)
    End Sub

    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
    End Sub

End Class