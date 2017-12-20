Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.FileRepository.Domain

Public Class UC_CommunityDiaryAddAttachment
    Inherits BaseControl
    Implements IViewAddAttachment


#Region "Context"
    Private _Presenter As AddAttachmentPresenter
    Private ReadOnly Property CurrentPresenter() As AddAttachmentPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New AddAttachmentPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewAddAttachment.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewAddAttachment.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewAddAttachment.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewAddAttachment.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Public Property DisplayInfo As Boolean Implements IViewAddAttachment.DisplayInfo
        Get
            Return ViewStateOrDefault("DisplayInfo", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayInfo") = value
        End Set
    End Property

    Private Property CurrentAction As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions Implements IViewAddAttachment.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.none)
        End Get
        Set(value As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
            ViewState("CurrentAction") = value
            Resource.setButtonByValue(BTNaddAttachment, value.ToString, True)
        End Set
    End Property
    Private Property IdEvent As Long Implements IViewAddAttachment.IdEvent
        Get
            Return ViewStateOrDefault("IdEvent", 0)
        End Get
        Set(value As Long)
            ViewState("IdEvent") = value
        End Set
    End Property
    Private Property IdEventItem As Long Implements IViewAddAttachment.IdEventItem
        Get
            Return ViewStateOrDefault("IdEventItem", 0)
        End Get
        Set(value As Long)
            ViewState("IdEventItem") = value
        End Set
    End Property
    Private Property IdEventCommunity As Integer Implements IViewAddAttachment.IdEventCommunity
        Get
            Return ViewStateOrDefault("IdEventCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdEventCommunity") = value
        End Set
    End Property
    Private Property LessonNumber As Integer Implements IViewAddAttachment.LessonNumber
        Get
            Return ViewStateOrDefault("LessonNumber", 0)
        End Get
        Set(value As Integer)
            ViewState("LessonNumber") = value
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
    Public Property UploaderCssClass As String
        Get
            Return ViewStateOrDefault("UploaderCssClass", "")
        End Get
        Set(value As String)
            ViewState("UploaderCssClass") = value
        End Set
    End Property
    Public Event WorkingSessionExpired()
    Public Event ItemsAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event ItemsNotAdded(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event NoFilesToAdd(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions)
    Public Event EventItemNotFound()

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CommunityDiary", "Modules", "CommunityDiary")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '    .setButton(BTNaddAttachment, True)
            .setLinkButton(LNBcloseAttachmentWindow, False, True)
            .setLabel(LBtitle_t)
            .setLabel(LBitemTime_t)
            .setLabel(LBhideItemFiles_t)
            .setLabel(LBhideItemFiles)
            .setLabel(LBhideRepositoryFiles)
            .setLabel(LBhideRepositoryFiles_t)
        End With
    End Sub
#End Region

#Region "Implements"

#Region "Initializers"
    Public Sub InitializeControl(idEvent As Long, idEventItem As Long, lessonNumber As Integer, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action, description, dialogclass)
        CurrentPresenter.InitView(idEvent, idEventItem, lessonNumber, action, identifier)
    End Sub
    Public Sub InitializeControl(idEvent As Long, idEventItem As Long, lessonNumber As Integer, action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, Optional description As String = "", Optional dialogclass As String = "") Implements IViewAddAttachment.InitializeControl
        BaseInitializeControl(action, description, dialogclass)
        CurrentPresenter.InitView(idEvent, idEventItem, lessonNumber, action, identifier, rPermissions)
    End Sub
    Private Sub BaseInitializeControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, Optional description As String = "", Optional dialogclass As String = "")
        DVhiddenRepository.Visible = False
        DVhiddenItem.Visible = False
        Select Case action
            Case Repository.RepositoryAttachmentUploadActions.addurltomoduleitem,
                 Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                DVhiddenItem.Visible = True
            Case Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity,
                 Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity
                DVhiddenItem.Visible = True
                DVhiddenRepository.Visible = True
            Case Repository.RepositoryAttachmentUploadActions.linkfromcommunity
                DVhiddenItem.Visible = True
        End Select
        CBXhideItemFiles.Checked = False
        CBXhideRepositoryFiles.Checked = False
        DVeventItemInfo.Visible = False
        BTNaddAttachment.OnClientClick = ""
        If Not String.IsNullOrEmpty(dialogclass) Then
            UploaderCssClass = dialogclass
        End If
        If String.IsNullOrEmpty(description) Then
            DVdescription.Visible = False
        Else
            LTdescription.Text = description
        End If
    End Sub
#End Region

    Private Function UploadFiles(moduleCode As String, idObjectType As Integer, idAction As Integer, addToRepository As Boolean) As List(Of dtoModuleUploadedItem) Implements IViewAddAttachment.UploadFiles
        If addToRepository Then
            Return CTRLrepositoryItemsUploader.AddFilesToRepository(New EventItemFile(), 0, idObjectType, moduleCode, idAction)
        Else
            Return CTRLinternalUploader.AddModuleInternalFiles(New EventItemFile(), 0, idObjectType, moduleCode, idAction)
        End If
    End Function

    Private Sub InitializeUploaderControl(action As lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddAttachment.InitializeUploaderControl
        Select Case action
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CTRLinternalUploader.Visible = True
                CTRLinternalUploader.InitializeControl(PageUtility.CurrentContext.UserContext.CurrentUserID, identifier)
        End Select
    End Sub
    Private Sub InitializeCommunityUploader(identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier) Implements IViewAddAttachment.InitializeCommunityUploader
        CTRLrepositoryItemsUploader.Visible = True
        CTRLrepositoryItemsUploader.InitializeControl(0, identifier)
    End Sub
    Private Sub InitializeLinkRepositoryItems(idUser As Integer, rPermissions As lm.Comol.Core.FileRepository.Domain.ModuleRepository, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, alreadyLinkedFiles As List(Of RepositoryItemLinkBase(Of Long))) Implements IViewAddAttachment.InitializeLinkRepositoryItems
        CTRLlinkItems.Visible = True
        CTRLlinkItems.InitializeControl(idUser, identifier, alreadyLinkedFiles, False, False, rPermissions.Administration, rPermissions.Administration)
    End Sub

#Region "Display Messages"
    Private Sub DisplayWorkingSessionExpired() Implements IViewAddAttachment.DisplayWorkingSessionExpired
        BTNaddAttachment.Enabled = False
        If RaiseCommandEvents Then
            RaiseEvent WorkingSessionExpired()
        End If
    End Sub
    Private Sub DisplayItemsAdded() Implements IViewAddAttachment.DisplayItemsAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayItemsNotAdded() Implements IViewAddAttachment.DisplayItemsNotAdded
        If RaiseCommandEvents Then
            RaiseEvent ItemsNotAdded(CurrentAction)
        End If
    End Sub
    Private Sub DisplayProjectNotFound() Implements IViewAddAttachment.DisplayEventItemNotFound
        If RaiseCommandEvents Then
            RaiseEvent EventItemNotFound()
        End If
    End Sub
    Private Sub DisplayNoFilesToAdd() Implements IViewAddAttachment.DisplayNoFilesToAdd
        If RaiseCommandEvents Then
            RaiseEvent NoFilesToAdd(CurrentAction)
        End If
    End Sub
    Private Sub DisplayEventItemInfo(title As String, startOn As Date, endOn As Date, showDateInfo As Boolean, minutesDuration As Integer, Optional lessonNumber As Integer = 0) Implements IViewAddAttachment.DisplayEventItemInfo
        DVeventItemInfo.Visible = True
        LBtitle.Text = title
        If showDateInfo Then
            DVtime.Visible = True
            If String.IsNullOrEmpty(title) Then
                LBtitle.Text = String.Format(Resource.getValue("LBlez.text"), lessonNumber)
            Else
                LBtitle.Text &= " (" & String.Format(Resource.getValue("LBlez.text"), lessonNumber) & ")"
            End If
            If startOn.Day = endOn.Day AndAlso startOn.Month = endOn.Month AndAlso startOn.Year = endOn.Year Then
                LBitemTime.Text = startOn.ToString("ddd dd MMMM yyyy", Resource.CultureInfo.DateTimeFormat)
                LBitemTime.Text &= " " & FormatDateTime(startOn, DateFormat.ShortTime)
                LBitemTime.Text &= " - " & FormatDateTime(endOn, DateFormat.ShortTime)
            Else
                LBitemTime.Text = startOn.ToString("ddd dd MMMM yyyy", Resource.CultureInfo.DateTimeFormat)
                LBitemTime.Text &= " " & startOn.ToString("ddd dd MMMM yyyy", Resource.CultureInfo.DateTimeFormat)
            End If
            If minutesDuration > 0 Then
                LBitemTime.Text &= " - " & Resource.getValue("duration") & " " & GetDurationString(minutesDuration)
            End If
            'If Not IsNothing(oLabel) Then
            '    If oItem.ShowDateInfo Then
            '        If oItem.StartDate.Day = oItem.EndDate.Day AndAlso oItem.StartDate.Month = oItem.EndDate.Month AndAlso oItem.StartDate.Year = oItem.EndDate.Year Then
            '            oLabel.Text = oItem.StartDate.Date.ToString("ddd dd MMMM yyyy", Resource.CultureInfo.DateTimeFormat)
            '            oLabel.Text &= " " & FormatDateTime(oItem.StartDate, DateFormat.ShortTime)
            '            oLabel.Text &= " - " & FormatDateTime(oItem.EndDate, DateFormat.ShortTime)
            '        Else
            '            oLabel.Text = oItem.StartDate.Date.ToString("dddd dd MMMM", Resource.CultureInfo.DateTimeFormat)
            '            oLabel.Text &= " " & oItem.EndDate.Date.ToString("dddd dd MMMM", Resource.CultureInfo.DateTimeFormat)
            '        End If
            '        If oItem.MinutesDuration > 0 Then
            '            oLabel.ToolTip = Resource.getValue("duration") & " " & Me.GetDurationString(oItem.MinutesDuration)
            '        End If
            '    ElseIf oItem.MinutesDuration > 0 Then
            '        oLabel.Text = Resource.getValue("duration") & " " & Me.GetDurationString(oItem.MinutesDuration)
            '        oLabel.ToolTip = oLabel.Text
            '    End If
            'End If
        Else
            DVtime.Visible = False
            DVtitle.Visible = Not String.IsNullOrWhiteSpace(LBtitle.Text)
        End If
    End Sub
#End Region

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idEventItem As Long, action As ModuleCommunityDiary.ActionType) Implements IViewAddAttachment.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCommunityDiary.ObjectType.DiaryItem, idEventItem.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Internal"
    Private Sub CTRLinternalUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLinternalUploader.IsValidOperation
        isvalid = True
    End Sub

    Private Sub CTRLrepositoryItemsUploader_AllowUploadUpdate(allowUpload As Boolean) Handles CTRLrepositoryItemsUploader.AllowUploadUpdate
        BTNaddAttachment.Enabled = allowUpload
    End Sub
    Private Sub CTRLrepositoryItemsUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLrepositoryItemsUploader.IsValidOperation
        isvalid = True
    End Sub
    Private Sub BTNaddAttachment_Click(sender As Object, e As System.EventArgs) Handles BTNaddAttachment.Click
        Select Case CurrentAction
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.linkfromcommunity
                CurrentPresenter.AddCommunityFilesToItem(IdEvent, IdEventItem, IdEventCommunity, CTRLlinkItems.GetNewRepositoryItemLinks(), Not CBXhideItemFiles.Checked)
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitem
                CurrentPresenter.AddFilesToItem(IdEvent, IdEventItem, IdEventCommunity, Not CBXhideItemFiles.Checked)
            Case lm.Comol.Core.DomainModel.Repository.RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity
                CurrentPresenter.AddCommunityFilesToItem(IdEvent, IdEventItem, IdEventCommunity, Not CBXhideItemFiles.Checked, Not CBXhideRepositoryFiles.Checked)
        End Select
    End Sub
    Private Sub LNBcloseAttachmentWindow_Click(sender As Object, e As System.EventArgs) Handles LNBcloseAttachmentWindow.Click
        If RaiseCommandEvents Then

        End If
    End Sub

    Private Function GetDurationString(ByVal minutesDuration As Integer) As String
        Dim durationDate As New TimeSpan(0, minutesDuration, 0)
        Dim TotalHours As Integer
        If durationDate.TotalHours > Integer.MaxValue Then
            TotalHours = Integer.MaxValue
        Else
            TotalHours = Convert.ToInt32(durationDate.TotalHours)
        End If
        Dim Hours As Integer = durationDate.Hours
        Dim Minutes As Integer = durationDate.Minutes

        If TotalHours = 0 AndAlso Minutes > 0 Then
            Return durationDate.Minutes.ToString & " " & Resource.getValue("durationMinutes")
        ElseIf TotalHours = Hours Then
            Return durationDate.Hours.ToString & IIf(Hours = 1, Resource.getValue("durationHour"), Resource.getValue("durationHours")) & IIf(Minutes = 0, ".", " " & durationDate.Minutes.ToString & Resource.getValue("durationMinutes"))
        ElseIf Not TotalHours.Equals(0) Then
            Return durationDate.TotalHours.ToString & IIf(TotalHours = 1, Resource.getValue("durationHour"), Resource.getValue("durationHours")) & IIf(Minutes = 0, ".", "  " & durationDate.Minutes.ToString & Resource.getValue("durationMinutes"))
        End If
        Return ""
    End Function
#End Region


  
End Class