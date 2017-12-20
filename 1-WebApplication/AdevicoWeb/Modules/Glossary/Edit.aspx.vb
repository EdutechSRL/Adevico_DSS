Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.Wizard
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class EditGlossary
    Inherits GLpageBase
    Implements IViewGlossaryEdit

#Region "Context"

    Private _Presenter As GlossaryEditPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryEditPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryEditPresenter(PageUtility.CurrentContext, Me)
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

    Public Property ItemData() As DTO_Glossary Implements IViewGlossaryEdit.ItemData

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryEdit.SetTitle
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal languages As List(Of BaseLanguageItem), ByVal steps As List(Of NavigableWizardItem(Of Integer)), ByVal fromListGlossary As Boolean, ByVal showSaveNew As Boolean) Implements IViewGlossaryEdit.LoadViewData
        ItemData = glossary

        If Not IsNothing(CTRLsteps) Then
            CTRLsteps.Visible = True
            SetWizardStep(steps)
        End If
        CTRLglossarySettings.BindDati(idCommunity, glossary.Id, showSaveNew)

        Dim titleString = String.Format("{0} - {1}", Resource.getValue("EditGlossary"), glossary.Name)
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
    End Sub

    Public Sub SetWizardStep(ByRef steps As List(Of NavigableWizardItem(Of Integer)))

        Dim stepGeneral As NavigableWizardItem(Of Integer) = steps(0)
        stepGeneral.Active = True
        stepGeneral.Name = Resource.getValue("GeneralSetting")
        If ItemData.IsPublished Then
            stepGeneral.Status = WizardItemStatus.valid
            stepGeneral.Message = Resource.getValue("IsPublished")
        Else
            stepGeneral.Status = WizardItemStatus.warning
            stepGeneral.Message = Resource.getValue("NotIsPublished")
        End If

        stepGeneral.Url = RootObject.GlossaryEdit(IdGlossary, IdCommunity, False, False)

        Dim stepShare As NavigableWizardItem(Of Integer) = steps(1)
        stepShare.Active = False
        stepShare.Name = Resource.getValue("ShareSetting")
        stepShare.Url = RootObject.GlossaryEditShare(IdGlossary, IdCommunity, False, False)

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
        stepImportTerms.Url = RootObject.GlossaryImportTerms(IdCommunity, IdGlossary)
        stepImportTerms.Status = WizardItemStatus.none


        CTRLsteps.InitializeControl(steps)
    End Sub

    Public Sub GoToGlossaryList() Implements IViewGlossaryEdit.GoToGlossaryList
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryList(IdCommunity, IdGlossary))
    End Sub

    Public Sub GoToGlossaryView() Implements IViewGlossaryEdit.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, IdCommunity))
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewGlossaryEdit.ShowErrors
        Throw New NotImplementedException()
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewGlossaryEdit.ShowErrors
        Throw New NotImplementedException()
    End Sub

#End Region
End Class
