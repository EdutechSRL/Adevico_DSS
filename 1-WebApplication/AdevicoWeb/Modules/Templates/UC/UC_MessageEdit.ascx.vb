Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.TemplateMessages.Presentation
Imports lm.Comol.Core.TemplateMessages.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports System.Runtime
Imports lm.Comol.Core.DomainModel.Languages

Public Class UC_MessageEdit
    Inherits BaseControl
    Implements lm.Comol.Core.BaseModules.TemplateMessages.Presentation.IViewEditMessage

#Region "Context"
    Private _Presenter As EditMessagePresenter
    Private ReadOnly Property CurrentPresenter() As EditMessagePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditMessagePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property AllowDelete As Boolean Implements IViewEditMessage.AllowDelete
        Get
            Return ViewStateOrDefault("AllowDelete", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowDelete") = value
        End Set
    End Property
    Public Property AllowPreview As Boolean Implements IViewEditMessage.AllowPreview
        Get
            Return ViewStateOrDefault("AllowPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowPreview") = value
        End Set
    End Property
    Private Property AllowTemplateSelection As Boolean Implements IViewEditMessage.AllowTemplateSelection
        Get
            Return ViewStateOrDefault("AllowTemplateSelection", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowTemplateSelection") = value
        End Set
    End Property
    Private Property MantainRecipients As Boolean Implements IViewEditMessage.MantainRecipients
        Get
            Return ViewStateOrDefault("MantainRecipients", False)
        End Get
        Set(value As Boolean)
            ViewState("MantainRecipients") = value
        End Set
    End Property
    Private Property CurrentTranslations As List(Of dtoTemplateTranslation) Implements IViewEditMessage.CurrentTranslations
        Get
            Return ViewStateOrDefault("CurrentTranslations", New List(Of dtoTemplateTranslation))
        End Get
        Set(value As List(Of dtoTemplateTranslation))
            ViewState("CurrentTranslations") = value
        End Set
    End Property
    Private Property ContentModules As List(Of String) Implements IViewEditMessage.ContentModules
        Get
            Return ViewStateOrDefault("ContentModules", New List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("ContentModules") = value
        End Set
    End Property
    Public Property CurrentMode As lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode Implements IViewEditMessage.CurrentMode
        Get
            Return ViewStateOrDefault("CurrentMode", lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.Edit)
        End Get
        Set(value As lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode)
            ViewState("CurrentMode") = value
            Select Case value
                Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.Edit
                    Me.MLVcontrol.SetActiveView(VIWeditor)
                Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.SelectUsers
                    Me.MLVcontrol.SetActiveView(VIWusersSelection)
                Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.MessageSent
                    Me.MLVcontrol.SetActiveView(VIWmessageSent)
                Case lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.None
                    Me.MLVcontrol.SetActiveView(VIWsessionTimeout)
            End Select
        End Set
    End Property
    Private Property SelectionMode As UserSelection Implements IViewEditMessage.SelectionMode
        Get
            Return ViewStateOrDefault("SelectionMode", UserSelection.FromInputText)
        End Get
        Set(value As UserSelection)
            ViewState("SelectionMode") = value
            BTNselectRecipients.Visible = (value <> UserSelection.None)
        End Set
    End Property

    Private Property IdSelectedAction As Long Implements IViewEditMessage.IdSelectedAction
        Get
            Return ViewStateOrDefault("IdSelectedAction", 0)
        End Get
        Set(value As Long)
            ViewState("IdSelectedAction") = value
        End Set
    End Property
    Private Property IdSelectedTemplate As Long Implements IViewEditMessage.IdSelectedTemplate
        Get
            Return ViewStateOrDefault("IdSelectedTemplate", 0)
        End Get
        Set(value As Long)
            ViewState("IdSelectedTemplate") = value
        End Set
    End Property
    Private Property IdSelectedVersion As Long Implements IViewEditMessage.IdSelectedVersion
        Get
            Return ViewStateOrDefault("IdSelectedVersion", 0)
        End Get
        Set(value As Long)
            ViewState("IdSelectedVersion") = value
        End Set
    End Property
    Private Property ContainerIdCommunity As Integer Implements IViewEditMessage.ContainerIdCommunity
        Get
            Return ViewStateOrDefault("ContainerIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("ContainerIdCommunity") = value
        End Set
    End Property
    Private ReadOnly Property CurrentSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings Implements IViewEditMessage.CurrentSettings
        Get
            Return CTRLmailSettings.Settings
        End Get
    End Property
    Private ReadOnly Property PortalName As String Implements IViewEditMessage.PortalName
        Get
            Return Resource.getValue("PortalName")
        End Get
    End Property
    Private Property CurrentModuleObject As ModuleObject Implements IViewEditMessage.CurrentObject
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
            If String.IsNullOrEmpty(value.FQN) Then
                Select Case value.ServiceCode
                    Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                        Exit Property
                    Case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode
                        value.FQN = GetType(lm.Comol.Modules.Standard.WebConferencing.Domain.WbRoom).FullName
                    Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
                        value.FQN = GetType(lm.Comol.Modules.Standard.ProjectManagement.Domain.Project).FullName
                    Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                        value.FQN = GetType(lm.Comol.Core.BaseModules.Tickets.Domain.Ticket).FullName
                    Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                        value.FQN = GetType(lm.Comol.Modules.EduPath.Domain.Path).FullName
                    Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                        value.FQN = GetType(lm.Comol.Modules.CallForPapers.Domain.CallForPaper).FullName
                    Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                        value.FQN = GetType(lm.Comol.Modules.CallForPapers.Domain.RequestForMembership).FullName
                End Select
            End If
            ViewState("CurrentModuleObject") = value
        End Set
    End Property
    Private Property ModuleCode As String Implements IViewEditMessage.ModuleCode
        Get
            Return ViewStateOrDefault("ModuleCode", "")
        End Get
        Set(value As String)
            ViewState("ModuleCode") = value
        End Set
    End Property
    Private Property ObjectTranslatedName As String Implements IViewEditMessage.ObjectTranslatedName
        Get
            Return ViewStateOrDefault("ObjectTranslatedName", "")
        End Get
        Set(value As String)
            ViewState("ObjectTranslatedName") = value
        End Set
    End Property

#End Region

#Region "Inherits"
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
    Private Property PlaceHolders As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder)
        Get
            Return ViewStateOrDefault("PlaceHolders", New List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder))
        End Get
        Set(value As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder))
            ViewState("PlaceHolders") = value
        End Set
    End Property
    Public Event DisplayHeaderForControl(ByVal moduleCode)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setButton(BTNdeleteCurrentTranslation, True)
            .setButton(BTNtemplateMessagePreview, True)
            .setButton(BTNcloseMailMessageWindow, True)
            .setButton(BTNselectRecipients, True)
            '.setLinkButton(LNBsaveAsTemplate, False, True)
            '.setLinkButton(LNBsaveAsPersonalTemplate, False, True)
            .setLabel(LBinuseTemplate_t)
            If Not Page.IsPostBack Then
                .setLiteral(LTmessageEdit_t)
            End If

            DVpreview.Attributes.Add("title", Resource.getValue("MailMessagePreview.Title"))
            .setButton(BTNbackToMessage, True)
            .setButton(BTNsendMessage, True)
            .setLiteral(LTrecipientSelection_t)
            .setLiteral(LTmessageSentInfos_t)
            .setButton(BTNbackToRecipients, True)
            .setButton(BTNnewMessage, True)
            .setLiteral(LTrecipientManualSelection_t)
        End With
    End Sub
#End Region

#Region "implements"
    'availableSaveAs As List(Of OwnerType),
    Public Sub InitializeControl(mode As UserSelection, allowSelectTemplate As Boolean, containerIdCommunity As Integer, Optional currentCode As String = "", Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing, Optional objectName As String = "", Optional idTemplate As Long = 0, Optional idVersion As Long = 0, Optional idAction As Long = 0) Implements IViewEditMessage.InitializeControl
        ObjectTranslatedName = objectName
        Me.SelectionMode = mode
        Me.DVtemplateSelector.Visible = allowSelectTemplate
        'If allowSelectTemplate AndAlso idAction > 0 Then
        '    CTRLtemplateSelector.InitializeControl(idAction, containerIdCommunity, idTemplate, idVersion, 0, currentCode, obj)
        'End If
        'Me.LNBsaveAsObjectTemplate.Visible = availableSaveAs.Contains(OwnerType.Object)
        'Me.LNBsaveAsPersonalTemplate.Visible = availableSaveAs.Contains(OwnerType.Person)
        'Me.LNBsaveAsTemplate.Visible = availableSaveAs.Contains(OwnerType.Module)
        'If availableSaveAs.Count = 1 Then
        '    DVsaveButtons.Attributes("class") = DVsaveButtons.Attributes("class").Replace("enabled", "")
        'End If
        'Resource.setLinkButton(LNBsaveAsObjectTemplate, False, True)
        'If LNBsaveAsObjectTemplate.Text.Contains("{0}") Then
        '    LNBsaveAsObjectTemplate.Text = String.Format(LNBsaveAsObjectTemplate.Text, objectName)
        '    LNBsaveAsObjectTemplate.ToolTip = String.Format(LNBsaveAsObjectTemplate.ToolTip, objectName)
        'End If
        Me.CurrentPresenter.InitView(allowSelectTemplate, containerIdCommunity, currentCode, obj, idTemplate, idVersion, idAction)
    End Sub
    Private Function RemoveCurrentTranslation() As lm.Comol.Core.DomainModel.Languages.LanguageItem Implements IViewEditMessage.RemoveCurrentTranslation
        Me.DVpreview.Visible = False
        Dim translations As List(Of dtoTemplateTranslation) = CurrentTranslations
        translations = translations.Where(Function(t) t.IdLanguage <> CTRLlanguageSelector.SelectedItem.Id).ToList()
        CurrentTranslations = translations
        Return CTRLlanguageSelector.RemoveCurrent()

    End Function
    Private Sub DisplayMessagePreview(allowSendMail As Boolean, languageCode As String, tContent As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), cSettings As List(Of lm.Comol.Core.TemplateMessages.Domain.ChannelSettings), idCommunity As Integer, Optional obj As lm.Comol.Core.DomainModel.ModuleObject = Nothing) Implements IViewEditMessage.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.AllowSendMail = allowSendMail
        Me.CTRLmailpreview.InitializeControlForPreview(languageCode, tContent, modules, cSettings, idCommunity, obj)
    End Sub
    Private Sub DisplayMessagePreview(allowSendMail As Boolean, languageCode As String, tContent As lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation, modules As List(Of String), mSettings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, idCommunity As Integer, Optional obj As ModuleObject = Nothing) Implements IViewEditMessage.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.AllowSendMail = allowSendMail
        Me.CTRLmailpreview.InitializeControlForPreview(languageCode, tContent, modules, mSettings, idCommunity, obj)
    End Sub
    Private Sub InitializeMailSettings(settings As lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings, editSender As Boolean, editSubject As Boolean, editSignature As Boolean) Implements IViewEditMessage.InitializeMailSettings
        Me.CTRLmailSettings.InitializeControl(settings, editSender, editSubject, editSignature)
        DVmailSettings.Visible = editSender OrElse editSubject OrElse editSignature
    End Sub
    Private Sub LoadEditor(translations As List(Of dtoTemplateTranslation), currentCode As String, displayShortText As Boolean, onlyShortText As Boolean, availableLanguages As List(Of BaseLanguageItem), inUse As List(Of LanguageItem), current As LanguageItem) Implements IViewEditMessage.LoadEditor
        Me.CurrentTranslations = translations
        Me.DVpreview.Visible = False
        If translations.Any() Then
            Me.DVpreview.Visible = False
            CTRLlanguageSelector.InEditing = True
            Me.CTRLlanguageSelector.InitializeControl(availableLanguages, inUse, current)

            CTRLtranslator.ShowShortText = displayShortText
            CTRLtranslator.ShowBody = Not onlyShortText
            CTRLtranslator.InEditing = True
            Me.BTNdeleteCurrentTranslation.Visible = AllowDelete AndAlso Not current.IsMultiLanguage
            Me.BTNtemplateMessagePreview.Visible = AllowPreview
            Me.CTRLtranslator.InitializeControl(translations.Where(Function(t) t.LanguageCode = currentCode).FirstOrDefault(), current, GetContentPlaceHolders(ContentModules), False)
        End If
    End Sub
    Private Sub UpdateTranslationSelector(availableLanguages As List(Of BaseLanguageItem), inUse As List(Of LanguageItem), current As LanguageItem, content As dtoTemplateTranslation) Implements IViewEditMessage.UpdateTranslationSelector
        Me.CTRLlanguageSelector.InitializeControl(availableLanguages, inUse, current)
        Me.CTRLtranslator.InitializeControl(content, current, GetContentPlaceHolders(ContentModules), False)
    End Sub
    Private Sub UpdateTranslation(current As LanguageItem, content As dtoTemplateTranslation) Implements IViewEditMessage.UpdateTranslation
        Me.CTRLtranslator.InitializeControl(content, current, GetContentPlaceHolders(ContentModules), False)
    End Sub
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
    Private Function GetContentPlaceHolders(modulesCodes As List(Of String)) As List(Of lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder) Implements IViewEditMessage.GetContentPlaceHolders
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


                    Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                        moduleName = tModules.Where(Function(m) m.Id = lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode).Select(Function(m) m.Translation).FirstOrDefault()
                        If Not String.IsNullOrEmpty(moduleName) Then
                            moduleName = String.Format(Resource.getValue("Module.PlaceHoldersType.ToolTip"), moduleName)
                        End If

                        tags.AddRange((From e In lm.Comol.Modules.EduPath.TemplateEduPathPlaceHolders.PlaceHolders _
                                        Where e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.None Select CreatePlaceholder(CInt(e.Key), e.Key.ToString, e.Value, "ModuleEduPath", moduleName)).ToList().OrderBy(Function(t) t.Name).ToList())
                        'AndAlso e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizList AndAlso e.Key <> lm.Comol.Modules.EduPath.EduPathPlaceHoldersType.QuizTable

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

    Private Sub DisplayTemplateSaved(savedAs As lm.Comol.Core.TemplateMessages.Domain.OwnerType) Implements IViewEditMessage.DisplayTemplateSaved
        Me.CTRLmessages.Visible = True
        If savedAs = OwnerType.Object Then
            Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue("TemplateSaved.True." & savedAs.ToString), ObjectTranslatedName), Helpers.MessageType.success)
        Else
            Me.CTRLmessages.InitializeControl(Resource.getValue("TemplateSaved.True." & savedAs.ToString), Helpers.MessageType.success)
        End If
    End Sub
    Private Sub DisplayTemplateUnSaved(savedAs As lm.Comol.Core.TemplateMessages.Domain.OwnerType) Implements IViewEditMessage.DisplayTemplateUnSaved
        Me.CTRLmessages.Visible = True
        If savedAs = OwnerType.Object Then
            Me.CTRLmessages.InitializeControl(String.Format(Resource.getValue("TemplateSaved.False." & savedAs.ToString), ObjectTranslatedName), Helpers.MessageType.success)
        Else
            Me.CTRLmessages.InitializeControl(Resource.getValue("TemplateSaved.False." & savedAs.ToString), Helpers.MessageType.error)
        End If
    End Sub

