Imports System.Text
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_GlossaryShare
    Inherits GLbaseControl
    Implements IViewUC_GlossaryShare

    Public Event AddCommunityClicked()
    Public Event SelectedCommunities(ByVal idCommunities As List(Of Integer), identifier As String)

    Private _Presenter As UC_GlossarySharePresenter

    Private ReadOnly Property CurrentPresenter() As UC_GlossarySharePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_GlossarySharePresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTsystemWideSharing_t)
            .setLiteral(LTcommunitySharing_t)
            .setLiteral(LTpublicDescription)
            .setLiteral(LTcommunityDescription)

            .setButton(BTNaddCommunity)
            .setLinkButton(LNBsaveGlossaryShare, False, True)
            .setLinkButton(LNBback, False, True, False, True)
            SWHpublic.SetText(Resource, True, True)
            SWshared.SetText(Resource, True, True)
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Property ItemData As DTO_Glossary Implements IViewUC_GlossaryShare.ItemData
        Get
            Dim value As DTO_Glossary = New DTO_Glossary()
            value.Id = IdGlossary
            value.IdCommunity = IdCommunity
            value.IsPublic = SWHpublic.Status
            value.IsShared = SWshared.Status
            Return value
        End Get
        Set(value As DTO_Glossary)
            IdGlossary = value.Id
            SWHpublic.Status = value.IsPublic
            SWshared.Status = value.IsShared
        End Set
    End Property

    Public Sub SetTitle(ByVal name As String) Implements IViewUC_GlossaryShare.SetTitle
    End Sub

    Public Sub LoadViewData(ByVal idCommunity As Integer, ByVal glossary As DTO_Glossary, ByVal communityShareList As List(Of DTO_Share)) Implements IViewUC_GlossaryShare.LoadViewData
        ItemData = glossary
        RPTshare.DataSource = communityShareList
        RPTshare.DataBind()

        If communityShareList.Count = 0 Then
            LTselectedCommunity.Text = Resource.getValue("NoCommunitySelected")
        Else
            LTselectedCommunity.Text = Resource.getValue("SharingActiveOn")
        End If
    End Sub

    Public Sub BindDati(ByVal idCommunity As Int32, ByVal idGlossary As Int64)
        CurrentPresenter.InitView()
    End Sub

    Public Sub GoToGlossaryList() Implements IViewUC_GlossaryShare.GoToGlossaryList
    End Sub

    Public Sub GoToGlossaryView() Implements IViewUC_GlossaryShare.GoToGlossaryView
    End Sub

    Public Sub ShowErrors(ByVal resourceErrorList As List(Of String), Optional ByVal type As MessageType = MessageType.alert) Implements IViewUC_GlossaryShare.ShowErrors
        Dim errors As New StringBuilder()
        For Each key As String In resourceErrorList
            errors.AppendLine(Resource.getValue(key))
        Next
        SetErrorMessage(errors.ToString(), type)
    End Sub

    Public Sub ShowErrors(ByVal saveStateEnum As SaveStateEnum, Optional ByVal type As MessageType = MessageType.error) Implements IViewUC_GlossaryShare.ShowErrors
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

    Private Sub RTPshare_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTshare.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As DTO_Share = e.Item.DataItem

            Dim HDFid As HiddenField = e.Item.FindControl("HDFid")
            If Not IsNothing(HDFid) Then
                HDFid.Value = item.Id
            End If

            Dim LTcommunityName As Literal = e.Item.FindControl("LTcommunityName")
            If Not IsNothing(LTcommunityName) Then
                LTcommunityName.Text = item.FromCommunityName
            End If

            Dim LTactive As Literal = e.Item.FindControl("LTactive")
            If Not IsNothing(LTactive) Then
                LTactive.Text = Resource.getValue(String.Format("ShareStatusEnum.{0}", item.Status.ToString()))
            End If

            Dim LTactive2 As Literal = e.Item.FindControl("LTactive2")
            If Not IsNothing(LTactive2) Then
                LTactive2.Text = Resource.getValue(String.Format("ShareStatusEnum.{0}", item.Status.ToString()))
            End If

            Dim CBXforceActive As CheckBox = e.Item.FindControl("CBXforceActive")
            If Not IsNothing(CBXforceActive) Then
                CBXforceActive.Checked = item.Status = ShareStatusEnum.ForceActive
            End If

            Dim CBXaddTerm As CheckBox = e.Item.FindControl("CBXaddTerm")
            If Not IsNothing(CBXaddTerm) Then
                CBXaddTerm.Checked = item.HasPermission(SharePermissionEnum.AddTerm)
            End If

            Dim CBXdeleteTerm As CheckBox = e.Item.FindControl("CBXdeleteTerm")
            If Not IsNothing(CBXdeleteTerm) Then
                CBXdeleteTerm.Checked = item.HasPermission(SharePermissionEnum.DeleteTerm)
            End If

            Dim CBXeditTerm As CheckBox = e.Item.FindControl("CBXeditTerm")
            If Not IsNothing(CBXeditTerm) Then
                CBXeditTerm.Checked = item.HasPermission(SharePermissionEnum.EditTerm)
            End If

            Dim LTstatus_t As Literal = e.Item.FindControl("LTstatus_t")
            If Not IsNothing(LTstatus_t) Then
                LTstatus_t.Text = Resource.getValue("Status")
            End If

            Dim LT_permissions_t As Literal = e.Item.FindControl("LT_permissions_t")
            If Not IsNothing(LT_permissions_t) Then
                LT_permissions_t.Text = Resource.getValue("Permissions")
            End If


            Dim LBforceGlossary_t As Label = e.Item.FindControl("LBforceGlossary_t")
            If Not IsNothing(LBforceGlossary_t) Then
                Resource.setLabel(LBforceGlossary_t)
            End If

            Dim LTgrantPermissions_t As Literal = e.Item.FindControl("LTgrantPermissions_t")
            If Not IsNothing(LTgrantPermissions_t) Then
                Resource.setLiteral(LTgrantPermissions_t)
            End If

            Dim LBaddTerm_t As Label = e.Item.FindControl("LBaddTerm_t")
            If Not IsNothing(LBaddTerm_t) Then
                Resource.setLabel(LBaddTerm_t)
            End If

            Dim LBdeleteTerm_t As Label = e.Item.FindControl("LBdeleteTerm_t")
            If Not IsNothing(LBdeleteTerm_t) Then
                Resource.setLabel(LBdeleteTerm_t)
            End If

            Dim LBeditTerm_t As Label = e.Item.FindControl("LBeditTerm_t")
            If Not IsNothing(LBeditTerm_t) Then
                Resource.setLabel(LBeditTerm_t)
            End If

            Dim LNBvirtualDeleteShare As LinkButton = e.Item.FindControl("LNBvirtualDeleteShare")
            If Not IsNothing(LNBvirtualDeleteShare) Then
                LNBvirtualDeleteShare.Visible = True
                Resource.setLinkButton(LNBvirtualDeleteShare, True, True, False, True)
                LNBvirtualDeleteShare.CommandArgument = item.Id
            End If

        End If
    End Sub

    Public Sub RPTshare_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim idShare As Int64 = Convert.ToInt64(e.CommandArgument)
        If idShare > 0 Then
            CurrentPresenter.VirtualDeleteShare(idShare)
        End If
    End Sub

    Protected Function GetStateClass(ByVal shareStatusEnum As ShareStatusEnum) As String
        If shareStatusEnum = shareStatusEnum.Active Or shareStatusEnum = shareStatusEnum.ForceActive Or shareStatusEnum = shareStatusEnum.Pending Or shareStatusEnum = shareStatusEnum.Refused Then
            Return String.Format("option {0}", shareStatusEnum.ToString()).ToLower()
        Else
            Return "option"
        End If
    End Function

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBsaveGlossaryShare.Click


        Dim shareList As New List(Of DTO_Share)

        For Each item As RepeaterItem In RPTshare.Items

            Dim dataItem As New DTO_Share

            Dim HDFid As HiddenField = item.FindControl("HDFid")
            If Not IsNothing(HDFid) Then
                dataItem.Id = HDFid.Value
            End If

            Dim CBXforceActive As CheckBox = item.FindControl("CBXforceActive")
            If Not IsNothing(CBXforceActive) AndAlso CBXforceActive.Checked Then
                dataItem.Status = ShareStatusEnum.ForceActive
            End If

            dataItem.Permissions = SharePermissionEnum.None

            Dim CBXaddTerm As CheckBox = item.FindControl("CBXaddTerm")
            If Not IsNothing(CBXaddTerm) AndAlso CBXaddTerm.Checked Then
                dataItem.Permissions += SharePermissionEnum.AddTerm
            End If

            Dim CBXdeleteTerm As CheckBox = item.FindControl("CBXdeleteTerm")
            If Not IsNothing(CBXdeleteTerm) AndAlso CBXdeleteTerm.Checked Then
                dataItem.Permissions += SharePermissionEnum.DeleteTerm
            End If

            Dim CBXeditTerm As CheckBox = item.FindControl("CBXeditTerm")
            If Not IsNothing(CBXeditTerm) AndAlso CBXeditTerm.Checked Then
                dataItem.Permissions += SharePermissionEnum.EditTerm
            End If
            shareList.Add(dataItem)
        Next

        CurrentPresenter.SaveOrUpdate(ItemData, shareList)
    End Sub


