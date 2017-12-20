Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class ViewTerm
    Inherits GLpageBase
    Implements IViewTermView

#Region "Context"

    Private _Presenter As TermViewPresenter

    Private ReadOnly Property CurrentPresenter() As TermViewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New TermViewPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property


    Public Property IdTerm As Int64
        Get
            Return ViewStateOrDefault("IdTerm", 0)
        End Get
        Set(value As Int64)
            Me.ViewState("IdTerm") = value
        End Set
    End Property

    Public Property HasAttachements As Boolean
        Get
            Return ViewStateOrDefault("HasAttachements", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("HasAttachements") = value
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewTermView.SetTitle
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPback, False, False)
            .setHyperLink(HYPglossaryList, False, False)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal term As Term, ByVal fromViewMap As Boolean) Implements IViewTermView.LoadViewData
        'Init Title
        'Dim item As Term = CurrentPresenter.GetGlossaryItem(IdTerm)
        If term IsNot Nothing Then
            LTterm.Text = term.Name
            LTdefinition.Text = term.Description
        End If
        IdTerm = term.Id
        'Init Buttons
        HYPback.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryView(IdGlossary, idCommunity, fromViewMap)
        HYPglossaryList.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryList(idCommunity, IdGlossary)

        If Not term.ModifiedBy Is Nothing Then
            LTlastUpdate.Text = term.ModifiedOn.ToShortDateString()
            LTAuthor.Text = term.ModifiedBy.SurnameAndName
        Else
            LTlastUpdate.Text = term.CreatedOn.ToShortDateString()
            LTAuthor.Text = term.CreatedBy.SurnameAndName
        End If

        SWHpublish.Enabled = True
        SWHpublish.SetText(Resource, True, True)

        HYPedit.NavigateUrl = ApplicationUrlBase & RootObject.TermEdit(IdGlossary, IdTerm, idCommunity, 3)
        SWHpublish.Enabled = glossary.Permission.EditTerm
        HYPedit.Visible = glossary.Permission.EditTerm
        LNBdelete.Visible = glossary.Permission.DeleteTerm

        Master.ServiceTitle = term.Name
        Master.ServiceTitleToolTip = term.Name
        Master.ServiceNopermission = term.Name
        LTpageTitle_t.Text = term.Name
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBdelete.Click
        CurrentPresenter.VirtualDelete(IdTerm)
    End Sub

#End Region

    Private Sub SWHpublish_StatusChange(Status As Boolean) Handles SWHpublish.StatusChange
        CurrentPresenter.ChangePublishState(IdTerm, Status)
    End Sub

    Protected Function HasAttachments() As String
        If (HasAttachements) Then
            Return "attachments"
        Else
            Return "attachments noattachments"
        End If
    End Function
End Class