#Region "Editor"
    Private Sub DisplayEmptyMessage() Implements IViewEditMessage.DisplayEmptyMessage
        Me.DVpreview.Visible = False
        'Me.BTNsendMessage.Visible = False
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayEmptyMessage"), Helpers.MessageType.alert)
    End Sub
#End Region

#Region "Recipients"
    Private Sub DisplayNoRecipients() Implements IViewEditMessage.DisplayNoRecipients
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CTRLrecipientsMessages.Visible = True
        Me.CTRLrecipientsMessages.InitializeControl(Resource.getValue("DisplayNoRecipients"), Helpers.MessageType.alert)
    End Sub

    Private Sub DisplayMessageSentTo(sentTo As Integer, skipped As Integer) Implements IViewEditMessage.DisplayMessageSentTo
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CTRLrecipientsMessages.Visible = False
        Me.CTRLmessageSent.Visible = True
        Me.CurrentMode = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.MessageSent
        Dim m As String = "DisplayMessageSentTo.{0}.{1}"
        m = String.Format(m, IIf(sentTo < 2, sentTo, "n"), IIf(skipped < 2, skipped, "n"))
        If sentTo > 1 AndAlso skipped > 1 Then
            Me.CTRLmessageSent.InitializeControl(String.Format(Resource.getValue(m), sentTo, skipped), Helpers.MessageType.alert)
        ElseIf sentTo > 1 Then
            Me.CTRLmessageSent.InitializeControl(String.Format(Resource.getValue(m), sentTo), IIf(skipped > 0, Helpers.MessageType.alert, Helpers.MessageType.success))
        ElseIf skipped > 1 Then
            Me.CTRLmessageSent.InitializeControl(String.Format(Resource.getValue(m), skipped), Helpers.MessageType.alert)
        Else
            Me.CTRLmessageSent.InitializeControl(Resource.getValue(m), IIf(skipped > 0, Helpers.MessageType.alert, Helpers.MessageType.success))
        End If
        BTNbackToRecipients.Visible = (sentTo < 1)
    End Sub

    Private Sub DisplayUnableToSendMessage() Implements IViewEditMessage.DisplayUnableToSendMessage
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CTRLrecipientsMessages.Visible = True
        Me.CTRLrecipientsMessages.InitializeControl(Resource.getValue("DisplayUnableToSendMessage"), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayEmptyTranslations(languages As List(Of String)) Implements IViewEditMessage.DisplayEmptyTranslations
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CTRLrecipientsMessages.Visible = True
        If languages.Count = 1 Then
            Me.CTRLrecipientsMessages.InitializeControl(Resource.getValue("DisplayEmptyMessage.1"), Helpers.MessageType.alert)
        Else
            Dim message As String = Resource.getValue("DisplayEmptyMessage.n")
            If (Not String.IsNullOrEmpty(message)) Then
                message = String.Format(message, String.Join(", ", languages.ToArray))
            End If
            Me.CTRLrecipientsMessages.InitializeControl(message, Helpers.MessageType.alert)
        End If
    End Sub
    Private Sub LoadAvailableAddresses(items As List(Of BaseLanguageItem)) Implements IViewEditMessage.LoadAvailableAddresses
        DVmailRecipients.Visible = True
        DVmanualSelection.Visible = True
        RPTrecipients.DataSource = items
        RPTrecipients.DataBind()
    End Sub
    Private Sub LoadAvailableAddresses(items As List(Of BaseLanguageItem), recipients As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoTextualRecipient)) Implements IViewEditMessage.LoadAvailableAddresses
        LoadAvailableAddresses(items)
        For Each row As RepeaterItem In RPTrecipients.Items
            Dim oTextBox As TextBox = row.FindControl("TXBaddresses")
            If Not String.IsNullOrEmpty(oTextBox.Text) Then
                Dim oLiteral As Literal = row.FindControl("LTidLanguage")
                Dim oLiteralCode As Literal = row.FindControl("LTcodeLanguage")
                Dim item As New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoTextualRecipient

                If (recipients.Where(Function(r) r.IdLanguage = CInt(oLiteral.Text) AndAlso r.LanguageCode = oLiteralCode.Text).Any()) Then
                    oTextBox.Text = recipients.Where(Function(r) r.IdLanguage = CInt(oLiteral.Text) AndAlso r.LanguageCode = oLiteralCode.Text).FirstOrDefault().Addresses
                End If
            End If
        Next
    End Sub
    Private Function GetTextualRecipients() As List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoTextualRecipient) Implements IViewEditMessage.GetTextualRecipients
        Dim items As New List(Of lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoTextualRecipient)

        For Each row As RepeaterItem In RPTrecipients.Items
            Dim oTextBox As TextBox = row.FindControl("TXBaddresses")
            If Not String.IsNullOrEmpty(oTextBox.Text) Then
                Dim oLiteral As Literal = row.FindControl("LTidLanguage")
                Dim item As New lm.Comol.Core.BaseModules.TemplateMessages.Domain.dtoTextualRecipient
                item.IdLanguage = CInt(oLiteral.Text)

                oLiteral = row.FindControl("LTcodeLanguage")
                item.LanguageCode = oLiteral.Text
                item.Addresses = oTextBox.Text
                items.Add(item)
            End If
        Next
        Return items
    End Function
    Private Function SelectedRecipients() As List(Of lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient) Implements IViewEditMessage.SelectedRecipients
        Select Case ModuleCode
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode, lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                If CTRLcallUsersSelection.WasGridInitialized Then
                    Return CTRLcallUsersSelection.GetSelectedRecipients()
                End If
        End Select
        Return New List(Of lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient)
    End Function

    Private Sub SendMessage(recipients As List(Of lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient), translations As List(Of dtoTemplateTranslation), currentCode As String) Implements IViewEditMessage.SendMessage
        Dim messages As New List(Of lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage)
        Dim common As Boolean = False

        Dim callTranslations As New Dictionary(Of String, Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String))
        If currentCode = lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode OrElse currentCode = lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode Then
            For Each t As dtoTemplateTranslation In translations
                Dim r As New ResourceManager
                r.UserLanguages = t.LanguageCode
                r.ResourcesName = "pg_Templates"
                r.Folder_Level1 = "Modules"
                r.Folder_Level2 = "Templates"
                r.setCulture()

                Dim sTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
                    sTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
                Next

                callTranslations.Add(t.LanguageCode, sTranslations)
            Next
        End If
       
        Select Case currentCode
            Case lm.Comol.Core.BaseModules.ProfileManagement.ModuleProfileManagement.UniqueID
                Exit Sub

            Case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode
 Exit Sub
            Case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode
                Exit Sub
            Case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode
                Exit Sub
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                Dim callService As New lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(PageUtility.CurrentContext)

                If lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.HasUserValues(translations.Select(Function(t) t.Translation.Subject).ToList(), translations.Select(Function(t) t.Translation.Body).ToList()) Then
                    messages = callService.GetMessagesToSend(CurrentModuleObject.ObjectLongID, translations, recipients, PageUtility.ApplicationUrlBase, callTranslations)
                Else
                    common = True
                    messages = callService.GetMessagesToSend(CurrentModuleObject.ObjectLongID, translations, PageUtility.ApplicationUrlBase, callTranslations)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                Dim requestService As New lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(PageUtility.CurrentContext)
                If lm.Comol.Modules.CallForPapers.Domain.TemplatePlaceHolders.HasUserValues(translations.Select(Function(t) t.Translation.Subject).ToList(), translations.Select(Function(t) t.Translation.Body).ToList()) Then
                    messages = requestService.GetMessagesToSend(CurrentModuleObject.ObjectLongID, translations, recipients, PageUtility.ApplicationUrlBase, callTranslations)
                Else
                    common = True
                    messages = requestService.GetMessagesToSend(CurrentModuleObject.ObjectLongID, translations, PageUtility.ApplicationUrlBase, callTranslations)
                End If
        End Select
        If Not common Then
            'For Each m As lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage In messages
            '    Dim cRecipients As List(Of lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient) = recipients.Where(Function(r) m.IdLanguage = r.IdLanguage AndAlso (r.IdLanguage > 0 OrElse (r.IdLanguage = 0 AndAlso m.CodeLanguage = r.CodeLanguage))).ToList()
            '    m.RemovedRecipients = cRecipients.Where(Function(r) Not m.Recipients.Where(Function(mr) mr.MailAddress = r.MailAddress OrElse (mr.IdPerson = r.IdPerson AndAlso r.IsInternal)).Any()).ToList()
            'Next
        Else
            For Each m As lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage In messages
                m.Recipients = recipients.Where(Function(r) m.IdLanguage = r.IdLanguage AndAlso (r.IdLanguage > 0 OrElse (r.IdLanguage = 0 AndAlso m.CodeLanguage = r.CodeLanguage))).ToList()
                If m.CodeLanguage = "multi" Then
                    Dim idLanguages = messages.Where(Function(i) i.CodeLanguage <> "multi").Select(Function(i) i.IdLanguage).Distinct.ToList()

                    m.Recipients.AddRange(recipients.Where(Function(r) Not idLanguages.contains(r.IdLanguage) AndAlso r.CodeLanguage <> "multi").ToList())
                End If
            Next
        End If
        CurrentPresenter.SendMessage(PageUtility.CurrentSmtpConfig, messages)
    End Sub
    Private Sub InitializeModuleRecipientsSelector(isPortal As Boolean, idCommunity As Integer, obj As ModuleObject, idTemplate As Long, idVersion As Long, isTemplateCompliant As Boolean, translations As List(Of dtoTemplateTranslation)) Implements IViewEditMessage.InitializeModuleRecipientsSelector
        Select Case ModuleCode
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode
                DVinternalRecipients.Visible = True
                CTRLcallUsersSelection.Visible = True
                Me.CTRLcallUsersSelection.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.CallForBids, isPortal, idCommunity, obj, idTemplate, idVersion, isTemplateCompliant, translations)
            Case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode
                DVinternalRecipients.Visible = True
                CTRLcallUsersSelection.Visible = True
                Me.CTRLcallUsersSelection.InitializeControl(lm.Comol.Modules.CallForPapers.Domain.CallForPaperType.RequestForMembership, isPortal, idCommunity, obj, idTemplate, idVersion, isTemplateCompliant, translations)

            Case Else
                DVinternalRecipients.Visible = False
        End Select
        '      RaiseEvent DisplayHeaderForControl(ModuleCode)
    End Sub
    Private Sub DisplayModuleSelector() Implements IViewEditMessage.DisplayModuleSelector
        RaiseEvent DisplayHeaderForControl(ModuleCode)
    End Sub
