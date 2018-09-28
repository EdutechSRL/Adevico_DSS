Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.Comol.Core.ModuleLinks

Public Class UC_CertificationAction
    Inherits BaseControl
    Implements IViewModuleCertificationAction

#Region "Context"
    Private _BaseUrl As String
    Private _Presenter As ModuleCertificationActionPresenter

    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As ModuleCertificationActionPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ModuleCertificationActionPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected _serviceQS As COL_Questionario.Business.ServiceQuestionnaire
    Protected ReadOnly Property ServiceQS As COL_Questionario.Business.ServiceQuestionnaire
        Get
            If IsNothing(_serviceQS) Then
                _serviceQS = New COL_Questionario.Business.ServiceQuestionnaire(Me.PageUtility.CurrentContext)
            End If
            Return _serviceQS
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Base"
    Public Property ContainerCSS As String Implements IViewModuleCertificationAction.ContainerCSS
        Get
            Return ViewStateOrDefault("ContainerCSS", "")
        End Get
        Set(value As String)
            ViewState("ContainerCSS") = value
        End Set
    End Property
    Public Property Display As DisplayActionMode Implements IViewModuleCertificationAction.Display
        Get
            Return ViewStateOrDefault("Display", DisplayActionMode.defaultAction)
        End Get
        Set(value As DisplayActionMode)
            ViewState("Display") = value
            If value = DisplayActionMode.none Then
                Me.MLVcontrol.SetActiveView(VIWempty)
                Me.LBempty.Text = " "
            Else
                Me.MLVcontrol.SetActiveView(VIWdata)
                Me.RPTactions.Visible = ((value And DisplayActionMode.actions) > 0)
            End If
        End Set
    End Property
    Public Property IconSize As lm.Comol.Core.DomainModel.Helpers.IconSize Implements IGenericModuleDisplayAction.IconSize
        Get
            Return ViewStateOrDefault("IconSize", lm.Comol.Core.DomainModel.Helpers.IconSize.Medium)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Helpers.IconSize)
            ViewState("IconSize") = value
        End Set
    End Property
    'Private ReadOnly Property IconSizeToString As String
    '    Get
    '        Dim result As String = ViewStateOrDefault("IconSizeToString", "")
    '        If String.IsNullOrEmpty(result) Then
    '            Select Case IconSize
    '                Case Helpers.IconSize.Large
    '                    result = "_l"
    '                Case Helpers.IconSize.Medium
    '                    result = "_m"
    '                Case Helpers.IconSize.Small
    '                    result = "_s"
    '                Case Helpers.IconSize.Smaller
    '                    result = "_xs"
    '            End Select
    '            ViewState("IconSizeToString") = result
    '        End If
    '        Return result
    '    End Get
    'End Property
    Public Property ShortDescription As Boolean Implements IGenericModuleDisplayAction.ShortDescription
        Get
            Return ViewStateOrDefault("ShortDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("ShortDescription") = value
        End Set
    End Property
    Public Property EnableAnchor As Boolean Implements IGenericModuleDisplayAction.EnableAnchor
        Get
            Return ViewStateOrDefault("EnableAnchor", False)
        End Get
        Set(value As Boolean)
            ViewState("EnableAnchor") = value
        End Set
    End Property
#End Region
    'Public Property RefreshContainer As Boolean Implements IViewModuleCertificationAction.RefreshContainer
    '    Get
    '        Return ViewStateOrDefault("RefreshContainer", False)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("RefreshContainer") = value
    '    End Set
    'End Property
    Public Property ForUserId As Integer Implements IViewModuleCertificationAction.ForUserId
        Get
            Return ViewStateOrDefault("ForUserId", PageUtility.CurrentContext.UserContext.CurrentUserID)
        End Get
        Set(value As Integer)
            ViewState("ForUserId") = value
        End Set
    End Property
    Public Property InsideOtherModule As Boolean Implements IViewModuleCertificationAction.InsideOtherModule
        Get
            Return ViewStateOrDefault("InsideOtherModule", False)
        End Get
        Set(value As Boolean)
            ViewState("InsideOtherModule") = value
        End Set
    End Property
    Private Property ItemIdentifier As String Implements IViewModuleCertificationAction.ItemIdentifier
        Get
            Return ViewStateOrDefault("ItemIdentifier", "")
        End Get
        Set(value As String)
            ViewState("ItemIdentifier") = value
            If String.IsNullOrEmpty(value) Then
                LTidentifier.Visible = False
            Else
                LTidentifier.Text = "<a name=""" & value & """> </a>"
                LTidentifier.Visible = True
            End If
        End Set
    End Property

    Private ReadOnly Property PreLoadedContentView As ContentView Implements IViewModuleCertificationAction.PreLoadedContentView
        Get
            Return PageUtility.PreLoadedContentView
        End Get
    End Property
    'Protected ReadOnly Property DestinationUrl As String Implements IViewModuleCertificationAction.DestinationUrl
    '    Get
    '        Dim url As String = Request.Url.LocalPath
    '        If Me.BaseUrl <> "/" Then
    '            url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
    '        End If
    '        url = Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
    '        Return url
    '    End Get
    'End Property
    Protected ReadOnly Property FullDestinationUrl As String
        Get
            Dim url As String = Request.Url.LocalPath & Request.Url.Query
            If Not url.Contains("OidU") Then
                url &= "&OidU=" & IdUnit.ToString()
            End If
            If Not url.Contains("OidA") Then
                url &= "&OidA=" & IdActivity.ToString()
            End If
            'If Me.BaseUrl <> "/" Then
            '    url = Replace(Request.Url.LocalPath, Me.BaseUrl, "")
            'End If
            url &= IIf(EnableAnchor, "#" & ItemIdentifier, "") 'Server.UrlEncode(url & Request.Url.Query & IIf(EnableAnchor, "#" & ItemIdentifier, ""))
            Return url
        End Get
    End Property
    'Private ReadOnly Property PreviewCertificationUrl As String Implements IViewModuleCertificationAction.PreviewCertificationUrl
    '    Get

    '    End Get
    'End Property
    Private Property IdCommunityContainer As Integer Implements IViewModuleCertificationAction.IdCommunityContainer
        Get
            Return ViewStateOrDefault("IdCommunityContainer", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunityContainer") = value
        End Set
    End Property
    Private Property IdPath As Long Implements IViewModuleCertificationAction.IdPath
        Get
            Return ViewStateOrDefault("IdPath", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdPath") = value
        End Set
    End Property
    Private Property IdUnit As Long Implements IViewModuleCertificationAction.IdUnit
        Get
            Return ViewStateOrDefault("IdUnit", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdUnit") = value
        End Set
    End Property
    Private Property IdActivity As Long Implements IViewModuleCertificationAction.IdActivity
        Get
            Return ViewStateOrDefault("IdActivity", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdActivity") = value
        End Set
    End Property
    Private Property IdSubActivity As Long Implements IViewModuleCertificationAction.IdSubActivity
        Get
            Return ViewStateOrDefault("IdSubActivity", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubActivity") = value
        End Set
    End Property
    Private Property IdTemplate As Long Implements IViewModuleCertificationAction.IdTemplate
        Get
            Return ViewStateOrDefault("IdTemplate", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdTemplate") = value
        End Set
    End Property
    Private Property IdTemplateVersion As Long Implements IViewModuleCertificationAction.IdTemplateVersion
        Get
            Return ViewStateOrDefault("IdTemplateVersion", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdTemplateVersion") = value
        End Set
    End Property
    'Public Property SaveRunningAction As Boolean Implements IViewModuleCertificationAction.SaveRunningAction
    '    Get
    '        Return ViewStateOrDefault("SaveRunningAction", True)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("SaveRunningAction") = value
    '    End Set
    'End Property
    'Public Property SaveCertification As Boolean Implements IViewModuleCertificationAction.SaveCertification
    '    Get
    '        Return ViewStateOrDefault("SaveCertification", True)
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("SaveCertification") = value
    '    End Set
    'End Property
    Private Property AllowWithEmptyPlaceHolders As Boolean Implements IViewModuleCertificationAction.AllowWithEmptyPlaceHolders
        Get
            Return ViewStateOrDefault("AllowWithEmptyPlaceHolders", True)
        End Get
        Set(value As Boolean)
            ViewState("AllowWithEmptyPlaceHolders") = value
        End Set
    End Property
    Private Property AutoGenerated As Boolean Implements IViewModuleCertificationAction.AutoGenerated
        Get
            Return ViewStateOrDefault("AutoGenerated", True)
        End Get
        Set(value As Boolean)
            ViewState("AutoGenerated") = value
        End Set
    End Property

    'Public Property CookieName As String Implements IViewModuleCertificationAction.CookieName
    '    Get
    '        Return ViewStateOrDefault("CookieName", "")
    '    End Get
    '    Set(value As String)
    '        ViewState("CookieName") = value
    '    End Set
    'End Property
    Private ReadOnly Property CertificationFilePath As String Implements IViewModuleCertificationAction.CertificationFilePath
        Get
            Dim baseFilePath As String = ""
            If Me.SystemSettings.File.UserCertifications.DrivePath = "" Then
                baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.UserCertifications.VirtualPath)
            Else
                baseFilePath = Me.SystemSettings.File.UserCertifications.DrivePath
            End If
            Return baseFilePath
        End Get
    End Property
    Private Property CertificationName As String Implements IViewModuleCertificationAction.CertificationName
        Get
            Return ViewStateOrDefault("CertificationName", Resource.getValue("CertificationName"))
        End Get
        Set(value As String)
            ViewState("CertificationName") = value
        End Set
    End Property
    Public Property EvaluablePath As Boolean Implements IViewModuleCertificationAction.EvaluablePath
        Get
            Return ViewStateOrDefault("EvaluablePath", CurrentPresenter.IsEvaluablePath(IdPath))
        End Get
        Set(value As Boolean)
            ViewState("EvaluablePath") = value
        End Set
    End Property
    'Public Event RefreshContainerEvent(sender As Object, e As lm.Comol.Modules.EduPath.Presentation.RefreshContainerArgs) Implements lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction.RefreshContainerEvent
    'Public Event RefreshContainerEvent(sender As Object, e As RefreshContainerArgs) Implements IViewModuleCertificationAction.RefreshContainerEvent
    ' Public Event RefreshRequired(ByVal executed As Boolean) RefreshContainerEvent
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "UC PROPERTY / EVENTS"
    Public ReadOnly Property TemplateBaseUrl As String
        Get
            Return Me.Request.Url.AbsoluteUri.Replace( _
             Me.Request.Url.PathAndQuery, "") & Me.BaseUrl & MyBase.SystemSettings.DocTemplateSettings.BaseUrl
        End Get
    End Property
    'Public Event GetHiddenIdentifierValueEvent(ByRef value As String)
    'Public Event RefreshContainerEvent(sender As Object, e As lm.Comol.Modules.EduPath.Presentation.RefreshContainerArgs)
    Private _SmartTagsAvailable As Comol.Entity.SmartTags
    Public ReadOnly Property SmartTagsAvailable() As Comol.Entity.SmartTags
        Get
            If _SmartTagsAvailable Is Nothing Then
                _SmartTagsAvailable = ManagerConfiguration.GetSmartTags(Me.PageUtility.ApplicationUrlBase(True))
            End If
            Return _SmartTagsAvailable
        End Get
    End Property
    Public ReadOnly Property DisplayMessageToken(ByVal fileType As Helpers.Export.ExportFileType) As String
        Get
            Return Resource.getValue("DisplayMessageToken." & fileType.ToString)
        End Get
    End Property
    'Public Function GetControlScript(ByVal hiddenIdentifier As String, ByVal reloadPage As Boolean)
    '    Dim s As String = ""
    '    s &= "var TokenHiddenFieldId = """ & hiddenIdentifier & """;" & vbCrLf
    '    s &= "var CookieName = """ & CookieName & """;" & vbCrLf
    '    s &= "var DisplayMessagePdf = ""<h3>" & DisplayMessageToken(Helpers.Export.ExportFileType.pdf) & "</h3>"";" & vbCrLf
    '    s &= "var DisplayMessageRtf = ""<h3>" & DisplayMessageToken(Helpers.Export.ExportFileType.rtf) & "</h3>"";" & vbCrLf
    '    s &= "var fileDownloadCheckTimer;" & vbCrLf
    '    s &= "function blockUIForDownload(fileType,destinationUrl) {" & vbCrLf
    '    s &= "    var token = new Date().getTime(); //use the current timestamp as the token value" & vbCrLf
    '    s &= "    var message = """";" & vbCrLf
    '    s &= "    if (fileType == 2)" & vbCrLf
    '    s &= "        message = DisplayMessagePdf;" & vbCrLf
    '    s &= "    else if (fileType == 3)" & vbCrLf
    '    s &= "        message = DisplayMessageRtf;" & vbCrLf
    '    s &= "    $(""input[id='"" + TokenHiddenFieldId + ""']"").val(token);" & vbCrLf
    '    s &= "    $.blockUI({ message: """" + message + """", draggable: true });" & vbCrLf
    '    s &= "    fileDownloadCheckTimer = window.setInterval(function () {" & vbCrLf
    '    s &= "        var cookieValue = $.cookie(CookieName);" & vbCrLf
    '    s &= "        if (cookieValue == token)" & vbCrLf
    '    s &= "            finishDownload();" & vbCrLf
    '    s &= "    }, 1000);" & vbCrLf
    '    s &= "}" & vbCrLf & vbCrLf

    '    s &= "function finishDownload() {" & vbCrLf
    '    s &= "window.clearInterval(fileDownloadCheckTimer);" & vbCrLf
    '    s &= "    $.cookie(CookieName, null); //clears this cookie value" & vbCrLf
    '    s &= "    $.unblockUI();" & vbCrLf
    '    If (reloadPage) Then
    '        s &= "    window.location.reload();" & vbCrLf
    '    End If
    '    s &= "}" & vbCrLf
    '    Return s
    'End Function

    'Public Event ItemActionResult(err As lm.Comol.Core.Certifications.CertificationError, ByVal savingRequired As Boolean, ByVal saved As Boolean)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Certification", "EduPath")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBcertificationName_t)
            .setLabel(LBcertificationSavingOption_t)
            .setLabel(LBcertificationMinCompletionActivationOption_t)
            .setLabel(LBcertificationMinMarkActivationOption_t)
            .setLabel(LBusePathEndDateStatistics_t)
            .setLabel(LBactiveAfterPathEndDate_t)
            .setLabel(LBcertificationAutogenerateOption_t)
            .setLabel(LBcertificationAllowWithEmptyPlaceHolders_t)
            .setHyperLink(HYPpreviewCertification, False, True)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "InitializeControl"
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Function InitializeRemoteControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer)
    End Sub
    Public Sub InitializeControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        ForUserId = idUser
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Sub InitializeControl(initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) Implements IGenericModuleDisplayAction.InitializeControl
        CurrentPresenter.InitView(initializer, actionsToDisplay)
    End Sub
    Public Function InitializeRemoteControl(idUser As Integer, initializer As dtoModuleDisplayActionInitializer, actionsToDisplay As StandardActionType) As List(Of dtoModuleActionControl) Implements IGenericModuleDisplayAction.InitializeRemoteControl
        ForUserId = idUser
        Return CurrentPresenter.InitRemoteControlView(initializer, actionsToDisplay)
    End Function
