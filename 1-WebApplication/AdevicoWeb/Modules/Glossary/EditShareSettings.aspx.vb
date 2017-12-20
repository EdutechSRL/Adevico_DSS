Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.DomainModel.Filters
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.Wizard
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class EditShareSettings
    Inherits GLpageBase
    Implements IViewGlossaryEditShare

#Region "Context"

    Private _Presenter As GlossaryEditSharePresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryEditSharePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryEditSharePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        CurrentPresenter.InitView(False)
    End Sub

    Public Property ItemData() As DTO_Glossary Implements IViewGlossaryEditShare.ItemData

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryEditShare.SetTitle
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        CTRLcommunitySelectorHeader.ModalTitle = Me.Resource.getValue("selectCommunities")
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"


    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal languages As List(Of BaseLanguageItem), ByVal steps As List(Of NavigableWizardItem(Of Integer)), ByVal fromListGlossary As Boolean) Implements IViewGlossaryEditShare.LoadViewData
        ItemData = glossary

        If Not IsNothing(CTRLsteps) Then
            CTRLsteps.Visible = True
            SetWizardStep(steps)
        End If
        CTRLglossaryShare.BindDati(idCommunity, glossary.Id)

        Dim titleString = String.Format("{0} - {1}", Resource.getValue("EditGlossaryShare"), glossary.Name)
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
    End Sub

#Region "Community Selector"

    Private Sub CTRLglossaryShare_AddCommunityClicked() Handles CTRLglossaryShare.AddCommunityClicked
        CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID)
    End Sub

    Public Sub DisplayCommunityToAdd(forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability) Implements IViewGlossaryEditShare.DisplayCommunityToAdd
        CTRLcommunity.Visible = True
        Master.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
        If forAdministration Then
            CTRLcommunity.InitializeAdministrationControl(PageUtility.CurrentUser.ID, unloadIdCommunities, availability, New List(Of Integer))
        Else
            CTRLcommunity.InitializeControlByModules(PageUtility.CurrentUser.ID, requiredPermissions, unloadIdCommunities, availability)
        End If
    End Sub

    Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
        CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub

    Private Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
        Master.SetOpenDialogOnPostbackByCssClass("")
        CurrentPresenter.AddNewCommunity(idCommunities)
    End Sub


#End Region

    Public Sub SetWizardStep(ByRef steps As List(Of NavigableWizardItem(Of Integer)))

        'Dim stepGeneral As NavigableWizardItem(Of Integer) = steps(0)
        'stepGeneral.Active = False
        'stepGeneral.Name = Resource.getValue("GeneralSetting")
        'stepGeneral.Status = WizardItemStatus.valid
        'stepGeneral.Message = Resource.getValue("NotEdit")
        'stepGeneral.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryEdit(IdGlossary, IdCommunity, False, False)

        'Dim stepShare As NavigableWizardItem(Of Integer) = steps(1)
        'stepShare.Active = True
        'stepShare.Name = Resource.getValue("ShareSetting")
        'stepShare.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryEditShare(IdGlossary, IdCommunity, False, False)
        'stepShare.Message = Resource.getValue("NotEdit")

        'Dim stepImportTerms As NavigableWizardItem(Of Integer) = steps(2)
        'stepImportTerms.Active = False
        'stepImportTerms.Name = Resource.getValue("ImportTermsSetting")
        'stepImportTerms.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryImportTerms(IdCommunity, IdGlossary)
        'stepImportTerms.Status = WizardItemStatus.none

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
        stepShare.Active = True
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
        stepImportTerms.Active = False
        stepImportTerms.Name = Resource.getValue("ImportTermsSetting")
        stepImportTerms.Url = lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryImportTerms(IdCommunity, IdGlossary)
        stepImportTerms.Status = WizardItemStatus.none

        CTRLsteps.InitializeControl(steps)
    End Sub

    Public Sub GoToGlossaryList() Implements IViewGlossaryEditShare.GoToGlossaryList
        Response.Redirect(ApplicationUrlBase & lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryList(IdCommunity, IdGlossary))
    End Sub

    Public Sub GoToGlossaryView() Implements IViewGlossaryEditShare.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & lm.Comol.Modules.Standard.GlossaryNew.Domain.RootObject.GlossaryView(IdGlossary, IdCommunity))
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewGlossaryEditShare.ShowErrors
        Throw New NotImplementedException()
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewGlossaryEditShare.ShowErrors
        Throw New NotImplementedException()
    End Sub

#End Region
End Class