#End Region

#End Region


#Region "control - Editor Section"
    Private Sub CTRLlanguageSelector_LanguageAdded(l As BaseLanguageItem) Handles CTRLlanguageSelector.LanguageAdded
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadTranslation(New LanguageItem(l), CTRLtranslator.Content, True)


        Me.BTNtemplateMessagePreview.Visible = AllowPreview
        Me.BTNdeleteCurrentTranslation.Visible = AllowDelete AndAlso Not l.IsMultiLanguage
        BTNselectRecipients.Visible = (CurrentMode <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.None)
    End Sub
    Private Sub CTRLlanguageSelector_SelectedLanguage(l As LanguageItem) Handles CTRLlanguageSelector.SelectedLanguage
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.LoadTranslation(New LanguageItem(l), CTRLtranslator.Content, False)

        BTNselectRecipients.Visible = (CurrentMode <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.None)
        Me.BTNdeleteCurrentTranslation.Visible = AllowDelete AndAlso Not l.IsMultiLanguage
    End Sub
    Private Sub BTNtemplateMessagePreview_Click(sender As Object, e As System.EventArgs) Handles BTNtemplateMessagePreview.Click
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.PreviewMessage(CTRLtranslator.Content)
    End Sub
    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub BTNdeleteCurrentTranslation_Click(sender As Object, e As System.EventArgs) Handles BTNdeleteCurrentTranslation.Click
        Me.CTRLmessages.Visible = False
        Me.DVpreview.Visible = False
        Dim item As lm.Comol.Core.DomainModel.Languages.LanguageItem = RemoveCurrentTranslation()
        If Not IsNothing(item) Then
            Dim content As dtoTemplateTranslation = CurrentTranslations.Where(Function(t) t.IdLanguage = item.Id).FirstOrDefault()
            If Not IsNothing(content) Then
                Me.BTNtemplateMessagePreview.Visible = AllowPreview
                Me.BTNdeleteCurrentTranslation.Visible = AllowDelete AndAlso Not item.IsMultiLanguage
                Me.CTRLtranslator.InitializeControl(content, item, GetContentPlaceHolders(ContentModules), False)
                BTNselectRecipients.Visible = (CurrentMode <> lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.None)
            Else
                Me.BTNtemplateMessagePreview.Visible = False
                Me.BTNdeleteCurrentTranslation.Visible = False
                BTNselectRecipients.Visible = False
            End If
        End If
    End Sub
    Private Sub CTRLtemplateSelector_TemplateSelected(idTemplate As Long, idVersion As Long) Handles CTRLtemplateSelector.TemplateSelected
        Me.CurrentPresenter.InitView(AllowTemplateSelection, ContainerIdCommunity, ModuleCode, CurrentModuleObject, idTemplate, idVersion, IdSelectedAction)
    End Sub

    'Private Sub LNBsaveAsObjectTemplate_Click(sender As Object, e As System.EventArgs) Handles LNBsaveAsObjectTemplate.Click
    '    Me.CurrentPresenter.SaveAsTemplate(OwnerType.Object, Resource.getValue("TemplateSaveAsName.Object"), CurrentTranslations, ContentModules, CTRLmailSettings.Settings, CurrentModuleObject)
    'End Sub

    'Private Sub LNBsaveAsPersonalTemplate_Click(sender As Object, e As System.EventArgs) Handles LNBsaveAsPersonalTemplate.Click
    '    Me.CurrentPresenter.SaveAsTemplate(OwnerType.Person, Resource.getValue("TemplateSaveAsName.Person"), CurrentTranslations, ContentModules, CTRLmailSettings.Settings, CurrentModuleObject)
    'End Sub

    'Private Sub LNBsaveAsTemplate_Click(sender As Object, e As System.EventArgs) Handles LNBsaveAsTemplate.Click
    '    Me.CurrentPresenter.SaveAsTemplate(OwnerType.Module, Resource.getValue("TemplateSaveAsName"), CurrentTranslations, ContentModules, CTRLmailSettings.Settings, CurrentModuleObject)
    'End Sub