#End Region

#Region "Display Methods"
    Public Sub DisplayRemovedObject() Implements IGenericModuleDisplayAction.DisplayRemovedObject
        Me.LBempty.Text = Resource.getValue("action.RemovedObject")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyAction() Implements IViewModuleCertificationAction.DisplayEmptyAction
        Me.LBempty.Text = "&nbsp;"
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayUnknownAction() Implements IViewModuleCertificationAction.DisplayUnknownAction
        Me.LBempty.Text = Resource.getValue("action.unhandled")
        Me.MLVcontrol.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayItem(name As String) Implements IViewModuleCertificationAction.DisplayItem
        Me.LBcertificationActionBegin.Text = String.Format(Me.Resource.getValue("CertificationActionBegin"), name)
        Me.HYPexecute.Visible = False
        MLVcontrol.SetActiveView(VIWdata)
    End Sub
    Private Sub DisplayItemAdminInfo(cAction As dtoSubActivity, links As List(Of dtoSubActivityLink), p As Path) Implements IViewModuleCertificationAction.DisplayItemAdminInfo
        Me.MLVcontrol.SetActiveView(VIWsettingsInfo)
        Me.LBcertificationName.Text = cAction.Name

        Me.HYPpreviewCertification.Visible = (cAction.IdCertificate > 0 OrElse cAction.IdCertificateVersion > 0)
        Me.HYPpreviewCertification.NavigateUrl = BaseUrl & lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(cAction.IdCertificate, cAction.IdCertificateVersion, False)

        LBcertificationAllowWithEmptyPlaceHolders.Text = Me.Resource.getValue("Certification.AllowWithEmptyPlaceHolders." & cAction.AllowWithEmptyPlaceHolders.ToString)

        If cAction.AutoGenerated Then
            LBcertificationAutogenerateOption.Text = Me.Resource.getValue("Certification.AutoGenerated")
        ElseIf cAction.SaveCertificate Then
            LBcertificationSavingOption.Text &= ". " & Me.Resource.getValue("Certification.SaveCertificate")
        Else
            LBcertificationAutogenerateOption.Text &= ". " & Me.Resource.getValue("Certification.OnlyUserDownload")
        End If

        Dim advancedInfo As String = ""
        LBcertificationSavingOption.Text = IIf(cAction.SaveCertificate, Resource.getValue("yes"), Resource.getValue("no"))
        LBcertificationAutogenerateOption.Text = IIf(cAction.SaveCertificate, Resource.getValue("yes"), Resource.getValue("no"))

        If cAction.AutoGenerated Then
            LBcertificationAutogenerateOption.Text = Me.Resource.getValue("Certification.AutoGenerated")
        ElseIf cAction.SaveCertificate Then
            LBcertificationSavingOption.Text &= ". " & Me.Resource.getValue("Certification.SaveCertificate")
        Else
            LBcertificationAutogenerateOption.Text &= ". " & Me.Resource.getValue("Certification.OnlyUserDownload")
        End If

        Me.DVminCompletion.Visible = cAction.ActiveOnMinCompletion
        Me.DVminMark.Visible = cAction.ActiveOnMinMark

        Me.LTcertificationMinCompletionActivationOption.Text = IIf(cAction.ActiveOnMinCompletion, Resource.getValue("yes"), Resource.getValue("no"))
        Me.LTcertificationMinMarkActivationOption.Text = IIf(cAction.ActiveOnMinMark, Resource.getValue("yes"), Resource.getValue("no"))

        If cAction.ActiveOnMinCompletion Then
            LTcertificationMinCompletionActivationOption.Text = p.MinCompletion.ToString & "%"
        End If
        If cAction.ActiveOnMinMark Then
            LTcertificationMinMarkActivationOption.Text = p.MinMark.ToString
        End If

        Me.LTusePathEndDateStatisticsOption.Text = IIf(cAction.UsePathEndDateStatistics, Resource.getValue("yes"), Resource.getValue("no"))
        Me.LTactiveAfterPathEndDateOption.Text = IIf(cAction.ActiveAfterPathEndDate, Resource.getValue("yes"), Resource.getValue("no"))
        'LBactiveAfterPathEndDateDetails.Visible = cAction.UsePathEndDateStatistics
        'LBactiveAfterPathEndDateDetails.Visible = cAction.ActiveAfterPathEndDate

        If (Not p.EndDate Is Nothing) Then
            If (p.FloatingDeadlines) Then
                LBusePathEndDateStatisticsDetails.Text = Resource.getValue("EndDate.Options.Parcipipant")
                LBactiveAfterPathEndDateDetails.Text = LBusePathEndDateStatisticsDetails.Text
            Else
                LBusePathEndDateStatisticsDetails.Text = Resource.getValue("EndDate.Options.Date").Replace("{date}", "<span class='expireddate'>" + p.EndDate.Value.ToShortDateString() + "</span>")
                LBactiveAfterPathEndDateDetails.Text = LBusePathEndDateStatisticsDetails.Text
            End If
        Else
            LBusePathEndDateStatisticsDetails.Text = Resource.getValue("EndDate.Options.Not")
            LBactiveAfterPathEndDateDetails.Text = Resource.getValue("EndDate.Options.Not")
        End If
        If Not IsNothing(links) AndAlso links.Count > 0 Then
            Dim quizItems As Dictionary(Of Long, String)
            quizItems = ServiceQS.GetQuestionairesName(links.Select(Function(l) l.IdObject).ToList(), PageUtility.LinguaID)
            For Each l As dtoSubActivityLink In links
                l.Name = quizItems(l.IdObject)
            Next
            Me.RPTquestionnaires.DataSource = links
            Me.RPTquestionnaires.DataBind()
            Me.RPTquestionnaires.Visible = True
        Else
            Me.RPTquestionnaires.Visible = False
        End If
    End Sub

    Private Sub DisplayDownloadUrl(name As String, url As String, alreadyCreated As Boolean, refreshContainer As Boolean) Implements IViewModuleCertificationAction.DisplayDownloadUrl
        If alreadyCreated Then
            LBcertificationActionBegin.Text = Me.Resource.getValue("CertificationActionDownload")
            HYPexecute.Text = String.Format(Me.Resource.getValue("LNBgenerateCertification.Download"), name)
            HYPexecute.ToolTip = Me.Resource.getValue("LNBgenerateCertification.ToolTip")
        Else
            LBcertificationActionBegin.Text = Me.Resource.getValue("CertificationActionForGenerate")
            HYPexecute.Text = String.Format(Me.Resource.getValue("LNBgenerateCertification.GenerateDownload"), name)
            HYPexecute.ToolTip = Me.Resource.getValue("LNBgenerateCertification.GenerateToolTip")
        End If
        HYPexecute.Visible = True
        HYPexecute.NavigateUrl = PageUtility.ApplicationUrlBase & url
        If refreshContainer Then
            HYPexecute.CssClass = LTcssClassRefresh.Text
        Else
            HYPexecute.CssClass = LTcssClassNoRefresh.Text
        End If
        MLVcontrol.SetActiveView(VIWdata)
    End Sub

    'Private Sub DisplayItem(name As String, uniqueId As Guid, fileExtension As String) Implements IViewModuleCertificationAction.DisplayItem
    '    Me.LBcertificationActionBegin.Text = Me.Resource.getValue("CertificationActionDownload")

    '    LNBgenerateCertification.Text = String.Format(Me.Resource.getValue("LNBgenerateCertification.Download"), name)
    '    LNBgenerateCertification.ToolTip = Me.Resource.getValue("LNBgenerateCertification.ToolTip")
    '    LNBgenerateCertification.Visible = True
    '    Me.LNBgenerateCertification.CommandName = "download"
    '    Me.LNBgenerateCertification.CommandArgument = uniqueId.ToString & "," & fileExtension
    '    MLVcontrol.SetActiveView(VIWdata)
    'End Sub
    'Private Sub DisplayItemForGenerate(name As String, save As Boolean) Implements IViewModuleCertificationAction.DisplayItemForGenerate
    '    Me.LBcertificationActionBegin.Text = Me.Resource.getValue("CertificationActionForGenerate")
    '    'SaveCertification = save
    '    Me.Resource.setLinkButton(LNBgenerateCertification, True, True)
    '    LNBgenerateCertification.Text = String.Format(Me.Resource.getValue("LNBgenerateCertification.GenerateDownload"), name)
    '    LNBgenerateCertification.ToolTip = String.Format(Me.Resource.getValue("LNBgenerateCertification.GenerateToolTip"))
    '    LNBgenerateCertification.Visible = True
    '    Me.LNBgenerateCertification.CommandName = "generate"
    '    MLVcontrol.SetActiveView(VIWdata)
    'End Sub
    Private Sub DisplayRemovedTemplate() Implements IViewModuleCertificationAction.DisplayRemovedTemplate
        Me.LBcertificationActionBegin.Text = String.Format(Me.Resource.getValue("DisplayRemovedTemplate"), CertificationName)
    End Sub
    Private Sub DisplayUnselectedTemplate() Implements IViewModuleCertificationAction.DisplayUnselectedTemplate
        Me.LBcertificationActionBegin.Text = String.Format(Me.Resource.getValue("DisplayUnselectedTemplate"), CertificationName)
    End Sub
    Private Sub DisplayUnselectedTemplateInfo() Implements IViewModuleCertificationAction.DisplayUnselectedTemplateInfo
        Me.LBcertificationActionBegin.Text = String.Format(Me.Resource.getValue("DisplayUnselectedTemplateInfo"), CertificationName)
    End Sub
    Private Sub DisplayPlaceHolders(items As List(Of lm.Comol.Core.ModuleLinks.dtoPlaceHolder)) Implements IViewModuleCertificationAction.DisplayPlaceHolders
        Dim places As New Dictionary(Of PlaceHolderType, Integer)
        places.Add(PlaceHolderType.zero, 0)
        places.Add(PlaceHolderType.one, 1)
        places.Add(PlaceHolderType.two, 2)
        places.Add(PlaceHolderType.three, 3)
        places.Add(PlaceHolderType.four, 4)

        For Each item As lm.Comol.Core.ModuleLinks.dtoPlaceHolder In items.Where(Function(i) i.Type <> PlaceHolderType.fullContainer AndAlso i.Type <> PlaceHolderType.none).ToList()
            Dim oLabel As Label = FindControl("LBplace" & places(item.Type))
            If Not IsNothing(oLabel) Then
                oLabel.Text = item.Text
                oLabel.Visible = True
                If Not String.IsNullOrEmpty(item.CssClass) Then
                    oLabel.CssClass = "plh plh" & places(item.Type).ToString() & " " & item.CssClass
                End If
            End If
        Next
    End Sub

