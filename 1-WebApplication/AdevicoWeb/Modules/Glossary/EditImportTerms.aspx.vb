Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.Wizard
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class EditImportTerms
    Inherits GLpageBase
    Implements IViewGlossaryImportTerms

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

#Region "Context"

    Private _Presenter As GlossaryEditImportTermsPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryEditImportTermsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryEditImportTermsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property ItemData() As DTO_Glossary Implements IViewGlossaryImportTerms.ItemData

#End Region

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBselectCommunities, True, True, False, False)
            .setLinkButton(LNBConfirmImport, True, True, False, True)
            .setLiteral(LTtermImportTitle_t)
            .setLabel(LBglossaryImportDescription_t)
            .setLiteral(LTImportOptions)
            CTRLcommunitySelectorHeader.ModalTitle = Me.Resource.getValue("selectCommunities")
            '<asp:Literal runat="server" ID="LBglossaryImportTitle_t">Glossaries Imports</asp:Literal>
            '       </h4>
            '       <asp:Label runat="server" CssClass="description" ID="LBglossaryImportDescription_t">*click to expand</asp:Label>

        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

    Public Overrides Sub BindDati()
        CurrentPresenter.InitView()
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal steps As List(Of NavigableWizardItem(Of Integer))) Implements IViewGlossaryImportTerms.LoadViewData
        ItemData = glossary
        SetWizardStep(steps)
        Dim titleString = String.Format("{0} - {1}", Resource.getValue("ImportTermsSetting"), glossary.Name)
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
        CTRCommunityGlossaryTerms.BindDati(idCommunity)
        HYPback.NavigateUrl = ApplicationUrlBase & lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(idCommunity)

        RBImportOptions.Items.Clear()

        Dim rbAllPublished = New ListItem(Resource.getValue("AllPublished"), "1")
        rbAllPublished.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllPublished)

        Dim rbAllDraft = New ListItem(Resource.getValue("AllDraft"), "2")
        rbAllDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAllDraft)

        Dim rbAlreadyExistAsDraft = New ListItem(Resource.getValue("DraftIfAlreadyExist"), "3")
        rbAlreadyExistAsDraft.Attributes.Add("class", "option")
        RBImportOptions.Items.Add(rbAlreadyExistAsDraft)

        RBImportOptions.SelectedIndex = 0
    End Sub

    Public Sub SetWizardStep(ByRef steps As List(Of NavigableWizardItem(Of Integer)))

        Dim stepGeneral As NavigableWizardItem(Of Integer) = steps(0)
        stepGeneral.Active = False
        stepGeneral.Name = Resource.getValue("GeneralSetting")
        If ItemData.IsPublished Then
            stepGeneral.Status = WizardItemStatus.valid
            stepGeneral.Message = Resource.getValue("IsPublished")
        Else
            stepGeneral.Status = WizardItemStatus.warning
            stepGeneral.Message = Resource.getValue("NotIsPublished")
        End If
        stepGeneral.Message = Resource.getValue("EditMode")
        stepGeneral.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryEdit(IdGlossary, IdCommunity, False, False)

        Dim stepShare As NavigableWizardItem(Of Integer) = steps(1)
        stepShare.Active = False
        stepShare.Name = Resource.getValue("ShareSetting")
        stepShare.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryEditShare(IdGlossary, IdCommunity, False, False)

        If ItemData.IsShared AndAlso ItemData.IsPublic Then
            stepShare.Message = Resource.getValue("SharedPublic")
            stepShare.Status = WizardItemStatus.valid
        ElseIf ItemData.IsShared Then
            stepShare.Message = Resource.getValue("Shared")
            stepShare.Status = WizardItemStatus.valid
        ElseIf ItemData.IsPublic Then
            stepShare.Message = Resource.getValue("Public")
            stepShare.Status = WizardItemStatus.valid
        Else
            Dim pendingShareCount As Int32 = CurrentPresenter.HasPendingShare(IdGlossary, IdCommunity)
            If pendingShareCount > 0 Then
                stepShare.Message = String.Format(Resource.getValue("HasPendingShare"), pendingShareCount)
                stepShare.Status = WizardItemStatus.warning
            Else
                stepShare.Message = Resource.getValue("NotShared")
                stepShare.Status = WizardItemStatus.none
            End If
        End If

        Dim stepImportTerms As NavigableWizardItem(Of Integer) = steps(2)
        stepImportTerms.Active = True
        stepImportTerms.Name = Resource.getValue("ImportTermsSetting")
        stepImportTerms.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryImportTerms(IdCommunity, IdGlossary)
        stepImportTerms.Status = WizardItemStatus.none

        CTRLsteps.InitializeControl(steps)
    End Sub

    Private Sub LNBConfirmImport_Click(sender As Object, e As EventArgs) Handles LNBConfirmImport.Click
        Dim listId As New List(Of Long)
        For Each itemCommunity As RepeaterItem In CTRCommunityGlossaryTerms.GetRPTCommunites().Items
            Dim UCglossaryTerms As UC_GlossaryImportTerms = itemCommunity.FindControl("UCglossaryTerms")
            If Not IsNothing(UCglossaryTerms) Then
                listId.AddRange(UCglossaryTerms.GetSelectedTermIds(true))
            End If
        Next
        If listId.Count > 0 Then
            CurrentPresenter.ImportTerms(listId, RBImportOptions.SelectedIndex)
        End If
    End Sub

    Public Sub ShowInfo(ByVal saveStateEnum As SaveStateEnum, ByVal type As MessageType) Implements IViewGlossaryImportTerms.ShowInfo
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

#Region "Community Selector"

    Private Sub CTRLglossaryShare_AddCommunityClicked() Handles LNBselectCommunities.Click
        CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    End Sub

    Public Sub DisplayCommunityToAdd(forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability) Implements IViewGlossaryImportTerms.DisplayCommunityToAdd
        CTRLcommunity.Visible = True
        Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
        If forAdministration Then
            CTRLcommunity.InitializeAdministrationControl(PageUtility.CurrentUser.ID, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLcommunity.InitializeControlByModules(PageUtility.CurrentUser.ID, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub

    Public Sub ShowCommunity(ByVal idCommunities As List(Of Integer)) Implements IViewGlossaryImportTerms.ShowCommunity
        CTRCommunityGlossaryTerms.LoadViewData(idCommunities)
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddNewCommunity(idCommunities)

        LNBConfirmImport.Visible = idCommunities.Count > 0
    End Sub


#End Region
End Class