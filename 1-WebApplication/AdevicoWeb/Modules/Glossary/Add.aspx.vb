Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.DomainModel.Languages
Imports lm.Comol.Core.Wizard
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class AddGlossary
    Inherits GLpageBase
    Implements IViewGlossaryAdd

#Region "Context"

    Private _Presenter As GlossaryAddPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryAddPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property ItemData As DTO_Glossary Implements IViewGlossaryAdd.ItemData
        Get
            Dim Data As DTO_Glossary = New DTO_Glossary()
            Data.IdCommunity = IdCommunity
            Return Data
        End Get
        Set(value As DTO_Glossary)
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        SetErrorMessage(String.Empty, MessageType.none)
    End Sub

    Public Overrides Sub BindDati()
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryAdd.SetTitle

        Dim titleString = Resource.getValue("AddGlossary")
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString

        LTpageTitle_t.Text = titleString
    End Sub


    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            '.setHyperLink(HYPback, False, True)
            '.setLinkButton(LKBsave, False, True)
            '.setLabel(LBname_t)
            '.setLabel(LBdescription_t)
            '.setLabel(LBisDefault_t)
            '.setLabel(LBlanguage_t)
            '.setLabel(LBdisplayOrder)
            '.setLabel(LBtermsArePaged)
            '.setLabel(LBtermsPerPage)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal languages As List(Of BaseLanguageItem), ByVal steps As List(Of NavigableWizardItem(Of Integer)), ByVal fromListGlossary As Boolean) Implements IViewGlossaryAdd.LoadViewData

        Dim oControl As UC_GenericWizardSteps = CTRLsteps
        If Not IsNothing(oControl) Then
            oControl.Visible = True
            SetWizardStep(steps)
            oControl.InitializeControl(steps)
        End If

        CTRLglossarySettings.BindDati(idCommunity, - 1, False)
    End Sub

    Public Sub SetWizardStep(ByRef steps As List(Of NavigableWizardItem(Of Integer)))

        Dim stepGeneral As NavigableWizardItem(Of Integer) = steps(0)
        stepGeneral.Name = Resource.getValue("GeneralSetting")
        stepGeneral.Status = WizardItemStatus.none
        stepGeneral.Message = Resource.getValue("New")

        Dim stepShare As NavigableWizardItem(Of Integer) = steps(1)
        stepShare.Name = Resource.getValue("ShareSetting")
        stepShare.Status = WizardItemStatus.none
        stepShare.Message = Resource.getValue("NotShared")

        Dim stepImportTerms As NavigableWizardItem(Of Integer) = steps(2)
        stepImportTerms.Name = Resource.getValue("ImportTermsSetting")
        stepImportTerms.Status = WizardItemStatus.none
        stepShare.Message = Resource.getValue("NotShared")

        CTRLsteps.InitializeControl(steps)
    End Sub

    Public Sub GoToGlossaryList() Implements IViewGlossaryAdd.GoToGlossaryList
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryList(IdCommunity, IdGlossary))
    End Sub

    Public Sub GoToGlossaryView() Implements IViewGlossaryAdd.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, IdCommunity))
    End Sub


    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewGlossaryAdd.ShowErrors
        Dim errors As New StringBuilder()
        For Each key As String In resourceErrorList
            errors.AppendLine(Resource.getValue(key))
        Next
        SetErrorMessage(errors.ToString(), type)
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewGlossaryAdd.ShowErrors
        SetErrorMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetErrorMessage(ByVal message As String, ByVal type As MessageType)
        'If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
        '    DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
        '    CTRLmessagesInfo.Visible = False
        'Else
        '    DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
        '    CTRLmessagesInfo.Visible = True
        '    CTRLmessagesInfo.InitializeControl(message, type)
        'End If
    End Sub

#End Region
End Class