#Region "Community Selector"

    Public Sub BTNaddCommunity_Click(sender As Object, e As EventArgs) Handles BTNaddCommunity.Click
        RaiseEvent AddCommunityClicked()
    End Sub

    'Public Sub ShowCommunityDialog(ByVal master As AjaxPortal)
    '    CurrentPresenter.AddCommunityClick(PageUtility.CurrentUser.ID, master)
    'End Sub

    'Public Sub DisplayCommunityToAdd(idProfile As Integer, forAdministration As Boolean, requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, ByVal Master As Object) Implements IViewUC_GlossaryShare.DisplayCommunityToAdd
    '    'LBselected.Text = Resource.getValue("NoSelection")
    '    CTRLmessages.Visible = False
    '    CTRLcommunity.Visible = True
    '    Dim masterCasted As AjaxPortal = Master
    '    masterCasted.SetOpenDialogOnPostbackByCssClass(CTRLcommunity.ModalIdentifier)
    '    If forAdministration Then
    '        CTRLcommunity.InitializeAdministrationControl(idProfile, unloadIdCommunities, availability, New List(Of Integer))
    '    Else
    '        CTRLcommunity.InitializeControlByModules(idProfile, requiredPermissions, unloadIdCommunities, availability)
    '    End If
    'End Sub

    'Private Sub CTRLcommunity_LoadDefaultFiltersToHeader(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer)) Handles CTRLcommunity.LoadDefaultFiltersToHeader
    '    CTRLcommunitySelectorHeader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    'End Sub

    'Public Sub CTRLcommunity_SelectedCommunities(idCommunities As List(Of Integer), identifier As String) Handles CTRLcommunity.SelectedCommunities
    '    RaiseEvent SelectedCommunities(idCommunities, identifier)
    'End Sub

    'Public Sub CloseCommunityDialog(idCommunities As List(Of Integer), ByVal master As AjaxPortal)
    '    master.SetOpenDialogOnPostbackByCssClass("")
    '    CurrentPresenter.AddNewCommunity(idCommunities)
    '    'LBselected.Text = String.Format("{0} {1}", Resource.getValue("Selected"), String.Join(",", idCommunities))
    'End Sub

#End Region

    Private Sub LNBback_Click(sender As Object, e As EventArgs) Handles LNBback.Click
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryList(IdCommunity))
    End Sub
End Class