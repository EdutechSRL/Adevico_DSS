Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class RecycleTerms
    Inherits GLpageBase
    Implements IViewRecycleBinTerms

#Region "Context"

    Private Const termRecoverCommand As String = "termRecoverCommand"
    Private Const termDeleteCommand As String = "termDeleteCommand"

    Private _Presenter As RecycleBinTermsPresenter

    Private ReadOnly Property CurrentPresenter() As RecycleBinTermsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RecycleBinTermsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        HYPback.NavigateUrl = String.Format("{0}{1}", ApplicationUrlBase, RootObject.GlossaryView(PreloadIdGlossary, PreloadIdCommunity, PreloadFromViewMap))
        CurrentPresenter.InitView()
    End Sub

    Private Sub SetTitle(name As String) Implements IViewRecycleBinTerms.SetTitle
        Dim titleString = Resource.getValue("RecycleBinTerm")
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
            Master.ServiceTitle = .getValue("Glossary.ServiceTitle.Recycle")
            Master.ServiceTitleToolTip = .getValue("Glossary.ServiceTitle.Recycle.ToolTip")
            Master.ServiceNopermission = .getValue("Glossary.ServiceTitle.Recycle.NoPermission")
            .setHyperLink(HYPback, False, False, False, False)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal terms As List(Of DTO_TermDelete)) Implements IViewRecycleBinTerms.LoadViewData
        RPTlist.DataSource = terms
        RPTlist.DataBind()
    End Sub

    Private Sub RPTlist_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTlist.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim LTname_t As Literal = e.Item.FindControl("LTname_t")
            If Not IsNothing(LTname_t) Then
                Resource.setLiteral(LTname_t)
            End If

            Dim LTdeleteOn_t As Literal = e.Item.FindControl("LTdeleteOn_t")
            If Not IsNothing(LTdeleteOn_t) Then
                Resource.setLiteral(LTdeleteOn_t)
            End If

            Dim LTdeleteFrom_t As Literal = e.Item.FindControl("LTdeleteFrom_t")
            If Not IsNothing(LTdeleteFrom_t) Then
                Resource.setLiteral(LTdeleteFrom_t)
            End If

            Dim LTstats_t As Literal = e.Item.FindControl("LTstats_t")
            If Not IsNothing(LTstats_t) Then
                Resource.setLiteral(LTstats_t)
            End If

            Dim LTtype_t As Literal = e.Item.FindControl("LTtype_t")
            If Not IsNothing(LTtype_t) Then
                Resource.setLiteral(LTtype_t)
            End If
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim myobj As DTO_TermDelete = e.Item.DataItem

            Dim LTname As Literal = e.Item.FindControl("LTname")
            If Not IsNothing(LTname) Then
                LTname.Text = myobj.Name
            End If

            Dim LTdeleteOn As Literal = e.Item.FindControl("LTdeleteOn")
            If Not IsNothing(LTdeleteOn) Then
                LTdeleteOn.Text = myobj.DeletedOn.ToShortDateString()
            End If

            Dim LTdeleteFrom As Literal = e.Item.FindControl("LTdeleteFrom")
            If Not IsNothing(LTdeleteFrom) Then
                LTdeleteFrom.Text = myobj.DeletedBy
            End If

            Dim LNBtermRecover As LinkButton = e.Item.FindControl("LNBtermRecover")
            If Not IsNothing(LNBtermRecover) Then
                LNBtermRecover.CommandName = termRecoverCommand
                LNBtermRecover.CommandArgument = myobj.Id
                Resource.setLinkButton(LNBtermRecover, True, True, False, True)
            End If

            Dim LNBtermDelete As LinkButton = e.Item.FindControl("LNBtermDelete")
            If Not IsNothing(LNBtermDelete) Then
                LNBtermDelete.CommandName = termDeleteCommand
                LNBtermDelete.CommandArgument = myobj.Id
                Resource.setLinkButton(LNBtermDelete, True, True, False, True)
            End If

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
            oTableItem.Visible = (RPTlist.DataSource.Count = 0)
            If (RPTlist.DataSource.Count = 0) Then
                Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                oLabel.Text = Resource.getValue("NoTermFound")
            End If
        End If
    End Sub

    Private Sub RPTlist_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTlist.ItemCommand
        Dim currentTermId As Int64 = Convert.ToInt64(e.CommandArgument)
        If currentTermId > 0 Then
            Select Case e.CommandName
                Case termRecoverCommand
                    CurrentPresenter.RecoverTerm(currentTermId)
                Case termDeleteCommand
                    CurrentPresenter.DeleteTerm(currentTermId)
                Case Else
            End Select
        End If
    End Sub

#End Region
End Class