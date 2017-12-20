Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview


Public Class AddTerm
    Inherits GLpageBase
    Implements IViewTermAdd

#Region "Context"

    Private _Presenter As TermAddPresenter

    Private ReadOnly Property CurrentPresenter() As TermAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TermAddPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property ItemData As DTO_Term Implements IViewTermAdd.ItemData
        Get
            Dim data As DTO_Term = New DTO_Term()
            data.Id = IdTerm
            data.Name = TXBname.Text
            data.Description = Me.CTRLeditorText.HTML
            data.DescriptionText = Me.CTRLeditorText.Text
            data.IdGlossary = IdGlossary
            data.IsPublished = SWHpublishAdd.Status
            Return data
        End Get
        Set(value As DTO_Term)
            TXBname.Text = value.Name
            CTRLeditorText.HTML = value.Description
            SWHpublishAdd.Status = value.IsPublished
        End Set
    End Property


#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        SetErrorMessage(String.Empty, MessageType.none)
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CTRLeditorText.InitializeControl(ModuleGlossaryNew.UniqueCode)
        SWHpublishAdd.Enabled = True
        SWHpublishAdd.SetText(Resource, True, True)
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewTermAdd.SetTitle
        Dim titleString = Resource.getValue("AddTerm")
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

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal idGlossary As Long) Implements IViewTermAdd.LoadViewData
        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(idGlossary, idCommunity)
    End Sub

    Public Sub GoToGlossaryView() Implements IViewTermAdd.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, IdCommunity, FromViewMap))
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBsave.Click
        Dim value = ItemData
        If (CurrentPresenter.ValidateFields(value)) Then
            CurrentPresenter.SaveOrUpdate(value)
        End If
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewTermAdd.ShowErrors
        Dim errors As New StringBuilder()
        For Each key As String In resourceErrorList
            errors.AppendLine(Resource.getValue(key))
        Next
        SetErrorMessage(errors.ToString(), type)
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewTermAdd.ShowErrors
        SetErrorMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetErrorMessage(ByVal message As String, ByVal type As MessageType)
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
End Class