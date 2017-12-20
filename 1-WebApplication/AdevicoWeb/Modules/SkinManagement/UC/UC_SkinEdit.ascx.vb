Imports lm.Comol.Modules.Standard.Skin

Public Class UC_SkinEdit
    Inherits BaseControlSession
    Implements Presentation.iViewSkinEdit

#Region "Internal Property"
    Private Property CurrentSkinId As Int64
        Get
            Dim Id As Int64
            Try
                Id = System.Convert.ToInt64(Me.ViewState("CurrentSkinId"))
            Catch ex As Exception
                Id = 0
            End Try
            Return Id

        End Get
        Set(ByVal value As Int64)
            Me.ViewState("CurrentSkinId") = value
        End Set
    End Property

    Private _presenter As Presentation.SkinEditPresenter
    Private ReadOnly Property presenter As Presentation.SkinEditPresenter
        Get
            If IsNothing(_presenter) Then
                Me._presenter = New Presentation.SkinEditPresenter(MyBase.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property

    Public Property SkinData As Domain.DTO.DtoSkin Implements Presentation.iViewSkinEdit.SkinData
        Get
            Dim SkData As New Domain.DTO.DtoSkin
            SkData.Id = CurrentSkinId
            SkData.Name = Me.TXBskinName.Text
            SkData.OverrideFootLogos = Me.CTRLfootLogo.OverrideVoid
            Return SkData
        End Get
        Set(ByVal value As Domain.DTO.DtoSkin)
            CurrentSkinId = value.Id
            LTskinId.Text = value.Id.ToString()
            Me.TXBskinName.Text = value.Name
            Me.CTRLfootLogo.OverrideVoid = value.OverrideFootLogos
        End Set
    End Property

#End Region

#Region "Page and PageBase"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            Me.Show(ViewIndex.CSS)
            Me.SetInternazionalizzazione()
        End If
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_SkinEdit", "Skin", "UC")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If
        With MyBase.Resource
            .setLabel(LBskinName_t)
            .setLinkButton(LNBsaveSkinName, True, True)
            '.setCheckBox(Cbx_IsActive)
            .setLabel(LBskinId_t)

            For Each Tab As Telerik.Web.UI.RadTab In Me.TBSSkinEdit.Tabs
                .setRadTab(Tab, True)
            Next
        End With
    End Sub

#End Region
    

#Region "Navigazione"
    Private Sub TBSSkinEdit_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSSkinEdit.TabClick
        Me.Show(Me.TBSSkinEdit.SelectedIndex)
    End Sub

    Private Sub Show(ByVal View As ViewIndex)
        Select Case View
            Case ViewIndex.CSS
                Me.MLVskinEdit.SetActiveView(Me.VIWcss)

            Case ViewIndex.Images
                Me.MLVskinEdit.SetActiveView(Me.VIWimages)

            Case ViewIndex.MainLogo
                Me.MLVskinEdit.SetActiveView(Me.VIWmainLogo)

            Case ViewIndex.FootLogo
                Me.MLVskinEdit.SetActiveView(Me.VIWfootLogo)

            Case ViewIndex.FootText
                Me.MLVskinEdit.SetActiveView(Me.VIWfootText)

            Case ViewIndex.Association
                Me.MLVskinEdit.SetActiveView(Me.VIWassociation)

        End Select

        Me.TBSSkinEdit.SelectedIndex = View

    End Sub

    Private Enum ViewIndex As Integer
        CSS = 0
        Images = 1
        MainLogo = 2
        FootLogo = 3
        FootText = 4
        Association = 5
    End Enum
#End Region

    


    '!! ATTENZIONE !! - Spostare il bind dei controlli a quando vengono visualizzati!!!
    Public Sub BindData(ByVal SkinId As Int64)
        Me.presenter.Bind(SkinId)

        Me.CTRLcss.InitializeControl(SkinId, True)
        Me.CTRLimages.InitializeControl(SkinId, True)
        Me.CTRLmainLogo.InitializeControl(SkinId, True)
        Me.CTRLfootLogo.InitializeControl(SkinId, True)
        Me.CTRLfootText.InitializeControl(SkinId, True)
        Me.CTRLassociation.BindAssociation(SkinId)
    End Sub

    Private Sub LNBsaveSkinName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsaveSkinName.Click
        Me.presenter.Save()
    End Sub


End Class