#Region "Display Actions"
    Private Sub DisplayActions(actions As List(Of dtoModuleActionControl)) Implements IViewModuleCertificationAction.DisplayActions
        If actions.Count = 0 Then
            Me.RPTactions.Visible = False
        End If
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
        Me.MLVcontrol.SetActiveView(VIWdata)
    End Sub
    Private Sub DisplayEmptyActions() Implements IViewModuleCertificationAction.DisplayEmptyActions
        Me.RPTactions.Visible = False
    End Sub

#End Region
#End Region

#Region "Get Details / Translations"
    'Private Function GetProfileName(idProfileType As Integer) As String Implements IViewModuleCertificationAction.GetProfileName
    '    Dim profileType As COL_TipoPersona = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID = idProfileType Select o).FirstOrDefault
    '    If IsNothing(profileType) Then
    '        Return ""
    '    Else
    '        Return profileType.Descrizione
    '    End If
    'End Function
    'Private Function GetRoleName(subscription As Subscription) As String Implements IViewModuleCertificationAction.GetRoleName
    '    If Not IsNothing(subscription) Then
    '        Dim role As Comol.Entity.Role = (From o In COL_TipoRuolo.List(Me.PageUtility.LinguaID) Where o.ID = subscription.Role.Id Select o).FirstOrDefault
    '        If IsNothing(role) Then
    '            Return ""
    '        Else
    '            Return role.Name
    '        End If
    '    Else
    '        Return ""
    '    End If

    'End Function

    'Private Function GetBaseUrl() As String Implements IViewModuleCertificationAction.GetBaseUrl
    '    Return PageUtility.BaseUrl()
    'End Function
    Private Function GetBaseUrl(useSSL As Boolean) As String Implements IViewModuleCertificationAction.GetBaseUrl
        Return PageUtility.BaseUrl(useSSL)
    End Function

