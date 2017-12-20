Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class Import
    Inherits GLpageBase
    Implements IViewImport

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

#Region "Context"

    Private _Presenter As ImportPresenter

    Private ReadOnly Property CurrentPresenter() As ImportPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ImportPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBselectCommunities, True, True, False, False)
            .setLinkButton(LNBConfirmImportGlossary, True, True, False, True)
            .setHyperLink(HYPback, False, False, False, False)
            .setLiteral(LTglossaryImports_s)
            .setLiteral(LTImportGlossaryOptions)

        End With
        CTRLcommunitySelectorHeader.ModalTitle = Me.Resource.getValue("selectCommunities")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

    Public Overrides Sub BindDati()
        HYPback.NavigateUrl = PageUtility.BaseUrl & lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(IdCommunity)

        CurrentPresenter.InitView()
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal communityName As String) Implements IViewImport.LoadViewData
        Dim titleString = Resource.getValue("LTglossaryImports_s.text")
        ' Master.ServiceTitle = titleString 'Testo ad inizio contenuti
        ' Master.ServiceTitleToolTip = titleString 'Tooltip ad inizio contenuti
        Master.DisplayTitleRow = False
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString ' Testo tab
        LBglossaryImportsDescription_s.Text = String.Format(Resource.getValue("LBglossaryImportsDescription_s"), communityName)

        CTRCommunityGlossaryTerms.BindDati(idCommunity)

        RBImportOptions.Items.Clear()

        Dim rbAllPublished = New ListItem(Resource.getValue("AllPublic"), "1")
        rbAllPublished.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllPublished)

        Dim rbAllDraft = New ListItem(Resource.getValue("AllNotPublic"), "2")
        rbAllDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllDraft)

        Dim rbAlreadyExistAsDraft = New ListItem(Resource.getValue("NotPublicIfAlreadyExist"), "3")
        rbAlreadyExistAsDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAlreadyExistAsDraft)

        RBImportOptions.SelectedIndex = 0
    End Sub

#Region "Community Selector"

    Private Sub CTRLglossaryShare_AddCommunityClicked() Handles LNBselectCommunities.Click
        CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    End Sub

    Public Sub DisplayCommunityToAdd(forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability) Implements IViewImport.DisplayCommunityToAdd
        CTRLcommunity.Visible = True
        Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
        If forAdministration Then
            CTRLcommunity.InitializeAdministrationControl(PageUtility.CurrentUser.ID, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLcommunity.InitializeControlByModules(PageUtility.CurrentUser.ID, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub

    Public Sub ShowCommunity(ByVal idCommunities As List(Of Integer)) Implements IViewImport.ShowCommunity
        CTRLmessagesInfo.Visible = False
        CTRCommunityGlossaryTerms.LoadViewData(idCommunities)
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
        CTRLmessagesInfo.Visible = False
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddNewCommunity(idCommunities)
        LNBConfirmImportGlossary.Visible = idCommunities.Count > 0
    End Sub

    Private Sub LNBConfirmImport_Click(sender As Object, e As EventArgs) Handles LNBConfirmImportGlossary.Click
        Dim listId As List(Of Long) = CTRCommunityGlossaryTerms.GetSelectedTermIds()

        If listId.Count > 0 Then
            If CurrentPresenter.ImportGlossaries(listId, RBImportOptions.SelectedIndex) Then
                ShowInfo(SaveStateEnum.Saved, MessageType.success)
            Else
                ShowInfo(SaveStateEnum.NotSaved, MessageType.error)
            End If
        Else
            ShowInfo(SaveStateEnum.NoRecord, MessageType.alert)
        End If
    End Sub

    Public Sub ShowInfo(ByVal saveStateEnum As SaveStateEnum, ByVal type As MessageType) Implements IViewImport.ShowInfo
        SetMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetMessage(ByVal message As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
            'DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            'DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, type)
        End If
    End Sub
#End Region
End Class