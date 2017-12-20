Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_CommunityGlossaryTerms
    Inherits GLbaseControl
    Implements IViewUC_CommunityGlossaryTerms

    Private _Presenter As UC_CommunityGlossaryTermsPresenter

    Private ReadOnly Property CurrentPresenter() As UC_CommunityGlossaryTermsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_CommunityGlossaryTermsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property ShowCommunityMapper As Boolean
        Get
            Return ViewStateOrDefault("ShowCommunityMapper", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowCommunityMapper") = value
        End Set
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

    Public Sub LoadViewData(ByVal communityList As List(Of DTO_ImportCommunity), ByVal showCommunityMapper As Boolean) Implements IViewUC_CommunityGlossaryTerms.LoadViewData
        RPTCommunites.DataSource = communityList
        RPTCommunites.DataBind()
    End Sub

    Public Sub LoadViewData(ByVal glossaryContainer As GlossaryContainer) Implements IViewUC_CommunityGlossaryTerms.LoadViewData
    End Sub

    Public Sub LoadViewData(ByVal communityList As List(Of Int32)) Implements IViewUC_CommunityGlossaryTerms.InitViewData
        CurrentPresenter.UpdateView(communityList)
    End Sub

    Public Sub LoadViewDataFromFile(ByVal filenamePath As String, ByVal showCommunityMapper As Boolean)
        Me.ShowCommunityMapper = showCommunityMapper
        CurrentPresenter.UpdateView(filenamePath, showCommunityMapper)
    End Sub


    Public Sub SetTitle(ByVal name As String) Implements IViewUC_CommunityGlossaryTerms.SetTitle
    End Sub

    Public Sub RPTCommunites_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTCommunites.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim UCglossaryTerms As UC_GlossaryImportTerms = e.Item.FindControl("UCglossaryTerms")
            If Not IsNothing(UCglossaryTerms) Then
                Dim itemData As DTO_ImportCommunity = e.Item.DataItem
                UCglossaryTerms.LoadViewData(itemData, ShowCommunityMapper)
            End If
        End If
    End Sub

    Public Function GetRPTCommunites() As Repeater
        Return RPTCommunites
    End Function

#Region "Community Selector"

    'Public Event AddCommunityClicked()
    'Public Event SelectedCommunities(ByVal idCommunities As List(Of Integer), identifier As String)

    'Public Sub BTNaddCommunity_Click(sender As Object, e As EventArgs) Handles LNBselectCommunities.Click
    '    RaiseEvent AddCommunityClicked()
    'End Sub

#End Region
End Class