#End Region

#Region "Description"
    Public Function getDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction.getDescriptionByLink
        Return ""
    End Function
    Public Function GetInLineDescriptionByLink(link As lm.Comol.Core.DomainModel.ModuleLink, isGeneric As Boolean) As String Implements lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction.GetInLineDescriptionByLink
        Return ""
    End Function
    Public Function getDescriptionByActivity(sActivity As lm.Comol.Modules.EduPath.Domain.dtoSubActivity) As String Implements IViewModuleCertificationAction.getDescriptionByActivity
        If IsNothing(Resource) Then
            SetCultureSettings()
        End If
        Dim translation As String = Me.Resource.getValue("CertificationActionBegin")
        If Not String.IsNullOrEmpty(translation) Then
            translation = String.Format(translation, sActivity.Name)
        Else
            translation = sActivity.Name
        End If
        Return translation
    End Function
#End Region

#Region "AutoGenerate"
    Public Function GetQuizInfos(idItems As List(Of Long)) As List(Of dtoQuizInfo) Implements IViewModuleCertificationAction.GetQuizInfos
        Dim items As New List(Of dtoQuizInfo)
        For Each idQuestionaire As Integer In idItems
            Dim q As COL_Questionario.LazyQuestionnaire = ServiceQS.GetItem(Of COL_Questionario.LazyQuestionnaire)(idQuestionaire)
            If Not IsNothing(q) Then
                Dim evaluable As Boolean = False
                evaluable = (EvaluablePath AndAlso q.IdType <> COL_Questionario.QuestionnaireType.Poll AndAlso q.IdType <> COL_Questionario.QuestionnaireType.QuestionLibrary AndAlso q.IdType <> COL_Questionario.QuestionnaireType.Model) _
                    OrElse (Not EvaluablePath AndAlso q.IdType = COL_Questionario.QuestionnaireType.RandomMultipleAttempts)
                items.Add(New dtoQuizInfo() With {.IdQuestionnaire = q.Id, .Evaluable = evaluable, .EvaluationScale = q.EvaluationScale, .MinScore = q.MinScore, .Name = ServiceQS.GetItemName(q.Id, PageUtility.LinguaID), .Attempts = (From a In ServiceQS.GetQuestionnaireAttempts(q.Id, ForUserId, 0) Select New dtoQuizAttemptInfo() With {.Id = a.Id, .IdQuestionnaire = a.IdQuestionnnaire, .CorrectAnswers = a.CorrectAnswers, .Completed = a.CompletedOn.HasValue, .CompletedOn = a.CompletedOn, .QuestionsCount = a.QuestionsCount, .QuestionsSkipped = a.QuestionsSkipped, .RelativeScore = a.RelativeScore, .Score = a.Score, .UngradedAnswers = a.UngradedAnswers, .WrongAnswers = a.WrongAnswers, .Passed = a.RelativeScore >= q.MinScore}).ToList()})
            End If
        Next
        Return items
    End Function
    Public Function IsCertificationActive(idPath As Long, idUser As Integer, item As lm.Comol.Modules.EduPath.Domain.dtoSubActivity, viewStatBefore As DateTime) As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction.IsCertificationActive
        Return Me.CurrentPresenter.isActionActive(idPath, idUser, item, viewStatBefore)
    End Function
    Public Function HasCertificationToAutoGenerate(idPath As Long, idCommunity As Integer, idUser As Integer, item As lm.Comol.Modules.EduPath.Domain.dtoSubActivity, viewStatBefore As Date) As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction.HasCertificationToAutoGenerate
        Return Me.CurrentPresenter.HasCertificationToAutoGenerate(idPath, idCommunity, idUser, item, viewStatBefore)
    End Function
