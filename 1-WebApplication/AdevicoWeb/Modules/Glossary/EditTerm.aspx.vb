Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class EditTerm
    Inherits GLpageBase
    Implements IViewTermEdit

#Region "Context"

    Private _Presenter As TermEditPresenter

    Private ReadOnly Property CurrentPresenter() As TermEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TermEditPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property


    Public Property ItemData As DTO_Term Implements IViewTermEdit.ItemData
        Get
            Dim data As DTO_Term = New DTO_Term()
            data.Id = IdTerm
            data.Name = Me.TXBname.Text
            data.Description = Me.CTRLeditorText.HTML
            data.DescriptionText = Me.CTRLeditorText.Text
            data.IdGlossary = IdGlossary
            data.IsPublished = SWHpublish.Status
            Return data
        End Get
        Set(value As DTO_Term)
            TXBname.Text = value.Name
            CTRLeditorText.HTML = value.Description
            SWHpublish.Status = value.IsPublished
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ShowInfo(SaveStateEnum.None, MessageType.none)
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False

        CTRLeditorText.InitializeControl(ModuleGlossaryNew.UniqueCode)
        SWHpublish.Enabled = True
        SWHpublish.SetText(Resource, True, True)
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewTermEdit.SetTitle
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
        LNBsave.Visible = False
        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, IdCommunity, True)
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBtermName_t)
            .setLabel(LBtermDefinition_t)
            .setLabel(LBtermStatus_t)
            .setHyperLink(HYPback, False, True)
            .setLinkButton(LNBsave, False, True)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal idGlossary As Long, ByVal term As DTO_Term, ByVal fromType As Int32, ByVal pageIndex As Int32, ByVal loadCookies As Boolean, ByVal idCookies As String) Implements IViewTermEdit.LoadViewData

        If fromType = 1 Then
            HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryViewPage(idGlossary, idCommunity, False, False, loadCookies, idCookies, pageIndex)
        ElseIf fromType = 2 Then
            HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(idGlossary, idCommunity, True)
        ElseIf fromType = 3 Then
            HYPback.NavigateUrl = ApplicationUrlBase & RootObject.TermView(idGlossary, term.Id, idCommunity, False)
        End If

        ItemData = term

        Dim titleString = String.Format("{0} - {1}", Resource.getValue("EditTerm"), term.Name)
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
    End Sub

    Public Sub GoToGlossaryView() Implements IViewTermEdit.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, IdCommunity, FromViewMap))
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBsave.Click
        Dim value = ItemData
        If (CurrentPresenter.ValidateFields(value)) Then
            CurrentPresenter.SaveOrUpdate(value)
        End If
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewTermEdit.ShowErrors
        Dim errors As New StringBuilder()
        For Each key As String In resourceErrorList
            errors.AppendLine(Resource.getValue(key))
        Next
        SetMessage(errors.ToString(), type)
    End Sub

    Public Sub ShowInfo(ByVal saveStateEnum As SaveStateEnum, ByVal type As MessageType) Implements IViewTermEdit.ShowInfo
        SetMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetMessage(ByVal message As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, type)
        End If
    End Sub

#End Region

#Region "Attachment"

    'Public Property UploaderCssClass As String
    '    Get
    '        Return ViewStateOrDefault("UploaderCssClass", "")
    '    End Get
    '    Set(value As String)
    '        ViewState("UploaderCssClass") = value
    '    End Set
    'End Property

    'Private Sub BaseInitializeControlAttachment(action As RepositoryAttachmentUploadActions, Optional description As String = "", Optional dialogclass As String = "")
    '    BTNaddAttachment.OnClientClick = ""
    '    If Not String.IsNullOrEmpty(dialogclass) Then
    '        UploaderCssClass = dialogclass
    '    End If
    '    If String.IsNullOrEmpty(description) Then
    '        DVdescription.Visible = False
    '    Else
    '        LTdescription.Text = description
    '    End If
    '    Me.CTRLcommands.Visible = True
    '    'Temp
    '    Dim actions = New List(Of RepositoryAttachmentUploadActions)()
    '    actions.add(RepositoryAttachmentUploadActions.addurltomoduleitem)
    '    actions.add(RepositoryAttachmentUploadActions.addurltomoduleitemandcommunity)
    '    actions.add(RepositoryAttachmentUploadActions.linkfromcommunity)
    '    actions.add(RepositoryAttachmentUploadActions.uploadtomoduleitem)
    '    actions.add(RepositoryAttachmentUploadActions.uploadtomoduleitemandcommunity)


    '    Me.CTRLcommands.InitializeControlForJQuery(actions, RepositoryAttachmentUploadActions.uploadtomoduleitem) 'action)
    'End Sub

    'Private Sub InitializeLinkRepositoryItems(rPermissions As CoreModuleRepository, idCommunity As Integer, alreadyLinkedFiles As List(Of iCoreItemFileLink(Of Long))) Implements IViewTermEdit.InitializeLinkRepositoryItems
    '    CTRLlinkItems.Visible = True
    '    CTRLlinkItems.InitializeControl(idCommunity, alreadyLinkedFiles, False, False, rPermissions.Administration, rPermissions.Administration)
    'End Sub

    'Private Sub InitializeCommunityUploader(rPermissions As CoreModuleRepository, idCommunity As Integer) Implements IViewTermEdit.InitializeCommunityUploader
    '    CTRLrepositoryUploader.Visible = True
    '    CTRLrepositoryUploader.InitializeControl(0, idCommunity, rPermissions)
    '    BTNaddAttachment.OnClientClick = "ProgressStart();"
    'End Sub

    'Private Sub InitializeUploaderControl(action As RepositoryAttachmentUploadActions, Optional idCommunity As Integer = - 1) Implements IViewTermEdit.InitializeUploaderControl
    '    Select Case action
    '        Case RepositoryAttachmentUploadActions.addurltomoduleitem
    '            CTRLurls.Visible = True
    '            CTRLurls.InitializeControl()
    '        Case RepositoryAttachmentUploadActions.uploadtomoduleitem
    '            CTRLinternalUploader.Visible = True
    '            CTRLinternalUploader.InitializeControl(idCommunity)
    '            BTNaddAttachment.OnClientClick = "ProgressStart();"
    '    End Select
    'End Sub

#End Region
End Class