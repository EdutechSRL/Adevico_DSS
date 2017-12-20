Imports lm.Comol.Modules.Standard.Skin

Public Class UC_SkinAssociation
    Inherits BaseControl
    Implements Presentation.iViewSkinShare

#Region "Internal Property"
    Private _presenter As Presentation.SkinSharePresenter

    Private ReadOnly Property presenter As Presentation.SkinSharePresenter
        Get
            If IsNothing(_presenter) Then
                Me._presenter = New Presentation.SkinSharePresenter(MyBase.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property

    Public ReadOnly Property SkinId As Long Implements Presentation.iViewSkinShare.SkinId
        Get
            Return InternalSkinId
        End Get
    End Property

    Private Property InternalSkinId As Int64
        Get
            Dim Id As Int64 = 0
            Try
                Id = System.Convert.ToInt64(Me.ViewState("SkinId"))
            Catch ex As Exception

            End Try
            Return Id
        End Get
        Set(ByVal value As Int64)
            Me.ViewState("SkinId") = value
        End Set
    End Property

    Private Function getOrgnIds() As IList(Of Int32)
        Dim OrgnIds As New List(Of Int32)

        For i As Integer = 0 To Me.Rpt_OrgnMod.Items.Count - 1
            Dim itm As RepeaterItem = Me.Rpt_OrgnMod.Items(i)
            If itm.ItemType = ListItemType.Item OrElse itm.ItemType = ListItemType.AlternatingItem Then
                Dim Cbx_Orgn As CheckBox = itm.FindControl("Cbx_Orgn")
                If Cbx_Orgn.Checked Then
                    Dim Hid_OrgnId As HiddenField = itm.FindControl("Hid_OrgnId")
                    OrgnIds.Add(Hid_OrgnId.Value)
                End If
            End If
        Next

        Return OrgnIds
    End Function
#End Region

#Region "Page and PageBase"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_SkinAssociation", "Skin", "UC")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLinkButton(Lkb_RemPortal, True, True, False, True)
            .setLinkButton(Lkb_AddPortal, True, True, False, True)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

    Private Enum Views
        Main
        Organizations
        Communities
    End Enum

    Private Sub Show(ByVal CurView As Views)
        Select Case CurView
            Case Views.Main
                Me.MLVassociation.SetActiveView(Me.V_List)
            Case Views.Organizations
                Me.MLVassociation.SetActiveView(Me.V_SetOrgn)
            Case Views.Communities
                Me.MLVassociation.SetActiveView(Me.V_SetCom)
        End Select
    End Sub

    Public Sub BindAssociation(ByVal SkinId As Int64)
        Me.InternalSkinId = SkinId
        Me.presenter.BindMainList()
    End Sub

#Region "Bind dati"
    Public Sub BindCommunities(ByVal CommunitiesId As System.Collections.Generic.IList(Of Integer)) Implements Presentation.iViewSkinShare.BindCommunities
        Me.Show(Views.Communities)

        Me.UC_SearchCommunity.SelectionMode = ListSelectionMode.Multiple
        Me.UC_SearchCommunity.isModalitaAmministrazione = True
        Me.UC_SearchCommunity.AllowMultipleOrganizationSelection = False
        Me.UC_SearchCommunity.InitializeControl(0, CommunitiesId)
        'Me.UC_SearchCommunity.InitializeControl(-1)


        'Me.UC_SearchCommunity.GridCurrentPage = 1

    End Sub
    Public Sub BindMainList(ByVal Shares As Domain.DTO.DtoSkinShares) Implements Presentation.iViewSkinShare.BindMainList
        Me.Show(Views.Main)

        If (Shares.IsPortal = True) Then
            Me.Lkb_RemPortal.Visible = True
            Me.Lkb_AddPortal.Visible = False
            Resource.setLabel_To_Value(Me.Lbl_AssPortale, "Portal_t.Associata")
        Else
            Me.Lkb_RemPortal.Visible = False
            Me.Lkb_AddPortal.Visible = True
            Resource.setLabel_To_Value(Me.Lbl_AssPortale, "Portal_t.NonAssociata")
        End If

        Me.Rpt_OrgnList.DataSource = Shares.Organizations
        Me.Rpt_ComList.DataSource = Shares.Communities
        Me.Rpt_OrgnList.DataBind()
        Me.Rpt_ComList.DataBind()

    End Sub
    Public Sub BindOrganizations(ByVal Organization As System.Collections.Generic.IList(Of Domain.DTO.DtoSkinOrganization)) Implements Presentation.iViewSkinShare.BindOrganizations
        Me.Show(Views.Organizations)

        Me.Rpt_OrgnMod.DataSource = Organization
        Me.Rpt_OrgnMod.DataBind()
    End Sub
#End Region
#Region "Repeater DataBound & Command"
    Private Sub Rpt_OrgnList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_OrgnList.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim Item As Domain.DTO.DtoSkinShareItem = e.Item.DataItem

            Dim Lkb_DelOrgnAss As LinkButton = e.Item.FindControl("Lkb_DelOrgnAss")
            Lkb_DelOrgnAss.CommandName = "DelOrgn"
            Lkb_DelOrgnAss.CommandArgument = Item.Id

            Dim Lbl_OrgnName As Label = e.Item.FindControl("Lbl_OrgnName")
            Lbl_OrgnName.Text = Item.Name
        End If
    End Sub

    Private Sub Rpt_OrgnList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Rpt_OrgnList.ItemCommand
        Select Case e.CommandName
            Case "DelOrgn"
                Dim ID As Int32 = e.CommandArgument
                Me.presenter.RemOrganizationShare(ID)
                Me.presenter.BindMainList()
        End Select

    End Sub

    Private Sub Rpt_ComList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_ComList.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim Item As Domain.DTO.DtoSkinShareItem = e.Item.DataItem

            Dim Lkb_DelComAss As LinkButton = e.Item.FindControl("Lkb_DelComAss")
            Lkb_DelComAss.CommandName = "DelCom"
            Lkb_DelComAss.CommandArgument = Item.Id

            Dim Lbl_ComName As Label = e.Item.FindControl("Lbl_ComName")
            Lbl_ComName.Text = Item.Name
        End If
    End Sub

    Private Sub Rpt_ComList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Rpt_ComList.ItemCommand
        Select Case e.CommandName
            Case "DelCom"
                Dim ID As Int32 = e.CommandArgument
                Me.presenter.RemCommunityShare(ID)
                Me.presenter.BindMainList()
        End Select
    End Sub


    Private Sub Rpt_OrgnMod_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Rpt_OrgnMod.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DtoOrgn As Domain.DTO.DtoSkinOrganization = e.Item.DataItem

            Dim Cbx_Orgn As CheckBox = e.Item.FindControl("Cbx_Orgn")
            Cbx_Orgn.Text = DtoOrgn.Name
            Cbx_Orgn.Checked = DtoOrgn.IsChecked

            Dim Hid_OrgnId As HiddenField = e.Item.FindControl("Hid_OrgnId")
            Hid_OrgnId.Value = DtoOrgn.Id

            Dim Hyp_Orgn As HyperLink = e.Item.FindControl("Hyp_Orgn")
            Hyp_Orgn.NavigateUrl = DtoOrgn.Url
            Hyp_Orgn.Text = DtoOrgn.Url
        End If

    End Sub

#End Region

#Region "Pulsanti salva e navigazione"

    Private Sub Lkb_ModOrgn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_ModOrgn.Click
        Me.presenter.BindOrganizationList()
    End Sub
    Private Sub Lkb_ModCom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_ModCom.Click
        Me.presenter.BindCommunity()
    End Sub
    Private Sub Lkb_BackCom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_BackCom.Click
        Me.presenter.BindMainList()
    End Sub
    Private Sub Lkb_BackOrgn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_BackOrgn.Click
        Me.presenter.BindMainList()
    End Sub

    Private Sub Lkb_AddPortal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_AddPortal.Click
        Me.presenter.SetPortal()

        Me.presenter.BindMainList()

    End Sub
    Private Sub Lkb_RemPortal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_RemPortal.Click
        Me.presenter.RemPortal()
        Me.presenter.BindMainList()
    End Sub
    Private Sub Lkb_SaveOrgn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_SaveOrgn.Click

        presenter.SetOrganization(getOrgnIds())
        Me.presenter.BindMainList()
    End Sub

    Private Sub Lkb_SaveCom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_SaveCom.Click
        Me.presenter.AddCommunities(Me.UC_SearchCommunity.SelectedCommunitiesID)
        Me.presenter.BindMainList()
    End Sub
#End Region

End Class