#End Region

#Region "Last Version"
    'Public Sub DownloadCertification(idPath As Long, idUser As Integer, item As lm.Comol.Modules.EduPath.Domain.dtoSubActivity, viewStatBefore As DateTime, cookieValue As String)
    '    '    Dim webFileName As String = CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName"))

    '    '    If item.SaveCertificate Then
    '    '        Dim certification As lm.Comol.Core.Certifications.Certification = CurrentPresenter.GetAvailableUserCertification(item, IdCommunityContainer, idUser)
    '    '        Dim storedFileName As String = Me.CertificationFilePath
    '    '        If Not String.IsNullOrEmpty(storedFileName) AndAlso Not IsNothing(certification) Then
    '    '            storedFileName &= IIf(storedFileName.EndsWith("/") OrElse storedFileName.EndsWith("\"), "", "\") & idUser.ToString() & "\" & certification.FileUniqueId.ToString & ".cer"
    '    '        End If
    '    '        If Not String.IsNullOrEmpty(storedFileName) AndAlso lm.Comol.Core.File.Exists.File(storedFileName) Then
    '    '            DownloadExistingFile(CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName")), storedFileName, certification, item.SaveCertificate, cookieValue)
    '    '        Else
    '    '            GenerateCertificationFile(IIf(idUser = PageUtility.CurrentUser.ID, lm.Comol.Core.Certifications.CertificationType.UserRequired, lm.Comol.Core.Certifications.CertificationType.ManagerProduced), idUser, False, False, False, cookieValue)
    '    '        End If
    '    '    Else
    '    '        GenerateCertificationFile(lm.Comol.Core.Certifications.CertificationType.RuntimeProduced, idUser, False, False, True, cookieValue)
    '    '    End If
    'End Sub

    'Private Function GenerateCertificationFile(ByVal certType As lm.Comol.Core.Certifications.CertificationType, idUser As Integer, ByVal saveFile As Boolean, ByVal saveAction As Boolean, ByVal onlyDownload As Boolean, ByVal cookieValue As String, Optional ByVal fileUniqueId As String = "", Optional ByVal fileExtension As String = "") As lm.Comol.Core.Certifications.CertificationError
    '    Dim result As lm.Comol.Core.Certifications.CertificationError = lm.Comol.Core.Certifications.CertificationError.None

    '    '    Dim fileType As Helpers.Export.ExportFileType = Helpers.Export.ExportFileType.pdf
    '    'Dim templateError As New lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template
    '    'If String.IsNullOrEmpty(fileExtension) Then
    '    '    fileExtension = fileType.ToString
    '    'End If
    '    Try
    '        If saveAction Then
    '            Me.CurrentPresenter.ExecuteAction(IdPath, IdSubActivity, StatusStatistic.CompletedPassed)
    '        End If

    '        Dim storedFileName As String = Me.CertificationFilePath
    '        If Not String.IsNullOrEmpty(storedFileName) AndAlso Not String.IsNullOrEmpty(fileUniqueId) Then
    '            storedFileName &= IIf(storedFileName.EndsWith("/") OrElse storedFileName.EndsWith("\"), "", "\") & idUser.ToString() & "\" & fileUniqueId & ".cer"
    '        End If
    '        If Not String.IsNullOrEmpty(storedFileName) AndAlso Not String.IsNullOrEmpty(fileUniqueId) AndAlso lm.Comol.Core.File.Exists.File(storedFileName) Then
    '            If String.IsNullOrEmpty(fileExtension) Then
    '                fileExtension = "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
    '            End If
    '            DownloadExistingFile(CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName")), storedFileName, fileExtension, saveFile, cookieValue)
    '        Else
    '            Dim webFileName As String = CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName"))
    '            Dim fileName As String = ""
    '            Dim err As lm.Comol.Core.Certifications.CertificationError = SaveCertificationFile(certType, AllowWithEmptyPlaceHolders, idUser, fileName)
    '            If String.IsNullOrEmpty(fileName) Then
    '                RaiseEvent ItemActionResult(err, saveFile, False)
    '            Else
    '                Dim contentType As String = Response.ContentType
    '                Dim cFilename As String = CurrentPresenter.ReplaceInvalidFileName(webFileName) & "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
    '                Response.AddHeader("Content-Disposition", "attachment; filename=" & cFilename)
    '                Response.ContentType = "application/pdf"

    '                Response.AppendCookie(New HttpCookie(CookieName, cookieValue))
    '                Dim chunkSize As Integer = 64
    '                Dim offset As Integer = 0, read As Integer = 0

    '                If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(fileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
    '                    RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, saveFile, True)
    '                    Response.ContentType = contentType
    '                    Response.Headers.Remove("Content-Disposition")
    '                End If
    '                Context.ApplicationInstance.CompleteRequest()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        NotifyError(PageUtility.SystemSettings, ex)
    '        result = lm.Comol.Core.Certifications.CertificationError.Unknown
    '    End Try
    '    Return result
    'End Function

    ''' <summary>
    ''' Consente di rigenerare un certificato
    ''' </summary>
    ''' <param name="idPath"></param>
    ''' <param name="idUser"></param>
    ''' <param name="item"></param>
    ''' <param name="cookieValue"></param>
    ''' <remarks></remarks>OLDDDDDDDDDDDDDDDDDDDDDDDD
    'Public Sub RestoreCertificate(idPath As Long, idUser As Integer, item As lm.Comol.Modules.EduPath.Domain.dtoSubActivity, cookieValue As String) Implements IViewModuleCertificationAction.RestoreCertificate
    '    Dim webFileName As String = CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName"))

    '    'Dim fileName As String = ""
    '    'Dim err As lm.Comol.Core.Certifications.CertificationError = SaveCertificationFile(IIf(item.AutoGenerated, lm.Comol.Core.Certifications.CertificationType.ManagerProduced, lm.Comol.Core.Certifications.CertificationType.RuntimeProduced), item.AllowWithEmptyPlaceHolders, idUser, fileName)
    '    'If String.IsNullOrEmpty(fileName) Then
    '    '    RaiseEvent ItemActionResult(err, item.SaveCertificate, False)
    '    'Else
    '    '    Dim contentType As String = Response.ContentType
    '    '    Dim cFilename As String = CurrentPresenter.ReplaceInvalidFileName(webFileName) & "." & Helpers.Export.ExportFileType.pdf.ToString().ToLower
    '    '    Response.AddHeader("Content-Disposition", "attachment; filename=" & cFilename)
    '    '    Response.ContentType = "application/pdf"

    '    '    Response.AppendCookie(New HttpCookie(CookieName, cookieValue))
    '    '    Dim chunkSize As Integer = 64
    '    '    Dim offset As Integer = 0, read As Integer = 0

    '    '    If Not lm.Comol.Core.File.TransmitFactory.TransmitFile(fileName, Response) = lm.Comol.Core.File.FileMessage.Read Then
    '    '        RaiseEvent ItemActionResult(lm.Comol.Core.Certifications.CertificationError.TransmittingFile, item.SaveCertificate, True)
    '    '        Response.ContentType = contentType
    '    '        Response.Headers.Remove("Content-Disposition")
    '    '    End If
    '    '    Context.ApplicationInstance.CompleteRequest()
    '    'End If
    'End Sub

    ''' <summary>
    ''' Si occupa di salvare su disco il certificato, sia esso prodotto in run-time, sia esso salvato su file system una-tantum
    ''' </summary>
    ''' <param name="crType"></param>
    ''' <param name="allowEmptyPlaceHolders"></param>
    ''' <param name="idUser"></param>
    ''' <param name="filename"></param>
    ''' <param name="template"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveCertificationFile(ByVal crType As lm.Comol.Core.Certifications.CertificationType, ByVal allowEmptyPlaceHolders As Boolean, ByVal idUser As Integer, ByRef filename As String, Optional template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = Nothing) As lm.Comol.Core.Certifications.CertificationError
        Dim result As lm.Comol.Core.Certifications.CertificationError = lm.Comol.Core.Certifications.CertificationError.None
        Dim allowSave As Boolean = True
        Try
            Dim baseFilePath As String = Me.CertificationFilePath
            Dim language As lm.Comol.Core.DomainModel.Language = CurrentPresenter.GetUserLanguage(idUser)
            If Not String.IsNullOrEmpty(baseFilePath) Then
                baseFilePath &= IIf(baseFilePath.EndsWith("/") OrElse baseFilePath.EndsWith("\"), "", "\") & idUser.ToString() & "\"
                If Not lm.Comol.Core.File.Exists.Directory(baseFilePath) Then
                    allowSave = lm.Comol.Core.File.Create.Directory(baseFilePath)
                End If
                If allowSave Then
                    If IsNothing(template) Then
                        template = CurrentPresenter.FillDataIntoTemplate(IdSubActivity, TemplateBaseUrl, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(language.Id), result)
                    End If

                    If result = lm.Comol.Core.Certifications.CertificationError.None OrElse (allowEmptyPlaceHolders AndAlso (result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItem OrElse result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItems)) Then
                        Dim idFile As Guid = Guid.NewGuid

                        'ToDo: Export
                        result = lm.Comol.Core.Certifications.CertificationError.SavingFile

                    End If
                Else
                    result = lm.Comol.Core.Certifications.CertificationError.RepositoryError
                End If
            Else
                result = lm.Comol.Core.Certifications.CertificationError.RepositoryError
            End If
        Catch ex As Exception
            result = lm.Comol.Core.Certifications.CertificationError.Unknown
        End Try
        Return result
    End Function


#End Region

#End Region

    Public Function AutoGenerateCertificationNEW(idPath As Long, idCommunity As Integer, idUser As Integer, item As lm.Comol.Modules.EduPath.Domain.dtoSubActivity, viewStatBefore As DateTime, status As StatusStatistic, ByRef newStatus As StatusStatistic) As lm.Comol.Core.Certifications.CertificationError Implements IViewModuleCertificationAction.AutoGenerateCertification
        Dim result As lm.Comol.Core.Certifications.CertificationError = lm.Comol.Core.Certifications.CertificationError.None
        Me.IdPath = idPath
        Me.IdCommunityContainer = idCommunity
        If Not IsNothing(item) Then
            Me.IdSubActivity = item.Id
            Me.IdTemplate = item.IdCertificate
            Me.IdTemplateVersion = item.IdCertificateVersion
        End If
        newStatus = status

        If HasCertificationToAutoGenerate(idPath, idCommunity, idUser, item, viewStatBefore) Then
            Dim baseFilePath As String = Me.CertificationFilePath
            If Not String.IsNullOrEmpty(baseFilePath) Then
                Try
                    Dim isMandatory As Boolean = CurrentPresenter.isCertificationActionMandatory(item)
                    If isMandatory Then
                        If CurrentPresenter.ExecuteAction(idPath, item.Id, StatusStatistic.CompletedPassed) Then
                            newStatus = StatusStatistic.CompletedPassed
                        End If
                    End If

                    Dim webFileName As String = CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName"))

                    Dim fileName As String = ""
                    result = SaveCertificationFile(lm.Comol.Core.Certifications.CertificationType.AutoProduced, item.AllowWithEmptyPlaceHolders, idUser, fileName)

                    If newStatus <> StatusStatistic.None AndAlso (result = lm.Comol.Core.Certifications.CertificationError.None OrElse (item.AllowWithEmptyPlaceHolders AndAlso (result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItem OrElse result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItems))) Then
                        'Verify!!!  Completed and passed insteed of Browser for AutoGeneratedCertifications.
                        Me.CurrentPresenter.ExecuteAction(idPath, item.Id, StatusStatistic.CompletedPassed)
                    ElseIf isMandatory AndAlso Not (result = lm.Comol.Core.Certifications.CertificationError.None OrElse (item.AllowWithEmptyPlaceHolders AndAlso (result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItem OrElse result = lm.Comol.Core.Certifications.CertificationError.EmptyTemplateItems))) Then
                        '    CurrentPresenter.ExecuteAction(idPath, item.Id, StatusStatistic.BrowsedStarted)
                    End If
                    ''Fix Attempt
                    'Dim t As lm.Comol.Core.Certifications.CertificationError
                    'Dim template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template = CurrentPresenter.FillDataIntoTemplate(idCommunity, idUser, idPath, item.Id, TemplateBaseUrl, item.IdCertificate, item.IdCertificateVersion, PageUtility.SystemSettings.Presenter.PortalDisplay.LocalizeIstanceName(PageUtility.LinguaID), t)
                    'Dim idFile As Guid = Guid.NewGuid




                    'If Not lm.Comol.Core.File.Exists.Directory(baseFilePath) Then
                    '    lm.Comol.Core.File.Create.Directory(baseFilePath)
                    'End If
                    'Dim fileType As Helpers.Export.ExportFileType = Helpers.Export.ExportFileType.pdf
                    'Dim fileName As String = CurrentPresenter.GetUserCertificationFileName(Resource.getValue("Certification.FileName"))
                    'Dim xp As New lm.Comol.Core.BaseModules.DocTemplate.Helpers.HelperExportPDF
                    'baseFilePath &= IIf(baseFilePath.EndsWith("/") OrElse baseFilePath.EndsWith("\"), "", "\") & idUser.ToString() & "\"

                    'xp.ExportToPdf(template, False, fileName, True, baseFilePath & idFile.ToString & ".cer", False, Response, Nothing)

                    'result = Me.CurrentPresenter.SaveCertificationFile(lm.Comol.Core.Certifications.CertificationType.AutoProduced, True, idCommunity, idUser, fileName, Resource.getValue("Certification.CertificationDescription"), idPath, item.Id, idFile, "." & fileType.ToString())
                    ''If Not result AndAlso status <> StatusStatistic.None Then
                    ''    Me.CurrentPresenter.ExecuteAction(idPath, IdSubActivity, status)
                    ''End If
                Catch ex As Exception
                    result = False
                End Try
            End If
        End If
        Return result
    End Function

    'Private Function HtmlCheckFileName(fileName As String) As String
    '    Return HttpUtility.UrlPathEncode(fileName.Replace(" ", "_"))
    'End Function


    '    string regex = String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
    'Regex removeInvalidChars = new Regex(regex, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

#Region "Internal Methods"

    'Protected Sub LNBgenerateCertification_Click(sender As Object, e As System.EventArgs)
    '    Dim cookieValue As String = ""
    '    Dim cerType As lm.Comol.Core.Certifications.CertificationType = lm.Comol.Core.Certifications.CertificationType.RuntimeProduced
    '    If AutoGenerated Then
    '        cerType = lm.Comol.Core.Certifications.CertificationType.AutoProduced
    '    Else
    '        If ForUserId = PageUtility.CurrentUser.ID Then
    '            cerType = lm.Comol.Core.Certifications.CertificationType.UserRequired
    '        Else
    '            cerType = lm.Comol.Core.Certifications.CertificationType.ManagerProduced
    '        End If
    '    End If
    '    RaiseEvent GetHiddenIdentifierValueEvent(cookieValue)
    '    Select Case Me.LNBgenerateCertification.CommandName
    '        Case "download"
    '            Dim s As String() = Me.LNBgenerateCertification.CommandArgument.Split(",")

    '            GenerateCertificationFile(cerType, ForUserId, SaveCertification, SaveRunningAction, True, cookieValue, s(0), s(1))
    '        Case Else
    '            GenerateCertificationFile(cerType, ForUserId, SaveCertification, SaveRunningAction, False, cookieValue)
    '    End Select
    'End Sub
#Region "Notify Error"
    Private Sub NotifyError(ByVal settings As ComolSettings, ByVal exception As Exception)

        Dim notificationService As ErrorsNotificationService.iErrorsNotificationService = Nothing
        Try
            notificationService = New ErrorsNotificationService.iErrorsNotificationServiceClient()
            Dim oError As ErrorsNotificationService.GenericWebError = New ErrorsNotificationService.GenericWebError()
            With oError
                'CONTEXT VARIABLES
                .ServerName = Server.MachineName
                .SentDate = Now
                .Day = .SentDate.Date
                .ComolUniqueID = settings.NotificationErrorService.ComolUniqueID

                .QueryString = Request.QueryString.ToString
                .UniqueID = System.Guid.NewGuid()
                .Url = Request.Url.AbsolutePath
                .Persist = settings.NotificationErrorService.FindPersistTo(ErrorsNotificationService.ErrorType.GenericWebError)
                .Type = ErrorsNotificationService.ErrorType.GenericWebError
                Dim oLastError As Exception = Server.GetLastError()
                Dim oException As Exception = oLastError.GetBaseException()

                .Message = oLastError.Message.ToString
                If Not IsNothing(Server.GetLastError.InnerException) Then
                    .InnerExceptionMessage = Server.GetLastError.InnerException.ToString
                End If
                .BaseExceptionStackTrace = oException.StackTrace
                .ExceptionSource = oLastError.Source.ToString

            End With
            notificationService.sendGenericWebError(oError)
            CloseNotificationService(notificationService)
        Catch ex As Exception
            CloseNotificationService(notificationService)
        End Try
    End Sub
    Private Sub CloseNotificationService(ByVal notificationService As ErrorsNotificationService.iErrorsNotificationService)
        If Not IsNothing(notificationService) Then
            Dim service As System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService) = DirectCast(notificationService, System.ServiceModel.ClientBase(Of ErrorsNotificationService.iErrorsNotificationService))
            service.Abort()
            service = Nothing
        End If
    End Sub
#End Region
    Private Function GetFileSize(ByVal size As Long) As String
        Dim sizeTranslated As String = ""
        If size = 0 Then
            sizeTranslated = "&nbsp;"
        Else
            If size < 1024 Then
                sizeTranslated = "1 kb"
            Else
                size = size / 1024
                If size < 1024 Then
                    sizeTranslated = Math.Round(size) & " kb"
                ElseIf size >= 1024 Then
                    sizeTranslated = Math.Round(size / 1024, 2) & " mb"
                End If
            End If
        End If
        Return sizeTranslated
    End Function
    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim action As dtoModuleActionControl = DirectCast(e.Item.DataItem, dtoModuleActionControl)
            Dim oButton As LinkButton = e.Item.FindControl("LNBaction")
            Dim link As HyperLink = e.Item.FindControl("HYPaction")
            link.Target = IIf(action.isPopupUrl, "_blank", "")
            link.CssClass = "action "
            link.ToolTip = Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString)

            oButton.Text = action.ControlType.ToString()
            oButton.CssClass = "action "
            oButton.ToolTip = Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString)
            oButton.Visible = (action.ControlType <> StandardActionType.Edit AndAlso action.ControlType = StandardActionType.Create)
            link.Visible = (action.ControlType = StandardActionType.Edit OrElse action.ControlType = StandardActionType.Create)
            If String.IsNullOrEmpty(action.LinkUrl) Then
                oButton.CommandName = IIf(String.IsNullOrEmpty(action.ExtraData), "generate", "download")
                oButton.CommandArgument = action.ExtraData
            End If
            Select Case action.ControlType
                Case StandardActionType.Play
                    oButton.CssClass &= "certification"

                    'link.ToolTip = Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString & ".QuizStatus." & ItemStatus.ToString)

                    'If String.IsNullOrEmpty(link.ToolTip) Then
                    '    link.ToolTip = String.Format(Me.Resource.getValue("ToolTip.StandardActionType." & action.ControlType.ToString), QuizName)
                    'ElseIf ItemStatus = QuizStatus.ViewCompiled OrElse ItemStatus = QuizStatus.Compiled Then
                    '    link.ToolTip = String.Format(link.ToolTip, QuizName, Score)
                    'Else
                    '    link.ToolTip = String.Format(link.ToolTip, QuizName)
                    'End If
                Case StandardActionType.ViewAdvancedStatistics
                    oButton.CssClass &= "stats" ' & IconSizeToString
                Case StandardActionType.ViewPersonalStatistics
                    oButton.CssClass &= "stats" '& IconSizeToString
                Case StandardActionType.ViewUserStatistics
                    oButton.CssClass &= "stats" '& IconSizeToString
                Case StandardActionType.Create
                    link.CssClass &= "add" '& IconSizeToString
                    link.Text = action.ControlType.ToString()
                    link.NavigateUrl = action.LinkUrl
                Case StandardActionType.Edit
                    link.CssClass &= "edit" '& IconSizeToString
                    link.Text = action.ControlType.ToString()
                    link.NavigateUrl = action.LinkUrl
            End Select
        End If
    End Sub
    Private Sub RPTquestionnaires_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTquestionnaires.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As dtoSubActivityLink = CType(e.Item.DataItem, dtoSubActivityLink)
            If (Not item Is Nothing) Then
                Dim oLabel As Label = e.Item.FindControl("LBselectedMandatoryOption")

                Dim olbm As Label = e.Item.FindControl("LBselectedMandatoryOption")
                olbm.Visible = item.Mandatory
                Dim olbv As Label = e.Item.FindControl("LBselectedVisibleOption")
                olbv.Visible = item.Visible

                oLabel.Text = Resource.getValue("LBselectedMandatoryOption." & item.Mandatory.ToString)
                oLabel.ToolTip = Resource.getValue("LBselectedMandatoryOption." & item.Mandatory.ToString & ".ToolTip")

                oLabel = e.Item.FindControl("LBselectedVisibleOption")
                oLabel.Text = Resource.getValue("LBselectedVisibleOption." & item.Visible.ToString)
                oLabel.ToolTip = Resource.getValue("LBselectedVisibleOption." & item.Visible.ToString & ".ToolTip")
            End If
        ElseIf (e.Item.ItemType = ListItemType.Header) Then
            Resource.setLabel(e.Item.FindControl("LBquizTitle"))
            Resource.setLabel(e.Item.FindControl("LBmandatoryTitle"))
            Resource.setLabel(e.Item.FindControl("LBvisibleTitle"))
        End If
    End Sub

#End Region


End Class