Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_ImportCommunityGlossary
    Inherits GLbaseControl
    Implements IViewUC_ImportCommunityGlossary

    Private _Presenter As UC_ImportCommunityGlossaryPresenter

    Private ReadOnly Property CurrentPresenter() As UC_ImportCommunityGlossaryPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_ImportCommunityGlossaryPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource

        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Sub BindDati(ByVal idCommunity As Int32)
        Me.IdCommunity = idCommunity
        CurrentPresenter.InitView()
    End Sub

    Public Sub LoadViewData(ByVal communityList As List(Of DTO_ImportCommunity)) Implements IViewUC_ImportCommunityGlossary.LoadViewData
        RPTCommunites.DataSource = communityList
        RPTCommunites.DataBind()
    End Sub

    Public Sub LoadViewData(ByVal communityList As List(Of Int32)) Implements IViewUC_ImportCommunityGlossary.InitViewData
        CurrentPresenter.UpdateView(communityList)
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewUC_ImportCommunityGlossary.SetTitle
    End Sub

    Public Sub RPTCommunites_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTCommunites.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As DTO_ImportCommunity = e.Item.DataItem
            Dim LBcommunityName As Label = e.Item.FindControl("LBcommunityName")
            If Not IsNothing(LBcommunityName) Then
                LBcommunityName.Text = item.Name
            End If

            Dim RPTglossary As Repeater = e.Item.FindControl("RPTglossary")
            If Not IsNothing(RPTglossary) Then
                RPTglossary.DataSource = item.GlossaryList
                RPTglossary.DataBind()
            End If

            Dim LBselectAll As Label = e.Item.FindControl("LBselectAll")
            If Not IsNothing(LBselectAll) Then
                Resource.setLabel(LBselectAll)
            End If

            Dim LBselectNone As Label = e.Item.FindControl("LBselectNone")
            If Not IsNothing(LBselectNone) Then
                Resource.setLabel(LBselectNone)
            End If

            Dim SWHcommunity As UC_ClientSideSwitch = e.Item.FindControl("SWHcommunity")
            If Not IsNothing(SWHcommunity) Then
                ' SWHcommunity.SetText(Resource, False, False)
                SWHcommunity.TextOn = Resource.getValue("Switch.TextOn")
                SWHcommunity.ToolTipOn = Resource.getValue("Switch.ToolTipOn")
                SWHcommunity.TextOff = Resource.getValue("Switch.TextOff")
                SWHcommunity.ToolTipOff = Resource.getValue("Switch.ToolTipOff")
                SWHcommunity.IsOn = True
            End If
            index += 1
        End If
    End Sub


#Region "Community Selector"

    'Public Event AddCommunityClicked()
    'Public Event SelectedCommunities(ByVal idCommunities As List(Of Integer), identifier As String)

    'Public Sub BTNaddCommunity_Click(sender As Object, e As EventArgs) Handles LNBselectCommunities.Click
    '    RaiseEvent AddCommunityClicked()
    'End Sub

#End Region

    Private index As Integer = 0

    Public Sub RPTglossary_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim itemData As DTO_ImportGlossary = e.Item.DataItem

            Dim LBglossary As Label = e.Item.FindControl("LBglossary")
            If Not IsNothing(LBglossary) Then
                LBglossary.Text = itemData.Name
            End If

            Dim CBXglossary As CheckBox = e.Item.FindControl("CBXglossary")
            If Not IsNothing(CBXglossary) Then
                CBXglossary.Attributes.Add("idGlossary", itemData.Id)
                CBXglossary.Checked = True
            End If

        End If
    End Sub

    'Public Sub LNBselectAll_Click(sender As Object, e As EventArgs)
    '    'Dim button As LinkButton = sender
    '    'Dim index As Integer = button.CommandArgument
    '    'changeCheckState(index, True)
    'End Sub

    'Public Sub LNBselectNone_Click(sender As Object, e As EventArgs)
    '    'Dim button As LinkButton = sender
    '    'Dim index As Integer = button.CommandArgument
    '    'changeCheckState(index, False)
    'End Sub

    'Private Sub changeCheckState(ByVal index As Integer, ByVal checked As Boolean)
    '    Dim RPTglossary As Repeater = RPTCommunites.Items(index).FindControl("RPTglossary")
    '    If Not IsNothing(RPTglossary) Then

    '        For Each item As RepeaterItem In RPTglossary.Items
    '            Dim CBXglossary As CheckBox = item.FindControl("CBXglossary")
    '            If Not IsNothing(CBXglossary) Then
    '                CBXglossary.Checked = checked
    '            End If
    '        Next
    '    End If
    'End Sub

    Public Function GetSelectedTermIds() As IEnumerable(Of Long)
        Dim result As New List(Of Int64)
        For Each itemGlossary As RepeaterItem In RPTCommunites.Items
            'Dim SWHcommunity As UC_Switch = itemGlossary.FindControl("SWHcommunity")
            Dim SWHcommunity As UC_ClientSideSwitch = itemGlossary.FindControl("SWHcommunity")
            If Not IsNothing(SWHcommunity) Then
                'If SWHcommunity.Status Then
                If SWHcommunity.IsOn Then
                    Dim RPTglossary As Repeater = RPTCommunites.Items(index).FindControl("RPTglossary")
                    If Not IsNothing(RPTglossary) Then
                        For Each item As RepeaterItem In RPTglossary.Items
                            Dim CBXglossary As CheckBox = item.FindControl("CBXglossary")
                            If Not IsNothing(CBXglossary) Then
                                If CBXglossary.Checked Then
                                    Dim idGlossary As Integer = CBXglossary.Attributes.Item("idGlossary")
                                    result.Add(idGlossary)
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Next
        result = result.Distinct().ToList()
        Return result
    End Function
End Class