#End Region

#Region "control - Recipients Section"
    Private Sub BTNselectRecipients_Click(sender As Object, e As System.EventArgs) Handles BTNselectRecipients.Click
        Me.CurrentPresenter.SelectRecipients(CTRLlanguageSelector.SelectedItem, CTRLtranslator.Content)
    End Sub
    Private Sub BTNbackToMessage_Click(sender As Object, e As System.EventArgs) Handles BTNbackToMessage.Click
        Me.CurrentMode = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.Edit
        Me.BTNsendMessage.Visible = True
    End Sub
    Private Sub BTNsendMessage_Click(sender As Object, e As System.EventArgs) Handles BTNsendMessage.Click
        Me.CurrentPresenter.AnalyzeMessageToSend(GetTextualRecipients, SelectedRecipients)
    End Sub
    Private Sub RPTrecipients_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrecipients.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As BaseLanguageItem = DirectCast(e.Item.DataItem, BaseLanguageItem)
            Dim oLiteral As Literal = e.Item.FindControl("LTmailAddressLanguage_t")
            Resource.setLiteral(oLiteral)
            Dim oLabel As Label = e.Item.FindControl("LBlanguageCode")
            oLabel.Text = item.ShortCode
            oLabel.ToolTip = item.Name

            oLiteral = e.Item.FindControl("LTmailAddressInvalidErrorsInfo")
            Resource.setLiteral(oLiteral)

            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPhideErrors")
            Resource.setHyperLink(oHyperlink, False, True)

        End If
    End Sub
#End Region

#Region "control - Message Sent"
    Private Sub BTNbackToRecipients_Click(sender As Object, e As System.EventArgs) Handles BTNbackToRecipients.Click
        Me.CurrentMode = lm.Comol.Core.BaseModules.TemplateMessages.Domain.EditMessageMode.SelectUsers
    End Sub
    Private Sub BTNnewMessage_Click(sender As Object, e As System.EventArgs) Handles BTNnewMessage.Click
        MantainRecipients = False
        Me.InitializeControl(CurrentMode, AllowTemplateSelection, ContainerIdCommunity, ModuleCode, CurrentModuleObject, ObjectTranslatedName, IdSelectedTemplate, IdSelectedVersion, IdSelectedAction)
    End Sub
#End Region



End Class