Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_InputRequiredFile
    Inherits BaseControl
    Implements IViewInputRequiredFile


#Region "Context"
    Private _Presenter As InputRequiredFilePresenter
    Private ReadOnly Property CurrentPresenter() As InputRequiredFilePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InputRequiredFilePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property Disabled As Boolean Implements IViewInputRequiredFile.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            Me.BTNremoveFile.Enabled = Not value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewInputRequiredFile.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewInputRequiredFile.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdLink As Long Implements IViewInputRequiredFile.IdLink
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property
    Private Property IdRequiredFile As Long Implements IViewInputRequiredFile.IdRequiredFile
        Get
            Return ViewStateOrDefault("IdRequiredFile", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdRequiredFile") = value
        End Set
    End Property
    Private Property IdSubmittedFile As Long Implements IViewInputRequiredFile.IdSubmittedFile
        Get
            Return ViewStateOrDefault("IdSubmittedFile", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmittedFile") = value
        End Set
    End Property

    Private Property Mandatory As Boolean Implements IViewInputRequiredFile.Mandatory
        Get
            Return ViewStateOrDefault("Mandatory", True)
        End Get
        Set(value As Boolean)
            ViewState("Mandatory") = value
        End Set
    End Property

    Public ReadOnly Property isValid As Boolean Implements IViewInputRequiredFile.isValid
        Get
            Dim isMandatory As Boolean = Me.Mandatory
            Return Not isMandatory OrElse (isMandatory AndAlso Me.IdLink >= 0)
        End Get
    End Property

    Private Property CurrentError As FieldError Implements IViewInputRequiredFile.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", FieldError.None)
        End Get
        Set(value As FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Public ReadOnly Property GetFile As dtoCallSubmissionFile Implements IViewInputRequiredFile.GetFile
        Get
            Dim dto As New dtoCallSubmissionFile With {.FileToSubmit = New dtoCallRequestedFile(), .Submitted = New dtoSubmittedFile()}
            dto.FileToSubmit.Id = IdRequiredFile
            dto.FileToSubmit.Mandatory = Mandatory
            dto.Submitted.Id = IdSubmittedFile
            dto.Submitted.ModuleLinkId = IdLink
            Return dto
        End Get
    End Property
    Public ReadOnly Property ToUpload As Boolean Implements IViewInputRequiredFile.ToUpload
        Get
            Return (IdLink = 0 OrElse IdSubmittedFile = 0)
        End Get
    End Property

    Private Property IdCallCommunity As Integer Implements IViewInputRequiredFile.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
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
    Private _PostBackTriggers As String
    Public Property PostBackTriggers As String
        Get
            Return _PostBackTriggers
        End Get
        Set(value As String)
            _PostBackTriggers = value
        End Set
    End Property
    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = FieldError.None, "", "error")
        End Get
    End Property
    Public Event RemoveFile(ByVal idSubmitted As Long)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(BTNremoveFile, True, , , True)
        End With
    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyField() Implements IViewInputRequiredFile.DisplayEmptyFile
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayInputError() Implements IViewInputRequiredFile.DisplayInputError
        LBerrorMessagefileinput.Text = Resource.getValue("FieldError." & FieldError.Invalid)
        If Not LBfileinputText.CssClass.Contains(" error") Then
            LBfileinputText.CssClass &= " error"
        End If
    End Sub
    Private Sub HideInputError() Implements IViewInputRequiredFile.HideInputError
        LBfileinputText.CssClass = Replace(LBfileinputText.CssClass, " error", "")
    End Sub

    Private Sub RefreshFileField() Implements IViewInputRequiredFile.RefreshFileField
        CTRLinternalUploader.Visible = True
        BTNremoveFile.Visible = False
        CTRLdisplayItem.Visible = False
        IdLink = 0
    End Sub
    Private Sub RefreshFileField(link As lm.Comol.Core.DomainModel.ModuleLink) Implements IViewInputRequiredFile.RefreshFileField
        If IsNothing(link) Then
            RefreshFileField()
        Else
            RefreshFileField(New liteModuleLink(link))
        End If
    End Sub

    Private Sub RefreshFileField(link As lm.Comol.Core.DomainModel.liteModuleLink) Implements IViewInputRequiredFile.RefreshFileField
        Dim uploadFile As Boolean = IsNothing(link)
        '     CTRLinternalUploader.AjaxEnabled = uploadFile
        CTRLinternalUploader.Visible = uploadFile
        BTNremoveFile.Visible = Not uploadFile
        CTRLdisplayItem.Visible = Not uploadFile

        If Not uploadFile Then
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            Dim actions As List(Of dtoModuleActionControl)
            If Disabled Then
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
            Else
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
            End If

            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'CTRLdisplayFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            'initializer.Link = link
            'CTRLdisplayFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
            IdLink = link.Id
            LBrequiredFileHelp.Visible = False
        Else
            IdLink = 0
        End If
    End Sub

#End Region

#Region "Internal"
    Private Sub BTNremoveFile_Click(sender As Object, e As System.EventArgs) Handles BTNremoveFile.Click
        RaiseEvent RemoveFile(IdSubmittedFile)
    End Sub
#End Region
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, item As dtoCallSubmissionFile, disabled As Boolean, allowAnonymous As Boolean) Implements IViewInputRequiredFile.InitializeControl
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, item, disabled, allowAnonymous)
    End Sub
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, item As dtoCallSubmissionFile, disabled As Boolean, err As FieldError, allowAnonymous As Boolean) Implements IViewInputRequiredFile.InitializeControl
        CurrentError = err
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, item, disabled, allowAnonymous)

        If Not IsNothing(item) AndAlso Not IsNothing(item.FileToSubmit) Then
            Me.LBerrorMessagefileinput.Visible = Not (err = FieldError.None)
            If Not (err = FieldError.None) Then
                LBerrorMessagefileinput.Text = Resource.getValue("FieldError." & err.ToString())
            End If
            If (err = FieldError.None) Then
                LBfileinputText.CssClass = Replace(LBfileinputText.CssClass, " error", "")
            Else
                If Not LBfileinputText.CssClass.Contains(" error") Then
                    LBfileinputText.CssClass &= " error"
                End If
            End If
        End If
    End Sub
    Private Sub SetupView(item As dtoCallSubmissionFile, idUploader As Integer, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, allowAnonymous As Boolean) Implements IViewInputRequiredFile.SetupView
        SetInternazionalizzazione()
        Me.IdRequiredFile = item.FileToSubmit.Id
        LBfileinputDescription.Text = item.FileToSubmit.Description

        If Not String.IsNullOrEmpty(item.FileToSubmit.Tooltip) Then
            Me.LBrequiredFileHelp.Visible = True
            LBrequiredFileHelp.Text = item.FileToSubmit.Tooltip
        Else
            Me.LBrequiredFileHelp.Visible = False
        End If
        LBfileinputText.Text = IIf(item.FileToSubmit.Mandatory, "(*)", "") & item.FileToSubmit.Name

        Dim toUpload As Boolean = IsNothing(item.Submitted) OrElse IsNothing(item.Submitted.Link)
        'Me.CTRLfileUploader.AjaxEnabled = Not Disabled
        CTRLinternalUploader.Enabled = Not Disabled
        CTRLinternalUploader.Visible = toUpload AndAlso Not Disabled
        Me.BTNremoveFile.Visible = Not toUpload AndAlso Not Disabled
        CTRLdisplayItem.Visible = Not toUpload

        If Not toUpload Then
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = item.Submitted.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            Dim actions As List(Of dtoModuleActionControl)
            If Disabled Then
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
            Else
                CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
            End If
            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'CTRLdisplayFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            'initializer.Link = item.Submitted.Link
            'CTRLdisplayFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
            Me.IdLink = item.Submitted.Link.Id
            Me.IdSubmittedFile = item.Submitted.Id
            Me.LBrequiredFileHelp.Visible = False
        Else
            Me.IdLink = 0
            CTRLinternalUploader.PostbackTriggers = PostBackTriggers
            CTRLinternalUploader.AllowAnonymousUpload = allowAnonymous
            CTRLinternalUploader.InitializeControl(idUploader, identifier)
        End If

        'If Not IsNothing(oTextBox) Then
        Dim cssClass = DVrequiredFile.Attributes("class")
        If (item.FieldError = FieldError.None) Then
            cssClass = Replace(cssClass, " error", "")
        Else
            If Not cssClass.Contains(" error") Then
                cssClass &= " error"
            End If
        End If
        DVrequiredFile.Attributes("class") = cssClass
        Me.MLVfield.SetActiveView(VIWfile)
    End Sub

 
    Public Function AddInternalFile(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As dtoRequestedFileUpload Implements IViewInputRequiredFile.AddInternalFile
        Dim items As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) = CTRLinternalUploader.AddModuleInternalFiles(submission, submission.Id, objectType, moduleCode, moduleAction)

        If IsNothing(items) OrElse Not items.Where(Function(i) i.IsAdded).Any Then
            Return Nothing
        Else
            Dim uploadedFile As New dtoRequestedFileUpload
            uploadedFile.IdRequired = IdRequiredFile
            uploadedFile.ActionLink = items.Where(Function(i) i.IsAdded).Select(Function(i) i.Link).FirstOrDefault()
            Return uploadedFile
        End If

        'uploadedFile.ActionLink = CTRLfileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, submission, moduleCode, moduleAction, objectType)
        'If uploadedFile.ActionLink Is Nothing Then
        '    uploadedFile = Nothing
        'Else
        '    uploadedFile.IdRequired = IdRequiredFile
        'End If
        'Return uploadedFile
    End Function
    Private Sub CTRLinternalUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLinternalUploader.IsValidOperation
        isvalid = True
    End Sub
End Class